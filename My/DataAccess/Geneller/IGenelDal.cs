using My.Core.Data;
using My.Entities.Geneller;
using My.Entities.Mikro;
using System.Collections.Generic;

namespace My.DataAccess.Geneller
{
    public interface IGenelDal : IBaseDal< Genel>
    {
        IEnumerable<string> GrupListesi(string tabloadi, string sutun);
    
        string GetEvrakNo(string kodu);
    }
}