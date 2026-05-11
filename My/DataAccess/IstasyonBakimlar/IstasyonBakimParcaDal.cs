using My.Core.Data;
using My.Entities.IstasyonBakimlar;
using System.Data;

namespace My.DataAccess.IstasyonBakimlar
{
    public class IstasyonBakimParcaDal : BaseDal<IstasyonBakimParca>, IIstasyonBakimParcaDal
    {
        public IstasyonBakimParcaDal(IDbConnection connection) : base(connection)
        {
        }
    }
}
