using My.Core.Data;
using My.Core.Result;
using My.Entities.IstasyonTakipler;
using My.Entities.UretimOperasyonlar;
using System.Collections.Generic;

namespace My.Business.Service.IstasyonTakipler {
    public interface IIstasyonTakipHareketService : IBaseService<IstasyonTakipHareket> {
        IDataResult<IEnumerable<IstasyonTakipHareket>> GetViewListWhere(string whereSql);
     
    }
}
