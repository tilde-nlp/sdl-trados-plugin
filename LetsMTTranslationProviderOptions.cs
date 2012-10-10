using System;
using System.Collections.Generic;
using System.Text;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using System.Windows.Forms;

namespace LetsMT.MTProvider
{
    /// <summary>
    /// This class is used to hold the provider plug-in settings. All settings are automatically stored in a URI.
    /// </summary>
    public class LetsMTTranslationProviderOptions
    {
        public static readonly TranslationMethod ProviderTranslationMethod = TranslationMethod.MachineTranslation;
        private TranslationProviderUriBuilder m_uriBuilder;

        public LetsMTTranslationProviderOptions()
        {
            m_uriBuilder = new TranslationProviderUriBuilder(LetsMTTranslationProvider.TranslationProviderScheme);
        }

        //public LetsMTTranslationProviderOptions(ITranslationProviderCredentialStore credentialStore)
        //{

        //    //ad a new uri to handle multiple plugins
        //    int letsmtNum = 1;
        //    Uri letsmtUri = new TranslationProviderUriBuilder("letsmt" + letsmtNum.ToString()).Uri;

        //    //new TranslationProviderUriBuilder("letsmt" + letsmtNum.ToString()).Uri;
        //    //letsmtUri.
        //    TranslationProviderCredential credentialData = credentialStore.GetCredential(letsmtUri);
        //    while (credentialData != null)
        //    {
        //        letsmtNum++;
        //        letsmtUri = new TranslationProviderUriBuilder("letsmt" + letsmtNum.ToString()).Uri ;
        //        //letsmtUri.
        //        credentialData = credentialStore.GetCredential(letsmtUri);
        //    }



        //    m_uriBuilder = new TranslationProviderUriBuilder(letsmtUri);
        //    m_uriBuilder.Type = LetsMTTranslationProvider.TranslationProviderScheme;
            
        //    //m_uriBuilder.Uri = ;
            
        //    //SetStringParameter("param1", "value1");
        //}

        //public LetsMTTranslationProviderOptions(Uri uri)
        //{
        //    m_uriBuilder = new TranslationProviderUriBuilder(uri);
        //}

        //private void SetStringParameter(string p, string value)
        //{
        //    m_uriBuilder[p] = value;
        //}

        //private string GetStringParameter(string p)
        //{
        //    string paramString = m_uriBuilder[p];
        //    return paramString;
        //}

        public Uri Uri
        {
            get { return m_uriBuilder.Uri; }
        }
    }
}