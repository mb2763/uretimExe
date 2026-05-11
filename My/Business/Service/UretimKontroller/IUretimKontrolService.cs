using My.Core.Data;
using My.Core.Result;
using My.Entities.UretimIstasyonlar;
using My.Entities.UretimKontroller;
using System.Collections.Generic;

namespace My.Business.Service.UretimKontroller {
    public interface IUretimKontrolService : IBaseService<UretimKontrol> {
        IDataResult<IEnumerable<UretimKontrol>> GetViewListWhere(string whereSql);
    }
}
