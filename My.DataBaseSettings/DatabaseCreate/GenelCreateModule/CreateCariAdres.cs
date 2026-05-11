using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule {
    public static class CreateCariAdres {
        private static string _tabloAdi = "CariAdres";
        private static string _idAdi = "CariAdresId";
        public static void CariAdresCreate(this List<string> list) {
            CreateTable(list);
            CreateColumns(list);
            AddData(list);
        }

        static void CreateTable(List<string> list) {
            var sql = TableCreate.SqlTablo(_tabloAdi, _idAdi);
            list.Add(sql);
        }
        static void CreateColumns(List<string> list) {

            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariId", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kodu", TableCreate.VarChar(25), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Adres1", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Adres2", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Adres3", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Il", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Ilce", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Sokak", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Mahalle", TableCreate.VarChar(50), ""));
        }
        static void AddData(List<string> list) {

        }

    }

}
