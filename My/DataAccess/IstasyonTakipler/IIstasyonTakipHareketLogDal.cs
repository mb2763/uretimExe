using My.Core.Data;
using My.Entities.IstasyonTakipler;
using System.Collections.Generic;

namespace My.DataAccess.IstasyonTakipler {
    public interface IIstasyonTakipHareketLogDal : IBaseDal<IstasyonTakipHareketLog> {
        /// <summary>
        /// Log Tablo LG  Hareket Tablo HR
        /// </summary>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        IEnumerable<IstasyonTakipHareketLog> GetViewListWhere(string whereSql);
    }
}
