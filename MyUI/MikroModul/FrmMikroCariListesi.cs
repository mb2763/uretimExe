using DevExpress.XtraGrid.Views.Grid;
using My.Business;
using My.Entities.Mikro;
using System;
using System.Linq;
using System.Windows.Forms;
using My.Business.Service.MikroModul;

namespace MyUI.MikroModule
{
    public partial class FrmMikroCariListesi : My.Kontrol.Formlar.MyFrmListe
    {
        private DatabaseFactoryMikro _db = Ortak.DbMikro;
        IMikroCariService _srv = Ortak.DbMikro.Cariler;
        public FrmMikroCariListesi()
        {
            InitializeComponent();
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            Bagla();
            myView1.Columns["CrGuid"].Visible = false;
            myGrid1.GridYerlesimYukle(myGrid1.MyGridKayitAdi);
            myView1.IndicatorWidth = 30;
            myView1.CustomDrawRowIndicator += myView1_CustomDrawRowIndicator;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            TxtAra.Focus();
        }
        void myView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            if (myView1.FocusedRowHandle < 0) return;
            var itmAl = myView1.GetRow(myView1.FocusedRowHandle);
            if (itmAl == null || itmAl.GetType() != typeof(MikroCari)) return;
            var itm = (MikroCari)itmAl;
            if (SecimIcinAcildi)
            {
                SecilenKod = itm.CariKodu;
                SecilenRow = itm;
                Secildi = true;
                this.Close();
            }
            else
            {
            }
        }
        void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla();
            if (!string.IsNullOrEmpty(sor))
            {
                sor = "where 1=1 " + sor;
            }
            var rs = _srv.GetViewListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid1.DataSource = rs.Data;
            lblKayitSayisi.Caption = rs.Data.Count().ToString();
            Cursor.Current = Cursors.Default;
        }
        public string SorguAyarla()
        {
            string sor = "";
            string t1 = TxtKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND C.cari_kod    like('%{t1}%')"; }
            t1 = TxtUnvani.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND (C.cari_unvan1 like('%{t1}%') or C.cari_unvan2 like('%{t1}%') )"; }
            t1 = TxtAra.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND (C.cari_kod    like('%{t1}%')  or C.cari_unvan1 like('%{t1}%') or C.cari_unvan2 like('%{t1}%') )"; }
            return sor;
        }
        private void BtnAra_Click(object sender, EventArgs e)
        {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            TxtKodu.Text = "";
            TxtUnvani.Text = "";
            TxtAra.Text = "";
        }
    }
}