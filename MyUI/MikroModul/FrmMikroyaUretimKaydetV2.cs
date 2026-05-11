using My.Business.Manager;
using My.Business.Service.IstasyonTakipler;
using My.Business.Service.Templer;
using My.Entities.Ayarlar;
using My.Entities.IstasyonTakipler;
using My.Entities.Mikro;
using My.Entities.Models;
using My.Entities.Raporlar;
using My.Entities.StokDepoRaflar;
using My.Entities.Templer;
using My.Kontrol.Formlar;
using My.Kontrol.Yazdirma;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace MyUI.MikroModul {
    public partial class FrmMikroyaUretimKaydetV2 : MyFrmKayit {
        private SiparisManager _mngSip;
        private ReceteManager _mngRecete;
        private MikroKayitManager _mngMikroKayit;
        private MikroConvertManager _mngConvert;
        private SiparisKayitModel _mdl;
        private ITempMikroStokService _srvTmpStok = Ortak.DbPro.TempMikroStok;
        private List<IstasyonTakipStokHareketKullanilan> _listStok;
        private List<IstasyonTakipHareketDetay> _listStokFire;
        private List<IstasyonTakipHareketDetay> _listStokFirePartili;
        private List<MalKabulFisKullanilanStokModel> _listMalKabulStok;
        private List<MalKabulFisKullanilanStokModel> _listMalKabulStokKalan;
        private List<TempMikroStok> _TempStok;
        private KaliteRaporModel _KaliteRaporData;
        private IIstasyonTakipStokHareketService _srvStok = Ortak.DbPro.IstasyonTakipStokHareket;
        private IIstasyonTakipHareketDetayService _srvHrDetay = Ortak.DbPro.IstasyonTakipHareketDetay;
        private IIstasyonTakipStokHareketDetayService _srvSHrDetay = Ortak.DbPro.IstasyonTakipStokHareketDetay;
        public Guid? SipId = Guid.Empty;
        public string Turu = "Siparis";
        public FrmMikroyaUretimKaydetV2() {
            InitializeComponent();
            this.Load += Frm_Load;
            }
        private void Frm_Load(object sender, System.EventArgs e) {
            _mngSip = new SiparisManager(Ortak.DbPro, Ortak.DbMikro);
            _mngMikroKayit = new MikroKayitManager(Ortak.DbPro, Ortak.DbMikro);
            _mngConvert = new MikroConvertManager(Ortak.DbPro, Ortak.DbMikro);
            _mngRecete = new ReceteManager(Ortak.DbPro);
            DetaylarGuncelle();
            Bagla();
            Ortak.IstasyonAyarlarBagla();
            BaglaStokFirelerByParti();
            if (Ortak.MalKabulKullan) {
                BaglaStoklarMalKabul();
                } else {
                BaglaStoklar();
                }
            BaglaStokFireler();
            TempStokBagla();
            AcilisBittimi = true;
            }
        private void Bagla() {
            if (_mngSip == null) {
                _mngSip = new SiparisManager(Ortak.DbPro, Ortak.DbMikro);
                }
            YeniKayit = false;
            var rs = _mngSip.GetSiparis(SipId);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
                }
            _mdl = rs.Data;
            AktarTextlere();
            GridBagla();
            }
        private void BaglaStoklar() {
            var rs = _srvStok.GetViewListKullanimWherePartiLot(" and   HRD.SipId =  '" + SipId + "' ");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
                }
            _listStok = rs.Data.ToList();
            myGrid2.DataSource = _listStok;
            myView2.SutunGizle("Id");
            myGrid2.GridYerlesimYukle();
            if (Ortak.PlKapat) {
                myView2.SutunGizle("Parti");
                myView2.SutunGizle("Lot");
                }
            }
        private void BaglaStoklarMalKabul() {
            var rs = _srvStok.GetViewListKullanimWhereMalKabul(" and   UrST.SipId =  '" + SipId + "' ");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
                }
            var rs2 = _srvStok.GetViewListKullanimMalKabulFis(SipId);
            if (!rs2.Success) {
                MesajHata(rs2.Message);
                return;
                }
            _listMalKabulStok = rs2.Data.ToList();
            // firede kullanilan parti varsa düş
            var cls = new MikroyaKaydetMalKabulHesaplama(rs2.Data.ToList(), rs.Data.ToList(), _listStokFirePartili);
            var lis = cls.Convert();
            _listMalKabulStokKalan = cls.GetKalanList();
            _listStok = lis;
            myGrid2.DataSource = _listStok;
            myView2.SutunGizle("Id");
            myGrid2.GridYerlesimYukle();
            if (Ortak.PlKapat) {
                myView2.SutunGizle("Parti");
                myView2.SutunGizle("Lot");
                }
            }
        private void BaglaStokFireler() {
            //  IstasyonTakipHareketDetay DT    UretimIstasyon UrI UretimOperasyon   UrO  
            var rs = _srvHrDetay.GetViewListStokFire(" and UrI.SipId = '" + SipId + "' ");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
                }
            List<IstasyonTakipHareketDetay> _listFire;
            if (Ortak.MalKabulKullan) {
                var cls = new MikroyaKaydetMalKabulFireHesaplama(_listMalKabulStokKalan, _listMalKabulStok, rs.Data.ToList());
                _listFire = cls.Convert();
                } else {
                _listFire = rs.Data.ToList();
                }
            _listStokFire = _listFire;
            }
        private void BaglaStokFirelerByParti() {
            //  IstasyonTakipHareketDetay DT    UretimIstasyon UrI UretimOperasyon   UrO  
            var rs = _srvHrDetay.GetViewListStokFire(" and UrI.SipId = '" + SipId + "' and coalesce(IstHD.Parti,'')<>''  ");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
                }
            if (rs.Data == null) {
                _listStokFirePartili = new List<IstasyonTakipHareketDetay>();
                } else {
                _listStokFirePartili = rs.Data.ToList();
                }
            }
        private void BaglaKaliteYazdir() {
            if (_mngSip == null) {
                _mngSip = new SiparisManager(Ortak.DbPro, Ortak.DbMikro);
                }
            YeniKayit = false;
            var rs = _mngSip.GetKaliteYazdir(SipId);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
                }
            _KaliteRaporData = rs.Data;
            const string YazdirmaAdi = "KaliteRapor";
            DataSet ds = new DataSet("KaliteRaporDS");
            ds.Tables.Add(_KaliteRaporData.ToDataTable("KaliteRapor"));
            ds.Yaz(YazdirmaAdi, false);
            }
        private void TempStokBagla() {
            var lisStokKodlari = new List<string>();
            foreach (var itm in _listStok) {
                var f = lisStokKodlari.Find(c => c == itm.StokKodu);
                if (f == null) {
                    lisStokKodlari.Add(itm.StokKodu);
                    }
                }
            foreach (var itm in _listStokFire) {
                var f = lisStokKodlari.Find(c => c == itm.StokKodu);
                if (f == null) {
                    lisStokKodlari.Add(itm.StokKodu);
                    }
                }
            string sqlIn = "";
            foreach (var itm in lisStokKodlari) {
                if (string.IsNullOrEmpty(sqlIn)) {
                    sqlIn = $"'{itm}'";
                    } else {
                    sqlIn = sqlIn + $",'{itm}'";
                    }
                }
            string sqlWhere = $" where StokKodu in({sqlIn})";
            var rs = _srvTmpStok.SelectListWhere(sqlWhere);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
                }
            _TempStok = rs.Data.ToList();
            }
        private void DetaylarGuncelle() {
            var rs = _srvSHrDetay.DetaylarGuncelleBySipId(SipId);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
                }
            }
        private void AktarTextlere() {
            Turu = _mdl.Siparis.Turu;
            TxtSiparisKodu.Text = _mdl.Siparis.SiparisKodu;
            TxtCariKodu.Text = _mdl.Siparis.CariKodu;
            TxtCariUnvani.Text = _mdl.Siparis.CariUnvani;
            TxtTarih.Text = _mdl.Siparis.Tarih.ToString();
            TxtTeslimTarihi.Text = _mdl.Siparis.TeslimTarihi.ToString();
            }
        private void GridBagla() {
            myView1.SutunGizle("Id");
            myGrid1.DataSource = null;
            myGrid1.DataSource = _mdl.Hareketler;
            myGrid1.GridYerlesimYukle();
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
                }
            }
        private void BtnKaydet_Click(object sender, System.EventArgs e) {
            if (_mdl.Siparis.Ent) {
                if (!MesajSor("Uretim Daha Once Mikroya Kayıt Edilmiş Devam Ederseniz Eski fişleri silmeniz Gerekir")) {
                    return;
                    }
                }
            Kaydet(TxtSiparisKodu.Text, Ortak.MikroEntAyarlar.Where(c => c.Modul == "MikroEntegre").ToList());
            }
        private void Kaydet(string belgeNo, List<Ayar> MikroEntAyarlar) {
            bool stdMaliyet = false;
            bool rctMaliyet = false;
            var std = MikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "UretimStokCikisFisi" && c.Kodu == "STANDARTMALIYET");
            if (std != null) {
                stdMaliyet = Convert.ToBoolean(Convert.ToInt16(std.Deger));
                }
            var rct = MikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "UretimStokCikisFisi" && c.Kodu == "RECETEMALIYET");
            if (rct != null) {
                rctMaliyet = Convert.ToBoolean(Convert.ToInt16(rct.Deger));
                }
            bool stdMaliyetFr = false;
            bool rctMaliyetFr = false;
            var stdFr = MikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "UretimStokFireCikisFisi" && c.Kodu == "STANDARTMALIYET");
            if (stdFr != null) {
                stdMaliyetFr = Convert.ToBoolean(Convert.ToInt16(stdFr.Deger));
                }
            var rctFr = MikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "UretimStokFireCikisFisi" && c.Kodu == "RECETEMALIYET");
            if (rctFr != null) {
                rctMaliyetFr = Convert.ToBoolean(Convert.ToInt16(rctFr.Deger));
                }
            MikroFisGirisTurleri UretimUrunGirisFisiTuru = MikroFisGirisTurleri.StokVirmanFisi;
            var ft_UUGFT = MikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "FisTuru" && c.Kodu == "UretimUrunGirisFisi");
            if (ft_UUGFT != null) {
                UretimUrunGirisFisiTuru = MikroKayitFisTurleri.GetMikroGirisFisiTuru(ft_UUGFT.Deger);
                }
            MikroFisCikisTurleri UretimUrunFireCikisFisiTuru = MikroFisCikisTurleri.StokVirmanFisi;
            var ft_UUFGFT = MikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "FisTuru" && c.Kodu == "UretimUrunFireCikisFisi");
            if (ft_UUFGFT != null) {
                UretimUrunFireCikisFisiTuru = MikroKayitFisTurleri.GetMikroCikisFisiTuru(ft_UUFGFT.Deger);
                }
            MikroFisCikisTurleri UretimStokCikisFisiTuru = MikroFisCikisTurleri.StokVirmanFisi;
            var ft_USCFT = MikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "FisTuru" && c.Kodu == "UretimStokCikisFisi");
            if (ft_USCFT != null) {
                UretimStokCikisFisiTuru = MikroKayitFisTurleri.GetMikroCikisFisiTuru(ft_USCFT.Deger);
                }
            MikroFisCikisTurleri UretimStokFireCikisFisiTuru = MikroFisCikisTurleri.StokVirmanFisi;
            var ft_USFCFT = MikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "FisTuru" && c.Kodu == "UretimStokFireCikisFisi");
            if (ft_USFCFT != null) {
                UretimStokFireCikisFisiTuru = MikroKayitFisTurleri.GetMikroCikisFisiTuru(ft_USFCFT.Deger);
                }
            List<StokHareketleriModel> _lisUrunGiris = new List<StokHareketleriModel>();
            List<StokHareketleriModel> _lisUrunFireCikis = new List<StokHareketleriModel>();
            List<StokHareketleriModel> _lisStokCikis = new List<StokHareketleriModel>();
            List<StokHareketleriModel> _lisStokFireCikis = new List<StokHareketleriModel>();
            double uruntoplami = 0;
            int? rafOmru = null;
            DateTime? fisTarih = DateTime.Now;
            foreach (var itm in _mdl.Hareketler) {
                fisTarih = Convert.ToDateTime(Convert.ToDateTime(itm.Tarih).ToShortDateString());
                var rsRaf = _mngRecete.GetRafOmru(itm.RcAId);
                if (rsRaf.IsError) {
                    MesajHata(rsRaf.Message);
                    return;
                    }
                rafOmru = rsRaf.Data;
                double miktar = itm.UretimMiktari + itm.FireMiktari;
                uruntoplami += miktar;
                if (string.IsNullOrEmpty(itm.Lot)) {
                    itm.Lot = "0";
                    }
                var sth = new StokHareketleriModel() {
                    sth_Guid = Guid.NewGuid(),
                    sth_aciklama = "UretimGirisi",
                    sth_belge_no = belgeNo,
                    sth_belge_tarih = DateTime.Now,
                    sth_birim_pntr = 1,
                    sth_malkbl_sevk_tarihi = DateTime.Now,
                    sth_miktar = miktar,
                    sth_miktar2 = miktar,
                    sth_stok_kod = itm.StokKodu,
                    sth_tarih = fisTarih,
                    sth_parti_kodu = itm.Parti,
                    sth_lot_no = itm.Lot.ToInt32(),
                    sth_tip = 0,
                    Renk = itm.Renk,
                    Beden = itm.Beden,
                    IsEmriNo = _mdl.Siparis.IsEmriNo,
                    IsEmriKodu = _mdl.Siparis.SiparisKodu,
                    StokAdi = itm.StokAdi
                    };
                if (rafOmru != null && rafOmru > 0) {
                    sth.UretimTarihi = DateTime.Now.Date;
                    sth.SonKullanmaTarihi = DateTime.Now.AddDays((int)rafOmru).Date;
                    }
                sth = _mngConvert.SetUretimUrunGirisFisiAyar(sth, MikroEntAyarlar);
                _lisUrunGiris.Add(sth);
                if (itm.FireMiktari > 0) {
                    var sthF = new StokHareketleriModel() {
                        sth_Guid = Guid.NewGuid(),
                        sth_aciklama = "UretimFireGirisi",
                        sth_belge_no = belgeNo,
                        sth_belge_tarih = DateTime.Now,
                        sth_birim_pntr = 1,
                        sth_malkbl_sevk_tarihi = DateTime.Now,
                        sth_miktar = itm.FireMiktari,
                        sth_miktar2 = itm.FireMiktari,
                        sth_stok_kod = itm.StokKodu,
                        sth_tarih = fisTarih,
                        sth_parti_kodu = itm.Parti,
                        sth_lot_no = itm.Lot.ToInt32(),
                        sth_tip = 1,
                        Renk = itm.Renk,
                        Beden = itm.Beden,
                        IsEmriNo = _mdl.Siparis.IsEmriNo,
                        IsEmriKodu = _mdl.Siparis.SiparisKodu,
                        StokAdi = itm.StokAdi
                        };
                    sthF = _mngConvert.SetUretimUrunFireCikisFisiAyar(sthF, MikroEntAyarlar);
                    _lisUrunFireCikis.Add(sthF);
                    }
                }
            double hesaplananMiktar = 0;
            double hesaplananMiktar2 = 0;
            short birimpntr = 1;
            double hrkMiktar = 0;
            foreach (var itm in _listStok) {
                if (string.IsNullOrEmpty(itm.Lot)) {
                    itm.Lot = "0";
                    }
                if (itm.StokMiktar > 0) {
                    hesaplananMiktar = 0;
                    hesaplananMiktar2 = 0;
                    hrkMiktar = 0;
                    birimpntr = 1;
                    foreach (var it1 in _mdl.Hareketler) {
                        if (itm.SipHId == it1.Id) {
                            hrkMiktar = it1.UretimMiktari + it1.FireMiktari;
                            break;
                            }
                        }
                    hesaplananMiktar2 = itm.Carpan * hrkMiktar;
                    hesaplananMiktar = hesaplananMiktar2;
                    var tmpStk = _TempStok.Find(c => c.StokKodu == itm.StokKodu);
                    if (tmpStk != null) {
                        if (itm.Birim == tmpStk.Birim2) {
                            double stkcarpan = 1;
                            double katsayi = tmpStk.Katsayi2;
                            if (katsayi < 0) {
                                stkcarpan = (tmpStk.Katsayi1 / katsayi) * -1;
                                } else {
                                stkcarpan = (tmpStk.Katsayi1 * katsayi);
                                }
                            birimpntr = 2;
                            hesaplananMiktar = hesaplananMiktar2 / stkcarpan;
                            }
                        if (itm.Birim == tmpStk.Birim3) {
                            double stkcarpan = 1;
                            double katsayi = tmpStk.Katsayi3;
                            if (katsayi < 0) {
                                stkcarpan = (tmpStk.Katsayi1 / katsayi) * -1;
                                } else {
                                stkcarpan = (tmpStk.Katsayi1 * katsayi);
                                }
                            birimpntr = 3;
                            hesaplananMiktar = hesaplananMiktar2 / stkcarpan;
                            }
                        if (itm.Birim == tmpStk.Birim4) {
                            double stkcarpan = 1;
                            double katsayi = tmpStk.Katsayi4;
                            if (katsayi < 0) {
                                stkcarpan = (tmpStk.Katsayi1 / katsayi) * -1;
                                } else {
                                stkcarpan = (tmpStk.Katsayi1 * katsayi);
                                }
                            birimpntr = 4;
                            hesaplananMiktar = hesaplananMiktar2 / stkcarpan;
                            }
                        }
                    var sth = new StokHareketleriModel() {
                        sth_Guid = Guid.NewGuid(),
                        sth_aciklama = "Uretim Stok Cikis",
                        sth_belge_no = belgeNo,
                        sth_belge_tarih = DateTime.Now,
                        sth_birim_pntr = birimpntr,
                        sth_malkbl_sevk_tarihi = DateTime.Now,
                        sth_miktar = hesaplananMiktar,
                        sth_miktar2 = hesaplananMiktar2,
                        sth_stok_kod = itm.StokKodu,
                        sth_tarih = fisTarih,
                        sth_parti_kodu = itm.Parti,
                        sth_lot_no = itm.Lot.ToInt32(),
                        sth_tip = 1,
                        Renk = itm.Renk,
                        Beden = itm.Beden,
                        IsEmriNo = _mdl.Siparis.IsEmriNo,
                        IsEmriKodu = _mdl.Siparis.SiparisKodu,
                        StokAdi = itm.StokAdi
                        };
                    sth = _mngConvert.SetUretimStokCikisFisiAyar(sth, MikroEntAyarlar);
                    _lisStokCikis.Add(sth);
                    }
                }
            foreach (var itm in _listStokFire) {
                if (string.IsNullOrEmpty(itm.Lot)) {
                    itm.Lot = "0";
                    }
                if (itm.FireMiktar > 0) {
                    hesaplananMiktar = 0;
                    hesaplananMiktar2 = 0;
                    birimpntr = 1;
                    hesaplananMiktar2 = itm.FireMiktar;
                    hesaplananMiktar = hesaplananMiktar2;
                    var tmpStk = _TempStok.Find(c => c.StokKodu == itm.StokKodu);
                    if (tmpStk != null) {
                        if (itm.Birim == tmpStk.Birim2) {
                            double stkcarpan = 1;
                            double katsayi = tmpStk.Katsayi2;
                            if (katsayi < 0) {
                                stkcarpan = (tmpStk.Katsayi1 / katsayi) * -1;
                                } else {
                                stkcarpan = (tmpStk.Katsayi1 * katsayi);
                                }
                            birimpntr = 2;
                            hesaplananMiktar = hesaplananMiktar2 / stkcarpan;
                            }
                        if (itm.Birim == tmpStk.Birim3) {
                            double stkcarpan = 1;
                            double katsayi = tmpStk.Katsayi3;
                            if (katsayi < 0) {
                                stkcarpan = (tmpStk.Katsayi1 / katsayi) * -1;
                                } else {
                                stkcarpan = (tmpStk.Katsayi1 * katsayi);
                                }
                            birimpntr = 3;
                            hesaplananMiktar = hesaplananMiktar2 / stkcarpan;
                            }
                        if (itm.Birim == tmpStk.Birim4) {
                            double stkcarpan = 1;
                            double katsayi = tmpStk.Katsayi4;
                            if (katsayi < 0) {
                                stkcarpan = (tmpStk.Katsayi1 / katsayi) * -1;
                                } else {
                                stkcarpan = (tmpStk.Katsayi1 * katsayi);
                                }
                            birimpntr = 4;
                            hesaplananMiktar = hesaplananMiktar2 / stkcarpan;
                            }
                        }
                    var sth = new StokHareketleriModel() {
                        sth_Guid = Guid.NewGuid(),
                        sth_aciklama = "Uretim Fire Stok Cikis",
                        sth_belge_no = belgeNo,
                        sth_belge_tarih = DateTime.Now,
                        sth_birim_pntr = birimpntr,
                        sth_malkbl_sevk_tarihi = DateTime.Now,
                        sth_miktar = hesaplananMiktar,
                        sth_miktar2 = hesaplananMiktar2,
                        sth_stok_kod = itm.StokKodu,
                        sth_tarih = fisTarih,
                        sth_parti_kodu = itm.Parti,
                        sth_lot_no = itm.Lot.ToInt32(),
                        sth_tip = 1,
                        Renk = itm.Renk,
                        Beden = itm.Beden,
                        IsEmriNo = _mdl.Siparis.IsEmriNo,
                        IsEmriKodu = _mdl.Siparis.SiparisKodu,
                        StokAdi = itm.StokAdi
                        };
                    sth = _mngConvert.SetUretimStokFireCikisFisiAyar(sth, MikroEntAyarlar);
                    _lisStokFireCikis.Add(sth);
                    }
                }
            /* Fiyat Ayar */
            double stoktutar = 0;
            double firetutar = 0;
            // sto_standartmaliyet 
            foreach (var itm in _lisStokCikis)// fiyat al
            {
                var rs = _mngMikroKayit.GetMikroStokMaliyetListWhere(" where S.sto_kod='" + itm.sth_stok_kod + "' ");
                if (!rs.Success) {
                    MesajHata(rs.Message);
                    return;
                    }
                if (stdMaliyet) {
                    var data = rs.Data.FirstOrDefault();
                    if (!string.IsNullOrEmpty(data.STANDARTMALIYET)) {
                        itm.Fiyat = Convert.ToDouble(data.STANDARTMALIYET);
                        }
                    } else if (rctMaliyet) {
                    var data = rs.Data.FirstOrDefault();
                    if (data != null && !string.IsNullOrEmpty(data.RECETEMALIYET)) {
                        itm.Fiyat = Convert.ToDouble(data.RECETEMALIYET);
                        }
                    }
                stoktutar = stoktutar + (itm.sth_miktar * itm.Fiyat);
                }
            // sto_standartmaliyet 
            foreach (var itm in _lisStokFireCikis)// fiyat al
            {
                var rs = _mngMikroKayit.GetMikroStokMaliyetListWhere(" where S.sto_kod='" + itm.sth_stok_kod + "' ");
                if (!rs.Success) {
                    MesajHata(rs.Message);
                    return;
                    }
                if (stdMaliyetFr) {
                    var data = rs.Data.FirstOrDefault();
                    if (!string.IsNullOrEmpty(data.STANDARTMALIYET)) {
                        itm.Fiyat = Convert.ToDouble(data.STANDARTMALIYET);
                        }
                    } else if (rctMaliyetFr) {
                    var data = rs.Data.FirstOrDefault();
                    if (data != null && !string.IsNullOrEmpty(data.RECETEMALIYET)) {
                        itm.Fiyat = Convert.ToDouble(data.RECETEMALIYET);
                        }
                    }
                firetutar = firetutar + (itm.sth_miktar * itm.Fiyat);
                }
            /* Toplamı fiyata yaz */
            var urunfiyat = (stoktutar + firetutar) / uruntoplami;
            foreach (var itm in _lisUrunGiris) {
                itm.Fiyat = urunfiyat;
                }
            foreach (var itm in _lisUrunFireCikis) {
                itm.Fiyat = urunfiyat;
                }
            /* Toplamı fiyata yaz */
            List<MikroStokHareketleri> _lisMikro = new List<MikroStokHareketleri>();
            List<StokHareketleriModel> _lisStokVirmanFisi = new List<StokHareketleriModel>();
            List<StokHareketleriModel> _lisUretimHareketFisi = new List<StokHareketleriModel>();
            List<StokHareketleriModel> _lisUretimdenGirisFisi = new List<StokHareketleriModel>();
            List<StokHareketleriModel> _lisSayimDepoGirisFisi = new List<StokHareketleriModel>();
            List<StokHareketleriModel> _lisUretimeCikisFisi = new List<StokHareketleriModel>();
            List<StokHareketleriModel> _lisSarfDepoCikisFisi = new List<StokHareketleriModel>();
            List<StokHareketleriModel> _lisFireCikisFisi = new List<StokHareketleriModel>();
            if (!(_lisUrunGiris.Count > 0)) { // ürün yoksa çık
                MesajHata("Aktarılacak Ürün Bulunamadı");
                return;
                }
            // Memhmedin yazdıgı depoya gonder 
            List<StokDepoRaf> depoRaf = new List<StokDepoRaf>();
            DateTime trh = DateTime.Now;
            foreach (var itm in _lisUrunGiris) {
                var raf = new StokDepoRaf();
                raf.StokKodu = itm.sth_stok_kod;
                raf.StokAdi = itm.StokAdi;
                raf.DepoNo = itm.sth_giris_depo_no;
                raf.DepoAdi = "";
                raf.Parti = itm.sth_parti_kodu;
                raf.LotNo = itm.sth_lot_no;
                raf.Raf = "A";
                raf.Miktar = itm.sth_miktar;
                raf.KayitTarihi = itm.sth_tarih;
                raf.HareketTarihi = trh;
                raf.HareketAciklamasi = "URETIMGİRİŞ-" + itm.sth_miktar.ToString();
                raf.CariKodu = "";
                raf.CariAdi = "";
                raf.IsEmriNo = itm.IsEmriNo;
                raf.IsEmriKodu = itm.IsEmriKodu;
                depoRaf.Add(raf);
                }
            // MikroAyarFisTurleri  , MikroFisCikisTurleri , MikroFisGirisTurleri  
            // ilk satırdan seri sira alinacak giriş fişini öne al
            StokHareketleriModel girisIlkSatir;
            girisIlkSatir = _lisUrunGiris.FirstOrDefault().Clone();
            if (_lisUrunGiris.Count > 0) {
                if (UretimUrunGirisFisiTuru == MikroFisGirisTurleri.StokVirmanFisi) {
                    _lisStokVirmanFisi.AddRange(_lisUrunGiris);
                    _lisUrunGiris.Clear();
                    } else if (UretimUrunGirisFisiTuru == MikroFisGirisTurleri.UretimHareketFisi) {
                    _lisUretimHareketFisi.AddRange(_lisUrunGiris);
                    _lisUrunGiris.Clear();
                    } else if (UretimUrunGirisFisiTuru == MikroFisGirisTurleri.UretimdenGirisFisi) {
                    _lisUretimdenGirisFisi.AddRange(_lisUrunGiris);
                    _lisUrunGiris.Clear();
                    } else if (UretimUrunGirisFisiTuru == MikroFisGirisTurleri.SayimDepoGirisFisi) {
                    _lisSayimDepoGirisFisi.AddRange(_lisUrunGiris);
                    _lisUrunGiris.Clear();
                    } else {
                    throw new Exception(UretimUrunGirisFisiTuru.ToString() + " Fiş türü bulunamadı. ");
                    }
                }
            if (_lisStokCikis.Count > 0) {
                if (UretimStokCikisFisiTuru == MikroFisCikisTurleri.StokVirmanFisi) {
                    _lisStokVirmanFisi.AddRange(_lisStokCikis);
                    _lisStokCikis.Clear();
                    } else if (UretimStokCikisFisiTuru == MikroFisCikisTurleri.UretimHareketFisi) {
                    _lisUretimHareketFisi.AddRange(_lisStokCikis);
                    _lisStokCikis.Clear();
                    } else if (UretimStokCikisFisiTuru == MikroFisCikisTurleri.UretimeCikisFisi) {
                    _lisUretimeCikisFisi.AddRange(_lisStokCikis);
                    _lisStokCikis.Clear();
                    } else if (UretimStokCikisFisiTuru == MikroFisCikisTurleri.SarfDepoCikisFisi) {
                    _lisSarfDepoCikisFisi.AddRange(_lisStokCikis);
                    _lisStokCikis.Clear();
                    } else if (UretimStokCikisFisiTuru == MikroFisCikisTurleri.FireCikisFisi) {
                    _lisFireCikisFisi.AddRange(_lisStokCikis);
                    _lisStokCikis.Clear();
                    } else {
                    throw new Exception(UretimStokCikisFisiTuru.ToString() + " Fiş türü bulunamadı. ");
                    }
                }
            if (_lisUrunFireCikis.Count > 0) {
                if (UretimUrunFireCikisFisiTuru == MikroFisCikisTurleri.StokVirmanFisi) {
                    _lisStokVirmanFisi.AddRange(_lisUrunFireCikis);
                    _lisUrunFireCikis.Clear();
                    } else if (UretimUrunFireCikisFisiTuru == MikroFisCikisTurleri.UretimHareketFisi) {
                    _lisUretimHareketFisi.AddRange(_lisUrunFireCikis);
                    _lisUrunFireCikis.Clear();
                    } else if (UretimUrunFireCikisFisiTuru == MikroFisCikisTurleri.UretimeCikisFisi) {
                    _lisUretimeCikisFisi.AddRange(_lisUrunFireCikis);
                    _lisUrunFireCikis.Clear();
                    } else if (UretimUrunFireCikisFisiTuru == MikroFisCikisTurleri.SarfDepoCikisFisi) {
                    _lisSarfDepoCikisFisi.AddRange(_lisUrunFireCikis);
                    _lisUrunFireCikis.Clear();
                    } else if (UretimUrunFireCikisFisiTuru == MikroFisCikisTurleri.FireCikisFisi) {
                    _lisFireCikisFisi.AddRange(_lisUrunFireCikis);
                    _lisUrunFireCikis.Clear();
                    } else {
                    throw new Exception(UretimUrunFireCikisFisiTuru.ToString() + " Fiş türü bulunamadı. ");
                    }
                }
            if (_lisStokFireCikis.Count > 0) {
                if (UretimStokFireCikisFisiTuru == MikroFisCikisTurleri.StokVirmanFisi) {
                    _lisStokVirmanFisi.AddRange(_lisStokFireCikis);
                    _lisStokFireCikis.Clear();
                    } else if (UretimStokFireCikisFisiTuru == MikroFisCikisTurleri.UretimHareketFisi) {
                    _lisUretimHareketFisi.AddRange(_lisStokFireCikis);
                    _lisStokFireCikis.Clear();
                    } else if (UretimStokFireCikisFisiTuru == MikroFisCikisTurleri.UretimeCikisFisi) {
                    _lisUretimeCikisFisi.AddRange(_lisStokFireCikis);
                    _lisStokFireCikis.Clear();
                    } else if (UretimStokFireCikisFisiTuru == MikroFisCikisTurleri.SarfDepoCikisFisi) {
                    _lisSarfDepoCikisFisi.AddRange(_lisStokFireCikis);
                    _lisStokFireCikis.Clear();
                    } else if (UretimStokFireCikisFisiTuru == MikroFisCikisTurleri.FireCikisFisi) {
                    _lisFireCikisFisi.AddRange(_lisStokFireCikis);
                    _lisStokFireCikis.Clear();
                    } else {
                    throw new Exception(UretimStokFireCikisFisiTuru.ToString() + " Fiş türü bulunamadı. ");
                    }
                }
            if (_lisStokVirmanFisi.Count > 0) {
                var rs1 = _mngConvert.ConvertStokVirmanFisi(_lisStokVirmanFisi, Ortak.MikroEntAyarlar);
                if (!rs1.Success) {
                    MesajHata(rs1.Message);
                    return;
                    }
                _lisMikro.AddRange(rs1.Data);
                }
            if (_lisUretimHareketFisi.Count > 0) {
                var rs1 = _mngConvert.ConvertUretimHareketFisi(_lisUretimHareketFisi, Ortak.MikroEntAyarlar);
                if (!rs1.Success) {
                    MesajHata(rs1.Message);
                    return;
                    }
                _lisMikro.AddRange(rs1.Data);
                }
            if (_lisUretimdenGirisFisi.Count > 0) {
                var rs1 = _mngConvert.ConvertUretimdenGirisFisi(_lisUretimdenGirisFisi, Ortak.MikroEntAyarlar);
                if (!rs1.Success) {
                    MesajHata(rs1.Message);
                    return;
                    }
                _lisMikro.AddRange(rs1.Data);
                }
            if (_lisSayimDepoGirisFisi.Count > 0) {
                var rs1 = _mngConvert.ConvertSayimDepoGiris(_lisSayimDepoGirisFisi, Ortak.MikroEntAyarlar);
                if (!rs1.Success) {
                    MesajHata(rs1.Message);
                    return;
                    }
                _lisMikro.AddRange(rs1.Data);
                }
            if (_lisUretimeCikisFisi.Count > 0) {
                var rs1 = _mngConvert.ConvertUretimeCikisFisi(_lisUretimeCikisFisi, Ortak.MikroEntAyarlar);
                if (!rs1.Success) {
                    MesajHata(rs1.Message);
                    return;
                    }
                _lisMikro.AddRange(rs1.Data);
                }
            if (_lisSarfDepoCikisFisi.Count > 0) {
                var rs1 = _mngConvert.ConvertSarfDepoCikis(_lisSarfDepoCikisFisi, Ortak.MikroEntAyarlar);
                if (!rs1.Success) {
                    MesajHata(rs1.Message);
                    return;
                    }
                _lisMikro.AddRange(rs1.Data);
                }
            if (_lisFireCikisFisi.Count > 0) {
                var rs1 = _mngConvert.ConvertFireCikis(_lisFireCikisFisi, Ortak.MikroEntAyarlar);
                if (!rs1.Success) {
                    MesajHata(rs1.Message);
                    return;
                    }
                _lisMikro.AddRange(rs1.Data);
                }
            MikroyaKaydet(_lisMikro, depoRaf);
            }
        private void MikroyaKaydet(List<MikroStokHareketleri> _lisMikro, List<StokDepoRaf> depoRaf) {
            var rsKyt = _mngMikroKayit.StokHareketKaydet(_lisMikro, depoRaf);
            if (!rsKyt.Success) {
                MesajHata(rsKyt.Message);
                return;
                } else {
                string seri = _lisMikro[0].sth_evrakno_seri;
                string sira = _lisMikro[0].sth_evrakno_sira.ToString();
                string sonseri = _lisMikro[_lisMikro.Count - 1].sth_evrakno_seri.ToString();
                string sonsira = _lisMikro[_lisMikro.Count - 1].sth_evrakno_sira.ToString();
                var son = _mngSip.SiparisEntGuncelle(_mdl.Siparis.Id, seri, sira, sonseri, sonsira);
                if (!son.Success) {
                    MesajHata(son.Message);
                    return;
                    }
                MesajBilgi("KayıtEdildi");
                BtnKaydet.Enabled = false;
                }
            }
        private void myButton1_Click(object sender, EventArgs e) {
            var rs = _srvStok.GetViewListKullanimWhereMalKabul(" and   UrST.SipId =  '" + SipId + "' ");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
                }
            var rs2 = _srvStok.GetViewListKullanimMalKabulFis(SipId);
            if (!rs2.Success) {
                MesajHata(rs2.Message);
                return;
                }
            FrmMikroMalKabulHesaplama f = new FrmMikroMalKabulHesaplama();
            f.MalKabulFis = rs2.Data.ToList();
            f.IstasyonHareket = rs.Data.ToList();
            f.StokFireListPartili = _listStokFirePartili;
            f.ShowDialog();
            }
        private void BtnYazdir_Click(object sender, EventArgs e) {
            BaglaKaliteYazdir();
            }
        }
    }