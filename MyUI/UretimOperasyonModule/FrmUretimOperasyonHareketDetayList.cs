using My.Business.Service.Geneller;
using My.Business.Service.UretimIstasyonlar;
using My.Business.Service.UretimOperasyonlar;
using My.Entities.UretimIstasyonlar;
using My.Entities.UretimOperasyonlar;
using My.Kontrol.Formlar;
using MyUI.UretimIstasyonModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.UretimOperasyonModule
{
    public partial class FrmUretimOperasyonHareketDetayList : MyFrmListe
    {
        private readonly IUretimIstasyonService _srvUIst = Ortak.DbPro.UretimIstasyon;
        private readonly IUretimOperasyonHareketDetayService _srv = Ortak.DbPro.UretimOperasyonHareketDetay;
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private List<UretimOperasyonHareketDetay> _list;
        private List<UretimIstasyon> _listUIst;
        public bool DetayGoster = false;
        public Guid? DetayId = Guid.Empty;
        public FrmUretimOperasyonHareketDetayList()
        {
            InitializeComponent();
            this.Load += Frm_Load;

        }
        private void Frm_Load(object sender, EventArgs e)
        {
            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            TxtTarihi2.Text = "";
            BtnAra.Click += BtnAra_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;

            BaglaDurum();
            BaglaReceteAdi();
            BaglaOperasyon();
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
        }

        private void Bagla()
        {
            if (DetayGoster)
            {
                // UrOHId
                var rs1 = _srv.GetViewListWhere(" where UrOH.Id= '" + DetayId + "'");
                if (!rs1.Success)
                {
                    Cursor.Current = Cursors.Default;
                    MesajHata(rs1.Message);
                    return;
                }
                _list = rs1.Data.ToList();
                myGrid1.DataSource = _list;
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
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
            _list = rs.Data.ToList();
            myGrid1.DataSource = _list;
            if (rs.Data.Any())
            {
                BaglaUIst(rs.Data.FirstOrDefault().Id);
            }
            Cursor.Current = Cursors.Default;
        }
        private void BaglaUIst(Guid? UrOHDId)
        {
            Cursor.Current = Cursors.WaitCursor;

            string sor = "where UrOHDId= '" + UrOHDId + "' ";

            var rs = _srvUIst.GetViewListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _listUIst = rs.Data.ToList();
            myGrid2.DataSource = _listUIst;
            SutunGizle2();
            myGrid2.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }
        private void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("UrOId");
            myView1.SutunGizle("UrOHId");
            myView1.SutunGizle("RcAId");
            myView1.SutunGizle("RcOId");
            myView1.SutunGizle("SipId");

        }
        private void SutunGizle2()
        {
            myView2.SutunGizle("Id");
            myView2.SutunGizle("UrId");
            myView2.SutunGizle("UrOId");
            myView2.SutunGizle("UrOHId");
            myView2.SutunGizle("UrOHDId");
            myView2.SutunGizle("RcAId");
            myView2.SutunGizle("RcOId");
            myView2.SutunGizle("RcIstId");
            myView2.SutunGizle("SipId");

        }
        private void BaglaDurum()
        {
            var rs = _srvGenel.GrupListesi("UretimOperasyon", "Durumu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbDurumu.DataSource = dt;
        }
        private void BaglaReceteAdi()
        {
            var rs = _srvGenel.GrupListesi("UretimOperasyon", "ReceteAdi");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbReceteAdi.DataSource = dt;
        }
        private void BaglaOperasyon()
        {
            var rs = _srvGenel.GrupListesi("UretimOperasyon", "OperasyonKodu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbOperasyon.DataSource = dt;
        }
        private string SorguAyarla()
        {
            string sor = "";
            string t1 = CmbOperasyon.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  UrO.OperasyonKodu = '{t1}' "; }
            t1 = CmbDurumu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  UrO.Durumu = '{t1}' "; }
            t1 = CmbReceteAdi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  UrO.ReceteAdi like('%{t1}%')  "; }
            return sor;
        }
        private string SorguAyarlaTrh()
        {
            string sor = "";
            string t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UrOHD.Tarih,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST ( coalesce(UrOHD.Tarih,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
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
        private void MyView1_MyEventDoubleClickEnter()
        {
            if (SecimIcinAcildi)
            {
                var itm = myView1.MyGetCurrentItem<UretimOperasyonHareketDetay>();
                if (itm != null)
                {
                    SecilenRow = itm;
                    Secildi = true;
                    this.Close();
                }
            }
            else
            {
                var itm = myView1.MyGetCurrentItem<UretimOperasyonHareketDetay>();
                if (itm != null)
                {
                    FrmUretimIstasyonED f = new FrmUretimIstasyonED
                    {
                        OperasyonTuru = OperasyonTuruEnum.Degistir,
                        IdGuid = itm.Id,
                        OprId = itm.UrOHId
                    };
                    f.ShowDialog();
                }
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {


            var itm = myView1.MyGetCurrentItem<UretimOperasyonHareketDetay>();
            if (itm != null)
            {
                BaglaUIst(itm.Id);
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
        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            CmbDurumu.Text = "";
            CmbReceteAdi.Text = "";
            TxtTarihi1.Text = "";
            TxtTarihi2.Text = "";
        }
    }
}
