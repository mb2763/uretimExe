using My.Business.Service.ReceteIstasyonGruplar;
using My.Core; 
using My.Entities.Models; 
using My.Entities.ReceteIstasyonGruplar;
using My.Kontrol.Formlar; 
using System.Collections.Generic;
using System;
using System.Linq;
using My.Business.Service.Receteler;
using DevExpress.Utils.Extensions;
using My.Entities.Receteler;

namespace MyUI.ReceteIstasyonGrupModul
{
    public partial class FrmReceteIstasyonGrupIstasyonEslestir : MyFrmKayit
    { 
   
         
        private readonly IReceteIstasyonGrupIstasyonService _srv = Ortak.DbPro.ReceteIstasyonGrupIstasyonlar;
        private readonly IReceteIstasyonGrupOperasyonService _srvGrup = Ortak.DbPro.ReceteIstasyonGrupOperasyonlar;
        private readonly IReceteOperasyonService _srvOp = Ortak.DbPro.ReceteOperasyon;
        private readonly IReceteAnaService _srvRct = Ortak.DbPro.ReceteAna;
  

        private List<ReceteIstasyonGrupIstasyon> _list;
        private ReceteIstasyonGrupIstasyon _mdl;


      public Guid RcAId { get; set; }


        ReceteAna recete { get; set; }

    
     
        public FrmReceteIstasyonGrupIstasyonEslestir()
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

            ReceteBagla();
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
            TxtGrupKodu.Enabled = false;
          
           
        }
        private void TxtLeriAc()
        {
            BtnKaydet.Enabled = true;
            BtnSil.Enabled = true;
            TxtGrupKodu.Enabled = true;
             
        } 
        public void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("RcAId"); 
        }
        public void Bagla()
        {
            YeniKayit = true;
            var rs = _srv.SelectListWhere(" where RcAId='" + RcAId + "' Order By GrupKodu ");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            bs.DataSource = _list;
            TxtReceteKodu.Text = recete.ReceteKodu;
            TxtReceteAdi.Text =  recete.ReceteAdi;
            GridBagla();

        }

        public void ReceteBagla()
        {
            YeniKayit = true;
            var rs = _srvRct.SelectFind(  RcAId );
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            recete = rs.Data ; 
        }


        private void TemizleText()
        {
           
            _mdl = null;
            IdGuid = Guid.Empty; 
            TxtGrupKodu.Text = "";
         
            
        }
        private void AktarTextlere()
        {
            IdGuid = _mdl.Id;
           
             TxtGrupKodu.Text = _mdl.GrupKodu;
            
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
                _mdl = new ReceteIstasyonGrupIstasyon() { Id = MyGuid.NewGuid() };
            }
            if (IdGuid == Guid.Empty) IdGuid = MyGuid.NewGuid();
            _mdl.Id = IdGuid;
            _mdl.RcAId = RcAId;
            _mdl.GrupKodu = TxtGrupKodu.Text;
            
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
            if (string.IsNullOrEmpty(TxtReceteKodu.Text))
            {
                MesajHata("Lütfen Recete kodunu giriniz");
                return false;
            }
            
            return true;
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            if (SecimIcinAcildi)
            {
                var itm = myView1.MyGetCurrentItem<ReceteIstasyonGrupIstasyon>();
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

            var itm = myView1.MyGetCurrentItem<ReceteIstasyonGrupIstasyon>();
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
            _mdl = new ReceteIstasyonGrupIstasyon() { Id = Guid.Empty };
        }

        private void TxtGrupKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FrmReceteIstasyonGrupKodlari f = new FrmReceteIstasyonGrupKodlari();
            f.SecimIcinAcildi = true;
            f.ShowDialog();
            if (f.Secildi)
            {
                string kod =f.SecilenKod;
                if (!ReceteEslesiyormu(kod))
                { 
                    return;
                }

                TxtGrupKodu.Text = kod; 
                Bagla();
            }
        }


        public bool ReceteEslesiyormu(string kod)
        {
            var rsOp = _srvOp.SelectList(c=> c.RcAId==RcAId);
            var rsGr = _srvGrup.SelectList(c=> c.GrupKodu==kod);

            if (!rsOp.Success)
            {
                MesajHata(rsOp.Message);
                return false;
            }
            if (!rsGr.Success)
            {
                MesajHata(rsGr.Message);
                return false;
            }
            var opr = rsOp.Data.ToList();
            var grp = rsGr.Data.ToList();

            foreach (var itm in rsOp.Data)
            {
                var fnd = grp.Find(c=> c.OperasyonKodu==itm.OperasyonKodu);
                if (fnd==null)
                {
                    MesajHata(" Secilen Grup ile Recete Operasyonları eşleşmiyor.\n" +itm.OperasyonKodu + " Kodlu Operasyon Bulunamadı ");                  
                    return false;
                }
            } 
            return true;
        }

    }
}
