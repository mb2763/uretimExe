using My.Core.Data; 
using My.Core.Result;
using My.Entities.Templer; 
using System.Collections.Generic; 

namespace My.Business.Service.Templer {
    public interface ITempMikroStokService   : IBaseService<TempMikroStok>  {

        IDataResult<IEnumerable<TempMikroStok>> MikroStokGuncelle(string mikroDb);
        IDataResult<IEnumerable<TempMikroStokKategori>> MikroStokKategoriGuncelle(string mikroDb);
        IDataResult<IEnumerable<TempMikroStokKategori>> GetStokKategoriListKaliteKontrol( );
        IDataResult<IEnumerable<TempMikroStokKategori>> GetStokKategoriListStokKategori( );
        IDataResult<IEnumerable<TempMikroStokKategori>> GetStokReyonList( );
      
    }
}
