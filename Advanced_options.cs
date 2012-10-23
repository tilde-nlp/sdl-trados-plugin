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
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(mascedScoreBox.Text))
            {
                int score = int.Parse(mascedScoreBox.Text);
                if (score > 99) { score = 99; }
                m_editProvider.m_resultScore = score;
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

   

    }
}
