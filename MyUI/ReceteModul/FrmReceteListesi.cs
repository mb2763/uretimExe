using DevExpress.Utils;
using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.Receteler;
using My.Entities.Receteler;
using My.Entities.Siparisler;
using My.Kontrol.Formlar;
using MyUI.ReceteModul;
using MyUI.UretimModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.ReceteModule
{
    public partial class FrmReceteListesi : MyFrmListe
    {
        private readonly IReceteAnaService _srv = Ortak.DbPro.ReceteAna;
        private readonly IReceteDetayService _srvDetay = Ortak.DbPro.ReceteDatay;
        private readonly IReceteOperasyonService _srvOperasyon = Ortak.DbPro.ReceteOperasyon;
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        public List<MikroStokCinsi> stokCinsiListFull = new List<MikroStokCinsi>();
        public FrmReceteListesi()
        {
            InitializeComponent();
            EventlerBagla();
        }
        private void EventlerBagla()
        {
            this.Load += Frm_Load;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            BaglaGrup();
            BaglaStokCinsi();
            Bagla();
            // ContexMenuyeEkle();
            myView1.SutunGizle("Id");
            myGrid1.GridYerlesimYukle();
        }

        private void ContexMenuyeEkle()
        {
            var fm1 = new ToolStripMenuItem("Uretim_EmriOlustur")
            {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Uretim EmriOlustur/Guncelle",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Uretim EmriOlustur"
            };
            fm1.Click += ConIsEmriOlusturV2;
            myGrid1.MyContextMenu.Items.Add(fm1);
        }
        private void ConIsEmriOlusturV2(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm != null)
            {
                FrmUretimEmriED f = new FrmUretimEmriED { SipId = itm.Id, ActionAktar = Bagla };
                f.ShowDialog();
            }
        }
        private void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla();
            if (!string.IsNullOrEmpty(sor))
            {
                sor = "where  1=1 " + sor;
            }
            var rs = _srv.GetListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid1.MyGridKayitAdi = "ReceteListesi";
            myGrid1.DataSource = rs.Data;
            myView1.SutunGizle("Id");
            myView1.SutunFormat(nameof(ReceteAna.KayitTarihi), FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunFormat(nameof(ReceteAna.DegistirmeTarihi), FormatType.DateTime, "dd.MM.yyyy HH:mm");

            myGrid1.GridYerlesimYukle();
            myGrid2.DataSource = null;
            myGrid3.DataSource = null;
            if (rs.Data.Any())
            {
                BaglaDetay(rs.Data.FirstOrDefault().Id);
                BaglaOperasyon(rs.Data.FirstOrDefault().Id);
            }

            Cursor.Current = Cursors.Default;
        }
        public void BaglaStokCinsi()
        {

            /*  stokCinsiList MikroStokCinsiManager.GetCinsList();*/
            stokCinsiListFull = MikroStokCinsiManager.GetCinsListFull();
            CmbStokCinsi.MyDataBagla(stokCinsiListFull, "Kodu", "Adi", new int[] { 1, 2 });
            CmbStokCinsi.Text = "Mamül";
        }
        private void BaglaDetay(Guid? rcAId)
        {

            Cursor.Current = Cursors.WaitCursor;
            var rs = _srvDetay.SelectList(c => c.RcAId == rcAId);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid2.DataSource = rs.Data;
            myView2.SutunGizle("Id");
            myView2.SutunGizle("RcAId");

            myGrid2.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }

        private void BaglaOperasyon(Guid? rcAId)
        {
            Cursor.Current = Cursors.WaitCursor;

            var rs = _srvOperasyon.SelectList(c => c.RcAId == rcAId);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid3.DataSource = rs.Data;
            myView3.SutunGizle("Id");
            myView3.SutunGizle("RcAId");

            myGrid3.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }

        private string SorguAyarla()
        {
            string sor = "";
            string t1 = TxtKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND ReceteKodu like('%{t1}%')"; }
            t1 = TxtAdi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND ReceteAdi like('%{t1}%')"; }
            t1 = CmbGrubu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND Grubu ='{t1}'"; }
            t1 = CmbStokCinsi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND StokCinsiAdi ='{t1}'"; }
            t1 = TxtAra.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND ( ReceteKodu like('%{t1}%')  or ReceteAdi  like('%{t1}%') )"; }


            return sor;
        }
        private void BaglaGrup()
        {
            var rs = _srvGenel.GrupListesi("ReceteAna", "Grubu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbGrubu.MyDataBagla(dt);
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            var _rwid = myView1.FocusedRowHandle;
            var itm = myView1.MyGetCurrentItem<ReceteAna>();
            if (itm == null) return;
            if (SecimIcinAcildi)
            {
                SecilenKod = itm.ReceteKodu;
                SecilenRow = itm;
                SecilenId = itm.Id.ToString();
                Secildi = true;
                this.Close();
                return;
            }

            FrmReceteED f = new FrmReceteED { IdGuid = itm.Id };
            f.ShowDialog();
            if (f.KayitEdildi)
            {
                BtnAra.PerformClick();
                myView1.FocusedRowHandle = _rwid;
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        { 
            var itm = myView1.MyGetCurrentItem<ReceteAna>();
            if (itm != null)
            {
                BaglaDetay(itm.Id);
                BaglaOperasyon(itm.Id);
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
            TxtKodu.Text = "";
            TxtAdi.Text = "";
            CmbGrubu.Text = "";
        }
        private void BtnReceteEkle_Click(object sender, EventArgs e)
        {
            FrmReceteED f = new FrmReceteED();
            f.ShowDialog();
            if (f.KayitEdildi)
            {
                BtnAra.PerformClick();
            }
        }
        private void BtnOperasyonEkle_Click(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<ReceteAna>();
            if (itm != null)
            {
                FrmReceteOperasyonED f = new FrmReceteOperasyonED { IdGuid = itm.Id };
                f.ShowDialog();
                if (f.KayitEdildi)
                {
                    BtnAra.PerformClick();
                }
            }
            else
            {
                MesajBilgi("Lütfen Reçete Seçiniz..");
            }
        }
        private void BtnOperasyonKopyala_Click(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<ReceteAna>();
            if (itm != null)
            {
                FrmReceteListesi fsec = new FrmReceteListesi { SecimIcinAcildi = true };
                fsec.ShowDialog();
                if (fsec.Secildi)
                {
                    var don = (ReceteAna)fsec.SecilenRow;
                    if (don != null)
                    {
                        FrmReceteOperasyonED f = new FrmReceteOperasyonED
                        {
                            IdGuid = don.Id,
                            Kolonlanacak = true,
                            KolonRecete = itm.Clone()
                        };
                        f.ShowDialog();
                        if (f.KayitEdildi)
                        {
                            BtnAra.PerformClick();
                        }
                    }
                }
            }
            else
            {
                MesajBilgi("Lütfen Operasyon Seçiniz..");
            }
        }

        private void BtnStokMaliyet_Click(object sender, EventArgs e) {
            var _rwid = myView1.FocusedRowHandle;
            var itm = myView1.MyGetCurrentItem<ReceteAna>();
            if (itm == null) return; 
            FrmReceteMaliyetStoklar f = new FrmReceteMaliyetStoklar { IdGuid = itm.Id };
            f.ShowDialog();
            
        }
    }
}