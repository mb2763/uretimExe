using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.DatabaseSettings.Base;

namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule {
   public static  class CreateKullaniciIzinler {

        private const string _tabloAdi = "KullaniciIzinler";
        public static void KullaniciIzinlerCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
            //list.AddData();
        }
        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Durum", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KulId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "YetId", TableCreate.UniqueIdentifier(), ""));

        }
        static void AddData(this List<string> list) {
            string sql = $@" 
      IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '188E361D-5985-4BF4-9A6D-4F7888A8BC29') 
      begin INSERT [dbo].[" + _tabloAdi + @"] (Id) VALUES ('188E361D-5985-4BF4-9A6D-4F7888A8BC29' ) end ;  
     ";
           // string sqlMaster = $@"     SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] ON  " + sql + @" SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] OFF";
            string sqlMaster = $@"     " + sql + @"  ";
            list.Add(sqlMaster);
        }
    }
}
