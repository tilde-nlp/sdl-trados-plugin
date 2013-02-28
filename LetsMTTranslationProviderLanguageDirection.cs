using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
    public enum TagTypeEnum
    {
        Opening,
        SelfClosing,
        Closing
    }

    public class LetsMTTranslationProviderLanguageDirection : ITranslationProviderLanguageDirection
    {
        #region "PrivateMembers"
        private LetsMTTranslationProvider _provider;
        private LanguagePair _languageDirection;
        #endregion

        #region "ITranslationProviderLanguageDirection Members"

        /// <summary>
        /// Instantiates the variables and fills the list file content into a Dictionary collection object.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="languages"></param>
        public LetsMTTranslationProviderLanguageDirection(LetsMTTranslationProvider provider, LanguagePair languages)
        {
            _provider = provider;
            _languageDirection = languages;
        }

        static string Segment2Html(Sdl.LanguagePlatform.Core.Segment segment)
        {
            var stringBuilder = new StringBuilder();
            foreach (var element in segment.Elements)
            {
                var text = element as Sdl.LanguagePlatform.Core.Text;
                if (text != null)
                {
                    stringBuilder.Append(text.Value);
                }
                else
                {
                    var tag = element as Sdl.LanguagePlatform.Core.Tag;
                    if (tag != null)
                    {
                        switch (tag.Type)
                        {
                            case Sdl.LanguagePlatform.Core.TagType.Start:
                                stringBuilder.AppendFormat("<span class='{0}' id='{1}'>",
                                        tag.Type, tag.Anchor);
                                break;
                            case Sdl.LanguagePlatform.Core.TagType.End:
                                stringBuilder.AppendFormat("</span>");
                                break;
                            case Sdl.LanguagePlatform.Core.TagType.Standalone:
                            case Sdl.LanguagePlatform.Core.TagType.TextPlaceholder:
                            case Sdl.LanguagePlatform.Core.TagType.LockedContent:
                                //stringBuilder.AppendFormat("<span class='{0}' id='{1}'></span>",=tag.Type, tag.Anchor);
                                stringBuilder.AppendFormat("<span class='{0}' id='{1}'></span> ",
                                         tag.Type, tag.Anchor);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return stringBuilder.ToString();
        }

        static Sdl.LanguagePlatform.Core.Segment TranslatedHtml2Segment(Sdl.LanguagePlatform.Core.Segment sourceSegment, string translatedText)
        {
            var htmlTagName = "span"; // the only we feed for translation is span, so we expect the translation only has span tags too.
            var xmlFragment = "<segment>" + translatedText + "</segment>";
            var xmlReader = new System.Xml.XmlTextReader(xmlFragment, System.Xml.XmlNodeType.Element, null);
            var tagStack = new Stack<Sdl.LanguagePlatform.Core.Tag>();
            var translatedSegment = new Sdl.LanguagePlatform.Core.Segment();
            try
            {
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case System.Xml.XmlNodeType.Element:
                            if (xmlReader.Name == htmlTagName)
                            {
                                var tagClass = xmlReader.GetAttribute("class");
                                var tagType = (Sdl.LanguagePlatform.Core.TagType)
                                     Enum.Parse(typeof(Sdl.LanguagePlatform.Core.TagType), tagClass);
                                int id = Convert.ToInt32(xmlReader.GetAttribute("id"));
                                Sdl.LanguagePlatform.Core.Tag sourceTag = sourceSegment.FindTag(tagType, id);
                                tagStack.Push(sourceTag);
                                translatedSegment.Add(sourceTag.Duplicate());
                            }
                            break;
                        case System.Xml.XmlNodeType.EndElement:
                            {
                                if (xmlReader.Name == htmlTagName)
                                {
                                    var startTag = tagStack.Pop();
                                    if (startTag.Type != Sdl.LanguagePlatform.Core.TagType.Standalone)
                                    {
                                        var endTag = sourceSegment.FindTag(
                                           Sdl.LanguagePlatform.Core.TagType.End, startTag.Anchor);
                                        if (endTag != null)
                                            translatedSegment.Add(endTag.Duplicate());
                                    }
                                }
                            }
                            break;
                        case System.Xml.XmlNodeType.Text:
                            translatedSegment.Add(xmlReader.Value);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                var paintextSegment = new Sdl.LanguagePlatform.Core.Segment();
                string plaitext = Regex.Replace(translatedText, "<[^>]+>", "");
                paintextSegment.Add(plaitext);
                return paintextSegment;
            }

            return translatedSegment;
        }


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
        /// Performs the actual search by looping through the delimited segment pairs contained in the text file.
        /// Depening on the search mode, a segment lookup (with exact machting) or a source / target
        /// concordance search is done.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        #region "SearchSegment"
        public SearchResults SearchSegment(SearchSettings settings, Segment segment)
        {
            //FOR DEBUg
            //string[] credParams = _provider.GetStoreCridential().Split('\t');
            //if (credParams[0] != _provider.m_username) {}
           

            // segments taggs ar converted to html tags
            string strSourceText = Segment2Html(segment);
            //segmet with translated htmlgs are translated with letsMT api
            string translText = _provider.TranslateText(_languageDirection, strSourceText);

            Segment translation = new Segment(_languageDirection.TargetCulture);
            //trasnlated text with html is converted to trados text segment with trados tags 
            translation=TranslatedHtml2Segment(segment, translText);
            //result is stored as SearchResults
            SearchResults results = new SearchResults();
            results.SourceSegment = segment.Duplicate();
            results.Add(CreateSearchResult(segment, translation, _provider.m_resultScore));

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
        private SearchResult CreateSearchResult(Segment searchSegment, Segment translation, int score)
        {                
                TranslationUnit tu = new TranslationUnit();
                tu.SourceSegment = searchSegment.Duplicate();
                tu.TargetSegment = translation;
               
                tu.ResourceId = new PersistentObjectToken(tu.GetHashCode(), Guid.Empty);
                tu.Origin = TranslationUnitOrigin.MachineTranslation;
                //tu.SystemFields.CreationDate = DateTime.UtcNow;
                //tu.SystemFields.ChangeDate = tu.SystemFields.CreationDate;

                SearchResult searchResult = new SearchResult(tu);
                searchResult.ScoringResult = new ScoringResult();
                searchResult.TranslationProposal = tu.Duplicate();
                searchResult.ScoringResult.BaseScore = score;

                
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

            bool tryAgain = true;
            //MessageBox.Show("Waiting for system to start.");

            try
            {
                _provider.TranslateText(_languageDirection, "test");
                tryAgain = false;
            }
            catch { 
                //System.Threading.Thread.Sleep(15000); 
            }

            while (tryAgain)
            {
                try
                {
                    _provider.TranslateText(_languageDirection, "test");
                    tryAgain = false;
                }
                catch (Exception ex)
                {

                    tryAgain = false;
                    if (ex.Message.Contains("system is starting"))
                    {
                        DialogResult Result = MessageBox.Show("Automated system is starting up. keep waiting?", "System starting", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (Result == DialogResult.Yes)
                        {
                            tryAgain = true;
                            //MessageBox.Show("Waiting for system to start.(segments)");

                            System.Threading.Thread.Sleep(15000);
                        }
                        else
                        {
                            tryAgain = false;
                        }
                    }

                }
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

            bool tryAgain = true;
            //MessageBox.Show("Waiting for system to start.");

            try
            {
                _provider.TranslateText(_languageDirection, "test");
                tryAgain = false;
            }
            catch
            {
                System.Threading.Thread.Sleep(15000); 
            }

            while (tryAgain)
            {
                try
                {
                    _provider.TranslateText(_languageDirection, "test");
                    tryAgain = false;
                }
                catch (Exception ex)
                {

                    tryAgain = false;
                    if (ex.Message.Contains("system is starting"))
                    {
                        DialogResult Result = MessageBox.Show("Automated system is starting up. keep waiting?", "System starting", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (Result == DialogResult.Yes)
                        {
                            tryAgain = true;
                            //MessageBox.Show("Waiting for system to start.(segments)");

                            System.Threading.Thread.Sleep(15000);
                        }
                        else
                        {
                            tryAgain = false;
                        }
                    }

                }
            }

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

    class ReplacementFragment
    {
        private int m_nPosition;
        private int m_nLength;
        private string m_strTagName;
        private TagTypeEnum m_ttType;
        private string m_strOriginal;
        private string m_strReplacement;

        public ReplacementFragment(int nPosition, int nLength, string strTagName, TagTypeEnum ttType, string strOriginal, string strReplacement)
        {
            m_nPosition = nPosition;
            m_nLength = nLength;
            m_strTagName = strTagName;
            m_ttType = ttType;
            m_strOriginal = strOriginal;
            m_strReplacement = strReplacement;
        }

        public int GetOffset() { return m_nPosition; }
        public int GetLength() { return m_nLength; }
        public string GetTagName() { return m_strTagName; }
        public TagTypeEnum GetTagType() { return m_ttType; }
        public string GetOriginal() { return m_strOriginal; }
        public string GetReplacement() { return m_strReplacement; }
    }




    //class ListTranslationProviderElementVisitor : ISegmentElementVisitor
    //{
    //    //private LetsMTTranslationProviderOptions _options;
    //    private string _plainText;

    //    public string PlainText
    //    {
    //        get
    //        {
    //            if (_plainText == null)
    //            {
    //                _plainText = "";
    //            }
    //            return _plainText;
    //        }
    //        set
    //        {
    //            _plainText = value;
    //        }
    //    }

    //    public void Reset()
    //    {
    //        _plainText = "";
    //    }

    //    public ListTranslationProviderElementVisitor(/*LetsMTTranslationProviderOptions options*/)
    //    {
    //        //_options = options;
    //    }

    //    #region ISegmentElementVisitor Members

    //    public void VisitDateTimeToken(Sdl.LanguagePlatform.Core.Tokenization.DateTimeToken token)
    //    {
    //        _plainText += token.Text;
    //    }

    //    public void VisitMeasureToken(Sdl.LanguagePlatform.Core.Tokenization.MeasureToken token)
    //    {
    //        _plainText += token.Text;
    //    }

    //    public void VisitNumberToken(Sdl.LanguagePlatform.Core.Tokenization.NumberToken token)
    //    {
    //        _plainText += token.Text;
    //    }

    //    public void VisitSimpleToken(Sdl.LanguagePlatform.Core.Tokenization.SimpleToken token)
    //    {
    //        _plainText += token.Text;
    //    }

    //    public void VisitTag(Tag tag)
    //    {
    //        _plainText += tag.TextEquivalent;
    //    }

    //    public void VisitTagToken(Sdl.LanguagePlatform.Core.Tokenization.TagToken token)
    //    {
    //        _plainText += token.Text;
    //    }

    //    public void VisitText(Text text)
    //    {
    //        _plainText += text;
    //    }

    //    #endregion
    //}
}
