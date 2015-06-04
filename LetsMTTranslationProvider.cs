using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Net;
using System.Net.Security;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using LetsMT.MTProvider;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Channels;

using System.Text.RegularExpressions;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Threading;

namespace LetsMT.MTProvider
{
    public class LetsMTTranslationProvider : ITranslationProvider
    {
        ///<summary>
        /// This string needs to be a unique value. This is the string that precedes the plug-in URI.
        ///</summary>
        public static readonly string TranslationProviderScheme = "letsmt";
        public string m_username;
        private string m_strCredential;
        private ITranslationProviderCredentialStore m_store;
        private Uri m_uri;
        public string m_strAppID;
        public int m_resultScore;
        public LetsMTAPI.TranslationServiceContractClient m_service;
        public CMtProfileCollection m_profileCollection;
        public bool m_userRetryWarning;


        /// <summary>
        /// Constructor taht get cridentals
        /// </summary>
        /// <param name="credentialStore"></param>
        /// <param name="translationProviderUri"></param>
        /// <param name="resultScore"></param>
        public LetsMTTranslationProvider(ITranslationProviderCredentialStore credentialStore, Uri translationProviderUri, int resultScore)
        {
            m_resultScore = resultScore;
            m_uri = translationProviderUri;
            m_store = credentialStore;
            m_userRetryWarning = true;

            TranslationProviderCredential credentialData = credentialStore.GetCredential(translationProviderUri); //Make sure we have credentials, if not, throw exception to ask user
            if (credentialData == null)
                throw new TranslationProviderAuthenticationException();

            string credential = credentialData.Credential; //Get the credentials in form "{0}\t{1}\t{3}", where 0 - username, 1 - password and 3 - appId

            m_strCredential = credential;

            

            string[] credParams = m_strCredential.Split('\t');

            string m_strToken = "";
            m_strAppID = "";

            if (credParams.Length > 0)
                m_strToken = credParams[0];
            if (credParams.Length > 1)
            {
                m_strAppID = credParams[1];
            }

            InitService(m_strToken);
        }

        public void InitService(string m_strToken)
        {
            global::System.Resources.ResourceManager resourceManager = new global::System.Resources.ResourceManager("LetsMT.MTProvider.PluginResources", typeof(PluginResources).Assembly);

            // create Web Service client
            string url = resourceManager.GetString("LetsMTWebServiceUrl");
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            // remove buffet limmit
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            // binding.ReceiveTimeout = 

            //try to read Software\\Tilde\\LetsMT\\url registry string entry and it it exists replace the link
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


            //TODO: HACK {
            // Attach custom Certificate validator to pass validation of untrusted development certificate 
            // TODO: This should be removed when trusted CA certificate will be used (or callback method have to do harder checking)
            // ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            //TODO: HACK }

            //binding.Security.Transport.
            //if (m_strAppID != "")
            //{
            //    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            //}
            //else
            //{
            //    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            //}
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            m_service = new LetsMTAPI.TranslationServiceContractClient(binding, endpoint);

            HeaderManagerMessageInspector inspector = new HeaderManagerMessageInspector("client-id", m_strToken);
            InspectorBehaviour addUserIdBehaviour = new InspectorBehaviour(inspector);
            m_service.Endpoint.Behaviors.Add(addUserIdBehaviour);

            // m_service.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.PeerTrust;

            m_profileCollection = null;
        }


        private static bool ValidateRemoteCertificate(object sender,
                                                      X509Certificate certificate,
                                                      X509Chain chain,
                                                      SslPolicyErrors policyErrors)
        {
            return true;
        }

        private static string RemoveControlCharacters(string inString)
        { 
            if (inString == null) return null;

            StringBuilder newString = new StringBuilder();
            char ch;

            for (int i = 0; i < inString.Length; i++)
            {

                ch = inString[i];

                if (!char.IsControl(ch))
                {
                    newString.Append(ch);
                }
                else
                {
                    newString.Append(' ');
                }
            }
            return newString.ToString();
        }


        public  Form IsFormAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }

            return null;
        }

        /// <summary>
        /// Calls the notificaton form in a separet thread.
        /// </summary>
        /// <param name="url"></param>
        [STAThread]
        public  void CallForm(string url)
        {
            LimitationForm LimitForm = new LimitationForm(url);
            LimitForm.ShowDialog();
        }
        

        /// <summary>
        /// The main method that dose the translation.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public void StoreTranslation(LanguagePair direction, string sourceText, string targhetText)
        {
            string system = m_profileCollection.GetActiveSystemForProfile(direction);
            if (system != "")
            {
                try
                {
                    m_service.UpdateTranslation(m_strAppID, system, RemoveControlCharacters(sourceText), RemoveControlCharacters(targhetText), "client=SDLTradosStudio,version=1.5");
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// The main method that dose the translation.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public string TranslateText(LanguagePair direction, string text)
        {
            string system = m_profileCollection.GetActiveSystemForProfile(direction);
            string terms = m_profileCollection.GetActiveTermCorporaForSystem(direction, system);

            if (system != "")
            {
                string result = "";
                try
                {
                    ((IContextChannel)m_service.InnerChannel).OperationTimeout = new TimeSpan(0, 0, 10); 
                    //removes control characters  to work around imperfections in the way .NET handles SOAP.
                    result = m_service.Translate(m_strAppID, system, RemoveControlCharacters(text), string.Format("client=SDLTradosStudio,version=1.5,termCorpusId={0}", terms));
                }
                catch(Exception ex)
                {
                    if (ex.Message.Contains("code: 1"))
                    {
                        Form UForm = null;
                        
                        if  ( (UForm = IsFormAlreadyOpen(typeof(LimitationForm))) == null)
                        {

                            Regex r = new Regex(@"(?<=code: )\d+");
                            Match m = r.Match(ex.Message);
                            string erNum = m.Value;
                            string Error_url = string.Format("https://www.letsmt.eu/Error.aspx?code={0}&user={1}", erNum, m_service.ClientCredentials.UserName.UserName);
                            var t = new Thread(() => CallForm(Error_url));

                            t.SetApartmentState(ApartmentState.STA);
                            t.Start();
                            ////close the form if it is open
                        }
                    }
                    else if (ex.Message.Contains("code: 41")) // Term corpora id failed validation
                    {
                        m_profileCollection.SetActiveTermCorporaForSystem(direction, system, ""); // This doesn't get serialized until the user opens the settings form. If Trados is closed before that the faulty term id is used again on the next run.
                        return TranslateText(direction, text);
                    }
                    else if    (ex.Message.Contains("code:"))
                    {
                        Regex r = new Regex(@"(?<=description: ).+$");
                        Match m = r.Match(ex.Message);
                        string errText;
                        if (m.Success)
                        {
                            errText = m.Value;
                        }
                        else
                        {
                            errText = "The service was unable to acquire a translation.";
                        }
                        throw new Exception(errText);
                    }
                    else if (ex.Message.StartsWith("The request channel timed out "))
                    {
                        return "";
                    }
                    else if (ex.Message.Contains("The HTTP request is unauthorized"))
                    {
                        // throw new Exception("Unrecognized username or password.");
                        throw new TranslationProviderAuthenticationException();
                    }
                    else
                    {
                        throw new Exception("Could not connect to translation provider.");
                    }
                   
                }

                return result;
            }
                //return "";
            throw new Exception("Default system for this languge pair not selected.");
            
        }



        public bool ValidateCredentials()
        {
           //There is no reson to revalidate the user.
            return true; 


            //bool bCredentialsValid = false;
            //try
            //{
            //    m_service.GetUserInfo("");
            //    bCredentialsValid = true;
            //    //m_service.Translate(m_strAppID, "*", "*", "client=SDLTradosStudio");
            //    //LetsMTWebService.MTSystem[] mtList = m_service.GetSystemList(, null);
            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message.Contains("The HTTP request is unauthorized"))
            //    {
            //        throw new Exception("Unrecognized username or password.");
            //    }
            //    else if (ex.Message.Contains("User limitation error:"))
            //    {

            //        Form UForm = null;

            //        if ((UForm = IsFormAlreadyOpen(typeof(LimitationForm))) == null)
            //        {

            //            Regex r = new Regex(@"(?<=User limitation error: )\d+");
            //            Match m = r.Match(ex.Message);
            //            string erNum = m.Value;
            //            string Error_url = string.Format("https://www.letsmt.eu/Error.aspx?code={0}&user={1}", erNum, m_service.ClientCredentials.UserName.UserName);
            //            var t = new Thread(() => CallForm(Error_url));

            //            t.SetApartmentState(ApartmentState.STA);
            //            t.Start();
            //            ////close the form if it is open
            //            //UForm.Close();
            //        }

                   

            //        //TODO: It would be nice to diable the plugin afterwards
            //        throw new Exception("User limitation reched.");

            //    }
            //    else 
            //    {        
            //         throw new Exception("Cannot connect to server.");
            //    }
            //}
            //bCredentialsValid = true;

            //return bCredentialsValid;
        }

        public string GetStoreCridential()
        {

            TranslationProviderCredential credentialData = m_store.GetCredential(m_uri); //Make sure we have credentials, if not, throw exception to ask user
            if (credentialData == null)
                throw new TranslationProviderAuthenticationException();

            //string credential = 
             return  credentialData.Credential; //Get the credentials in form "{0}\t{1}\t{3}", where 0 - username, 1 - password and 3 - appId
        }


        #region "ITranslationProvider Members"
        public void DownloadProfileList(bool bForce = false)
        {
            if (m_profileCollection != null && !bForce)
                return;

            string state = null;

            if (m_profileCollection != null)
                state = SerializeState();

            //LetsMTWebService.MTSystem[] mtList = m_service.GetSystemList(m_strAppID, null);
            ((IContextChannel)m_service.InnerChannel).OperationTimeout = new TimeSpan(0, 1, 0);
            LetsMTAPI.MTSystem[] mtList = null;
            try
            {
                mtList = m_service.GetSystemList(m_strAppID, null, null).System;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The HTTP request is unauthorized"))
                {
                    throw new TranslationProviderAuthenticationException();
                }
                else
                {
                    throw new Exception("Could not connect to translation provider.");
                }

            }
            m_profileCollection = new CMtProfileCollection(mtList);

            if (state != null)
                LoadState(state);
        }

        public ITranslationProviderLanguageDirection GetLanguageDirection(LanguagePair languageDirection)
        {
            return new LetsMTTranslationProviderLanguageDirection(this, languageDirection);
        }

        public void LoadState(string translationProviderState)
        {
            if (m_profileCollection == null)
                DownloadProfileList();
            if (translationProviderState.StartsWith("resultScore"))
            {
                int lineindex = translationProviderState.IndexOf('\n');
                if (lineindex != -1)
                {
                    string scroresString = translationProviderState.Substring(0, lineindex);
                    Regex r = new Regex(@"(?<=resultScore:)\d+");
                    Match m = r.Match(scroresString);
                    string Strscore = m.Value;
                    int score;
                    if (int.TryParse(Strscore, out score))
                    {
                        m_resultScore = score;
                    }

                    if (lineindex < translationProviderState.Length)
                    {
                        translationProviderState = translationProviderState.Remove(0, lineindex + 1);
                    }
                    else
                    {
                        return;
                    }

                }
               
            }
            m_profileCollection.DeserializeState(translationProviderState);
        }

        public string SerializeState()
        {
            if (m_profileCollection == null)
                DownloadProfileList();

            string state = "resultScore:" + m_resultScore.ToString() + "\n" + m_profileCollection.SerializeState();
            
            return state;
        }

        public void RefreshStatusInfo()
        {
        }

        public ProviderStatusInfo StatusInfo
        {
            get { return new ProviderStatusInfo(true, PluginResources.Plugin_NiceName); }
        }

        public bool SupportsLanguageDirection(LanguagePair languageDirection)
        {
            //this causes high waiting times and gives no valuabel informaion
            return true;
            
            //if (m_profileCollection == null)
               //DownloadProfileList();

            //return m_profileCollection.HasProfile(languageDirection);
        }

        public Uri Uri
        {
            get
            {
                return m_uri;
                //Uri uri = new LetsMTTranslationProviderOptions().Uri;
                //return uri;
            }
        }



        public string Name { get { return PluginResources.Plugin_NiceName; } }
        public TranslationMethod TranslationMethod { get { return TranslationMethod.MachineTranslation; } }
        public bool IsReadOnly { get { return false; } }
        public bool SupportsConcordanceSearch { get { return false; } }
        public bool SupportsDocumentSearches { get { return false; } }
        public bool SupportsFilters { get { return false; } }
        public bool SupportsFuzzySearch { get { return false; } }
        public bool SupportsMultipleResults { get { return false; } }
        public bool SupportsPenalties { get { return false; } }
        public bool SupportsPlaceables { get { return true; } }
        public bool SupportsScoring { get { return false; } }
        public bool SupportsSearchForTranslationUnits { get { return true; } }
        public bool SupportsSourceConcordanceSearch { get { return false; } }
        public bool SupportsTargetConcordanceSearch { get { return false; } }
        public bool SupportsStructureContext { get { return false; } }
        public bool SupportsTaggedInput { get { return true; } }
        public bool SupportsTranslation { get { return true; } }
        public bool SupportsUpdate { get { return true; } }
        public bool SupportsWordCounts { get { return false; } }

        #endregion
    }
}