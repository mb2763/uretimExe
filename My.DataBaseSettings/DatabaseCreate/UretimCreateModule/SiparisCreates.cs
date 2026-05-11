using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule {
    public static class SiparisCreates {
        private const string _tabloAdi = "Siparis";
        public static void SiparisCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
        }
        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Turu", TableCreate.VarChar(35), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SiparisKodu", TableCreate.VarChar(35), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariUnvani", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tarih", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "TeslimTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Miktar", TableCreate.Float(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.VarChar(500), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Notu", TableCreate.VarChar(500), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kapandi", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Durumu", TableCreate.VarChar(100), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kargo", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Email", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Ent", TableCreate.SmallInt(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntSeri", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntSira", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntCode", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntDate", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntKayitSeri", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntKayitSira", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntKayitGuid", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntIptal", TableCreate.SmallInt(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntIptalSeri", TableCreate.VarChar(50), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntIptalSira", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EtiketBasildi", TableCreate.SmallInt(), "0")); 
        }
    }
}