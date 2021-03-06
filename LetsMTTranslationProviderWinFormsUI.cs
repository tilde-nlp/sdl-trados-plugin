using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

using System.Threading;
using System.Text.RegularExpressions;

using System.Web.Services.Protocols;

namespace LetsMT.MTProvider
{
    [TranslationProviderWinFormsUi(Id = "LetsMTTranslationProviderWinFormsUI")]

    public class LetsMTTranslationProviderWinFormsUI : ITranslationProviderWinFormsUI
    {
        #region ITranslationProviderWinFormsUI Members

        /// <summary>
        /// Determines whether the plug-in settings can be changed by displaying the Settings button in SDL Trados Studio.
        /// </summary>
        public bool SupportsEditing { get { return true; } }
        public string TypeName { get { return PluginResources.Plugin_NiceName + "..."; } }
        public string TypeDescription { get { return PluginResources.Plugin_Description; } }

        /// <summary>
        /// Show the plug-in settings form when the user is adding the translation provider plug-in through the GUI of SDL Trados Studio
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="languagePairs"></param>
        /// <param name="credentialStore"></param>
        /// <returns></returns>
        public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            LetsMTTranslationProviderOptions opts = new LetsMTTranslationProviderOptions();

            int letsmtNum = 0;
            Uri letsmtUri = null;

            TranslationProviderCredential credentialData = null;

            do
            {
                letsmtNum++;
                letsmtUri = new Uri(opts.Uri.ToString() + letsmtNum.ToString());
                credentialData = credentialStore.GetCredential(letsmtUri);
            } while (credentialData != null);

            int lastUsedLetsmtNum = letsmtNum - 1;
            Uri lastUsedLetsmtUri = new Uri(opts.Uri.ToString() + lastUsedLetsmtNum.ToString());
            string lastUsedToken = null;
            if (lastUsedLetsmtNum > 0)
            {
                credentialData = credentialStore.GetCredential(lastUsedLetsmtUri);
                ApiCredential apiCredentaial = ApiCredential.ParseCredential(credentialData.Credential);
                lastUsedToken = apiCredentaial.Token;
            }

            PasswordForm pf = new PasswordForm(lastUsedToken);

            while (pf.ShowDialog(owner) == DialogResult.OK)
            {

                if (lastUsedToken != pf.strToken)
                {
                    //TODO: check how to minimize the amount odfsystem list calls
                    string credentials = string.Format("{0}\t{1}", pf.strToken, pf.strAppId);

                    TranslationProviderCredential tc = new TranslationProviderCredential(credentials, pf.bRemember);
                    //ad a new uri to handle multiple plugins and users

                    credentialStore.AddCredential(letsmtUri, tc);
                }
                else
                {
                    letsmtUri = lastUsedLetsmtUri;
                }

                LetsMTTranslationProvider testProvider = new LetsMTTranslationProvider(credentialStore, letsmtUri,85);// (dialog.Options);

                //credentialStore.AddCredential(opts.Uri, tc);
                ////TODO: Check if we need a "testProvider"
                //LetsMTTranslationProvider testProvider = new LetsMTTranslationProvider(credentialStore, opts.Uri);// (dialog.Options);

                if (ValidateTranslationProviderLocaly(testProvider))
                {

                    Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProvider[] ResultProv = new ITranslationProvider[] { testProvider };
                    //Open system select screen emidetly for user frendlier setup
                    if (Edit(owner, ResultProv[ResultProv.Length - 1], languagePairs, credentialStore))
                    {
                        return ResultProv;
                    }
                }
                else
                {
                    //IF USERNAME INFOREC REMOVE DATA FROM STORE
                    credentialStore.RemoveCredential(letsmtUri);
                }
                
            }
            return null;
        }

        /// <summary>
        /// If the plug-in settings can be changed by the user, SDL Trados Studio will display a Settings button.
        /// By clicking this button, users raise the plug-in user interface, in which they can modify any applicable settings, in our implementation
        /// the delimiter character and the list file name.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="translationProvider"></param>
        /// <param name="languagePairs"></param>
        /// <param name="credentialStore"></param>
        /// <returns></returns>
        public bool Edit(IWin32Window owner, ITranslationProvider translationProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            LetsMTTranslationProvider editProvider = translationProvider as LetsMTTranslationProvider;
            if (editProvider == null)
                return false;

            editProvider.DownloadProfileList(true);

            SettingsForm settings = new SettingsForm(ref editProvider, languagePairs, credentialStore);
            if (!settings.TranslationProviderInitialized)
            {
                return false;
            }

            DialogResult  dResult = settings.ShowDialog(owner);
            if (dResult == DialogResult.OK)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Can be used in implementations in which a user login is required, e.g. for connecting to an online translation provider.
        /// In our implementation, however, this is not required, so we simply set this member to return always True.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="translationProviderUri"></param>
        /// <param name="translationProviderState"></param>
        /// <param name="credentialStore"></param>
        /// <returns></returns>
        public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {
            PasswordForm pf = new PasswordForm();

            if (pf.ShowDialog(owner) == DialogResult.OK)
            {
                //TranslationProviderCredential tc = new TranslationProviderCredential(string.Format("{0}\t{1}\t{2}", pf.strUsername, pf.strPassword,pf.strAppId), pf.bRemember);
                TranslationProviderCredential tc = new TranslationProviderCredential(string.Format("{0}\t{1}", pf.strToken, pf.strAppId), pf.bRemember);

                credentialStore.AddCredential(translationProviderUri, tc);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Used for displaying the plug-in info such as the plug-in name, tooltip, and icon.
        /// </summary>
        /// <param name="translationProviderUri"></param>
        /// <param name="translationProviderState"></param>
        /// <returns></returns>
        public TranslationProviderDisplayInfo GetDisplayInfo(Uri translationProviderUri, string translationProviderState)
        {
            TranslationProviderDisplayInfo info = new TranslationProviderDisplayInfo();

            info.Name = PluginResources.Plugin_NiceName;
            info.TranslationProviderIcon = PluginResources.LetsMT;
            info.TooltipText = PluginResources.Plugin_Tooltip;
            info.SearchResultImage = PluginResources.logo_small;


            return info;
        }

        public bool SupportsTranslationProviderUri(Uri translationProviderUri)
        {
            if (translationProviderUri == null)
                throw new ArgumentNullException("URI not supported by the plug-in.");

            return string.Equals(translationProviderUri.Scheme, LetsMTTranslationProvider.TranslationProviderScheme, StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion

        //NOT USED FOR NOW

        private bool ValidateCredentialsLocaly(string username, string password, string appID)
        {


            global::System.Resources.ResourceManager resourceManager = new global::System.Resources.ResourceManager("LetsMT.MTProvider.PluginResources", typeof(PluginResources).Assembly);
            // create Web Service client
            string url = resourceManager.GetString("WebServiceUrl");

            try
            {
                Microsoft.Win32.RegistryKey key;
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\\Tilde\\LetsMT");
                if (key != null)
                {
                    string RegUrl = key.GetValue("url", "none").ToString();
                    if (RegUrl.Length > 3)
                    {
                        if (RegUrl.Substring(0, 4) == "http") { url = RegUrl; }
                    }
                }

            }
            catch (Exception) { }

            EndpointAddress endpoint = new EndpointAddress(url);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;


            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            LetsMTAPI.TranslationServiceContractClient TestService = new LetsMTAPI.TranslationServiceContractClient(binding, endpoint);

           TestService.ClientCredentials.UserName.UserName = username;
           TestService.ClientCredentials.UserName.Password = password;


            bool bCredentialsValid = true;
            try
            {
                TestService.GetUserInfo(appID);
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("The HTTP request is unauthorized"))
                {
                    MessageBox.Show("Unrecognized username or password.", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    bCredentialsValid = false;
                }
                else
                {
                    MessageBox.Show(ex.ToString());
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("Cannot connect to server.", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    bCredentialsValid = false;
                }
            }
            

            return bCredentialsValid;
        }


        public static bool ValidateTranslationProviderLocaly(LetsMTTranslationProvider testProvider)
        {




            bool bCredentialsValid = true;
            try
            {
                testProvider.m_service.GetUserInfo(testProvider.m_strAppID);
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("The HTTP request is unauthorized"))
                {
                    MessageBox.Show("Invalid Client ID.", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    bCredentialsValid = false;
                }
                else if (ex.Message.Contains("code:"))
                {

                    Form UForm = null;

                    if ((UForm = testProvider.IsFormAlreadyOpen(typeof(LimitationForm))) == null)
                    {
                        Regex r = new Regex(@"(?<=cide: 1)\d+");
                        Match m = r.Match(ex.Message);
                        string erNum = m.Value;
                        string Error_url = string.Format("https://www.letsmt.eu/Error.aspx?code={0}&user={1}", erNum, testProvider.m_service.ClientCredentials.UserName.UserName);
                        AutoResetEvent formLoadedEvent = new AutoResetEvent(false);
                        var t = new Thread(() => testProvider.CallForm(Error_url, formLoadedEvent));

                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();
                        bCredentialsValid = false;

                        // wait until the limitationForm has loaded
                        try
                        {
                            formLoadedEvent.WaitOne();
                        }
                        catch (AbandonedMutexException aex)
                        {
                            // the limitform crashed or something. continue
                        }
                        ////close the form if it is open
                        //UForm.Close();
                    }



                }
                else
                {
                    MessageBox.Show(ex.ToString());
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("Cannot connect to server.", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    bCredentialsValid = false;
                }
            }


            return bCredentialsValid;
        }
        
    }

}
