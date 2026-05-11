using DevExpress.Utils;
using My.Business.Manager;
using My.Entities.IstasyonTakipler;
using My.Kontrol.Formlar;
using System;
using System.Windows.Forms;

namespace MyUI.IstasyonModul
{
    public partial class FrmIstasyonBekleyenler : MyFrmListe
    {

        private IstasyonHareketManager _mng;
        public bool SiparisKodundanFiltrele { get; set; } = false;
        public string SiparisKodu { get; set; } = "";

        public FrmIstasyonBekleyenler()
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
            _mng = new IstasyonHareketManager(Ortak.DbPro, Ortak.DbMikro);
            BaglaIstasyonList();
            if (SiparisKodundanFiltrele)
            {
                SiparisFiltreAyarla();
            }
            Bagla();

        }
        void SiparisFiltreAyarla()
        {
            TxtKodu.Text = SiparisKodu;
        }
        private void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();

            var rs = _mng.GetBekleyenler(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid1.DataSource = rs.Data;
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("UrIId");
            myView1.SutunFormat(nameof(IstasyonTakipBekleyenModel.Tarih), FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunFormat(nameof(IstasyonTakipBekleyenModel.TeslimTarihi), FormatType.DateTime, "dd.MM.yyyy");
            myView1.SutunCaptionDegistir("SiparisKodu", "IsEmriKodu"); 
            myGrid1.GridYerlesimYukle();
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
            }
            Cursor.Current = Cursors.Default;
        }
        private string SorguAyarla()
        {
            string sor = "";
            string t1 = CmbIstasyon.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" and UrI.IstasyonKodu = '{t1}' "; }
            t1 = TxtKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND Sip.SiparisKodu    like('%{t1}%')"; }
            return sor;
        }
        private string SorguAyarlaTrh()
        {
            string sor = ""; 
            //var t1 = TxtTarihi1.Text.Trim();
            //if (!string.IsNullOrEmpty(t1)) {
            //    t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.EditValue));
            //    if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( Tarih,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            //}
            //var t1 = TxtTarihi1.Text.Trim();
            //if (!string.IsNullOrEmpty(t1)) {
            //    t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.EditValue));
            //    if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( Tarih,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            //}
            //t1 = TxtTarihi2.Text.Trim();
            //if (!string.IsNullOrEmpty(t1)) {
            //    t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.EditValue));
            //    if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( Tarih,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            //}
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

            var rs = _mng.IstKartService.SelectListWhere("");
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
            var itm = myView1.MyGetCurrentItem<IstasyonTakipBekleyenModel>();
            if (itm == null) return;
            //if (SecimIcinAcildi) {
            //    SecilenKod = itm.ReceteKodu;
            //    SecilenRow = itm;
            //    SecilenId = itm.Id.ToString();
            //    Secildi = true;
            //    this.Close();
            //    return;
            //}

            //FrmReceteED f = new FrmReceteED { IdGuid = itm.Id };
            //f.ShowDialog();
            //if (f.KayitEdildi) {
            //    BtnAra.PerformClick();
            //    myView1.FocusedRowHandle = _rwid;
            //}
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
