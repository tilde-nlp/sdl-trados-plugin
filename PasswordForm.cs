using System;
using System.Windows.Forms;
using System.Diagnostics;

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
                m_strUsername = wndUsername.Text.Trim();
                m_strPassword = wndPassword.Text.Trim();
                m_bRemember = true;
                m_strAppId = "";

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

            private void wndUsername_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(wndUsername.Text.Trim())) && !(String.IsNullOrEmpty(wndPassword.Text.Trim())))
            {

                btnOk.Enabled = true; 
            }
            else
            {
                btnOk.Enabled = false;
            
            }
        }

        private void wndPassword_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(wndUsername.Text.Trim())) && !(String.IsNullOrEmpty(wndPassword.Text.Trim())))
            {
                btnOk.Enabled = true;
                
            }
            else
            {
                btnOk.Enabled = false;
            }
        }

        private void DemoLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process myProcess = new Process();

            try
            {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = "https://www.letsmt.eu/Register.aspx?type=demo";
                myProcess.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot open link!", "Connection problem");
            }
        }

   
    }
}