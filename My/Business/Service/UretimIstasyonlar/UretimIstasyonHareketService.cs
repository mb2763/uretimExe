using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.UretimIstasyonlar;
using My.Entities.UretimIstasyonlar;
using System;
using System.Collections.Generic;

namespace My.Business.Service.UretimIstasyonlar
{
    public class UretimIstasyonHareketService : BaseService<UretimIstasyonHareket>, IUretimIstasyonHareketService
    {
        private readonly IUretimIstasyonHareketDal _dal;
        private readonly ILogManager _ilogger;

        public UretimIstasyonHareketService(IUretimIstasyonHareketDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }

        public IDataResult<IEnumerable<UretimIstasyonHareket>> GetViewListWhere(string whereSql)
        {
            try
            {
                var r = _dal.GetViewListWhere(whereSql);
                return new SuccessDataResult<IEnumerable<UretimIstasyonHareket>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<UretimIstasyonHareket>>(e.Message);
            }
        }
    }
}