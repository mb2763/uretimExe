using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.UretimTalepler;
using My.Entities.UretimTalepler;
using My.Kontrol.Formlar;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.UretimTalepler
{
    public partial class FrmUretimTalepList : MyFrmListe
    {

        private readonly IUretimTalepService _srv = Ortak.DbPro.UretimTalep;
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly IUretimTalepHareketService _srvHareket = Ortak.DbPro.UretimTalepHareket;
        /*
                private ISiparisHareketDetayService _srvDetay = Ortak.DbPro.SiparisHareketDetay;
        */
        private UretimTalepManager _mng;
        public string Turu = "Siparis";

        public FrmUretimTalepList()
        {
            InitializeComponent();
            this.Load += Frm_Load;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
        }

        private void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
            if (!string.IsNullOrEmpty(sor))
            {
                sor = "where 1=1 " + sor;
            }
            var rs = _srv.SelectListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid1.DataSource = rs.Data;
            if (rs.Data.Any())
            {
                UretimTalep first = null;
                foreach (var siparis in rs.Data)
                {
                    first = siparis;
                    break;
                }
                if (first != null)
                    BaglaHareket(first.UrtTlpId);
            }
            Cursor.Current = Cursors.Default;
        }


        private void BaglaHareket(Guid? urtTlpId)
        {
            Cursor.Current = Cursors.WaitCursor;
            var rs = _srvHareket.SelectList(c => c.UrtTlpId == urtTlpId);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid2.DataSource = rs.Data;
            SutunGizle2();
            myGrid2.Refresh();
            myGrid2.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }

        private void BtnAra_Click(object sender, EventArgs e)
        {
            Bagla();
        }
        private void BtnEkleUretimTalep_Click(object sender, EventArgs e)
        {
            FrmUretimTalepED f = new FrmUretimTalepED
            {
                ActionAktar = Bagla
            };
            f.ShowDialog();
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {

            TxtTarihi1.Text = "";
            TxtTarihi2.Text = "";
        }



        private void Frm_Load(object sender, EventArgs e)
        {

            TxtTarihi1.EditValue = DateTime.Now.AddDays(-7);
            _mng = new UretimTalepManager(Ortak.DbPro);
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();

        }

        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<UretimTalep>();
            if (itm != null)
            {
                BaglaHareket(itm.UrtTlpId);
            }
        }

        private void MyView1_MyEventDoubleClickEnter()
        {
            var itm = myView1.MyGetCurrentItem<UretimTalep>();
            if (itm == null) return;
            if (SecimIcinAcildi)
            {
                SecilenKod = itm.EvrakNo.ToString();
                SecilenRow = itm;
                Secildi = true;
                this.Close();
            }
            else
            {

                FrmUretimTalepED f = new FrmUretimTalepED
                {
                    IdGuid = itm.UrtTlpId,
                    ActionAktar = Bagla
                };
                f.ShowDialog();

            }
        }

        private string SorguAyarla()
        {
            string sor = "";
            //string t1 = Turu;
            //if (!string.IsNullOrEmpty(t1)) { sor += $" AND  Turu = '{t1}' "; }
            //t1 = TxtKodu.Text.Trim();
            //if (!string.IsNullOrEmpty(t1)) { sor += $" AND  SiparisKodu    like('%{t1}%')"; }
            //t1 = TxtCariKodu.Text.Trim();
            //if (!string.IsNullOrEmpty(t1)) { sor += $" AND  CariKodu    like('%{t1}%')"; }
            //t1 = TxtAdi.Text.Trim();
            //if (!string.IsNullOrEmpty(t1)) { sor += $" AND  CariUnvani like('%{t1}%')  "; }
            //t1 = CmbDurumu.Text.Trim();
            //if (!string.IsNullOrEmpty(t1)) { sor += $" AND  Durumu = '{t1}' "; }
            //if (CmbKapandi.Text == "Kapandı") {
            //    sor += " AND  coalesce(Kapandi,0) = 1   ";
            //}
            //else if (CmbKapandi.Text == "Açık") {
            //    sor += " AND  coalesce(Kapandi,0) = 0   ";
            //}
            return sor;
        }

        private string SorguAyarlaTrh()
        {
            string sor = "";
            var t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.EditValue));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( Tarih,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.EditValue));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( Tarih,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            return sor;
        }

        private void SutunGizle()
        {
            myView1.SutunGizle("UrtTlpId");
            myView1.SutunGizle("Ent");
            myView1.SutunGizle("EntId");
            myView1.SutunGizle("EntKodu");
            myView1.SutunGizle("EntKodu2");
            myView1.SutunGizle("EntTarih");
        }

        private void SutunGizle2()
        {
            myView2.SutunGizle("UrtTlpHrId");
            myView2.SutunGizle("UrtTlpId");
            myView1.SutunGizle("Id");
            myView1.SutunGizle("Ent");
            myView1.SutunGizle("EntId");
            myView1.SutunGizle("EntKodu");
            myView1.SutunGizle("EntKodu2");
            myView1.SutunGizle("EntTarih");
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


    }
}
