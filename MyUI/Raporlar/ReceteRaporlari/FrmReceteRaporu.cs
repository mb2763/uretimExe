using My.Business.Manager;
using My.Entities.Raporlar;
using My.Entities.Receteler;
using My.Kontrol.Formlar;
using MyUI.ReceteModule;
using System;
using System.Collections.Generic;  
using System.Windows.Forms;

namespace MyUI.Raporlar.ReceteRaporlari {
    public partial class FrmReceteRaporu : MyFrmListe {
    
        ReceteManager _mng;
        IEnumerable<ReceteRaporuModel> _list;

        public FrmReceteRaporu() {
            InitializeComponent();
            EventlerBagla();
        }
        private void EventlerBagla() {
            this.Load += Frm_Load;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
            BtnAra.Click += BtnAra_Click;
        }
        private void Frm_Load(object sender, EventArgs e) {
            _mng = new ReceteManager(Ortak.DbPro);

            BtnAra.PerformClick();
        }
        private void Bagla() {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla();
            if (!string.IsNullOrEmpty(sor)) { sor = "where  1 = 1 " + sor; }
            var rs = _mng.GetReceteGenelRaporuList(sor);
            if (!rs.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data;
            myGrid1.DataSource = _list;
            myView1.SutunGizle("RcAId");
            myView1.SutunGizle("RcDId");
            myGrid1.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }
        private string SorguAyarla() {
            string sor = "";
             string t1 = "";
             t1 = TxtAra.Text.Trim();
             if (!string.IsNullOrEmpty(t1)) {
                 sor += $"   AND ( EntegreStokKodu LIKE('%{t1}%') OR EntegreStokAdi LIKE('%{t1}%')   )";
             }
            return sor;
        }
        private void MyView1_MyEventDoubleClickEnter() {
            var _rwid = myView1.FocusedRowHandle;
            var itm = myView1.MyGetCurrentItem<ReceteKullanilanStokModel>();
            if (itm == null) return;
            if (SecimIcinAcildi) {
                SecilenKod = itm.ReceteKodu;
                SecilenRow = itm;
                Secildi = true;
                this.Close();
                return;
            }
            FrmReceteED f = new FrmReceteED { IdGuid = itm.RcAId };
            f.ShowDialog();
            if (f.KayitEdildi) {
                BtnAra.PerformClick();
                myView1.FocusedRowHandle = _rwid;
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            var itm = myView1.MyGetCurrentItem<ReceteKullanilanStokModel>();
            if (itm != null) {
            }
            else {
            }
        }
        private void BtnAra_Click(object sender, EventArgs e) {
            Bagla();
        }
    }
}
