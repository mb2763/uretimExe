using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.DatabaseSettings.DatabaseCreate.TempCreateModule {
 
    public static class TempMikroStokCreates {
        private const string _tabloAdi = "TempMikroStok";
        public static void TempMikroStokCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
        }

        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi );
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokAdi", TableCreate.VarChar(255), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CinsKodu", TableCreate.SmallInt(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Cinsi", TableCreate.VarChar(30), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KategoriKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KategoriAdi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KaliteKontrolKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KaliteKontrolAdi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReyonKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReyonAdi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Birim1", TableCreate.VarChar(10), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Birim2", TableCreate.VarChar(10), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Birim3", TableCreate.VarChar(10), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Birim4", TableCreate.VarChar(10), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Katsayi1", TableCreate.Double(), "1"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Katsayi2", TableCreate.Double(), "1"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Katsayi3", TableCreate.Double(), "1"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Katsayi4", TableCreate.Double(), "1"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CreateDate", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EditDate", TableCreate.DateTime(), ""));  
            list.Add(TableCreate.SqlSutun(_tabloAdi, "TakipTip", TableCreate.Int(), ""));  
            list.Add(TableCreate.SqlSutun(_tabloAdi, "TakipTipAd", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RbTakipTip", TableCreate.Int(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RbTakipTipAd", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReyonKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReyonAdi", TableCreate.VarChar(50), ""));
          

        }

    }
}
