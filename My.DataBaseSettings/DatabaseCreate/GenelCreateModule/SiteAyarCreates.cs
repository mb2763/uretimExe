using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule {
    public static class SiteAyarCreates {

        private const string _tabloAdi = "SiteAyar";
        private const string _idAdi = "Id";
        public static void SiteAyarCreate(this List<string> list) {
            list.CreateTable("AyarGuid");
            list.CreateColumns();
            list.AddData();
        }
        static void CreateTable(this List<string> list, string idAdi = _idAdi) {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Modul", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Grup", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kontrol", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Deger1", TableCreate.VarCharMax(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Deger2", TableCreate.VarCharMax(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Deger3", TableCreate.VarCharMax(), ""));
        }
        static void AddData(this List<string> list) {

            string sqlBase = $@" 
    IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Modul = '{0}' and Grup='{1}' and Kodu='{2}')  
    begin INSERT  " + _tabloAdi + @"  
    ( Modul ,Grup, Kodu , Kontrol,Deger1,Deger2,Deger3 )  VALUES
    (N'{0}',N'{1}',N'{2}', {3} ,N'{4}',N'{5}',N'{6}' ) end ; ";

            string grup = "TabletIstasyon";

            List<AyarAdModel> lisAyar = new List<AyarAdModel>();
            lisAyar.Add(new AyarAdModel() { Modul = "Site", Kodu = "SiteAdi", Kontrol = "1", Deger1 = "Deneme Site", Deger2 = "", Deger3 = "", Grup = grup });
            lisAyar.Add(new AyarAdModel() { Modul = "Site", Kodu = "ApiAdres", Kontrol = "1", Deger1 = "", Deger2 = "", Deger3 = "", Grup = grup });


            lisAyar.Add(new AyarAdModel() { Modul = "Seri", Kodu = "FireFisi", Kontrol = "0", Deger1 = "FR", Deger2 = "", Deger3 = "", Grup = "" });
            lisAyar.Add(new AyarAdModel() { Modul = "Seri", Kodu = "SarfCikisFisi", Kontrol = "0", Deger1 = "SRF", Deger2 = "", Deger3 = "", Grup = "" });


            /***/
            lisAyar.Add(new AyarAdModel() { Modul = "FireFisi", Grup = "", Kodu = "Kullanilsin", Kontrol = "1", Deger1 = "", Deger2 = "", Deger3 = "" });
            lisAyar.Add(new AyarAdModel() { Modul = "FireFisi", Grup = "", Kodu = "MiktarEksiyeDusebilsin", Kontrol = "1", Deger1 = "", Deger2 = "", Deger3 = "" });
            lisAyar.Add(new AyarAdModel() { Modul = "FireFisi", Grup = "", Kodu = "ProjeKoduKullanilsin", Kontrol = "1", Deger1 = "", Deger2 = "", Deger3 = "" });
            lisAyar.Add(new AyarAdModel() { Modul = "FireFisi", Grup = "", Kodu = "SorMerKullanilsin", Kontrol = "1", Deger1 = "", Deger2 = "", Deger3 = "" });
            lisAyar.Add(new AyarAdModel() { Modul = "FireFisi", Grup = "", Kodu = "DepoKullanilsin", Kontrol = "1", Deger1 = "001", Deger2 = "", Deger3 = "" });
            /***/
            lisAyar.Add(new AyarAdModel() { Modul = "SarfCikisFisi", Grup = "", Kodu = "Kullanilsin", Kontrol = "1", Deger1 = "", Deger2 = "", Deger3 = "" });
            lisAyar.Add(new AyarAdModel() { Modul = "SarfCikisFisi", Grup = "", Kodu = "MiktarEksiyeDusebilsin", Kontrol = "1", Deger1 = "", Deger2 = "", Deger3 = "" });
            lisAyar.Add(new AyarAdModel() { Modul = "SarfCikisFisi", Grup = "", Kodu = "ProjeKoduKullanilsin", Kontrol = "1", Deger1 = "", Deger2 = "", Deger3 = "" });
            lisAyar.Add(new AyarAdModel() { Modul = "SarfCikisFisi", Grup = "", Kodu = "SorMerKullanilsin", Kontrol = "1", Deger1 = "", Deger2 = "", Deger3 = "" });
            lisAyar.Add(new AyarAdModel() { Modul = "SarfCikisFisi", Grup = "", Kodu = "DepoKullanilsin", Kontrol = "1", Deger1 = "001", Deger2 = "", Deger3 = "" });
            /***/
            foreach (var i in lisAyar) {
                string sql = string.Format(sqlBase, i.Modul, i.Grup, i.Kodu, i.Kontrol, i.Deger1, i.Deger2, i.Deger3);
                list.Add(sql);
            }

        }

        class AyarAdModel {
            public string Modul = "";
            public string Grup = "";
            public string Kodu = "";
            public string Kontrol = "";
            public string Deger1 = "";
            public string Deger2 = "";
            public string Deger3 = "";
        }


    }
}


