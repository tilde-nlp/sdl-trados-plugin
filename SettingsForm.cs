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

namespace LetsMT.MTProvider
{

  

    public partial class SettingsForm : Form
    {
        private LetsMTTranslationProvider m_translationProvider;
        private Dictionary<string, string> m_checkedState;
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

            wndTranslationDirections.DisplayMember = "text";
            wndTranslationDirections.ValueMember = "value";

            wndProfileProperties.DisplayMember = "text";
            wndProfileProperties.ValueMember = "value";

            sourceSelectComboBox.DisplayMember = "Text";
            sourceSelectComboBox.ValueMember = "Value";

            targetSelectComboBox.DisplayMember = "Text";
            targetSelectComboBox.ValueMember = "Value";


            //fill the system list
            m_pairs = languagePairs;
            m_checkedState = new Dictionary<string, string>();

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
            wndTranslationDirections.Items.Clear();
            wndProfileProperties.Items.Clear();
            sourceSelectComboBox.Items.Clear();
            sourceSelectComboBox.Text = "";
            targetSelectComboBox.Items.Clear();
            targetSelectComboBox.Text = "";


            List<CMtProfile> profiles = m_translationProvider.m_profileCollection.GetProfileList(wndRunningSystems.Checked);
            List<ListItem> profileListItems = m_translationProvider.m_profileCollection.GetProfileListItems(wndRunningSystems.Checked);

            foreach (ListItem item in profileListItems)
            {
                wndTranslationDirections.Items.Add(item);
            }
            
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

            //Select the profile if only one given (settings clicked on specific profile)
            bool bHasSelection = false;

            if (m_pairs.Length == 1)
            {
                string singleLang = string.Format("{0} - {1}", m_pairs[0].SourceCulture.TwoLetterISOLanguageName, m_pairs[0].TargetCulture.TwoLetterISOLanguageName);
                int index = -1;
                foreach (object addedProfile in wndTranslationDirections.Items)
                {
                    index++;
                    ListItem currentItem = addedProfile as ListItem;
                    if (currentItem.Value == singleLang)
                    {
                        wndTranslationDirections.SetSelected(index, true);
                        bHasSelection = true;
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

            //Select the first profile from global settings
            if (!bHasSelection)
            {
                if (wndTranslationDirections.Items.Count > 0)
                    wndTranslationDirections.SetSelected(0, true);
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

        //Fill the translation engines box with all available systems for the language pair
        private void wndTranslationDirections_SelectedValueChanged(object sender, EventArgs e)
        {
            wndProfileProperties.Items.Clear();

            if (wndTranslationDirections.SelectedIndex != -1)
            {
                ListItem item = wndTranslationDirections.SelectedItem as ListItem;

                string valSelected = item.Value.ToString();

                List<ListItem> systems = m_translationProvider.m_profileCollection.GetProfileSystemList(valSelected, wndRunningSystems.Checked);

                foreach (ListItem system in systems)
                {
                    wndProfileProperties.Items.Add(system);
                }

                string defaultSystem = m_translationProvider.m_profileCollection.GetActiveSystemForProfile(valSelected);

                if(m_checkedState.ContainsKey(valSelected))
                {
                    defaultSystem = m_checkedState[valSelected];
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

        //On selected system, fill the textbox with systems description etc...
        private void wndProfileProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wndProfileProperties.SelectedIndex != -1)
            {
                ListItem item = wndProfileProperties.SelectedItem as ListItem;

                string valSelected = item.Value.ToString();

                wndDescription.Lines = m_translationProvider.m_profileCollection.GetSystemById(valSelected).GetDescription().Split('\n');
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

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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
            ListItem sourceLanguageItem = sourceSelectComboBox.SelectedItem as ListItem;
            ListItem targetLanguageItem = targetSelectComboBox.SelectedItem as ListItem;
            if (sourceLanguageItem == null || targetLanguageItem == null)
            {
                return;
            }
            string sourceLanguageId = sourceLanguageItem.Value;
            string targetLanguageId = targetLanguageItem.Value;

            string progileId = CMtProfile.GenerateProfileId(sourceLanguageId, targetLanguageId);

            ListItem item = wndProfileProperties.Items[idx] as ListItem;

            string strNewValue = (e.NewValue == CheckState.Checked)?item.Value:"";

            if (m_checkedState.ContainsKey(progileId))
                m_checkedState[progileId] = strNewValue;
            else
                m_checkedState.Add(progileId, strNewValue);            
        }

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
            string password = "";
                
				username = group + "\\" + m_username;
                password = m_translationProvider.m_service.ClientCredentials.UserName.Password;

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


                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
				
                m_translationProvider.m_service = new LetsMTAPI.TranslationServiceContractClient(binding, endpoint);

                m_translationProvider.m_service.ClientCredentials.UserName.UserName = username;
                m_translationProvider.m_service.ClientCredentials.UserName.Password = password;
                m_translationProvider.m_profileCollection = null;
                m_translationProvider.DownloadProfileList(true);
                m_checkedState = new Dictionary<string, string>();

            //filee the new system list    
            FillProfileList();
                
            //save the cerdentials
            TranslationProviderCredential tc = new TranslationProviderCredential(string.Format("{0}\t{1}\t{2}", username, password, m_translationProvider.m_strAppID), true);
            m_credentialStore.AddCredential(m_translationProvider.Uri, tc);


        }

        private void sourceSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillTargetLanguageList();
        }

        private void targetSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            wndProfileProperties.Items.Clear();

            if (sourceSelectComboBox.SelectedIndex != -1 && targetSelectComboBox.SelectedIndex != -1)
            {
                ListItem sourceLanguageItem = sourceSelectComboBox.SelectedItem as ListItem;
                ListItem targetLanguageItem = targetSelectComboBox.SelectedItem as ListItem;
                if (sourceLanguageItem == null || targetLanguageItem == null)
                {
                    return;
                }

                string sourceLanguageId = sourceLanguageItem.Value;
                string targetLanguageId = targetLanguageItem.Value;

                string profileId = CMtProfile.GenerateProfileId(sourceLanguageId, targetLanguageId);

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
