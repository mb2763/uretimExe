using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule {
    public static class CreateCari {
        private static string _tabloAdi = "Cari";
        private static string _idAdi = "CariId";
        public static void CariCreate(this List<string> list) {
            CreateTable(list);
            CreateColumns(list);
            AddData(list);
        }
        static void CreateTable(List<string> list) {
            var sql = TableCreate.SqlTablo(_tabloAdi, _idAdi);
            list.Add(sql);
        }
        static void CreateColumns(List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariKodu", TableCreate.VarChar(25), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariUnvani", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "AdiSoyadi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tel1", TableCreate.VarChar(25), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tel2", TableCreate.VarChar(25), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Email", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Web", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "VergiDaire", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "VergiNo", TableCreate.VarChar(25), ""));
        }
        static void AddData(List<string> list) {

        }

    }

}
