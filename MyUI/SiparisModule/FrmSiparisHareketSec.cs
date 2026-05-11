using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.Siparisler;
using My.Entities.Siparisler;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.SiparisModule
{
    public partial class FrmSiparisHareketSec : MyFrmListe
    {
        ISiparisHareketService _srv = Ortak.DbPro.SiparisHareket;
        IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private SiparisManager _mng;
        private List<SiparisHareketModel> _list;
        public FrmSiparisHareketSec()
        {
            InitializeComponent();
            this.Load += Frm_Load;
        }

        private void Frm_Load(object sender, EventArgs e)
        {
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            TxtTarihi1.EditValue = DateTime.Now.AddMonths(-1);
            CmbKapandi.Text = "Tümü";
            _mng = new SiparisManager(Ortak.DbPro, Ortak.DbMikro);
            BaglaDurum();
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            TxtAdi.Focus();
        }
        private void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("SipId");
            myView1.SutunGizle("RcAId");
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
            var rs = _srvGenel.GrupListesi("Siparis", "Durumu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbDurumu.DataSource = dt;
        }
        private string SorguAyarla()
        {
            string sor = "";
            string t1 = TxtKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  S.SiparisKodu    like('%{t1}%')"; }
            t1 = TxtCariKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  S.CariKodu    like('%{t1}%')"; }
            t1 = TxtAdi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  S.CariUnvani like('%{t1}%')  "; }
            t1 = CmbDurumu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  S.Durumu = '{t1}' "; }
            if (CmbKapandi.Text.ToString() == "Kapandı")
            {
                sor += $" AND  coalesce(S.Kapandi,0) = 1   ";
            }
            else if (CmbKapandi.Text.ToString() == "Açık")
            {
                sor += $" AND  coalesce(S.Kapandi,0) = 0   ";
            }
            return sor;
        }
        private string SorguAyarlaTrh()
        {
            string sor = "";
            string t1 = "";
            t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.EditValue)).ToString();
            if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST ( S.Tarih AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.EditValue)).ToString();
            if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST ( S.Tarih AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
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
        private void BtnAra_Click(object sender, EventArgs e)
        {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            TxtKodu.Text = "";
            TxtAdi.Text = "";
            CmbDurumu.Text = "";
            TxtCariKodu.Text = "";
            TxtTarihi1.Text = "";
            TxtTarihi2.Text = "";
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            var itm = myView1.MyGetCurrentItem<SiparisHareketModel>();
            if (itm == null) return;
            if (SecimIcinAcildi)
            {
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