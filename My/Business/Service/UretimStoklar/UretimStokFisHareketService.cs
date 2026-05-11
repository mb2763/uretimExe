using My.Core.Data;
using My.Core.Logger;
using My.Entities.UretimStoklar;
using System.Data;

namespace My.Business.Service.UretimStoklar {
    public class UretimStokFisHareketService : BaseServiceCore<UretimStokFisHareket>, IUretimStokFisHareketService {
        public UretimStokFisHareketService(IDbConnection dbConnection, ILogManager log) : base(dbConnection, log) {

        }
    }
}
