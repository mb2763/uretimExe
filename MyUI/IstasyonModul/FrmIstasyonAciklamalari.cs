using My.Business.Service.Geneller;
using My.Business.Service.IstasyonAciklamalar;
using My.Core;
using My.Entities.IstasyonAciklamalar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.IstasyonModul {
    public partial class FrmIstasyonAciklamalari : MyFrmKayit {

        public IstasyonAciklamaModulTuru AciklamaModulTuru;

        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly IIstasyonAciklamaService _srv = Ortak.DbPro.IstasyonAciklama;
        private List<IstasyonAciklama> _list;
        private IstasyonAciklama _mdl;

        public FrmIstasyonAciklamalari() {
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
            try {
                this.Text = AciklamaModulTuru.ToString();
                lblBaslik.Text = AciklamaModulTuru.ToString();
            }
            catch (Exception) {

                throw;
            }
            BaglaGorevi();
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            AcilisBittimi = true;
        }
        public void SutunGizle() {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("Modul");
        }
        public void Bagla() {
            YeniKayit = true;
            var rs = _srv.SelectListWhere($"where Modul='{AciklamaModulTuru.ToString()}' Order By Kodu ");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            bs.DataSource = _list;
            GridBagla();
            TemizleText();
        }
        private void BaglaGorevi() {
            var rs = _srvGenel.GrupListesi("Personel", "Gorevi");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbGorevi.MyDataBagla(dt);
        }

        private void TemizleText() {
            _mdl = new IstasyonAciklama() { Id = Guid.Empty };
            IdGuid = Guid.Empty;
            TxtKodu.Text = "";
            TxtAdi.Text = "";
            CmbGorevi.Text = "";
            TxtSmsKodu.Text = "";
            ChcSmsGonder.Checked = false;

        }
        private void AktarTextlere() {
            IdGuid = _mdl.Id;
            TxtKodu.Text = _mdl.Kodu;
            TxtAdi.Text = _mdl.Deger;
            CmbGorevi.Text = _mdl.Gorevi;
            TxtSmsKodu.Text = _mdl.SmsKodu;
            ChcSmsGonder.Checked = _mdl.SmsGonder;
        }
        public void GridBagla() {
            myGrid1.DataSource = null;
            bs.DataSource = _list;
            myGrid1.DataSource = bs;
        }
        private void AktarModele() {
            if (_mdl == null) {
                _mdl = new IstasyonAciklama() { Id = MyGuid.NewGuid() };
            }
            if (IdGuid == Guid.Empty) IdGuid = MyGuid.NewGuid();
            _mdl.Id = IdGuid;
            _mdl.Kodu = TxtKodu.Text;
            _mdl.Deger = TxtAdi.Text;
            _mdl.Modul = AciklamaModulTuru.ToString();
            _mdl.Gorevi = CmbGorevi.Text.ToString();
            _mdl.SmsKodu = TxtSmsKodu.Text.ToString();
            _mdl.SmsGonder = ChcSmsGonder.Checked;

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
            }
            else {
                MesajHata(rs.Message);
            }
        }
        public void Sil() {
            var rs = _srv.Delete(_mdl);
            if (rs.Success) {
                KayitEdildi = true;
                //this.Close();
            }
            else {
                MesajHata(rs.Message);
            }
        }
        private bool TextLeriKontrolEt() {
            if (string.IsNullOrEmpty(TxtKodu.Text)) {
                MesajHata("Lütfen  kodunu giriniz");
                return false;
            }
            return true;
        }
        private void MyView1_MyEventDoubleClickEnter() {
            if (SecimIcinAcildi) {
                var itm = myView1.MyGetCurrentItem<IstasyonAciklama>();
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
            Bagla();
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

            var itm = myView1.MyGetCurrentItem<IstasyonAciklama>();
            if (itm == null) return;
            _mdl = itm.Clone();
            AcilisBittimi = false;
            AktarTextlere();
            AcilisBittimi = true;
        }
        private void BtnYeni_Click(object sender, EventArgs e) {
            TemizleText();
        }
    }

}
