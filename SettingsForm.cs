using System;
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


        // Dictionary with source language code as a key and a tuple of source language name and a list of target language code and name tuples as value.
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
            m_score = editProvider.m_resultScore;
            m_credentialStore = credentialStore;
            DialogResult = DialogResult.Cancel;
            m_trackGoupChange = false;
            

            Text = PluginResources.Plugin_NiceName + " Settings";

            InitializeComponent();

            m_translationProvider = editProvider;
            string WelcomeName = m_translationProvider.m_username;

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

            //Get the user friendly name for welcome lable
            string userDataName = userData.name;
            string userDataSurname = userData.surname;
            if (!string.IsNullOrEmpty(userDataName))
            {
                WelcomeName = userDataName;
            }
            if (!string.IsNullOrEmpty(userDataSurname))
            {
                WelcomeName = WelcomeName + " " + userDataSurname;
            }


            List<UserGroup> GoupList = new List<UserGroup>();

            foreach (LetsMTAPI.UserGroup group in userData.userGroups)
            {
                GoupList.Add(new UserGroup() { Name = group.name, Value = group.id });

            }
            this.GroupSelectComboBox.DataSource = GoupList;
            this.GroupSelectComboBox.DisplayMember = "Name";
            this.GroupSelectComboBox.ValueMember = "Value";
            this.GroupSelectComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            //select teh active group
           
            this.GroupSelectComboBox.SelectedValue = m_activeGroup;
            // int selectIndex = GroupSelectComboBox.Items.IndexOf(m_activeGroup);
            //if (selectIndex != -1)
            //{
            //    this.GroupSelectComboBox.SelectedIndex = selectIndex;
            //}
            

            UsernameLable.Text = "Welcome, " + WelcomeName + "!";

            wndProfileProperties.DisplayMember = "text";
            wndProfileProperties.ValueMember = "value";

            sourceSelectComboBox.DisplayMember = "Text";
            sourceSelectComboBox.ValueMember = "Value";

            targetSelectComboBox.DisplayMember = "Text";
            targetSelectComboBox.ValueMember = "Value";

            termCorporaSelectComboBox.DisplayMember = "Text";
            termCorporaSelectComboBox.ValueMember = "Value";


            //fill the system list
            m_pairs = languagePairs;
            m_checkedState = new Dictionary<string, string>();
            m_checkedTerms = new Dictionary<MyTuple<string, string>, string>();

            FillProfileList();

            m_trackGoupChange = true;
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

            foreach(string sourceLanguageId in languageChoices.Keys)
            {
                MyTuple<string, List<MyTuple<string, string>>> sourceAndTargetLenguageInfo = languageChoices[sourceLanguageId];
                string sourceLanguageName = sourceAndTargetLenguageInfo.Item1;
                sourceSelectComboBox.Items.Add(new ListItem { Text=sourceLanguageName, Value=sourceLanguageId});
            }

            //Select the profile if only one given (settings clicked on specific profile)
            bool sourceHasSelection = false;
            bool targetHasSelection = false;

            if (m_pairs.Length == 1)
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

                wndDescription.Lines = m_translationProvider.m_profileCollection.GetSystemById(systemId).GetDescription().Split('\n');

                termCorporaSelectComboBox.Items.Clear();
                termCorporaSelectComboBox.Text = "";

                Func<LetsMTAPI.TermCorpus[]> getTerms = () => m_translationProvider.m_service.GetSystemTermCorpora(systemId);
                getTerms.BeginInvoke(x =>
                { 
                    AsyncResult result = x as AsyncResult;
                    LetsMTAPI.TermCorpus[] termCorpora = getTerms.EndInvoke(result);

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

            string strNewValue = (e.NewValue == CheckState.Checked)?item.Value:"";

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

        private void GroupSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(m_trackGoupChange)) { return; }
            string group = GroupSelectComboBox.SelectedValue.ToString();

            //TODO implement this
           // MessageBox.Show("not implemented yet!" + group);
             
            string username = "";
            string token = "";
                
				username = group + "\\" + m_username;

                IEndpointBehavior serviceCookieManager = m_translationProvider.m_service.Endpoint.Behaviors[typeof(CookieManagementBehaviour)]; // TODO: check if for some reason empty
                token = (serviceCookieManager as CookieManagementBehaviour).Cookie;

                m_translationProvider.m_username = username;
				
                global::System.Resources.ResourceManager resourceManager = new global::System.Resources.ResourceManager("LetsMT.MTProvider.PluginResources", typeof(PluginResources).Assembly);
                // create Web Service client
                string url = resourceManager.GetString("LetsMTWebServiceUrl");
                
                try
                {
                    Microsoft.Win32.RegistryKey key;
                    key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\\Tilde\\LetsMT");
                    if (key != null)
                    {
                        string RegUrl = key.GetValue("url", "none").ToString();
                        if (RegUrl.Length > 3)
                        {
                            if (RegUrl.Substring(0, 4) == "http") { url = RegUrl; }
                        }
                    }

                }
                catch (Exception) { }

                EndpointAddress endpoint = new EndpointAddress(url);

                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                binding.MaxBufferSize = int.MaxValue;
                binding.MaxReceivedMessageSize = int.MaxValue;


                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
				
                m_translationProvider.m_service = new LetsMTAPI.TranslationServiceContractClient(binding, endpoint);
                m_translationProvider.m_service.Endpoint.Behaviors.Add(serviceCookieManager);

                m_translationProvider.m_profileCollection = null;
                m_translationProvider.DownloadProfileList(true);
                m_checkedState = new Dictionary<string, string>();

            //filee the new system list    
            FillProfileList();
                
            //save the cerdentials
            TranslationProviderCredential tc = new TranslationProviderCredential(string.Format("{0}\t{1}", token, m_translationProvider.m_strAppID), true);
            m_credentialStore.AddCredential(m_translationProvider.Uri, tc);


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
