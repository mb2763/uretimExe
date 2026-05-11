using My.Core.Data;
using My.Core.Result;
using My.Entities.IstasyonTakipler;
using System.Collections.Generic;

namespace My.Business.Service.IstasyonTakipler {
    public interface IIstasyonTakipHareketLogService : IBaseService<IstasyonTakipHareketLog> {

        /// <summary>
        /// Log Tablo LG  Hareket Tablo HR
        /// </summary>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        IDataResult<IEnumerable<IstasyonTakipHareketLog>> GetViewListWhere(string whereSql);
    }
}
