using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.DepoKabulCreateModule
{
    public static class CreateDepoStokHareket
    {

        private const string _tabloAdi = "DepoStokHareket";
        public static void DepoStokHareketCreate(this List<string> list)
        {
            list.CreateTable("Id");
            list.CreateColumns();
            //list.AddData();
        }
        static void CreateTable(this List<string> list, string idAdi = "Id")
        {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list)
        {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "FisId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "GirisCikis", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tarih", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SatirNo", TableCreate.Int(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "GirisMiktar", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CikisMiktar", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Birimi", TableCreate.String(20), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsBirimPntr", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "PartiNo", TableCreate.String(30), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "LotNo", TableCreate.String(30), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UretimTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SonKulTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Sira", TableCreate.Int(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "BarGui", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsHGuid", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StGuid", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Sil", TableCreate.Int(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "TakipKodu1", TableCreate.VarChar(50), "''"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "TakipKodu2", TableCreate.VarChar(50), "''"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "TakipKodu3", TableCreate.VarChar(50), "''"));

        }
        static void AddData(this List<string> list)
        {
            //string sql = @" 
            //IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = 1) 
            //begin INSERT[dbo].[" + _tabloAdi + @"] ([Id] ) VALUES(1 ) end;
            //";
            //string sqlMaster = $@"     SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] ON  " + sql + @" SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] OFF";
            //list.Add(sqlMaster);
        }
    }
}
