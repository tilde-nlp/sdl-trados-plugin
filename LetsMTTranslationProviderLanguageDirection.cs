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
            string strSourceText = "";
            foreach (var element in segment.Elements)
            {
                //MessageBox.Show(element.GetType().ToString());
                strSourceText += element.ToString();
            }

            SearchResults results = new SearchResults();
            results.SourceSegment = segment.Duplicate();

            Segment translation = new Segment(_languageDirection.TargetCulture);

            string input = strSourceText;





            List<ReplacementFragment> rplist = new List<ReplacementFragment>();

            //Regex tagregex = new Regex("</?\\w+((\\s+[-\\w]+(\\s*=\\s*(?:\".*?\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)/?>");
            Regex tagregex = new Regex("<(?<closing>/)?(?<tagname>\\w+)((\\s+(?<attrname>[-\\w]+)(\\s*=\\s*(?<attrval>\".*?\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)(?<selfclosing>/)?>");

            Match m = tagregex.Match(input);

            while (m.Success)
            {
                TagTypeEnum curTagType = TagTypeEnum.Opening;

                string strTagName = "";

                if (m.Groups["closing"].Captures.Count == 1) { curTagType = TagTypeEnum.Closing; }
                else if (m.Groups["selfclosing"].Captures.Count == 1) { curTagType = TagTypeEnum.SelfClosing; }

                if (m.Groups["tagname"].Captures.Count == 1)
                {
                    strTagName = m.Groups["tagname"].Captures[0].Value;

                    string replacement = string.Format("<{0}span{1}{2}>",
                                                       ((curTagType == TagTypeEnum.Closing) ? "/" : ""),
                                                       ((curTagType != TagTypeEnum.Closing) ? " id=\"" + strTagName + "\"" : ""),
                                                       ((curTagType == TagTypeEnum.SelfClosing) ? " /" : ""));

                    rplist.Add(new ReplacementFragment(m.Index, m.Length, strTagName, curTagType, m.Value, replacement));
                }

                m = m.NextMatch();
            }

            rplist.Sort((t1, t2) => t1.GetOffset() - t2.GetOffset());

            StringBuilder output = new StringBuilder(input.Length);

            int currentReplacement = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (rplist.Count > currentReplacement && rplist[currentReplacement].GetOffset() == i)
                {
                    output.Append(rplist[currentReplacement].GetReplacement());
                    i += rplist[currentReplacement].GetLength() - 1;
                    currentReplacement++;
                    continue;
                }
                else
                {
                    output.Append(input[i]);
                }
            }

            strSourceText = output.ToString();




            string translText = _provider.TranslateText(_languageDirection, strSourceText);




            input = translText;

            Stack<string> openTags = new Stack<string>();

            List<ReplacementFragment> rplistout = new List<ReplacementFragment>();

            m = tagregex.Match(input);

            while (m.Success)
            {
                TagTypeEnum curTagType = TagTypeEnum.Opening;

                if (m.Groups["closing"].Captures.Count == 1) { curTagType = TagTypeEnum.Closing; }
                else if (m.Groups["selfclosing"].Captures.Count == 1) { curTagType = TagTypeEnum.SelfClosing; }

                if (curTagType == TagTypeEnum.SelfClosing || curTagType == TagTypeEnum.Opening)
                {
                    string strTagId = "";

                    if (m.Groups["attrname"].Captures.Count == 1 && m.Groups["attrval"].Captures.Count == 1)
                    {
                        strTagId = m.Groups["attrval"].Captures[0].Value.Trim("\"".ToCharArray());
                    }

                    if (curTagType == TagTypeEnum.Opening)
                    {
                        openTags.Push(strTagId);
                    }

                    string replacement = "";
                    for (int i = 0; i < rplist.Count; i++)
                    {
                        if (rplist[i].GetTagName() == strTagId && rplist[i].GetTagType() == curTagType)
                        {
                            replacement = rplist[i].GetOriginal();
                            break;
                        }
                    }

                    rplistout.Add(new ReplacementFragment(m.Index, m.Length, strTagId, curTagType, m.Value, replacement));
                }
                else if (curTagType == TagTypeEnum.Closing)
                {
                    string replacement = string.Format("</{0}>", openTags.Pop());
                    rplistout.Add(new ReplacementFragment(m.Index, m.Length, "span", curTagType, m.Value, replacement));
                }

                m = m.NextMatch();
            }

            rplistout.Sort((t1, t2) => t1.GetOffset() - t2.GetOffset());

            StringBuilder outputtr = new StringBuilder(input.Length);

            /*int*/
            currentReplacement = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (rplistout.Count > currentReplacement && rplistout[currentReplacement].GetOffset() == i)
                {
                    outputtr.Append(rplistout[currentReplacement].GetReplacement());
                    i += rplistout[currentReplacement].GetLength() - 1;
                    currentReplacement++;
                    continue;
                }
                else
                {
                    outputtr.Append(input[i]);
                }
            }

            translText = outputtr.ToString();





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
