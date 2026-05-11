using My.Core.Data;
using My.Core.Result;
using My.Entities.Personeller;

namespace My.Business.Service.Personeller
{
    public interface IPersonelService : IBaseService<Personel>
    {
        string Sifrele64(string yazi1, string yazihash = "mikroconnect");
        IDataResult<string> PersonelKaydet(Personel personel, bool yenikayitmi);
        IDataResult<string> PersonelKaydetSifresiz(Personel personel, bool yenikayitmi);



    }
}