
namespace MyUI.UretimModule
{
    partial class FrmOperasyonTakip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOperasyonTakip));
            this.PnlOrta = new System.Windows.Forms.FlowLayoutPanel();
            this.operasyonCardControl1 = new MyUI.UretimOperasyonModule.OperasyonCardControlV2();
            this.operasyonCardControl2 = new MyUI.UretimOperasyonModule.OperasyonCardControlV2();
            this.operasyonCardControl3 = new MyUI.UretimOperasyonModule.OperasyonCardControlV2();
            this.operasyonCardControl4 = new MyUI.UretimOperasyonModule.OperasyonCardControlV2();
            this.TimerYenile = new System.Windows.Forms.Timer(this.components);
            this.BtnYenile = new My.Kontrol.Kontroller.MyButton();
            this.BtnTumOperasyonlar = new My.Kontrol.Kontroller.MyButton();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtn)).BeginInit();
            this.pnlAltBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtn2)).BeginInit();
            this.pnlAltBtn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtn1)).BeginInit();
            this.pnlAltBtn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAlt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlUst2)).BeginInit();
            this.pblAlt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            this.PnlOrta.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAltBtn
            // 
            this.pnlAltBtn.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.pnlAltBtn.Appearance.Options.UseBorderColor = true;
            this.pnlAltBtn.Controls.Add(this.BtnTumOperasyonlar);
            this.pnlAltBtn.Controls.Add(this.BtnYenile);
            this.pnlAltBtn.Location = new System.Drawing.Point(0, 650);
            this.pnlAltBtn.Size = new System.Drawing.Size(1265, 37);
            this.pnlAltBtn.Controls.SetChildIndex(this.pnlAltBtn1, 0);
            this.pnlAltBtn.Controls.SetChildIndex(this.pnlAltBtn2, 0);
            this.pnlAltBtn.Controls.SetChildIndex(this.BtnYenile, 0);
            this.pnlAltBtn.Controls.SetChildIndex(this.BtnTumOperasyonlar, 0);
            // 
            // pnlAltBtn2
            // 
            this.pnlAltBtn2.Size = new System.Drawing.Size(10, 33);
            // 
            // BtnYazdir
            // 
            this.BtnYazdir.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.BtnYazdir.Appearance.Options.UseFont = true;
            this.BtnYazdir.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnYazdir.AppearanceHovered.BackColor2 = System.Drawing.Color.DarkGray;
            this.BtnYazdir.AppearanceHovered.Options.UseBackColor = true;
            this.BtnYazdir.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnYazdir.ImageOptions.Image")));
            // 
            // BtnKopyala
            // 
            this.BtnKopyala.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.BtnKopyala.Appearance.Options.UseFont = true;
            this.BtnKopyala.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnKopyala.AppearanceHovered.BackColor2 = System.Drawing.Color.DarkGray;
            this.BtnKopyala.AppearanceHovered.Options.UseBackColor = true;
            this.BtnKopyala.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnKopyala.ImageOptions.Image")));
            // 
            // pnlAltBtn1
            // 
            this.pnlAltBtn1.Location = new System.Drawing.Point(1082, 2);
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnKaydet.Appearance.Options.UseFont = true;
            this.BtnKaydet.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnKaydet.AppearanceHovered.BackColor2 = System.Drawing.Color.DarkGray;
            this.BtnKaydet.AppearanceHovered.Options.UseBackColor = true;
            this.BtnKaydet.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnKaydet.ImageOptions.Image")));
            this.BtnKaydet.Visible = false;
            // 
            // BtnKapat
            // 
            this.BtnKapat.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.BtnKapat.Appearance.Options.UseFont = true;
            this.BtnKapat.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnKapat.AppearanceHovered.BackColor2 = System.Drawing.Color.DarkGray;
            this.BtnKapat.AppearanceHovered.Options.UseBackColor = true;
            this.BtnKapat.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnKapat.ImageOptions.Image")));
            // 
            // pnlSag
            // 
            this.pnlSag.Location = new System.Drawing.Point(1254, 10);
            this.pnlSag.Size = new System.Drawing.Size(11, 640);
            // 
            // pnlSol
            // 
            this.pnlSol.Size = new System.Drawing.Size(10, 640);
            // 
            // pnlAlt
            // 
            this.pnlAlt.Location = new System.Drawing.Point(0, 687);
            this.pnlAlt.Size = new System.Drawing.Size(1265, 10);
            // 
            // pnlUst2
            // 
            this.pnlUst2.Size = new System.Drawing.Size(1265, 10);
            // 
            // pblAlt
            // 
            this.pblAlt.Location = new System.Drawing.Point(0, 697);
            this.pblAlt.Size = new System.Drawing.Size(1265, 27);
            // 
            // lbl_bilgi
            // 
            this.lbl_bilgi.Location = new System.Drawing.Point(1091, 0);
            // 
            // PnlOrta
            // 
            this.PnlOrta.AutoScroll = true;
            this.PnlOrta.Controls.Add(this.operasyonCardControl1);
            this.PnlOrta.Controls.Add(this.operasyonCardControl2);
            this.PnlOrta.Controls.Add(this.operasyonCardControl3);
            this.PnlOrta.Controls.Add(this.operasyonCardControl4);
            this.PnlOrta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlOrta.Location = new System.Drawing.Point(10, 10);
            this.PnlOrta.Margin = new System.Windows.Forms.Padding(5);
            this.PnlOrta.Name = "PnlOrta";
            this.PnlOrta.Padding = new System.Windows.Forms.Padding(1);
            this.PnlOrta.Size = new System.Drawing.Size(1244, 640);
            this.PnlOrta.TabIndex = 27;
            // 
            // operasyonCardControl1
            // 
            this.operasyonCardControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.operasyonCardControl1.Hareketler = null;
            this.operasyonCardControl1.Location = new System.Drawing.Point(4, 4);
            this.operasyonCardControl1.Name = "operasyonCardControl1";
            this.operasyonCardControl1.OperasyonAdi = "";
            this.operasyonCardControl1.OperasyonKodu = "";
            this.operasyonCardControl1.Size = new System.Drawing.Size(575, 310);
            this.operasyonCardControl1.TabIndex = 0;
            // 
            // operasyonCardControl2
            // 
            this.operasyonCardControl2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.operasyonCardControl2.Hareketler = null;
            this.operasyonCardControl2.Location = new System.Drawing.Point(585, 4);
            this.operasyonCardControl2.Name = "operasyonCardControl2";
            this.operasyonCardControl2.OperasyonAdi = "";
            this.operasyonCardControl2.OperasyonKodu = "";
            this.operasyonCardControl2.Size = new System.Drawing.Size(626, 310);
            this.operasyonCardControl2.TabIndex = 1;
            // 
            // operasyonCardControl3
            // 
            this.operasyonCardControl3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.operasyonCardControl3.Hareketler = null;
            this.operasyonCardControl3.Location = new System.Drawing.Point(4, 320);
            this.operasyonCardControl3.Name = "operasyonCardControl3";
            this.operasyonCardControl3.OperasyonAdi = "";
            this.operasyonCardControl3.OperasyonKodu = "";
            this.operasyonCardControl3.Size = new System.Drawing.Size(575, 310);
            this.operasyonCardControl3.TabIndex = 2;
            // 
            // operasyonCardControl4
            // 
            this.operasyonCardControl4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.operasyonCardControl4.Hareketler = null;
            this.operasyonCardControl4.Location = new System.Drawing.Point(585, 320);
            this.operasyonCardControl4.Name = "operasyonCardControl4";
            this.operasyonCardControl4.OperasyonAdi = "";
            this.operasyonCardControl4.OperasyonKodu = "";
            this.operasyonCardControl4.Size = new System.Drawing.Size(626, 310);
            this.operasyonCardControl4.TabIndex = 3;
            // 
            // TimerYenile
            // 
            this.TimerYenile.Interval = 60000;
            // 
            // BtnYenile
            // 
            this.BtnYenile.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnYenile.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.BtnYenile.Appearance.Options.UseFont = true;
            this.BtnYenile.Appearance.Options.UseForeColor = true;
            this.BtnYenile.Dock = System.Windows.Forms.DockStyle.Left;
            this.BtnYenile.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnYenile.ImageOptions.Image")));
            this.BtnYenile.Location = new System.Drawing.Point(12, 2);
            this.BtnYenile.Name = "BtnYenile";
            this.BtnYenile.Size = new System.Drawing.Size(94, 33);
            this.BtnYenile.TabIndex = 2;
            this.BtnYenile.Text = "Yenile";
            this.BtnYenile.Click += new System.EventHandler(this.BtnYenile_Click);
            // 
            // BtnTumOperasyonlar
            // 
            this.BtnTumOperasyonlar.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnTumOperasyonlar.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.BtnTumOperasyonlar.Appearance.Options.UseFont = true;
            this.BtnTumOperasyonlar.Appearance.Options.UseForeColor = true;
            this.BtnTumOperasyonlar.Dock = System.Windows.Forms.DockStyle.Left;
            this.BtnTumOperasyonlar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnTumOperasyonlar.ImageOptions.Image")));
            this.BtnTumOperasyonlar.Location = new System.Drawing.Point(106, 2);
            this.BtnTumOperasyonlar.Name = "BtnTumOperasyonlar";
            this.BtnTumOperasyonlar.Size = new System.Drawing.Size(116, 33);
            this.BtnTumOperasyonlar.TabIndex = 2;
            this.BtnTumOperasyonlar.Text = "Tum\r\nOperasyonlar";
            this.BtnTumOperasyonlar.Click += new System.EventHandler(this.BtnTumOperasyonlar_Click);
            // 
            // FrmOperasyonTakip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 724);
            this.Controls.Add(this.PnlOrta);
            this.FormOlusturuldu = true;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmOperasyonTakip.IconOptions.Image")));
            this.Name = "FrmOperasyonTakip";
            this.Text = "Operasyon Takip";
            this.Controls.SetChildIndex(this.pblAlt, 0);
            this.Controls.SetChildIndex(this.pnlUst2, 0);
            this.Controls.SetChildIndex(this.pnlAlt, 0);
            this.Controls.SetChildIndex(this.pnlAltBtn, 0);
            this.Controls.SetChildIndex(this.pnlSol, 0);
            this.Controls.SetChildIndex(this.pnlSag, 0);
            this.Controls.SetChildIndex(this.PnlOrta, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtn)).EndInit();
            this.pnlAltBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtn2)).EndInit();
            this.pnlAltBtn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtn1)).EndInit();
            this.pnlAltBtn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlSag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAlt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlUst2)).EndInit();
            this.pblAlt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            this.PnlOrta.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel PnlOrta;
        private UretimOperasyonModule.OperasyonCardControlV2 operasyonCardControl1;
        private UretimOperasyonModule.OperasyonCardControlV2 operasyonCardControl2;
        private UretimOperasyonModule.OperasyonCardControlV2 operasyonCardControl3;
        private UretimOperasyonModule.OperasyonCardControlV2 operasyonCardControl4;
        private System.Windows.Forms.Timer TimerYenile;
        private My.Kontrol.Kontroller.MyButton BtnTumOperasyonlar;
        private My.Kontrol.Kontroller.MyButton BtnYenile;
    }
}