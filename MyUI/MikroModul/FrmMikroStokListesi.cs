using DevExpress.XtraGrid.Views.Grid;
using My.Business;
using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.MikroModul;
using My.Entities.Mikro;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.MikroModule {
    public partial class FrmMikroStokListesi : MyFrmListe {
        private DatabaseFactoryMikro _db = Ortak.DbMikro;
        IMikroStokService _srv = Ortak.DbMikro.Stoklar;
        IMikroGenelService _srvGenel = Ortak.DbMikro.GenelServis;
        public List<MikroStokCinsi> stokCinsiListFull = new List<MikroStokCinsi>();
        public bool TumStoklar = false;

        public FrmMikroStokListesi() {
            InitializeComponent();
        }
        private void Frm_Load(object sender, EventArgs e) {
            BaglaAnaGrup();
            BaglaStokCinsi();
            if (TumStoklar) {
                //ChcHammadde.Checked = false;
                CmbStokCinsi.Text = "";
            }
            else {
                Bagla();
            }
            //myView1.Columns["StGuid"].Visible = false;
            myGrid1.GridYerlesimYukle(myGrid1.MyGridKayitAdi);
            TxtAra.Focus();
            myView1.IndicatorWidth = 30;
            myView1.CustomDrawRowIndicator += myView1_CustomDrawRowIndicator;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
        }
        private void myView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e) {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        private void MyView1_MyEventDoubleClickEnter() {
            if (myView1.FocusedRowHandle < 0) return;
            var itmAl = myView1.GetRow(myView1.FocusedRowHandle);
            if (itmAl == null || itmAl.GetType() != typeof(MikroStok)) return;
            var itm = (MikroStok)itmAl;
            if (SecimIcinAcildi) {
                SecilenKod = itm.StokKodu;
                SecilenRow = itm;
                Secildi = true;
                this.Close();
            }
            else {
            }
        }
        private void Bagla() {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla();
            if (!string.IsNullOrEmpty(sor)) {
                sor = "where 1=1 " + sor;
            }
            var rs = _srv.GetViewListWhere(sor, Ortak.MikroStokGrubu);
            if (!rs.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid1.DataSource = rs.Data;
            lblKayitSayisi.Caption = rs.Data.Count().ToString();
            Cursor.Current = Cursors.Default;
        }
        private string SorguAyarla() {
            string sor = "";
            string t1 = TxtStokKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND S.sto_kod like('%{t1}%')"; }
            t1 = TxtStokAdi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND S.sto_isim like('%{t1}%')"; }
            t1 = CmbAnaGrubu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND S.{Ortak.MikroStokGrubu} ='{t1}'"; }
            t1 = TxtAra.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND (S.sto_kod like('%{t1}%')  or  S.sto_isim like('%{t1}%') )"; }

            var tur = GetCinsiKodu(CmbStokCinsi.Text);

            if (tur >= 0) {
                sor += $" AND  S.sto_cins = " + tur + " ";
            }
            return sor;
        }

        public int GetCinsiKodu(string tur) {

            foreach (var itm in stokCinsiListFull) {
                if (itm.Adi.ToString() == tur) {
                    return itm.Kodu;
                }
            }
            return -2;
        }

        private void BaglaAnaGrup() {
            var rs = _srvGenel.GrupListesi("STOKLAR", Ortak.MikroStokGrubu);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbAnaGrubu.MyDataTemizle(); ;
            CmbAnaGrubu.MyDataBagla(dt); ;
        }

        public void BaglaStokCinsi() {
            /*  stokCinsiList MikroStokCinsiManager.GetCinsList();*/
            stokCinsiListFull = MikroStokCinsiManager.GetCinsListFull();
            CmbStokCinsi.MyDataBagla(stokCinsiListFull, "Kodu", "Adi", new int[] { 1, 2 });
            CmbStokCinsi.Text = "";
        }
        private void BtnAra_Click(object sender, EventArgs e) {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e) {
            TxtStokKodu.Text = "";
            TxtStokAdi.Text = "";
            CmbAnaGrubu.Text = "";
            TxtAra.Text = "";
        }
    }
}