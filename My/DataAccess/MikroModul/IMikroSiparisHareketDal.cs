using My.Core.Data;
using My.Entities.Mikro;
using System.Collections.Generic;

namespace My.DataAccess.MikroModul
{
    public interface IMikroSiparisHareketDal : IBaseDal<MikroSiparisHareket>
    {
        IEnumerable<MikroSiparisHareket> GetViewListWhere(string whereSql = "");
        IEnumerable<MikroSiparisHareket> GetViewListSeriSira(string seri, string sira);
    }
}