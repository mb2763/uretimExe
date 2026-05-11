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
    public partial class FrmUretimIstasyonHareketDetayList : MyFrmListe
    {
        private readonly IUretimIstasyonHareketService _srv = Ortak.DbPro.UretimIstasyonHareket;
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private List<UretimIstasyonHareket> _list;
        public FrmUretimIstasyonHareketDetayList()
        {
            InitializeComponent();
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToString();
            TxtTarihi2.Text = "";
            BtnAra.Click += BtnAra_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
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
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UrIH.Tarih,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST ( coalesce(UrIH.Tarih,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
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
                var itm = myView1.MyGetCurrentItem<UretimIstasyonHareket>();
                if (itm != null)
                {
                    SecilenRow = itm;
                    Secildi = true;
                    this.Close();
                }
            }
            else
            {
                var itm = myView1.MyGetCurrentItem<UretimIstasyonHareket>();
                if (itm != null)
                {
                    FrmUretimIstasyonUretimGir f = new FrmUretimIstasyonUretimGir { IdGuid = itm.Id, Action = Bagla };
                    f.ShowDialog();
                }
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
        }
        private void BtnEkle_Click(object sender, EventArgs e)
        {
            FrmUretimIstasyonUretimGir f = new FrmUretimIstasyonUretimGir
            {
                Action = Bagla
            };
            f.ShowDialog();
        }
    }
}