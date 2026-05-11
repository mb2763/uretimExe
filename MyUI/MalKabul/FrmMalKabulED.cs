using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.UretimStoklar;
using My.Entities.UretimStoklar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;

namespace MyUI.MalKabul {
    public partial class FrmMalKabulED : MyFrmKayit {
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly IUretimStokFisService _srv = Ortak.DbPro.UretimStokFis;
        private MikroKayitManager _mngMikroKayit;
        UretimStokFisModel _mdl;

        public bool SiparisIdDenAra { get; set; } = false;

        public Guid? SipId { get; set; }

        public List<UretimStokFisHareket> Hareketler { get; set; }
        public FrmMalKabulED() {
            InitializeComponent();
            this.Load += Frm_Load;

        }
        private void Frm_Load(object sender, EventArgs e) {
            _mngMikroKayit = new MikroKayitManager(Ortak.DbPro, Ortak.DbMikro);
            if (SiparisIdDenAra) {
                BaglaSipId();
            }
            else {
                Bagla();
            }
            BaglaHareket();
            SutunGizle();
            myGrid1.GridYerlesimYukle();

        }

        public void Bagla() {


            if (IdGuid.IsNullOrEmpty()) {
                YeniKayit = true;

                _mdl = new UretimStokFisModel();
                TemizleText();
            }
            else {
                YeniKayit = false;
                var rs = _srv.GetFis((Guid)IdGuid);
                if (!rs.Success) {
                    MesajHata(rs.Message);
                    return;
                }

                _mdl = rs.Data;
                AktarTextlere();

            }
        }
        public void BaglaSipId() {


            if (SipId.IsNullOrEmpty()) {
                YeniKayit = true;

                _mdl = new UretimStokFisModel();
                TemizleText();
            }
            else {
                YeniKayit = false;
                var rs = _srv.GetFisFirst($" where Ur.SipId='{SipId}' ");
                if (!rs.Success) {
                    MesajHata(rs.Message);
                    return;
                }

                _mdl = rs.Data;
                IdGuid = _mdl.Id;
                AktarTextlere();

            }
        }
        public void BaglaHareket() {


            if (IdGuid.IsNullOrEmpty()) {
                Hareketler = new List<UretimStokFisHareket>();

            }
            else {
                var rs = _srv.GetStokHareketByFisId((Guid)IdGuid);
                if (!rs.Success) {
                    MesajHata(rs.Message);
                    return;
                }
                Hareketler = rs.Data;

            }
            GridBagla();
        }




        public void GridBagla() {
            myGrid1.DataSource = null;
            myGrid1.DataSource = Hareketler;
        }

        public void GridBaglaAcilis() {
            SutunGizle();
            //myGrid1.GridYerlesimYukle();
            //myView1.SutunEditAc(nameof(SiparisHareket.Miktar)); 
            //myView1.SutunCaptionColor(nameof(SiparisHareket.Miktar), Color.Green); 
            //myView1.SutunFormat("Miktar", DevExpress.Utils.FormatType.Numeric, "N0");
        }
        private void SutunGizle() {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("FisId");
            myView1.SutunGizle("IrsHGuid");
            myView1.SutunGizle("IrsBirimPntr");
            myView1.SutunGizle("StGuid");
            myView1.SutunGizle("Sil");
            myView1.SutunGizle("GirisCikis");
            myView1.SutunGizle("SatirNo");
            myView1.SutunFormat("Tarih", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy");
            myView1.SutunCaptionDegistir("SiparisKodu", "IsEmriKodu");
        }


        public void AktarTextlere() {

            TxtIsEmriKodu.Text = _mdl.IsEmriKodu;
            TxtIsEmriNo.Text = _mdl.IsEmriNo;
            TxtTarih.Text = _mdl.Tarih.ToString();
            TxtIstasyonKodu.Text = _mdl.IstasyonKodu;
            TxtIstasyonAdi.Text = _mdl.IstasyonAdi;
            TxtEvrakNo.Text = _mdl.EvrakNo;
            TxtBelgeNo.Text = _mdl.BelgeNo;
            TxtDurumu.Text = _mdl.Durumu;


            GridBagla();
            GridBaglaAcilis();
        }

        public void TemizleText() {
            TxtIsEmriKodu.Text = "";
            TxtIsEmriNo.Text = "";
            TxtTarih.Text = "";
            TxtIstasyonKodu.Text = "";
            TxtIstasyonAdi.Text = "";
            TxtBelgeNo.Text = "";
            TxtEvrakNo.Text = "";
            TxtTarih.Text = DateTime.Now.ToString();

            GridBagla();
            GridBaglaAcilis();
        }


        public void AktarModele() {
            //_mdl.Siparis.Turu = Turu;
            //_mdl.Siparis.SiparisKodu = TxtSiparisKodu.Text;
            //_mdl.Siparis.CariKodu = TxtCariKodu.Text;
            //_mdl.Siparis.CariUnvani = TxtCariUnvani.Text;

            //_mdl.Siparis.Aciklama = TxtAciklama.Text;
            //if (!TxtTarih.Text.IsNullOrEmpty()) {
            //    _mdl.Siparis.Tarih = Convert.ToDateTime(TxtTarih.Text);
            //}
            //else {
            //    _mdl.Siparis.Tarih = null;
            //}


        }
        private bool TextLeriKontrolEt() {
            if (TxtTarih.Text.IsNullOrEmpty()) {
                TxtTarih.EditValue = DateTime.Now;
            }

            //foreach (var itm in _mdl.Hareketler) {
            //  if (itm.YeniKayit) { MesajBilgi("Siparişde Ayarlanmamış Kayıtlar var lütfen Yeni Kayıtları ayarlayınız");
            //  return false;   } } 
            //if (string.IsNullOrEmpty(TxtCariKodu.Text)) {   MesajHata("Lütfen Cari kodunu giriniz");  return false; } 
            return true;
        }

        public void Kaydet() {
            // if (!TextLeriKontrolEt()) {   return; }  AktarModele();  var rs = _mng.SiparisKaydet(_mdl, YeniKayit);
            // if (rs.Success) {   KayitEdildi = true;    Close(); } else {  MesajHata(rs.Message); }
        }

        public void Sil() {
            if (!MesajSor("Kaydı silmek istiyormusunuz..")) { return; }
            var rs = _srv.FisSil(IdGuid);
            if (!rs.Success) { MesajHata(rs.Message); return; }
            KayitEdildi = true;
            var rs2 = _mngMikroKayit.DeleteMikroAktarilanFisByBelgeNo(_mdl.EvrakNo);
            if (rs2.Success) { MesajHata(rs2.Message); return; }
            this.Close();
        }

        private void BtnSil_Click(object sender, EventArgs e) {
            Sil();
        }
    }
}
