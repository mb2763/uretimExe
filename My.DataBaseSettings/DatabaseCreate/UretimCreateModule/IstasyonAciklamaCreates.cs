using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule {
    public static class IstasyonAciklamaCreates {
        private const string _tabloAdi = "IstasyonAciklama";
        public static void IstasyonAciklamaCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();

        }

        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Modul", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Deger", TableCreate.VarChar(50), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SmsGonder", TableCreate.SmallInt(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SmsKodu", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Gorevi", TableCreate.VarChar(50), ""));
           
        }
    }
}
