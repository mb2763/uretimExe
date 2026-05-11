using My.Core.Data;
using My.Core.Result;
using My.Entities.Models;
using System;
using System.Data;
using System.Linq;

namespace My.Business.Manager
{
    public class ReceteGrupManager
    {
        private readonly DatabaseFactoryPro _db;

        public ReceteGrupManager(DatabaseFactoryPro dbPro)
        {
            _db = dbPro;
        }

        public ReceteGrupKayitmodel GetReceteKayit()
        {
            return new ReceteGrupKayitmodel();
        }

        public IDataResult<ReceteGrupKayitmodel> GetReceteKayit(Guid? id)
        {
            var mdl = new ReceteGrupKayitmodel();
            var recete = _db.ReceteGrup.SelectFirst(c => c.Id == id);
            if (!recete.Success) return new ErrorDataResult<ReceteGrupKayitmodel>(recete.Message);
            mdl.Grup = recete.Data;
            var receteDetay = _db.ReceteGrupDetay.SelectList(c => c.RcGId == id);
            if (!receteDetay.Success) return new ErrorDataResult<ReceteGrupKayitmodel>(receteDetay.Message);
            mdl.Detaylar = receteDetay.Data.ToList();
            return new SuccessDataResult<ReceteGrupKayitmodel>(mdl);
        }

        public IResult ReceteGrupKaydet(ReceteGrupKayitmodel mdl, bool yenikayit)
        {
            var con = _db.ReceteGrup.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try
            {

                var rsAna = _db.ReceteGrup.InsertOrUpdate(mdl.Grup, trs);
                if (!rsAna.Success)
                {
                    trs.Dispose();
                    return new ErrorResult(rsAna.Message);
                }

                /* detaylar Sil */
                var rsDIDel = _db.ReceteGrupDetay.Delete(c => c.RcGId == mdl.Grup.Id, trs);
                if (!rsDIDel.Success)
                {
                    trs.Dispose();
                    return new ErrorResult(rsDIDel.Message);
                }
                /* detaylar */
                var rsDI = _db.ReceteGrupDetay.InsertOrUpdate(mdl.Detaylar, trs);
                if (!rsDI.Success)
                {
                    trs.Dispose();
                    return new ErrorResult(rsDI.Message);
                }

                trs.Commit();
            }
            catch (Exception e)
            {
                trs.Dispose();
                return new ErrorResult(e.Message);
            }
            finally
            {
                trs.Dispose();
            }

            return new SuccessResult();
        }

        public virtual IResult KodVarmi<T>(T entity, string kontrolalan, bool yenikayitmi)
        {
            var tabloadi = ClassExtensions.GetClassTableName(typeof(T));
            var GetId = ClassExtensions.GetClassColumnNameKey(typeof(T));
            var sql2 = $" where  {GetId} <> @{GetId} and {kontrolalan} = @{kontrolalan} ";
            if (yenikayitmi) sql2 = $" where  {kontrolalan} = @{kontrolalan} ";
            var sql = string.Format("Select count(*) From {0}  {1};", tabloadi, sql2);
            var rs = _db.ReceteGrup.Query<int>(sql, entity);
            if (!rs.Success) return new ErrorResult(rs.Message);
            if (rs.Data.FirstOrDefault() > 0) throw new Exception("Aynı " + kontrolalan + " Kodla Kayıt Var");
            return new SuccessResult();
        }

        public IResult ReceteGrupSil(ReceteGrupKayitmodel mdl)
        {
            var con = _db.ReceteAna.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try
            {
                var rsAna = _db.ReceteGrup.Delete(mdl.Grup, trs);
                if (!rsAna.Success)
                {
                    trs.Dispose();
                    return new ErrorResult(rsAna.Message);
                }

                /* detaylar Sil */
                var rsDI = _db.ReceteGrupDetay.Delete(c => c.RcGId == mdl.Grup.Id, trs);
                if (!rsDI.Success)
                {
                    trs.Dispose();
                    return new ErrorResult(rsDI.Message);
                }

                trs.Commit();
            }
            catch (Exception e)
            {
                trs.Dispose();
                return new ErrorResult(e.Message);
            }
            finally
            {
                trs.Dispose();
            }

            return new SuccessResult();
        }
    }
}