using My.Business.Service.Geneller;
using My.Business.Service.ReceteGruplar;
using My.Business.Service.Receteler;
using My.Entities.ReceteGruplar;
using My.Kontrol.Formlar;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.ReceteModule
{
    public partial class FrmReceteGrupListesi : MyFrmListe
    {
        private IReceteGrupService _srv = Ortak.DbPro.ReceteGrup;
        private IReceteGrupDetayService _srvDetay = Ortak.DbPro.ReceteGrupDetay;
        private IReceteAnaService _srvRecete = Ortak.DbPro.ReceteAna;
        private IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        public FrmReceteGrupListesi()
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
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
        }
        private void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla();
            if (!string.IsNullOrEmpty(sor))
            {
                sor = "where  1=1 " + sor;
            }
            var rs = _srv.SelectListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid1.MyGridKayitAdi = "ReceteGrupListesi";
            myGrid1.DataSource = rs.Data;
            Cursor.Current = Cursors.Default;
        }
        private void SutunGizle()
        {
            myView1.SutunGizle("Id");

        }
        private void BaglaHareket(Guid? rcGid)
        {
            Cursor.Current = Cursors.WaitCursor;
            var rs1 = _srvDetay.SelectList(c => c.RcGId == rcGid);
            ;
            if (!rs1.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs1.Message);
                return;
            }
            string idler = "";
            foreach (var itm in rs1.Data)
            {
                if (string.IsNullOrEmpty(idler))
                {
                    idler += " '" + itm.RcAId.ToString() + "'";
                }
                else
                {
                    idler += ",'" + itm.RcAId.ToString() + "'";
                }
            }
            if (string.IsNullOrEmpty(idler.Trim()))
            {
                return;
            }
            var rs = _srvRecete.SelectListWhere(" where Id IN(" + idler + ")");
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid2.DataSource = rs.Data;
            SutunGizle2();
            myGrid2.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }
        private void SutunGizle2()
        {
            myView2.SutunGizle("Id");

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
        private string SorguAyarla()
        {
            string sor = "";
            string t1 = TxtKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND ReceteGrupKodu like('%{t1}%')"; }
            t1 = TxtAdi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND Aciklama like('%{t1}%')"; }
            t1 = CmbGrubu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND Grubu ='{t1}'"; }
            return sor;
        }
        private void BtnAra_Click(object sender, EventArgs e)
        {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            TxtKodu.Text = "";
            TxtAdi.Text = "";
            CmbGrubu.Text = ""; ;
        }
        private void BtnGrupTakimEkle_Click(object sender, EventArgs e)
        {
            FrmReceteGrupED f = new FrmReceteGrupED();
            f.ShowDialog();
            if (f.KayitEdildi)
            {
                BtnAra.PerformClick();
            }
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            var itm = myView1.MyGetCurrentItem<ReceteGrup>();
            if (itm == null) return;
            if (SecimIcinAcildi)
            {
                SecilenKod = itm.ReceteGrupKodu;
                SecilenRow = itm;
                SecilenId = itm.Id.ToString();
                Secildi = true;
                this.Close();
            }
            else
            {
                FrmReceteGrupED f = new FrmReceteGrupED();
                f.IdGuid = itm.Id;
                f.ShowDialog();
                if (f.KayitEdildi)
                {
                    BtnAra.PerformClick();
                }
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<ReceteGrup>();
            if (itm != null)
            {
                BaglaHareket(itm.Id);
            }
        }
    }
}