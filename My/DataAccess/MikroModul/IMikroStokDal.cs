using My.Core.Data;
using My.Entities.Mikro;
using System.Collections.Generic;

namespace My.DataAccess.MikroModul {
    public interface IMikroStokDal : IBaseDal<MikroStok> {
        IEnumerable<MikroStok> GetViewListWhere(string whereSql, string stokGrubKodu);
        IEnumerable<MikroStokMaliyet> GetMikroStokMaliyetListWhere(string whereSql);
        IEnumerable<MikroStokRenk> GetRenkListWhere(string wheresql); 
        IEnumerable<MikroStokBeden> GetBedenListWhere(string wheresql);
        MikroStokRenk GetRenkByKodu(string renkKodu);
        MikroStokBeden GetBedenByKodu(string bedenKodu); 
        MikroStokRenk GetRenkByStokKodu(string stokKodu);
        MikroStokBeden GetBedenByStokKodu(string stokKodu);
        List<string> GetStokKategoriler();
    }
}