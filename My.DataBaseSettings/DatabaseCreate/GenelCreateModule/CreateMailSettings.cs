using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.DatabaseSettings.Base;

namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule
{
    public static class CreateMailSettings
    {
        private const string _tabloAdi = "MailSettings";
        public static void MailSettingsCreate(this List<string> list)
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
            list.Add(TableCreate.SqlSutun(_tabloAdi, "MailKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Host", TableCreate.VarChar(100), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Port", TableCreate.VarChar(10), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "MailAdres", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Pass", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DisplayName", TableCreate.VarChar(100), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EnableSsl", TableCreate.SmallInt(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Konu", TableCreate.VarChar(500), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Body", TableCreate.VarChar(4000), ""));
        }
        static void AddData(this List<string> list)
        {

        }
    }
}
