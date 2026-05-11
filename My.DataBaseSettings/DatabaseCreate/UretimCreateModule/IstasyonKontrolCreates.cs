using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule {
    public static class IstasyonKontrolCreates {
        private const string _tabloAdi = "IstasyonKontrol";
        private const string _idAdi = "Id";
        public static void IstasyonKontrolCreate(this List<string> list) {
            list.CreateTable(_idAdi);
            list.CreateColumns();
            list.AddData();
        }
        static void CreateTable(this List<string> list, string idAdi = _idAdi) {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {

            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstasyonKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tarih", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReceteKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReceteAdi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Miktar", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitEden", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.VarCharMax(), ""));

        }
        static void AddData(this List<string> list) {

        }
    }

}
