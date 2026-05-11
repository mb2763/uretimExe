using DevExpress.Utils;
using My.Business.Service.IstasyonBakimlar;
using My.Business.Service.IstasyonKartlar;
using My.Entities.IstasyonBakimlar;
using My.Kontrol.Formlar;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.IstasyonModul
{
    public partial class FrmIstasyonBakimList : MyFrmListe
    {

        private IIstasyonBakimService _srv;
        private IIstasyonBakimParcaService _srvParca;
        private IIstasyonKartiService _srvIst;

        public FrmIstasyonBakimList()
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
            _srv = Ortak.DbPro.IstasyonBakim;
            _srvIst = Ortak.DbPro.IstasyonKarti;
            _srvParca = Ortak.DbPro.IstasyonBakimParca;
            BaglaIstasyonList();
            Bagla();
        }
        private void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
            if (!string.IsNullOrEmpty(sor))
            {
                sor = "where  1 = 1 " + sor;
            }
            var rs = _srv.SelectListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid1.DataSource = rs.Data;
            myView1.SutunGizle("Id");
            myView1.SutunFormat(nameof(IstasyonBakim.Tarih), FormatType.DateTime, "dd.MM.yyyy");
            myGrid2.DataSource = null;

            if (rs.Data.Any())
            {
                BaglaDetay(rs.Data.FirstOrDefault().Id);
            }

            Cursor.Current = Cursors.Default;
        }
        private void BaglaDetay(Guid? IstBakId)
        {

            string sor = $" where  IstBakId='{IstBakId}'  ";
            var rs = _srvParca.SelectListWhere(sor);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            myGrid2.DataSource = rs.Data;
            myView2.SutunGizle("Id");
            myView2.SutunGizle("IstBakId");
            myGrid2.GridYerlesimYukle();
        }
        private string SorguAyarla()
        {
            string sor = "";
            string t1 = CmbIstasyon.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" and IstasyonKodu = '{t1}' "; }

            return sor;
        }
        private string SorguAyarlaTrh()
        {
            string sor = "";

            var t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.EditValue));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( Tarih,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.EditValue));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( Tarih,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
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
            var rs = _srvIst.SelectListWhere("");
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

        private void MyView1_MyEventDoubleClickEnter()
        {
            var _rwid = myView1.FocusedRowHandle;
            var itm = myView1.MyGetCurrentItem<IstasyonBakim>();
            if (itm == null) return;


            FrmIstasyonBakimEkle f = new FrmIstasyonBakimEkle { IdGuid = itm.Id };
            f.ShowDialog();
            if (f.KayitEdildi)
            {
                BtnAra.PerformClick();
                myView1.FocusedRowHandle = _rwid;
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {


            var itm = myView1.MyGetCurrentItem<IstasyonBakim>();
            if (itm != null)
            {

                BaglaDetay(itm.Id);

            }
            else
            {
                myGrid2.DataSource = null;

            }
        }

        private void BtnAra_Click(object sender, EventArgs e)
        {
            Bagla();
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            var _rwid = myView1.FocusedRowHandle;
            FrmIstasyonBakimEkle f = new FrmIstasyonBakimEkle();
            f.ShowDialog();
            if (f.KayitEdildi)
            {
                BtnAra.PerformClick();
                myView1.FocusedRowHandle = _rwid;
            }

        }
    }
}
