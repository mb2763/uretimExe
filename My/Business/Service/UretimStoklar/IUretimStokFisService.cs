using My.Core.Data;
using My.Core.Result;
using My.Entities.DepoStoklar;
using My.Entities.UretimStoklar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace My.Business.Service.UretimStoklar
{
    public interface IUretimStokFisService : IBaseService<UretimStokFis>
    {
        IDataResult<UretimStokFisModel> GetFis(Guid id);
        IDataResult<UretimStokFisModel> GetFisFirst(string sor);
        IDataResult<List<UretimStokFisModel>> GetFisList(string sor = "");
        IDataResult<List<UretimStokFisHareket>> GetStokHareketList(string sor = "");
        IDataResult<UretimStokFisHareket> GetStokHareketFirst(string sor);
        IDataResult<List<UretimStokFisHareket>> GetStokHareketByFisId(Guid? fisId);
        IDataResult<string> FisOnaySave(Guid? fisId, string durumu, string usercode);  
        IDataResult<string> FisSil(Guid? fisId   );
        IDataResult<string> StokKoduGuncelle(string eskiKod, string yeniKod);
        }
}
