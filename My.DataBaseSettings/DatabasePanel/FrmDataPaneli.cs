
using System;
using System.IO;
using My.Core;
using My.Core.Helpers; 

namespace My.DataPaneli
{
    public partial class FrmDataPaneli : MyFrmDataPaneli
    {
        const string AnaKey = "OzelAnahtar1234%&";
        
        static IniDosyasi AyarIni = new IniDosyasi(My.Core.ProgramYolAyarlari.AyarDosyasiYol);


        static IniDosyasi DatabaseIni = new IniDosyasi(My.Core.ProgramYolAyarlari.DataBaseAyarDosyasiYol);
        public static void KlasorleriOlustur()
        {
            if (!Directory.Exists(My.Core.ProgramYolAyarlari.AyarKlasoru))
            {
                Directory.CreateDirectory(My.Core.ProgramYolAyarlari.AyarKlasoru);
            }
            if (!Directory.Exists(My.Core.ProgramYolAyarlari.GridAyarKlasoru))
            {
                Directory.CreateDirectory(My.Core.ProgramYolAyarlari.GridAyarKlasoru);
            }
        }
        public FrmDataPaneli(string dbadi) : base(dbadi)
        {
            InitializeComponent();
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            this.Load += new System.EventHandler(this.MyFrmFirebirdDataPaneli_Load);
        }
        private void MyFrmFirebirdDataPaneli_Load(object sender, EventArgs e)
        {
            KlasorleriOlustur();
            if ( AyarIni == null)
            {    
                 AyarIni = new IniDosyasi(My.Core.ProgramYolAyarlari.AyarDosyasiYol);
            }    
            if ( DatabaseIni == null)
            {    
                 DatabaseIni = new IniDosyasi(My.Core.ProgramYolAyarlari.DataBaseAyarDosyasiYol);
            }
            lblBaslik.Text = DbAdi + " Database Olustur";
            IniOku();
        }
        private void IniOku()
        {
            GetKontrol(() =>
            {
                var db = DbConnectionSettings.GetSetting(DbAdi, AnaKey);
                txtDataBase.Text = db.Database;
                txtIp.Text = db.Server;
                txtKullaniciAdi.Text = db.UserName;
                txtSifre.Text = db.Password;
            });
        }
        public void SaveSettings()
        {
            DatabaseModel mdl = new DatabaseModel(DbAdi);
            mdl.Database = txtDataBase.Text;
            mdl.Server = txtIp.Text;
            mdl.UserName = txtKullaniciAdi.Text;
            mdl.Password = txtSifre.Text;
            DbConnectionSettings.SaveSetting(mdl, AnaKey);
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            SaveSettings();
            AyarDegisti = true;
        }
    }
}
