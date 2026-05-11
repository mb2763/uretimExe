using My.Business.Manager;
using My.Business.Service.UretimIstasyonlar;
using My.Business.Service.UretimOperasyonlar;
using My.Core;
using My.Core.Result;
using My.Entities.IstasyonKartlar;
using My.Entities.Models;
using My.Entities.UretimIstasyonlar;
using My.Entities.UretimOperasyonlar;
using My.Kontrol.Formlar;
using MyUI.IstasyonModul;
using MyUI.UretimOperasyonModule;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.UretimIstasyonModule
{
    public partial class FrmUretimIstasyonED : MyFrmKayitFull
    {

        public OperasyonTuruEnum OperasyonTuru { get; set; } = OperasyonTuruEnum.IstasyonEkle;
        public Guid? OprId = Guid.Empty;
        public Action Action;
        private readonly IUretimIstasyonService _srv = Ortak.DbPro.UretimIstasyon;
        private readonly IUretimOperasyonHareketDetayService _srvDetay = Ortak.DbPro.UretimOperasyonHareketDetay;
        private readonly IUretimOperasyonHareketService _srvHareket = Ortak.DbPro.UretimOperasyonHareket;
        private readonly IUretimOperasyonService _srvOperasyon = Ortak.DbPro.UretimOperasyon;
        private UretimOperasyonManager _mngOperasyon;
        private UretimTakipManagerV2 _mng;
        private UretimEmriManager _mngSip;
        private UretimEmriKayitModelSiparis Model;
        private UretimOperasyonHareket Hareket { get; set; }
        private UretimOperasyonHareket HareketYeni { get; set; }
        private UretimOperasyonHareketDetay Detay { get; set; }
        private List<UretimIstasyon> Istasyonlar { get; set; }
        public FrmUretimIstasyonED()
        {
            InitializeComponent();
            this.Load += Frm_Load;
            this.SizeChanged += new System.EventHandler(this.FrmUretimIstasyonED_SizeChanged);
            BtnKaydet.Click += BtnKaydet_Click;
        }

        #region MyRegion

        private void Frm_Load(object sender, EventArgs e)
        {
            operasyonIstasyonControlV21.MyIstasyonEkleEvent += MyIstasyonEkleEventCompleted;
            operasyonIstasyonControlV21.MyIstasyonSilEvent += MyIstasyonSilEventCompleted;
            _mng = new UretimTakipManagerV2(Ortak.DbPro);
            _mngOperasyon = new UretimOperasyonManager(Ortak.DbPro);
            if (Model == null && (!OprId.IsNullOrEmpty()))
            {
                _mngSip = new UretimEmriManager(Ortak.DbPro);
                var rs = _mngSip.GetUretimOperasyonSiparisEdit(OprId);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                Model = rs.Data;
            }
            var rshtr = _srvHareket.GetViewListWhere(" where UrOH.Id ='" + OprId + "'");
            if (!rshtr.Success)
            {
                MesajHata(rshtr.Message);
                return;
            }
            Hareket = rshtr.Data.FirstOrDefault();
            if (Hareket == null)
            {
                MesajHata("Hareket Bulunamadı ..");
                return;
            }
            if (OperasyonTuru == OperasyonTuruEnum.IstasyonEkle)
            {
                UretimEkle();
            }
            else if (OperasyonTuru == OperasyonTuruEnum.SonrakiOperasyon)
            {
                SonrakiOperasyonBaslat();
            }
            else if (OperasyonTuru == OperasyonTuruEnum.Degistir)
            {
                DegistirBagla();
            }
        }
        private void UretimEkle()
        {
            Istasyonlar = new List<UretimIstasyon>();
            Detay = new UretimOperasyonHareketDetay
            {
                Id = MyGuid.NewGuid(),
                Turu = "Acilis",
                ReceteKodu = Hareket.ReceteKodu,
                ReceteAdi = Hareket.ReceteAdi,
                OperasyonKodu = Hareket.OperasyonKodu,
                OperasyonAdi = Hareket.OperasyonAdi,
                Tarih = DateTime.Now,
                PlanlananMiktar = 0,
                IslemdekiMiktar = 0,
                UretimMiktari = 0,
                FireMiktari = 0,
                UrId = Hareket.UrId,
                UrOId = Hareket.UrOId,
                UrOHId = Hareket.Id,
                RcAId = Hareket.RcAId,
                RcOId = Hareket.RcOId,
                SipId = Hareket.SipId,

            };
            UretimBaslat(false);
        }
        private void UretimBaslat(bool degistir)
        {
            PanelOrta.Controls.Clear();
            OperasyonIstasyonControlV2 cnt = operasyonIstasyonControlV21;
            cnt.IdGuid = Guid.Empty;
            cnt.RcOId = Detay.RcOId;
            cnt.ReceteKodu = Detay.ReceteKodu;
            cnt.ReceteAdi = Detay.ReceteAdi;
            cnt.Operasyon = Detay.OperasyonKodu;
            if (degistir)
            {
                cnt.Miktar = (Hareket.PlanlananMiktar - Hareket.IslemdekiMiktar) + Detay.IslemdekiMiktar;
            }
            else
            {
                cnt.Miktar = Hareket.PlanlananMiktar - Hareket.IslemdekiMiktar;
            }
            var md = Model.OperasyonModeller.Find(c => c.Recete.Id == Detay.RcAId);
            cnt.oprKayMdl = md;
            cnt.IstasyonHareketler = IstasyonlarOlustur(Hareket);
            cnt.Bagla();
            PanelOrta.Controls.Add(cnt);
            if (cnt.IstasyonHareketler.Count <= 0)
            {
                IstasyonSec();
            }
        }
        private void SonrakiOperasyonBaslat()
        {
            var rsara = _srvHareket.SelectFirst(c => c.Id == OprId);
            if (!rsara.Success)
            {
                MesajHata("Operasyon Hareket  Bulunamadı");
                return;
            }
            var buOpHr = rsara.Data;
            var rsOnceki = _srvOperasyon.SelectFirst(c => c.Id == buOpHr.UrOId);
            if (!rsOnceki.Success)
            {
                MesajHata("Onceki Operasyon  Bulunamadı");
                return;
            }
            var OncekiOperasyon = rsOnceki.Data;
            int sira = OncekiOperasyon.Sira + 1;
            var rsSonraki = _srvOperasyon.SelectFirst(c => c.UrId == buOpHr.UrId && c.SipHId == OncekiOperasyon.SipHId && c.Sira == sira);
            if (!rsSonraki.Success)
            {
                MesajHata("Sonraki Operasyon  Bulunamadı");
                return;
            }
            var SonrakiOperasyon = rsSonraki.Data;
            var rs = _mng.GetSiparisKalanKontrol(Hareket.UrId);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            HareketYeni = new UretimOperasyonHareket
            {
                Id = MyGuid.NewGuid(),
                ReceteKodu = SonrakiOperasyon.ReceteKodu,
                ReceteAdi = SonrakiOperasyon.ReceteAdi,
                OperasyonKodu = SonrakiOperasyon.OperasyonKodu,
                OperasyonAdi = SonrakiOperasyon.OperasyonAdi,
                BaslangicTarihi = DateTime.Now,
                PlanlananMiktar = 0,
                IslemdekiMiktar = 0,
                UretimMiktari = 0,
                FireMiktari = 0,
                IptalMiktari = 0,
                UrId = SonrakiOperasyon.UrId,
                UrOId = SonrakiOperasyon.Id,
                RcAId = SonrakiOperasyon.RcAId,
                RcOId = SonrakiOperasyon.RcOId,
                SipId = SonrakiOperasyon.SipId,

                Sira = SonrakiOperasyon.Sira
            };
            double oncedenUretilen = 0;
            double operasyondaolan = 0;
            foreach (var itm in rs.Data)
            {
                if (itm.RcAId == SonrakiOperasyon.RcAId && itm.RcOId == SonrakiOperasyon.RcOId &&
                    itm.Sira == SonrakiOperasyon.Sira && itm.UrId == SonrakiOperasyon.UrId && itm.UrOId == SonrakiOperasyon.Id)
                {
                    operasyondaolan = itm.PlanlananMiktar;
                }
                else if (itm.RcAId == OncekiOperasyon.RcAId && itm.RcOId == OncekiOperasyon.RcOId &&
                         itm.Sira == OncekiOperasyon.Sira && itm.UrId == OncekiOperasyon.UrId && itm.UrOId == OncekiOperasyon.Id)
                {
                    oncedenUretilen = itm.UretimMiktari;
                }
            }
            HareketYeni.PlanlananMiktar = oncedenUretilen - operasyondaolan;
            Istasyonlar = new List<UretimIstasyon>();
            Detay = new UretimOperasyonHareketDetay
            {
                Id = MyGuid.NewGuid(),
                Turu = "Acilis",
                ReceteKodu = Hareket.ReceteKodu,
                ReceteAdi = Hareket.ReceteAdi,
                OperasyonKodu = Hareket.OperasyonKodu,
                OperasyonAdi = Hareket.OperasyonAdi,
                Tarih = DateTime.Now,
                IslemdekiMiktar = 0,
                UretimMiktari = 0,
                FireMiktari = 0,
                IptalMiktari = 0,
                UrId = Hareket.UrId,
                UrOId = Hareket.UrOId,
                UrOHId = Hareket.Id,
                RcAId = Hareket.RcAId,
                RcOId = Hareket.RcOId,
                SipId = Hareket.SipId,
            };
            SonrakiOperasyonAyarla();
        }
        private void SonrakiOperasyonAyarla()
        {
            PanelOrta.Controls.Clear();
            OperasyonIstasyonControlV2 cnt = operasyonIstasyonControlV21;
            OprId = HareketYeni.Id;
            cnt.IdGuid = Guid.Empty;
            cnt.RcOId = HareketYeni.RcOId;
            cnt.ReceteKodu = HareketYeni.ReceteKodu;
            cnt.ReceteAdi = HareketYeni.ReceteAdi;
            cnt.Operasyon = HareketYeni.OperasyonKodu;
            cnt.Miktar = HareketYeni.PlanlananMiktar;
            var md = Model.OperasyonModeller.Find(c => c.Recete.Id == HareketYeni.RcAId);
            cnt.oprKayMdl = md;
            cnt.IstasyonHareketler = IstasyonlarOlustur(HareketYeni);
            cnt.Bagla();
            PanelOrta.Controls.Add(cnt);
        }
        private void DegistirBagla()
        {
            var rs = _srvDetay.GetViewListWhere(" where UrOHD.Id = '" + IdGuid + "'");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            Detay = rs.Data.FirstOrDefault();
            if (Detay == null)
            {
                MesajHata("Hareket Bulunamadı ..");
                return;
            }
            var rsist = _srv.GetViewListWhere(" where UrI.UrOHDId = '" + IdGuid + "'");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            Istasyonlar = rsist.Data.ToList() ?? new List<UretimIstasyon>();
            UretimBaslat(true);
        }
        private List<UretimIstasyon> IstasyonlarOlustur(UretimOperasyonHareket opm)
        {
            List<UretimIstasyon> istasyonHareketler = new List<UretimIstasyon>();
            foreach (var itm in Istasyonlar)
            {
                if (opm.Id == itm.UrOHId)
                {
                    istasyonHareketler.Add(itm.Clone());
                }
            }
            List<UretimIstasyon> istasyonHareketKontrol = new List<UretimIstasyon>();
            var md = Model.OperasyonModeller.Find(c => c.Recete.Id == opm.RcAId);
            foreach (var opr in md.Operasyonlar)
            {
                if (opr.Id == opm.RcOId)// operasyon gelen operasyon harekete eşit ve ilk sirada ise
                {
                    foreach (var ist in md.Istasyonlar)
                    {
                        if (opr.Id == ist.RcOId)// istasyon operasyona baglı ve ilk sirada ise ekle
                        {
                            istasyonHareketKontrol.Add(new UretimIstasyon()
                            {
                                KayitEden = "",
                                Degistiren = "",
                                KayitTarihi = null,
                                DegistirmeTarihi = null,
                                IstasyonAdi = ist.IstasyonAdi,
                                IstasyonKodu = ist.IstasyonKodu,
                                BaslangicTarihi = DateTime.Now,
                                BitisTarihi = null,
                                FireMiktari = 0,
                                Id = MyGuid.NewGuid(),
                                UrId = opm.UrId,
                                UrOId = opm.UrOId,
                                UrOHId = opm.Id,
                                UrOHDId = Detay.Id,
                                SipId = Detay.SipId,
                                RcAId = ist.RcAId,
                                RcOId = ist.RcOId,
                                RcIstId = ist.Id,
                                UretimMiktari = 0,
                                PlanlananMiktar = 0,
                                Fason = ist.Fason,

                            });
                        }
                    }
                }
            }
            if (istasyonHareketler.Count < 1)
            {
                istasyonHareketler.InsertRange(0, istasyonHareketKontrol);
            }
            else
            {
                List<UretimIstasyon> istEklenecek = new List<UretimIstasyon>();
                foreach (var knt in istasyonHareketKontrol)
                {
                    foreach (var ist in istasyonHareketler)
                    {
                        if (knt.RcOId == ist.RcOId && knt.RcIstId != ist.RcIstId)
                        {
                            var eklenmismi = istEklenecek.Find(c => c.RcOId == knt.RcOId && c.RcIstId == knt.RcIstId);
                            var eklenmismihr = istasyonHareketler.Find(c => c.RcOId == knt.RcOId && c.RcIstId == knt.RcIstId);
                            if (eklenmismi == null && eklenmismihr == null)
                            {
                                istEklenecek.Add(knt.Clone());
                            }
                        }
                    }
                }
                istasyonHareketler.InsertRange(istasyonHareketler.Count, istEklenecek);
            }
            return istasyonHareketler;
        }
        private IDataResult<List<UretimIstasyon>> IstasyonlarAl()
        {
            List<UretimIstasyon> IstasyonHareketler = new List<UretimIstasyon>();
            foreach (var opr in PanelOrta.Controls)
            {
                var ot = opr.GetType();
                var orj = typeof(OperasyonIstasyonControlV2);
                if (ot == orj)
                {
                    var op = (OperasyonIstasyonControlV2)opr;
                    foreach (var ist in op.IstasyonHareketler)
                    {
                        if (!string.IsNullOrEmpty(ist.IstDurumu))
                        {

                            foreach (var itm in Istasyonlar)
                            {
                                if (itm.Id == ist.Id)
                                {
                                    if (itm.PlanlananMiktar != ist.PlanlananMiktar)
                                    {
                                        return new ErrorDataResult<List<UretimIstasyon>>(itm.IstasyonKodu + "- IstDurumu : İstasyon da işlem Yapılmış Miktar Değiştirilemez..");
                                    }
                                }
                            }
                        }

                        if (ist.PlanlananMiktar > 0)
                        {
                            if (ist.PlanlananMiktar < ist.UretimMiktari || ist.PlanlananMiktar < ist.FireMiktari)
                            {
                                return new ErrorDataResult<List<UretimIstasyon>>("Üretim Yapılmış hareketin planlanan miktari 0 olamaz");

                            }
                            var alinan = ist.Clone();
                            // alinan.UrOHDId = Guid.Empty;
                            IstasyonHareketler.Add(alinan);
                        }
                        else
                        {
                            if (ist.UretimMiktari > 0 || ist.FireMiktari > 0)
                            {
                                return new ErrorDataResult<List<UretimIstasyon>>("Üretim Yapılmış hareketin planlanan miktari 0 olamaz");
                            }
                        }
                    }
                }
            }
            return new SuccessDataResult<List<UretimIstasyon>>(IstasyonHareketler);
        }
        private bool Kaydet()
        {
            var rs = IstasyonlarAl();
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return false;
            }
            var ist = rs.Data;
            if (ist.Count < 1)
            {
                if (!MesajSor("Kayıt Edilecek İstasyon Bulunamadı \nİstasyon Girilmişse Planlanan Miktar Sıfır olanlari siler. \nDevam ederseniz uretim Beklemeye alınır."))
                {
                    return false;
                }
            }
            double planlananMiktar = 0;
            foreach (var itm in ist)
            {
                if (itm.Fason)
                {
                    if (string.IsNullOrEmpty(itm.FasonCariKodu))
                    {
                        MesajHata("Fason olarak Seçilen Hareketde CariKodu seçilmemiş..");
                        return false;
                    }
                }
                else
                {
                    itm.FasonCariKodu = "";
                    itm.FasonCariUnvani = "";
                }

                if (itm.PlanlananMiktar <= 0 && (itm.UretimMiktari > 0 || itm.FireMiktari > 0))
                {
                    MesajHata("Üretim girişi yapılmış planlama miktarı üretimden az olamaz.");
                    return false;
                }
                planlananMiktar += itm.PlanlananMiktar;
            }
            if (OperasyonTuru == OperasyonTuruEnum.Degistir)
            {
                if (planlananMiktar > ((Hareket.PlanlananMiktar - Hareket.IslemdekiMiktar) + Detay.IslemdekiMiktar))
                {
                    MesajHata(" Isleme Alınan miktar bekleyen miktardan fazla lütfen kontrol ediniz..");
                    return false;
                }
            }
            else if (OperasyonTuru == OperasyonTuruEnum.SonrakiOperasyon)
            {
                if (planlananMiktar > ((HareketYeni.PlanlananMiktar - HareketYeni.IslemdekiMiktar) + Detay.IslemdekiMiktar))
                {
                    MesajHata(" Isleme Alınan miktar bekleyen miktardan fazla lütfen kontrol ediniz..");
                    return false;
                }
            }
            else
            {
                if (planlananMiktar > (Hareket.PlanlananMiktar - Hareket.IslemdekiMiktar))
                {
                    MesajHata(" Isleme Alınan miktar bekleyen miktardan fazla lütfen kontrol ediniz..");
                    return false;
                }
            }
            Detay.PlanlananMiktar = planlananMiktar;
            Detay.IslemdekiMiktar = planlananMiktar;
            if (Detay.PlanlananMiktar <= 0 && (Detay.UretimMiktari > 0 || Detay.FireMiktari > 0))
            {
                MesajHata("Üretim girişi yapılmış planlama miktarı üretimden az olamaz.");
                return false;
            }

            if (OperasyonTuru == OperasyonTuruEnum.SonrakiOperasyon)
            {
                return KaydetSonraki(ist);
            }
            return KaydetNormal(ist);
        }
        private bool KaydetNormal(List<UretimIstasyon> ist)
        {
            List<UretimIstasyon> silinenler = new List<UretimIstasyon>();
            foreach (var itm in Istasyonlar)
            {
                var f = ist.FirstOrDefault(c => c.Id == itm.Id);
                if (f == null)
                {
                    silinenler.Add(itm);
                }
            }

            var rs = _mngOperasyon.KaydetNormal(ist, Detay, silinenler);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return false;
            }
            return true;
        }
        private bool KaydetSonraki(List<UretimIstasyon> ist)
        {
            var rs = _mngOperasyon.KaydetSonraki(ist, Detay, HareketYeni);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return false;
            }
            return true;

        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (Kaydet())
            {
                Action?.Invoke();
                this.Close();
            }
        } 
        private void FrmUretimIstasyonED_SizeChanged(object sender, EventArgs e)
        {
            operasyonIstasyonControlV21.Width = this.Width - 70;
            operasyonIstasyonControlV21.Height = this.Width - 140;

        }
        private void MyIstasyonEkleEventCompleted(object sender, object e)
        {

            IstasyonSec();
        } 
        private void IstasyonSec()
        {
            FrmIstasyonKartList f = new FrmIstasyonKartList();
            f.SecimIcinAcildi = true;
            f.BagliIstasyonSec = true;
            f.RcAId = Hareket.RcAId;
            f.ShowDialog();
            if (f.Secildi)
            {
                var ist = ((IstasyonKarti)f.SecilenRow);

                var opm = Hareket;
                var md = Model.OperasyonModeller.Find(c => c.Recete.Id == opm.RcAId);
                foreach (var opr in md.Operasyonlar)
                {
                    if (opr.Id == opm.RcOId)// operasyon gelen operasyon harekete eşit ve ilk sirada ise
                    {
                        OperasyonIstasyonControlV2 cnt = operasyonIstasyonControlV21;
                        cnt.IstasyonHareketler.Add(new UretimIstasyon()
                        {
                            KayitEden = "",
                            Degistiren = "",
                            KayitTarihi = null,
                            DegistirmeTarihi = null,
                            IstasyonAdi = ist.IstasyonAdi,
                            IstasyonKodu = ist.IstasyonKodu,
                            BaslangicTarihi = DateTime.Now,
                            BitisTarihi = null,
                            FireMiktari = 0,
                            Id = MyGuid.NewGuid(),
                            UrId = opm.UrId,
                            UrOId = opm.UrOId,
                            UrOHId = opm.Id,
                            UrOHDId = Detay.Id,
                            SipId = Detay.SipId,
                            RcAId = opm.RcAId,
                            RcOId = opm.RcOId,
                            RcIstId = Guid.Empty,
                            UretimMiktari = 0,
                            PlanlananMiktar = 0,
                            Fason = false,
                        });
                        cnt.GridBagla();

                    }
                }

            }

        } 
        private void MyIstasyonSilEventCompleted(object sender, object e)
        {
            // e as  UretimIstasyon 
            if (!(e is UretimIstasyon)) return;
            var ist = ((UretimIstasyon)e);
            OperasyonIstasyonControlV2 cnt = operasyonIstasyonControlV21;
            List<UretimIstasyon> lis = cnt.IstasyonHareketler.Where(c => c.Id != ist.Id).ToList();
            cnt.IstasyonHareketler.Clear();
            cnt.IstasyonHareketler.InsertRange(0, lis);
            cnt.GridBagla();
        }

        #endregion


    }
}