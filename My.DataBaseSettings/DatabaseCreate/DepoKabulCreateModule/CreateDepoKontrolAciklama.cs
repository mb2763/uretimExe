using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.DepoKabulCreateModule
{

    // DepoKontrolAciklama
    public static class CreateDepoKontrolAciklama
    {
        private const string _tabloAdi = "DepoKontrolAciklama";
        public static void DepoKontrolAciklamaCreate(this List<string> list)
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
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kodu", TableCreate.VarChar(255), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DepoKodu", TableCreate.VarChar(50), ""));
            
        }
        static void AddData(this List<string> list)
        { 
            Guid id1 = Guid.Parse("936FF5FD-1333-11ED-8F7E-E4AAEA428528");
            Guid id2 = Guid.Parse("936FF5FE-1333-11ED-8F7E-E4AAEA428528");
            Guid id3 = Guid.Parse("936FF5FF-1333-11ED-8F7E-E4AAEA428528");
            Guid id4 = Guid.Parse("936FF600-1333-11ED-8F7E-E4AAEA428528");
            Guid id5 = Guid.Parse("936FF601-1333-11ED-8F7E-E4AAEA428528");
            Guid id6 = Guid.Parse("936FF602-1333-11ED-8F7E-E4AAEA428528");
            Guid id7 = Guid.Parse("936FF603-1333-11ED-8F7E-E4AAEA428528");
            Guid id8 = Guid.Parse("936FF604-1333-11ED-8F7E-E4AAEA428528");
            Guid id9 = Guid.Parse("936FF605-1333-11ED-8F7E-E4AAEA428528");
            Guid id10 = Guid.Parse("936FF606-1333-11ED-8F7E-E4AAEA428528");
            Guid id11= Guid.Parse("936FF607-1333-11ED-8F7E-E4AAEA428528");
            Guid id12 = Guid.Parse("936FF608-1333-11ED-8F7E-E4AAEA428528");
            Guid id13 = Guid.Parse("936FF609-1333-11ED-8F7E-E4AAEA428528");

            // Guid id14 = Guid.Parse("936FF60A-1333-11ED-8F7E-E4AAEA428528");
            // Guid id15 = Guid.Parse("936FF60B-1333-11ED-8F7E-E4AAEA428528");
            // Guid id16 = Guid.Parse("936FF60C-1333-11ED-8F7E-E4AAEA428528");
            // Guid id17 = Guid.Parse("936FF60D-1333-11ED-8F7E-E4AAEA428528");
            // Guid id18 = Guid.Parse("936FF60E-1333-11ED-8F7E-E4AAEA428528");
            // Guid id19 = Guid.Parse("936FF60F-1333-11ED-8F7E-E4AAEA428528");
            // Guid id20 = Guid.Parse("936FF610-1333-11ED-8F7E-E4AAEA428528");

            string sql = $@" 
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id1 + @"') 
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id1 + @"','Malzeme kalınlığı tolerans üzerinde' ) end ; 

        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id2 + @"') 
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id2 + @"','Malzeme kalınlığı tolerans altında' ) end ;  
                                                                                     
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id3 + @"')   
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id3 + @"','Malzeme kesit ölçüleri tolerans üzerinde' ) end ;  
       
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id4 + @"')   
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id4 + @"','Malzeme kesit ölçüleri tolerans altında' ) end ;  
        
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id5 + @"')   
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id5 + @"','Malzeme yoğunluk tolerans üzerinde' ) end ;  
        
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id6 + @"')   
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id6 + @"','Malzeme yoğunluk tolerans altında' ) end ;  
        
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id7 + @"')   
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id7 + @"','Malzeme sertlik tolerans üzerinde' ) end ;  
        
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id8 + @"')   
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id8 + @"','Malzeme sertlik tolerans altında' ) end ;  
        
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id9 + @"')   
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id9 + @"','Plaka boyutlarının istenen ölçülerden büyük olması' ) end ;  
        
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id10 + @"')   
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id10 + @"','Plaka boyutlarının istenen ölçülerden küçük olması' ) end ;  
       
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id11 + @"')   
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id11 + @"','Malzeme renk uygunsuzluğu' ) end ;  
       
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id12 + @"')   
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id12 + @"','Malzeme görünüm bozukluğu' ) end ;  
       
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id13 + @"')   
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id13 + @"','Uygunsuz hammadde  test raporu' ) end ;   
       
        ";

            /* 
        IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id  + @"')   
        begin INSERT [dbo].[" + _tabloAdi + @"] (Id,Kodu ) VALUES ('" + id  + @"','' ) end ;  
                                                                           
        
             
             */
            list.Add(sql);
        }
    }
}
