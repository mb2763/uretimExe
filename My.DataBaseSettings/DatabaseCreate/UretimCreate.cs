using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper; 
using My.DatabaseSettings.Base;
using My.DatabaseSettings.DatabaseCreate.TempCreateModule;
using My.DatabaseSettings.DatabaseCreate.UretimCreateModule; 

namespace My.DatabaseSettings.DatabaseCreate {
    public class UretimCreate {
        private IDbConnection _conn;
        private List<string> _sqlList = new List<string>();
        public UretimCreate(IDbConnection conn)
        {
            _conn = conn;
        }
        void TabloOlustur()
        { 
            _sqlList.ReceteAnaCreate();
            _sqlList.ReceteDetayCreate();
            _sqlList.ReceteGrupCreate();
            _sqlList.ReceteGrupDetayCreate();
            _sqlList.ReceteStokCreate();
            _sqlList.SiparisCreate();
            _sqlList.SiparisHareketCreate();
            _sqlList.SiparisHareketDetayCreate();
            _sqlList.OperasyonKartiCreate();
            _sqlList.IstasyonKartiCreate();
            _sqlList.ReceteOperasyonCreate();
            _sqlList.ReceteIstasyonCreate();
            _sqlList.ReceteIstasyonCariCreate(); 
            _sqlList.UretimEmriCreate();
            _sqlList.UretimStokCreate();
            _sqlList.UretimStokFisCreate();
            _sqlList.UretimStokFisHareketCreate();
            _sqlList.UretimOperasyonCreate();
            _sqlList.UretimOperasyonHareketCreate();
            _sqlList.UretimOperasyonHareketDetayCreate();
            _sqlList.UretimIstasyonCreate();
            _sqlList.UretimIstasyonHareketCreate(); 
            /**/ 
            _sqlList.UretimTalepCreate();
            _sqlList.UretimTalepHareketCreate();
            _sqlList.ReceteyeBagliIstasyonCreate();
            _sqlList.IstasyonAciklamaCreate();
            _sqlList.AciklamaKodCreate();
            _sqlList.AciklamaDegerCreate();
            _sqlList.ReceteStokRenkBedenCreate(); 
            _sqlList.IstasyonTakipHareketCreate();
            _sqlList.IstasyonTakipHareketLogCreate();
            _sqlList.IstasyonTakipStokHareketCreate();
            _sqlList.IstasyonTakipHareketDetayCreate();
            _sqlList.IstasyonKontrolCreate();
            _sqlList.IstasyonBakimCreate();
            _sqlList.IstasyonBakimParcaCreate();
            _sqlList.SmsAyarCreate();
            _sqlList.ReceteIstasyonGrupKodCreate();
            _sqlList.ReceteIstasyonGrupOperasyonCreate();
            _sqlList.ReceteIstasyonGrupIstasyonCreate();
            _sqlList.UretimKontrolCreate();
            _sqlList.IstasyonTakipStokHareketDetayCreate();
           

            /* ********** */
            _sqlList.TempSiparisUretimMiktarCreate();
            _sqlList.TempMikroStokCreate();
            _sqlList.TempMikroStokKategoriCreate();
            _sqlList.TempSonGuncellemeCreate();
            
            /* ********** */
            //_sqlList.IstasyonHareketCreate();
            //_sqlList.OperasyonHareketCreate();
            /* ********** */
            _sqlList.Procedure_Create();
            _sqlList.Index_Create();
        }
        public MyResult<int> DatabaseKontrol(string databaseName)
        {
            try
            {
                string sql = $@"DECLARE @Durum int Set @Durum = 0;
                IF EXISTS(SELECT name FROM master.sys.databases WHERE name = N'{databaseName}')
                begin Set @Durum = 1; end select @Durum as Durum";
                var rs = _conn.Query<int>(sql).FirstOrDefault();
                return new MyResult<int>() { Success = true, Message = "", Data = rs };
            }
            catch (Exception e)
            {
                return new MyResult<int>() { Success = false, Message = e.Message };
            }
        }
        public MyResult<string> DatabaseOlustur(string databaseName)
        {
            try
            {
                var s = new SqlConnectionStringBuilder(_conn.ConnectionString);
                s.InitialCatalog = "master";
                var createsql = @" CREATE DATABASE  " + databaseName + " ";
                using (SqlConnection myConn = new SqlConnection(s.ConnectionString))
                {
                    SqlCommand myCommand = new SqlCommand(createsql, myConn);
                    try
                    {
                        myConn.Open();
                        myCommand.ExecuteNonQuery();
                    }
                    catch (System.Exception e)
                    {
                        return new MyResult<string>() { Success = false, Message = e.Message };
                    }
                }
                return DatabaseGuncelle();
            }
            catch (Exception e)
            {
                return new MyResult<string>() { Success = false, Message = e.Message };
            }
        }
        public MyResult<string> DatabaseGuncelle()
        {
            TabloOlustur();
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }
            IDbTransaction trs = _conn.BeginTransaction();
            string sonKod = "";

            try
            {
                foreach (var itm in _sqlList)
                {
                    sonKod = itm;
                    _conn.Execute(itm, transaction: trs);
                }
                trs.Commit();
                return new MyResult<string>() { Success = true, Message = "", Data = "true" };
            }
            catch (Exception e)
            {
                
                trs.Dispose();
                return new MyResult<string>() { Success = false, Message = e.Message ,Data = sonKod };
            }
        }
    }
}