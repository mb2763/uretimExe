using My.Business;
using My.Core;
using My.Entities.Kullanicilar;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace MyUI
{
    public partial class FrmLogin : MyFrmLoginPaneli
    {
        public bool GirisYapildi = false;
        public string KullaniciKodu = "";
        public Kullanici _rw;
        private string _aesMasterKey = "";
        private string ProDbAdi = My.Core.ProgramYolAyarlari.DbProAdi;
        private string MikroDbAdi = My.Core.ProgramYolAyarlari.DbMikroAdi;
        public FrmLogin()
        {
            InitializeComponent();
            this.Load += new EventHandler(this.MyFrmGiris_Load);
            BtnDbPro.Click += new EventHandler(this.BtnDbPro_Click);
            BtnDbMikro.Click += new EventHandler(this.BtnDbMikro_Click);
            BtnGiris.Click += new EventHandler(this.BtnGiris_Click);
            BtnKapat.Click += new EventHandler(this.BtnKapat_Click);
            this.TxtKullanici.KeyDown += new KeyEventHandler(this.TxtKullanici_KeyDown);
            this.TxtSifre.KeyDown += new KeyEventHandler(this.TxtSifre_KeyDown);
        }
        private void MyFrmGiris_Load(object sender, EventArgs e)
        {
            BtnDbPro.Visible = false;
            BtnDbMikro.Visible = false;
            LblVersiyon.Text = GetVersiyon();
            _aesMasterKey = Ortak.GetKey();
            HataKontrolu(() =>
           {
               Ortak.DbPro = new DatabaseFactoryPro(_aesMasterKey, ProDbAdi);
               Ortak.DbMikro = new DatabaseFactoryMikro(_aesMasterKey, MikroDbAdi);
               TxtKullanici.Text = Ortak.AyarIni.Oku("AYAR", "KULLANICI", "");
               TxtSifre.Focus();
           });
        }
        string GetVersiyon() {
            string versiyon = "0";
            if (File.Exists("versiyon.txt")) {
                versiyon = File.ReadAllText("versiyon.txt");
            }
            return "V:" + versiyon;
        }
        public static void HataKontrolu(Action action)
        {
            GetKontrol(() => { action(); });
        }
        private void BtnDbPro_Click(object sender, EventArgs e)
        {
            HataKontrolu(() =>
            {
                //FrmDataPaneli frm = new FrmDataPaneli(ProDbAdi);
                //frm.ShowDialog();
                //if (frm.AyarDegisti)
                //{
                //    Ortak.DbPro = new DatabaseFactoryPro(Ortak.ANA_KEY, ProDbAdi);
                //}
            });
        }
        private void BtnDbMikro_Click(object sender, EventArgs e)
        {
            HataKontrolu(() =>
           {
               //FrmDataPaneli frm = new FrmDataPaneli(MikroDbAdi);
               //frm.ShowDialog();
               //if (frm.AyarDegisti)
               //{
               //    Ortak.DbMikro = new DatabaseFactoryMikro(Ortak.ANA_KEY, MikroDbAdi);
               //}
           });
        }
        private void BtnGiris_Click(object sender, EventArgs e)
        {
            if (Kontrol() == true)
            {
                GirisYapildi = true;
                Ortak.AyarIni.Yaz("AYAR", "KULLANICI", TxtKullanici.Text);
                KullaniciKodu = TxtKullanici.Text.ToString();
                this.Close();
            }
            else
            {
                MessageBox.Show("Kullanıcı Adı Veya Şifre Geçersiz", "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void BtnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool Kontrol()
        {
            var ad = Ortak.DbPro.Kullanicilar.Query<int>("select count(*) as Adet from Kullanici ", null);
            if (ad.Success && ad.Data.Count() > 0)
            {
                if (ad.Data.ToList().FirstOrDefault() <= 0)
                {
                    Ortak.DbPro.Kullanicilar.Insert(new Kullanici() { KullaniciAdi = "Admin", Adi = "Admin", Id = MyGuid.NewGuid(), Sifre = "1".Sifrele(_aesMasterKey), Soyadi = "User", Admin = true });
                }
            }
            string ka = TxtKullanici.Text.ToString().ToUpper();
            string ps = TxtSifre.Text.ToString().ToUpper();
            string sif = ps.Sifrele(_aesMasterKey);
            var res = Ortak.DbPro.Kullanicilar.SelectFirst(k => k.KullaniciAdi == ka);
            if (res.Success)
            {
                _rw = res.Data;
                if (_rw == null)
                {
                    MessageBox.Show("Kullanici Bulunamadı");
                    return false;
                }
                if (ka == _rw.KullaniciAdi.ToUpper() && sif == _rw.Sifre)
                {
                    Ortak.KullaniciAdi = ka;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show(res.Message);
                return false;
            }
        }
        private void TxtSifre_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter))
            {
                if (Kontrol() == true)
                {
                    GirisYapildi = true;
                    Ortak.AyarIni.Yaz("AYAR", "KULLANICI", TxtKullanici.Text);
                    KullaniciKodu = TxtKullanici.Text.ToString();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kullanıcı Adı Veya Şifre Geçersiz", "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void TxtKullanici_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TxtSifre.Focus();
                TxtSifre.SelectAll();
            }
        }

        private void LblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}