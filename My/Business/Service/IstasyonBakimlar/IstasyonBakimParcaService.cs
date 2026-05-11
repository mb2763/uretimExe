using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.IstasyonBakimlar;
using My.Entities.IstasyonBakimlar;

namespace My.Business.Service.IstasyonBakimlar
{
    internal class IstasyonBakimParcaService : BaseService<IstasyonBakimParca>, IIstasyonBakimParcaService
    {
        private IIstasyonBakimParcaDal _dal;
        private ILogManager _ilogger;

        public IstasyonBakimParcaService(IIstasyonBakimParcaDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}
