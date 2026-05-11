using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.Personeller;
using My.Core;
using My.Entities.Personeller;
using My.Entities.UretimIstasyonlar;
using My.Kontrol.Formlar;
using System;
using System.Linq;

namespace MyUI.UretimIstasyonModule
{
    public partial class FrmUretimIstasyonUretimGir : MyFrmKayit
    {
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly IPersonelService _srvPersonel = Ortak.DbPro.Personel;
        private UretimIstasyonManager _mng;
        private UretimIstasyonHareket _mdl;
        public UretimIstasyon MdlIst;
        public Action Action;
        private double _editMiktar = 0;
        public FrmUretimIstasyonUretimGir()
        {
            InitializeComponent();
            EventlerBagla();
        }
        private void EventlerBagla()
        {
            this.Load += Frm_Load;
            this.CmbPersonelKodu.Leave += CmbPersonelKodu_Leave;
            this.CmbPersonelAdi.Leave += CmbPersonelAdi_Leave;
            this.CmbPersonelKodu.Popup += CmbPersonelKodu_Popup;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new UretimIstasyonManager(Ortak.DbPro);
            BaglaPersonel();
            Bagla();
            AcilisBittimi = true;
        }
        private void CmbPersonelKodu_Popup(object sender, EventArgs e)
        {
            CmbPersonelKodu.Properties.ForceInitialize();
        }
        private void CmbPersonelKodu_Leave(object sender, EventArgs e)
        {
            if (AcilisBittimi)
            {
                AcilisBittimi = false;
                var mdl = (Personel)CmbPersonelKodu.GetSelectedDataRow();
                if (mdl != null)
                {
                    CmbPersonelAdi.Text = mdl.Adi.ToString();
                }
                AcilisBittimi = true;
            }
        }
        private void CmbPersonelAdi_Leave(object sender, EventArgs e)
        {
            if (AcilisBittimi)
            {
                AcilisBittimi = false;
                var mdl = (Personel)CmbPersonelAdi.GetSelectedDataRow();
                if (mdl != null)
                {
                    CmbPersonelKodu.Text = mdl.Kodu.ToString();
                }
                AcilisBittimi = true;
            }
        }
        private void Bagla()
        {
            if (IdGuid.IsNullOrEmpty())
            {
                if (MdlIst == null)
                {
                    FrmUretimIstasyonHareketList f = new FrmUretimIstasyonHareketList { SecimIcinAcildi = true };
                    f.ShowDialog();
                    if (f.Secildi)
                    {
                        var secilen = f.SecilenRow as UretimIstasyon;
                        MdlIst = secilen;
                    }
                    else
                    {
                        this.Close();
                        return;
                    }
                }
                TemizleText();
                IstasyonuBagla();
            }
            else
            {
                var rsIH = _mng.GetUretimIstasyonHareket(IdGuid);
                if (!rsIH.Success)
                {
                    MesajHata(rsIH.Message);
                    return;
                }
                _mdl = rsIH.Data;
                var rsI = _mng.GetUretimIstasyon(_mdl.UrIId);
                if (!rsI.Success)
                {
                    MesajHata(rsI.Message);
                    return;
                }
                MdlIst = rsI.Data;
                AktarTextlere();
                IstasyonuBagla();
            }
        }
        private void BaglaPersonel()
        {
            var rs = _srvPersonel.SelectListWhere();
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();

            //foreach (var itm in dt)  { }

            CmbPersonelKodu.MyDataBagla(dt, "Kodu", "Kodu", new string[] { "Kodu", "Adi" });
            CmbPersonelAdi.MyDataBagla(dt, "Adi", "Adi", new string[] { "Kodu", "Adi" });
        }
        private void IstasyonuBagla()
        {
            if (MdlIst == null)
            {
                return;
            }
            TxtOperasyonAdi.Text = MdlIst.OperasyonAdi;
            TxtOperasyonKodu.Text = MdlIst.OperasyonKodu;
            TxtIstasyonAdi.Text = MdlIst.IstasyonAdi;
            TxtIstasyonKodu.Text = MdlIst.IstasyonKodu;
            TxtReceteAdi.Text = MdlIst.ReceteAdi;
            TxtReceteKodu.Text = MdlIst.ReceteKodu;
            TxtPlanlananMiktar.Text = MdlIst.PlanlananMiktar.ToString();
            TxtLblUretimMiktari.Text = MdlIst.UretimMiktari.ToString();
        }
        private void TemizleText()
        {
            _mdl = new UretimIstasyonHareket() { Id = Guid.Empty };
            IdGuid = Guid.Empty;
            TxtUretimMiktari.Text = "0";
            TxtFireMiktari.Text = "0";
            TxtTarih.Text = DateTime.Now.ToString();
            CmbPersonelKodu.Text = "";
            CmbPersonelAdi.Text = "";
        }
        private void AktarTextlere()
        {
            IdGuid = _mdl.Id;
            _editMiktar = _mdl.UretimMiktari;
            TxtUretimMiktari.Text = _mdl.UretimMiktari.ToString();
            TxtFireMiktari.Text = _mdl.FireMiktari.ToString();
            TxtTarih.Text = _mdl.Tarih.ToString();
            CmbPersonelKodu.Text = _mdl.PersonelKodu;
            CmbPersonelAdi.Text = _mdl.PersonelAdi;
        }
        private void AktarModele()
        {
            if (_mdl == null)
            {
                _mdl = new UretimIstasyonHareket() { Id = Guid.Empty };
            }

            _mdl.UretimMiktari = TxtUretimMiktari.MyDegerDouble();
            _mdl.FireMiktari = TxtFireMiktari.MyDegerDouble();
            _mdl.Tarih = Convert.ToDateTime(TxtTarih.EditValue);
            _mdl.PersonelKodu = CmbPersonelKodu.Text;
            _mdl.PersonelAdi = CmbPersonelAdi.Text;
            _mdl.UrId = MdlIst.UrId;
            _mdl.UrOId = MdlIst.UrOId;
            _mdl.UrOHId = MdlIst.UrOHId;
            _mdl.UrOHDId = MdlIst.UrOHDId;
            _mdl.UrIId = MdlIst.Id;
            _mdl.RcAId = MdlIst.RcAId;
            _mdl.RcOId = MdlIst.RcOId;
            _mdl.SipId = MdlIst.SipId;

            if (!IdGuid.IsNullOrEmpty())
            {
                _mdl.KayitEden = Ortak.KullaniciAdi;
                _mdl.KayitTarihi = DateTime.Now;
            }
            else
            {
                _mdl.Degistiren = Ortak.KullaniciAdi;
                _mdl.DegistirmeTarihi = DateTime.Now;
            }
            if (IdGuid == Guid.Empty) IdGuid = MyGuid.NewGuid();
            _mdl.Id = IdGuid;
        }
        private void Kaydet()
        {
            if (!TextLeriKontrolEt())
            {
                return;
            }
            AktarModele();
            if (!MiktarKontrolEt())
            {
                return;
            }
            var rs = _mng.UretimIstasyonHareketKaydet(_mdl);
            if (rs.Success)
            {
                KayitEdildi = true;
                Action?.Invoke();
                this.Close();
                return;
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        private void Sil()
        {
            var rs = _mng.UretimIstasyonHareketSil(_mdl);
            if (rs.Success)
            {
                KayitEdildi = true;
                Action?.Invoke();
                this.Close();
                return;
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        private bool TextLeriKontrolEt()
        {
            //if (string.IsNullOrEmpty(TxtTarih.Text)) { MesajHata("Lütfen tarih giriniz"); return false; } 
            if (TxtUretimMiktari.MyDegerInteger() < 1)
            {
                MesajHata("Lütfen üretim miktarını giriniz");
                return false;
            }
            return true;
        }
        private bool MiktarKontrolEt()
        {
            double hesaplanan;
            if (!IdGuid.IsNullOrEmpty())
            {
                hesaplanan = (MdlIst.PlanlananMiktar - (MdlIst.UretimMiktari - _editMiktar));
                if (_mdl.UretimMiktari > hesaplanan)
                {
                    MesajHata($"üretim miktarı en fazla {hesaplanan + _editMiktar} olabilir ");
                    return false;
                }
            }
            else
            {
                hesaplanan = (MdlIst.PlanlananMiktar - MdlIst.UretimMiktari);
                if (_mdl.UretimMiktari > hesaplanan)
                {
                    MesajHata($"üretim miktarı en fazla {hesaplanan} olabilir ");
                    return false;
                }
            }
            return true;
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            Kaydet();
        }
        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            if (_mdl.Id.IsNullOrEmpty())
            {
                MesajBilgi("Silinecek Kayıt Seçilmemiş Kayda Çift Tıklayın");
            }
            Sil();
        }
    }
}