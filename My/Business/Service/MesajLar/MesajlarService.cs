using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.MesajLar;
using My.Entities.MesajLar;

namespace My.Business.Service.MesajLar
{
    public class MesajlarService : BaseService<Mesajlar>, IMesajlarService
    {
        private IMesajlarDal _dal;
        private ILogManager _ilogger;

        public MesajlarService(IMesajlarDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}
