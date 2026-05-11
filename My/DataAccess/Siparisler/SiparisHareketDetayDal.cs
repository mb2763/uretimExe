using My.Core.Data;
using My.Entities.Siparisler;
using System.Data;

namespace My.DataAccess.Siparisler
{
    public class SiparisHareketDetayDal : BaseDal<SiparisHareketDetay>, ISiparisHareketDetayDal
    {
        public SiparisHareketDetayDal(IDbConnection connection) : base(connection)
        {
        }
    }
}