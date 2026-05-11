using My.Core.Data;
using My.Core.Result;
using My.Entities.IstasyonTakipler;
using System;
using System.Collections.Generic;

namespace My.Business.Service.IstasyonTakipler {
    public interface IIstasyonTakipHareketDetayService : IBaseService<IstasyonTakipHareketDetay> {
        IDataResult<IEnumerable<IstasyonTakipHareketDetay>> GetViewListWhere(string whereSql); 
        IDataResult<IEnumerable<IstasyonTakipHareketDetay>> GetViewListStokFire(string andwhereSql);
    }
}
