using My.Core.Data;
using My.Entities.UretimOperasyonlar;
using System.Collections.Generic;

namespace My.DataAccess.UretimOperasyonlar
{
    public interface IUretimOperasyonHareketDetayDal : IBaseDal< UretimOperasyonHareketDetay>
    {
        IEnumerable< UretimOperasyonHareketDetay> GetViewListWhere(string whereSql);
    }
}