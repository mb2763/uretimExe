using My.Core.Data;
using My.Entities.Ayarlar;
using System.Data;

namespace My.DataAccess.Ayarlar {
    public class AyarDal : BaseDal<Ayar>, IAyarDal {
        public AyarDal(IDbConnection connection) : base(connection) {
        }

    }
}
