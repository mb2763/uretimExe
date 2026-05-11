using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.IstasyonKartlar;
using My.Business.Service.OperasyonKartlar;
using My.Core;
using My.Entities.IstasyonKartlar;
using My.Entities.OperasyonKartlar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.UretimOperasyonModule
{
    public partial class FrmOperasyonKartlari : MyFrmKayit
    {
        private OperasyonManager _mng  ;
        private IOperasyonKartiService _srv = Ortak.DbPro.OperasyonKarti;
        private readonly IIstasyonKartiService _srvIst = Ortak.DbPro.IstasyonKarti;
        private IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private List<OperasyonKarti> _list;
        private OperasyonKarti _mdl;
        public FrmOperasyonKartlari()
        {
            InitializeComponent();
            EventlerBagla();
        }
        public void EventlerBagla()
        {
            this.Load += Frm_Load;
            myView1.MyEventDoubleClickEnter += myView1_MyEventDoubleClickEnter;
            BtnKaydet.Click += BtnKaydet_Click;
            BtnSil.Click += BtnSil_Click;
            BtnDuzenle.Click += BtnDegistir_Click;
            BtnYeni.Click += BtnYeni_Click;
            this.CmbIstasyonKodu.Leave += CmbIstasyonKodu_Leave;
            this.CmbIstasyonAdi.Leave += CmbIstasyonAdi_Leave;
            this.CmbIstasyonKodu.Popup += CmbIstasyonKodu_Popup;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new OperasyonManager(Ortak.DbPro);
            BaglaIstasyon();
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            AcilisBittimi = true;
        }
        private void CmbIstasyonKodu_Popup(object sender, EventArgs e)
        {
            CmbIstasyonKodu.Properties.ForceInitialize();
        }
        private void CmbIstasyonKodu_Leave(object sender, EventArgs e)
        {
            if (AcilisBittimi)
            {
                AcilisBittimi = false;
                var mdl = (IstasyonKarti)CmbIstasyonKodu.GetSelectedDataRow();
                if (mdl != null)
                {
                    CmbIstasyonAdi.Text = mdl.IstasyonAdi;
                }
                AcilisBittimi = true;
            }
        }
        private void CmbIstasyonAdi_Leave(object sender, EventArgs e)
        {
            if (AcilisBittimi)
            {
                AcilisBittimi = false;
                var mdl = (IstasyonKarti)CmbIstasyonAdi.GetSelectedDataRow();
                if (mdl != null)
                {
                    CmbIstasyonKodu.Text = mdl.IstasyonKodu;
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
            var rs = _srv.SelectListWhere(" Order By OperasyonKodu");
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
        public void BaglaIstasyon()
        {
            var rs = _srvIst.SelectListWhere(" Order By IstasyonKodu ");//GrupListesi("OperasyonKarti", "OperasyonKodu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            CmbIstasyonKodu.MyDataBagla(dt, "IstasyonKodu", "IstasyonKodu", new int[] { 1, 2 });
            CmbIstasyonAdi.MyDataBagla(dt, "IstasyonAdi", "IstasyonAdi", new int[] { 1, 2 });
        }
        private void TemizleText()
        {
            _mdl = new OperasyonKarti() { Id = Guid.Empty };
            IdGuid = Guid.Empty;
            TxtKodu.Text = "";
            TxtAdi.Text = "";
            CmbIstasyonKodu.Text = "";
            CmbIstasyonAdi.Text = "";
        }
        private void AktarTextlere()
        {
            IdGuid = _mdl.Id;
            TxtKodu.Text = _mdl.OperasyonKodu;
            TxtAdi.Text = _mdl.OperasyonAdi;
            CmbIstasyonKodu.Text = _mdl.VarsayilanIstasyonKodu;
            CmbIstasyonAdi.Text = _mdl.VarsayilanIstasyonAdi;
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
                _mdl = new OperasyonKarti() { Id = Guid.Empty };
            }
            if (IdGuid == Guid.Empty) IdGuid = MyGuid.NewGuid();
            _mdl.Id = IdGuid;
            _mdl.OperasyonKodu = TxtKodu.Text.ToString();
            _mdl.OperasyonAdi = TxtAdi.Text.ToString();
            _mdl.VarsayilanIstasyonKodu = CmbIstasyonKodu.Text.ToString();
            _mdl.VarsayilanIstasyonAdi = CmbIstasyonAdi.Text.ToString();
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
        private void myView1_MyEventDoubleClickEnter()
        {
            if (SecimIcinAcildi)
            {
                var itm = myView1.MyGetCurrentItem<OperasyonKarti>();
                if (itm != null)
                {
                    SecilenKod = itm.OperasyonKodu;
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

            var itm = myView1.MyGetCurrentItem<OperasyonKarti>();
            if (itm == null)
            {
                return;
            }
            YeniKayit = false;
            _mdl = itm.Clone();
            AktarTextlere();
        }
        private void BtnYeni_Click(object sender, EventArgs e)
        {
            YeniKayit = true;
            TemizleText();
        }
    }
}