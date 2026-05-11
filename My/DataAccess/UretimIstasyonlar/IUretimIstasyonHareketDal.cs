using My.Core.Data;
using My.Entities.UretimIstasyonlar;
using System.Collections.Generic;

namespace My.DataAccess.UretimIstasyonlar
{
    public interface IUretimIstasyonHareketDal : IBaseDal<UretimIstasyonHareket>
    {
        IEnumerable<UretimIstasyonHareket> GetViewListWhere(string whereSql);
    }
}