using DevExpress.XtraGrid.Views.Grid;
using My.Business;
using My.Business.Service.Geneller;
using My.Business.Service.UretimIstasyonlar;
using My.Entities.UretimIstasyonlar;
using My.Kontrol.Formlar;
using System;

namespace MyUI.UretimIstasyonModule
{
    public partial class FrmUretimIstasyonHareketSec : MyFrmSade
    {
        private DatabaseFactoryMikro _db = Ortak.DbMikro;
        IUretimIstasyonService _srv = Ortak.DbPro.UretimIstasyon;
        IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        public Guid? UrOHDId = Guid.Empty;
        public bool OperasyondanBagla = false;
        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }

        public FrmUretimIstasyonHareketSec()
        {
            InitializeComponent();
            myView1.MyEventDoubleClickEnter += MyView1_MyEditAc;
        }
        private void FrmUretimIstasyonSec_Load(object sender, EventArgs e)
        {
            if (OperasyondanBagla)
            {
                // UrO.OperasyonKodu,UrO.OperasyonAdi

                BaglaOperasyondan();
            }
            else
            {
                Bagla();
            }

            myView1.IndicatorWidth = 30;
        }

        void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        public void Bagla()
        {
            var rs = _srv.GetViewListWhere(" where  UrI.UrOHDId= '" + UrOHDId + "'");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
            }

            myGrid1.DataSource = rs.Data;
            myGrid1.GridYerlesimYukle(myGrid1.MyGridKayitAdi);

        }

        public void BaglaOperasyondan()
        {
            var rs = _srv.GetViewListWhere(" where  UrO.OperasyonKodu= '" + OperasyonKodu + "'");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
            }

            myGrid1.DataSource = rs.Data;
            myGrid1.GridYerlesimYukle(myGrid1.MyGridKayitAdi);

        }

        private void MyView1_MyEditAc()
        {
            if (myView1.FocusedRowHandle < 0) return;

            var itmAl = myView1.GetRow(myView1.FocusedRowHandle);
            if (itmAl == null || itmAl.GetType() != typeof(UretimIstasyon)) return;
            var itm = (UretimIstasyon)itmAl;
            if (SecimIcinAcildi)
            {
                SecilenKod = itm.IstasyonKodu;
                SecilenRow = itm;
                Secildi = true;
                this.Close();
            }
            else
            {

            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (myView1.FocusedRowHandle < 0) return;

            var itmAl = myView1.GetRow(myView1.FocusedRowHandle);
            if (itmAl == null || itmAl.GetType() != typeof(UretimIstasyon)) return;
            var itm = (UretimIstasyon)itmAl;
            if (SecimIcinAcildi)
            {
                SecilenKod = itm.IstasyonKodu;
                SecilenRow = itm;
                Secildi = true;
                this.Close();
            }
            else
            {

            }
        }
    }
}
