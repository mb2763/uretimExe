using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.UretimOperasyonlar;
using My.Entities.UretimOperasyonlar;
using System;
using System.Collections.Generic;

namespace My.Business.Service.UretimOperasyonlar
{
    public class UretimOperasyonHareketDetayService : BaseService<UretimOperasyonHareketDetay>,
        IUretimOperasyonHareketDetayService
    {
        private readonly IUretimOperasyonHareketDetayDal _dal;
        private readonly ILogManager _ilogger;

        public UretimOperasyonHareketDetayService(IUretimOperasyonHareketDetayDal dal, ILogManager ilogger) : base(dal,
            ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }

        public IDataResult<IEnumerable<UretimOperasyonHareketDetay>> GetViewListWhere(string whereSql)
        {
            try
            {
                var r = _dal.GetViewListWhere(whereSql);
                return new SuccessDataResult<IEnumerable<UretimOperasyonHareketDetay>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<UretimOperasyonHareketDetay>>(e.Message);
            }
        }
    }
}