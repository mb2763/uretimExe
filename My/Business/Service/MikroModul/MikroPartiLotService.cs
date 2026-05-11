using My.Core.Data;
using My.Core.Logger;
using My.Entities.Mikro;
using System.Data;

namespace My.Business.Service.MikroModul {
    public class MikroPartiLotService : BaseServiceCore<MikroPartiLot>, IMikroPartiLotService {
        public MikroPartiLotService(IDbConnection dbConnection, ILogManager log) : base(dbConnection, log) {

        } 
    }
}


