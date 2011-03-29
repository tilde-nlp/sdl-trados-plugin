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
    public class ListTranslationOptions
    {
        public static readonly TranslationMethod ProviderTranslationMethod = TranslationMethod.MachineTranslation;
        private TranslationProviderUriBuilder m_uriBuilder;

        public ListTranslationOptions()
        {
            m_uriBuilder = new TranslationProviderUriBuilder(LetsMTTranslationProvider.TranslationProviderScheme);
            //SetStringParameter("param1", "value1");
        }

        //public ListTranslationOptions(Uri uri)
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