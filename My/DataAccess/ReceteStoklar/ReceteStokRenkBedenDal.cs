using My.Core.Data;
using My.Entities.ReceteStoklar;
using System.Data;

namespace My.DataAccess.ReceteStoklar {
    public class ReceteStokRenkBedenDal : BaseDal<ReceteStokRenkBeden>, IReceteStokRenkBedenDal {
        public ReceteStokRenkBedenDal(IDbConnection connection) : base(connection) {
        }
    }
}
