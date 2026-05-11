using My.Business;
using My.Business.Manager;
using My.Business.Service.MikroModul;
using My.Core;
using My.Entities.Mikro;
using My.Entities.Models;
using My.Entities.Receteler;
using My.Entities.Siparisler;
using My.Kontrol.Formlar;
using MyUI.UretimModule;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.MikroModule
{
    public partial class FrmMikroUretimEmriOlustur : MyFrmKayit
    {
        private readonly DatabaseFactoryPro _dbPro = Ortak.DbPro;
        private readonly IMikroSiparisService _srvSipMikro = Ortak.DbMikro.Siparisler;
        private readonly IMikroSiparisHareketService _srvSipHarektMikro = Ortak.DbMikro.SiparisHareketler;
        private SiparisManager _mng;
        private ReceteManager _mngRecete;
        private SiparisKayitModel _mdl;
        public Guid? SipGuid { get; set; }
        public Action Action;
        private MikroSiparis _mdlSipMikro;
        private List<MikroSiparisHareket> _mdlSipHrMikro;
        public FrmMikroUretimEmriOlustur()
        {
            InitializeComponent();
        }
        private void FrmMikroUretimEmriOlustur_Load(object sender, EventArgs e)
        {
            MikroSipBagla();
            BaglaSiparis();
        }
        public void MikroSipBagla()
        {
            var rs1 = _srvSipMikro.GetViewListWhere($" where Sip.SipGuid ='{SipGuid.ToString().ToUpper()}'");
            if (!rs1.Success)
            {
                MesajHata(rs1.Message);
            }
            _mdlSipMikro = rs1.Data.FirstOrDefault();
            var rs2 = _srvSipHarektMikro.GetViewListSeriSira(_mdlSipMikro.EvrakSeri, _mdlSipMikro.EvrakSira);
            if (!rs2.Success)
            {
                MesajHata(rs2.Message);
            }
            _mdlSipHrMikro = rs2.Data.ToList();
            myGrid1.DataSource = rs1.Data;
            myGrid2.DataSource = rs2.Data;
        }
        public void BaglaSiparis()
        {
            if (_mng == null)
            {
                _mng = new SiparisManager(Ortak.DbPro, Ortak.DbMikro);
            }
            if (_mngRecete == null)
            {
                _mngRecete = new ReceteManager(Ortak.DbPro);
            }
            YeniKayit = true;
            _mdl = _mng.GetSiparis();
            _mdl.Siparis.Turu = "MikroSiparis";
            _mdl.Siparis.SiparisKodu = _mdlSipMikro.EvrakSeri.Trim() + _mdlSipMikro.EvrakSira;
            _mdl.Siparis.CariKodu = _mdlSipMikro.CariKodu.Trim();
            _mdl.Siparis.CariUnvani = _mdlSipMikro.CariUnvani.Trim();
            _mdl.Siparis.Tarih = _mdlSipMikro.Tarih;
            _mdl.Siparis.TeslimTarihi = _mdlSipMikro.TeslimTarihi;
            _mdl.Siparis.Miktar = _mdlSipMikro.Miktar;
            _mdl.Siparis.Aciklama = "Mikro siparişi";
            _mdl.Siparis.Notu = "Mikro siparişi";
            _mdl.Siparis.Kapandi = false;
            _mdl.Siparis.Durumu = "Beklemede";
            _mdl.Siparis.EntKayitSeri = _mdlSipMikro.EvrakSeri;
            _mdl.Siparis.EntKayitSira = _mdlSipMikro.EvrakSira;
            _mdl.Siparis.EntKayitGuid = _mdlSipMikro.SipGuid;
            SiparisiAyarla();
        }
        public void SiparisiAyarla()
        {
            foreach (var itm in _mdlSipHrMikro)
            {
                ReceteTekSec(itm);
            }
        }
        public void ReceteTekSec(MikroSiparisHareket sipH)
        {
            var rsA = _dbPro.ReceteAna.SelectFirst(c => c.EntegreStokKodu == sipH.StokKodu);
            if (!rsA.Success)
            {
                MesajHata(rsA.Message);
            }
            if (rsA.Data == null)
            {
                MesajHata(sipH.StokKodu + " - " + sipH.StokAdi + " Ürüne ait Reçete Bulunamadı");
                this.Close();
                return;
            }
            var st = rsA.Data.Clone();
            var rec = _mngRecete.GetReceteKayit(st.Id);
            if (!rec.Success)
            {
                MesajHata(rec.Message);
                return;
            }
            var hareket = new SiparisHareket();
            var Detaylar = new List<SiparisHareketDetay>();
            var Recete = rec.Data;
            hareket.SipId = _mdl.Siparis.Id;
            hareket.ReceteGrupKodu = "";
            hareket.ReceteKodu = Recete.Recete.ReceteKodu;
            hareket.ReceteAdi = Recete.Recete.ReceteAdi;
            hareket.RcAId = Recete.Recete.Id;
            hareket.Miktar = sipH.Miktar;
            hareket.EntKayitSeri = sipH.EvrakSeri;
            hareket.EntKayitSira = sipH.EvrakSira;
            hareket.EntKayitGuid = sipH.SipHGuid;
            hareket.YeniKayit = false;
            bool ilk = true;
            SiparisHareketDetay det;
            foreach (ReceteDetay itm in Recete.ReceteDetaylar)
            {
                if (ilk)
                {
                    hareket.StokKodu = itm.VarsayilanStokKodu;
                    hareket.StokAdi = itm.VarsayilanStokAdi;
                    hareket.Birim = itm.Birim;
                    hareket.Renk = itm.Renk;
                    hareket.Beden = itm.Beden;
                    ilk = false;
                }

                det = new SiparisHareketDetay
                {
                    Id = MyGuid.NewGuid(),
                    Cinsi = itm.Cinsi,
                    StokKodu = itm.VarsayilanStokKodu,
                    StokAdi = itm.VarsayilanStokAdi,
                    Birim = itm.Birim,
                    Renk = itm.Renk,
                    Beden = itm.Beden,
                    Miktar = itm.Miktar,
                    Aciklama = itm.Aciklama,
                    SipHId = hareket.Id,
                    RcAId = itm.RcAId,
                    RcDId = itm.Id,
                    SipId = hareket.SipId
                };
                det.SipHId = hareket.Id;
                Detaylar.Add(det);
            }
            _mdl.Hareketler.Add(hareket);
            _mdl.Detaylar.InsertRange(0, Detaylar);
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            var rs = _mng.SiparisKaydet(_mdl, YeniKayit);
            if (rs.Success)
            {
                KayitEdildi = true;
                Action?.Invoke();

                FrmUretimEmriED f = new FrmUretimEmriED { SipId = rs.Data.Siparis.Id, UretimTuru = "MikroSiparis" };
                this.Hide();
                f.ShowDialog();
                this.Close();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
    }
}