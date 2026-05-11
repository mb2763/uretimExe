using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule {
    public static class MesajlarCreates {

        private const string _tabloAdi = "Mesajlar";
        public static void MesajlarCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
            list.AddData();
        }

        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Modul", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Personel", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Mesaj", TableCreate.VarChar(4000), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "BitisTarihi", TableCreate.DateTime(), ""));
        }
        static void AddData(this List<string> list) {
            string sql = $@" 
     IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = 'A70C1056-C416-11EC-B9C8-00155D67607E') 
     begin INSERT [dbo].[" + _tabloAdi + @"] (Id, Modul , Kodu ,Personel ,Mesaj,Tarihi,BitisTarihi) VALUES ('A70C1056-C416-11EC-B9C8-00155D67607E', N'Genel', N'Acil','', N'Acil Mesaj', null,null) end ;    
                                                                                                                           
     
";
            // string sqlMaster = $@"     SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] ON  " + sql + @" SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] OFF";
            string sqlMaster = $@"    " + sql + @"  ";
            list.Add(sqlMaster);
        }
    }
}
