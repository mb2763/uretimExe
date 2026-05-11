using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.Siparisler;
using My.Entities.Siparisler;
using System;
using System.Collections.Generic;

namespace My.Business.Service.Siparisler
{
    public class SiparisHareketService : BaseService<SiparisHareket>, ISiparisHareketService
    {
        private readonly ISiparisHareketDal _dal;
        private readonly ILogManager _ilogger;

        public SiparisHareketService(ISiparisHareketDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }

        public IDataResult<IEnumerable<SiparisHareketModel>> GetViewListWhere(string whereSql)
        {
            try
            {
                var r = _dal.GetViewListWhere(whereSql);
                return new SuccessDataResult<IEnumerable<SiparisHareketModel>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<SiparisHareketModel>>(e.Message);
            }
        } 
        public IDataResult<IEnumerable<SiparisHareket>> GetViewListWhere2(string whereSql)
        {
            try
            {
                var r = _dal.GetViewListWhere2(whereSql);
                return new SuccessDataResult<IEnumerable<SiparisHareket>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<SiparisHareket>>(e.Message);
            }
        } 
        public IDataResult<IEnumerable<SiparisHareket>> GetViewListKalanMiktarliWhere(string whereSql)
        {
            try
            {
                var r = _dal.GetViewListKalanMiktarliWhere(whereSql);
                return new SuccessDataResult<IEnumerable<SiparisHareket>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<SiparisHareket>>(e.Message);
            }
        }
    }
}