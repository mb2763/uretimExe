using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule
{
    public static class CreateDepo
    {
        private const string _tabloAdi = "Depo";
        public static void DepoCreate(this List<string> list)
        {
            list.CreateTable("Id");
            list.CreateColumns();
             list.AddData();
        }

        static void CreateTable(this List<string> list, string idAdi = "Id")
        {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list)
        {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DepoKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DepoAdi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "MikroDepoNo", TableCreate.Int(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kullanicilar", TableCreate.VarCharMax(), "''")); 
        }
        static void AddData(this List<string> list)
        {
            Guid id1 = Guid.Parse("936FF5FD-1333-11ED-8F7E-E4AAEA428528");
            Guid id2 = Guid.Parse("936FF5FE-1333-11ED-8F7E-E4AAEA428528"); 
            string sql = @" 
              IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id1 + @"') 
              begin INSERT [dbo].[" + _tabloAdi + @"] (Id,DepoKodu,DepoAdi,MikroDepoNo ) VALUES ('" + id1 + @"','01','Merkez Depo',1 ) end ; 

            ";

            list.Add(sql);
        }
    }
}
