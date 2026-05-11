using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule {
    public static class UretimTalepHareketCreates {
        private const string _tabloAdi = "UretimTalepHareket";
        private const string _idAdi = "UrtTlpHrId";
        public static void UretimTalepHareketCreate(this List<string> list) {
            list.CreateTable( );
            list.CreateColumns();
        }
        static void CreateTable(this List<string> list ) {
            var sql = TableCreate.SqlTablo(_tabloAdi, _idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrtTlpId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tarih", TableCreate.DateTime(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EvrakNo", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstasyonKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstasyonAdi", TableCreate.VarChar(150), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kullanici", TableCreate.String(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Miktar", TableCreate.Double(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Birimi", TableCreate.VarChar(35), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.String(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Parti", TableCreate.String(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Lot", TableCreate.Int(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitEden", TableCreate.String(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Ent", TableCreate.SmallInt(), "0")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntKodu2", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntTarih", TableCreate.DateTime(), ""));
             
             

        }
    }
}
