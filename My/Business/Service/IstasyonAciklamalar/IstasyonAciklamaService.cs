using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.IstasyonAciklamalar;
using My.Entities.IstasyonAciklamalar;

namespace My.Business.Service.IstasyonAciklamalar {
    public class IstasyonAciklamaService : BaseService<IstasyonAciklama>, IIstasyonAciklamaService {
        private IIstasyonAciklamaDal _dal;
        private ILogManager _ilogger;

        public IstasyonAciklamaService(IIstasyonAciklamaDal dal, ILogManager ilogger) : base(dal, ilogger) {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}
