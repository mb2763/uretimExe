using My.Business.Service.ReceteIstasyonGruplar;
using My.Core;
using My.Entities.IstasyonKartlar;
using My.Entities.OperasyonKartlar;
using My.Entities.ReceteIstasyonGruplar;
using My.Kontrol.Formlar;
using MyUI.IstasyonModul;
using MyUI.UretimOperasyonModule;
using System;
using System.Collections.Generic; 
using System.Linq; 

namespace MyUI.ReceteIstasyonGrupModul
{
    public partial class FrmReceteIstasyonGrupOperasyonEslestir : MyFrmKayit
    {
     
        private readonly IReceteIstasyonGrupOperasyonService _srv = Ortak.DbPro.ReceteIstasyonGrupOperasyonlar;

        private List<ReceteIstasyonGrupOperasyon> _list;
        private ReceteIstasyonGrupOperasyon _mdl;

   

        public FrmReceteIstasyonGrupOperasyonEslestir()
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

        }
        private void Frm_Load(object sender, EventArgs e)
        { 
            Bagla();
            TemizleText();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            TxtLeriKapat();
            AcilisBittimi = true;
        }
        private void TxtLeriKapat()
        {
            BtnKaydet.Enabled = false;
            BtnSil.Enabled = false;
            TxtOperasyonKodu.Enabled = false;
            TxtOperasyonAdi.Enabled = false;
            TxtIstasyonKodu.Enabled = false;
            TxtIstasyonAdi.Enabled = false;
        }
       private void TxtLeriAc()
        {
            BtnKaydet.Enabled = true;
            BtnSil.Enabled = true;
            TxtOperasyonKodu.Enabled = true;
            TxtOperasyonAdi.Enabled = true;
            TxtIstasyonKodu.Enabled = true;
            TxtIstasyonAdi.Enabled = true;
        }
        

    
        public void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("OprId");
            myView1.SutunGizle("IstId");
        }
        public void Bagla()
        {
            YeniKayit = true;
            var rs = _srv.SelectListWhere(" where GrupKodu='"+TxtGrupKodu.Text+ "' Order By GrupKodu ");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            bs.DataSource = _list;
            GridBagla();
          
        }


        private void TemizleText()
        {
            // _mdl = new ReceteIstasyonGrupOperasyon() { Id = Guid.Empty };
            _mdl = null;
            IdGuid = Guid.Empty;
        //    TxtGrupKodu.Text = "";
            TxtOperasyonKodu.Text = "";
            TxtOperasyonAdi.Text = "";
            TxtIstasyonKodu.Text = "";
            TxtIstasyonAdi.Text = ""; 
        }
        private void AktarTextlere()
        {
            IdGuid = _mdl.Id;
            
           // TxtGrupKodu.Text = _mdl.GrupKodu;
            TxtOperasyonKodu.Text =  _mdl.OperasyonKodu;
            TxtOperasyonAdi.Text  =  _mdl.OperasyonAdi;
            TxtIstasyonKodu.Text  =  _mdl.IstasyonKodu;
            TxtIstasyonAdi.Text = _mdl.IstasyonAdi;
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
                _mdl = new ReceteIstasyonGrupOperasyon() { Id = MyGuid.NewGuid() };
            }
            if (IdGuid == Guid.Empty) IdGuid = MyGuid.NewGuid();
            _mdl.Id = IdGuid;
          
            _mdl.GrupKodu = TxtGrupKodu.Text;
            _mdl.OperasyonKodu = TxtOperasyonKodu.Text;
            _mdl.OperasyonAdi = TxtOperasyonAdi.Text;
            _mdl.IstasyonKodu = TxtIstasyonKodu.Text;
            _mdl.IstasyonAdi = TxtIstasyonAdi.Text;
        }
        public void Kaydet()
        {
            if (!TextLeriKontrolEt())
            {
                return;
            }
            AktarModele();
            var rs = _srv.InsertOrUpdate(_mdl);
            if (rs.Success)
            {
                KayitEdildi = true;
                Bagla();
                TxtLeriKapat();
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
                Bagla();
                TxtLeriKapat();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        private bool TextLeriKontrolEt()
        {
            if (string.IsNullOrEmpty(TxtGrupKodu.Text))
            {
                MesajHata("Lütfen Grup kodunu giriniz");
                return false;
            }
             if (string.IsNullOrEmpty(TxtOperasyonKodu.Text))
            {
                MesajHata("Lütfen Operasyon kodunu giriniz");
                return false;
            }
             if (string.IsNullOrEmpty(TxtIstasyonKodu.Text))
            {
                MesajHata("Lütfen Istasyon kodunu giriniz");
                return false;
            }
            return true;
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            if (SecimIcinAcildi)
            {
                var itm = myView1.MyGetCurrentItem<ReceteIstasyonGrupOperasyon>();
                if (itm != null)
                {
                    SecilenKod = itm.GrupKodu;
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
            TemizleText();
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
            TemizleText();
        }
        private void BtnDegistir_Click(object sender, EventArgs e)
        {

            var itm = myView1.MyGetCurrentItem<ReceteIstasyonGrupOperasyon>();
            if (itm == null) return;
            _mdl = itm.Clone();
            AcilisBittimi = false;
            AktarTextlere();
            TxtLeriAc();
            AcilisBittimi = true;
        }
        private void BtnYeni_Click(object sender, EventArgs e)
        {
            TemizleText();
            TxtLeriAc();
            _mdl = new ReceteIstasyonGrupOperasyon() { Id = Guid.Empty };
        }

        private void TxtGrupKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FrmReceteIstasyonGrupKodlari f =new FrmReceteIstasyonGrupKodlari();
            f.SecimIcinAcildi = true;
            f.ShowDialog();
            if (f.Secildi)
            {
                TxtGrupKodu.Text = f.SecilenKod;
                Bagla();
            }
        }

        private void TxtOperasyonKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (_mdl==null) return; 
            FrmOperasyonKartlari f = new FrmOperasyonKartlari();
            f.SecimIcinAcildi = true;
            f.ShowDialog();
            if (f.Secildi)
            {
                var rw = f.SecilenRow as OperasyonKarti; 
                TxtOperasyonKodu.Text = rw.OperasyonKodu;
                TxtOperasyonAdi.Text = rw.OperasyonAdi; 
            }
        }

        private void TxtIstasyonKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (_mdl == null) return;
            FrmIstasyonKartList f = new FrmIstasyonKartList();
            f.SecimIcinAcildi = true;
            f.ShowDialog();
            if (f.Secildi)
            {
                var rw = f.SecilenRow as IstasyonKarti;
              
                TxtIstasyonKodu.Text = rw.IstasyonKodu;
                TxtIstasyonAdi.Text = rw.IstasyonAdi; 
            }
        }
    }
}
