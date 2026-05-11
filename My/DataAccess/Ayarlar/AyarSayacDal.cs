using My.Core.Data;
using My.Entities.Ayarlar;
using System.Data;

namespace My.DataAccess.Ayarlar
{
    public class AyarSayacDal : BaseDal<AyarSayac>, IAyarSayacDal
    {
        public AyarSayacDal(IDbConnection connection) : base(connection)
        {
        }
    }
}