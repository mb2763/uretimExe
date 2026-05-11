using My.Business.Service.Geneller;
using My.Business.Service.Kullanicilar;
using My.Core;
using My.Entities.Kullanicilar;
using My.Kontrol.Formlar;
using System;

namespace MyUI.KullaniciModule
{
    public partial class FrmKullaniciKayit : MyFrmKayit
    {
        private IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private IKullaniciService _srv = Ortak.DbPro.Kullanicilar;
        private Kullanici _mdl;
        public FrmKullaniciKayit()
        {
            InitializeComponent();
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            Bagla();
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
        }
        public void Bagla()
        {
            var rs = _srv.SelectListWhere();
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var data = rs.Data;
            myGrid1.DataSource = null;
            myGrid1.DataSource = data;
            SutunGizle();
            myGrid1.GridYerlesimYukle(myGrid1.MyGridKayitAdi);
        }
        public void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("Sifre");
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            var data = myView1.MyGetCurrentItem<Kullanici>();
            if (data == null)
            {
                return;
            }
            if (SecimIcinAcildi)
            {
                SecilenKod = data.KullaniciAdi;
                SecilenRow = data;
                Secildi = true;
                this.Close();
            }
            else
            {
                _mdl = data.Clone();
                TxtKullaniciAdi.Text = _mdl.KullaniciAdi;
                TxtSifre.Text = string.IsNullOrEmpty(_mdl.Sifre.ToString()) ? "" : _mdl.Sifre.SifreCoz(Ortak.GetKey());
                TxtAdi.Text = _mdl.Adi;
                TxtSoyadi.Text = _mdl.Soyadi;
                ChcAdmin.Checked = _mdl.Admin;
            }
        }
        public void Temizle()
        {
            _mdl = new Kullanici();
            TxtKullaniciAdi.Text = "";
            TxtSifre.Text = "";
            TxtAdi.Text = "";
            TxtSoyadi.Text = "";
            ChcAdmin.Checked = false;
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            _mdl.KullaniciAdi = TxtKullaniciAdi.Text;
            _mdl.Sifre = TxtSifre.Text.Sifrele(Ortak.GetKey());
            _mdl.Adi = TxtAdi.Text;
            _mdl.Soyadi = TxtSoyadi.Text;
            _mdl.Admin = ChcAdmin.Checked;
            var rs = _srv.InsertOrUpdate(_mdl);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            Bagla();
            Temizle();
        }
        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz"))
            {
                return;
            }
            var data = myView1.MyGetCurrentItem<Kullanici>();
            if (data == null)
            {
                return;
            }
            var rs = _srv.Delete(data);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            Bagla();
            Temizle();
        }
        private void BtnDegistir_Click(object sender, EventArgs e)
        {
            var data = myView1.MyGetCurrentItem<Kullanici>();
            if (data == null)
            {
                return;
            }
            _mdl = data.Clone();
            TxtKullaniciAdi.Text = _mdl.KullaniciAdi;
            TxtSifre.Text = string.IsNullOrEmpty(_mdl.Sifre.ToString()) ? "" : _mdl.Sifre.SifreCoz(Ortak.GetKey());
            TxtAdi.Text = _mdl.Adi;
            TxtSoyadi.Text = _mdl.Soyadi;
            ChcAdmin.Checked = _mdl.Admin;
        }
        private void BtnYeni_Click(object sender, EventArgs e)
        {
            Temizle();
        }
    }
}