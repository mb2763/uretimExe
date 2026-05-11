using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.ReceteIstasyonlar;
using My.Entities.ReceteIstasyonlar;

namespace My.Business.Service.ReceteIstasyonlar
{ 
    public class ReceteyeBagliIstasyonService : BaseService<ReceteyeBagliIstasyon>, IReceteyeBagliIstasyonService
    {
        public ReceteyeBagliIstasyonService(IReceteyeBagliIstasyonDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
        } 
    }
}
