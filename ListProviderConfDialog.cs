﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LetsMT.MTProvider
{
    public partial class ListProviderConfDialog : Form
    {
        #region "ListProviderConfDialog"
        public ListProviderConfDialog(ListTranslationOptions options)
        {
            Options = options;
            InitializeComponent();
            UpdateDialog();
        }

        public ListTranslationOptions Options
        {
            get;
            set;
        }
        #endregion


        #region "UpdateDialog"
        private void UpdateDialog()
        {
            txt_ListFile.Text = Options.ListFileName;
            combo_delimiter.Text = Options.Delimiter;
        }
        #endregion

        
        private void ListProviderConfDialog_Load(object sender, EventArgs e)
        {

        }        


        #region "Browse"
        private void btn_Browse_Click(object sender, EventArgs e)
        {
            this.dlg_OpenFile.ShowDialog();
            string fileName = dlg_OpenFile.FileName;

            if (fileName != "")
            {
                txt_ListFile.Text = fileName;
            }
        }
        #endregion

        #region "OK"
        private void bnt_OK_Click(object sender, EventArgs e)
        {
            Options.Delimiter = this.combo_delimiter.Text;
            Options.ListFileName = this.txt_ListFile.Text;
        }
        #endregion

        private void btn_Cancel_Click(object sender, EventArgs e)
        {

        }




    }
}
