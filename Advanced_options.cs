using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LetsMT.MTProvider
{
    public partial class Advanced_options : Form
    {
        private LetsMTTranslationProvider m_editProvider;
        public Advanced_options(ref LetsMTTranslationProvider editProvider)
        {
            m_editProvider = editProvider;     
            InitializeComponent();
            mascedScoreBox.Text = m_editProvider.m_resultScore.ToString();
            maskedTimeoutBox.Text = m_editProvider.m_timeout.ToString();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(mascedScoreBox.Text))
            {
                int score = int.Parse(mascedScoreBox.Text);
                if (score > 99) { score = 99; }
                m_editProvider.m_resultScore = score;
            }
            if (!string.IsNullOrEmpty(maskedTimeoutBox.Text))
            {
                int score = int.Parse(maskedTimeoutBox.Text);
                if (score <= 0) { score = 1; }
                m_editProvider.m_timeout = score;
            }
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mascedScoreBox_Validating(object sender, CancelEventArgs e)
        {
            int num;
            if (Int32.TryParse(mascedScoreBox.Text, out num) == true)
            {
                if (num < 0 || num > 99)
                {
                    mascedScoreBox.Text = "99";
                }

            }
            else
            {
                mascedScoreBox.Text = "0";
            }
        }

        private void maskedTimeoutBox_Validating(object sender, CancelEventArgs e)
        {
            int num;
            if (Int32.TryParse(maskedTimeoutBox.Text, out num) == true)
            {
                if (num <= 0)
                {
                    maskedTimeoutBox.Text = "1";
                }

            }
            else
            {
                maskedTimeoutBox.Text = "30";
            }
        }

   

    }
}
