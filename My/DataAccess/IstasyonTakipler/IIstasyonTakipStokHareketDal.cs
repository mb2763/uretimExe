using My.Core.Data;
using My.Entities.IstasyonTakipler;
using My.Entities.Models;
using System;
using System.Collections.Generic;

namespace My.DataAccess.IstasyonTakipler
{
    public interface IIstasyonTakipStokHareketDal : IBaseDal<IstasyonTakipStokHareket>
    {

        IEnumerable<IstasyonTakipStokHareket> GetViewListWhere(string whereSql);
        IEnumerable<IstasyonTakipStokHareketKullanilan> GetViewListKullanimWhere(string andwhereSql);
        IEnumerable<IstasyonTakipStokHareketKullanilan> GetViewListKullanimWherePartiLot(string andwhereSql);
        IEnumerable<IstasyonTakipStokHareketKullanilan> GetViewListKullanimWhereMalKabul(string andwhereSql);
        IEnumerable<MalKabulFisKullanilanStokModel> GetViewListKullanimMalKabulFis(Guid? sipid);
        IEnumerable<IstasyonTakipStokHareket> GetStokHareketByUrIId(Guid urIId);
    }
}
