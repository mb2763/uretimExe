using My.Core.Data;
using My.Entities.Personeller;
using System.Data;

namespace My.DataAccess.Personeller
{
    public class PersonelDal : BaseDal<Personel>, IPersonelDal
    {
        public PersonelDal(IDbConnection connection) : base(connection)
        {
        }
    }
}