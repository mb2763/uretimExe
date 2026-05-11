namespace MyUI.MailModule
{
    partial class FrmFisMailGonder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFisMailGonder));
            this.myPanelControl1 = new My.Kontrol.Kontroller.MyPanelControl();
            this.TxtBody = new System.Windows.Forms.RichTextBox();
            this.BtnGonderXlsx = new My.Kontrol.Kontroller.MyButton();
            this.BtnGonderPdf = new My.Kontrol.Kontroller.MyButton();
            this.BtnKapat = new My.Kontrol.Kontroller.MyButton();
            this.TxtMailAdress = new My.Kontrol.Kontroller.MyTextEdit();
            this.TmrKapat = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.myPanelControl1)).BeginInit();
            this.myPanelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtMailAdress.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // myPanelControl1
            // 
            this.myPanelControl1.Controls.Add(this.BtnGonderPdf);
            this.myPanelControl1.Controls.Add(this.BtnGonderXlsx);
            this.myPanelControl1.Controls.Add(this.BtnKapat);
            this.myPanelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.myPanelControl1.Location = new System.Drawing.Point(0, 313);
            this.myPanelControl1.Name = "myPanelControl1";
            this.myPanelControl1.Size = new System.Drawing.Size(381, 44);
            this.myPanelControl1.TabIndex = 7;
            // 
            // TxtBody
            // 
            this.TxtBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtBody.Location = new System.Drawing.Point(0, 0);
            this.TxtBody.Name = "TxtBody";
            this.TxtBody.Size = new System.Drawing.Size(381, 289);
            this.TxtBody.TabIndex = 8;
            this.TxtBody.Text = "";
            // 
            // BtnGonderXlsx
            // 
            this.BtnGonderXlsx.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BtnGonderXlsx.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.BtnGonderXlsx.Appearance.Options.UseFont = true;
            this.BtnGonderXlsx.Appearance.Options.UseForeColor = true;
            this.BtnGonderXlsx.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnGonderXlsx.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnGonderXlsx.ImageOptions.Image")));
            this.BtnGonderXlsx.Location = new System.Drawing.Point(218, 2);
            this.BtnGonderXlsx.Name = "BtnGonderXlsx";
            this.BtnGonderXlsx.Size = new System.Drawing.Size(86, 40);
            this.BtnGonderXlsx.TabIndex = 1;
            this.BtnGonderXlsx.Text = "Gönder \r\nXlsx";
            this.BtnGonderXlsx.Click += new System.EventHandler(this.BtnGonderXlsx_Click);
            // 
            // BtnGonderPdf
            // 
            this.BtnGonderPdf.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BtnGonderPdf.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.BtnGonderPdf.Appearance.Options.UseFont = true;
            this.BtnGonderPdf.Appearance.Options.UseForeColor = true;
            this.BtnGonderPdf.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnGonderPdf.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnGonderPdf.ImageOptions.Image")));
            this.BtnGonderPdf.Location = new System.Drawing.Point(111, 2);
            this.BtnGonderPdf.Name = "BtnGonderPdf";
            this.BtnGonderPdf.Size = new System.Drawing.Size(107, 40);
            this.BtnGonderPdf.TabIndex = 0;
            this.BtnGonderPdf.Text = "Gönder Pdf\r\n(Varsayılan)";
            this.BtnGonderPdf.Click += new System.EventHandler(this.BtnGonderPdf_Click);
            // 
            // BtnKapat
            // 
            this.BtnKapat.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BtnKapat.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.BtnKapat.Appearance.Options.UseFont = true;
            this.BtnKapat.Appearance.Options.UseForeColor = true;
            this.BtnKapat.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKapat.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnKapat.ImageOptions.Image")));
            this.BtnKapat.Location = new System.Drawing.Point(304, 2);
            this.BtnKapat.Name = "BtnKapat";
            this.BtnKapat.Size = new System.Drawing.Size(75, 40);
            this.BtnKapat.TabIndex = 0;
            this.BtnKapat.Text = "Kapat";
            this.BtnKapat.Click += new System.EventHandler(this.BtnKapat_Click);
            // 
            // TxtMailAdress
            // 
            this.TxtMailAdress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TxtMailAdress.EnterMoveNextControl = true;
            this.TxtMailAdress.Location = new System.Drawing.Point(0, 289);
            this.TxtMailAdress.MyDeger = "";
            this.TxtMailAdress.MyMaxLength = 75;
            this.TxtMailAdress.MyReadOnlymi = false;
            this.TxtMailAdress.Name = "TxtMailAdress";
            this.TxtMailAdress.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtMailAdress.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtMailAdress.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtMailAdress.Properties.Appearance.Options.UseFont = true;
            this.TxtMailAdress.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtMailAdress.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtMailAdress.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtMailAdress.Properties.MaxLength = 75;
            this.TxtMailAdress.Size = new System.Drawing.Size(381, 24);
            this.TxtMailAdress.TabIndex = 9;
            // 
            // TmrKapat
            // 
            this.TmrKapat.Interval = 2000;
            this.TmrKapat.Tick += new System.EventHandler(this.TmrKapat_Tick);
            // 
            // FrmFisMailGonder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 357);
            this.Controls.Add(this.TxtBody);
            this.Controls.Add(this.TxtMailAdress);
            this.Controls.Add(this.myPanelControl1);
            this.Name = "FrmFisMailGonder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sipariş Mail Gonder";
            this.Load += new System.EventHandler(this.FrmMail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.myPanelControl1)).EndInit();
            this.myPanelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TxtMailAdress.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private My.Kontrol.Kontroller.MyPanelControl myPanelControl1;
        private My.Kontrol.Kontroller.MyButton BtnGonderXlsx;
        private My.Kontrol.Kontroller.MyButton BtnGonderPdf;
        private My.Kontrol.Kontroller.MyButton BtnKapat;
        private System.Windows.Forms.RichTextBox TxtBody;
        private My.Kontrol.Kontroller.MyTextEdit TxtMailAdress;
        private System.Windows.Forms.Timer TmrKapat;
    }
}