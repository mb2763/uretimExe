
namespace My.DataPaneli
{
    partial class FrmDataPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDataPanel));
            this.BtnKapat = new System.Windows.Forms.Button();
            this.BtnBaglantiPro = new System.Windows.Forms.Button();
            this.BtnBaglantiMikro = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BtnDatabaseKontrol = new System.Windows.Forms.Button();
            this.BtnDatabaseOlusturUretim = new System.Windows.Forms.Button();
            this.BtnDatabaseOlusturDepoKabul = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.BtnKullaniciAyar = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnKapat
            // 
            this.BtnKapat.BackColor = System.Drawing.Color.Maroon;
            this.BtnKapat.ForeColor = System.Drawing.Color.White;
            this.BtnKapat.Location = new System.Drawing.Point(247, 232);
            this.BtnKapat.Name = "BtnKapat";
            this.BtnKapat.Size = new System.Drawing.Size(207, 40);
            this.BtnKapat.TabIndex = 0;
            this.BtnKapat.Text = "Kapat";
            this.BtnKapat.UseVisualStyleBackColor = false;
            this.BtnKapat.Click += new System.EventHandler(this.BtnKapat_Click);
            // 
            // BtnBaglantiPro
            // 
            this.BtnBaglantiPro.Location = new System.Drawing.Point(6, 19);
            this.BtnBaglantiPro.Name = "BtnBaglantiPro";
            this.BtnBaglantiPro.Size = new System.Drawing.Size(207, 47);
            this.BtnBaglantiPro.TabIndex = 0;
            this.BtnBaglantiPro.Text = "Program Bağlantı Ayar";
            this.BtnBaglantiPro.UseVisualStyleBackColor = true;
            this.BtnBaglantiPro.Click += new System.EventHandler(this.BtnBaglantiPro_Click);
            // 
            // BtnBaglantiMikro
            // 
            this.BtnBaglantiMikro.Location = new System.Drawing.Point(6, 72);
            this.BtnBaglantiMikro.Name = "BtnBaglantiMikro";
            this.BtnBaglantiMikro.Size = new System.Drawing.Size(207, 47);
            this.BtnBaglantiMikro.TabIndex = 0;
            this.BtnBaglantiMikro.Text = "Mikro Bağlantı Ayar";
            this.BtnBaglantiMikro.UseVisualStyleBackColor = true;
            this.BtnBaglantiMikro.Click += new System.EventHandler(this.BtnBaglantiMikro_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnBaglantiPro);
            this.groupBox1.Controls.Add(this.BtnBaglantiMikro);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 134);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Connection String Ayar";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BtnDatabaseKontrol);
            this.groupBox2.Controls.Add(this.BtnDatabaseOlusturUretim);
            this.groupBox2.Controls.Add(this.BtnDatabaseOlusturDepoKabul);
            this.groupBox2.Location = new System.Drawing.Point(241, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 214);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Database Oluştur";
            // 
            // BtnDatabaseKontrol
            // 
            this.BtnDatabaseKontrol.Location = new System.Drawing.Point(6, 19);
            this.BtnDatabaseKontrol.Name = "BtnDatabaseKontrol";
            this.BtnDatabaseKontrol.Size = new System.Drawing.Size(207, 47);
            this.BtnDatabaseKontrol.TabIndex = 0;
            this.BtnDatabaseKontrol.Text = "Database Kontrol";
            this.BtnDatabaseKontrol.UseVisualStyleBackColor = true;
            this.BtnDatabaseKontrol.Click += new System.EventHandler(this.BtnDatabaseKontrol_Click);
            // 
            // BtnDatabaseOlusturUretim
            // 
            this.BtnDatabaseOlusturUretim.Location = new System.Drawing.Point(6, 72);
            this.BtnDatabaseOlusturUretim.Name = "BtnDatabaseOlusturUretim";
            this.BtnDatabaseOlusturUretim.Size = new System.Drawing.Size(207, 47);
            this.BtnDatabaseOlusturUretim.TabIndex = 0;
            this.BtnDatabaseOlusturUretim.Text = "Üretim Tablolarını Oluştur";
            this.BtnDatabaseOlusturUretim.UseVisualStyleBackColor = true;
            this.BtnDatabaseOlusturUretim.Click += new System.EventHandler(this.BtnDatabaseOlusturUretim_Click);
            // 
            // BtnDatabaseOlusturDepoKabul
            // 
            this.BtnDatabaseOlusturDepoKabul.Location = new System.Drawing.Point(6, 125);
            this.BtnDatabaseOlusturDepoKabul.Name = "BtnDatabaseOlusturDepoKabul";
            this.BtnDatabaseOlusturDepoKabul.Size = new System.Drawing.Size(207, 47);
            this.BtnDatabaseOlusturDepoKabul.TabIndex = 0;
            this.BtnDatabaseOlusturDepoKabul.Text = "Depo Kabul Tablolarını Oluştur";
            this.BtnDatabaseOlusturDepoKabul.UseVisualStyleBackColor = true;
            this.BtnDatabaseOlusturDepoKabul.Click += new System.EventHandler(this.BtnDatabaseOlusturDepoKabul_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.BtnKullaniciAyar);
            this.groupBox3.Location = new System.Drawing.Point(12, 151);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(223, 74);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Kullanıcı Ayarları";
            // 
            // BtnKullaniciAyar
            // 
            this.BtnKullaniciAyar.Location = new System.Drawing.Point(6, 19);
            this.BtnKullaniciAyar.Name = "BtnKullaniciAyar";
            this.BtnKullaniciAyar.Size = new System.Drawing.Size(207, 47);
            this.BtnKullaniciAyar.TabIndex = 0;
            this.BtnKullaniciAyar.Text = "Kullanıcı Ayar Ve Yetkiler";
            this.BtnKullaniciAyar.UseVisualStyleBackColor = true;
            this.BtnKullaniciAyar.Click += new System.EventHandler(this.BtnKullaniciAyar_Click);
            // 
            // FrmDataPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 278);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnKapat);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmDataPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Ayarları";
            this.Load += new System.EventHandler(this.Frm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnKapat;
        private System.Windows.Forms.Button BtnBaglantiPro;
        private System.Windows.Forms.Button BtnBaglantiMikro;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BtnDatabaseOlusturUretim;
        private System.Windows.Forms.Button BtnDatabaseOlusturDepoKabul;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button BtnKullaniciAyar;
        private System.Windows.Forms.Button BtnDatabaseKontrol;
    }
}

