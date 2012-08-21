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
    public partial class LimitationForm : Form
    {
        private string c_url;
        public LimitationForm(string  url)
        {
            c_url = url;
            InitializeComponent();
            webBrowserObject.Navigate(c_url);
            
        }

        private void LimitationForm_Load(object sender, EventArgs e)
        {
            
          
        }

      
       
    }
}
