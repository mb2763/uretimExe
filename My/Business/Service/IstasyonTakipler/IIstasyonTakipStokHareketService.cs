using My.Core.Data;
using My.Core.Result;
using My.Entities.IstasyonTakipler;
using My.Entities.Models;
using System;
using System.Collections.Generic;

namespace My.Business.Service.IstasyonTakipler {
    public interface IIstasyonTakipStokHareketService : IBaseService<IstasyonTakipStokHareket> {
        IDataResult<IEnumerable<IstasyonTakipStokHareket>> GetViewListWhere(string whereSql);
        IDataResult<IEnumerable<IstasyonTakipStokHareketKullanilan>> GetViewListKullanimWhere(string andwhereSql);
        IDataResult<IEnumerable<IstasyonTakipStokHareketKullanilan>> GetViewListKullanimWherePartiLot(string andwhereSql);
        IDataResult<IEnumerable<IstasyonTakipStokHareketKullanilan>> GetViewListKullanimWhereMalKabul(string andwhereSql);
        IDataResult<IEnumerable<MalKabulFisKullanilanStokModel>> GetViewListKullanimMalKabulFis(Guid? sipid);
        IDataResult<IEnumerable<IstasyonTakipStokHareket>> GetStokHareketByUrIId(Guid urIId);
    }
}
