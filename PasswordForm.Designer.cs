﻿namespace LetsMT.MTProvider
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
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(344, 186);
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
            this.DemoLink.Location = new System.Drawing.Point(198, 161);
            this.DemoLink.Name = "DemoLink";
            this.DemoLink.Size = new System.Drawing.Size(167, 13);
            this.DemoLink.TabIndex = 7;
            this.DemoLink.TabStop = true;
            this.DemoLink.Text = "Get your free DEMO access here.";
            this.DemoLink.Visible = false;
            this.DemoLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DemoLink_LinkClicked);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(12, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(393, 58);
            this.label3.TabIndex = 10;
            this.label3.Text = global::LetsMT.MTProvider.PluginResources.WelcomeText;
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
            this.apiUserIdTextBox.Location = new System.Drawing.Point(15, 134);
            this.apiUserIdTextBox.Name = "apiUserIdTextBox";
            this.apiUserIdTextBox.Size = new System.Drawing.Size(310, 20);
            this.apiUserIdTextBox.TabIndex = 13;
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(344, 132);
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
            this.label1.Location = new System.Drawing.Point(12, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Client ID";
            // 
            // getClientIdLinkLabel
            // 
            this.getClientIdLinkLabel.AutoSize = true;
            this.getClientIdLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.getClientIdLinkLabel.Location = new System.Drawing.Point(12, 158);
            this.getClientIdLinkLabel.Name = "getClientIdLinkLabel";
            this.getClientIdLinkLabel.Size = new System.Drawing.Size(102, 16);
            this.getClientIdLinkLabel.TabIndex = 16;
            this.getClientIdLinkLabel.TabStop = true;
            this.getClientIdLinkLabel.Text = "Get my Client ID";
            this.getClientIdLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.getClientIdLinkLabel_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.label2.Location = new System.Drawing.Point(12, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 12);
            this.label2.TabIndex = 26;
            this.label2.Text = "Powered by";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::LetsMT.MTProvider.PluginResources.LetsMT_logo;
            this.pictureBox2.Location = new System.Drawing.Point(78, 190);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(57, 19);
            this.pictureBox2.TabIndex = 25;
            this.pictureBox2.TabStop = false;
            // 
            // PasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 218);

#if PRESIDENCY
            this.label3.Location = new System.Drawing.Point(102, 25);
            this.label3.Size = new System.Drawing.Size(303, 100);
            this.pictureBox1.Location = new System.Drawing.Point(15, 25);
            this.pictureBox1.Size = new System.Drawing.Size(80, 61);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.apiUserIdTextBox.Location = new System.Drawing.Point(15, 144);
            this.label1.Location = new System.Drawing.Point(12, 123);
            this.goButton.Location = new System.Drawing.Point(344, 142);
#endif
#if (!PRESIDENCY)
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.DemoLink);
#endif
#if LETSMT
            this.Controls.Add(this.getClientIdLinkLabel);
#endif
            this.Controls.Add(this.label1);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.apiUserIdTextBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(global::LetsMT.MTProvider.PluginResources.LetsMT));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.PasswordForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}