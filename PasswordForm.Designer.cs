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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.wndUsername = new System.Windows.Forms.TextBox();
            this.wndPassword = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.wndRemember = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password:";
            // 
            // wndUsername
            // 
            this.wndUsername.Location = new System.Drawing.Point(76, 28);
            this.wndUsername.Name = "wndUsername";
            this.wndUsername.Size = new System.Drawing.Size(230, 20);
            this.wndUsername.TabIndex = 2;
            this.wndUsername.WordWrap = false;
            // 
            // wndPassword
            // 
            this.wndPassword.Location = new System.Drawing.Point(76, 69);
            this.wndPassword.Name = "wndPassword";
            this.wndPassword.Size = new System.Drawing.Size(230, 20);
            this.wndPassword.TabIndex = 3;
            this.wndPassword.UseSystemPasswordChar = true;
            this.wndPassword.WordWrap = false;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(150, 110);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(231, 110);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // wndRemember
            // 
            this.wndRemember.AutoSize = true;
            this.wndRemember.Checked = true;
            this.wndRemember.CheckState = System.Windows.Forms.CheckState.Checked;
            this.wndRemember.Location = new System.Drawing.Point(15, 114);
            this.wndRemember.Name = "wndRemember";
            this.wndRemember.Size = new System.Drawing.Size(77, 17);
            this.wndRemember.TabIndex = 6;
            this.wndRemember.Text = "Remember";
            this.wndRemember.UseVisualStyleBackColor = true;
            // 
            // PasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 145);
            this.Controls.Add(this.wndRemember);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.wndPassword);
            this.Controls.Add(this.wndUsername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "LetsMT! Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox wndUsername;
        private System.Windows.Forms.TextBox wndPassword;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox wndRemember;
    }
}