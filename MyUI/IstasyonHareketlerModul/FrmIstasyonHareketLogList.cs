using My.Business.Service.Geneller;
using My.Business.Service.IstasyonKartlar;
using My.Business.Service.IstasyonTakipler;
using My.Business.Service.OperasyonKartlar;
using My.Entities.IstasyonTakipler;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.IstasyonModul {
    public partial class FrmIstasyonHareketLogList : MyFrmListe {

        private readonly IIstasyonTakipHareketLogService _srv = Ortak.DbPro.IstasyonTakipHareketLog;
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly IIstasyonKartiService _srvIst = Ortak.DbPro.IstasyonKarti;
        private readonly IOperasyonKartiService _srvOpr = Ortak.DbPro.OperasyonKarti;


        private List<IstasyonTakipHareketLog> _list;
        public FrmIstasyonHareketLogList() {
            InitializeComponent();
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, EventArgs e) {
            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            TxtTarihi2.Text = "";
            TxtSaat1.Text = "00:00:00";
            TxtSaat2.Text = "23:59:59";
            BtnAra.Click += BtnAra_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            BaglaOperasyon();
            BagliIstasyon();

            Bagla();
            SutunGizle();

            myGrid1.GridYerlesimYukle();
        }
        private void SutunGizle() {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("UrIId");
            myView1.SutunGizle("IstHrId");
            myView1.SutunFormat("Tarih", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunFormat("KayitTarihi", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
        }

        private void Bagla() {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
            if (!string.IsNullOrEmpty(sor)) {
                sor = "where 1=1 " + sor;
            }
            var rs = _srv.GetViewListWhere(sor);
            if (!rs.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            myGrid1.DataSource = _list;
            Cursor.Current = Cursors.Default;
        }
        public void BaglaOperasyon() {
            var rs = _srvOpr.SelectListWhere(" Order By OperasyonKodu");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            CmbOperasyon.MyDataBagla(dt, "OperasyonKodu", "OperasyonKodu", new int[] { 1, 2 });
        }
        public void BagliIstasyon() {
            var rs = _srvIst.SelectListWhere(" Order By IstasyonKodu");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            CmbIstasyon.MyDataBagla(dt, "IstasyonKodu", "IstasyonKodu", new int[] { 1, 2 });
        }
        private string SorguAyarla() {
            string sor = "";
            string t1 = CmbOperasyon.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  TH.OperasyonKodu = '{t1}' "; }
            t1 = CmbIstasyon.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  TH.IstasyonKodu = '{t1}' "; }

            return sor;
        }
        private string SorguAyarlaTrh() {
            string sor = "";
            string t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.Text)) + " " + TxtSaat1.Text;
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( LG.Tarih,'1901-01-01') AS DATETIME ) >=  CAST('{t1}'  AS DATETIME ) ";
            }
            t1 = TxtTarihi2.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.Text)) + " " + TxtSaat2.Text;
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST ( coalesce(LG.Tarih,'1901-01-01') AS DATETIME ) <=  CAST('{t1}'  AS DATETIME ) ";
            }

            return sor;
        }
        private string TarihAyarla(DateTime? T1) {
            if (T1 == null) {
                return "";
            }
            string FX = T1?.Year + "-" + T1?.Month.ToString().PadLeft(2, '0') + "-" +
                        T1?.Day.ToString().PadLeft(2, '0');
            return FX;
        }
        private void MyView1_MyEventDoubleClickEnter() {
            if (SecimIcinAcildi) {
                var itm = myView1.MyGetCurrentItem<IstasyonTakipHareketLog>();
                if (itm != null) {
                    SecilenRow = itm;
                    Secildi = true;
                    this.Close();
                }
            }
            else {
                var itm = myView1.MyGetCurrentItem<IstasyonTakipHareketLog>();
                if (itm != null) {
                    //FrmUretimEmriED_V2 f = new FrmUretimEmriED_V2();
                    //f.AltBarStatusPanel.Visible = false;
                    //f.Idsi = itm.Id;
                    //f.ActionAktar = Bagla;
                    //f.ShowDialog();
                }
            }
        }
        private void BtnAra_Click(object sender, EventArgs e) {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e) {

            TxtTarihi1.Text = "";
            TxtTarihi2.Text = "";
            TxtSaat1.Text = "00:00:00";
            TxtSaat2.Text = "23:59:59";
        }
    }
}
