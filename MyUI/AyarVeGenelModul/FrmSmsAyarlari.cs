using My.Business.Service.SmsAyarlar;
using My.Core;
using My.Entities.SmsAyarlar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.AyarVeGenelModul {
    public partial class FrmSmsAyarlari : MyFrmKayit {
        private readonly ISmsAyarService _srv = Ortak.DbPro.SmsAyar;
        private List<SmsAyar> _list;
        private SmsAyar _mdl;

        public FrmSmsAyarlari() {
            InitializeComponent();
            EventlerBagla();
        }
        public void EventlerBagla() {
            this.Load += Frm_Load;
            BtnKaydet.Click += BtnKaydet_Click;
            BtnSil.Click += BtnSil_Click;
            BtnDuzenle.Click += BtnDegistir_Click;
            BtnYeni.Click += BtnYeni_Click;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;

        }

        private void Frm_Load(object sender, EventArgs e) {
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            AcilisBittimi = true;
        }
        public void SutunGizle() {
            myView1.SutunGizle("Id");
        }
        public void Bagla() {
            YeniKayit = true;
            var rs = _srv.SelectListWhere("");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            bs.DataSource = _list;
            GridBagla();
            TemizleText();
        }
        private void TemizleText() {
            _mdl = new SmsAyar() { Id = Guid.Empty };
            IdGuid = Guid.Empty;
            TxtKullaniciKodu.Text = "";
            TxtSifre.Text = "";
            TxtBaslik.Text = "";
            TxtGunderimUrl.Text = _mdl.GunderimUrl;
            TxtRaporUrl.Text = _mdl.RaporUrl;

        }
        private void AktarTextlere() {
            IdGuid = _mdl.Id;
            TxtKullaniciKodu.Text = _mdl.KullaniciKodu;
            TxtSifre.Text = _mdl.Sifre;
            TxtBaslik.Text = _mdl.Baslik;
            TxtGunderimUrl.Text = _mdl.GunderimUrl;
            TxtRaporUrl.Text = _mdl.RaporUrl;
        }
        public void GridBagla() {
            myGrid1.DataSource = null;
            bs.DataSource = _list;
            myGrid1.DataSource = bs;
        }
        private void AktarModele() {
            if (_mdl == null) {
                _mdl = new SmsAyar() { Id = MyGuid.NewGuid() };
            }
            if (IdGuid == Guid.Empty) IdGuid = MyGuid.NewGuid();
            _mdl.Id = IdGuid;
            _mdl.KullaniciKodu = TxtKullaniciKodu.Text;
            _mdl.Sifre = TxtSifre.Text;
            _mdl.Baslik = TxtBaslik.Text;
            _mdl.GunderimUrl = TxtGunderimUrl.Text;
            _mdl.RaporUrl = TxtRaporUrl.Text;


        }
        public bool Kaydet() {
            if (!TextLeriKontrolEt()) {
                return false;
            }
            AktarModele();
            var rs = _srv.InsertOrUpdate(_mdl);
            if (rs.Success) {
                KayitEdildi = true;
                return true;
            }
            else {
                MesajHata(rs.Message);
                return false;
            }
        }
        public void Sil() {
            var rs = _srv.Delete(_mdl);
            if (rs.Success) {
                KayitEdildi = true;
                Bagla();
                //this.Close();
            }
            else {
                MesajHata(rs.Message);
            }
        }
        private bool TextLeriKontrolEt() {
            if (string.IsNullOrEmpty(TxtKullaniciKodu.Text)) {
                MesajHata("Lütfen  Kullanici Kodunu giriniz");
                return false;
            }
            if (string.IsNullOrEmpty(TxtSifre.Text)) {
                MesajHata("Lütfen  Sifre giriniz");
                return false;
            }
            if (string.IsNullOrEmpty(TxtBaslik.Text)) {
                MesajHata("Lütfen  Baslik  giriniz");
                return false;
            }
            if (string.IsNullOrEmpty(TxtGunderimUrl.Text)) {
                MesajHata("Lütfen  GunderimUrl  giriniz");
                return false;
            }
            if (string.IsNullOrEmpty(TxtRaporUrl.Text)) {
                MesajHata("Lütfen RaporUrl  giriniz");
                return false;
            }
            return true;
        }
        private void MyView1_MyEventDoubleClickEnter() {
            if (SecimIcinAcildi) {
                var itm = myView1.MyGetCurrentItem<SmsAyar>();
                if (itm != null) {
                    SecilenKod = itm.KullaniciKodu;
                    SecilenRow = itm;
                    SecilenId = itm.Id.ToString();
                    Secildi = true;
                    this.Close();
                }
            }
            else {
                BtnDuzenle.PerformClick();
            }
        }
        private void BtnKaydet_Click(object sender, EventArgs e) {
            if (Kaydet()) {
                Bagla();
            }
        }
        private void BtnSil_Click(object sender, EventArgs e) {
            if (!MesajSor("Kaydı silmek istiyormusunuz..")) {
                return;
            }
            if (_mdl.Id.IsNullOrEmpty()) {
                MesajBilgi("Silinecek Kayıt Seçilmemiş Kayda Çift Tıklayın");
            }
            Sil();
            Bagla();
        }
        private void BtnDegistir_Click(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<SmsAyar>();
            if (itm == null) return;
            _mdl = itm.Clone();
            AcilisBittimi = false;
            AktarTextlere();
            AcilisBittimi = true;
        }
        private void BtnYeni_Click(object sender, EventArgs e) {
            TemizleText();
        }



        private void myButton1_Click(object sender, EventArgs e) {
            TxtGunderimUrl.Text = "";
        }
        private void myButton2_Click(object sender, EventArgs e) {
            TxtRaporUrl.Text = "";
        }


    }


}
