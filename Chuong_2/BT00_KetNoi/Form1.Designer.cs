
namespace BT00_KetNoi
{
    partial class Form1
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
            this.btnacc2003 = new System.Windows.Forms.Button();
            this.btnacc2019 = new System.Windows.Forms.Button();
            this.btnsqlwd = new System.Windows.Forms.Button();
            this.btnsqlsa = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnacc2003
            // 
            this.btnacc2003.Location = new System.Drawing.Point(41, 32);
            this.btnacc2003.Name = "btnacc2003";
            this.btnacc2003.Size = new System.Drawing.Size(88, 46);
            this.btnacc2003.TabIndex = 0;
            this.btnacc2003.Text = "Access 2003";
            this.btnacc2003.UseVisualStyleBackColor = true;
            this.btnacc2003.Click += new System.EventHandler(this.btnacc2003_Click);
            // 
            // btnacc2019
            // 
            this.btnacc2019.Location = new System.Drawing.Point(135, 32);
            this.btnacc2019.Name = "btnacc2019";
            this.btnacc2019.Size = new System.Drawing.Size(88, 46);
            this.btnacc2019.TabIndex = 0;
            this.btnacc2019.Text = "Access 2019";
            this.btnacc2019.UseVisualStyleBackColor = true;
            this.btnacc2019.Click += new System.EventHandler(this.btnacc2019_Click);
            // 
            // btnsqlwd
            // 
            this.btnsqlwd.Location = new System.Drawing.Point(229, 32);
            this.btnsqlwd.Name = "btnsqlwd";
            this.btnsqlwd.Size = new System.Drawing.Size(88, 46);
            this.btnsqlwd.TabIndex = 0;
            this.btnsqlwd.Text = "SQL window";
            this.btnsqlwd.UseVisualStyleBackColor = true;
            this.btnsqlwd.Click += new System.EventHandler(this.btnsqlwd_Click);
            // 
            // btnsqlsa
            // 
            this.btnsqlsa.Location = new System.Drawing.Point(323, 32);
            this.btnsqlsa.Name = "btnsqlsa";
            this.btnsqlsa.Size = new System.Drawing.Size(88, 46);
            this.btnsqlsa.TabIndex = 0;
            this.btnsqlsa.Text = "SQL Sa";
            this.btnsqlsa.UseVisualStyleBackColor = true;
            this.btnsqlsa.Click += new System.EventHandler(this.btnsqlsa_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 248);
            this.Controls.Add(this.btnsqlsa);
            this.Controls.Add(this.btnsqlwd);
            this.Controls.Add(this.btnacc2019);
            this.Controls.Add(this.btnacc2003);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kết Nối";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnacc2003;
        private System.Windows.Forms.Button btnacc2019;
        private System.Windows.Forms.Button btnsqlwd;
        private System.Windows.Forms.Button btnsqlsa;
    }
}

