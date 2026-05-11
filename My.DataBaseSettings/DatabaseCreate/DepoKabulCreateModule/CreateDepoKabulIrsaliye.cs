using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.DepoKabulCreateModule {
    public static class CreateDepoKabulIrsaliye {

        private const string _tabloAdi = "DepoKabulIrsaliye";
        public static void DepoKabulIrsaliyeCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
            //list.AddData();
        }

        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EvrakSeri", TableCreate.VarChar(30), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EvrakSira", TableCreate.Int(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsTarih", TableCreate.DateTime(),""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KontrolTarih", TableCreate.DateTime(),"")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariCinsi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KontrolAciklama", TableCreate.VarChar(255), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Depo", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EvrakAdi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Cinsi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tipi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "NormalIade", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SatirSayisi", TableCreate.Int(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Personel", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitEden", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Degistiren", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DegistirmeTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Yerlestirildi", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DepoNoGiris", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DepoNoCikis", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsGuid", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Yazdirildi", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsBelgeNo", TableCreate.VarChar(50), ""));
        }
        static void AddData(this List<string> list) {

            //string sql = @" 
            //IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = 1) 
            //begin INSERT[dbo].[" + _tabloAdi + @"] ([Id] ) VALUES(1 ) end;
            //";

            //list.Add(sql);
        }
    }
}
