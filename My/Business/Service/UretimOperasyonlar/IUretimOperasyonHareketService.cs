using My.Core.Data;
using My.Core.Result;
using My.Entities.UretimOperasyonlar;
using System.Collections.Generic;

namespace My.Business.Service.UretimOperasyonlar
{
    public interface IUretimOperasyonHareketService : IBaseService<UretimOperasyonHareket>
    {
        /// <summary>
        ///     UretimOperasyonHareket UrOH   UretimOperasyon UrO
        /// </summary>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        IDataResult<IEnumerable<UretimOperasyonHareket>> GetViewListWhere(string whereSql);
    }
}