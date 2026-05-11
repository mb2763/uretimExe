using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.DatabaseSettings.Base;

namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule {
  public static   class CreateKullaniciYetkiler {
        private const string _tabloAdi = "KullaniciYetkiler";
        public static void KullaniciYetkilerCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
            list.AddData();
        }
        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Modul", TableCreate.String(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Yetki", TableCreate.String(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.String(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Sec", TableCreate.SmallInt(), "0")); 
           
        }
        static void AddData(this List<string> list) {

            Guid id1 = Guid.Parse("B97E2A6A-10FB-11ED-8F7E-E4AAEA428528");
            Guid id2 = Guid.Parse("B97E2A6B-10FB-11ED-8F7E-E4AAEA428528");
            Guid id3 = Guid.Parse("B97E2A6C-10FB-11ED-8F7E-E4AAEA428528");
            Guid id4 = Guid.Parse("B97E2A6D-10FB-11ED-8F7E-E4AAEA428528");
            Guid id5 = Guid.Parse("B97E2A6E-10FB-11ED-8F7E-E4AAEA428528");
            Guid id6 = Guid.Parse("B97E2A6F-10FB-11ED-8F7E-E4AAEA428528");


            string sql = $@" 
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id1 + @"') 
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Modul,Yetki,Aciklama,Sec) VALUES ('" + id1 + @"','GENEL','YETKILI','Tüm Yetkiler Açık',0) end ;  
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id2 + @"') 
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Modul,Yetki,Aciklama,Sec) VALUES ('" + id2 + @"','DEPOKABUL','DEPOKABUL_KONTROL','',0) end ;  
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id3 + @"') 
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Modul,Yetki,Aciklama,Sec) VALUES ('" + id3 + @"','DEPOKABUL','DEPOKABUL_MALKABUL','',0) end ; 
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id4 + @"') 
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Modul,Yetki,Aciklama,Sec) VALUES ('" + id4 + @"','DEPOKABUL','DEPOKABUL_RAPORLAR','',0) end ;  
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id5 + @"') 
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Modul,Yetki,Aciklama,Sec) VALUES ('" + id5 + @"','DEPOKABUL','DEPOKABUL_KARTLAR','',0) end ;  
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id6 + @"') 
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Modul,Yetki,Aciklama,Sec) VALUES ('" + id6 + @"','DEPOKABUL','DEPOKABUL_AYARLAR','',0) end ;  
       ";

     //       string sql = $@" 
     //IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = 1) 
     //    begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Modul,Yetki,Aciklama,Sec) VALUES (1,'GENEL','YETKILI','Tüm Yetkiler Açık',0) end ;  
     //IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = 2) 
     //    begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Modul,Yetki,Aciklama,Sec) VALUES (2,'DEPOKABUL','DEPOKABUL_KONTROL','',0) end ;
     //IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = 3) 
     //    begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Modul,Yetki,Aciklama,Sec) VALUES (3,'DEPOKABUL','DEPOKABUL_MALKABUL','',0) end ;
     //";
            //string sqlMaster = $@"     SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] ON  " + sql + @" SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] OFF";
            list.Add(sql);
        }
    }
}
