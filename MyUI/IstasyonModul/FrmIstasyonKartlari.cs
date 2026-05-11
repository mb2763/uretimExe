using My.Business.Manager;
using My.Business.Service.IstasyonKartlar;
using My.Business.Service.OperasyonKartlar;
using My.Core;
using My.Entities.IstasyonKartlar;
using My.Entities.Mikro;
using My.Entities.OperasyonKartlar;
using My.Kontrol.Formlar;
using MyUI.MikroModule;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.UretimIstasyonModule
{
    public partial class FrmIstasyonKartlari : MyFrmKayit
    {
        private readonly IIstasyonKartiService _srv = Ortak.DbPro.IstasyonKarti;
        private readonly IOperasyonKartiService _srvOpr = Ortak.DbPro.OperasyonKarti;
        IstasyonManager _mng;
        private List<IstasyonKarti> _list;
        private IstasyonKarti _mdl;
        public string Aranan = "";
        public FrmIstasyonKartlari()
        {
            InitializeComponent();
            EventlerBagla();
        }
        public void EventlerBagla()
        {
            this.Load += Frm_Load;
            BtnKaydet.Click += BtnKaydet_Click;
            BtnSil.Click += BtnSil_Click;
            BtnDuzenle.Click += BtnDegistir_Click;
            BtnYeni.Click += BtnYeni_Click;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            this.CmbOperasyon.Leave += CmbOperasyon_Leave;
            this.CmbOperasyonAdi.Leave += CmbOperasyonAdi_Leave;
            this.CmbOperasyon.Popup += CmbOperasyon_Popup;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new IstasyonManager(Ortak.DbPro);
            BaglaOperasyon();
            if (SecimIcinAcildi && !string.IsNullOrEmpty(Aranan))
            {
                Bagla2();
            }
            else
            {
                Bagla();
            }
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            AcilisBittimi = true;
        }
        private void CmbOperasyon_Popup(object sender, EventArgs e)
        {
            CmbOperasyon.Properties.ForceInitialize();
        }
        private void CmbOperasyon_Leave(object sender, EventArgs e)
        {
            if (AcilisBittimi)
            {
                AcilisBittimi = false;
                var mdl = (OperasyonKarti)CmbOperasyon.GetSelectedDataRow();
                if (mdl != null)
                {
                    CmbOperasyonAdi.Text = mdl.OperasyonAdi;
                }
                AcilisBittimi = true;
            }
        }
        private void CmbOperasyonAdi_Leave(object sender, EventArgs e)
        {
            if (AcilisBittimi)
            {
                AcilisBittimi = false;
                var mdl = (OperasyonKarti)CmbOperasyonAdi.GetSelectedDataRow();
                if (mdl != null)
                {
                    CmbOperasyon.Text = mdl.OperasyonKodu;
                }
                AcilisBittimi = true;
            }
        }

        public void SutunGizle()
        {
            myView1.SutunGizle("Id");
        }
        public void Bagla()
        {
            YeniKayit = true;
            var rs = _srv.SelectListWhere(" Order By IstasyonKodu ");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            bs.DataSource = _list;
            GridBagla();
            TemizleText();
        }
        public void Bagla2()
        {
            YeniKayit = true;
            var rs = _srv.SelectListWhere(" where Operasyon ='" + Aranan + "'");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            bs.DataSource = _list;
            GridBagla();
            TemizleText();
            CmbOperasyon.Text = Aranan;
        }
        public void BaglaOperasyon()
        {
            var rs = _srvOpr.SelectListWhere(" Order By OperasyonKodu");//GrupListesi("OperasyonKarti", "OperasyonKodu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            CmbOperasyon.MyDataBagla(dt, "OperasyonKodu", "OperasyonKodu", new int[] { 1, 2 });
            CmbOperasyonAdi.MyDataBagla(dt, "OperasyonAdi", "OperasyonAdi", new int[] { 1, 2 });
        }
        private void TemizleText()
        {
            _mdl = new IstasyonKarti() { Id = Guid.Empty };
            IdGuid = Guid.Empty;
            TxtKodu.Text = "";
            TxtAdi.Text = "";
            CmbOperasyon.Text = "";
            CmbOperasyonAdi.Text = "";
        }
        private void AktarTextlere()
        {
            IdGuid = _mdl.Id;
            TxtKodu.Text = _mdl.IstasyonKodu;
            TxtAdi.Text = _mdl.IstasyonAdi;
            CmbOperasyon.Text = _mdl.Operasyon;
            CmbOperasyonAdi.Text = _mdl.OperasyonAdi;
            ChcFason.Checked = _mdl.Fason;
            myChcYazdir.Checked = _mdl.Yazdirilmali;
            ChcKaliteKontrol.Checked = _mdl.KaliteKontrol;
            TxtFasonCariKodu.Text = _mdl.FasonCariKodu;
            TxtFasonCariAdi.Text = _mdl.FasonCariAdi;
        }
        public void GridBagla()
        {
            myGrid1.DataSource = null;
            bs.DataSource = _list;
            myGrid1.DataSource = bs;
        }
        private void AktarModele()
        {
            if (_mdl == null)
            {
                _mdl = new IstasyonKarti() { Id = MyGuid.NewGuid() };
            }
            if (IdGuid == Guid.Empty) IdGuid = MyGuid.NewGuid();
            _mdl.Id = IdGuid;
            _mdl.IstasyonKodu = TxtKodu.Text;
            _mdl.IstasyonAdi = TxtAdi.Text;
            _mdl.Operasyon = CmbOperasyon.Text;
            _mdl.OperasyonAdi = CmbOperasyonAdi.Text;
            _mdl.Fason = ChcFason.Checked;
            _mdl.Yazdirilmali = myChcYazdir.Checked;
            _mdl.KaliteKontrol = ChcKaliteKontrol.Checked;
            _mdl.FasonCariKodu = TxtFasonCariKodu.Text;
            _mdl.FasonCariAdi = TxtFasonCariAdi.Text;
        }
        public void Kaydet()
        {
            if (!TextLeriKontrolEt())
            {
                return;
            }
            AktarModele();
            var rs = _mng.Kaydet(_mdl, YeniKayit);
            if (rs.Success)
            {
                KayitEdildi = true;
                Bagla();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        public void Sil()
        {
            var rs = _srv.Delete(_mdl);
            if (rs.Success)
            {
                KayitEdildi = true;
                //this.Close();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        private bool TextLeriKontrolEt()
        {
            if (string.IsNullOrEmpty(TxtKodu.Text))
            {
                MesajHata("Lütfen  kodunu giriniz");
                return false;
            }
            return true;
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            if (SecimIcinAcildi)
            {
                var itm = myView1.MyGetCurrentItem<IstasyonKarti>();
                if (itm != null)
                {
                    SecilenKod = itm.IstasyonKodu;
                    SecilenRow = itm;
                    SecilenId = itm.Id.ToString();
                    Secildi = true;
                    this.Close();
                }
            }
            else
            {
                BtnDuzenle.PerformClick();
            }
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            Kaydet();
            Bagla();
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
            Bagla();
        }
        private void BtnDegistir_Click(object sender, EventArgs e)
        {

            var itm = myView1.MyGetCurrentItem<IstasyonKarti>();
            if (itm == null) return;

            YeniKayit = false;
            _mdl = itm.Clone();
            AcilisBittimi = false;
            AktarTextlere();
            AcilisBittimi = true;
        }
        private void BtnYeni_Click(object sender, EventArgs e)
        {
            YeniKayit = true;
            TemizleText();
        }

        private void TxtFasonCariKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FrmMikroCariListesi f = new FrmMikroCariListesi();
            f.SecimIcinAcildi = true;
            f.ShowDialog();
            if (f.Secildi)
            {
                var cr = ((MikroCari)f.SecilenRow);
                TxtFasonCariKodu.Text = cr.CariKodu.ToString();
                TxtFasonCariAdi.Text = cr.CariUnvani1.ToString();
            }
        }

        private void BtnBagla_Click(object sender, EventArgs e)
        {
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            AcilisBittimi = true;
        }
    }
}

