using My.Business.Manager;
using My.Business.Service.IstasyonKartlar;
using My.Business.Service.Personeller;
using My.Entities.IstasyonTakipler;
using My.Entities.Raporlar.IstasyonRaporlari;
using My.Kontrol.Formlar;
using System;
using System.Windows.Forms;

namespace MyUI.Raporlar.IstasyonRaporlari
{
    public partial class FrmIstasyonRaporu : MyFrmListe
    { 
        private IstasyonRaporManager _mng;
        private IIstasyonKartiService _srvIstasyon = Ortak.DbPro.IstasyonKarti;
        private IPersonelService _srvPersonel = Ortak.DbPro.Personel;
        IstasyonRaporModel _mdl;
        public FrmIstasyonRaporu()
        {
            InitializeComponent();
            EventlerBagla();
        }
        private void EventlerBagla()
        {
            this.Load += Frm_Load;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new IstasyonRaporManager(Ortak.DbPro);
            BaglaIstasyonList();
            BaglaPersonelList();
            BtnAra.PerformClick();
        }
        private void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
            // if (!string.IsNullOrEmpty(sor)) {  sor = "where  1 = 1 " + sor;  }
            var rs = _mng.GetIstasyonRapor(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _mdl = rs.Data;
            myGrid1.DataSource = _mdl.Hareketler;
            //  myView1.SutunFormat(nameof(IstasyonRaporHareketModel.Tarih), FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myGrid1.GridYerlesimYukle();
            myGrid2.DataSource = _mdl.Toplamlar;
            myGrid2.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }
        private string SorguAyarla()
        {
            string sor = "";
            string t1 = CmbIstasyon.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND Ist.IstasyonKodu='{t1}' "; }
            t1 = CmbPersonel.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND HR.KayitEden='{t1}' "; }

            return sor;
        }
        private string SorguAyarlaTrh()
        {
            string sor = "";
            var saat1 = TxtSaat1.Text.ToString();
            var saat2 = TxtSaat2.Text.ToString();

            var t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.EditValue));
                t1 = t1 + " " + saat1;
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( HR.Tarih,'1901-01-01') AS datetime ) >=  CAST('{t1}'  AS datetime ) ";
            }
            t1 = TxtTarihi2.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.EditValue));
                t1 = t1 + " " + saat2;
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( HR.Tarih,'1901-01-01') AS datetime ) <=  CAST('{t1}'  AS datetime ) ";
            }
            return sor;
        }
        private string TarihAyarla(DateTime? T1)
        {
            if (T1 == null)
            {
                return "";
            }
            string FX = T1?.Year + "-" + T1?.Month.ToString().PadLeft(2, '0') + "-" +
                        T1?.Day.ToString().PadLeft(2, '0');
            return FX;
        }
        public void BaglaIstasyonList()
        {

            var rs = _srvIstasyon.SelectListWhere("");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data;
            foreach (var itm in dt)
            {
                CmbIstasyon.Items.Add(itm.IstasyonKodu);
            }
        }
        public void BaglaPersonelList()
        {

            var rs = _srvPersonel.SelectListWhere("");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data;
            foreach (var itm in dt)
            {
                CmbPersonel.Items.Add(itm.Kodu);
            }
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            var _rwid = myView1.FocusedRowHandle;
            var itm = myView1.MyGetCurrentItem<IstasyonTakipHareket>();
            if (itm == null) return;
            if (SecimIcinAcildi)
            {
                SecilenKod = itm.ReceteKodu;
                SecilenRow = itm;
                SecilenId = itm.Id.ToString();
                Secildi = true;
                this.Close();
                return;
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<IstasyonTakipHareket>();
            if (itm != null)
            {
            }
            else
            {
            }
        }
        private void BtnAra_Click(object sender, EventArgs e)
        {
            Bagla();
        }
  
    }
}
