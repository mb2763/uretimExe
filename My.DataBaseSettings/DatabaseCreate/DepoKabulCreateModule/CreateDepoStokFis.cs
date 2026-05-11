using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.DatabaseSettings.Base;

namespace My.DatabaseSettings.DatabaseCreate.DepoKabulCreateModule {
   public static class CreateDepoStokFis {
        private const string _tabloAdi = "DepoStokFis";
        public static void DepoStokFisCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
            //list.AddData();
        }
        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DkId", TableCreate.UniqueIdentifier(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "FisTuru", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "FisSeri", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "FisNo", TableCreate.Int(), "0")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tarih", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Personel", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitEden", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Degistiren", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DegistirmeTarihi", TableCreate.DateTime(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsEvrakSeri", TableCreate.VarChar(30), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsEvrakSira", TableCreate.Int(), "0")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsGuid", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsBelgeNo", TableCreate.VarChar(50), ""));
        }
        static void AddData(this List<string> list) {
            //string sql = @" 
            //IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = 1) 
            //begin INSERT[dbo].[" + _tabloAdi + @"] ([Id] ) VALUES(1 ) end;
            //";
            //string sqlMaster = $@"     SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] ON  " + sql + @" SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] OFF";
            //list.Add(sqlMaster);
        }
    }
}
