using My.Business.Service.UretimAciklamalar;
using My.Core;
using My.Entities.UretimAciklamalar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.Aciklamalar {
    public partial class FrmAciklamaKodlar : MyFrmKayit {

        public AciklamaModulTuru AciklamaModulTuru;

        private readonly IAciklamaKodService _srv = Ortak.DbPro.AciklamaKod;
        private List<AciklamaKod> _list;
        private AciklamaKod _mdl;
        public FrmAciklamaKodlar() {
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
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            AcilisBittimi = true;
        }
        public void SutunGizle() {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("Modul");
            myView1.SutunGizle("Deger2");
            myView1.SutunGizle("Deger3");
            myView1.SutunEditAc("Sira");
            myView1.SutunReadOnlyKapat("Sira");
        }
        public void Bagla() {
            YeniKayit = true;
            var rs = _srv.SelectListWhere($"where Modul='{AciklamaModulTuru.ToString()}' Order By Sira,Kodu ");
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
            _mdl = new AciklamaKod() { Id = Guid.Empty };
            IdGuid = Guid.Empty;
            TxtKodu.Text = "";
            TxtAdi.Text = "";

        }
        private void AktarTextlere() {
            IdGuid = _mdl.Id;
            TxtKodu.Text = _mdl.Kodu;
            TxtAdi.Text = _mdl.Deger1;

        }
        public void GridBagla() {
            myGrid1.DataSource = null;
            bs.DataSource = _list;
            myGrid1.DataSource = bs;
        }
        private void AktarModele() {
            if (_mdl == null) {
                _mdl = new AciklamaKod() { Id = MyGuid.NewGuid() };
            }
            if (IdGuid == Guid.Empty) IdGuid = MyGuid.NewGuid();
            _mdl.Id = IdGuid;
            _mdl.Kodu = TxtKodu.Text;
            _mdl.Deger1 = TxtAdi.Text;
            _mdl.Modul = AciklamaModulTuru.ToString();

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
                var itm = myView1.MyGetCurrentItem<AciklamaKod>();
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

            var itm = myView1.MyGetCurrentItem<AciklamaKod>();
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
