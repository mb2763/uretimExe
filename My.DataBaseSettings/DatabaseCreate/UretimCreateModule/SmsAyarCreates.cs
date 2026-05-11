using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule {
    public static class SmsAyarCreates {
        private const string _tabloAdi = "SmsAyar";
        public static void SmsAyarCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();

        }

        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KullaniciKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Sifre", TableCreate.VarChar(250), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Baslik", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "GunderimUrl", TableCreate.VarChar(250), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RaporUrl", TableCreate.VarChar(250), "")); 

        }
    }
}
