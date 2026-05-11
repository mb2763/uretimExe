using My.Business.Manager;
using My.Core;
using My.Entities.IstasyonBakimlar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.IstasyonModul
{
    public partial class FrmIstasyonBakimEkle : MyFrmKayit
    {
        IstasyonBakimManager _mng;
        public Action ActionAktar;
        IstasyonBakim _mdl;
        List<IstasyonBakimParca> _parcalar;
        public FrmIstasyonBakimEkle()
        {
            InitializeComponent();
            EventlerBagla();
        }
        private void EventlerBagla()
        {
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new IstasyonBakimManager(Ortak.DbPro, Ortak.DbMikro);
            BaglaIstasyonList();
            Bagla();
        }
        public void Bagla()
        {
            if (IdGuid.IsNullOrEmpty())
            {
                YeniKayit = true;
                _mdl = new IstasyonBakim();
                _parcalar = new List<IstasyonBakimParca>();
                GridBagla();
                TemizleText();
            }
            else
            {
                YeniKayit = false;
                var rs = _mng.IstBakimService.SelectFirst(c => c.Id == IdGuid);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                _mdl = rs.Data;
                BaglaParcaList();
                GridBagla();

                AktarTextlere();
            }
        }
        public void BaglaParcaList()
        {
            var rs = _mng.IstParcaService.SelectList(c => c.IstBakId == IdGuid);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            _parcalar = rs.Data.ToList();
        }
        public void GridBagla()
        {

            myGrid1.DataSource = null;
            myGrid1.DataSource = _parcalar;
            myView1.SutunGizle("Id");
            myView1.SutunGizle("IstBakId");
            myGrid1.GridYerlesimYukle();
            myView1.SutunReadOnlyKapat("Parca");
            myView1.SutunReadOnlyKapat("ParcaNo");
            myView1.SutunReadOnlyKapat("EvrakNo");
            myView1.SutunReadOnlyKapat("Aciklama");
            myView1.SutunReadOnlyKapat("Garanti");
        }
        public void BaglaIstasyonList()
        {
            var rs = _mng.IstKartService.SelectListWhere("");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data;
            foreach (var itm in dt)
            {
                CmbIstasyonKodu.Properties.Items.Add(itm.IstasyonKodu);
            }
        }
        public void AktarTextlere()
        {
            CmbIstasyonKodu.Text = _mdl.IstasyonKodu;
            TxtPersonel.Text = _mdl.Personel;
            TxtIslemTuru.Text = _mdl.IslemTuru;
            TxtAciklama.Text = _mdl.Aciklama;
            TxtTarih.Text = _mdl.Tarih.ToString();
        }
        public void TemizleText()
        {
            CmbIstasyonKodu.Text = "";
            TxtPersonel.Text = "";
            TxtIslemTuru.Text = "";
            TxtAciklama.Text = "";
            TxtTarih.Text = DateTime.Now.ToString();
        }
        public void AktarModele()
        {
            if (IdGuid.IsNullOrEmpty())
            {
                IdGuid = MyGuid.NewGuid();
                _mdl.KayitEden = Ortak.KullaniciAdi;
                _mdl.KayitTarihi = DateTime.Now;

            }
            _mdl.Id = IdGuid;
            _mdl.IstasyonKodu = CmbIstasyonKodu.Text;
            _mdl.Personel = TxtPersonel.Text;
            _mdl.IslemTuru = TxtIslemTuru.Text;
            _mdl.Aciklama = TxtAciklama.Text;
            if (!TxtTarih.Text.IsNullOrEmpty())
            {
                _mdl.Tarih = Convert.ToDateTime(TxtTarih.Text);
            }
            else
            {
                _mdl.Tarih = null;
            }
            _mdl.Degistiren = Ortak.KullaniciAdi;
            _mdl.DegistirmeTarihi = DateTime.Now;
            foreach (var itm in _parcalar)
            {
                itm.IstBakId = _mdl.Id;
            }
        }
        public void Kaydet()
        {
            if (!TextLeriKontrolEt())
            {
                return;
            }

            AktarModele();
            var rs = _mng.Kaydet(_mdl, _parcalar);
            if (rs.Success)
            {
                KayitEdildi = true;
                ActionAktar?.Invoke();
                Close();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }

        public void Sil()
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            var rs = _mng.Sil(_mdl);
            if (rs.Success)
            {
                ActionAktar?.Invoke();
                KayitEdildi = true;
                Close();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        private bool TextLeriKontrolEt()
        {
            if (TxtTarih.Text.IsNullOrEmpty())
            {
                TxtTarih.EditValue = DateTime.Now;
            }


            if (string.IsNullOrEmpty(CmbIstasyonKodu.Text))
            {
                MesajHata("Lütfen Istasyon kodunu giriniz");
                return false;
            }


            return true;
        }

        private void BtnStokEkle_Click(object sender, EventArgs e)
        {
            FrmIstasyonBakimParcaEkle f = new FrmIstasyonBakimParcaEkle();
            f.Parca = new IstasyonBakimParca() { Id = MyGuid.NewGuid() };
            f.YeniKayit = true;
            f.ShowDialog();
            if (f.KayitEdildi)
            {
                _parcalar.Add(f.Parca.Clone());
                GridBagla();
            }
        }

        private void BtnStokSil_Click(object sender, EventArgs e)
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            var data = myView1.MyGetCurrentItem<IstasyonBakimParca>();
            if (data == null)
            {
                return;
            }
            var rc = _parcalar.Find(c => c.Id == data.Id);
            _parcalar.Remove(rc);
            GridBagla();

        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            Kaydet();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            Sil();
        }
    }
}
