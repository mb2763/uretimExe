using My.Business.Service.Geneller;
using My.Business.Service.UretimIstasyonlar;
using My.Entities.UretimIstasyonlar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.UretimIstasyonModule
{
    public partial class FrmUretimIstasyonHareketList : MyFrmListe
    {
        private readonly IUretimIstasyonService _srv = Ortak.DbPro.UretimIstasyon;
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly IUretimIstasyonHareketService _srvDty = Ortak.DbPro.UretimIstasyonHareket;
        private List<UretimIstasyonHareket> _listDty;
        private List<UretimIstasyon> _list;
        public FrmUretimIstasyonHareketList()
        {
            InitializeComponent();
            this.Load += Frm_Load;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToString();
            TxtTarihi2.Text = "";
            TxtTarihi3.Text = "";
            TxtTarihi4.Text = "";
            BtnAra.Click += BtnAra_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            BtnTemizle.Click += BtnTemizle_Click;

            BaglaDurum();
            BaglaReceteAdi();
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
        }

        private void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("UrOId");
            myView1.SutunGizle("UrOHId");
            myView1.SutunGizle("UrOHDId");
            myView1.SutunGizle("RcAId");
            myView1.SutunGizle("RcOId");
            myView1.SutunGizle("RcIstId");
            myView1.SutunGizle("SipId");

        }
        private void SutunGizle2()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("UrOId");
            myView1.SutunGizle("UrOHId");
            myView1.SutunGizle("UrOHDId");
            myView1.SutunGizle("UrIId");
            myView1.SutunGizle("RcAId");
            myView1.SutunGizle("RcOId");
            myView1.SutunGizle("SipId");

        }
        private void Bagla()
        {
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
                BaglaDetay(rs.Data.FirstOrDefault().Id);
            }
            Cursor.Current = Cursors.Default;
        }
        private void BaglaDetay(Guid? UrIId)
        {

            string sor = "where UrIId ='" + UrIId + "' ";

            var rs = _srvDty.GetViewListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _listDty = rs.Data.ToList();
            myGrid2.DataSource = _listDty;
            SutunGizle2();
            myGrid2.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }
        private void BaglaDurum()
        {
            var rs = _srvGenel.GrupListesi("UretimIstasyon", "IstasyonKodu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbIstasyonKodu.DataSource = dt;
        }
        private void BaglaReceteAdi()
        {
            var rs = _srvGenel.GrupListesi("UretimEmri", "ReceteAdi");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbReceteAdi.DataSource = dt;
        }
        private string SorguAyarla()
        {
            string sor = "";
            string t1 = CmbIstasyonKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  UrI.IstasyonKodu = '{t1}' "; }
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
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UrI.BaslangicTarihi,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST ( coalesce(UrI.BaslangicTarihi,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi3.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi3.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UrI.BitisTarihi,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi4.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi4.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UrI.BitisTarihi,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            return sor;
        }
        private static string TarihAyarla(DateTime? T1)
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
                var itm = myView1.MyGetCurrentItem<UretimIstasyon>();
                if (itm != null)
                {
                    SecilenRow = itm;
                    Secildi = true;
                    this.Close();
                }
            }
            else
            {
                var itm = myView1.MyGetCurrentItem<UretimIstasyon>();
                if (itm != null)
                {
                    //FrmUretimEmriED_V2 f = new FrmUretimEmriED_V2();
                    //f.AltBarStatusPanel.Visible = false;
                    //f.Idsi = itm.Id;
                    //f.ActionAktar = Bagla;
                    //f.ShowDialog();
                }
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            var itm = myView1.MyGetCurrentItem<UretimIstasyon>();
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
        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            CmbIstasyonKodu.Text = "";
            CmbReceteAdi.Text = "";
            TxtTarihi1.Text = "";
            TxtTarihi2.Text = "";
            TxtTarihi3.Text = "";
            TxtTarihi4.Text = "";
        }
    }
}