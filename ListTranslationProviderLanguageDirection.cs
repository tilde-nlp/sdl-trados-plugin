using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.LanguagePlatform.Core;
using Sdl.Core.Globalization;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using System.ServiceModel;
using System.Resources;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace LetsMT.MTProvider
{
    public class ListTranslationProviderLanguageDirection : ITranslationProviderLanguageDirection
    {
        #region "PrivateMembers"
        private LetsMTTranslationProvider _provider;
        private LanguagePair _languageDirection;
        private ListTranslationOptions _options;
        private ListTranslationProviderElementVisitor _visitor;
        //private TranslationService.TranslationServiceSoapClient _service;
        private LetsMTWebService.TranslationWebServiceSoapClient _service;
        //private Dictionary<string, string> _listOfTranslations;
        
        /// <summary>
        /// Callback used to validate the certificate in an SSL conversation. This validator recognises all certificates as valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="policyErrors"></param>
        /// <returns></returns>
        private static bool ValidateRemoteCertificate(
        object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors policyErrors
        )
        {
            return true;
        }

        #endregion

        #region "ITranslationProviderLanguageDirection Members"

        /// <summary>
        /// Instantiates the variables and fills the list file content into
        /// a Dictionary collection object.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="languages"></param>
        #region "ListTranslationProviderLanguageDirection"
        public ListTranslationProviderLanguageDirection(LetsMTTranslationProvider provider, LanguagePair languages)
        {
            _provider = provider;
            _languageDirection = languages;

            global::System.Resources.ResourceManager resourceManager = new global::System.Resources.ResourceManager("LetsMT.MTProvider.PluginResources", typeof(PluginResources).Assembly);

            // Attach custom Certificate validator to pass validation of untrusted development certificate 
            // TODO: This should be removed when trusted CA certificate will be used (or callback method have to do harder checking)
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);

            //_options = _provider.Options;
            _visitor = new ListTranslationProviderElementVisitor(_options);
            //System.ServiceModel.Channels.Binding binding = new System.ServiceModel.BasicHttpBinding();
            //EndpointAddress endpoint = new EndpointAddress(resourceManager.GetString("TranslationServiceUrl"));
            //_service = new TranslationService.TranslationServiceSoapClient(binding, endpoint);

            // create Web Service client
            string url = resourceManager.GetString("LetsMTWebServiceUrl");
            System.ServiceModel.Channels.Binding binding = new System.ServiceModel.BasicHttpBinding(BasicHttpSecurityMode.Transport);
            EndpointAddress endpoint = new EndpointAddress(url);
            //System.ServiceModel.HttpTransportSecurity.ClientCredentialType.

            _service = new LetsMTWebService.TranslationWebServiceSoapClient(binding, endpoint);
            // provide authentication
            NetworkCredential netCredential = new NetworkCredential(Properties.Settings.Default.user, Properties.Settings.Default.password, "");
            _service.ClientCredentials.Windows.ClientCredential = netCredential.GetCredential(new Uri(url), "Basic");

            File.WriteAllLines(@"c:\Temp\LetsMT.log", new string[] { 
                url,
                Properties.Settings.Default.applicationID, 
                Properties.Settings.Default.systemID,
                Properties.Settings.Default.user, 
                Properties.Settings.Default.password }, Encoding.UTF8);
        }

        #endregion

        public System.Globalization.CultureInfo SourceLanguage
        {
            get { return _languageDirection.SourceCulture; }
        }

        public System.Globalization.CultureInfo TargetLanguage
        {
            get { return _languageDirection.TargetCulture; }
        }

        public ITranslationProvider TranslationProvider
        {
            get { return _provider; }
        }

        /// <summary>
        /// Performs the actual search by looping through the
        /// delimited segment pairs contained in the text file.
        /// Depening on the search mode, a segment lookup (with exact machting) or a source / target
        /// concordance search is done.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        #region "SearchSegment"
        public SearchResults SearchSegment(SearchSettings settings, Segment segment)
        {
            // Loop through segment elements to 'filter out' e.g. tags in order to 
            // make certain that only plain text information is retrieved for
            // this simplified implementation.            
            _visitor.Reset();
            foreach (var element in segment.Elements)
            {
                element.AcceptSegmentElementVisitor(_visitor);
            }

            SearchResults results = new SearchResults();
            results.SourceSegment = segment.Duplicate();

            Segment translation = new Segment(_languageDirection.TargetCulture);
            string translText = _service.Translate(Properties.Settings.Default.applicationID, Properties.Settings.Default.systemID, _visitor.PlainText, "");
            translation.Add(translText);
            results.Add(CreateSearchResult(segment, translation));
            
            return results;
        }
        #endregion


        /// <summary>
        /// Creates the translation unit as it is later shown in the Translation Results
        /// window of SDL Trados Studio.
        /// </summary>
        /// <param name="searchSegment"></param>
        /// <param name="translation"></param>
        /// <param name="sourceSegment"></param>
        /// <returns></returns>
        #region "CreateSearchResult"
        private SearchResult CreateSearchResult(Segment searchSegment, Segment translation)
        {                
                TranslationUnit tu = new TranslationUnit();
                tu.SourceSegment = searchSegment.Duplicate();
                tu.TargetSegment = translation; 
                tu.ResourceId = new PersistentObjectToken(tu.GetHashCode(), Guid.Empty);
                tu.Origin = TranslationUnitOrigin.MachineTranslation;
                tu.SystemFields.CreationDate = DateTime.UtcNow;
                tu.SystemFields.ChangeDate = tu.SystemFields.CreationDate;
                SearchResult searchResult = new SearchResult(tu);
                searchResult.ScoringResult = new ScoringResult();
                searchResult.TranslationProposal = tu.Duplicate();
                //tu.ConfirmationLevel = ConfirmationLevel.Translated;
                return searchResult;
        }
        #endregion


        public bool CanReverseLanguageDirection
        {
            get { return false; }
        }
        
        public SearchResults[] SearchSegments(SearchSettings settings, Segment[] segments)
        {
            SearchResults[] results = new SearchResults[segments.Length];
            for (int p = 0; p < segments.Length; ++p)
            {
                results[p] = SearchSegment(settings, segments[p]);
            }
            return results;
        }

        public SearchResults[] SearchSegmentsMasked(SearchSettings settings, Segment[] segments, bool[] mask)
        {
            if (segments == null)
            {
                throw new ArgumentNullException("segments in SearchSegmentsMasked");
            }
            if (mask == null || mask.Length != segments.Length)
            {
                throw new ArgumentException("mask in SearchSegmentsMasked");
            }

            SearchResults[] results = new SearchResults[segments.Length];
            for (int p = 0; p < segments.Length; ++p)
            {
                if (mask[p])
                {
                    results[p] = SearchSegment(settings, segments[p]);
                }
                else
                {
                    results[p] = null;
                }
            }

            return results;
        }

        public SearchResults SearchText(SearchSettings settings, string segment)
        {
            Segment s = new Sdl.LanguagePlatform.Core.Segment(_languageDirection.SourceCulture);
            s.Add(segment);
            return SearchSegment(settings, s);
        }

        public SearchResults SearchTranslationUnit(SearchSettings settings, TranslationUnit translationUnit)
        {
            return SearchSegment(settings, translationUnit.SourceSegment);
        }

        public SearchResults[] SearchTranslationUnits(SearchSettings settings, TranslationUnit[] translationUnits)
        {
            SearchResults[] results = new SearchResults[translationUnits.Length];
            for (int p = 0; p < translationUnits.Length; ++p)
            {
                results[p] = SearchSegment(settings, translationUnits[p].SourceSegment);
            }
            return results;
        }

        public SearchResults[] SearchTranslationUnitsMasked(SearchSettings settings, TranslationUnit[] translationUnits, bool[] mask)
        {
            List<SearchResults> results = new List<SearchResults>();

            int i = 0;
            foreach (var tu in translationUnits)
            {
                if (mask == null || mask[i])
                {
                    var result = SearchTranslationUnit(settings, tu);
                    results.Add(result);
                }
                else
                {
                    results.Add(null);
                }
                i++;
            }

            return results.ToArray();
        }



        #region "NotForThisImplementation"
        /// <summary>
        /// Not required for this implementation.
        /// </summary>
        /// <param name="translationUnits"></param>
        /// <param name="settings"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public ImportResult[] AddTranslationUnitsMasked(TranslationUnit[] translationUnits, ImportSettings settings, bool[] mask)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not required for this implementation.
        /// </summary>
        /// <param name="translationUnit"></param>
        /// <returns></returns>
        public ImportResult UpdateTranslationUnit(TranslationUnit translationUnit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not required for this implementation.
        /// </summary>
        /// <param name="translationUnits"></param>
        /// <returns></returns>
        public ImportResult[] UpdateTranslationUnits(TranslationUnit[] translationUnits)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Not required for this implementation.
        /// </summary>
        /// <param name="translationUnits"></param>
        /// <param name="previousTranslationHashes"></param>
        /// <param name="settings"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public ImportResult[] AddOrUpdateTranslationUnitsMasked(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings, bool[] mask)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not required for this implementation.
        /// </summary>
        /// <param name="translationUnit"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public ImportResult AddTranslationUnit(TranslationUnit translationUnit, ImportSettings settings)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not required for this implementation.
        /// </summary>
        /// <param name="translationUnits"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public ImportResult[] AddTranslationUnits(TranslationUnit[] translationUnits, ImportSettings settings)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not required for this implementation.
        /// </summary>
        /// <param name="translationUnits"></param>
        /// <param name="previousTranslationHashes"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public ImportResult[] AddOrUpdateTranslationUnits(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings)
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion
    }
}
