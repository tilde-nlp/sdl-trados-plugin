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
            this.SuspendLayout();
            // 
            // wndTranslationDirections
            // 
            this.wndTranslationDirections.FormattingEnabled = true;
            this.wndTranslationDirections.IntegralHeight = false;
            this.wndTranslationDirections.Location = new System.Drawing.Point(12, 12);
            this.wndTranslationDirections.Name = "wndTranslationDirections";
            this.wndTranslationDirections.Size = new System.Drawing.Size(196, 285);
            this.wndTranslationDirections.Sorted = true;
            this.wndTranslationDirections.TabIndex = 0;
            this.wndTranslationDirections.SelectedValueChanged += new System.EventHandler(this.wndTranslationDirections_SelectedValueChanged);
            // 
            // wndProfileProperties
            // 
            this.wndProfileProperties.FormattingEnabled = true;
            this.wndProfileProperties.IntegralHeight = false;
            this.wndProfileProperties.Location = new System.Drawing.Point(214, 12);
            this.wndProfileProperties.Name = "wndProfileProperties";
            this.wndProfileProperties.Size = new System.Drawing.Size(371, 167);
            this.wndProfileProperties.Sorted = true;
            this.wndProfileProperties.TabIndex = 1;
            this.wndProfileProperties.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.wndProfileProperties_ItemCheck);
            this.wndProfileProperties.SelectedIndexChanged += new System.EventHandler(this.wndProfileProperties_SelectedIndexChanged);
            // 
            // wndDescription
            // 
            this.wndDescription.Location = new System.Drawing.Point(214, 185);
            this.wndDescription.Multiline = true;
            this.wndDescription.Name = "wndDescription";
            this.wndDescription.ReadOnly = true;
            this.wndDescription.Size = new System.Drawing.Size(371, 112);
            this.wndDescription.TabIndex = 5;
            this.wndDescription.TabStop = false;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(429, 303);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(510, 303);
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
            this.wndRunningSystems.Location = new System.Drawing.Point(12, 307);
            this.wndRunningSystems.Name = "wndRunningSystems";
            this.wndRunningSystems.Size = new System.Drawing.Size(153, 17);
            this.wndRunningSystems.TabIndex = 2;
            this.wndRunningSystems.Text = "Show only running systems";
            this.wndRunningSystems.UseVisualStyleBackColor = true;
            this.wndRunningSystems.CheckedChanged += new System.EventHandler(this.wndRunningSystems_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 338);
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
    }
}