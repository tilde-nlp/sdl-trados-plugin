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

namespace LetsMT.MTProvider
{
    public class LetsMTTranslationProvider : ITranslationProvider
    {
        ///<summary>
        /// This string needs to be a unique value. This is the string that precedes the plug-in URI.
        ///</summary>
        public static readonly string TranslationProviderScheme = "letsmt";
        private string m_strCredential;
        private LetsMTWebService.TranslationWebServiceSoapClient m_service;
        public CMtProfileCollection m_profileCollection;

        private static bool ValidateRemoteCertificate(object sender,
                                                      X509Certificate certificate,
                                                      X509Chain chain,
                                                      SslPolicyErrors policyErrors)
        {
            return true;
        }

        public string TranslateText(LanguagePair direction, string text)
        {
            string system = m_profileCollection.GetActiveSystemForProfile(direction);

            if(system != "")
                return m_service.Translate(null, system, text, null);

            //return "";
            throw new Exception("Default system not selected.");
        }

        public LetsMTTranslationProvider(string credential)
        {
            m_strCredential = credential;

            global::System.Resources.ResourceManager resourceManager = new global::System.Resources.ResourceManager("LetsMT.MTProvider.PluginResources", typeof(PluginResources).Assembly);

            // create Web Service client
            string url = resourceManager.GetString("LetsMTWebServiceUrl");
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            EndpointAddress endpoint = new EndpointAddress(url);

            string[] credParams = m_strCredential.Split('\t');

            string strUsername = "";
            string strPassword = "";

            if (credParams.Length > 0)
                strUsername = credParams[0];
            if (credParams.Length > 1)
                strPassword = credParams[1];

            //TODO: HACK {
            // Attach custom Certificate validator to pass validation of untrusted development certificate 
            // TODO: This should be removed when trusted CA certificate will be used (or callback method have to do harder checking)
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            //TODO: HACK }

            m_service = new LetsMTWebService.TranslationWebServiceSoapClient(binding, endpoint);

            m_service.ClientCredentials.UserName.UserName = strUsername;
            m_service.ClientCredentials.UserName.Password = strPassword;

            m_profileCollection = null;
        }

        public bool ValidateCredentials()
        {
            bool bCredentialsValid = false;

            LetsMTWebService.MTSystem[] mtList = m_service.GetSystemList(null, null);

            bCredentialsValid = true;

            return bCredentialsValid;
        }

        public void DownloadProfileList(bool bForce = false)
        {
            if (m_profileCollection != null && !bForce)
                return;

            string state = null;

            if(m_profileCollection != null)
                 state = SerializeState();

            LetsMTWebService.MTSystem[] mtList = m_service.GetSystemList(null, null);

            m_profileCollection = new CMtProfileCollection(mtList);

            if (state != null)
                LoadState(state);
        }

        #region "ITranslationProvider Members"
        public ITranslationProviderLanguageDirection GetLanguageDirection(LanguagePair languageDirection)
        {
            return new ListTranslationProviderLanguageDirection(this, languageDirection);
        }

        public void LoadState(string translationProviderState)
        {
            if (m_profileCollection == null)
                DownloadProfileList();

            m_profileCollection.DeserializeState(translationProviderState);
        }

        public string SerializeState()
        {
            if (m_profileCollection == null)
                DownloadProfileList();

            string state = m_profileCollection.SerializeState();
            
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
            if (m_profileCollection == null)
                DownloadProfileList();

            return m_profileCollection.HasProfile(languageDirection);
        }

        public Uri Uri
        {
            get
            {
                Uri uri = new ListTranslationOptions().Uri;
                return uri;
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
        public bool SupportsPlaceables { get { return false; } }
        public bool SupportsScoring { get { return false; } }
        public bool SupportsSearchForTranslationUnits { get { return true; } }
        public bool SupportsSourceConcordanceSearch { get { return false; } }
        public bool SupportsTargetConcordanceSearch { get { return false; } }
        public bool SupportsStructureContext { get { return false; } }
        public bool SupportsTaggedInput { get { return false; } }
        public bool SupportsTranslation { get { return true; } }
        public bool SupportsUpdate { get { return false; } }
        public bool SupportsWordCounts { get { return false; } }

        #endregion
    }
}