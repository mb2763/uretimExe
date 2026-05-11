using Dapper;
using My.DatabaseSettings.Base;
using My.DatabaseSettings.DatabaseCreate.DepoKabulCreateModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace My.DatabaseSettings.DatabaseCreate {
    public class DepoKabulCreate {
        private IDbConnection _conn;
        private List<string> _sqlList = new List<string>();
        public DepoKabulCreate(IDbConnection conn) {
            _conn = conn;
        }
        void TabloOlustur() {
            _sqlList.DepoKabulIrsaliyeCreate();
            _sqlList.DepoKabulIrsaliyeHareketCreate(); 
            _sqlList.DepoStokCreate(); 
            _sqlList.DepoStokBarkodCreate(); 
            _sqlList.DepoStokFisCreate(); 
            _sqlList.DepoStokHareketCreate();  
            _sqlList.DepoKabulIrsaliyeDetayCreate();  
            _sqlList.DepoKontrolAciklamaCreate();  
            _sqlList.DepoCreate();  
            _sqlList.DepoKontrolIadeSevkCreate();  
            _sqlList.SayimFisiCreate();  
            _sqlList.SayimFisiHareketCreate();  
             
            /* ********** */
            _sqlList.Procedure_Create();
            _sqlList.Index_Create();
        }
        public MyResult<int> DatabaseKontrol(string databaseName) {
            try {
                string sql = $@" 
                DECLARE @Durum int Set @Durum = 0;
                IF EXISTS(SELECT name FROM master.sys.databases WHERE name = N'{databaseName}')
                begin Set @Durum = 1; end select @Durum as Durum ";

                var rs = _conn.Query<int>(sql).FirstOrDefault();
                return new MyResult<int>() { Success = true, Message = "", Data = rs };
            }
            catch (Exception e) {
                return new MyResult<int>() { Success = false, Message = e.Message };
            }
        }
        public MyResult<string> DatabaseOlustur(string databaseName) {
            try {
                var s = new SqlConnectionStringBuilder(_conn.ConnectionString);
                s.InitialCatalog = "master";
                var createsql = @" CREATE DATABASE  " + databaseName + " ";
                using (SqlConnection myConn = new SqlConnection(s.ConnectionString)) {
                    SqlCommand myCommand = new SqlCommand(createsql, myConn);
                    try {
                        myConn.Open();
                        myCommand.ExecuteNonQuery();
                    }
                    catch (System.Exception e) {
                        return new MyResult<string>() { Success = false, Message = e.Message };
                    }
                }
                return DatabaseGuncelle();
            }
            catch (Exception e) {
                return new MyResult<string>() { Success = false, Message = e.Message };
            }
        }
        public MyResult<string> DatabaseGuncelle() {
            TabloOlustur();
            if (_conn.State != ConnectionState.Open) {
                _conn.Open();
            }
            IDbTransaction trs = _conn.BeginTransaction();
            string sonKod = "";

            try {
                foreach (var itm in _sqlList) {
                    sonKod = itm;
                    _conn.Execute(itm, transaction: trs);
                }
                trs.Commit();
                return new MyResult<string>() { Success = true, Message = "", Data = "true" };
            }
            catch (Exception e) {

                trs.Dispose();
                return new MyResult<string>() { Success = false, Message = e.Message, Data = sonKod };
            }
        }
    }
}
