using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.DatabaseSettings.DatabaseCreate.TempCreateModule {
   
    public static class TempSonGuncellemeCreates {
        private const string _tabloAdi = "TempSonGuncelleme";
        public static void TempSonGuncellemeCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();

        }

        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Modul", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tarih", TableCreate.DateTime(), ""));

        }

    }
}
