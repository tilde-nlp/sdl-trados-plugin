using System;
using System.Windows.Forms;

namespace LetsMT.MTProvider
{
    public partial class PasswordForm : Form
    {
        private string m_strUsername;
        private string m_strPassword;
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
            m_strUsername = wndUsername.Text.Trim();
            m_strPassword = wndPassword.Text.Trim();
            m_bRemember = wndRemember.Checked;

            DialogResult = DialogResult.OK;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}