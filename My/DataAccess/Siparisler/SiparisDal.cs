using My.Core.Data;
using My.Entities.Siparisler;
using System.Data;

namespace My.DataAccess.Siparisler
{
    public class SiparisDal : BaseDal<Siparis>, ISiparisDal
    {
        public SiparisDal(IDbConnection connection) : base(connection)
        {
        }
    }
}