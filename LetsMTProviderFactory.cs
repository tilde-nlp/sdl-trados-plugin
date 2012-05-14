using System;
using System.Collections.Generic;
using System.Text;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using LetsMT.MTProvider;

namespace LetsMT.MTProvider
{
    [TranslationProviderFactory(Id = "LetsMTProviderFactory")] //Setting only mandatory arguments

    public class LetsMTProviderFactory : ITranslationProviderFactory
    {
        public ITranslationProvider CreateTranslationProvider(Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {
            if (translationProviderUri == null || credentialStore == null) //Throw allowed exception if no arguments. TODO: translationProviderState, optional?
                throw new ArgumentNullException("Missing arguments.");

            if (!SupportsTranslationProviderUri(translationProviderUri)) //Wrong Uri, Trados is probably looking for another provider
                throw new Exception("Cannot handle URI.");

            TranslationProviderCredential credentialData = credentialStore.GetCredential(translationProviderUri); //Make sure we have credentials, if not, throw exception to ask user
            if(credentialData == null)
                throw new TranslationProviderAuthenticationException();

            string credential = credentialData.Credential; //Get the credentials in form "{0}\t{1}\t{3}", where 0 - username, 1 - password and 3 - appId

            LetsMTTranslationProvider translationProvider = new LetsMTTranslationProvider(credential); //Create the provider passing required parameters
            
            if(!translationProvider.ValidateCredentials()) //If credentials are incorrect, ask again
                throw new TranslationProviderAuthenticationException();

            translationProvider.LoadState(translationProviderState);

            return translationProvider; //Provider is good and user is authorized, return
        }

        public TranslationProviderInfo GetTranslationProviderInfo(Uri translationProviderUri, string translationProviderState)
        {
            if (translationProviderUri == null) //Throw allowed exception if no arguments, translationProviderState is optional
                throw new ArgumentNullException("Missing arguments.");

            if (!SupportsTranslationProviderUri(translationProviderUri)) //Wrong Uri, Trados is probably looking for another provider
                throw new Exception("Cannot handle URI.");

            TranslationProviderInfo info = new TranslationProviderInfo(); //User friendly name and translation method entry point

            info.Name = PluginResources.Plugin_NiceName;
            info.TranslationMethod = LetsMTTranslationProviderOptions.ProviderTranslationMethod; //TODO: get from provider, merge options and provider?

            return info;
        }

        public bool SupportsTranslationProviderUri(Uri translationProviderUri)
        {
            if (translationProviderUri == null) //Throw allowed exception if no arguments
                throw new ArgumentNullException("Missing arguments.");

            return string.Equals(translationProviderUri.Scheme, LetsMTTranslationProvider.TranslationProviderScheme, StringComparison.OrdinalIgnoreCase);
        }
    }
}