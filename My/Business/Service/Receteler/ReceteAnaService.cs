using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.Receteler;
using My.Entities.Receteler;
using System;
using System.Collections.Generic;

namespace My.Business.Service.Receteler
{
    public class ReceteAnaService : BaseService<ReceteAna>, IReceteAnaService
    {
        private readonly IReceteAnaDal _dal;
        private readonly ILogManager _ilogger;
        public ReceteAnaService(IReceteAnaDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
        public IDataResult<IEnumerable<ReceteAna>> GetListWhere(string where)
        {
            try
            {
                var r = _dal.GetListWhere(where);
                return new SuccessDataResult<IEnumerable<ReceteAna>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<ReceteAna>>(e.Message);
            }
        }

    }
}