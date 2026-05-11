using My.Core.Data;
using My.Entities.UretimOperasyonlar;
using System.Collections.Generic;

namespace My.DataAccess.UretimOperasyonlar
{
    public interface IUretimOperasyonDal : IBaseDal<UretimOperasyon>
    {
        IEnumerable<UretimOperasyon> GetViewListWhere(string whereSql);
    }
}