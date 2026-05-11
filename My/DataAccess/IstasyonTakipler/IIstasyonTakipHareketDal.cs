using My.Core.Data;
using My.Entities.IstasyonTakipler;
using System.Collections.Generic;

namespace My.DataAccess.IstasyonTakipler {
    public interface IIstasyonTakipHareketDal : IBaseDal<IstasyonTakipHareket> {

        IEnumerable<IstasyonTakipHareket> GetViewListWhere(string whereSql);
    }
}
