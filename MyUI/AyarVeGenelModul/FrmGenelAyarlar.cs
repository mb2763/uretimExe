using My.Business.Service.Ayarlar;
using My.Core;
using My.Entities.Ayarlar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.AyarVeGenelModul {
    public partial class FrmGenelAyarlar : MyFrmKayit {

        string ayarModul = "Genel";
        private readonly IAyarService _srv = Ortak.DbPro.Ayarlar;
        private List<Ayar> _list;
        private Ayar _mdl;

        public FrmGenelAyarlar() {
            InitializeComponent();
            EventlerBagla();
        }
        public void EventlerBagla() {
            this.Load += Frm_Load;
            BtnKaydet.Click += BtnKaydet_Click;
            BtnSil.Click += BtnSil_Click;
            BtnDuzenle.Click += BtnDuzenle_Click;
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
            var rs = _srv.SelectListWhere($"where Modul='{ayarModul}' Order By Kodu ");
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
        public void GridBagla() {
            myGrid1.DataSource = null;
            bs.DataSource = _list;
            myGrid1.DataSource = bs;
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
                Ortak.GenelAyarlarBagla();
            }
            else {
                MesajHata(rs.Message);
            }
        }

        private void Duzenle() {
            var itm = myView1.MyGetCurrentItem<Ayar>();
            if (itm == null) return;
            _mdl = itm.Clone();
            AcilisBittimi = false;
            AktarTextlere();
            AcilisBittimi = true;
        }

        public void Sil() {
            //var rs = _srv.Delete(_mdl); if (rs.Success) {    KayitEdildi = true;   //this.Close(); }
            //else {   MesajHata(rs.Message); }
        }
        private bool TextLeriKontrolEt() {
            //if (string.IsNullOrEmpty(TxtDeger.Text)) {
            //    MesajHata("Lütfen  kodunu giriniz");   return false; }
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
                Duzenle();
            }
        }
        private void BtnKaydet_Click(object sender, EventArgs e) {
            Kaydet();
            Bagla();
            Ortak.MikroEntAyarlarBagla();
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
     
        private void BtnYeni_Click(object sender, EventArgs e) {
            TemizleText();
        }

        private void BtnDuzenle_Click(object sender, EventArgs e) {
            Duzenle();
        }
    }


}
