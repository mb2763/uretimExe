using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule {
    public static class PersonelAyarAdCreates {
        private const string _tabloAdi = "PersonelAyarAd";
        private const string _idAdi = "Id";
        public static void PersonelAyarAdCreate(this List<string> list) {
            list.CreateTable(_idAdi);
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
    ( Modul ,Grup,   Kodu ,Kontrol,Deger1,Deger2,Deger3 )  VALUES
    (N'{0}', N'{1}',N'{2}',{3},N'{4}',N'{5}',N'{6}' ) end ; ";


            List<AyarAdModel> lisAyar = new List<AyarAdModel>();

            //lisAyar.Add(new  AyarAdModel() { Modul = "Seri", Kodu = "PerakendeSiparis", Kontrol = "0", Deger1 = "PER", Deger2 = "", Deger3 = "", Grup = "" });
            //lisAyar.Add(new  AyarAdModel() { Modul = "Seri", Kodu = "ToptanSiparis", Kontrol = "0", Deger1 = "TOP", Deger2 = "", Deger3 = "", Grup = "" });
            //lisAyar.Add(new  AyarAdModel() { Modul = "Seri", Kodu = "Teklif", Kontrol = "0", Deger1 = "TEK", Deger2 = "", Deger3 = "", Grup = "" });
            //lisAyar.Add(new  AyarAdModel() { Modul = "Seri", Kodu = "YeniCariKodu", Kontrol = "0", Deger1 = "120.91.", Deger2 = "1", Deger3 = "", Grup = "" });




            //       (Modul, Grup,  Kodu,  Kontrol, Deger1, Deger2, Deger3) VALUES
            //      (N'{0}', N'{1}',N'{2}' ,N'{3}', N'{4}', N'{5}',  N'{6}' ) end; ";
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
