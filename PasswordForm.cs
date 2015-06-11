using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.IO;
using System.Web;
using System.Runtime.Remoting.Messaging;
using System.ComponentModel;
using System.Net.Sockets;

namespace LetsMT.MTProvider
{
    public partial class PasswordForm : Form
    {
        private string m_strUsername;
        private string m_strPassword;
        private string m_strAppId;
        private string m_strToken;
        private bool m_bRemember;
        private RunState serverCanceledState;
        private bool serverRunning = false;
        private string httpTemporaryListenAddresses = "";

        #region "Getters & Setters"
        public string strUsername
        {
            get { return m_strUsername; }
            set { m_strUsername = value; }
        }

        public string strPassword
        {
            get { return m_strPassword; }
            set { m_strPassword = value; }
        }

        public string strAppId
        {
            get { return m_strAppId; }
            set { m_strAppId = value; }
        }

        public string strToken
        {
            get { return m_strToken; }
            set { m_strToken = value; }
        }

        public bool bRemember
        {
            get { return m_bRemember; }
            set { m_bRemember = value; }
        }
        #endregion

        public PasswordForm()
        {
            InitializeComponent();

            DialogResult = DialogResult.Cancel;
            serverCanceledState = new RunState { Canceled = false };
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            serverCanceledState.Canceled = true;
            Close();
        }


        private void DemoLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process myProcess = new Process();

            try
            {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = "https://www.letsmt.eu/Register.aspx?type=demo";
                myProcess.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot open link!", "Connection problem");
            }
        }

        private void afterReceiveToken(string token)
        {
            this.apiUserIdTextBox.Text = token;

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
        }

        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        private static string GetAuthorizationUrl(string redirectUrl)
        {
            global::System.Resources.ResourceManager resourceManager = new global::System.Resources.ResourceManager("LetsMT.MTProvider.PluginResources", typeof(PluginResources).Assembly);
            string letsMTLoginUrl = resourceManager.GetString("LetsMTLoginUrl");

            //try to read Software\\Tilde\\LetsMT\\loginUrl registry string entry and it it exists replace the link
            try
            {
                Microsoft.Win32.RegistryKey key;
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\\Tilde\\LetsMT");
                if (key != null)
                {
                    string RegUrl = key.GetValue("loginUrl", "none").ToString();
                    if (RegUrl.Length > 3)
                    {
                        if (RegUrl.Substring(0, 4) == "http") { letsMTLoginUrl = RegUrl; }
                    }
                }

            }
            catch (Exception) { }

            return string.Format("{0}?returnUrl={1}", letsMTLoginUrl, HttpUtility.UrlEncode(redirectUrl));
        }

        private static string GetRedirectUrl()
        {
            global::System.Resources.ResourceManager resourceManager = new global::System.Resources.ResourceManager("LetsMT.MTProvider.PluginResources", typeof(PluginResources).Assembly);
            return string.Format("{0}/Temporary_Listen_Addresses/", resourceManager.GetString("HttpTemporaryListenAddresses"));
        }

        private static string GetRandomRedirectUrl()
        {
            return string.Format("http://{0}:{1}/Temporary_Listen_Addresses/", "localhost", GetRandomUnusedPort());
        }

        private string GetCodeFromLocalHost(RunState state, bool useRandomPort = false)
        {
            httpTemporaryListenAddresses = useRandomPort? GetRandomRedirectUrl() : GetRedirectUrl();
            string redirectUrl = httpTemporaryListenAddresses;

            string code = null;
            using (var listener = new HttpListener())
            {
                string localHostUrl = string.Format(httpTemporaryListenAddresses);

                listener.Prefixes.Add(localHostUrl);
                if (state != null)
                {
                    state.PropertyChanged += (sender, args) =>
                    {
                        if (state != null && state.Canceled && listener != null && listener.IsListening)
                        {
                            listener.Close();
                        }
                    };
                }

                // try to start listening on the address/port speciffied in the resources. if that fails retry on a random localhost port
                try
                {
                    listener.Start();
                }
                catch (Exception)
                {
                    if (!useRandomPort)
                    {
                        return GetCodeFromLocalHost(state, true);
                    }
                }

                using (Process.Start(GetAuthorizationUrl(redirectUrl)))
                {
                    while (!state.Canceled)
                    {
                        var start = DateTime.Now;
                        HttpListenerContext context;
                        try
                        {
                            context = listener.GetContext();
                        }
                        catch
                        {
                            break;
                        }
                        var usedTime = DateTime.Now.Subtract(start);
                        //timeout = timeout.Subtract(usedTime);

                        if (context.Request.Url.AbsolutePath == "/Temporary_Listen_Addresses/")
                        {
                            //foreach (Cookie cook in context.Request.Cookies)
                            //{
                            //    if (cook.Name == "apiUserId")
                            //    {
                            //        code = cook.Value;
                            //    }
                            //}
                            code = context.Request.QueryString["apiUserId"];
                            if (string.IsNullOrEmpty(code))
                            {
                                //throw new AuthenticationException("Access denied, no return code was returned");
                            }

                            var writer = new StreamWriter(context.Response.OutputStream);
                            writer.WriteLine(CloseWindowResponse);
                            writer.Flush();

                            Thread.Sleep(300); // for some reason if we don't wait IE fails to receive the HTML
                            context.Response.Close();
                            break;
                        }

                        context.Response.StatusCode = 404;
                        context.Response.Close();
                    }
                }
            }
            return code;
        }

        //private const string CloseWindowResponse = "<!DOCTYPE html><html><head></head><body onload=\"closeThis();\"><h1>Authorization Successfull</h1><p>You can now close this window</p><script type=\"text/javascript\">function closeMe() { window.close(); } function closeThis() { window.close(); }</script></body></html>";
        private const string CloseWindowResponse =
            @"<!DOCTYPE html>
            <html>
                <head>
                    <style>
                        #head_logo {
                            height: 37px;
                            width: 133px;
                            background-image: url('https://www.letsmt.eu/images/mt.svg');
                            background-size: contain;
                            background-repeat: no-repeat;
                            margin: 0em 0 -0.3em 0.5em;
                            display: inline-block;
                            border-bottom: 3px solid #bf1a37;
                        }
                        #logo_powered {
                            height: 19px;
                            width: 57px;
                            background-image: url('https://readymtdevlogic.tilde.lv/Content/images/letsmt/LetsMT_logo.png');
                            background-size: contain;
                            background-repeat: no-repeat;
                            margin: 0em 0 -0.3em 0.5em;
                            display: inline-block;
                        }
                        body {
                            width: 100%;
                        }
                        #border {
                            margin: 0 auto;
                            max-width: 70em;
                        }
                        #content {
                            margin: 0 0 3em 5em;
                        }
                    </style>
                </head>
                <body>
                    <div id='border'>
                        <div id='head_logo'></div>
                        <div id='content'>
                            <h1>Authorization Successfull</h1>
                            <p>You can now close this window</p>
                        </div>
                        <div id='logo_powered_container'>
                            <span>Powered by</span>
                            <div id='logo_powered'></div>
                        </div>
                    </div>
                </body>
            </html>";

        private void goButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(apiUserIdTextBox.Text.Trim()))
            {
                MessageBox.Show(@"Please provide a valid Client ID. To retrieve your Client ID click ""Get my Client ID"" below.");
                return;
            }
            // stop listening if user hasnt finished authentication procedure and has entered the userId manually
            serverCanceledState.Canceled = true;
            m_strToken = apiUserIdTextBox.Text.Trim();
            m_bRemember = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void getClientIdLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (serverRunning)
            {
                Process myProcess = new Process();
                try
                {
                    // true is the default, but it is important not to set it to false
                    myProcess.StartInfo.UseShellExecute = true;
                    myProcess.StartInfo.FileName = GetAuthorizationUrl(httpTemporaryListenAddresses);
                    myProcess.Start();
                }
                catch (Exception)
                {
                    MessageBox.Show("Cannot open link!", "Connection problem");
                }
                return;
            }
            else
            {
                ThreadPool.QueueUserWorkItem((x) =>
                {
                    serverCanceledState.Canceled = false;
                    serverRunning = true;
                    string token = GetCodeFromLocalHost(serverCanceledState);
                    serverRunning = false;
                    if (token != null)
                    {
                        this.BeginInvoke(new Action(() => afterReceiveToken(token)));
                    }
                });
            }
        }
    }

    #region helper class
    public class RunState : INotifyPropertyChanged
    {
        private bool _canceled;

        public bool Canceled
        {
            get { return _canceled; }
            set
            {
                _canceled = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Cancel"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    #endregion
}