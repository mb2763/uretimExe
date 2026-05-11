using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.UretimIstasyonlar;
using My.Entities.UretimIstasyonlar;
using System;
using System.Collections.Generic;

namespace My.Business.Service.UretimIstasyonlar
{
    public class UretimIstasyonService : BaseService<UretimIstasyon>, IUretimIstasyonService
    {
        private readonly IUretimIstasyonDal _dal;
        private readonly ILogManager _ilogger;

        public UretimIstasyonService(IUretimIstasyonDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }

        public IDataResult<IEnumerable<UretimIstasyon>> GetViewListWhere(string whereSql)
        {
            try
            {
                var r = _dal.GetViewListWhere(whereSql);
                return new SuccessDataResult<IEnumerable<UretimIstasyon>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<UretimIstasyon>>(e.Message);
            }
        }
    }
}