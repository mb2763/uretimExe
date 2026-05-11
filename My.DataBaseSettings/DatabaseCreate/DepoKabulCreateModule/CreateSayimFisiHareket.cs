using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.DatabaseSettings.DatabaseCreate.DepoKabulCreateModule {
     public static class CreateSayimFisiHareket {
        private const string _tabloAdi = "SayimFisiHareket";
        public static void SayimFisiHareketCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
            //list.AddData();
            }
        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
            }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "FisId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "GirisCikis", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tarih", TableCreate.DateTime(), ""));
            
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Miktar", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Fiyat", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tutar", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "GirisMiktar", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CikisMiktar", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Birimi", TableCreate.String(20), "0"));

            list.Add(TableCreate.SqlSutun(_tabloAdi, "PartiNo", TableCreate.String(30), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "LotNo", TableCreate.String(30), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SatirNo", TableCreate.Int(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Sil", TableCreate.Int(), "0"));
             
            }
        static void AddData(this List<string> list) {
            string sql = @" 
IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = 1) 
begin INSERT[dbo].[" + _tabloAdi + @"] ([Id] ) VALUES(1 ) end;
";
            string sqlMaster = $@"     SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] ON  " + sql + @" SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] OFF";
            list.Add(sqlMaster);
            }
        }
    }
