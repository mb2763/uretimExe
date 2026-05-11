
using DevExpress.XtraEditors;
using My.Business;
using My.Business.Manager;
using My.Entities.Models;
using My.Entities.Receteler;
using My.Kontrol.Yazdirma;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyUI.ReceteModule
{
    public partial class FrmReceteMaliyetGenel : XtraForm
    {
        public ReceteKayitModel Model;
        private ReceteManager _mng;
        private MikroReceteManager _mngMikro;
        public List<ReceteMaliyetGenelDetayModel> _list;
        public List<ReceteOperasyon> _operasyonlar;
        private ReceteMaliyetGenelToplamModel _toplamModel;
        private DatabaseFactoryPro _db;
        public FrmReceteMaliyetGenel()
        {
            InitializeComponent();
        }
        private void FrmReceteStokMaliyet_Load(object sender, System.EventArgs e)
        {
            _db = Ortak.DbPro;
            _mng = new ReceteManager(Ortak.DbPro);
            _mngMikro = new MikroReceteManager(Ortak.DbPro, Ortak.DbMikro);
            Bagla();
        }
        private void BtnBagla_Click(object sender, System.EventArgs e)
        {
            Bagla();
        }
        private void BtnKapat_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private void Bagla()
        {
            _list = new List<ReceteMaliyetGenelDetayModel>();
            foreach (var itm in Model.ReceteDetaylar)
            {
                if (!itm.VarsayilanStokKodu.IsNullOrEmpty())
                {
                    _list.Add(new ReceteMaliyetGenelDetayModel()
                    {
                        Cinsi = itm.Cinsi,
                        ReceteSira = itm.ReceteSira,
                        StokKullan = itm.StokKullan,
                        StokTuru = itm.StokTuru,
                        VarsayilanStokKodu = itm.VarsayilanStokKodu,
                        VarsayilanStokAdi = itm.VarsayilanStokAdi,
                        Birim = itm.Birim,
                        Renk = itm.Renk,
                        Beden = itm.Beden,
                        Miktar = itm.Miktar,
                        Fiyat = 0,
                        Tutar = 0,
                        Aciklama = itm.Aciklama,
                        //OperasyonMaliyet = itm.OperasyonMaliyet,
                    });
                }
            }
            MikroStolarBagla();
            OperasyonBagla();
            MaliyetTopla();

        }

        private void OperasyonBagla()
        {
            _operasyonlar = new List<ReceteOperasyon>();
            var operasyon = _db.ReceteOperasyon.SelectList(c => c.RcAId == Model.Recete.Id);
            if (!operasyon.Success)
            {
                operasyon.Message.MesajHata();
                return;
            }
            _operasyonlar = operasyon.Data.ToList();
            myGrid2.DataSource = _operasyonlar;
            myGrid2.GridYerlesimYukle();
            myView2.SutunGizle("Id");
            myView2.SutunGizle("UretimSure");
            myView2.SutunGizle("RcAId");
        }

        private void MaliyetTopla()
        {
            double maliyetStok = 0;
            double maliyetOperasyon = 0;

            foreach (var itm in _list)
            {

                maliyetStok += itm.Tutar;
            }

            TxtStokMaliyet.Text = Math.Round(maliyetStok, 2).ToString();
            foreach (var itm in _operasyonlar)
            {
                maliyetOperasyon += itm.MaliyetFiyat;
            }
            TxtOperasyonMaliyet.Text = Math.Round(maliyetOperasyon, 2).ToString();
            TxtGenelMaliyet.Text = Math.Round(maliyetStok + maliyetOperasyon, 2).ToString();
            _toplamModel = new ReceteMaliyetGenelToplamModel();
            _toplamModel.StokMaliyet = Math.Round(maliyetStok, 2);
            _toplamModel.OperasyonMaliyet = Math.Round(maliyetOperasyon, 2);
            _toplamModel.ToplamMaliyet = _toplamModel.StokMaliyet + _toplamModel.OperasyonMaliyet;
        }
        private void MikroStolarBagla()
        {
            string stokKodlari = "";
            foreach (var itm in _list)
            {
                if (!itm.VarsayilanStokKodu.IsNullOrEmpty())
                {

                    if (stokKodlari.IsNullOrEmpty())
                    {
                        stokKodlari += "'" + itm.VarsayilanStokKodu.ToString() + "'";
                    }
                    else
                    {
                        stokKodlari += ",'" + itm.VarsayilanStokKodu.ToString() + "'";
                    }
                }
            }
            if (stokKodlari.IsNullOrEmpty()) return;
            var rs = _mngMikro.GetStokStandartMaliyetler(stokKodlari);
            if (!rs.Success)
            {
                rs.Message.MesajHata();
                return;
            }
            var data = rs.Data.ToList();
            if (data == null) return;
            foreach (var itm in _list)
            {
                foreach (var sto in data)
                {
                    if (itm.VarsayilanStokKodu == sto.StokKodu)
                    {
                        itm.Fiyat = sto.StandartMaliyet;
                        itm.Tutar = itm.Miktar * itm.Fiyat;
                        //itm.GenelMaliyet = itm.Tutar + itm.OperasyonMaliyet;
                    }
                }
            }
            myGrid1.DataSource = _list;
            myGrid1.GridYerlesimYukle();
        }
        private void BtnYazdir_Click(object sender, System.EventArgs e)
        {
            var _mdl = _list;
            const string YazdirmaAdi = "ReceteMaliyet";
            DataSet ds = new DataSet("ReceteMaliyet");
            ds.Tables.Add(_list.ToDataTable("StokMaliyet"));
            ds.Tables.Add(_operasyonlar.ToDataTable("OperasyonMaliyet"));
            ds.Tables.Add(_toplamModel.ToDataTable("ToplamMaliyet"));
            ds.Yaz(YazdirmaAdi, false);
        }
    }
}
