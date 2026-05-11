using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.ReceteIstasyonGruplar;
using My.Entities.ReceteIstasyonGruplar;
using System;
using System.Linq;

namespace My.Business.Service.ReceteIstasyonGruplar
{
    public class ReceteIstasyonGrupKodService : BaseService<ReceteIstasyonGrupKod>, IReceteIstasyonGrupKodService
    {
        private IReceteIstasyonGrupKodDal _dal;
        private ILogManager _ilogger;

        public ReceteIstasyonGrupKodService(IReceteIstasyonGrupKodDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }


        public   IResult KodVarmi<T>(T entity, string kontrolalan, bool yenikayitmi)
        {
            try
            {
                var tabloadi = ClassExtensions.GetClassTableName(typeof(T));
                var GetId = ClassExtensions.GetClassColumnNameKey(typeof(T));
                var sql2 = $" where  {GetId} <> @{GetId} and {kontrolalan} = @{kontrolalan} ";
                if (yenikayitmi) sql2 = $" where  {kontrolalan} = @{kontrolalan} ";
                var sql = string.Format("Select count(*) From {0}  {1};", tabloadi, sql2);
                var rs = _dal.Query<int>(sql, entity);
                if (rs.FirstOrDefault() > 0) throw new Exception("Aynı " + kontrolalan + " Kodla Kayıt Var");
                return new SuccessResult();
               
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);

            }
           
        }
    }
}
