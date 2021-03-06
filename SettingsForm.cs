﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Sdl.LanguagePlatform.Core;
using System.Xml;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using System.ServiceModel;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.ServiceModel.Description;
using System.Diagnostics;

namespace LetsMT.MTProvider
{

  

    public partial class SettingsForm : Form
    {
        private LetsMTTranslationProvider m_translationProvider;
        private Dictionary<string, string> m_checkedState;
        /// <summary>
        /// Stores a term corpora id for a given profileId and systemId pair
        /// </summary>
        private Dictionary<MyTuple<string, string>, string> m_checkedTerms;
        private LanguagePair[] m_pairs;
        private int m_score;
        //used in group change function
        private string m_activeGroup;
        private string m_username;
        private bool m_trackGoupChange;
        public bool TranslationProviderInitialized { get; private set; }

        /// <summary>
        /// Indicates whether qe checkbox should be checked for systems
        /// that support the feature. To ilustrate -
        /// 1) qe is available for a system;
        /// 2) user checks qe, qeWasCheckedWhenAvailable is set to true;
        /// 3) user switches to another system for which qe is not available, qe gets un-checked;
        /// 4) user switches to another system for which qe is available;
        /// 5) qe gets checked because qeWasCheckedWhenAvailable is true;
        /// 6) if user un-checks qe, qeWasCheckedWhenAvailable gets set to false.
        ///
        /// All this is done because the qe setting is global (instead of per-system), but
        /// we still wan't to enable some persistence for the setting when switching among systems.
        /// </summary>
        private bool qeWasCheckedWhenAvailable;
        /// <summary>
        /// While dealing with <see cref="qeWasCheckedWhenAvailable"/> allows to distunguish when the qe checkbox was checked by the user.
        /// </summary>
        private bool qeCheckedProgrammatically;


        /// <summary>
        /// Dictionary with source language code as a key and a tuple of source language name and a list of target language code and name tuples as value.
        /// </summary>
        private Dictionary<string, MyTuple<string, List<MyTuple<string, string>>>> languageChoices = new Dictionary<string, MyTuple<string, List<MyTuple<string, string>>>>();


        ITranslationProviderCredentialStore m_credentialStore;

        public class UserGroup
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public override string ToString() { return this.Name; }
        }


        public SettingsForm(ref LetsMTTranslationProvider editProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            // we allow to skip the check if credential store == null. there is code other than LetsMTTranslationProviderWinFormsUI.Edit() method that calls SettingsForm
            // and it does not have easy access to the credentialStore. the alternative would be to save credentialStore somewhare and make it available ouside the method
            if (credentialStore != null && !CheckTranslationProviderCredentials(editProvider, credentialStore))
            {
                return;
            }

            TranslationProviderInitialized = true;

            m_score = editProvider.m_resultScore;
            m_credentialStore = credentialStore;
            DialogResult = DialogResult.Cancel;
            m_trackGoupChange = false;
            

            Text = PluginResources.Plugin_NiceName + " Settings";

            InitializeComponent();

            m_translationProvider = editProvider;

            LetsMTAPI.UserData userData = m_translationProvider.m_service.GetUserInfo("");

            // get teh username whitout group
            string username = m_translationProvider.m_username;
            string userEmail = userData.email;
            if (!string.IsNullOrEmpty(userEmail))
            {
                username = userEmail;
            }

            m_username = username;



            // get teh username whitout group
            string activeGroup = "";
            string userDataActiveGroup = userData.activeGroup;
            if (userDataActiveGroup != null)
            {
                activeGroup = userDataActiveGroup;
            }

            m_activeGroup = activeGroup;

            // since we use UserIds for logging in we should have had received only one group
            this.GroupLabel.Text = "Group: " + userData.userGroups[0].name.Trim();

            wndProfileProperties.DisplayMember = "text";
            wndProfileProperties.ValueMember = "value";

            sourceSelectComboBox.DisplayMember = "Text";
            sourceSelectComboBox.ValueMember = "Value";

            targetSelectComboBox.DisplayMember = "Text";
            targetSelectComboBox.ValueMember = "Value";

            termCorporaSelectComboBox.DisplayMember = "Text";
            termCorporaSelectComboBox.ValueMember = "Value";

            qualityEstimateTextBox.Text = editProvider.m_minAllowedQualityEstimateScore.ToString();
            qualityEstimateCheckBox.Checked = editProvider.m_useQualityEstimates;
            qualityEstimateTextBox.Enabled = editProvider.m_useQualityEstimates;
            qeWasCheckedWhenAvailable = editProvider.m_useQualityEstimates;


            //fill the system list
            m_pairs = languagePairs;
            m_checkedState = new Dictionary<string, string>();
            m_checkedTerms = new Dictionary<MyTuple<string, string>, string>();

            FillProfileList();

            m_trackGoupChange = true;
        }

        /// <summary>
        /// Checks whether the given translation provider has a working connection to the translation API.
        /// If not then display the password form and re-initialized the translation provider.
        /// </summary>
        /// <param name="editProvider">The translation provider to be checked.</param>
        /// <param name="credentialStore">The credential store that should contain</param>
        /// <returns>Returns true if the credential store already has the respective credentials or if the translation provider is successfully re-initialized. Returns false otherwise.</returns>
        private bool CheckTranslationProviderCredentials(LetsMTTranslationProvider editProvider, ITranslationProviderCredentialStore credentialStore)
        {
            if (credentialStore.GetCredential(editProvider.Uri) != null)
            {
                return true;
            }

            PasswordForm pf = new PasswordForm();

            while (pf.ShowDialog(this) == DialogResult.OK)
            {

                //TODO: check how to minimize the amount odfsystem list calls
                string credentials = string.Format("{0}\t{1}", pf.strToken, pf.strAppId);
                TranslationProviderCredential tc = new TranslationProviderCredential(credentials, pf.bRemember);
                credentialStore.AddCredential(editProvider.Uri, tc);

                // Create a new connection to the translation API
                editProvider.InitService(pf.strToken);

                if (!LetsMTTranslationProviderWinFormsUI.ValidateTranslationProviderLocaly(editProvider))
                {
                    //IF USERNAME INFOREC REMOVE DATA FROM STORE
                    credentialStore.RemoveCredential(editProvider.Uri);
                }
                else
                {
                    editProvider.DownloadProfileList(true);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Parses profile list in a dictionary with target language list for a given source language.
        /// </summary>
        /// <param name="profiles"> List of language profles. </param>
        /// <returns> A dictionary with source language code as a key and a tuple of source language name and a list of target language code and name tuples as value. </returns>
        private Dictionary<string, MyTuple<string, List<MyTuple<string, string>>>> ParseLanguageChoices(List<CMtProfile> profiles)
        {
            Dictionary<string, MyTuple<string, List<MyTuple<string, string>>>> languageChoices = new Dictionary<string, MyTuple<string, List<MyTuple<string, string>>>>();

            foreach (CMtProfile profile in profiles)
            {
                MyTuple<string, List<MyTuple<string, string>>> sourceAndTargetLenguageInfo;
                if (!languageChoices.TryGetValue(profile.SourceLanguageId, out sourceAndTargetLenguageInfo))
                {
                    sourceAndTargetLenguageInfo = MyTuple.Create(profile.SourceLanguageName, new List<MyTuple<string, string>>());
                    languageChoices.Add(profile.SourceLanguageId, sourceAndTargetLenguageInfo);
                }
                List<MyTuple<string, string>> targetLanguageCollection = sourceAndTargetLenguageInfo.Item2;
                targetLanguageCollection.Add(MyTuple.Create(profile.TargetLanguageId, profile.TargetLanguageName));
            }

            return languageChoices;
        }

        private void FillProfileList()
        {
            wndProfileProperties.Items.Clear();
            sourceSelectComboBox.Items.Clear();
            sourceSelectComboBox.Text = "";
            targetSelectComboBox.Items.Clear();
            targetSelectComboBox.Text = "";


            List<CMtProfile> profiles = m_translationProvider.m_profileCollection.GetProfileList(wndRunningSystems.Checked);
                        
            //languageChoices.Clear(); // TODO: should we dispose of the old values or let the garbage collector do it's work? 
            languageChoices = ParseLanguageChoices(profiles);
            List<string> sortedKeys = new List<string>(languageChoices.Keys);
            sortedKeys.Sort((x, y) =>
            {
                int firstOrder = x.CompareTo(y);
                return firstOrder == 0 ? languageChoices[x].Item1.CompareTo(languageChoices[y].Item1) : firstOrder;
            });

            foreach (string sourceLanguageId in sortedKeys)
            {
                MyTuple<string, List<MyTuple<string, string>>> sourceAndTargetLenguageInfo = languageChoices[sourceLanguageId];
                string sourceLanguageName = sourceAndTargetLenguageInfo.Item1;
                sourceSelectComboBox.Items.Add(new ListItem { Text=sourceLanguageName, Value=sourceLanguageId});
            }

            //Select the profile if only one given (settings clicked on specific profile)
            bool sourceHasSelection = false;
            bool targetHasSelection = false;

            if (m_pairs != null && m_pairs.Length == 1)
            {
                string selectedSourceLanguageId = m_pairs[0].SourceCulture.TwoLetterISOLanguageName;
                string selectedTargetLanguageId = m_pairs[0].TargetCulture.TwoLetterISOLanguageName;
                int sourceIndex = -1;
                int targetIndex = -1;
                foreach (object addedSourceObject in sourceSelectComboBox.Items)
                {
                    sourceIndex++;
                    ListItem addedSourceItem = addedSourceObject as ListItem;
                    if (addedSourceItem.Value == selectedSourceLanguageId)
                    {
                        sourceSelectComboBox.SelectedIndex = sourceIndex;
                        // this is when selectionChanged callback is invoked and the target language combobox is filled.
                        // we can proceed to select the target language now

                        sourceHasSelection = true;
                        break;
                    }
                }

                foreach (object addedTargetObject in targetSelectComboBox.Items)
                {
                    targetIndex++;
                    ListItem addedTargetItem = addedTargetObject as ListItem;
                    if (addedTargetItem.Value == selectedTargetLanguageId)
                    {
                        targetSelectComboBox.SelectedIndex = targetIndex;
                        targetHasSelection = true;
                        break;
                    }
                }
            }


            if (!sourceHasSelection && sourceSelectComboBox.Items.Count > 0)
            {
                sourceSelectComboBox.SelectedIndex = 0;
            }

            if (!targetHasSelection && targetSelectComboBox.Items.Count > 0)
            {
                targetSelectComboBox.SelectedIndex = 0;
            }
        }

        public void FillTargetLanguageList()
        {
            targetSelectComboBox.Items.Clear();
            targetSelectComboBox.Text = "";
            
            ListItem selectedItem = sourceSelectComboBox.SelectedItem as ListItem;
            if (selectedItem != null)
            {
                string sourceLanguageId = selectedItem.Value;
                if (sourceLanguageId != null)
                {
                    MyTuple<string, List<MyTuple<string, string>>> sourceAndTargetLenguageInfo = languageChoices[sourceLanguageId];

                    List<MyTuple<string, string>> targetLanguageList = sourceAndTargetLenguageInfo.Item2;
                    targetLanguageList.Sort((x, y) =>
                    {
                        int firstOrder = x.Item1.CompareTo(y.Item1);
                        return firstOrder == 0 ? x.Item2.CompareTo(y.Item2) : firstOrder;
                    });

                    foreach (MyTuple<string, string> targetLanguageInfo in targetLanguageList)
                    {
                        targetSelectComboBox.Items.Add(new ListItem { Text = targetLanguageInfo.Item2, Value = targetLanguageInfo.Item1 });
                    }
                }
            }

            if (targetSelectComboBox.Items.Count > 0)
            {
                targetSelectComboBox.SelectedIndex = 0;
            }
        }

        private string getCheckedOrDefaultTermCorporaId(string profileId, string systemId)
        {

            string previousSessionDefaultTermCorporaId = m_translationProvider.m_profileCollection.GetActiveTermCorporaForSystem(profileId, systemId);
            string defaultTermCorporaId = m_checkedTerms.GetValueOrDefault(MyTuple.Create(profileId, systemId), previousSessionDefaultTermCorporaId);

            return defaultTermCorporaId;
        }

        private void FillTermCorporaList(string profileId, string systemId, LetsMTAPI.TermCorpus[] termCorpora)
        {
            // Clear the comboboxes again. The onclick event that this method is attached to (which also clears the comboboxes) can get fired
            // two succesive times with no interwineing calls to this method resulting in this method being called twice in a row afterwards.
            termCorporaSelectComboBox.Items.Clear();
            termCorporaSelectComboBox.Text = "";

            termCorporaSelectComboBox.Items.Add(new ListItem { Text = "", Value = "" });  // added to allow to not select a term corpora

            if (termCorpora.Length == 0)
            {
                termCorporaSelectComboBox.Enabled = false;
            }
            else
            {
                termCorporaSelectComboBox.Enabled = true;
                int toSelectIndex = 0;

                string toSelectTermCorporaId = getCheckedOrDefaultTermCorporaId(profileId, systemId);

                foreach (LetsMTAPI.TermCorpus corpus in termCorpora)
                {
                    if (corpus.Status == "Ready" || corpus.Status == "Processing")
                    {
                        termCorporaSelectComboBox.Items.Add(new ListItem { Text = string.Format("{0} ({1})", corpus.Title, corpus.Status), Value = corpus.CorpusId });
                        if (corpus.CorpusId == toSelectTermCorporaId)
                        {
                            toSelectIndex = termCorporaSelectComboBox.Items.Count - 1;
                        }
                    }
                }

                // if the previously saved term id isn't available now, forget it
                if (toSelectIndex == 0 && !string.IsNullOrEmpty(toSelectTermCorporaId))
                {
                    MyTuple<string, string> profileAndSystemId = MyTuple.Create(profileId, systemId);
                    if (m_checkedTerms.ContainsKey(profileAndSystemId))
                    {
                        m_checkedTerms.Remove(profileAndSystemId);
                    }

                    m_translationProvider.m_profileCollection.SetActiveTermCorporaForSystem(profileId, systemId, "");
                }


                termCorporaSelectComboBox.SelectedIndex = toSelectIndex;

            }
        }


        //On selected system, fill the textbox with systems description etc...
        private void wndProfileProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wndProfileProperties.SelectedIndex != -1)
            {
                ListItem item = wndProfileProperties.SelectedItem as ListItem;

                string systemId = item.Value.ToString();
                string profileId = getSelectedProfileId();

                CMtSystem selectedSystem = m_translationProvider.m_profileCollection.GetSystemById(systemId);
                bool qeAvailable = selectedSystem.GetQeAvailability();
                qualityEstimateCheckBox.Enabled = qeAvailable;
                qualityEstimateTextBox.Enabled = qeAvailable;
                qeCheckedProgrammatically = true;
                qualityEstimateCheckBox.Checked = qeAvailable && qeWasCheckedWhenAvailable;
                qeCheckedProgrammatically = false;

                termCorporaSelectComboBox.Items.Clear();
                termCorporaSelectComboBox.Text = "";

                Func<LetsMTAPI.TermCorpus[]> getTerms = () => m_translationProvider.m_service.GetSystemTermCorpora(systemId);
                getTerms.BeginInvoke(x =>
                { 
                    AsyncResult result = x as AsyncResult;
                    LetsMTAPI.TermCorpus[] termCorpora;
                    try
                    {
                        termCorpora = getTerms.EndInvoke(result);
                    }
                    catch { return; }

                    // This callback is executed on a ThreadPool thraed. Filling the term corpora list must be done on the UI thread.
                    this.BeginInvoke(new Action(() =>
                        {
                            FillTermCorporaList(profileId, systemId, termCorpora);
                        }));
                }, null);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            Dictionary<string, string>.Enumerator changes =  m_checkedState.GetEnumerator();

            while (changes.MoveNext())
            {
                m_translationProvider.m_profileCollection.SetActiveSystemForProfile(changes.Current.Key, changes.Current.Value);
            }

            Dictionary<MyTuple<string, string>, string>.Enumerator defaultTermSelectionChages = m_checkedTerms.GetEnumerator();
            while (defaultTermSelectionChages.MoveNext())
            {
                m_translationProvider.m_profileCollection.SetActiveTermCorporaForSystem(defaultTermSelectionChages.Current.Key.Item1,
                    defaultTermSelectionChages.Current.Key.Item2,
                    defaultTermSelectionChages.Current.Value);
            }

            m_translationProvider.m_minAllowedQualityEstimateScore = double.Parse(qualityEstimateTextBox.Text);
            m_translationProvider.m_useQualityEstimates = qualityEstimateCheckBox.Checked;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private string getSelectedProfileId()
        {
            ListItem sourceLanguageItem = sourceSelectComboBox.SelectedItem as ListItem;
            ListItem targetLanguageItem = targetSelectComboBox.SelectedItem as ListItem;
            if (sourceLanguageItem == null || targetLanguageItem == null)
            {
                return null;
            }
            string sourceLanguageId = sourceLanguageItem.Value;
            string targetLanguageId = targetLanguageItem.Value;

            return CMtProfile.GenerateProfileId(sourceLanguageId, targetLanguageId);
        }

        private string getSelectedTermCorporaId()
        {
            throw new NotImplementedException();
        }

        private void wndProfileProperties_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int idx = e.Index;
            btnOk.Enabled = true;

            for (int i = 0; i < wndProfileProperties.Items.Count; i++)
            {
                if(i != idx)
                    wndProfileProperties.SetItemCheckState(i, CheckState.Unchecked);
            }            

            //ListItem profile = wndTranslationDirections.SelectedItem as ListItem;
            string profileId = getSelectedProfileId();
            if (profileId == null)
            {
                return;
            }

            ListItem item = wndProfileProperties.Items[idx] as ListItem;

            string strNewValue = (e.NewValue == CheckState.Checked) ? item.Value : "";

            if (m_checkedState.ContainsKey(profileId))
                m_checkedState[profileId] = strNewValue;
            else
                m_checkedState.Add(profileId, strNewValue);            
        }

        // occurs when "Show only running systems" checkbox is checked
        private void wndRunningSystems_CheckedChanged(object sender, EventArgs e)
        {
            FillProfileList();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Advanced_options advanced = new Advanced_options(ref m_translationProvider);
            advanced.ShowDialog(this);
        }

        private void sourceSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillTargetLanguageList();
        }

        private void targetSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            termCorporaSelectComboBox.Items.Clear();
            termCorporaSelectComboBox.Text = "";
            wndProfileProperties.Items.Clear();

            if (sourceSelectComboBox.SelectedIndex != -1 && targetSelectComboBox.SelectedIndex != -1)
            {
                string profileId = getSelectedProfileId();
                if(profileId == null)
                {
                    return;
                }

                List<ListItem> systems = m_translationProvider.m_profileCollection.GetProfileSystemList(profileId, wndRunningSystems.Checked);

                foreach (ListItem system in systems)
                {
                    wndProfileProperties.Items.Add(system);
                }

                string defaultSystem = m_translationProvider.m_profileCollection.GetActiveSystemForProfile(profileId);

                if (m_checkedState.ContainsKey(profileId))
                {
                    defaultSystem = m_checkedState[profileId];
                }

                //Select default system
                bool bHasSelection = false;

                for (int i = 0; i < wndProfileProperties.Items.Count; i++)
                {
                    if ((wndProfileProperties.Items[i] as ListItem).Value == defaultSystem)
                    {
                        wndProfileProperties.SetSelected(i, true);
                        wndProfileProperties.SetItemCheckState(i, CheckState.Checked);
                        bHasSelection = true;
                        break;
                    }
                }

                //Select first one
                if (!bHasSelection && wndProfileProperties.Items.Count > 0)
                    wndProfileProperties.SetSelected(0, true);
            }
        }

        private void GetSystemTermCorpora()
        {
            LetsMTAPI.TermCorpus[] terms = m_translationProvider.m_service.GetSystemTermCorpora("");
        }

        private void termCorporaSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wndProfileProperties.CheckedItems.Count == 0 || termCorporaSelectComboBox.SelectedItem == null)
            {
                return;
            }
            ListItem selectedSystemItem = wndProfileProperties.CheckedItems[0] as ListItem;
            string systemId = selectedSystemItem.Value;
            string profileId = getSelectedProfileId();

            ListItem selectedTermItem = termCorporaSelectComboBox.SelectedItem as ListItem;

            string checkedOrDefaultTermCorporaId = getCheckedOrDefaultTermCorporaId(profileId, systemId);

            if (!string.IsNullOrEmpty(selectedTermItem.Value) || !string.IsNullOrEmpty(checkedOrDefaultTermCorporaId))  // allow also to reset chosen term corpora to none if empty string received
            {
                m_checkedTerms[MyTuple.Create(profileId, systemId)] = selectedTermItem.Value;
            }
        }

        private void logOutLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            m_credentialStore.RemoveCredential(m_translationProvider.Uri);
            Close();
        }

        private void qualityEstimateTextBox_Validating(object sender, CancelEventArgs e)
        {
            double qeScore;
            string value = qualityEstimateTextBox.Text;
            //Try parsing in the current culture
            if (double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out qeScore) ||
                //Then try in US english
                double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out qeScore) ||
                //Then in neutral language
                double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out qeScore))
            {
                if (qeScore < 0)
                {
                    qualityEstimateTextBox.Text = "0";
                }
                else if(qeScore > 1)
                {
                    qualityEstimateTextBox.Text = "1";
                }
            }
            else
            {
                qualityEstimateTextBox.Text = "0";
            }
        }

        private void qualityEstimateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            qualityEstimateTextBox.Enabled = qualityEstimateCheckBox.Checked;
            if (!qeCheckedProgrammatically)
            {
                qeWasCheckedWhenAvailable = qualityEstimateCheckBox.Checked;
            }
        }
    }

    #region "ListItem helper class"
    public class ListItem
    {
        private string _text;
        private string _value;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public ListItem()
        {
        }

        public ListItem(string text, string value)
        {
            _text = text;
            _value = value;
        }
    }
    #endregion
}
