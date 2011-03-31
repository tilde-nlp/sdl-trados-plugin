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

            List<ListItem> profiles = m_translationProvider.m_profileCollection.GetProfileList();
            m_checkedState = new Dictionary<string, string>();

            foreach (ListItem item in profiles)
            {
                wndTranslationDirections.Items.Add(item);
            }

            //Select the profile if only one given (settings clicked on specific profile)
            bool bHasSelection = false;

            if (languagePairs.Length == 1)
            {
                string singleLang = languagePairs[0].SourceCulture.TwoLetterISOLanguageName + " - " + languagePairs[0].TargetCulture.TwoLetterISOLanguageName;
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
            if(!bHasSelection)
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

                List<ListItem> systems = m_translationProvider.m_profileCollection.GetProfileSystemList(valSelected);

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

                wndDescription.Text = m_translationProvider.m_profileCollection.GetSystemById(valSelected).GetDescription();
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
