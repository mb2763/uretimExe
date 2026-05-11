using My.Core.Data;
using My.Entities.IstasyonTakipler;
using System.Collections.Generic;

namespace My.DataAccess.IstasyonTakipler
{
    public interface IIstasyonTakipHareketDetayDal : IBaseDal<IstasyonTakipHareketDetay>
    {
        IEnumerable<IstasyonTakipHareketDetay> GetViewListWhere(string whereSql); 
        IEnumerable<IstasyonTakipHareketDetay> GetViewListStokFire(string andwhereSql);

    }
}
