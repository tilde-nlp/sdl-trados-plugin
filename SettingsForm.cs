using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Sdl.LanguagePlatform.Core;

namespace LetsMT.MTProvider
{
    public partial class SettingsForm : Form
    {
        private LetsMTTranslationProvider m_translationProvider;
        private Dictionary<string, string> m_checkedState;
        private LanguagePair[] m_pairs;

        public SettingsForm(ref LetsMTTranslationProvider editProvider, LanguagePair[] languagePairs)
        {
            DialogResult = DialogResult.Cancel;

            InitializeComponent();

            Text = PluginResources.Plugin_NiceName + " Settings";

            wndTranslationDirections.DisplayMember = "text";
            wndTranslationDirections.ValueMember = "value";

            wndProfileProperties.DisplayMember = "text";
            wndProfileProperties.ValueMember = "value";

            m_translationProvider = editProvider;
            if (m_translationProvider.m_username == "Public access")
            {
                UsernameLable.Text = "Public access";
                LoginLabel.Text = "Sign In";
            }
            else 
            { 
                UsernameLable.Text = "Welcome, " + m_translationProvider.m_username + "!";
                LoginLabel.Text = "Sign Out";
            }
           
            m_pairs = languagePairs;
            m_checkedState = new Dictionary<string, string>();

            FillProfileList();
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult = DialogResult.Retry;
            Close();

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
