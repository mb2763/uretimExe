using My.Business.Manager;
using My.Business.Service.IstasyonTakipler;
using My.Core;
using My.Entities.Ayarlar;
using My.Entities.IstasyonTakipler;
using My.Entities.Mikro;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace MyUI.MikroModul {
    public partial class FrmMikroyaSarfFireKaydet : MyFrmKayit {
        IIstasyonTakipHareketDetayService srv = Ortak.DbPro.IstasyonTakipHareketDetay;
        public List<IstasyonTakipHareketDetay> FisList;
        private MikroKayitManager _mngMikroKayit;
        private MikroConvertManager _mngConvert;
        public FrmMikroyaSarfFireKaydet() {
            InitializeComponent();
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, System.EventArgs e) {
            _mngMikroKayit = new MikroKayitManager(Ortak.DbPro, Ortak.DbMikro);
            _mngConvert = new MikroConvertManager(Ortak.DbPro, Ortak.DbMikro);
            Bagla();
            DahaOnceKayitEdilmismi();
            AcilisBittimi = true;
        }
        private void Bagla() {
            GridBagla();
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("UrIId");
            myView1.SutunGizle("IstHrId");
            myGrid1.GridYerlesimYukle();
            myView1.SutunGizle("Sec");
        }
        private void GridBagla() {
            myGrid1.DataSource = null;
            myGrid1.DataSource = FisList;
        }
        private void DahaOnceKayitEdilmismi() {
            foreach (var itm in FisList) {
                if (!string.IsNullOrEmpty(itm.EntCode)) {
                    var id = Guid.Parse(itm.EntCode);
                    var rs = _mngMikroKayit.StokHareketIdKayitEdilmismi(id);
                    if (!rs.Success) {
                        MesajHata(rs.Message);
                        BtnKaydet.Visible = false;
                        return;
                    }
                    if (rs.Data?.FirstOrDefault() > 0) {
                        MesajHata("Önceden Aktarılmış Kayıtlar var Aktarım Hataya Sebep Olur.");
                        BtnKaydet.Visible = false;
                        return;
                    }
                }
            }
        }

        private void BtnKaydet_Click(object sender, System.EventArgs e) {
            Kaydet();
        }

        private void Kaydet() {
            List<Ayar> MikroEntAyarlar = Ortak.MikroEntAyarlar;
            MikroFisCikisTurleri SarfCikisFisiTuru = MikroFisCikisTurleri.SarfDepoCikisFisi;
            var ft_UUGFT = MikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "FisTuru" && c.Kodu == "SarfCikisFisi");
            if (ft_UUGFT != null) {
                SarfCikisFisiTuru = MikroKayitFisTurleri.GetMikroCikisFisiTuru(ft_UUGFT.Deger);
            }
            MikroFisCikisTurleri FireGirisFisiTuru = MikroFisCikisTurleri.SarfDepoCikisFisi;
            var ft_UUFGFT = MikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "FisTuru" && c.Kodu == "FireGirisFisi");
            if (ft_UUFGFT != null) {
                FireGirisFisiTuru = MikroKayitFisTurleri.GetMikroCikisFisiTuru(ft_UUFGFT.Deger);
            }

            List<StokHareketleriModel> _lisSarf = new List<StokHareketleriModel>();
            List<StokHareketleriModel> _lisFire = new List<StokHareketleriModel>(); 
            List<StokHareketleriModel> _lisVirman = new List<StokHareketleriModel>();

            string sarfAciklama = "Uretim_" + IstasyonTakipHareketDetayTuru.SarfCikisFisi.ToString();
            string fireAciklama = "Uretim_" + IstasyonTakipHareketDetayTuru.FireGirisFisi.ToString();
            foreach (var itm in FisList) {
                var newid = MyGuid.NewGuid();
                if (string.IsNullOrEmpty(itm.EntCode)) {
                    itm.EntCode = newid.ToString().ToUpper();
                }
                else {
                    newid = Guid.Parse(itm.EntCode);
                }
                if (string.IsNullOrEmpty(itm.Lot)) {
                    itm.Lot = "0";
                }
                var sth = new StokHareketleriModel() {
                    sth_Guid = newid,
                    sth_belge_no = "",
                    sth_belge_tarih = DateTime.Now,
                    sth_birim_pntr = 1,
                    sth_cikis_depo_no = 1,
                    sth_giris_depo_no = 1,
                    sth_evrakno_seri = "",
                    sth_evrakno_sira = 0,
                    sth_malkbl_sevk_tarihi = DateTime.Now,
                    sth_satirno = 0,
                    sth_stok_kod = itm.StokKodu,
                    sth_tarih = DateTime.Now,
                    sth_miktar = itm.FireMiktar,
                    sth_miktar2 = itm.FireMiktar,
                    sth_parti_kodu = itm.Parti,
                    sth_lot_no = itm.Lot.ToInt32(),
                    sth_tip = 1,
                    Renk = itm.Renk,
                    Beden = itm.Beden, 
                    StokAdi = itm.StokAdi
                };
                if (itm.Turu == IstasyonTakipHareketDetayTuru.SarfCikisFisi.ToString()) {
                    sth = _mngConvert.SetSarfCikisFisiTuruAyar(sth, MikroEntAyarlar);
                    sth.sth_aciklama = sarfAciklama;
                    if (SarfCikisFisiTuru == MikroFisCikisTurleri.StokVirmanFisi) {
                        _lisVirman.Add(sth);
                    }
                    else {
                        _lisSarf.Add(sth);
                    }
                }
                else if (itm.Turu == IstasyonTakipHareketDetayTuru.FireGirisFisi.ToString()) {
                    sth = _mngConvert.SetFireGirisFisiTuruAyar(sth, MikroEntAyarlar);
                    sth.sth_aciklama = fireAciklama;
                     if (FireGirisFisiTuru == MikroFisCikisTurleri.StokVirmanFisi) {
                        _lisVirman.Add(sth);
                    }
                    else if (FireGirisFisiTuru == MikroFisCikisTurleri.SarfDepoCikisFisi) {
                        _lisSarf.Add(sth);
                    }
                    else { 
                         _lisFire.Add(sth);
                    }
                }
            }

            string sarfSeri = "";
            string sarfSira = "";
            string fireSeri = "";
            string fireSira = "";
            List<MikroStokHareketleri> _lisMikro = new List<MikroStokHareketleri>();
            if (_lisVirman.Count > 0) {
                var rsV = _mngConvert.ConvertStokVirmanFisi(_lisVirman, Ortak.MikroEntAyarlar);
                if (!rsV.Success) {
                    MesajHata(rsV.Message);
                    return;
                }
                _lisMikro.AddRange(rsV.Data);
            }
            if (_lisSarf.Count > 0) { 
                    var rsSr = _mngConvert.ConvertSarfDepoCikis(_lisSarf, Ortak.MikroEntAyarlar);
                    if (!rsSr.Success) {
                        MesajHata(rsSr.Message);
                        return;
                    }
                    _lisMikro.AddRange(rsSr.Data); 
            }
           if (_lisFire.Count > 0) {  
                   var rsFr = _mngConvert.ConvertSarfDepoCikis(_lisFire, Ortak.MikroEntAyarlar);
                   if (!rsFr.Success) {
                       MesajHata(rsFr.Message);
                       return;
                   }
                   _lisMikro.AddRange(rsFr.Data); 
           }
            

            foreach (var itm in _lisMikro) {
                if (itm.sth_aciklama == fireAciklama) {
                    fireSeri = itm.sth_evrakno_seri;
                    fireSira = itm.sth_evrakno_sira.ToString();
                }
                else if (itm.sth_aciklama == sarfAciklama) {
                    sarfSeri = itm.sth_evrakno_seri;
                    sarfSira = itm.sth_evrakno_sira.ToString();
                }
            }

            var rs3 = _mngMikroKayit.StokHareketKaydet(_lisMikro);
            if (!rs3.Success) {
                MesajHata(rs3.Message);
                return;
            }
            else {
                foreach (var itm in FisList) {
                    if (itm.Turu == IstasyonTakipHareketDetayTuru.SarfCikisFisi.ToString()) {
                        itm.Ent = true;
                        itm.EntSeri = sarfSeri;
                        itm.EntSira = sarfSira;
                        itm.EntDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    }
                    else if (itm.Turu == IstasyonTakipHareketDetayTuru.FireGirisFisi.ToString()) {
                        itm.Ent = true;
                        itm.EntSeri = fireSeri;
                        itm.EntSira = fireSira;
                        itm.EntDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    }
                }
                
                var son = srv.InsertOrUpdate(FisList);
                if (!son.Success) {
                    MesajHata(son.Message);
                    return;
                }

                MesajBilgi("KayıtEdildi");
            }
        }

    }
}
