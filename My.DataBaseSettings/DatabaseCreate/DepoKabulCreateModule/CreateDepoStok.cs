using My.DatabaseSettings.Base;
using System.Collections.Generic;
namespace My.DatabaseSettings.DatabaseCreate.DepoKabulCreateModule
{
    public static class CreateDepoStok
    {
        private const string _tabloAdi = "DepoStok";
        public static void DepoStokCreate(this List<string> list)
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
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Birimi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Depo", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Bolge", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Raf", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kat", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "AnaGrup", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "AltGrup", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokCins", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StGuid", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Barkodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KasaKoliTipi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KasaAdedi", TableCreate.Double(), "0"));
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