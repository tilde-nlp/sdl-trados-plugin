namespace LetsMT.MTProvider
{
    partial class RetryWarningForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RetryWarningForm));
            this.WrningTextLable = new System.Windows.Forms.Label();
            this.YesButton = new System.Windows.Forms.Button();
            this.NoButton = new System.Windows.Forms.Button();
            this.autoRetry = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // WrningTextLable
            // 
            this.WrningTextLable.Location = new System.Drawing.Point(12, 9);
            this.WrningTextLable.Name = "WrningTextLable";
            this.WrningTextLable.Size = new System.Drawing.Size(355, 46);
            this.WrningTextLable.TabIndex = 0;
            this.WrningTextLable.Text = "Your automated translation system is starting up. Translation will start automati" +
    "cally as soon as the system is ready. Would you like to try again?";
            // 
            // YesButton
            // 
            this.YesButton.Location = new System.Drawing.Point(172, 58);
            this.YesButton.Name = "YesButton";
            this.YesButton.Size = new System.Drawing.Size(75, 23);
            this.YesButton.TabIndex = 1;
            this.YesButton.Text = "Retry";
            this.YesButton.UseVisualStyleBackColor = true;
            this.YesButton.Click += new System.EventHandler(this.YesButton_Click);
            // 
            // NoButton
            // 
            this.NoButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.NoButton.Location = new System.Drawing.Point(253, 58);
            this.NoButton.Name = "NoButton";
            this.NoButton.Size = new System.Drawing.Size(114, 23);
            this.NoButton.TabIndex = 2;
            this.NoButton.Text = "Continue without MT ";
            this.NoButton.UseVisualStyleBackColor = true;
            this.NoButton.Click += new System.EventHandler(this.NoButton_Click);
            // 
            // autoRetry
            // 
            this.autoRetry.Enabled = true;
            this.autoRetry.Interval = 15000;
            this.autoRetry.Tick += new System.EventHandler(this.autoRetry_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.label2.Location = new System.Drawing.Point(13, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 12);
            this.label2.TabIndex = 28;
            this.label2.Text = "Powered by";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::LetsMT.MTProvider.PluginResources.LetsMT_logo;
            this.pictureBox2.Location = new System.Drawing.Point(79, 62);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(57, 19);
            this.pictureBox2.TabIndex = 27;
            this.pictureBox2.TabStop = false;
            // 
            // RetryWarningForm
            // 
            this.AcceptButton = this.YesButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.NoButton;
            this.ClientSize = new System.Drawing.Size(379, 93);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.NoButton);
            this.Controls.Add(this.YesButton);
            this.Controls.Add(this.WrningTextLable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RetryWarningForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System starting up";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WrningTextLable;
        private System.Windows.Forms.Button YesButton;
        private System.Windows.Forms.Button NoButton;
        private System.Windows.Forms.Timer autoRetry;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}