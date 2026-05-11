using DevExpress.XtraGrid.Views.Grid;
using My.Business;
using My.Business.Service.Geneller;
using My.Business.Service.MikroModul;
using My.Business.Service.Siparisler;
using My.Entities.Mikro;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.MikroModule
{
    public partial class FrmMikroSiparisListesi : MyFrmListe
    {
        private DatabaseFactoryMikro _db = Ortak.DbMikro;
        private IMikroSiparisService _srv = Ortak.DbMikro.Siparisler;
        private ISiparisService _srvSiparisPro = Ortak.DbPro.Siparis;
        private IMikroSiparisHareketService _srvHareket = Ortak.DbMikro.SiparisHareketler;
        private IMikroGenelService _srvGenel = Ortak.DbMikro.GenelServis;
        private List<MikroSiparis> _list;
        public FrmMikroSiparisListesi()
        {
            InitializeComponent();
            this.Load += Frm_Load;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView1.RowStyle += MyGridView_RowStyle;
            myView1.CustomDrawRowIndicator += myView1_CustomDrawRowIndicator;
        }
        void myView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            Bagla();
            TxtTarihi1.EditValue = DateTime.Now.AddYears(-1);
            myView1.SutunGizle("SipGuid");
            ContexMenuyeEkle();
            myGrid1.GridYerlesimYukle();
            myView1.IndicatorWidth = 30;
            TxtAra.Focus();
            TxtTarihi2.Text = "";
        }
        private void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
            if (!string.IsNullOrEmpty(sor)) sor = "where 1 = 1 " + sor;
            var rs = _srv.GetViewListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            bs.DataSource = _list;
            BaglaProSiparisList();
            myGrid1.DataSource = bs;
            myGrid2.DataSource = null;
            if (rs.Data.Count() > 0)
            {
                BaglaHareket(rs.Data.FirstOrDefault().EvrakSeri.ToString(), rs.Data.FirstOrDefault().EvrakSira.ToString());
            }
            Cursor.Current = Cursors.Default;
        }
        private void BaglaProSiparisList()
        {
            var rs = _srvSiparisPro.SelectListWhere(" where Turu ='MikroSiparis' ");
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            if (rs.Data != null)
            {
                foreach (var itm in rs.Data)
                {
                    foreach (var itmx in _list)
                    {
                        if (itm.SiparisKodu.ToString().Trim() == itmx.EvrakSeri.ToString().Trim() + itmx.EvrakSira.ToString().Trim())
                        {
                            itmx.Aktarildi = true;
                            goto Atla;
                        }
                    }
                Atla:;
                }
                bs.EndEdit();
            }
            Cursor.Current = Cursors.Default;
        }
        private void GridView_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string priority = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Aktarildi"]);
                if (priority == "Checked")
                {
                    e.Appearance.BackColor = Color.FromArgb(200, Color.Indigo);
                    e.Appearance.ForeColor = Color.LightGray;
                }
                else
                {
                    e.Appearance.ForeColor = Color.Black;
                }
            }
            e.HighPriority = true;
        }
        private void MyGridView_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                string priority = myView1.GetRowCellValue(e.RowHandle, "Aktarildi").ToString();
                if (priority == "True")
                {
                    e.Appearance.BackColor = Color.FromArgb(200, Color.DarkGreen);
                    e.Appearance.ForeColor = Color.LightGray;
                }
                e.HighPriority = true;
            }
        }
        private void BaglaHareket(string seri, string sira)
        {
            Cursor.Current = Cursors.WaitCursor;
            var rs = _srvHareket.GetViewListSeriSira(seri, sira);
            ;
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid2.DataSource = rs.Data;
            myView2.SutunGizle("SipHGuid");
            myGrid2.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }
        private void ContexMenuyeEkle()
        {
            ToolStripMenuItem Fm1;
            Fm1 = new ToolStripMenuItem("Uretim_EmriOlustur")
            {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Uretim EmriOlustur/Guncelle",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Uretim EmriOlustur"
            };
            Fm1.Click += ConIsEmriOlusturV2;
            myGrid1.MyContextMenu.Items.Add(Fm1);
        }
        private void ConIsEmriOlusturV2(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<MikroSiparis>();
            if (itm != null)
            {
                FrmMikroUretimEmriOlustur f = new FrmMikroUretimEmriOlustur();
                f.SipGuid = itm.SipGuid;
                f.Action = Bagla;
                f.ShowDialog();
            }
        }
        private string SorguAyarla()
        {
            string sor = "";
            string t1 = TxtKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND Sip.CariKodu    like('%{t1}%')"; }
            t1 = TxtAdi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND (Sip.CariUnvani like('%{t1}%')   )"; }
            t1 = TxtAra.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND (Sip.CariKodu    like('%{t1}%')  or Sip.CariUnvani like('%{t1}%')   )"; }
            if (ChcSiparisAcikKapali.Checked)
            {
                sor += $" AND (Sip.SiparisAcikKapali  ='Kapalı'  )";
            }
            else
            {
                sor += $" AND (Sip.SiparisAcikKapali   = 'Açık' )";
            }
            return sor;
        }
        private string SorguAyarlaTrh()
        {
            string sor = "";
            string t1 = "";
            t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.EditValue));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( Sip.Tarih,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.EditValue));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( Sip.Tarih,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
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
        private void BtnAra_Click(object sender, EventArgs e)
        {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            TxtKodu.Text = "";
            TxtAdi.Text = "";
            TxtAra.Text = "";
            TxtTarihi1.Text = "";
            TxtTarihi2.Text = "";
            TxtAra.Focus();
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            if (SecimIcinAcildi)
            {
                var itm = myView1.MyGetCurrentItem<MikroSiparis>();
                if (itm != null)
                {
                    SecilenKod = itm.CariKodu;
                    SecilenRow = itm;
                    Secildi = true;
                    this.Close();
                }
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<MikroSiparis>();
            if (itm != null)
            {
                BaglaHareket(itm.EvrakSeri, itm.EvrakSira);
            }
            else
            {
                myGrid2.DataSource = null;
            }
        }
    }
}