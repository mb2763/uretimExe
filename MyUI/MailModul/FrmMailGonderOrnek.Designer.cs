namespace MyUI.MailModule
{
    partial class FrmMailGonderOrnek
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMailGonderOrnek));
            this.myPanelControl1 = new My.Kontrol.Kontroller.MyPanelControl();
            this.BtnGonder = new My.Kontrol.Kontroller.MyButton();
            this.BtnKapat = new My.Kontrol.Kontroller.MyButton();
            this.TxtBody = new System.Windows.Forms.RichTextBox();
            this.myLabel3 = new My.Kontrol.Kontroller.MyLabel();
            this.myLabel2 = new My.Kontrol.Kontroller.MyLabel();
            this.myLabel1 = new My.Kontrol.Kontroller.MyLabel();
            this.TxtKonu = new My.Kontrol.Kontroller.MyTextEdit();
            this.TxtMail = new My.Kontrol.Kontroller.MyTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.myPanelControl1)).BeginInit();
            this.myPanelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtKonu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtMail.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // myPanelControl1
            // 
            this.myPanelControl1.Controls.Add(this.BtnGonder);
            this.myPanelControl1.Controls.Add(this.BtnKapat);
            this.myPanelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.myPanelControl1.Location = new System.Drawing.Point(0, 184);
            this.myPanelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.myPanelControl1.Name = "myPanelControl1";
            this.myPanelControl1.Size = new System.Drawing.Size(373, 36);
            this.myPanelControl1.TabIndex = 12;
            // 
            // BtnGonder
            // 
            this.BtnGonder.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BtnGonder.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.BtnGonder.Appearance.Options.UseFont = true;
            this.BtnGonder.Appearance.Options.UseForeColor = true;
            this.BtnGonder.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnGonder.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnGonder.ImageOptions.Image")));
            this.BtnGonder.Location = new System.Drawing.Point(243, 2);
            this.BtnGonder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnGonder.Name = "BtnGonder";
            this.BtnGonder.Size = new System.Drawing.Size(64, 32);
            this.BtnGonder.TabIndex = 0;
            this.BtnGonder.Text = "Gönder";
            this.BtnGonder.Click += new System.EventHandler(this.BtnGonder_Click);
            // 
            // BtnKapat
            // 
            this.BtnKapat.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BtnKapat.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.BtnKapat.Appearance.Options.UseFont = true;
            this.BtnKapat.Appearance.Options.UseForeColor = true;
            this.BtnKapat.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKapat.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnKapat.ImageOptions.Image")));
            this.BtnKapat.Location = new System.Drawing.Point(307, 2);
            this.BtnKapat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnKapat.Name = "BtnKapat";
            this.BtnKapat.Size = new System.Drawing.Size(64, 32);
            this.BtnKapat.TabIndex = 0;
            this.BtnKapat.Text = "Kapat";
            this.BtnKapat.Click += new System.EventHandler(this.BtnKapat_Click);
            // 
            // TxtBody
            // 
            this.TxtBody.Location = new System.Drawing.Point(87, 72);
            this.TxtBody.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtBody.Name = "TxtBody";
            this.TxtBody.Size = new System.Drawing.Size(267, 103);
            this.TxtBody.TabIndex = 11;
            this.TxtBody.Text = "";
            // 
            // myLabel3
            // 
            this.myLabel3.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.myLabel3.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.myLabel3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.myLabel3.Appearance.Options.UseBorderColor = true;
            this.myLabel3.Appearance.Options.UseFont = true;
            this.myLabel3.Appearance.Options.UseForeColor = true;
            this.myLabel3.Location = new System.Drawing.Point(36, 73);
            this.myLabel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.myLabel3.Name = "myLabel3";
            this.myLabel3.Size = new System.Drawing.Size(34, 16);
            this.myLabel3.TabIndex = 8;
            this.myLabel3.Text = "Mesaj";
            // 
            // myLabel2
            // 
            this.myLabel2.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.myLabel2.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.myLabel2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.myLabel2.Appearance.Options.UseBorderColor = true;
            this.myLabel2.Appearance.Options.UseFont = true;
            this.myLabel2.Appearance.Options.UseForeColor = true;
            this.myLabel2.Location = new System.Drawing.Point(36, 51);
            this.myLabel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.myLabel2.Name = "myLabel2";
            this.myLabel2.Size = new System.Drawing.Size(28, 16);
            this.myLabel2.TabIndex = 9;
            this.myLabel2.Text = "Konu";
            // 
            // myLabel1
            // 
            this.myLabel1.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.myLabel1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.myLabel1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.myLabel1.Appearance.Options.UseBorderColor = true;
            this.myLabel1.Appearance.Options.UseFont = true;
            this.myLabel1.Appearance.Options.UseForeColor = true;
            this.myLabel1.Location = new System.Drawing.Point(36, 27);
            this.myLabel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.myLabel1.Name = "myLabel1";
            this.myLabel1.Size = new System.Drawing.Size(23, 16);
            this.myLabel1.TabIndex = 10;
            this.myLabel1.Text = "Mail";
            // 
            // TxtKonu
            // 
            this.TxtKonu.EditValue = "";
            this.TxtKonu.EnterMoveNextControl = true;
            this.TxtKonu.Location = new System.Drawing.Point(87, 48);
            this.TxtKonu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtKonu.MyDeger = "";
            this.TxtKonu.MyMaxLength = 75;
            this.TxtKonu.MyReadOnlymi = false;
            this.TxtKonu.Name = "TxtKonu";
            this.TxtKonu.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtKonu.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtKonu.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtKonu.Properties.Appearance.Options.UseFont = true;
            this.TxtKonu.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtKonu.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtKonu.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtKonu.Properties.MaxLength = 75;
            this.TxtKonu.Size = new System.Drawing.Size(267, 24);
            this.TxtKonu.TabIndex = 6;
            // 
            // TxtMail
            // 
            this.TxtMail.EditValue = "";
            this.TxtMail.EnterMoveNextControl = true;
            this.TxtMail.Location = new System.Drawing.Point(87, 19);
            this.TxtMail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtMail.MyDeger = "";
            this.TxtMail.MyMaxLength = 75;
            this.TxtMail.MyReadOnlymi = false;
            this.TxtMail.Name = "TxtMail";
            this.TxtMail.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtMail.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtMail.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtMail.Properties.Appearance.Options.UseFont = true;
            this.TxtMail.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtMail.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtMail.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtMail.Properties.MaxLength = 75;
            this.TxtMail.Size = new System.Drawing.Size(267, 24);
            this.TxtMail.TabIndex = 7;
            // 
            // FrmMailGonderOrnek
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 220);
            this.Controls.Add(this.myPanelControl1);
            this.Controls.Add(this.TxtBody);
            this.Controls.Add(this.myLabel3);
            this.Controls.Add(this.myLabel2);
            this.Controls.Add(this.myLabel1);
            this.Controls.Add(this.TxtKonu);
            this.Controls.Add(this.TxtMail);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmMailGonderOrnek";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mail Gonder Ornek";
            this.Load += new System.EventHandler(this.FrmMailSendOrnek_Load);
            ((System.ComponentModel.ISupportInitialize)(this.myPanelControl1)).EndInit();
            this.myPanelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TxtKonu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtMail.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private My.Kontrol.Kontroller.MyPanelControl myPanelControl1;
        private My.Kontrol.Kontroller.MyButton BtnGonder;
        private My.Kontrol.Kontroller.MyButton BtnKapat;
        private System.Windows.Forms.RichTextBox TxtBody;
        private My.Kontrol.Kontroller.MyLabel myLabel3;
        private My.Kontrol.Kontroller.MyLabel myLabel2;
        private My.Kontrol.Kontroller.MyLabel myLabel1;
        private My.Kontrol.Kontroller.MyTextEdit TxtKonu;
        private My.Kontrol.Kontroller.MyTextEdit TxtMail;
    }
}