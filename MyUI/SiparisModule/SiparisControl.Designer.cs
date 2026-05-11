
namespace MyUI.SiparisModule
{
    partial class SiparisControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.TxtCinsi1 = new My.Kontrol.Kontroller.MyGroupControl();
            this.TxtAciklama = new My.Kontrol.Kontroller.MyTextEdit();
            this.TxtMiktar = new My.Kontrol.Kontroller.MyTextEditSayi();
            this.TxtBirim = new My.Kontrol.Kontroller.MyTextEdit();
            this.CmbBeden = new My.Kontrol.Kontroller.MyComboBox();
            this.CmbRenk = new My.Kontrol.Kontroller.MyComboBox();
            this.CmbStokAdi = new My.Kontrol.Kontroller.MyLookupEdit();
            this.CmbStokKodu = new My.Kontrol.Kontroller.MyLookupEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.TxtCinsi1)).BeginInit();
            this.TxtCinsi1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtAciklama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtMiktar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtBirim.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbBeden.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbRenk.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbStokAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbStokKodu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            this.SuspendLayout();
            // 
            // TxtCinsi1
            // 
            this.TxtCinsi1.Controls.Add(this.TxtAciklama);
            this.TxtCinsi1.Controls.Add(this.TxtMiktar);
            this.TxtCinsi1.Controls.Add(this.TxtBirim);
            this.TxtCinsi1.Controls.Add(this.CmbBeden);
            this.TxtCinsi1.Controls.Add(this.CmbRenk);
            this.TxtCinsi1.Controls.Add(this.CmbStokAdi);
            this.TxtCinsi1.Controls.Add(this.CmbStokKodu);
            this.TxtCinsi1.Controls.Add(this.label6);
            this.TxtCinsi1.Controls.Add(this.label8);
            this.TxtCinsi1.Controls.Add(this.label5);
            this.TxtCinsi1.Controls.Add(this.label7);
            this.TxtCinsi1.Controls.Add(this.label4);
            this.TxtCinsi1.Controls.Add(this.label3);
            this.TxtCinsi1.Controls.Add(this.label2);
            this.TxtCinsi1.Controls.Add(this.label1);
            this.TxtCinsi1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtCinsi1.Location = new System.Drawing.Point(0, 0);
            this.TxtCinsi1.Name = "TxtCinsi1";
            this.TxtCinsi1.Size = new System.Drawing.Size(309, 238);
            this.TxtCinsi1.TabIndex = 0;
            this.TxtCinsi1.Text = "...";
            // 
            // TxtAciklama
            // 
            this.TxtAciklama.EditValue = "";
            this.TxtAciklama.EnterMoveNextControl = true;
            this.TxtAciklama.Location = new System.Drawing.Point(105, 211);
            this.TxtAciklama.MyDeger = "";
            this.TxtAciklama.MyMaxLength = 150;
            this.TxtAciklama.MyReadOnlymi = false;
            this.TxtAciklama.Name = "TxtAciklama";
            this.TxtAciklama.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtAciklama.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtAciklama.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtAciklama.Properties.Appearance.Options.UseFont = true;
            this.TxtAciklama.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtAciklama.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtAciklama.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtAciklama.Properties.MaxLength = 150;
            this.TxtAciklama.Size = new System.Drawing.Size(196, 24);
            this.TxtAciklama.TabIndex = 25;
            // 
            // TxtMiktar
            // 
            this.TxtMiktar.EditValue = "0";
            this.TxtMiktar.EnterMoveNextControl = true;
            this.TxtMiktar.Location = new System.Drawing.Point(105, 181);
            this.TxtMiktar.MyDeger = "0";
            this.TxtMiktar.MyEksiGirilebilirmi = false;
            this.TxtMiktar.MyKurusHane = 2;
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
            this.TxtMiktar.Size = new System.Drawing.Size(196, 24);
            this.TxtMiktar.TabIndex = 24;
            // 
            // TxtBirim
            // 
            this.TxtBirim.EditValue = "";
            this.TxtBirim.EnterMoveNextControl = true;
            this.TxtBirim.Location = new System.Drawing.Point(105, 151);
            this.TxtBirim.MyDeger = "";
            this.TxtBirim.MyMaxLength = 75;
            this.TxtBirim.MyReadOnlymi = false;
            this.TxtBirim.Name = "TxtBirim";
            this.TxtBirim.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.TxtBirim.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TxtBirim.Properties.Appearance.Options.UseBorderColor = true;
            this.TxtBirim.Properties.Appearance.Options.UseFont = true;
            this.TxtBirim.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.TxtBirim.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.TxtBirim.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.TxtBirim.Properties.MaxLength = 75;
            this.TxtBirim.Size = new System.Drawing.Size(196, 24);
            this.TxtBirim.TabIndex = 23;
            // 
            // CmbBeden
            // 
            this.CmbBeden.EditValue = "";
            this.CmbBeden.EnterMoveNextControl = true;
            this.CmbBeden.Location = new System.Drawing.Point(105, 121);
            this.CmbBeden.MyDeger = "";
            this.CmbBeden.MyMaxLength = 0;
            this.CmbBeden.MyReadOnlymi = false;
            this.CmbBeden.Name = "CmbBeden";
            this.CmbBeden.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.CmbBeden.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.CmbBeden.Properties.Appearance.Options.UseBorderColor = true;
            this.CmbBeden.Properties.Appearance.Options.UseFont = true;
            this.CmbBeden.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 11F);
            this.CmbBeden.Properties.AppearanceDropDown.Options.UseFont = true;
            this.CmbBeden.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.CmbBeden.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.CmbBeden.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.CmbBeden.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbBeden.Size = new System.Drawing.Size(196, 24);
            this.CmbBeden.TabIndex = 22;
            // 
            // CmbRenk
            // 
            this.CmbRenk.EditValue = "";
            this.CmbRenk.EnterMoveNextControl = true;
            this.CmbRenk.Location = new System.Drawing.Point(105, 91);
            this.CmbRenk.MyDeger = "";
            this.CmbRenk.MyMaxLength = 0;
            this.CmbRenk.MyReadOnlymi = false;
            this.CmbRenk.Name = "CmbRenk";
            this.CmbRenk.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.CmbRenk.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.CmbRenk.Properties.Appearance.Options.UseBorderColor = true;
            this.CmbRenk.Properties.Appearance.Options.UseFont = true;
            this.CmbRenk.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 11F);
            this.CmbRenk.Properties.AppearanceDropDown.Options.UseFont = true;
            this.CmbRenk.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.CmbRenk.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.CmbRenk.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.CmbRenk.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbRenk.Size = new System.Drawing.Size(196, 24);
            this.CmbRenk.TabIndex = 22;
            // 
            // CmbStokAdi
            // 
            this.CmbStokAdi.EditValue = "";
            this.CmbStokAdi.EnterMoveNextControl = true;
            this.CmbStokAdi.Location = new System.Drawing.Point(105, 61);
            this.CmbStokAdi.MyDeger = "";
            this.CmbStokAdi.MyDegerValue = "";
            this.CmbStokAdi.MyMaxLength = 0;
            this.CmbStokAdi.MyReadOnlymi = false;
            this.CmbStokAdi.Name = "CmbStokAdi";
            this.CmbStokAdi.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.CmbStokAdi.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.CmbStokAdi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.CmbStokAdi.Properties.Appearance.Options.UseBorderColor = true;
            this.CmbStokAdi.Properties.Appearance.Options.UseFont = true;
            this.CmbStokAdi.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 10F);
            this.CmbStokAdi.Properties.AppearanceDropDown.Options.UseFont = true;
            this.CmbStokAdi.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.CmbStokAdi.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.CmbStokAdi.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.CmbStokAdi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbStokAdi.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("StokKodu", "StokKodu", 150, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("StokAdi", "StokAdi", 250, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.CmbStokAdi.Properties.NullText = "";
            this.CmbStokAdi.Properties.PopupFormMinSize = new System.Drawing.Size(400, 0);
            this.CmbStokAdi.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;
            this.CmbStokAdi.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.CmbStokAdi.Size = new System.Drawing.Size(196, 24);
            this.CmbStokAdi.TabIndex = 21;
            // 
            // CmbStokKodu
            // 
            this.CmbStokKodu.EditValue = "";
            this.CmbStokKodu.EnterMoveNextControl = true;
            this.CmbStokKodu.Location = new System.Drawing.Point(105, 28);
            this.CmbStokKodu.MyDeger = "";
            this.CmbStokKodu.MyDegerValue = "";
            this.CmbStokKodu.MyMaxLength = 0;
            this.CmbStokKodu.MyReadOnlymi = false;
            this.CmbStokKodu.Name = "CmbStokKodu";
            this.CmbStokKodu.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.CmbStokKodu.Properties.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.CmbStokKodu.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.CmbStokKodu.Properties.Appearance.Options.UseBorderColor = true;
            this.CmbStokKodu.Properties.Appearance.Options.UseFont = true;
            this.CmbStokKodu.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 10F);
            this.CmbStokKodu.Properties.AppearanceDropDown.Options.UseFont = true;
            this.CmbStokKodu.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MintCream;
            this.CmbStokKodu.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.CmbStokKodu.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.CmbStokKodu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbStokKodu.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("StokKodu", "StokKodu", 150, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("StokAdi", "StokAdi", 250, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.CmbStokKodu.Properties.NullText = "";
            this.CmbStokKodu.Properties.PopupFormMinSize = new System.Drawing.Size(400, 0);
            this.CmbStokKodu.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;
            this.CmbStokKodu.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.CmbStokKodu.Size = new System.Drawing.Size(196, 24);
            this.CmbStokKodu.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.ForeColor = System.Drawing.Color.DimGray;
            this.label6.Location = new System.Drawing.Point(5, 211);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 14);
            this.label6.TabIndex = 20;
            this.label6.Text = "Açıklama";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label8.ForeColor = System.Drawing.Color.DimGray;
            this.label8.Location = new System.Drawing.Point(5, 122);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 14);
            this.label8.TabIndex = 20;
            this.label8.Text = "Beden";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(5, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 14);
            this.label5.TabIndex = 20;
            this.label5.Text = "Miktar";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(5, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 14);
            this.label7.TabIndex = 20;
            this.label7.Text = "Renk";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(5, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 14);
            this.label4.TabIndex = 20;
            this.label4.Text = "Birim";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(5, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 14);
            this.label3.TabIndex = 20;
            this.label3.Text = "Renk";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(5, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 14);
            this.label2.TabIndex = 20;
            this.label2.Text = "Stok Adı";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(5, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 14);
            this.label1.TabIndex = 20;
            this.label1.Text = "Stok Kodu";
            // 
            // SiparisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TxtCinsi1);
            this.Name = "SiparisControl";
            this.Size = new System.Drawing.Size(309, 238);
            ((System.ComponentModel.ISupportInitialize)(this.TxtCinsi1)).EndInit();
            this.TxtCinsi1.ResumeLayout(false);
            this.TxtCinsi1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtAciklama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtMiktar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtBirim.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbBeden.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbRenk.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbStokAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbStokKodu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private My.Kontrol.Kontroller.MyGroupControl TxtCinsi1;
        private My.Kontrol.Kontroller.MyTextEdit TxtAciklama;
        private My.Kontrol.Kontroller.MyTextEditSayi TxtMiktar;
        private My.Kontrol.Kontroller.MyTextEdit TxtBirim;
        private My.Kontrol.Kontroller.MyComboBox CmbRenk;
        private My.Kontrol.Kontroller.MyLookupEdit CmbStokAdi;
        private My.Kontrol.Kontroller.MyLookupEdit CmbStokKodu;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bs;
        private My.Kontrol.Kontroller.MyComboBox CmbBeden;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}
