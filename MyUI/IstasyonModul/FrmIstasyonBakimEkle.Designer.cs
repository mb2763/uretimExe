namespace MyUI.IstasyonModul {
    partial class FrmIstasyonBakimEkle {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmIstasyonBakimEkle));
            this.myGroupControl1 = new My.Kontrol.Kontroller.MyGroupControl();
            this.TxtTarih = new My.Kontrol.Kontroller.MyDateEdit();
            this.myLabel5 = new My.Kontrol.Kontroller.MyLabel();
            this.TxtAciklama = new My.Kontrol.Kontroller.MyTextEdit();
            this.TxtIslemTuru = new My.Kontrol.Kontroller.MyTextEdit();
            this.TxtPersonel = new My.Kontrol.Kontroller.MyTextEdit();
            this.CmbIstasyonKodu = new My.Kontrol.Kontroller.MyComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.myTabPane1 = new My.Kontrol.Kontroller.MyTabPane();
            this.tabNavigationPage1 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.myGrid1 = new My.Kontrol.Kontroller.MyGrid();
            this.myView1 = new My.Kontrol.Kontroller.MyView();
            this.myPanelControl1 = new My.Kontrol.Kontroller.MyPanelControl();
            this.BtnStokEkle = new My.Kontrol.Kontroller.MyButton();
            this.BtnStokSil = new My.Kontrol.Kontroller.MyButton();
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
            ((System.ComponentModel.ISupportInitialize)(this.myGroupControl1)).BeginInit();
            this.myGroupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtTarih.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtTarih.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtAciklama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtIslemTuru.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPersonel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbIstasyonKodu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myTabPane1)).BeginInit();
            this.myTabPane1.SuspendLayout();
            this.tabNavigationPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myPanelControl1)).BeginInit();
            this.myPanelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAltBtn
            // 
            this.pnlAltBtn.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.pnlAltBtn.Appearance.Options.UseBorderColor = true;
            this.pnlAltBtn.Location = new System.Drawing.Point(0, 541);
            this.pnlAltBtn.Size = new System.Drawing.Size(590, 37);
            // 
            // pnlAltBtn1
            // 
            this.pnlAltBtn1.Location = new System.Drawing.Point(166, 2);
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
            this.BtnSil.Click += new System.EventHandler(this.BtnSil_Click);
            // 
            // lblBaslik
            // 
            this.lblBaslik.Size = new System.Drawing.Size(590, 40);
            this.lblBaslik.Text = "Istasyon Bakım";
            // 
            // pblAlt
            // 
            this.pblAlt.Location = new System.Drawing.Point(0, 578);
            this.pblAlt.Size = new System.Drawing.Size(590, 27);
            // 
            // lbl_bilgi
            // 
            this.lbl_bilgi.Location = new System.Drawing.Point(416, 0);
            // 
            // pnlSag
            // 
            this.pnlSag.Location = new System.Drawing.Point(579, 50);
            this.pnlSag.Size = new System.Drawing.Size(11, 481);
            // 
            // pnlSol
            // 
            this.pnlSol.Size = new System.Drawing.Size(10, 481);
            // 
            // pnlAlt
            // 
            this.pnlAlt.Location = new System.Drawing.Point(0, 531);
            this.pnlAlt.Size = new System.Drawing.Size(590, 10);
            // 
            // pnlUst2
            // 
            this.pnlUst2.Size = new System.Drawing.Size(590, 10);
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
            // myGroupControl1
            // 
            this.myGroupControl1.Controls.Add(this.TxtTarih);
            this.myGroupControl1.Controls.Add(this.myLabel5);
            this.myGroupControl1.Controls.Add(this.TxtAciklama);
            this.myGroupControl1.Controls.Add(this.TxtIslemTuru);
            this.myGroupControl1.Controls.Add(this.TxtPersonel);
            this.myGroupControl1.Controls.Add(this.CmbIstasyonKodu);
            this.myGroupControl1.Controls.Add(this.label6);
            this.myGroupControl1.Controls.Add(this.label1);
            this.myGroupControl1.Controls.Add(this.label4);
            this.myGroupControl1.Controls.Add(this.label3);
            this.myGroupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.myGroupControl1.Location = new System.Drawing.Point(10, 50);
            this.myGroupControl1.Name = "myGroupControl1";
            this.myGroupControl1.Size = new System.Drawing.Size(569, 151);
            this.myGroupControl1.TabIndex = 0;
            // 
            // TxtTarih
            // 
            this.TxtTarih.EditValue = "25.05.2023";
            this.TxtTarih.EnterMoveNextControl = true;
            this.TxtTarih.Location = new System.Drawing.Point(382, 32);
            this.TxtTarih.MyDeger = "25.05.2023";
            this.TxtTarih.MyReadOnlymi = false;
            this.TxtTarih.Name = "TxtTarih";
            this.TxtTarih.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtTarih.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtTarih.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtTarih.Properties.Appearance.Options.UseFont = true;
            this.TxtTarih.Properties.Appearance.Options.UseTextOptions = true;
            this.TxtTarih.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TxtTarih.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtTarih.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtTarih.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtTarih.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.TxtTarih.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.TxtTarih.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.TxtTarih.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.TxtTarih.Properties.EditFormat.FormatString = "dd.MM.yyyy";
            this.TxtTarih.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.TxtTarih.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.DateTimeMaskManager));
            this.TxtTarih.Properties.MaskSettings.Set("useAdvancingCaret", true);
            this.TxtTarih.Properties.MaskSettings.Set("mask", "dd.MM.yyyy");
            this.TxtTarih.Size = new System.Drawing.Size(176, 24);
            this.TxtTarih.TabIndex = 1;
            // 
            // myLabel5
            // 
            this.myLabel5.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.myLabel5.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.myLabel5.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.myLabel5.Appearance.Options.UseBorderColor = true;
            this.myLabel5.Appearance.Options.UseFont = true;
            this.myLabel5.Appearance.Options.UseForeColor = true;
            this.myLabel5.Location = new System.Drawing.Point(343, 33);
            this.myLabel5.Name = "myLabel5";
            this.myLabel5.Size = new System.Drawing.Size(33, 16);
            this.myLabel5.TabIndex = 26;
            this.myLabel5.Text = "Tarihi";
            // 
            // TxtAciklama
            // 
            this.TxtAciklama.EditValue = "";
            this.TxtAciklama.EnterMoveNextControl = true;
            this.TxtAciklama.Location = new System.Drawing.Point(100, 121);
            this.TxtAciklama.MyDeger = "";
            this.TxtAciklama.MyMaxLength = 75;
            this.TxtAciklama.MyReadOnlymi = false;
            this.TxtAciklama.Name = "TxtAciklama";
            this.TxtAciklama.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtAciklama.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtAciklama.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtAciklama.Properties.Appearance.Options.UseFont = true;
            this.TxtAciklama.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtAciklama.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtAciklama.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtAciklama.Properties.MaxLength = 75;
            this.TxtAciklama.Size = new System.Drawing.Size(458, 24);
            this.TxtAciklama.TabIndex = 4;
            // 
            // TxtIslemTuru
            // 
            this.TxtIslemTuru.EditValue = "";
            this.TxtIslemTuru.EnterMoveNextControl = true;
            this.TxtIslemTuru.Location = new System.Drawing.Point(100, 91);
            this.TxtIslemTuru.MyDeger = "";
            this.TxtIslemTuru.MyMaxLength = 75;
            this.TxtIslemTuru.MyReadOnlymi = false;
            this.TxtIslemTuru.Name = "TxtIslemTuru";
            this.TxtIslemTuru.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtIslemTuru.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtIslemTuru.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtIslemTuru.Properties.Appearance.Options.UseFont = true;
            this.TxtIslemTuru.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtIslemTuru.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtIslemTuru.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtIslemTuru.Properties.MaxLength = 75;
            this.TxtIslemTuru.Size = new System.Drawing.Size(458, 24);
            this.TxtIslemTuru.TabIndex = 3;
            // 
            // TxtPersonel
            // 
            this.TxtPersonel.EditValue = "";
            this.TxtPersonel.EnterMoveNextControl = true;
            this.TxtPersonel.Location = new System.Drawing.Point(100, 60);
            this.TxtPersonel.MyDeger = "";
            this.TxtPersonel.MyMaxLength = 75;
            this.TxtPersonel.MyReadOnlymi = false;
            this.TxtPersonel.Name = "TxtPersonel";
            this.TxtPersonel.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtPersonel.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtPersonel.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtPersonel.Properties.Appearance.Options.UseFont = true;
            this.TxtPersonel.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtPersonel.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtPersonel.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtPersonel.Properties.MaxLength = 75;
            this.TxtPersonel.Size = new System.Drawing.Size(171, 24);
            this.TxtPersonel.TabIndex = 2;
            // 
            // CmbIstasyonKodu
            // 
            this.CmbIstasyonKodu.EditValue = "";
            this.CmbIstasyonKodu.EnterMoveNextControl = true;
            this.CmbIstasyonKodu.Location = new System.Drawing.Point(100, 30);
            this.CmbIstasyonKodu.MyDeger = "";
            this.CmbIstasyonKodu.MyMaxLength = 0;
            this.CmbIstasyonKodu.MyReadOnlymi = false;
            this.CmbIstasyonKodu.Name = "CmbIstasyonKodu";
            this.CmbIstasyonKodu.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.CmbIstasyonKodu.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.CmbIstasyonKodu.Properties.Appearance.Options.UseBorderColor = true;
            this.CmbIstasyonKodu.Properties.Appearance.Options.UseFont = true;
            this.CmbIstasyonKodu.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 11F);
            this.CmbIstasyonKodu.Properties.AppearanceDropDown.Options.UseFont = true;
            this.CmbIstasyonKodu.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.CmbIstasyonKodu.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.CmbIstasyonKodu.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.CmbIstasyonKodu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbIstasyonKodu.Size = new System.Drawing.Size(171, 24);
            this.CmbIstasyonKodu.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "IstasyonKodu";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bakım/Islem Turu";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Açıklaması";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Personel";
            // 
            // myTabPane1
            // 
            this.myTabPane1.Controls.Add(this.tabNavigationPage1);
            this.myTabPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myTabPane1.Location = new System.Drawing.Point(10, 201);
            this.myTabPane1.Name = "myTabPane1";
            this.myTabPane1.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.tabNavigationPage1});
            this.myTabPane1.RegularSize = new System.Drawing.Size(569, 330);
            this.myTabPane1.SelectedPage = this.tabNavigationPage1;
            this.myTabPane1.Size = new System.Drawing.Size(569, 330);
            this.myTabPane1.TabIndex = 30;
            this.myTabPane1.Text = "myTabPane1";
            // 
            // tabNavigationPage1
            // 
            this.tabNavigationPage1.Caption = "Parcalar";
            this.tabNavigationPage1.Controls.Add(this.myGrid1);
            this.tabNavigationPage1.Controls.Add(this.myPanelControl1);
            this.tabNavigationPage1.Name = "tabNavigationPage1";
            this.tabNavigationPage1.Size = new System.Drawing.Size(569, 293);
            // 
            // myGrid1
            // 
            this.myGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myGrid1.Location = new System.Drawing.Point(0, 0);
            this.myGrid1.LookAndFeel.SkinName = "DevExpress Style";
            this.myGrid1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.myGrid1.MainView = this.myView1;
            this.myGrid1.MyContextMenu = null;
            this.myGrid1.MyGridKayitAdi = "IstasyonBakimEkleParcalar";
            this.myGrid1.Name = "myGrid1";
            this.myGrid1.Size = new System.Drawing.Size(518, 293);
            this.myGrid1.TabIndex = 33;
            this.myGrid1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.myView1});
            // 
            // myView1
            // 
            this.myView1.Appearance.EvenRow.BackColor = System.Drawing.Color.AliceBlue;
            this.myView1.Appearance.EvenRow.Options.UseBackColor = true;
            this.myView1.Appearance.FocusedRow.BackColor = System.Drawing.Color.CornflowerBlue;
            this.myView1.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.myView1.Appearance.FocusedRow.Options.UseBackColor = true;
            this.myView1.Appearance.FocusedRow.Options.UseForeColor = true;
            this.myView1.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.myView1.Appearance.FooterPanel.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.myView1.Appearance.FooterPanel.Options.UseFont = true;
            this.myView1.Appearance.FooterPanel.Options.UseForeColor = true;
            this.myView1.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.myView1.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.myView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.myView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.myView1.Appearance.OddRow.BackColor = System.Drawing.Color.MintCream;
            this.myView1.Appearance.OddRow.Options.UseBackColor = true;
            this.myView1.Appearance.ViewCaption.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.myView1.Appearance.ViewCaption.Options.UseForeColor = true;
            this.myView1.ColumnPanelRowHeight = 27;
            this.myView1.GridControl = this.myGrid1;
            this.myView1.MyBaslik = "";
            this.myView1.MyKurusHane = 4;
            this.myView1.MyRowIdsi = "";
            this.myView1.MyTarihFormat = "dd.MM.yyyy";
            this.myView1.Name = "myView1";
            this.myView1.OptionsNavigation.EnterMoveNextColumn = true;
            this.myView1.OptionsPrint.AutoWidth = false;
            this.myView1.OptionsPrint.PrintFooter = false;
            this.myView1.OptionsPrint.PrintGroupFooter = false;
            this.myView1.OptionsView.ColumnAutoWidth = false;
            this.myView1.OptionsView.EnableAppearanceEvenRow = true;
            this.myView1.OptionsView.EnableAppearanceOddRow = true;
            this.myView1.OptionsView.RowAutoHeight = true;
            this.myView1.OptionsView.ShowGroupPanel = false;
            this.myView1.RowHeight = 27;
            // 
            // myPanelControl1
            // 
            this.myPanelControl1.Controls.Add(this.BtnStokEkle);
            this.myPanelControl1.Controls.Add(this.BtnStokSil);
            this.myPanelControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.myPanelControl1.Location = new System.Drawing.Point(518, 0);
            this.myPanelControl1.Name = "myPanelControl1";
            this.myPanelControl1.Size = new System.Drawing.Size(51, 293);
            this.myPanelControl1.TabIndex = 0;
            // 
            // BtnStokEkle
            // 
            this.BtnStokEkle.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BtnStokEkle.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.BtnStokEkle.Appearance.Options.UseFont = true;
            this.BtnStokEkle.Appearance.Options.UseForeColor = true;
            this.BtnStokEkle.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnStokEkle.ImageOptions.Image")));
            this.BtnStokEkle.Location = new System.Drawing.Point(3, 5);
            this.BtnStokEkle.Name = "BtnStokEkle";
            this.BtnStokEkle.Size = new System.Drawing.Size(46, 44);
            this.BtnStokEkle.TabIndex = 0;
            this.BtnStokEkle.Text = "Ekle";
            this.BtnStokEkle.Click += new System.EventHandler(this.BtnStokEkle_Click);
            // 
            // BtnStokSil
            // 
            this.BtnStokSil.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BtnStokSil.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.BtnStokSil.Appearance.Options.UseFont = true;
            this.BtnStokSil.Appearance.Options.UseForeColor = true;
            this.BtnStokSil.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnStokSil.ImageOptions.Image")));
            this.BtnStokSil.Location = new System.Drawing.Point(3, 55);
            this.BtnStokSil.Name = "BtnStokSil";
            this.BtnStokSil.Size = new System.Drawing.Size(46, 38);
            this.BtnStokSil.TabIndex = 1;
            this.BtnStokSil.Text = "Sil";
            this.BtnStokSil.Click += new System.EventHandler(this.BtnStokSil_Click);
            // 
            // FrmIstasyonBakimEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 605);
            this.Controls.Add(this.myTabPane1);
            this.Controls.Add(this.myGroupControl1);
            this.FormOlusturuldu = true;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmIstasyonBakimEkle.IconOptions.Icon")));
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmIstasyonBakimEkle.IconOptions.Image")));
            this.Name = "FrmIstasyonBakimEkle";
            this.Text = "FrmIstasyonBakimEkle";
            this.Controls.SetChildIndex(this.pblAlt, 0);
            this.Controls.SetChildIndex(this.pnlAltBtn, 0);
            this.Controls.SetChildIndex(this.lblBaslik, 0);
            this.Controls.SetChildIndex(this.pnlUst2, 0);
            this.Controls.SetChildIndex(this.pnlAlt, 0);
            this.Controls.SetChildIndex(this.pnlSol, 0);
            this.Controls.SetChildIndex(this.pnlSag, 0);
            this.Controls.SetChildIndex(this.myGroupControl1, 0);
            this.Controls.SetChildIndex(this.myTabPane1, 0);
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
            ((System.ComponentModel.ISupportInitialize)(this.myGroupControl1)).EndInit();
            this.myGroupControl1.ResumeLayout(false);
            this.myGroupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtTarih.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtTarih.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtAciklama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtIslemTuru.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPersonel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbIstasyonKodu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myTabPane1)).EndInit();
            this.myTabPane1.ResumeLayout(false);
            this.tabNavigationPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.myGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myPanelControl1)).EndInit();
            this.myPanelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private My.Kontrol.Kontroller.MyGroupControl myGroupControl1;
        private My.Kontrol.Kontroller.MyTextEdit TxtAciklama;
        private My.Kontrol.Kontroller.MyTextEdit TxtPersonel;
        private My.Kontrol.Kontroller.MyComboBox CmbIstasyonKodu;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private My.Kontrol.Kontroller.MyTabPane myTabPane1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage1;
        private My.Kontrol.Kontroller.MyPanelControl myPanelControl1;
        private My.Kontrol.Kontroller.MyButton BtnStokEkle;
        private My.Kontrol.Kontroller.MyButton BtnStokSil;
        private My.Kontrol.Kontroller.MyGrid myGrid1;
        private My.Kontrol.Kontroller.MyView myView1;
        private My.Kontrol.Kontroller.MyDateEdit TxtTarih;
        private My.Kontrol.Kontroller.MyLabel myLabel5;
        private My.Kontrol.Kontroller.MyTextEdit TxtIslemTuru;
        private System.Windows.Forms.Label label1;
    }
}