using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.IO;
using System.Web;
using System.Runtime.Remoting.Messaging;
using System.ComponentModel;

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

        private void authenticateButton_Click(object sender, EventArgs e)
        {
            if (serverRunning)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem((x) =>
            {
                serverRunning = true;
                string token = GetCodeFromLocalHost(serverCanceledState);
                serverRunning = false;
                if (token != null)
                {
                    this.BeginInvoke(new Action(() => afterReceiveToken(token)));
                }
            });
        }

        private void afterReceiveToken(string token)
        {
            m_strToken = token;
            m_bRemember = true;
            DialogResult = DialogResult.OK;

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
            this.Close();
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

        private static string GetCodeFromLocalHost(RunState state)
        {
            global::System.Resources.ResourceManager resourceManager = new global::System.Resources.ResourceManager("LetsMT.MTProvider.PluginResources", typeof(PluginResources).Assembly);
            string httpTemporaryListenAddresses = string.Format("{0}/Temporary_Listen_Addresses/", resourceManager.GetString("HttpTemporaryListenAddresses"));
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
                
                listener.Start();

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
                            foreach (Cookie cook in context.Request.Cookies)
                            {
                                if (cook.Name == "smts")
                                {
                                    code = cook.ToString();
                                }
                            }
                            if (code == null)
                            {
                                //throw new AuthenticationException("Access denied, no return code was returned");
                            }

                            var writer = new StreamWriter(context.Response.OutputStream);
                            writer.WriteLine(CloseWindowResponse);
                            writer.Flush();

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

        private const string CloseWindowResponse = "<!DOCTYPE html><html><head></head><body onload=\"closeThis();\"><h1>Authorization Successfull</h1><p>You can now close this window</p><script type=\"text/javascript\">function closeMe() { window.close(); } function closeThis() { window.close(); }</script></body></html>";
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