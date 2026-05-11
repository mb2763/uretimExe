using DevExpress.XtraGrid.Views.Grid;
using My.Business.Manager;
using My.Business.Service.Receteler;
using My.Entities.Models;
using My.Kontrol.Formlar;
using MyUI.MikroModul;
using MyUI.ReceteModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.MikroModule
{
    public partial class FrmMikroReceteListesi : MyFrmListe
    {
        private MikroReceteManager _mng;
        private readonly IReceteAnaService _srvRecete = Ortak.DbPro.ReceteAna;
        private readonly IReceteOperasyonService _srvOperasyon = Ortak.DbPro.ReceteOperasyon;
        private List<MikroReceteModel> _list;
        public FrmMikroReceteListesi()
        {
            InitializeComponent();
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new MikroReceteManager(Ortak.DbPro, Ortak.DbMikro);
            comboBox1.Text = "Aktif";
            Bagla();
            ContexMenuyeEkle();
            myGrid1.GridYerlesimYukle(myGrid1.MyGridKayitAdi);
            myView1.IndicatorWidth = 30;
            myView1.CustomDrawRowIndicator += MyView1_CustomDrawRowIndicator;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
            myView1.RowStyle += GridView_RowStyle;
            TxtAra.Focus();

        }
        private void ContexMenuyeEkle()
        {
            ToolStripMenuItem Fm1;
            Fm1 = new ToolStripMenuItem("ReceteAktar")
            {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Receteyi İçeri Aktar",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Receteyi İçeri Aktar"
            };
            Fm1.Click += ContexMenuReceteAktar;
            myGrid1.MyContextMenu.Items.Add(Fm1);

            ToolStripMenuItem Fm2;
            Fm2 = new ToolStripMenuItem("ReceteFireGuncelle")
            {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Recete Fire Yüzde Guncelle",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Recete Fire Yüzde Guncelle"
            };
            Fm2.Click += ContexMenuFireGuncelle;
            myGrid1.MyContextMenu.Items.Add(Fm2);

        }
        private void ContexMenuReceteAktar(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<MikroReceteModel>();
            if (itm != null)
            {
                FrmReceteED f = new FrmReceteED();
                f.MikroReceteKodu = itm.ReceteKodu;
                f.MikrodanAktar = true;
                f.ShowDialog();
            }
        }
        private void ContexMenuFireGuncelle(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<MikroReceteModel>();
            if (itm != null)
            {
                FrmMikroReceteFireGuncelle f = new FrmMikroReceteFireGuncelle();
                f.MikroReceteKodu = itm.ReceteKodu;
                f.MikrodanAktar = true;
                f.ShowDialog();
            }
        }
        private void MyView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            if (myView1.FocusedRowHandle < 0) return;
            var itmAl = myView1.GetRow(myView1.FocusedRowHandle);
            if (itmAl == null || itmAl.GetType() != typeof(MikroReceteModel)) return;
            var itm = (MikroReceteModel)itmAl;
            if (SecimIcinAcildi)
            {
                SecilenKod = itm.ReceteKodu;
                SecilenRow = itm;
                Secildi = true;
                this.Close();
            }
            else
            {
            }
        }
        private void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla();
            sor = " where rec_iptal=0   " + sor;
            var rs = _mng.GetMikroReceteList(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            BaglaProReceteList();
            myGrid1.DataSource = _list;
            myGrid1.GridYerlesimYukle();
            myGrid2.DataSource = null;
            myGrid3.DataSource = null;
            if (rs.Data.Any())
            {
                BaglaPro(rs.Data.FirstOrDefault().ReceteKodu);
            }
            lblKayitSayisi.Caption = rs.Data.Count().ToString();
            Cursor.Current = Cursors.Default;
        }
        private void BaglaProReceteList()
        {
            var rs = _srvRecete.SelectListWhere();
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
                        if (itm.ReceteKodu.ToString().Trim() == itmx.ReceteKodu.ToString().Trim())
                        {
                            itmx.Aktarildi = true;
                            goto Atla;
                        }
                    }
                Atla:;
                }
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
                    e.Appearance.BackColor = Color.FromArgb(200, Color.DarkGreen);
                    e.Appearance.ForeColor = Color.LightGray;
                }
            }
            e.HighPriority = true;
        }
        private void BaglaPro(string receteKodu)
        {
            string sor = " where ReceteKodu= '" + receteKodu + "'";
            var rs = _srvRecete.SelectListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid2.DataSource = rs.Data;
            myView2.SutunGizle("Id");
            myView2.SutunGizle("RcAGuid");
            myGrid2.GridYerlesimYukle();
            myGrid3.DataSource = null;
            if (rs.Data.Any())
            {
                BaglaOperasyon(rs.Data.FirstOrDefault().Id);
            }
            Cursor.Current = Cursors.Default;
        }
        private void BaglaOperasyon(Guid? rcaid)
        {
            Cursor.Current = Cursors.WaitCursor;
            var rs = _srvOperasyon.SelectList(c => c.RcAId == rcaid);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid3.DataSource = rs.Data;
            myView3.SutunGizle("Id");
            myView3.SutunGizle("RcAId");
            myView3.SutunGizle("RcAGuid");
            myView3.SutunGizle("RcOGuid");
            myGrid3.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }
        public string SorguAyarla()
        {
            string sor = "";
            string t1 = TxtAra.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND ( rec_anakod like('%{t1}%')  or sto_isim  like('%{t1}%') )"; }

            t1 = comboBox1.Text.Trim();
            if (!string.IsNullOrEmpty(t1) && t1 != "Tümü")
            {

                if (t1 == "Pasif")
                {
                    sor += $" AND   coalesce(sto_pasif_fl,0)  = 1 ";
                }
                else
                {
                    sor += $" AND   coalesce(sto_pasif_fl,0)  = 0 ";
                }
            }

            return sor;
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<MikroReceteModel>();
            if (itm != null)
            {
                BaglaPro(itm.ReceteKodu);
            }
            else
            {
                myGrid2.DataSource = null;
                myGrid3.DataSource = null;
            }
        }
        private void BtnAra_Click(object sender, EventArgs e)
        {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            TxtAra.Text = "";
        }
    }
}