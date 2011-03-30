using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace LetsMT.MTProvider
{
    [TranslationProviderWinFormsUi(Id = "ListTranslationProviderWinFormsUI")]

    public class ListTranslationProviderWinFormsUI : ITranslationProviderWinFormsUI
    {
        #region ITranslationProviderWinFormsUI Members

        /// <summary>
        /// Determines whether the plug-in settings can be changed by displaying the Settings button in SDL Trados Studio.
        /// </summary>
        public bool SupportsEditing { get { return true; } }
        public string TypeName { get { return PluginResources.Plugin_NiceName; } }
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
            ListTranslationOptions opts = new ListTranslationOptions();

            PasswordForm pf = new PasswordForm();

            if (pf.ShowDialog(owner) == DialogResult.OK)
            {
                string credentials = string.Format("{0}\t{1}", pf.strUsername, pf.strPassword);

                TranslationProviderCredential tc = new TranslationProviderCredential(credentials, pf.bRemember);

                credentialStore.AddCredential(opts.Uri, tc);

                LetsMTTranslationProvider testProvider = new LetsMTTranslationProvider(credentials);// (dialog.Options);

                if(testProvider.ValidateCredentials())
                    return new ITranslationProvider[] { testProvider }; 
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

            SettingsForm settings = new SettingsForm(ref editProvider, languagePairs);
            if(settings.ShowDialog(owner) == DialogResult.OK)
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
                TranslationProviderCredential tc = new TranslationProviderCredential(string.Format("{0}\t{1}", pf.strUsername, pf.strPassword), pf.bRemember);

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
            info.TranslationProviderIcon = PluginResources.band_aid_icon;
            info.TooltipText = PluginResources.Plugin_Tooltip;
            info.SearchResultImage = PluginResources.band_aid_symbol;

            return info;
        }

        public bool SupportsTranslationProviderUri(Uri translationProviderUri)
        {
            if (translationProviderUri == null)
                throw new ArgumentNullException("URI not supported by the plug-in.");

            return string.Equals(translationProviderUri.Scheme, LetsMTTranslationProvider.TranslationProviderScheme, StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion
    }
}
