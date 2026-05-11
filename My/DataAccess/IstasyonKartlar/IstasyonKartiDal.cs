using My.Core.Data;
using My.Entities.IstasyonKartlar;
using System.Data;

namespace My.DataAccess.IstasyonKartlar
{
    public class IstasyonKartiDal : BaseDal<IstasyonKarti>, IIstasyonKartiDal
    {
        public IstasyonKartiDal(IDbConnection connection) : base(connection)
        {
        }
    }
}