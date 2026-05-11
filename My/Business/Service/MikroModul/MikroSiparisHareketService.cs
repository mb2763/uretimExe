using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.MikroModul;
using My.Entities.Mikro;
using System;
using System.Collections.Generic;

namespace My.Business.Service.MikroModul
{
    public class MikroSiparisHareketService : BaseService<MikroSiparisHareket>, IMikroSiparisHareketService
    {
        private readonly IMikroSiparisHareketDal _dal;
        private readonly ILogManager _ilogger;

        public MikroSiparisHareketService(IMikroSiparisHareketDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }

        public IDataResult<IEnumerable<MikroSiparisHareket>> GetViewListWhere(string whereSql)
        {
            try
            {
                var r = _dal.GetViewListWhere(whereSql);
                return new SuccessDataResult<IEnumerable<MikroSiparisHareket>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<MikroSiparisHareket>>(e.Message);
            }
        }

        public IDataResult<IEnumerable<MikroSiparisHareket>> GetViewListSeriSira(string seri, string sira)
        {
            try
            {
                var r = _dal.GetViewListSeriSira(seri, sira);
                return new SuccessDataResult<IEnumerable<MikroSiparisHareket>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<MikroSiparisHareket>>(e.Message);
            }
        }
    }
}