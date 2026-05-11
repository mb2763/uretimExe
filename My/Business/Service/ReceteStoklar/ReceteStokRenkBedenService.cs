using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.ReceteStoklar;
using My.Entities.ReceteStoklar;

namespace My.Business.Service.ReceteStoklar {
    internal class ReceteStokRenkBedenService : BaseService<ReceteStokRenkBeden>, IReceteStokRenkBedenService {
        public ReceteStokRenkBedenService(IReceteStokRenkBedenDal dal, ILogManager ilogger) : base(dal, ilogger) {
        }
    }
}
