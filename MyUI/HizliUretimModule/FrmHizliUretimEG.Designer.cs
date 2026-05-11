namespace MyUI.HizliUretimModule {
    partial class FrmHizliUretimEG {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHizliUretimEG));
            this.LblName7 = new My.Kontrol.Kontroller.MyLabel();
            this.LblName3 = new My.Kontrol.Kontroller.MyLabel();
            this.TxtStokAdi = new My.Kontrol.Kontroller.MyButtonEdit();
            this.TxtStokKodu = new My.Kontrol.Kontroller.MyButtonEdit();
            this.LblName4 = new My.Kontrol.Kontroller.MyLabel();
            this.TxtBirim1 = new My.Kontrol.Kontroller.MyTextEdit();
            this.myLabel2 = new My.Kontrol.Kontroller.MyLabel();
            this.TxtMiktar = new My.Kontrol.Kontroller.MyTextEditSayi();
            this.TxtPartiNo = new My.Kontrol.Kontroller.MyTextEdit();
            this.LblPartiNo = new My.Kontrol.Kontroller.MyLabel();
            this.TxtLotNo = new My.Kontrol.Kontroller.MyTextEditSayi();
            this.LblLotNo = new My.Kontrol.Kontroller.MyLabel();
            this.bsDpGir = new System.Windows.Forms.BindingSource(this.components);
            this.myLabel5 = new My.Kontrol.Kontroller.MyLabel();
            this.bsDpCik = new System.Windows.Forms.BindingSource(this.components);
            this.TxtDepoNoGiris = new My.Kontrol.Kontroller.MyLookupEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtn)).BeginInit();
            this.pnlAltBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtnNav)).BeginInit();
            this.pnlAltBtnNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtn1)).BeginInit();
            this.pnlAltBtn1.SuspendLayout();
            this.pblAlt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAlt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlUst2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtn2)).BeginInit();
            this.pnlAltBtn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtStokAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtStokKodu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtBirim1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtMiktar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPartiNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtLotNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDpGir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDpCik)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtDepoNoGiris.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlAltBtn
            // 
            this.pnlAltBtn.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.pnlAltBtn.Appearance.Options.UseBorderColor = true;
            this.pnlAltBtn.Location = new System.Drawing.Point(0, 186);
            this.pnlAltBtn.Size = new System.Drawing.Size(583, 37);
            this.pnlAltBtn.TabIndex = 8;
            // 
            // pnlAltBtn1
            // 
            this.pnlAltBtn1.Location = new System.Drawing.Point(159, 2);
            // 
            // BtnKapat
            // 
            this.BtnKapat.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnKapat.Appearance.Options.UseFont = true;
            this.BtnKapat.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnKapat.AppearanceHovered.BackColor2 = System.Drawing.Color.DarkGray;
            this.BtnKapat.AppearanceHovered.Options.UseBackColor = true;
            this.BtnKapat.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnKapat.ImageOptions.Image")));
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnKaydet.Appearance.Options.UseFont = true;
            this.BtnKaydet.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnKaydet.AppearanceHovered.BackColor2 = System.Drawing.Color.DarkGray;
            this.BtnKaydet.AppearanceHovered.Options.UseBackColor = true;
            this.BtnKaydet.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnKaydet.ImageOptions.Image")));
            this.BtnKaydet.Location = new System.Drawing.Point(12, 0);
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // BtnYazdir
            // 
            this.BtnYazdir.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnYazdir.Appearance.Options.UseFont = true;
            this.BtnYazdir.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnYazdir.AppearanceHovered.BackColor2 = System.Drawing.Color.DarkGray;
            this.BtnYazdir.AppearanceHovered.Options.UseBackColor = true;
            this.BtnYazdir.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnYazdir.ImageOptions.Image")));
            // 
            // BtnSil
            // 
            this.BtnSil.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnSil.Appearance.Options.UseFont = true;
            this.BtnSil.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnSil.AppearanceHovered.BackColor2 = System.Drawing.Color.DarkGray;
            this.BtnSil.AppearanceHovered.Options.UseBackColor = true;
            this.BtnSil.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnSil.ImageOptions.Image")));
            this.BtnSil.Visible = false;
            // 
            // lblBaslik
            // 
            this.lblBaslik.Size = new System.Drawing.Size(583, 40);
            this.lblBaslik.Text = "Hizli Üretim Kayıt";
            // 
            // pblAlt
            // 
            this.pblAlt.Location = new System.Drawing.Point(0, 223);
            this.pblAlt.Size = new System.Drawing.Size(583, 27);
            // 
            // lbl_bilgi
            // 
            this.lbl_bilgi.Location = new System.Drawing.Point(409, 0);
            // 
            // pnlSag
            // 
            this.pnlSag.Location = new System.Drawing.Point(572, 50);
            this.pnlSag.Size = new System.Drawing.Size(11, 126);
            // 
            // pnlSol
            // 
            this.pnlSol.Size = new System.Drawing.Size(10, 126);
            // 
            // pnlAlt
            // 
            this.pnlAlt.Location = new System.Drawing.Point(0, 176);
            this.pnlAlt.Size = new System.Drawing.Size(583, 10);
            // 
            // pnlUst2
            // 
            this.pnlUst2.Size = new System.Drawing.Size(583, 10);
            // 
            // BtnYeni
            // 
            this.BtnYeni.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnYeni.Appearance.Options.UseFont = true;
            this.BtnYeni.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnYeni.AppearanceHovered.BackColor2 = System.Drawing.Color.DarkGray;
            this.BtnYeni.AppearanceHovered.Options.UseBackColor = true;
            this.BtnYeni.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnYeni.ImageOptions.Image")));
            // 
            // BtnSon
            // 
            this.BtnSon.Appearance.BackColor = System.Drawing.Color.CadetBlue;
            this.BtnSon.Appearance.BorderColor = System.Drawing.Color.White;
            this.BtnSon.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BtnSon.Appearance.ForeColor = System.Drawing.Color.White;
            this.BtnSon.Appearance.Options.UseBackColor = true;
            this.BtnSon.Appearance.Options.UseBorderColor = true;
            this.BtnSon.Appearance.Options.UseFont = true;
            this.BtnSon.Appearance.Options.UseForeColor = true;
            this.BtnSon.AppearanceHovered.BackColor = System.Drawing.Color.Purple;
            this.BtnSon.AppearanceHovered.BackColor2 = System.Drawing.Color.Goldenrod;
            this.BtnSon.AppearanceHovered.ForeColor = System.Drawing.Color.White;
            this.BtnSon.AppearanceHovered.Options.UseBackColor = true;
            this.BtnSon.AppearanceHovered.Options.UseForeColor = true;
            this.BtnSon.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.BtnSon.LookAndFeel.UseDefaultLookAndFeel = false;
            // 
            // BtnSonraki
            // 
            this.BtnSonraki.Appearance.BackColor = System.Drawing.Color.CadetBlue;
            this.BtnSonraki.Appearance.BorderColor = System.Drawing.Color.White;
            this.BtnSonraki.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BtnSonraki.Appearance.ForeColor = System.Drawing.Color.White;
            this.BtnSonraki.Appearance.Options.UseBackColor = true;
            this.BtnSonraki.Appearance.Options.UseBorderColor = true;
            this.BtnSonraki.Appearance.Options.UseFont = true;
            this.BtnSonraki.Appearance.Options.UseForeColor = true;
            this.BtnSonraki.AppearanceHovered.BackColor = System.Drawing.Color.Purple;
            this.BtnSonraki.AppearanceHovered.BackColor2 = System.Drawing.Color.Goldenrod;
            this.BtnSonraki.AppearanceHovered.ForeColor = System.Drawing.Color.White;
            this.BtnSonraki.AppearanceHovered.Options.UseBackColor = true;
            this.BtnSonraki.AppearanceHovered.Options.UseForeColor = true;
            this.BtnSonraki.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.BtnSonraki.LookAndFeel.UseDefaultLookAndFeel = false;
            // 
            // BtnOnceki
            // 
            this.BtnOnceki.Appearance.BackColor = System.Drawing.Color.CadetBlue;
            this.BtnOnceki.Appearance.BorderColor = System.Drawing.Color.White;
            this.BtnOnceki.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BtnOnceki.Appearance.ForeColor = System.Drawing.Color.White;
            this.BtnOnceki.Appearance.Options.UseBackColor = true;
            this.BtnOnceki.Appearance.Options.UseBorderColor = true;
            this.BtnOnceki.Appearance.Options.UseFont = true;
            this.BtnOnceki.Appearance.Options.UseForeColor = true;
            this.BtnOnceki.AppearanceHovered.BackColor = System.Drawing.Color.Purple;
            this.BtnOnceki.AppearanceHovered.BackColor2 = System.Drawing.Color.Goldenrod;
            this.BtnOnceki.AppearanceHovered.ForeColor = System.Drawing.Color.White;
            this.BtnOnceki.AppearanceHovered.Options.UseBackColor = true;
            this.BtnOnceki.AppearanceHovered.Options.UseForeColor = true;
            this.BtnOnceki.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.BtnOnceki.LookAndFeel.UseDefaultLookAndFeel = false;
            // 
            // BtnIlk
            // 
            this.BtnIlk.Appearance.BackColor = System.Drawing.Color.CadetBlue;
            this.BtnIlk.Appearance.BorderColor = System.Drawing.Color.White;
            this.BtnIlk.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BtnIlk.Appearance.ForeColor = System.Drawing.Color.White;
            this.BtnIlk.Appearance.Options.UseBackColor = true;
            this.BtnIlk.Appearance.Options.UseBorderColor = true;
            this.BtnIlk.Appearance.Options.UseFont = true;
            this.BtnIlk.Appearance.Options.UseForeColor = true;
            this.BtnIlk.AppearanceHovered.BackColor = System.Drawing.Color.Purple;
            this.BtnIlk.AppearanceHovered.BackColor2 = System.Drawing.Color.Goldenrod;
            this.BtnIlk.AppearanceHovered.ForeColor = System.Drawing.Color.White;
            this.BtnIlk.AppearanceHovered.Options.UseBackColor = true;
            this.BtnIlk.AppearanceHovered.Options.UseForeColor = true;
            this.BtnIlk.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.BtnIlk.LookAndFeel.UseDefaultLookAndFeel = false;
            // 
            // BtnDuzenle
            // 
            this.BtnDuzenle.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnDuzenle.Appearance.Options.UseFont = true;
            this.BtnDuzenle.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnDuzenle.AppearanceHovered.BackColor2 = System.Drawing.Color.DarkGray;
            this.BtnDuzenle.AppearanceHovered.Options.UseBackColor = true;
            this.BtnDuzenle.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnDuzenle.ImageOptions.Image")));
            // 
            // LblName7
            // 
            this.LblName7.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.LblName7.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LblName7.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.LblName7.Appearance.Options.UseBorderColor = true;
            this.LblName7.Appearance.Options.UseFont = true;
            this.LblName7.Appearance.Options.UseForeColor = true;
            this.LblName7.Location = new System.Drawing.Point(13, 90);
            this.LblName7.Name = "LblName7";
            this.LblName7.Size = new System.Drawing.Size(43, 16);
            this.LblName7.TabIndex = 54;
            this.LblName7.Text = "StokAdi";
            // 
            // LblName3
            // 
            this.LblName3.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.LblName3.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LblName3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.LblName3.Appearance.Options.UseBorderColor = true;
            this.LblName3.Appearance.Options.UseFont = true;
            this.LblName3.Appearance.Options.UseForeColor = true;
            this.LblName3.Location = new System.Drawing.Point(13, 60);
            this.LblName3.Name = "LblName3";
            this.LblName3.Size = new System.Drawing.Size(53, 16);
            this.LblName3.TabIndex = 55;
            this.LblName3.Text = "StokKodu";
            // 
            // TxtStokAdi
            // 
            this.TxtStokAdi.EditValue = "";
            this.TxtStokAdi.EnterMoveNextControl = true;
            this.TxtStokAdi.Location = new System.Drawing.Point(77, 86);
            this.TxtStokAdi.MyDeger = "";
            this.TxtStokAdi.MyId = null;
            this.TxtStokAdi.MyMaxLength = 75;
            this.TxtStokAdi.MyReadOnlymi = false;
            this.TxtStokAdi.Name = "TxtStokAdi";
            this.TxtStokAdi.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtStokAdi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtStokAdi.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtStokAdi.Properties.Appearance.Options.UseFont = true;
            this.TxtStokAdi.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtStokAdi.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtStokAdi.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtStokAdi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.TxtStokAdi.Properties.MaxLength = 75;
            this.TxtStokAdi.Size = new System.Drawing.Size(303, 24);
            this.TxtStokAdi.TabIndex = 1;
            // 
            // TxtStokKodu
            // 
            this.TxtStokKodu.EditValue = "";
            this.TxtStokKodu.EnterMoveNextControl = true;
            this.TxtStokKodu.Location = new System.Drawing.Point(77, 56);
            this.TxtStokKodu.MyDeger = "";
            this.TxtStokKodu.MyId = null;
            this.TxtStokKodu.MyMaxLength = 75;
            this.TxtStokKodu.MyReadOnlymi = false;
            this.TxtStokKodu.Name = "TxtStokKodu";
            this.TxtStokKodu.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtStokKodu.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtStokKodu.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtStokKodu.Properties.Appearance.Options.UseFont = true;
            this.TxtStokKodu.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtStokKodu.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtStokKodu.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtStokKodu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.TxtStokKodu.Properties.MaxLength = 75;
            this.TxtStokKodu.Size = new System.Drawing.Size(303, 24);
            this.TxtStokKodu.TabIndex = 0;
            // 
            // LblName4
            // 
            this.LblName4.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.LblName4.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LblName4.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.LblName4.Appearance.Options.UseBorderColor = true;
            this.LblName4.Appearance.Options.UseFont = true;
            this.LblName4.Appearance.Options.UseForeColor = true;
            this.LblName4.Location = new System.Drawing.Point(386, 60);
            this.LblName4.Name = "LblName4";
            this.LblName4.Size = new System.Drawing.Size(29, 16);
            this.LblName4.TabIndex = 58;
            this.LblName4.Text = "Birim";
            // 
            // TxtBirim1
            // 
            this.TxtBirim1.EditValue = "";
            this.TxtBirim1.EnterMoveNextControl = true;
            this.TxtBirim1.Location = new System.Drawing.Point(430, 56);
            this.TxtBirim1.MyDeger = "";
            this.TxtBirim1.MyMaxLength = 75;
            this.TxtBirim1.MyReadOnlymi = false;
            this.TxtBirim1.Name = "TxtBirim1";
            this.TxtBirim1.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtBirim1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtBirim1.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtBirim1.Properties.Appearance.Options.UseFont = true;
            this.TxtBirim1.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtBirim1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtBirim1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtBirim1.Properties.MaxLength = 75;
            this.TxtBirim1.Size = new System.Drawing.Size(117, 24);
            this.TxtBirim1.TabIndex = 4;
            // 
            // myLabel2
            // 
            this.myLabel2.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.myLabel2.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.myLabel2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.myLabel2.Appearance.Options.UseBorderColor = true;
            this.myLabel2.Appearance.Options.UseFont = true;
            this.myLabel2.Appearance.Options.UseForeColor = true;
            this.myLabel2.Location = new System.Drawing.Point(386, 90);
            this.myLabel2.Name = "myLabel2";
            this.myLabel2.Size = new System.Drawing.Size(35, 16);
            this.myLabel2.TabIndex = 65;
            this.myLabel2.Text = "Miktar";
            // 
            // TxtMiktar
            // 
            this.TxtMiktar.EditValue = "0";
            this.TxtMiktar.EnterMoveNextControl = true;
            this.TxtMiktar.Location = new System.Drawing.Point(430, 86);
            this.TxtMiktar.MyDeger = "0";
            this.TxtMiktar.MyEksiGirilebilirmi = false;
            this.TxtMiktar.MyKurusHane = 4;
            this.TxtMiktar.MyMaxLength = 0;
            this.TxtMiktar.MyReadOnlymi = false;
            this.TxtMiktar.Name = "TxtMiktar";
            this.TxtMiktar.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtMiktar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtMiktar.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtMiktar.Properties.Appearance.Options.UseFont = true;
            this.TxtMiktar.Properties.Appearance.Options.UseTextOptions = true;
            this.TxtMiktar.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.TxtMiktar.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtMiktar.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtMiktar.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtMiktar.Properties.EditFormat.FormatString = "n2";
            this.TxtMiktar.Size = new System.Drawing.Size(117, 24);
            this.TxtMiktar.TabIndex = 5;
            // 
            // TxtPartiNo
            // 
            this.TxtPartiNo.EditValue = "";
            this.TxtPartiNo.EnterMoveNextControl = true;
            this.TxtPartiNo.Location = new System.Drawing.Point(430, 116);
            this.TxtPartiNo.MyDeger = "";
            this.TxtPartiNo.MyMaxLength = 75;
            this.TxtPartiNo.MyReadOnlymi = false;
            this.TxtPartiNo.Name = "TxtPartiNo";
            this.TxtPartiNo.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtPartiNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtPartiNo.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtPartiNo.Properties.Appearance.Options.UseFont = true;
            this.TxtPartiNo.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtPartiNo.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtPartiNo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtPartiNo.Properties.MaxLength = 75;
            this.TxtPartiNo.Size = new System.Drawing.Size(117, 24);
            this.TxtPartiNo.TabIndex = 6;
            // 
            // LblPartiNo
            // 
            this.LblPartiNo.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.LblPartiNo.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LblPartiNo.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.LblPartiNo.Appearance.Options.UseBorderColor = true;
            this.LblPartiNo.Appearance.Options.UseFont = true;
            this.LblPartiNo.Appearance.Options.UseForeColor = true;
            this.LblPartiNo.Location = new System.Drawing.Point(386, 120);
            this.LblPartiNo.Name = "LblPartiNo";
            this.LblPartiNo.Size = new System.Drawing.Size(41, 16);
            this.LblPartiNo.TabIndex = 58;
            this.LblPartiNo.Text = "PartiNo";
            // 
            // TxtLotNo
            // 
            this.TxtLotNo.EditValue = "0";
            this.TxtLotNo.EnterMoveNextControl = true;
            this.TxtLotNo.Location = new System.Drawing.Point(430, 146);
            this.TxtLotNo.MyDeger = "0";
            this.TxtLotNo.MyEksiGirilebilirmi = false;
            this.TxtLotNo.MyKurusHane = 4;
            this.TxtLotNo.MyMaxLength = 0;
            this.TxtLotNo.MyReadOnlymi = false;
            this.TxtLotNo.Name = "TxtLotNo";
            this.TxtLotNo.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtLotNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtLotNo.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtLotNo.Properties.Appearance.Options.UseFont = true;
            this.TxtLotNo.Properties.Appearance.Options.UseTextOptions = true;
            this.TxtLotNo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.TxtLotNo.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtLotNo.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtLotNo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtLotNo.Properties.EditFormat.FormatString = "n2";
            this.TxtLotNo.Size = new System.Drawing.Size(117, 24);
            this.TxtLotNo.TabIndex = 7;
            // 
            // LblLotNo
            // 
            this.LblLotNo.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.LblLotNo.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LblLotNo.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.LblLotNo.Appearance.Options.UseBorderColor = true;
            this.LblLotNo.Appearance.Options.UseFont = true;
            this.LblLotNo.Appearance.Options.UseForeColor = true;
            this.LblLotNo.Location = new System.Drawing.Point(386, 150);
            this.LblLotNo.Name = "LblLotNo";
            this.LblLotNo.Size = new System.Drawing.Size(17, 16);
            this.LblLotNo.TabIndex = 65;
            this.LblLotNo.Text = "Lot";
            // 
            // myLabel5
            // 
            this.myLabel5.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.myLabel5.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.myLabel5.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.myLabel5.Appearance.Options.UseBorderColor = true;
            this.myLabel5.Appearance.Options.UseFont = true;
            this.myLabel5.Appearance.Options.UseForeColor = true;
            this.myLabel5.Location = new System.Drawing.Point(13, 120);
            this.myLabel5.Name = "myLabel5";
            this.myLabel5.Size = new System.Drawing.Size(58, 16);
            this.myLabel5.TabIndex = 67;
            this.myLabel5.Text = "Giris Depo";
            // 
            // TxtDepoNoGiris
            // 
            this.TxtDepoNoGiris.EditValue = "";
            this.TxtDepoNoGiris.EnterMoveNextControl = true;
            this.TxtDepoNoGiris.Location = new System.Drawing.Point(77, 116);
            this.TxtDepoNoGiris.MyDeger = "";
            this.TxtDepoNoGiris.MyDegerValue = "";
            this.TxtDepoNoGiris.MyMaxLength = 0;
            this.TxtDepoNoGiris.MyReadOnlymi = false;
            this.TxtDepoNoGiris.Name = "TxtDepoNoGiris";
            this.TxtDepoNoGiris.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.TxtDepoNoGiris.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtDepoNoGiris.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtDepoNoGiris.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtDepoNoGiris.Properties.Appearance.Options.UseFont = true;
            this.TxtDepoNoGiris.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtDepoNoGiris.Properties.AppearanceDropDown.Options.UseFont = true;
            this.TxtDepoNoGiris.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtDepoNoGiris.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtDepoNoGiris.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtDepoNoGiris.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.TxtDepoNoGiris.Properties.NullText = "";
            this.TxtDepoNoGiris.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.TxtDepoNoGiris.Size = new System.Drawing.Size(303, 24);
            this.TxtDepoNoGiris.TabIndex = 2;
            // 
            // FrmHizliUretimEG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 250);
            this.Controls.Add(this.TxtDepoNoGiris);
            this.Controls.Add(this.myLabel5);
            this.Controls.Add(this.LblLotNo);
            this.Controls.Add(this.myLabel2);
            this.Controls.Add(this.TxtLotNo);
            this.Controls.Add(this.TxtMiktar);
            this.Controls.Add(this.LblPartiNo);
            this.Controls.Add(this.LblName4);
            this.Controls.Add(this.TxtPartiNo);
            this.Controls.Add(this.TxtBirim1);
            this.Controls.Add(this.LblName7);
            this.Controls.Add(this.LblName3);
            this.Controls.Add(this.TxtStokAdi);
            this.Controls.Add(this.TxtStokKodu);
            this.FormOlusturuldu = true;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmHizliUretimEG.IconOptions.Icon")));
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmHizliUretimEG.IconOptions.Image")));
            this.Name = "FrmHizliUretimEG";
            this.Text = "FrmHizliUretimEG";
            this.Load += new System.EventHandler(this.FrmHizliUretimEG_Load);
            this.Controls.SetChildIndex(this.pblAlt, 0);
            this.Controls.SetChildIndex(this.pnlAltBtn, 0);
            this.Controls.SetChildIndex(this.lblBaslik, 0);
            this.Controls.SetChildIndex(this.pnlUst2, 0);
            this.Controls.SetChildIndex(this.pnlAlt, 0);
            this.Controls.SetChildIndex(this.pnlSol, 0);
            this.Controls.SetChildIndex(this.pnlSag, 0);
            this.Controls.SetChildIndex(this.TxtStokKodu, 0);
            this.Controls.SetChildIndex(this.TxtStokAdi, 0);
            this.Controls.SetChildIndex(this.LblName3, 0);
            this.Controls.SetChildIndex(this.LblName7, 0);
            this.Controls.SetChildIndex(this.TxtBirim1, 0);
            this.Controls.SetChildIndex(this.TxtPartiNo, 0);
            this.Controls.SetChildIndex(this.LblName4, 0);
            this.Controls.SetChildIndex(this.LblPartiNo, 0);
            this.Controls.SetChildIndex(this.TxtMiktar, 0);
            this.Controls.SetChildIndex(this.TxtLotNo, 0);
            this.Controls.SetChildIndex(this.myLabel2, 0);
            this.Controls.SetChildIndex(this.LblLotNo, 0);
            this.Controls.SetChildIndex(this.myLabel5, 0);
            this.Controls.SetChildIndex(this.TxtDepoNoGiris, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtn)).EndInit();
            this.pnlAltBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtnNav)).EndInit();
            this.pnlAltBtnNav.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtn1)).EndInit();
            this.pnlAltBtn1.ResumeLayout(false);
            this.pblAlt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlSag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAlt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlUst2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAltBtn2)).EndInit();
            this.pnlAltBtn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TxtStokAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtStokKodu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtBirim1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtMiktar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPartiNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtLotNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDpGir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDpCik)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtDepoNoGiris.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private My.Kontrol.Kontroller.MyLabel LblName7;
        private My.Kontrol.Kontroller.MyLabel LblName3;
        private My.Kontrol.Kontroller.MyButtonEdit TxtStokAdi;
        private My.Kontrol.Kontroller.MyButtonEdit TxtStokKodu;
        private My.Kontrol.Kontroller.MyLabel LblName4;
        private My.Kontrol.Kontroller.MyTextEdit TxtBirim1;
        private My.Kontrol.Kontroller.MyLabel myLabel2;
        private My.Kontrol.Kontroller.MyTextEditSayi TxtMiktar;
        private My.Kontrol.Kontroller.MyTextEdit TxtPartiNo;
        private My.Kontrol.Kontroller.MyLabel LblPartiNo;
        private My.Kontrol.Kontroller.MyTextEditSayi TxtLotNo;
        private My.Kontrol.Kontroller.MyLabel LblLotNo;
        private System.Windows.Forms.BindingSource bsDpGir;
        private My.Kontrol.Kontroller.MyLabel myLabel5;
        private System.Windows.Forms.BindingSource bsDpCik;
        private My.Kontrol.Kontroller.MyLookupEdit TxtDepoNoGiris;
    }
}