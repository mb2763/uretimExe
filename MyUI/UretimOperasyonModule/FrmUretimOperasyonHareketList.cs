using My.Business.Service.Geneller;
using My.Business.Service.UretimOperasyonlar;
using My.Entities.UretimOperasyonlar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.UretimOperasyonModule
{
    public partial class FrmUretimOperasyonHareketList : MyFrmListe
    {
        private readonly IUretimOperasyonHareketService _srv = Ortak.DbPro.UretimOperasyonHareket;
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private List<UretimOperasyonHareket> _list;
        public FrmUretimOperasyonHareketList()
        {
            InitializeComponent();
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            TxtTarihi2.Text = "";
            TxtTarihi3.Text = "";
            TxtTarihi4.Text = "";
            BtnAra.Click += BtnAra_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            BaglaDurum();
            BaglaReceteAdi();
            BaglaOperasyon();
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
        }
        private void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("UrOId");
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
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UrOH.BaslangicTarihi,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST ( coalesce(UrOH.BaslangicTarihi,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi3.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi3.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UrOH.BitisTarihi,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi4.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi4.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UrOH.BitisTarihi,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            return sor;
        }
        private string TarihAyarla(DateTime? T1)
        {
            if (T1 == null) return "";
            string FX = T1?.Year + "-" + T1?.Month.ToString().PadLeft(2, '0') + "-" +
                        T1?.Day.ToString().PadLeft(2, '0');
            return FX;
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            if (SecimIcinAcildi)
            {
                var itm = myView1.MyGetCurrentItem<UretimOperasyonHareket>();
                if (itm != null)
                {
                    SecilenRow = itm;
                    Secildi = true;
                    this.Close();
                }
            }
            else
            {
                var itm = myView1.MyGetCurrentItem<UretimOperasyonHareket>();
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
            TxtTarihi3.Text = "";
            TxtTarihi4.Text = "";
        }
    }
}