using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.IstasyonTakipler;
using My.Entities.IstasyonTakipler;
using System;
using System.Collections.Generic;

namespace My.Business.Service.IstasyonTakipler
{
    public class IstasyonTakipHareketDetayService : BaseService<IstasyonTakipHareketDetay>, IIstasyonTakipHareketDetayService
    {
        private IIstasyonTakipHareketDetayDal _dal;
        private ILogManager _ilogger;

        public IstasyonTakipHareketDetayService(IIstasyonTakipHareketDetayDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }

        public IDataResult<IEnumerable<IstasyonTakipHareketDetay>> GetViewListWhere(string whereSql)
        {
            try
            {
                var r = _dal.GetViewListWhere(whereSql);
                return new SuccessDataResult<IEnumerable<IstasyonTakipHareketDetay>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<IstasyonTakipHareketDetay>>(e.Message);
            }
        } 
        public IDataResult<IEnumerable<IstasyonTakipHareketDetay>> GetViewListStokFire(string andwhereSql) {
            try {
                var r = _dal.GetViewListStokFire(andwhereSql);
                return new SuccessDataResult<IEnumerable<IstasyonTakipHareketDetay>>(r);
            } catch (Exception e) {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<IstasyonTakipHareketDetay>>(e.Message);
            }
        }
    }
}
