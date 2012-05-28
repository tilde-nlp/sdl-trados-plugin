﻿using System;
using System.Windows.Forms;

namespace LetsMT.MTProvider
{
    public partial class PasswordForm : Form
    {
        private string m_strUsername;
        private string m_strPassword;
        private string m_strAppId;
        private bool m_bRemember;

        #region "Getters & Setters"
        public string strUsername
        {
            get { return m_strUsername;  }
            set { m_strUsername = value;  }
        }

        public string strPassword
        {
            get { return m_strPassword; }
            set { m_strPassword = value; }
        }

        public string strAppId
        {
            get { return m_strAppId; }
            set { m_strAppId = value; }
        }
        public bool bRemember
        {
            get { return m_bRemember; }
            set { m_bRemember = value; }
        }
        #endregion

        public PasswordForm()
        {
            InitializeComponent();

            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(wndUsername.Text.Trim())) && !(String.IsNullOrEmpty(wndPassword.Text.Trim())))
            {
                if (RegisteredRadio.Checked == true)
                {
                    m_strUsername = wndUsername.Text.Trim();
                    m_strPassword = wndPassword.Text.Trim();
                    m_bRemember = true;
                    m_strAppId = "";
                }
                else if (PublicRadio.Checked == true)
                {
                    m_strUsername = "Public access";
                    //random nonemty value
                    m_strPassword = "*";
                    m_bRemember = true;
                    m_strAppId = "LetsMT_Trados_Plugin";
                }

                DialogResult = DialogResult.OK;

                Close();
            }
            else
            {
                MessageBox.Show("Password and Username fields cannot be empty!", "Empty field");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RegisteredRadio_CheckedChanged(object sender, EventArgs e)
        {
            wndPassword.Enabled = true;
            wndUsername.Enabled = true;
        }

        private void PublicRadio_CheckedChanged(object sender, EventArgs e)
        {
            wndPassword.Enabled = false;
            wndUsername.Enabled = false;
            wndPassword.Text = "";
            wndUsername.Text = "";
           
        }

   
    }
}