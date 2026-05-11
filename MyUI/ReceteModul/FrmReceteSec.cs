using My.Business.Service.Geneller;
using My.Business.Service.Receteler;
using My.Entities.Receteler;
using My.Kontrol.Formlar;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.ReceteModule
{
    public partial class FrmReceteSec : MyFrmListe
    {
        private IReceteAnaService _srv = Ortak.DbPro.ReceteAna;
        private IReceteDetayService _srvDetay = Ortak.DbPro.ReceteDatay;
        private IReceteOperasyonService _srvOperasyon = Ortak.DbPro.ReceteOperasyon;
        private IGenelService _srvGenel = Ortak.DbPro.GenelServis;

        public FrmReceteSec()
        {
            InitializeComponent();
            this.Load += Frm_Load;
            myView1.MyEventDoubleClickEnter += myView1_MyEventDoubleClickEnter;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            BaglaGrup();
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
        }
        private void SutunGizle()
        {
            myView1.SutunGizle("Id");

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
            ;
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid1.DataSource = rs.Data;
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
        private void myView1_MyEventDoubleClickEnter()
        {
            var itm = myView1.MyGetCurrentItem<ReceteAna>();
            if (itm == null)
            {
                return;
            }
            if (SecimIcinAcildi)
            {
                SecilenKod = itm.ReceteKodu;
                SecilenRow = itm;
                SecilenId = itm.Id.ToString();
                Secildi = true;
                this.Close();
                return;
            }
            FrmReceteED f = new FrmReceteED();
            f.IdGuid = itm.Id;
            f.ShowDialog();
            if (f.KayitEdildi)
            {
                BtnAra.PerformClick();
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
            CmbGrubu.Text = ""; ;
        }
    }
}