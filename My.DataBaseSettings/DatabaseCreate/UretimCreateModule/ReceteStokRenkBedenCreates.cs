using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule {
    public static class ReceteStokRenkBedenCreates {
        private const string _tabloAdi = "ReceteStokRenkBeden";
        public static void ReceteStokRenkBedenCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();

        }

        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list) {

            list.Add(TableCreate.SqlSutun(_tabloAdi, "Turu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Miktar", TableCreate.Float(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcAId", TableCreate.UniqueIdentifier(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcDId", TableCreate.UniqueIdentifier(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcSTId", TableCreate.UniqueIdentifier(), "")); 
        }
    }
}
