using My.Core.Data;
using My.Core.Result;
using My.Entities.Geneller;
using My.Entities.Mikro; 
using System.Collections.Generic; 

namespace My.Business.Service.MikroModul {
    public interface IMikroGenelService : IBaseService<Genel> {
        IDataResult<IEnumerable<string>> GrupListesi(string tabloadi, string sutun);
        IDataResult<IEnumerable<MikroDepo>> GetMikroDepoListesi(string where );
     
    }
}
