using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.Geneller;
using My.Entities.Geneller;
using My.Entities.Mikro;
using System;
using System.Collections.Generic;

namespace My.Business.Service.Geneller
{
    public class GenelService : BaseService<Genel>, IGenelService
    {
        private readonly IGenelDal _dal;
        private readonly ILogManager _ilogger;

        public GenelService(IGenelDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }

        public IDataResult<IEnumerable<string>> GrupListesi(string tabloadi, string sutun)
        {
            try
            {
                var r = _dal.GrupListesi(tabloadi, sutun);
                return new SuccessDataResult<IEnumerable<string>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<string>>(e.Message);
            }
        }

      

        public IDataResult<string> GetEvrakNo(string kodu)
        {
            try
            {
                var r = _dal.GetEvrakNo(kodu);
                return new SuccessDataResult<string>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<string>(e.Message);
            }
        }
    }
}