using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.IstasyonTakipler;
using My.Entities.IstasyonTakipler;
using System;
using System.Collections.Generic;

namespace My.Business.Service.IstasyonTakipler
{
    public class IstasyonTakipHareketService : BaseService<IstasyonTakipHareket>, IIstasyonTakipHareketService
    {
        private IIstasyonTakipHareketDal _dal;
        private ILogManager _ilogger;

        public IstasyonTakipHareketService(IIstasyonTakipHareketDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
        public IDataResult<IEnumerable<IstasyonTakipHareket>> GetViewListWhere(string whereSql)
        {
            try
            {
                var r = _dal.GetViewListWhere(whereSql);
                return new SuccessDataResult<IEnumerable<IstasyonTakipHareket>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<IstasyonTakipHareket>>(e.Message);
            }
        }
    }
}
