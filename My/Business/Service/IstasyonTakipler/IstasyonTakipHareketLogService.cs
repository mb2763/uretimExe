
using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.IstasyonTakipler;
using My.Entities.IstasyonTakipler;
using System.Collections.Generic;
using System;

namespace My.Business.Service.IstasyonTakipler {
    public class IstasyonTakipHareketLogService : BaseService<IstasyonTakipHareketLog>, IIstasyonTakipHareketLogService {
        private IIstasyonTakipHareketLogDal _dal;
        private ILogManager _ilogger;

        public IstasyonTakipHareketLogService(IIstasyonTakipHareketLogDal dal, ILogManager ilogger) : base(dal, ilogger) {
            _dal = dal;
            _ilogger = ilogger;
        }
        /// <summary>
        /// Log Tablo LG  Hareket Tablo HR
        /// </summary>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        public IDataResult<IEnumerable<IstasyonTakipHareketLog>> GetViewListWhere(string whereSql) {
            try {
                var r = _dal.GetViewListWhere(whereSql);
                return new SuccessDataResult<IEnumerable<IstasyonTakipHareketLog>>(r);
            }
            catch (Exception e) {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<IstasyonTakipHareketLog>>(e.Message);
            }
        }


    }

}
