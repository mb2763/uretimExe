using System.Collections.Generic;
using My.DatabaseSettings.Base;
namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule
{
    public static class CreateAyarSayac
    {
        private const string _tabloAdi = "AyarSayac";
        public static void AyarSayacCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
            list.AddData();
        }
        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "BasamakSayisi", TableCreate.Int(), "7"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "BasinaEkle", TableCreate.VarChar(10), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Verilecek", TableCreate.Int(), "0"));
        } 
        static void AddData(this List<string> list) {
            string sql = $@" 
     IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id ='395743A7-C417-11EC-B9C8-00155D67607E') 
     begin INSERT [dbo].[" + _tabloAdi + @"] ([Id], [Kodu], [Aciklama], [BasamakSayisi], [BasinaEkle], [Verilecek]) VALUES ('395743A7-C417-11EC-B9C8-00155D67607E', N'Siparis', N'Siparişler', 7, N'SIP', 20) end ;    
     IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '395743A8-C417-11EC-B9C8-00155D67607E') 
     begin INSERT [dbo].[" + _tabloAdi + @"] ([Id], [Kodu], [Aciklama], [BasamakSayisi], [BasinaEkle], [Verilecek]) VALUES ('395743A8-C417-11EC-B9C8-00155D67607E', N'Recete', N'Reçeteler', 7, N'RC', 20) end ;
     IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '395743A9-C417-11EC-B9C8-00155D67607E') 
     begin INSERT [dbo].[" + _tabloAdi + @"] ([Id], [Kodu], [Aciklama], [BasamakSayisi], [BasinaEkle], [Verilecek]) VALUES ('395743A9-C417-11EC-B9C8-00155D67607E', N'UretimEmri', N'Üretim İş Emri', 7, N'URE', 20) end ;
     IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '395743AA-C417-11EC-B9C8-00155D67607E') 
     begin INSERT [dbo].[" + _tabloAdi + @"] ([Id], [Kodu], [Aciklama], [BasamakSayisi], [BasinaEkle], [Verilecek]) VALUES ('395743AA-C417-11EC-B9C8-00155D67607E', N'Uretim', N'Reçeteden Üretim', 7, N'UR', 20) end ;
     IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '395743AB-C417-11EC-B9C8-00155D67607E') 
     begin INSERT [dbo].[" + _tabloAdi + @"] ([Id], [Kodu], [Aciklama], [BasamakSayisi], [BasinaEkle], [Verilecek]) VALUES ('395743AB-C417-11EC-B9C8-00155D67607E', N'Personel', N'Personel', 7, N'PER', 20) end ;
     IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '08DB28AB-63E2-ABC5-1291-F8524402A042') 
     begin INSERT [dbo].[" + _tabloAdi + @"] ([Id], [Kodu], [Aciklama], [BasamakSayisi], [BasinaEkle], [Verilecek]) VALUES ('08DB28AB-63E2-ABC5-1291-F8524402A042', N'UretimTalep', N'UretimTalep', 7, N'', 20) end ;
     ";
            // string sqlMaster = $@"     SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] ON  " + sql + @" SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] OFF";
            string sqlMaster = $@"      " + sql + @"  ";
            list.Add(sqlMaster);

        }
    }
}