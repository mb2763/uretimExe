using My.Core.Data;
using My.Core.Result; 
using My.Entities.Templer;
using System;
using System.Collections.Generic; 
namespace My.Business.Service.Templer {
    public interface ITempSiparisUretimMiktarService : IBaseService<TempSiparisUretimMiktar> {

        IDataResult<IEnumerable<TempSiparisUretimMiktar>> GetTempSiparisUretimMiktarBySipId(Guid? sipId, string kullanici);
        IDataResult<IEnumerable<TempSiparisUretimMiktar>> GetTempSiparisUretimMiktarBySipKodu(string sipkodu, string kullanici);

    }
}
