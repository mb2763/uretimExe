 
using DevExpress.XtraTab;
using My.Business.Manager;
using My.Business.Service.Ayarlar;
using My.Core;
using My.Entities.Ayarlar;
using My.Kontrol.Formlar;
using My.Kontrol.Kontroller;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.AyarVeGenelModul {
    public partial class FrmMikroEntAyarlari : MyFrmKayit {

        string ayarModul = "MikroEntegre";


        private readonly IAyarService _srv = Ortak.DbPro.Ayarlar;
        private List<Ayar> _list;
        private List<Ayar> _listGenelVeTur;
        private List<Ayar> _listDetaylar;
        private List<string> _listGrupAd;
        private Ayar _mdl;
        string aktifpage = "";
        public FrmMikroEntAyarlari() {
            InitializeComponent();
            EventlerBagla();
        }
        public void EventlerBagla() {
            this.Load += Frm_Load;
            BtnKaydet.Click += BtnKaydet_Click;
            // BtnSil.Click += BtnSil_Click;
            BtnDuzenle.Click += BtnDegistir_Click;
            BtnYeni.Click += BtnYeni_Click;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;

        }

        private void Frm_Load(object sender, EventArgs e) {
           
            Bagla();
            GenelBagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            AcilisBittimi = true;
        }
        public void SutunGizle() {
            myView1.SutunGizle("Id");
        }
        public void Bagla() {
            YeniKayit = true;
            var rs = _srv.SelectListWhere($"where Modul='{ayarModul}' Order By Modul,Grup,Kodu ");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
         
            TemizleText(); 
           
            AcilisBittimi = true;
        }

        private void GenelBagla() {
            _listGenelVeTur = new List<Ayar>();
            _listGrupAd = new List<string>();
            foreach (var itm in _list) {
                if (itm.Grup == "GENEL" || itm.Grup == "FisTuru") {
                    _listGenelVeTur.Add(itm.Clone());
                }
            }
            foreach (var itm in MikroKayitFisTurleri.GetAyarFisTurleriList()) {
                _listGrupAd.Add(itm);
            }
            CmbUretimUrunGirisFisi.Properties.Items.Clear();
            CmbUretimStokCikisFisi.Properties.Items.Clear();
            CmbUretimUrunFireGirisFisi.Properties.Items.Clear();
            CmbUretimStokFireCikisFisi.Properties.Items.Clear();
            CmbSarfCikisFisi.Properties.Items.Clear();
            CmbFireGirisFisi.Properties.Items.Clear();
            CmbHizliUretimFisi.Properties.Items.Clear();

            foreach (var itm in MikroKayitFisTurleri.GetUretimUrunGirisFisiTuruList()) {
                CmbUretimUrunGirisFisi.Properties.Items.Add(itm);
            }
            foreach (var itm in MikroKayitFisTurleri.GetUretimUrunFireCikisFisiTuruList()) {
                CmbUretimUrunFireGirisFisi.Properties.Items.Add(itm);
            }
            foreach (var itm in MikroKayitFisTurleri.GetUretimStokCikisFisiTuruList()) {
                CmbUretimStokCikisFisi.Properties.Items.Add(itm);
            }
            foreach (var itm in MikroKayitFisTurleri.GetUretimStokFireCikisFisiTuruList()) {
                CmbUretimStokFireCikisFisi.Properties.Items.Add(itm);
            }
            foreach (var itm in MikroKayitFisTurleri.GetSarfCikisFisiTuruList()) {
                CmbSarfCikisFisi.Properties.Items.Add(itm);
            }
            foreach (var itm in MikroKayitFisTurleri.GetFireGirisFisiTuruList()) {
                CmbFireGirisFisi.Properties.Items.Add(itm);
            }
            foreach (var itm in MikroKayitFisTurleri.GetHizliUretimFisiTuruList()) {
                CmbHizliUretimFisi.Properties.Items.Add(itm);
            }

            foreach (var itm in _listGenelVeTur) {
                if (itm.Grup == "GENEL" && itm.Kodu == "FirmaNo") {
                    TxtFirmaNo.Text = itm.Deger;
                }
                else if (itm.Grup == "GENEL" && itm.Kodu == "KullaniciKodu") {
                    TxtKullaniciKodu.Text = itm.Deger;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.UretimUrunGirisFisi.ToString()) {
                    CmbUretimUrunGirisFisi.Text = itm.Deger;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.UretimStokCikisFisi.ToString()) {
                    CmbUretimStokCikisFisi.Text = itm.Deger;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.UretimStokFireCikisFisi.ToString()) {
                    CmbUretimStokFireCikisFisi.Text = itm.Deger;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.UretimUrunFireCikisFisi.ToString()) {
                    CmbUretimUrunFireGirisFisi.Text = itm.Deger;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.SarfCikisFisi.ToString()) {
                    CmbSarfCikisFisi.Text = itm.Deger;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.FireGirisFisi.ToString()) {
                    CmbFireGirisFisi.Text = itm.Deger;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.HizliUretimFisi.ToString()) {
                    CmbHizliUretimFisi.Text = itm.Deger;
                }
            }
           
          
            TabGrupAdlari.TabPages.Clear(); 
            foreach (var itm in _listGrupAd) {
               
                TabGrupAdlari.TabPages.Add(itm);
            }
            if (TabGrupAdlari.TabPages.Count>0) {
                aktifpage = TabGrupAdlari.TabPages[0].Text;
                DetayBagla(aktifpage);
            }
          

        }
        private void DetayBagla(string grubu) {
            _listDetaylar = new List<Ayar>();
            foreach (var itm in _list) {
                if (itm.Grup == grubu) {
                    _listDetaylar.Add(itm.Clone());
                }
            }
            myGrid1.DataSource = null;
            bs.DataSource = _listDetaylar;
            myGrid1.DataSource = bs;
        }
    

        private void TemizleText() {
            _mdl = new Ayar() { Id = Guid.Empty };
            IdGuid = Guid.Empty;
            TxtKodu.Text = "";
            TxtAciklama.Text = "";
            TxtDeger.Text = "";

        }
        private void AktarTextlere() {
            IdGuid = _mdl.Id;
            TxtKodu.Text = _mdl.Kodu;
            TxtAciklama.Text = _mdl.Aciklama;
            TxtDeger.Text = _mdl.Deger;

        }
        private void AktarModele() {
            if (_mdl == null) {
                _mdl = new Ayar() { Id = MyGuid.NewGuid() };
            }
            if (IdGuid == Guid.Empty) IdGuid = MyGuid.NewGuid();
            _mdl.Id = IdGuid;
            _mdl.Aciklama = TxtAciklama.Text;
            _mdl.Deger = TxtDeger.Text;


        }
        public void Kaydet() {
            if (!TextLeriKontrolEt()) {
                return;
            }
            AktarModele();
            var rs = _srv.InsertOrUpdate(_mdl);
            if (rs.Success) {
                KayitEdildi = true;
                Bagla(); 
                DetayBagla(aktifpage); 
            }
            else {
                MesajHata(rs.Message);
            }
        }
        public void Sil() {
            // var rs = _srv.Delete(_mdl);
            // if (rs.Success) { KayitEdildi = true;   //this.Close(); }
            // else {   MesajHata(rs.Message); }
        }
        private void BtnSil_Click(object sender, EventArgs e) {
            //if (!MesajSor("Kaydı silmek istiyormusunuz..")) {   return; }
            //if (_mdl.Id.IsNullOrEmpty()) {   MesajBilgi("Silinecek Kayıt Seçilmemiş Kayda Çift Tıklayın"); }
            //Sil(); Bagla();
        }
        private bool TextLeriKontrolEt() {
            // if (string.IsNullOrEmpty(TxtDeger.Text)) {
            // MesajHata("Lütfen  kodunu giriniz");  return false; }
            return true;
        }
        private void MyView1_MyEventDoubleClickEnter() {
            if (SecimIcinAcildi) {
                var itm = myView1.MyGetCurrentItem<Ayar>();
                if (itm != null) {
                    SecilenKod = itm.Kodu;
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
            Kaydet(); 
            Ortak.MikroEntAyarlarBagla();
        }

        private void BtnDegistir_Click(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Ayar>();
            if (itm == null) return;
            _mdl = itm.Clone();
            AcilisBittimi = false;
            AktarTextlere();
            AcilisBittimi = true;
        }
        private void BtnYeni_Click(object sender, EventArgs e) {
            TemizleText();
        }

        private void BtnGenelKaydet_Click(object sender, EventArgs e) {
            GenelAktar();
            var rs = _srv.InsertOrUpdate(_listGenelVeTur);
            if (rs.Success) {
                KayitEdildi = true;
                Bagla();
                Ortak.MikroEntAyarlarBagla();
            }
            else {
                MesajHata(rs.Message);
            }
        }
        private void GenelAktar() {

            foreach (var itm in _listGenelVeTur) {
                if (itm.Grup == "GENEL" && itm.Kodu == "FirmaNo") {
                    itm.Deger = TxtFirmaNo.Text;
                }
                else if (itm.Grup == "GENEL" && itm.Kodu == "KullaniciKodu") {
                    itm.Deger = TxtKullaniciKodu.Text;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.UretimUrunGirisFisi.ToString()) {
                    itm.Deger = CmbUretimUrunGirisFisi.Text;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.UretimUrunFireCikisFisi.ToString()) {
                    itm.Deger = CmbUretimUrunFireGirisFisi.Text;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.UretimStokCikisFisi.ToString()) {
                    itm.Deger = CmbUretimStokCikisFisi.Text;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.UretimStokFireCikisFisi.ToString()) {
                    itm.Deger = CmbUretimStokFireCikisFisi.Text;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.SarfCikisFisi.ToString()) {
                    itm.Deger = CmbSarfCikisFisi.Text;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.FireGirisFisi.ToString()) {
                    itm.Deger = CmbFireGirisFisi.Text;
                }
                else if (itm.Grup == "FisTuru" && itm.Kodu == MikroAyarFisTurleri.HizliUretimFisi.ToString()) {
                    itm.Deger = CmbHizliUretimFisi.Text;
                }
            }
        }

        private void BtnAyarlarSil_Click(object sender, EventArgs e) {
            if (!MesajSor("Kaydı Sıfırlamak istiyormusunuz..\nSildikten Sonra Database Güncelleme Yapmanız lazım")) { return; }

            AyarlarSil();
        }
        public void AyarlarSil() {
            var rs = _srv.Delete(c => c.Modul == ayarModul);
            if (rs.Success) { KayitEdildi = true; this.Close(); }
            else { MesajHata(rs.Message); }
        }

    
        private void TabGrupAdlari_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e) {
          //  xtraTabControl1
            var pg = (XtraTabControl)sender;
            if (e.Page == null) return;
            aktifpage = e.Page.Text.ToString();
            DetayBagla(aktifpage);

        }
    }


}
