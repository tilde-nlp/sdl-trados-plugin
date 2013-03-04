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
    public partial class RetryWarningForm : Form
    {
        public RetryWarningForm()
        {
            DialogResult = DialogResult.Abort;
            InitializeComponent();
            autoRetry.Start();
        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            DialogResult =  DialogResult.Yes;
            this.Close();
        }

        private void NoButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            this.Close();
        }

        private void autoRetry_Tick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            this.Close();
        }

    }
}
