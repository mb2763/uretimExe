using DevExpress.XtraEditors;
using My.Business.Manager;
using My.Business;
using My.Entities.Models;
using My.Entities.Receteler;
using My.Kontrol.Yazdirma;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.CodeParser;

namespace MyUI.ReceteModul {
    public partial class FrmReceteMaliyetStoklar : XtraForm {
        public Guid? IdGuid { get; set; }
        public ReceteKayitModel Model;
        private ReceteManager _mng;
        private MikroReceteManager _mngMikro;
        public List<ReceteMaliyetStokDetayModel> _list;
        private ReceteMaliyetStokToplamModel _toplamModel;
        private DatabaseFactoryPro _db;
        public FrmReceteMaliyetStoklar() {
            InitializeComponent();
        }
        private void FrmReceteStokMaliyet_Load(object sender, System.EventArgs e) {

            _db = Ortak.DbPro;
            _mng = new ReceteManager(Ortak.DbPro);
            _mngMikro = new MikroReceteManager(Ortak.DbPro, Ortak.DbMikro);
            Bagla();
        }
        private void BtnBagla_Click(object sender, System.EventArgs e) {
            Bagla();
        }
        private void BtnKapat_Click(object sender, System.EventArgs e) {
            this.Close();
        }
        private void Bagla() {

            var rs = _mng.GetReceteKayit(IdGuid);
            if (!rs.Success) {
                rs.Message.MesajHata();
                return;
            }
            Model = rs.Data;


            _list = new List<ReceteMaliyetStokDetayModel>();
            foreach (var itm in Model.ReceteDetaylar) {
                if (!itm.VarsayilanStokKodu.IsNullOrEmpty()) {
                    _list.Add(new ReceteMaliyetStokDetayModel() {
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
                        SonAlis = 0,
                        Son5AlisOrtalama = 0,
                        StandartMaliyet = 0,
                        SonSayimGiris = 0,
                        DevirGiris = 0,
                        SonAlisTutar = 0,
                        Son5AlisTutar = 0,
                        StandartMaliyetTutar = 0,
                        SonSayimGirisTutar = 0,
                        DevirGirisTutar = 0,
                        Aciklama = itm.Aciklama,
                        //OperasyonMaliyet = itm.OperasyonMaliyet,
                    });
                }
            }
            MikroStolarBagla();

            MaliyetTopla();

        }



        private void MaliyetTopla() {
        
            double SonAlis = 0;
            double Son5Alis  = 0;
            double StandartMaliyet = 0;
            double SonSayimGiris = 0;
            double DevirGiris = 0;


            foreach (var itm in _list) {

                SonAlis += itm.SonAlisTutar;
                Son5Alis  += itm.Son5AlisTutar;
                StandartMaliyet += itm.StandartMaliyetTutar;
                SonSayimGiris += itm.SonSayimGirisTutar;
                DevirGiris += itm.DevirGirisTutar;
            }

            TxtSonAlis.Text = Math.Round(SonAlis, 2).ToString();
            TxtSon5Alis.Text = Math.Round(Son5Alis, 2).ToString();
            TxtStandart .Text = Math.Round(StandartMaliyet, 2).ToString();
            TxtSonSayim .Text = Math.Round(SonSayimGiris, 2).ToString();
            TxtDevir .Text = Math.Round(DevirGiris, 2).ToString(); 
            _toplamModel = new ReceteMaliyetStokToplamModel();
            _toplamModel.SonAlisTutar = Math.Round(SonAlis, 2);
            _toplamModel.Son5AlisTutar = Math.Round(Son5Alis, 2);
            _toplamModel.StandartMaliyetTutar = Math.Round(StandartMaliyet, 2);
            _toplamModel.SonSayimGirisTutar = Math.Round(SonSayimGiris, 2);
            _toplamModel.DevirGirisTutar = Math.Round(DevirGiris, 2);
             
        }
        private void MikroStolarBagla() {
            string stokKodlari = "";
            foreach (var itm in _list) {
                if (!itm.VarsayilanStokKodu.IsNullOrEmpty()) {

                    if (stokKodlari.IsNullOrEmpty()) {
                        stokKodlari += "'" + itm.VarsayilanStokKodu.ToString() + "'";
                    }
                    else {
                        stokKodlari += ",'" + itm.VarsayilanStokKodu.ToString() + "'";
                    }
                }
            }
            if (stokKodlari.IsNullOrEmpty()) return;
            var rs = _mngMikro.GetStokSonAlisSatisFiyatlar(stokKodlari);
            if (!rs.Success) {
                rs.Message.MesajHata();
                return;
            }
            var data = rs.Data.ToList();
            if (data == null) return;
            foreach (var itm in _list) {
                foreach (var sto in data) {
                    if (itm.VarsayilanStokKodu == sto.StokKodu) {
                        itm.SonAlis = sto.SonAlis;
                        itm.Son5AlisOrtalama = sto.Son5AlisOrtalama;
                        itm.StandartMaliyet = sto.StandartMaliyet;
                        itm.SonSayimGiris = sto.SonSayimGiris;
                        itm.DevirGiris = sto.DevirGiris;
                        itm.SonAlisTutar = itm.Miktar * itm.SonAlis;
                        itm.Son5AlisTutar = itm.Miktar * itm.Son5AlisOrtalama;
                        itm.StandartMaliyetTutar = itm.Miktar * itm.StandartMaliyet;
                        itm.SonSayimGirisTutar = itm.Miktar * itm.SonSayimGiris;
                        itm.DevirGirisTutar = itm.Miktar * itm.DevirGiris;
                        //itm.GenelMaliyet = itm.Tutar + itm.OperasyonMaliyet;
                    }
                }
            }
            myGrid1.DataSource = _list;
            myGrid1.GridYerlesimYukle();
        }
        private void BtnYazdir_Click(object sender, System.EventArgs e) {
            var _mdl = _list;
            const string YazdirmaAdi = "ReceteStoklarMaliyet";
            DataSet ds = new DataSet("ReceteStoklarMaliyet");
            ds.Tables.Add(_list.ToDataTable("StokMaliyet"));
            ds.Tables.Add(_toplamModel.ToDataTable("ToplamMaliyet"));
            ds.Yaz(YazdirmaAdi, false);
        }


    }
}
