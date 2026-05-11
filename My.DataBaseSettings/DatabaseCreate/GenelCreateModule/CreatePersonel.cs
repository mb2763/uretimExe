using My.DatabaseSettings.Base;
using System.Collections.Generic;
namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule { 
    public static class CreatePersonel {
        private const string _tabloAdi = "Personel";
        public static void PersonelCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns(); 
        }
        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Adi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Soyadi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Grup", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Gorevi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Sifre", TableCreate.VarChar(2000), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstasyonKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Yetkili", TableCreate.SmallInt(), "0"));  
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Admin", TableCreate.SmallInt(), "0"));  
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstasyonPersoneli", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CepTel", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SmsGonder", TableCreate.SmallInt(), "0"));
        } 
    }
}
