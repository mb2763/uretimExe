using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.MikroModul;
using My.Entities.Mikro;
using System;
using System.Collections.Generic;

namespace My.Business.Service.MikroModul
{
    public class MikroSiparisService : BaseService<MikroSiparis>, IMikroSiparisService
    {
        private readonly IMikroSiparisDal _dal;
        private readonly ILogManager _ilogger;

        public MikroSiparisService(IMikroSiparisDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }

        public IDataResult<IEnumerable<MikroSiparis>> GetViewListWhere(string whereSql)
        {
            try
            {
                var r = _dal.GetViewListWhere(whereSql);
                return new SuccessDataResult<IEnumerable<MikroSiparis>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<MikroSiparis>>(e.Message);
            }
        }
    }
}