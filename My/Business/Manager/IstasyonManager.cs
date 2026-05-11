using My.Business.Service.IstasyonKartlar;
using My.Core.Data;
using My.Core.Result;
using My.Entities.IstasyonKartlar;
using My.Entities.Models;
using My.Entities.OperasyonKartlar;
using System;
using System.Data;
using System.Linq;

namespace My.Business.Manager
{
    public class IstasyonManager
    {
        private readonly DatabaseFactoryPro _dbPro;
        public IIstasyonKartiService Service { get; set; }

        public IstasyonManager(DatabaseFactoryPro dbPro)
        {
            _dbPro = dbPro;
            Service = _dbPro.IstasyonKarti;
        }

        public IDataResult<string> Kaydet(IstasyonKarti mdl, bool yenikayit)
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
                if (dt1.IstasyonKodu != mdl.IstasyonKodu)
                {
                    update = true;
                    oldCode = dt1.IstasyonKodu;
                }
            }


            var rsKod = KodVarmi(mdl, "IstasyonKodu", yenikayit);
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
                    /* ReceteIstasyon  */
                    string sql3 = $@" UPDATE ReceteIstasyon SET IstasyonKodu= '{mdl.IstasyonKodu}' ,IstasyonAdi='{mdl.IstasyonAdi}' 
                                  where IstasyonKodu='{oldCode}' ;";
                    var rs3 = Service.Execute(sql3, null, trs);
                    if (!rs3.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs3.Message);
                    }
                    /* ReceteIstasyonGrupOperasyon  */
                    string sql4 = $@" UPDATE ReceteIstasyonGrupOperasyon SET IstasyonKodu= '{mdl.IstasyonKodu}' ,IstasyonAdi='{mdl.IstasyonAdi}' 
                                  where IstasyonKodu='{oldCode}' ;";
                    var rs4 = Service.Execute(sql4, null, trs);
                    if (!rs4.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs4.Message);
                    }
                    /* UretimIstasyon */
                    string sql5 = $@" UPDATE UretimIstasyon SET IstasyonKodu= '{mdl.IstasyonKodu}' ,IstasyonAdi='{mdl.IstasyonAdi}' 
                                  where IstasyonKodu='{oldCode}' ;";
                    var rs5 = Service.Execute(sql5, null, trs);
                    if (!rs5.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs5.Message);
                    }
                    /* IstasyonTakipHareket */
                    string sql6 = $@" UPDATE IstasyonTakipHareket SET IstasyonKodu= '{mdl.IstasyonKodu}' ,IstasyonAdi='{mdl.IstasyonAdi}' 
                                  where IstasyonKodu='{oldCode}' ;";
                    var rs6 = Service.Execute(sql6, null, trs);
                    if (!rs6.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs6.Message);
                    }
                    /* IstasyonKontrol */
                    string sql7 = $@" UPDATE IstasyonKontrol SET IstasyonKodu= '{mdl.IstasyonKodu}'   
                                  where IstasyonKodu='{oldCode}' ;";
                    var rs7 = Service.Execute(sql7, null, trs);
                    if (!rs7.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs7.Message);
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
