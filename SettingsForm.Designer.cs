namespace LetsMT.MTProvider
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.wndTranslationDirections = new System.Windows.Forms.ListBox();
            this.wndProfileProperties = new System.Windows.Forms.CheckedListBox();
            this.wndDescription = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.wndRunningSystems = new System.Windows.Forms.CheckBox();
            this.UsernameLable = new System.Windows.Forms.Label();
            this.LoginLabel = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // wndTranslationDirections
            // 
            this.wndTranslationDirections.FormattingEnabled = true;
            this.wndTranslationDirections.IntegralHeight = false;
            this.wndTranslationDirections.Location = new System.Drawing.Point(12, 33);
            this.wndTranslationDirections.Name = "wndTranslationDirections";
            this.wndTranslationDirections.Size = new System.Drawing.Size(196, 306);
            this.wndTranslationDirections.Sorted = true;
            this.wndTranslationDirections.TabIndex = 0;
            this.wndTranslationDirections.SelectedValueChanged += new System.EventHandler(this.wndTranslationDirections_SelectedValueChanged);
            // 
            // wndProfileProperties
            // 
            this.wndProfileProperties.CheckOnClick = true;
            this.wndProfileProperties.FormattingEnabled = true;
            this.wndProfileProperties.IntegralHeight = false;
            this.wndProfileProperties.Location = new System.Drawing.Point(214, 33);
            this.wndProfileProperties.Name = "wndProfileProperties";
            this.wndProfileProperties.Size = new System.Drawing.Size(371, 194);
            this.wndProfileProperties.Sorted = true;
            this.wndProfileProperties.TabIndex = 1;
            this.wndProfileProperties.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.wndProfileProperties_ItemCheck);
            this.wndProfileProperties.SelectedIndexChanged += new System.EventHandler(this.wndProfileProperties_SelectedIndexChanged);
            // 
            // wndDescription
            // 
            this.wndDescription.BackColor = System.Drawing.SystemColors.Control;
            this.wndDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wndDescription.Location = new System.Drawing.Point(214, 233);
            this.wndDescription.Multiline = true;
            this.wndDescription.Name = "wndDescription";
            this.wndDescription.ReadOnly = true;
            this.wndDescription.Size = new System.Drawing.Size(371, 106);
            this.wndDescription.TabIndex = 5;
            this.wndDescription.TabStop = false;
            // 
            // btnOk
            // 
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(429, 345);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(510, 345);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // wndRunningSystems
            // 
            this.wndRunningSystems.AutoSize = true;
            this.wndRunningSystems.Checked = true;
            this.wndRunningSystems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.wndRunningSystems.Location = new System.Drawing.Point(12, 351);
            this.wndRunningSystems.Name = "wndRunningSystems";
            this.wndRunningSystems.Size = new System.Drawing.Size(153, 17);
            this.wndRunningSystems.TabIndex = 2;
            this.wndRunningSystems.Text = "Show only running systems";
            this.wndRunningSystems.UseVisualStyleBackColor = true;
            this.wndRunningSystems.CheckedChanged += new System.EventHandler(this.wndRunningSystems_CheckedChanged);
            // 
            // UsernameLable
            // 
            this.UsernameLable.Location = new System.Drawing.Point(211, 17);
            this.UsernameLable.Name = "UsernameLable";
            this.UsernameLable.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UsernameLable.Size = new System.Drawing.Size(316, 13);
            this.UsernameLable.TabIndex = 6;
            this.UsernameLable.Text = "Logged in as: User";
            this.UsernameLable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LoginLabel
            // 
            this.LoginLabel.AutoSize = true;
            this.LoginLabel.Location = new System.Drawing.Point(533, 17);
            this.LoginLabel.Name = "LoginLabel";
            this.LoginLabel.Size = new System.Drawing.Size(46, 13);
            this.LoginLabel.TabIndex = 9;
            this.LoginLabel.TabStop = true;
            this.LoginLabel.Text = "Sign out";
            this.LoginLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.LoginLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LetsMT.MTProvider.PluginResources.band_aid_symbol;
            this.pictureBox1.Location = new System.Drawing.Point(12, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(63, 23);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(597, 374);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LoginLabel);
            this.Controls.Add(this.UsernameLable);
            this.Controls.Add(this.wndRunningSystems);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.wndDescription);
            this.Controls.Add(this.wndProfileProperties);
            this.Controls.Add(this.wndTranslationDirections);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SettingsForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox wndTranslationDirections;
        private System.Windows.Forms.CheckedListBox wndProfileProperties;
        private System.Windows.Forms.TextBox wndDescription;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox wndRunningSystems;
        private System.Windows.Forms.Label UsernameLable;
        private System.Windows.Forms.LinkLabel LoginLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}