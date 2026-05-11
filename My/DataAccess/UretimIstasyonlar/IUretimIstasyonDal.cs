using My.Core.Data;
using My.Entities.UretimIstasyonlar;
using System.Collections.Generic;

namespace My.DataAccess.UretimIstasyonlar
{
    public interface IUretimIstasyonDal : IBaseDal< UretimIstasyon>
    {
        IEnumerable< UretimIstasyon> GetViewListWhere(string whereSql);
    }
}