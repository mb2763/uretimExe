using My.Core.Data;
using My.Entities.UretimOperasyonlar;
using System.Collections.Generic;

namespace My.DataAccess.UretimOperasyonlar
{
    public interface IUretimOperasyonHareketDal : IBaseDal< UretimOperasyonHareket>
    {
        IEnumerable< UretimOperasyonHareket> GetViewListWhere(string whereSql);
    }
}