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
            
            
            XmlNode node = m_translationProvider.m_service.GetUserInfo("");

            // get teh username whitout group
            string username = m_translationProvider.m_username;
            XmlNode UsernameXMl = node.SelectSingleNode("email");
            if (UsernameXMl != null)
            {
                username = UsernameXMl.InnerText;
            }

            m_username = username;




            // get teh username whitout group
            string activeGroup = "";
            XmlNode activeGroupXML = node.SelectSingleNode("activeGroup");
            if (activeGroupXML != null)
            {
                activeGroup = activeGroupXML.InnerText;
            }

            m_activeGroup = activeGroup;

            //Get the user friendly name for welcome lable
            XmlNode NameXML = node.SelectSingleNode("name");
            XmlNode SurnameXML = node.SelectSingleNode("surname");
            if (NameXML != null)
            {
                WelcomeName = NameXML.InnerText;
            }
            if (SurnameXML != null)
            {
                WelcomeName = WelcomeName + " " + SurnameXML.InnerText;
            }


            List<UserGroup> GoupList = new List<UserGroup>();

            foreach (XmlNode n in node.SelectNodes("userGroups/group"))
            {
                GoupList.Add(new UserGroup() { Name = n.Attributes["name"].Value, Value = n.Attributes["id"].Value });

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


            //fill the system list
            m_pairs = languagePairs;
            m_checkedState = new Dictionary<string, string>();

            FillProfileList();

            m_trackGoupChange = true;
            



        }
        

        private void FillProfileList()
        {
            wndTranslationDirections.Items.Clear();
            wndProfileProperties.Items.Clear();

            List<ListItem> profiles = m_translationProvider.m_profileCollection.GetProfileList(wndRunningSystems.Checked);

            foreach (ListItem item in profiles)
            {
                wndTranslationDirections.Items.Add(item);
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

            //Select the first profile from global settings
            if (!bHasSelection)
            {
                if (wndTranslationDirections.Items.Count > 0)
                    wndTranslationDirections.SetSelected(0, true);
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

            ListItem profile = wndTranslationDirections.SelectedItem as ListItem;
            ListItem item = wndProfileProperties.Items[idx] as ListItem;

            string strNewValue = (e.NewValue == CheckState.Checked)?item.Value:"";

            if(m_checkedState.ContainsKey(profile.Value))
                m_checkedState[profile.Value] = strNewValue;
            else
                m_checkedState.Add(profile.Value, strNewValue);            
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
				
                m_translationProvider.m_service = new LocalLetsMTWebService.TranslationWebServiceSoapClient(binding, endpoint);

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
