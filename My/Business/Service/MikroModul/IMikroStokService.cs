using My.Core.Data;
using My.Core.Result;
using System.Collections.Generic;
using My.Entities.Mikro;

namespace My.Business.Service.MikroModul
{
    public interface IMikroStokService : IBaseService<MikroStok>
    {
        IDataResult<IEnumerable<MikroStok>> GetViewListWhere(string whereSql, string stokGrubKodu);
        IDataResult<IEnumerable<MikroStokMaliyet>> GetMikroStokMaliyetListWhere(string whereSql);

        IDataResult<IEnumerable<MikroStokRenk>> GetRenkListWhere(string wheresql);
        IDataResult<IEnumerable<MikroStokBeden>> GetBedenListWhere(string wheresql);
        IDataResult<MikroStokRenk> GetRenkByKodu(string renkKodu);
        IDataResult<MikroStokBeden> GetBedenByKodu(string bedenKodu);

        IDataResult<MikroStokRenk> GetRenkByStokKodu(string stokKodu);
        IDataResult<MikroStokBeden> GetBedenByStokKodu(string stokKodu);
        IDataResult<List<string>> GetStokKategoriler();

    }
}