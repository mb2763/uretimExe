using My.Core.Data;
using System.Data;
using My.Entities.Mikro;

namespace My.DataAccess.MikroModul
{
    public class MikroStokHareketleriDal : BaseDal<MikroStokHareketleri>, IMikroStokHareketleriDal
    {
        public MikroStokHareketleriDal(IDbConnection connection) : base(connection)
        {
        }
    }
}