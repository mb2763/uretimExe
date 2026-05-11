using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule {
    public static class PersonelAyarCreates {
        private const string _tabloAdi = "PersonelAyar";
        private const string _idAdi = "Id";
        public static void PersonelAyarCreate(this List<string> list) {
            list.CreateTable(_idAdi);
            list.CreateColumns();
            list.AddData();
        }
        static void CreateTable(this List<string> list, string idAdi = _idAdi) {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "PerId", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "PerAyarAdId", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Modul", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Grup", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kontrol", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Deger1", TableCreate.VarCharMax(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Deger2", TableCreate.VarCharMax(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Deger3", TableCreate.VarCharMax(), ""));

        }
        static void AddData(this List<string> list) {

        }


    }
}
