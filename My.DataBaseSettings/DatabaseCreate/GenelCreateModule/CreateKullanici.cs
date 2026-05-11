using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.DatabaseSettings.Base;

namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule {
    public static class CreateKullanici
    {
        private const string _tabloAdi = "Kullanici";
        public static void KullaniciCreate(this List<string> list)
        {
            list.CreateTable("Id");
            list.CreateColumns();
        }
         
        static void CreateTable(this List<string> list, string idAdi = "Id")
        {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list)
        {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Adi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Soyadi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KullaniciAdi", TableCreate.VarChar(100), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Sifre", TableCreate.VarChar(500), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Admin", TableCreate.SmallInt(), "0"));

        }

    }
}
