using My.Core.Data;
using My.Entities.Siparisler;
using System.Collections.Generic;

namespace My.DataAccess.Siparisler
{
    public interface ISiparisHareketDal : IBaseDal<SiparisHareket>
    {
        /// <summary>
        ///     siparis S sipariş hareket SH
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        IEnumerable<SiparisHareketModel> GetViewListWhere(string wheresql);
        IEnumerable<SiparisHareket> GetViewListWhere2(string wheresql);
        IEnumerable<SiparisHareket> GetViewListKalanMiktarliWhere(string whereSql);
    }
}