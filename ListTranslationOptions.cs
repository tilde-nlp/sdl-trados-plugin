﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using System.Windows.Forms;

namespace LetsMT.MTProvider
{
    /// <summary>
    /// This class is used to hold the provider plug-in settings. 
    /// All settings are automatically stored in a URI.
    /// </summary>
    public class ListTranslationOptions
    {
        #region "TranslationMethod"
        public static readonly TranslationMethod ProviderTranslationMethod = TranslationMethod.MachineTranslation;
        #endregion

        #region "TranslationProviderUriBuilder"
        TranslationProviderUriBuilder _uriBuilder;        

        public ListTranslationOptions()
        {
            _uriBuilder = new TranslationProviderUriBuilder(LetsMTTranslationProvider.TranslationProviderScheme);
            SetStringParameter("param1", "value1");
        }

        public ListTranslationOptions(Uri uri)
        {
            _uriBuilder = new TranslationProviderUriBuilder(uri);            
        }
        #endregion

        /// <summary>
        /// Set and retrieve the name and path of the delimited list file.
        /// </summary>
        #region "ListFileName"
        public string ListFileName
        {
            get { return GetStringParameter("listfile"); }
            set { SetStringParameter("listfile", value); }            
        }
        #endregion

        /// <summary>
        /// Set and retrieve the delimiter character.
        /// </summary>
        #region "Delimiter"
        public string Delimiter
        {
            get { return GetStringParameter("delimiter");}
            set {SetStringParameter("delimiter", value);}
        }
        #endregion

        #region "SetStringParameter"
        private void SetStringParameter(string p, string value)
        {
            _uriBuilder[p] = value;
        }
        #endregion

        #region "GetStringParameter"
        private string GetStringParameter(string p)
        {
            string paramString = _uriBuilder[p];
            return paramString;
        }
        #endregion


        #region "Uri"
        public Uri Uri
        {            
            get
            {
                return _uriBuilder.Uri;                
            }
        }
        #endregion
    }
}
