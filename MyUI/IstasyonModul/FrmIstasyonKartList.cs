using My.Business.Service.IstasyonKartlar;
using My.Business.Service.OperasyonKartlar;
using My.Business.Service.ReceteIstasyonlar;
using My.Entities.IstasyonKartlar;
using My.Entities.ReceteIstasyonlar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.IstasyonModul
{
    public partial class FrmIstasyonKartList : MyFrmListe
    {

        private readonly IIstasyonKartiService _srv = Ortak.DbPro.IstasyonKarti;
        private readonly IReceteyeBagliIstasyonService _srvBIst = Ortak.DbPro.ReceteyeBagliIstasyon;
        private readonly IOperasyonKartiService _srvOpr = Ortak.DbPro.OperasyonKarti;
        private List<IstasyonKarti> _list;
        private List<ReceteyeBagliIstasyon> _listBI;
        public string Aranan = "";
        public bool BagliIstasyonSec = false;
        public Guid? RcAId = null;

        public FrmIstasyonKartList()
        {
            InitializeComponent();
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            BaglaOperasyon();
            BaglaBagliIstasyon();
            if (SecimIcinAcildi && !string.IsNullOrEmpty(Aranan))
            {
                CmbOperasyon.Text = Aranan;
                Bagla();
            }
            else
            {
                Bagla();
            }
            SutunGizle();
            myGrid1.GridYerlesimYukle();
        }
        public void SutunGizle()
        {
            myView1.SutunGizle("Id");
        }
        public void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla();
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
            var gdt = rs.Data.ToList();
            if (ChcTumu.Checked)
            {
                _list = rs.Data.ToList();
            }
            else
            {
                if (_listBI!=null && _listBI.Count > 0)
                {
                    _list = new List<IstasyonKarti>();
                    foreach (var itm in gdt)
                    {
                        foreach (var ib in _listBI)
                        {
                            if (itm.Id == ib.RcIId)
                            {
                                _list.Add(itm.Clone());
                            }
                        }
                    }
                }
                else
                {
                    _list = rs.Data.ToList();
                }
            }

            bs.DataSource = _list;
            myGrid1.DataSource = bs;
            GridBagla();
            lblKayitSayisi.Caption = rs.Data.Count().ToString();
            Cursor.Current = Cursors.Default;
        }

        private string SorguAyarla()
        {
            string sor = "";
            string t1 = CmbOperasyon.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND Operasyon ='{t1}"; }//'('%{t1}%')
            return sor;
        }
        public void GridBagla()
        {
            myGrid1.DataSource = null;
            bs.DataSource = _list;
            myGrid1.DataSource = bs;
        }
        public void BaglaOperasyon()
        {
            var rs = _srvOpr.SelectListWhere(" Order By OperasyonKodu");//GrupListesi("OperasyonKarti", "OperasyonKodu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            CmbOperasyon.MyDataBagla(dt, "OperasyonKodu", "OperasyonKodu", new int[] { 1, 2 });
        }
        public void BaglaBagliIstasyon()
        {
            if (RcAId == null) return;
            var rs = _srvBIst.SelectListWhere(" where  RcAId='" + RcAId + "'");//GrupListesi("OperasyonKarti", "OperasyonKodu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            _listBI = rs.Data.ToList();
        }
        private void TemizleText()
        {
            CmbOperasyon.Text = "";
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            TemizleText();
        }

        private void BtnAra_Click(object sender, EventArgs e)
        {
            Bagla();
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            if (myView1.FocusedRowHandle < 0) return;
            var itmAl = myView1.GetRow(myView1.FocusedRowHandle);
            if (itmAl == null || itmAl.GetType() != typeof(IstasyonKarti)) return;
            var itm = (IstasyonKarti)itmAl;
            if (SecimIcinAcildi)
            {
                SecilenKod = itm.IstasyonKodu;
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
