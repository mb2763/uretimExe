using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule {
    public static class IstasyonBakimParcaCreates {
        private const string _tabloAdi = "IstasyonBakimParca";
        public static void IstasyonBakimParcaCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();

        }

        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstBakId", TableCreate.Guid(), ""));  
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Parca", TableCreate.VarChar(250), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ParcaNo", TableCreate.VarChar(50), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EvrakNo", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.VarCharMax(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Garanti", TableCreate.SmallInt(), ""));

        }
    }
}
