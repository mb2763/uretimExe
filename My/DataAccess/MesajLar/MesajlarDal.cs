using My.Core.Data;
using My.Entities.MesajLar;
using System.Data;

namespace My.DataAccess.MesajLar
{
    public class MesajlarDal : BaseDal<Mesajlar>, IMesajlarDal
    {
        public MesajlarDal(IDbConnection connection) : base(connection)
        {
        }
    }
}
