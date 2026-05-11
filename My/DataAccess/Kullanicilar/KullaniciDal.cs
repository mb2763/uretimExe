using My.Core.Data;
using My.Entities.Kullanicilar;
using System.Data;

namespace My.DataAccess.Kullanicilar
{
    public class KullaniciDal : BaseDal<Kullanici>, IKullaniciDal
    {
        public KullaniciDal(IDbConnection connection) : base(connection)
        {
        }
    }
}