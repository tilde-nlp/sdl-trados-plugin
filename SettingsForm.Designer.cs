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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.wndProfileProperties = new System.Windows.Forms.CheckedListBox();
            this.wndDescription = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.wndRunningSystems = new System.Windows.Forms.CheckBox();
            this.UsernameLable = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupLable = new System.Windows.Forms.Label();
            this.GroupSelectComboBox = new System.Windows.Forms.ComboBox();
            this.BusnesText = new System.Windows.Forms.Label();
            this.sourceSelectComboBox = new System.Windows.Forms.ComboBox();
            this.targetSelectComboBox = new System.Windows.Forms.ComboBox();
            this.sourceSelectionLabel = new System.Windows.Forms.Label();
            this.targetSelectionLabel = new System.Windows.Forms.Label();
            this.termCorporaSelectComboBox = new System.Windows.Forms.ComboBox();
            this.termsSelectionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // wndProfileProperties
            // 
            this.wndProfileProperties.CheckOnClick = true;
            this.wndProfileProperties.FormattingEnabled = true;
            this.wndProfileProperties.IntegralHeight = false;
            this.wndProfileProperties.Location = new System.Drawing.Point(12, 100);
            this.wndProfileProperties.Name = "wndProfileProperties";
            this.wndProfileProperties.Size = new System.Drawing.Size(573, 151);
            this.wndProfileProperties.Sorted = true;
            this.wndProfileProperties.TabIndex = 1;
            this.wndProfileProperties.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.wndProfileProperties_ItemCheck);
            this.wndProfileProperties.SelectedIndexChanged += new System.EventHandler(this.wndProfileProperties_SelectedIndexChanged);
            // 
            // wndDescription
            // 
            this.wndDescription.BackColor = System.Drawing.SystemColors.Control;
            this.wndDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wndDescription.Location = new System.Drawing.Point(12, 285);
            this.wndDescription.Multiline = true;
            this.wndDescription.Name = "wndDescription";
            this.wndDescription.ReadOnly = true;
            this.wndDescription.Size = new System.Drawing.Size(573, 72);
            this.wndDescription.TabIndex = 5;
            this.wndDescription.TabStop = false;
            // 
            // btnOk
            // 
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(429, 366);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(510, 366);
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
            this.wndRunningSystems.Location = new System.Drawing.Point(89, 370);
            this.wndRunningSystems.Name = "wndRunningSystems";
            this.wndRunningSystems.Size = new System.Drawing.Size(160, 17);
            this.wndRunningSystems.TabIndex = 2;
            this.wndRunningSystems.Text = "Show only available systems";
            this.wndRunningSystems.UseVisualStyleBackColor = true;
            this.wndRunningSystems.CheckedChanged += new System.EventHandler(this.wndRunningSystems_CheckedChanged);
            // 
            // UsernameLable
            // 
            this.UsernameLable.Location = new System.Drawing.Point(290, 13);
            this.UsernameLable.Name = "UsernameLable";
            this.UsernameLable.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UsernameLable.Size = new System.Drawing.Size(295, 16);
            this.UsernameLable.TabIndex = 6;
            this.UsernameLable.Text = "Logged in as: User";
            this.UsernameLable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 366);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Advanced..";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LetsMT.MTProvider.PluginResources.Logo_71x23;
            this.pictureBox1.Location = new System.Drawing.Point(12, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(71, 24);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // groupLable
            // 
            this.groupLable.AutoSize = true;
            this.groupLable.Location = new System.Drawing.Point(426, 40);
            this.groupLable.Name = "groupLable";
            this.groupLable.Size = new System.Drawing.Size(39, 13);
            this.groupLable.TabIndex = 13;
            this.groupLable.Text = "Group:";
            // 
            // GroupSelectComboBox
            // 
            this.GroupSelectComboBox.FormattingEnabled = true;
            this.GroupSelectComboBox.Location = new System.Drawing.Point(468, 37);
            this.GroupSelectComboBox.Name = "GroupSelectComboBox";
            this.GroupSelectComboBox.Size = new System.Drawing.Size(117, 21);
            this.GroupSelectComboBox.TabIndex = 14;
            this.GroupSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.GroupSelectComboBox_SelectedIndexChanged);
            // 
            // BusnesText
            // 
            this.BusnesText.AutoSize = true;
            this.BusnesText.Location = new System.Drawing.Point(9, 32);
            this.BusnesText.Name = "BusnesText";
            this.BusnesText.Size = new System.Drawing.Size(248, 26);
            this.BusnesText.TabIndex = 15;
            this.BusnesText.Text = "To start translation, please choose a language pair \r\nand a translation system.";
            // 
            // sourceSelectComboBox
            // 
            this.sourceSelectComboBox.FormattingEnabled = true;
            this.sourceSelectComboBox.Location = new System.Drawing.Point(54, 72);
            this.sourceSelectComboBox.Name = "sourceSelectComboBox";
            this.sourceSelectComboBox.Size = new System.Drawing.Size(121, 21);
            this.sourceSelectComboBox.TabIndex = 16;
            this.sourceSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.sourceSelectComboBox_SelectedIndexChanged);
            // 
            // targetSelectComboBox
            // 
            this.targetSelectComboBox.FormattingEnabled = true;
            this.targetSelectComboBox.Location = new System.Drawing.Point(219, 72);
            this.targetSelectComboBox.Name = "targetSelectComboBox";
            this.targetSelectComboBox.Size = new System.Drawing.Size(121, 21);
            this.targetSelectComboBox.TabIndex = 17;
            this.targetSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.targetSelectComboBox_SelectedIndexChanged);
            // 
            // sourceSelectionLabel
            // 
            this.sourceSelectionLabel.AutoSize = true;
            this.sourceSelectionLabel.Location = new System.Drawing.Point(9, 80);
            this.sourceSelectionLabel.Name = "sourceSelectionLabel";
            this.sourceSelectionLabel.Size = new System.Drawing.Size(30, 13);
            this.sourceSelectionLabel.TabIndex = 18;
            this.sourceSelectionLabel.Text = "From";
            // 
            // targetSelectionLabel
            // 
            this.targetSelectionLabel.AutoSize = true;
            this.targetSelectionLabel.Location = new System.Drawing.Point(193, 79);
            this.targetSelectionLabel.Name = "targetSelectionLabel";
            this.targetSelectionLabel.Size = new System.Drawing.Size(20, 13);
            this.targetSelectionLabel.TabIndex = 19;
            this.targetSelectionLabel.Text = "To";
            // 
            // termCorporaSelectComboBox
            // 
            this.termCorporaSelectComboBox.FormattingEnabled = true;
            this.termCorporaSelectComboBox.Location = new System.Drawing.Point(54, 258);
            this.termCorporaSelectComboBox.Name = "termCorporaSelectComboBox";
            this.termCorporaSelectComboBox.Size = new System.Drawing.Size(121, 21);
            this.termCorporaSelectComboBox.TabIndex = 20;
            this.termCorporaSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.termCorporaSelectComboBox_SelectedIndexChanged);
            // 
            // termsSelectionLabel
            // 
            this.termsSelectionLabel.AutoSize = true;
            this.termsSelectionLabel.Location = new System.Drawing.Point(12, 265);
            this.termsSelectionLabel.Name = "termsSelectionLabel";
            this.termsSelectionLabel.Size = new System.Drawing.Size(36, 13);
            this.termsSelectionLabel.TabIndex = 21;
            this.termsSelectionLabel.Text = "Terms";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(597, 399);
            this.Controls.Add(this.termsSelectionLabel);
            this.Controls.Add(this.termCorporaSelectComboBox);
            this.Controls.Add(this.targetSelectionLabel);
            this.Controls.Add(this.sourceSelectionLabel);
            this.Controls.Add(this.targetSelectComboBox);
            this.Controls.Add(this.sourceSelectComboBox);
            this.Controls.Add(this.BusnesText);
            this.Controls.Add(this.GroupSelectComboBox);
            this.Controls.Add(this.groupLable);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.UsernameLable);
            this.Controls.Add(this.wndRunningSystems);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.wndDescription);
            this.Controls.Add(this.wndProfileProperties);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SettingsForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox wndProfileProperties;
        private System.Windows.Forms.TextBox wndDescription;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox wndRunningSystems;
        private System.Windows.Forms.Label UsernameLable;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label groupLable;
        private System.Windows.Forms.ComboBox GroupSelectComboBox;
        private System.Windows.Forms.Label BusnesText;
        private System.Windows.Forms.ComboBox sourceSelectComboBox;
        private System.Windows.Forms.ComboBox targetSelectComboBox;
        private System.Windows.Forms.Label sourceSelectionLabel;
        private System.Windows.Forms.Label targetSelectionLabel;
        private System.Windows.Forms.ComboBox termCorporaSelectComboBox;
        private System.Windows.Forms.Label termsSelectionLabel;
    }
}