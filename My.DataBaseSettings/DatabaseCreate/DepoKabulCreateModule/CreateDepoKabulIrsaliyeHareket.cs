using My.DatabaseSettings.Base;
using System.Collections.Generic;
namespace My.DatabaseSettings.DatabaseCreate.DepoKabulCreateModule {
    public static class CreateDepoKabulIrsaliyeHareket {
        private const string _tabloAdi = "DepoKabulIrsaliyeHareket";
        public static void DepoKabulIrsaliyeHareketCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
            //list.AddData();
        }
        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DkId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EvrakSeri", TableCreate.VarChar(30), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EvrakSira", TableCreate.Int(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SatirNo", TableCreate.Int(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsaliyeMiktar", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KontrolMiktar", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KontrolAciklama", TableCreate.VarChar(255), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Birimi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsTarih", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DepoIsmi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsBirimPntr", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DepoNoGiris", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DepoNoCikis", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "PartiNo", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "LotNo", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UretimTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SonKulTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsHGuid", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StGuid", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Sil", TableCreate.Int(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IadeDepoKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Yazdirildi", TableCreate.SmallInt(), "0"));
        }
        static void AddData(this List<string> list) {
            //string sql = @" 
            //IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = 1) 
            //begin INSERT[dbo].[" + _tabloAdi + @"] ([Id] ) VALUES(1 ) end;
            //";
            //string sqlMaster = $@"     SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] ON  " + sql + @" SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] OFF";
            //list.Add(sqlMaster);
        }
    }
}