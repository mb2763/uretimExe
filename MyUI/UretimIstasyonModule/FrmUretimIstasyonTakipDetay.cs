using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using My.Business.Manager;
using My.Entities.Models;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.UretimIstasyonModule {
    public partial class FrmUretimIstasyonTakipDetay : MyFrmSadeFull {

        private List<UretimIstasyonTakipDetayModel> _list;
        private UretimIstasyonTakipManager _mng;
        public string IstasyonKodu { get; set; }
        public int Fason { get; set; }

        public FrmUretimIstasyonTakipDetay() {
            InitializeComponent();
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, EventArgs e) {
            _mng = new UretimIstasyonTakipManager(Ortak.DbPro);
            Bagla();

            this.Text = "IstasyonKodu : " + IstasyonKodu;
        }
        private void Bagla() {
            var rs = _mng.GetTakipDetayList(IstasyonKodu, Fason);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            var query = _list.Select((fruit) => fruit.OperasyonKodu);
            var Grp1 = query.GroupBy(item => item).ToList();
            myGrid1.DataSource = _list;
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            (myView1.Columns["BaslangicTarihi"] as GridColumn).DisplayFormat.FormatType = FormatType.Custom;
            (myView1.Columns["BaslangicTarihi"] as GridColumn).DisplayFormat.FormatString = "dd.MM.yyyy HH:mm";

        }
        public void SutunGizle() {

        }
    }
}
