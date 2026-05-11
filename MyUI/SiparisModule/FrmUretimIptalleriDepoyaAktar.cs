using DevExpress.XtraDiagram.Base;
using My.Business.Manager;
using My.Business.Service.IstasyonTakipler;
using My.Business.Service.UretimStoklar;
using My.Core;
using My.Entities.DepoStoklar;
using My.Entities.IstasyonTakipler;
using My.Entities.Models;
using My.Kontrol.Formlar; 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MyUI.SiparisModule {
    public partial class FrmUretimIptalleriDepoyaAktar : MyFrmKayit {
        private DepoStokFisManager _mngDep;
    
        private SiparisManager _mngSip; 
        private SiparisKayitModel _mdl;
        private List<IstasyonTakipStokHareketKullanilan> _listStok;
        private  DepoStokFis  _fis;
        private List<DepoStokHareket> _list;
        private IIstasyonTakipStokHareketService _srvStok = Ortak.DbPro.IstasyonTakipStokHareket;
     
        public Guid? SipId = Guid.Empty;
        int fisno = 0;
        public FrmUretimIptalleriDepoyaAktar() {
            InitializeComponent();
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, System.EventArgs e) {
            _mngSip = new SiparisManager(Ortak.DbPro, Ortak.DbMikro); 
            _mngDep = new DepoStokFisManager(Ortak.DbPro);
            Bagla();
            BaglaStoklar();
            AktarilacakAyarla();
            IslemTuru = "UretimIade";
            YeniKayit = true;
            AcilisBittimi = true;
        }
        private void Bagla() {
           
            if (_mngSip == null) {
                _mngSip = new SiparisManager(Ortak.DbPro, Ortak.DbMikro);
            }
          
            var rs = _mngSip.GetSiparis(SipId);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            _mdl = rs.Data;
            myView1.SutunGizle("Id");

            AktarTextlere();
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
        }

        private void AktarTextlere() { 
            TxtSiparisKodu.Text = _mdl.Siparis.SiparisKodu;
            TxtCariKodu.Text = _mdl.Siparis.CariKodu;
            TxtCariUnvani.Text = _mdl.Siparis.CariUnvani;
            TxtTarih.Text = _mdl.Siparis.Tarih.ToString();
            TxtTeslimTarihi.Text = _mdl.Siparis.TeslimTarihi.ToString();
            GridBagla();
        }
        private void GridBagla() {
            myGrid1.DataSource = null;
            myGrid1.DataSource = _mdl.Hareketler;
            myGrid1.GridYerlesimYukle();
        }
        private void AktarilacakAyarla() {
            _list = new List<DepoStokHareket>();
            double hesaplananMiktar = 0;
            double hrkMiktar = 0;
            int satirno = 0;
            foreach (var it1 in _mdl.Hareketler) {
                hrkMiktar = 0;
                if (it1.IptalMiktari > 0 && it1.Miktar > (it1.UretimMiktari + it1.FireMiktari)) {
                    hrkMiktar = it1.Miktar - (it1.UretimMiktari + it1.FireMiktari);
                    foreach (var itm in _listStok) {
                        if (itm.SipHId == it1.Id) {
                            hesaplananMiktar = itm.Carpan * hrkMiktar;
                            satirno = ++satirno;
                            _list.Add(new DepoStokHareket() {
                                GirisCikis = 0,
                                Birimi = itm.Birim,
                                StokKodu = itm.StokKodu,
                                StokAdi = itm.StokAdi,
                                GirisMiktar = hesaplananMiktar,
                                SatirNo = satirno,
                                Sira = satirno,
                                SonKulTarihi = DateTime.Now,
                                UretimTarihi = DateTime.Now,
                            });
                        }
                    }
                }
            }
            if (_list.Count<=0) {
                MesajHata( "Aktarılacak Kayıt Bulunamadı");
                return;
            }    
            var rs = _mngDep.GetDepoStok(_list);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            foreach (var it1 in rs.Data) { 
                foreach (var itm in _list) {
                    if (it1.StokKodu == itm.StokKodu) {
                        itm.StGuid = it1.StGuid;
                        itm.Birimi = it1.Birimi;
                    }
               }
                            }
            
            myGrid3.DataSource = _list;
            myView3.SutunGizle("Id");
            myView3.SutunGizle("FisId");
            myView3.SutunGizle("GirisCikis");
            myView3.SutunGizle("Tarih");
            myView3.SutunGizle("CikisMiktar");
            myView3.SutunGizle("IrsBirimPntr");
            myView3.SutunGizle("IrsHGuid");
            myView3.SutunGizle("Sil");
            myView3.SutunGizle("TakipKodu1");
            myView3.SutunGizle("TakipKodu2");
            myView3.SutunGizle("TakipKodu3");
            myView3.SutunGizle("SatirNo");
            myView3.SutunGizle("Sira");
            myView3.SutunGizle(nameof(DepoStokHareket.BarGui));

            myGrid3.GridYerlesimYukle();

            myView3.SutunEditAc(nameof(DepoStokHareket.PartiNo));
            myView3.SutunEditAc(nameof(DepoStokHareket.GirisMiktar));
            myView3.SutunEditAc(nameof(DepoStokHareket.LotNo));
            myView3.SutunEditAc(nameof(DepoStokHareket.UretimTarihi));
            myView3.SutunEditAc(nameof(DepoStokHareket.SonKulTarihi));
            myView3.SutunCaptionColor(nameof(DepoStokHareket.GirisMiktar), Color.Green);
            myView3.SutunCaptionColor(nameof(DepoStokHareket.PartiNo), Color.Green);
            myView3.SutunCaptionColor(nameof(DepoStokHareket.LotNo), Color.Green);
            myView3.SutunCaptionColor(nameof(DepoStokHareket.UretimTarihi), Color.Green);
            myView3.SutunCaptionColor(nameof(DepoStokHareket.SonKulTarihi), Color.Green);
            myView3.SutunFormat(nameof(DepoStokHareket.UretimTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy");
            myView3.SutunFormat(nameof(DepoStokHareket.SonKulTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy");


        }

        private void AktarRowa() {
            if (fisno==0) {
                FisNoAl();
            }
            _fis=new DepoStokFis();
           
            _fis.FisTuru = IslemTuru;
            _fis.FisSeri = "IA";
            _fis.FisNo = fisno;
            _fis.Aciklama = "üretim iade";
            _fis.Tarih = DateTime.Now;
            if (YeniKayit) {
                _fis.Personel = Ortak.KullaniciAdi;
                _fis.KayitEden = Ortak.KullaniciAdi;
                _fis.KayitTarihi = DateTime.Now;
            }
            else {
                _fis.Degistiren = Ortak.KullaniciAdi;
                _fis.DegistirmeTarihi = DateTime.Now;
            }
        }
        private void FisNoAl() {
            var rs = _mngDep.FisNoAl(IslemTuru, "IA");
            if (!rs.Success) {
                rs.Message.MesajHata();
                return;
            }
            fisno = rs.Data ;
        }

        private void BtnKaydet_Click(object sender, EventArgs e) {

            foreach (var itm in _list) {
                if (itm.StGuid==null || itm.StGuid==Guid.Empty) {
                    MesajHata(itm.StokKodu +"-" + itm.StokAdi +"   Stok Depoda Kayıtlı Değil. ");
                }
            } 
            Kaydet();
        }
        private void  Kaydet() {
            if (myView1.IsEditing == true) {
                myView1.CloseEditor();
            } // UIbutton gride end yaptıramıyor 
         
            AktarRowa(); 
            var rs = _mngDep.KaydetDepoStokFis(_fis, _list, YeniKayit, false);
            if (!rs.Success) {
                rs.Message.MesajHata();
                return  ;
            }
            else {
                var dt = rs.Data;
                string seri = dt.FisSeri;
                string sira = dt.FisNo.ToString(); 
                var son = _mngSip.SiparisIptalEntGuncelle(_mdl.Siparis.Id, seri, sira );
                if (!son.Success) {
                    MesajHata(son.Message);
                    return;
                }
                MesajBilgi("KayıtEdildi");
                BtnKaydet.Enabled = false;
            }
            
        }
    }
}
