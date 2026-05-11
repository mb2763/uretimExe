
namespace My.DatabaseSettings {
    partial class FrmKullaniciAyar {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtKullaniciAdi = new System.Windows.Forms.TextBox();
            this.ChcAdmin = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtSifre = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtAdi = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtSoyadi = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnKaydet = new System.Windows.Forms.Button();
            this.BtnDegistir = new System.Windows.Forms.Button();
            this.BtnSil = new System.Windows.Forms.Button();
            this.BtnKapat = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.BtnYeni = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.BtnYetkiler = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 404);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(721, 43);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ChcAdmin);
            this.panel2.Controls.Add(this.TxtSifre);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.TxtSoyadi);
            this.panel2.Controls.Add(this.TxtAdi);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.TxtKullaniciAdi);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(721, 87);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kullanıcı Adı";
            // 
            // TxtKullaniciAdi
            // 
            this.TxtKullaniciAdi.Location = new System.Drawing.Point(82, 9);
            this.TxtKullaniciAdi.Name = "TxtKullaniciAdi";
            this.TxtKullaniciAdi.Size = new System.Drawing.Size(139, 20);
            this.TxtKullaniciAdi.TabIndex = 1;
            // 
            // ChcAdmin
            // 
            this.ChcAdmin.AutoSize = true;
            this.ChcAdmin.Location = new System.Drawing.Point(82, 61);
            this.ChcAdmin.Name = "ChcAdmin";
            this.ChcAdmin.Size = new System.Drawing.Size(55, 17);
            this.ChcAdmin.TabIndex = 2;
            this.ChcAdmin.Text = "Admin";
            this.ChcAdmin.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Şifre";
            // 
            // TxtSifre
            // 
            this.TxtSifre.Location = new System.Drawing.Point(82, 35);
            this.TxtSifre.Name = "TxtSifre";
            this.TxtSifre.PasswordChar = '*';
            this.TxtSifre.Size = new System.Drawing.Size(139, 20);
            this.TxtSifre.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(227, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Adı";
            // 
            // TxtAdi
            // 
            this.TxtAdi.Location = new System.Drawing.Point(276, 9);
            this.TxtAdi.Name = "TxtAdi";
            this.TxtAdi.Size = new System.Drawing.Size(139, 20);
            this.TxtAdi.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(227, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Soyadı";
            // 
            // TxtSoyadi
            // 
            this.TxtSoyadi.Location = new System.Drawing.Point(276, 35);
            this.TxtSoyadi.Name = "TxtSoyadi";
            this.TxtSoyadi.Size = new System.Drawing.Size(139, 20);
            this.TxtSoyadi.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.BtnKaydet);
            this.flowLayoutPanel1.Controls.Add(this.BtnDegistir);
            this.flowLayoutPanel1.Controls.Add(this.BtnYeni);
            this.flowLayoutPanel1.Controls.Add(this.BtnSil);
            this.flowLayoutPanel1.Controls.Add(this.BtnKapat);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(276, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(445, 43);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Location = new System.Drawing.Point(3, 3);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(81, 37);
            this.BtnKaydet.TabIndex = 0;
            this.BtnKaydet.Text = "Kaydet";
            this.BtnKaydet.UseVisualStyleBackColor = true;
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // BtnDegistir
            // 
            this.BtnDegistir.Location = new System.Drawing.Point(90, 3);
            this.BtnDegistir.Name = "BtnDegistir";
            this.BtnDegistir.Size = new System.Drawing.Size(81, 37);
            this.BtnDegistir.TabIndex = 0;
            this.BtnDegistir.Text = "Değiştir";
            this.BtnDegistir.UseVisualStyleBackColor = true;
            this.BtnDegistir.Click += new System.EventHandler(this.BtnDegistir_Click);
            // 
            // BtnSil
            // 
            this.BtnSil.Location = new System.Drawing.Point(264, 3);
            this.BtnSil.Name = "BtnSil";
            this.BtnSil.Size = new System.Drawing.Size(81, 37);
            this.BtnSil.TabIndex = 0;
            this.BtnSil.Text = "Sil";
            this.BtnSil.UseVisualStyleBackColor = true;
            this.BtnSil.Click += new System.EventHandler(this.BtnSil_Click);
            // 
            // BtnKapat
            // 
            this.BtnKapat.Location = new System.Drawing.Point(351, 3);
            this.BtnKapat.Name = "BtnKapat";
            this.BtnKapat.Size = new System.Drawing.Size(81, 37);
            this.BtnKapat.TabIndex = 0;
            this.BtnKapat.Text = "Kapat";
            this.BtnKapat.UseVisualStyleBackColor = true;
            this.BtnKapat.Click += new System.EventHandler(this.BtnKapat_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 87);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 5;
            this.dataGridView1.Size = new System.Drawing.Size(721, 317);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // BtnYeni
            // 
            this.BtnYeni.Location = new System.Drawing.Point(177, 3);
            this.BtnYeni.Name = "BtnYeni";
            this.BtnYeni.Size = new System.Drawing.Size(81, 37);
            this.BtnYeni.TabIndex = 1;
            this.BtnYeni.Text = "Yeni";
            this.BtnYeni.UseVisualStyleBackColor = true;
            this.BtnYeni.Click += new System.EventHandler(this.BtnYeni_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.BtnYetkiler);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(91, 43);
            this.panel3.TabIndex = 1;
            // 
            // BtnYetkiler
            // 
            this.BtnYetkiler.Location = new System.Drawing.Point(3, 3);
            this.BtnYetkiler.Name = "BtnYetkiler";
            this.BtnYetkiler.Size = new System.Drawing.Size(81, 37);
            this.BtnYetkiler.TabIndex = 1;
            this.BtnYetkiler.Text = "Yetkiler";
            this.BtnYetkiler.UseVisualStyleBackColor = true;
            this.BtnYetkiler.Click += new System.EventHandler(this.BtnYetkiler_Click);
            // 
            // FrmKullaniciAyar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 447);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FrmKullaniciAyar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kullanici Ayar";
            this.Load += new System.EventHandler(this.FrmKullaniciAyar_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox ChcAdmin;
        private System.Windows.Forms.TextBox TxtSifre;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtKullaniciAdi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtSoyadi;
        private System.Windows.Forms.TextBox TxtAdi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button BtnKaydet;
        private System.Windows.Forms.Button BtnDegistir;
        private System.Windows.Forms.Button BtnSil;
        private System.Windows.Forms.Button BtnKapat;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource bs;
        private System.Windows.Forms.Button BtnYeni;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button BtnYetkiler;
    }
}