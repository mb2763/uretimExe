using My.Core.Data;
using My.Entities.IstasyonBakimlar;
using System.Data;

namespace My.DataAccess.IstasyonBakimlar
{
    public class IstasyonBakimDal : BaseDal<IstasyonBakim>, IIstasyonBakimDal
    {
        public IstasyonBakimDal(IDbConnection connection) : base(connection)
        {
        }
    }
}
