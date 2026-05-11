using My.Business.Service.OperasyonKartlar;
using My.Core.Data;
using My.Core.Result;
using My.Entities.Models;
using My.Entities.OperasyonKartlar;
using System;
using System.Data;
using System.Linq;

namespace My.Business.Manager
{
    public class OperasyonManager
    {

        private readonly DatabaseFactoryPro _dbPro;
        public IOperasyonKartiService Service { get; set; }

        public OperasyonManager(DatabaseFactoryPro dbPro)
        {
            _dbPro = dbPro;
            Service = _dbPro.OperasyonKarti;
        }

        public IDataResult<string> Kaydet(OperasyonKarti mdl, bool yenikayit)
        {
            bool update = false;
            string oldCode = "";
            if (!yenikayit)
            {
                var rsFin = Service.SelectFind(mdl.Id);
                if (!rsFin.Success)
                {
                    return new ErrorDataResult<string>(rsFin.Message);
                }
                var dt1 = rsFin.Data;
                if (dt1.OperasyonKodu != mdl.OperasyonKodu)
                {
                    update = true;
                    oldCode = dt1.OperasyonKodu;
                }
            }
             
            var rsKod = KodVarmi(mdl, "OperasyonKodu", yenikayit);
            if (!rsKod.Success) return new ErrorDataResult<string>(rsKod.Message);
            var con = Service.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try
            {
                var rsAna = Service.InsertOrUpdate(mdl, trs);
                if (!rsAna.Success)
                {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rsAna.Message);
                }
                if (update)
                { 
                    /* IstasyonKarti  */
                    string sql1 = $@" UPDATE IstasyonKarti SET Operasyon= '{mdl.OperasyonKodu}' ,OperasyonAdi='{mdl.OperasyonAdi}' 
                                  where Operasyon='{oldCode}' ;";
                    var rs1 = Service.Execute(sql1, null, trs);
                    if (!rs1.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs1.Message);
                    }
                    /* ReceteOperasyon  */
                    string sql2 = $@" UPDATE ReceteOperasyon SET OperasyonKodu= '{mdl.OperasyonKodu}' ,OperasyonAdi='{mdl.OperasyonAdi}' 
                                  where OperasyonKodu='{oldCode}' ;";
                    var rs2 = Service.Execute(sql2, null, trs);
                    if (!rs2.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs2.Message);
                    }
                    /* ReceteIstasyon  */
                    string sql3 = $@" UPDATE ReceteIstasyon SET OperasyonKodu= '{mdl.OperasyonKodu}' ,OperasyonAdi='{mdl.OperasyonAdi}' 
                                  where OperasyonKodu='{oldCode}' ;";
                    var rs3 = Service.Execute(sql3, null, trs);
                    if (!rs3.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs3.Message);
                    }
                    /* ReceteIstasyonGrupOperasyon  */
                    string sql4 = $@" UPDATE ReceteIstasyonGrupOperasyon SET OperasyonKodu= '{mdl.OperasyonKodu}' ,OperasyonAdi='{mdl.OperasyonAdi}' 
                                  where OperasyonKodu='{oldCode}' ;";
                    var rs4 = Service.Execute(sql4, null, trs);
                    if (!rs4.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs4.Message);
                    }
                    /* UretimOperasyon */
                    string sql5 = $@" UPDATE UretimOperasyon SET OperasyonKodu= '{mdl.OperasyonKodu}' ,OperasyonAdi='{mdl.OperasyonAdi}' 
                                  where OperasyonKodu='{oldCode}' ;";
                    var rs5 = Service.Execute(sql5, null, trs);
                    if (!rs5.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs5.Message);
                    }
                    /* IstasyonTakipHareket */
                    string sql6 = $@" UPDATE IstasyonTakipHareket SET OperasyonKodu= '{mdl.OperasyonKodu}' ,OperasyonAdi='{mdl.OperasyonAdi}' 
                                  where OperasyonKodu='{oldCode}' ;";
                    var rs6 = Service.Execute(sql6, null, trs);
                    if (!rs6.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs6.Message);
                    }
                }
                trs.Commit();
            }
            catch (Exception e)
            {
                trs.Dispose();
                return new ErrorDataResult<string>(e.Message);
            }
            finally
            {
                trs.Dispose();
            }

            return new SuccessDataResult<string>();
        }
        public virtual IResult KodVarmi<T>(T entity, string kontrolalan, bool yenikayitmi)
        {
            var tabloadi = ClassExtensions.GetClassTableName(typeof(T));
            var GetId = ClassExtensions.GetClassColumnNameKey(typeof(T));
            var sql2 = $" where  {GetId} <> @{GetId} and {kontrolalan} = @{kontrolalan} ";
            if (yenikayitmi) sql2 = $" where  {kontrolalan} = @{kontrolalan} ";
            var sql = string.Format("Select count(*) From {0}  {1};", tabloadi, sql2);
            var rs = Service.Query<int>(sql, entity);
            if (!rs.Success) return new ErrorResult(rs.Message);
            if (rs.Data.FirstOrDefault() > 0) return new ErrorResult("Aynı " + kontrolalan + " Kodla Kayıt Var");
            return new SuccessResult();
        }
    }

}
