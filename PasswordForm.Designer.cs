namespace LetsMT.MTProvider
{
    partial class PasswordForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.Tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.DemoLink = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.apiUserIdTextBox = new System.Windows.Forms.TextBox();
            this.goButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.getClientIdLinkLabel = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(304, 194);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(61, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DemoLink
            // 
            this.DemoLink.AutoSize = true;
            this.DemoLink.Location = new System.Drawing.Point(12, 199);
            this.DemoLink.Name = "DemoLink";
            this.DemoLink.Size = new System.Drawing.Size(167, 13);
            this.DemoLink.TabIndex = 7;
            this.DemoLink.TabStop = true;
            this.DemoLink.Text = "Get your free DEMO access here.";
            this.DemoLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DemoLink_LinkClicked);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(12, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(353, 76);
            this.label3.TabIndex = 10;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LetsMT.MTProvider.PluginResources.MT_logo;
            this.pictureBox1.Location = new System.Drawing.Point(15, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(133, 37);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // apiUserIdTextBox
            // 
            this.apiUserIdTextBox.Location = new System.Drawing.Point(15, 142);
            this.apiUserIdTextBox.Name = "apiUserIdTextBox";
            this.apiUserIdTextBox.Size = new System.Drawing.Size(270, 20);
            this.apiUserIdTextBox.TabIndex = 13;
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(304, 140);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(61, 23);
            this.goButton.TabIndex = 14;
            this.goButton.Text = "Go";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Client ID";
            // 
            // getClientIdLinkLabel
            // 
            this.getClientIdLinkLabel.AutoSize = true;
            this.getClientIdLinkLabel.Location = new System.Drawing.Point(12, 165);
            this.getClientIdLinkLabel.Name = "getClientIdLinkLabel";
            this.getClientIdLinkLabel.Size = new System.Drawing.Size(83, 13);
            this.getClientIdLinkLabel.TabIndex = 16;
            this.getClientIdLinkLabel.TabStop = true;
            this.getClientIdLinkLabel.Text = "Get my Client ID";
            this.getClientIdLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.getClientIdLinkLabel_LinkClicked);
            // 
            // PasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 227);
            this.Controls.Add(this.getClientIdLinkLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.apiUserIdTextBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DemoLink);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LetsMT Authentication (v 1.2)";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip Tooltip;
        private System.Windows.Forms.LinkLabel DemoLink;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox apiUserIdTextBox;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel getClientIdLinkLabel;
    }
}