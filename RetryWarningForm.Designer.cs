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
            this.NoButton = new System.Windows.Forms.Button();
            this.autoRetry = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.waitingLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // WrningTextLable
            // 
            this.WrningTextLable.Location = new System.Drawing.Point(12, 9);
            this.WrningTextLable.Name = "WrningTextLable";
            this.WrningTextLable.Size = new System.Drawing.Size(259, 46);
            this.WrningTextLable.TabIndex = 0;
            this.WrningTextLable.Text = "Your automated translation system is starting up. Translation will start automati" +
    "cally as soon as the system is ready.";
            // 
            // NoButton
            // 
            this.NoButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.NoButton.Location = new System.Drawing.Point(157, 94);
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
            this.label2.Location = new System.Drawing.Point(13, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 12);
            this.label2.TabIndex = 28;
            this.label2.Text = "Powered by";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::LetsMT.MTProvider.PluginResources.LetsMT_logo;
            this.pictureBox2.Location = new System.Drawing.Point(79, 97);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(57, 19);
            this.pictureBox2.TabIndex = 27;
            this.pictureBox2.TabStop = false;
            // 
            // waitingLabel
            // 
            this.waitingLabel.AutoSize = true;
            this.waitingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.waitingLabel.Location = new System.Drawing.Point(104, 64);
            this.waitingLabel.Name = "waitingLabel";
            this.waitingLabel.Size = new System.Drawing.Size(53, 16);
            this.waitingLabel.TabIndex = 29;
            this.waitingLabel.Text = "Waiting";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LetsMT.MTProvider.PluginResources.ajax_loader;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(163, 64);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.TabIndex = 30;
            this.pictureBox1.TabStop = false;
            // 
            // RetryWarningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.NoButton;
            this.ClientSize = new System.Drawing.Size(283, 129);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.waitingLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.NoButton);
            this.Controls.Add(this.WrningTextLable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RetryWarningForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System starting up";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WrningTextLable;
        private System.Windows.Forms.Button NoButton;
        private System.Windows.Forms.Timer autoRetry;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label waitingLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}