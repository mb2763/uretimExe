using My.Entities.IstasyonTakipler;
using My.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.MikroModul {
    public class MikroyaKaydetMalKabulHesaplama {
        public List<MalKabulFisKullanilanStokModel> MalKabulFisOrj;
        public List<MalKabulFisKullanilanStokModel> MalKabulFis;
        public List<MalKabulFisKullanilanStokModel> MalKabulFisKalan;
        public List<IstasyonTakipStokHareketKullanilan> IstasyonHareket;
        List<IstasyonTakipStokHareketKullanilan> Hesaplanan;
        List<IstasyonTakipHareketDetay> PartiliFireler;
        public MikroyaKaydetMalKabulHesaplama(List<MalKabulFisKullanilanStokModel> malKabulFis, List<IstasyonTakipStokHareketKullanilan> istasyonHareket,List<IstasyonTakipHareketDetay> partiliFireler) {
            IstasyonHareket = istasyonHareket;
            MalKabulFisOrj = malKabulFis;
            PartiliFireler = partiliFireler;
            MalKabulFis = new List<MalKabulFisKullanilanStokModel>();
            foreach (var itm in MalKabulFisOrj) {
                MalKabulFis.Add(itm.Clone());
            }
        }

        public List<IstasyonTakipStokHareketKullanilan> Convert() {
            MalKabulFis = new List<MalKabulFisKullanilanStokModel>();
            MalKabulFisKalan = new List<MalKabulFisKullanilanStokModel>();
            foreach (var itm in MalKabulFisOrj) {
                MalKabulFis.Add(itm.Clone());
            }
            Hesaplanan = new List<IstasyonTakipStokHareketKullanilan>();

            /*Partili Fireleri Düş */
           
            foreach (var pf in PartiliFireler)
            {
                double pfKalan = pf.Miktar;
                if (pf.Parti.IsNullOrEmpty())  pf.Parti = ""; 
                if (pf.Lot.IsNullOrEmpty()) pf.Lot = "0"; 
                foreach (var f in MalKabulFis)
                {
                    if (f.PartiNo.IsNullOrEmpty())  f.PartiNo = ""; 
                    if (f.LotNo.IsNullOrEmpty()) f.LotNo = "0";  
                    if (pf.StokKodu==f.StokKodu && pf.Parti == f.PartiNo && pf.Lot == f.LotNo  )
                    {
                        if (pfKalan<=f.Miktar)
                        {
                            f.Miktar = f.Miktar - pf.Miktar;
                            pfKalan = 0;
                            break;
                        }
                        else
                        { 
                            pfKalan = pf.Miktar - f.Miktar;
                            f.Miktar = 0; 
                        }
                    }
                   
                } 
            }

            /**/
            double _kalanMiktar = 0;
            double H_Miktar = 0;
            double H_FireMiktar = 0;
            double H_İptalMiktar = 0;
            foreach (var u in IstasyonHareket) {
                _kalanMiktar = u.StokMiktar + u.StokFireMiktar;
                while (_kalanMiktar > 0) {
                    foreach (var f in MalKabulFis) {
                        if (u.StokKodu == f.StokKodu && u.Birim == f.Birimi && f.Miktar > 0) {
                            if (f.Miktar == _kalanMiktar) {
                                H_Miktar = _kalanMiktar - u.StokFireMiktar;
                                H_FireMiktar = u.StokFireMiktar;
                                H_İptalMiktar = u.StokIptalMiktar;
                                var upd = u.Clone();
                                upd.StokMiktar = H_Miktar;
                                upd.StokFireMiktar = H_FireMiktar;
                                upd.StokIptalMiktar = H_İptalMiktar;
                                upd.Parti = f.PartiNo;
                                upd.Lot = f.LotNo;
                                Hesaplanan.Add(upd);
                                f.Miktar = 0;
                                _kalanMiktar = 0;
                            }
                            else if (f.Miktar > _kalanMiktar) {
                                H_Miktar = _kalanMiktar - u.StokFireMiktar;
                                H_FireMiktar = u.StokFireMiktar;
                                H_İptalMiktar = u.StokIptalMiktar;
                                var upd = u.Clone();
                                upd.StokMiktar = H_Miktar;
                                upd.StokFireMiktar = H_FireMiktar;
                                upd.StokIptalMiktar = H_İptalMiktar;
                                upd.Parti = f.PartiNo;
                                upd.Lot = f.LotNo;
                                Hesaplanan.Add(upd);
                                f.Miktar = f.Miktar - _kalanMiktar;
                                _kalanMiktar = 0;
                            }
                            else if (f.Miktar < _kalanMiktar) {
                                H_FireMiktar = (f.Miktar / u.UretimMiktari) * u.UretimFireMiktari;
                                H_Miktar = f.Miktar - H_FireMiktar;
                                H_İptalMiktar = (f.Miktar / u.UretimMiktari) * u.UretimIptalMiktari;
                                var upd = u.Clone();
                                upd.StokMiktar = H_Miktar;
                                upd.StokFireMiktar = H_FireMiktar;
                                upd.StokIptalMiktar = H_İptalMiktar;
                                upd.Parti = f.PartiNo;
                                upd.Lot = f.LotNo;
                                Hesaplanan.Add(upd);
                                _kalanMiktar = _kalanMiktar - f.Miktar;
                                f.Miktar = 0;
                            }
                        }

                    }
                    if (_kalanMiktar > 0) {
                        H_Miktar = _kalanMiktar;
                        H_FireMiktar = (H_Miktar / u.UretimMiktari) * u.UretimFireMiktari;
                        H_İptalMiktar = (H_Miktar / u.UretimMiktari) * u.UretimIptalMiktari;
                        var upd = u.Clone();
                        upd.StokMiktar = H_Miktar;
                        upd.StokFireMiktar = H_FireMiktar;
                        upd.StokIptalMiktar = H_İptalMiktar;
                        Hesaplanan.Add(upd);
                        _kalanMiktar = 0;
                    }
                }

            }
            foreach (var itm in MalKabulFis) {
                if (itm.Miktar > 0) {
                    MalKabulFisKalan.Add(itm.Clone());
                }
            }
            return Hesaplanan;
        }

        public List<MalKabulFisKullanilanStokModel> GetKalanList() {
            return MalKabulFisKalan.ToList();
        }
    }
    public class MikroyaKaydetMalKabulFireHesaplama {

        public List<MalKabulFisKullanilanStokModel> MalKabulKalanOrj;
        public List<MalKabulFisKullanilanStokModel> MalKabulKalan;
        public List<MalKabulFisKullanilanStokModel> MalKabulStokOrj;
        public List<MalKabulFisKullanilanStokModel> MalKabulStok;
        public List<IstasyonTakipHareketDetay> FireFisOrj;
        public List<IstasyonTakipHareketDetay> Hesaplanan;

        public MikroyaKaydetMalKabulFireHesaplama(List<MalKabulFisKullanilanStokModel> _MalKabulKalanStok, List<MalKabulFisKullanilanStokModel> _MalKabulStok, List<IstasyonTakipHareketDetay> _listFire) {
            FireFisOrj = _listFire;
            MalKabulKalanOrj = _MalKabulKalanStok;
            MalKabulStokOrj = _MalKabulStok; 
        }

        public List<IstasyonTakipHareketDetay> Convert() {
            Hesaplanan = new List<IstasyonTakipHareketDetay>();
            MalKabulKalan = new List<MalKabulFisKullanilanStokModel>();
            foreach (var itm in MalKabulKalanOrj) {
                MalKabulKalan.Add(itm.Clone());
            }
            MalKabulStok = new List<MalKabulFisKullanilanStokModel>();
            foreach (var itm in MalKabulStokOrj) {
                MalKabulStok.Add(itm.Clone());
            }
            double _kalanMiktar = 0;
            double H_FireMiktar = 0;
            foreach (var u in FireFisOrj) {
                _kalanMiktar = u.FireMiktar; 
                if (!string.IsNullOrEmpty(u.Parti))
                {
                    var upd = u.Clone();
                    Hesaplanan.Add(upd);
                    _kalanMiktar = 0;
                    continue;
                }

                while (_kalanMiktar > 0) {
                    foreach (var f in MalKabulKalan) {
                       
                        if (u.StokKodu == f.StokKodu && u.Birim == f.Birimi && f.Miktar > 0) {
                            if (f.Miktar == _kalanMiktar) {
                                H_FireMiktar = u.FireMiktar;
                                var upd = u.Clone();
                                upd.FireMiktar = H_FireMiktar;
                                upd.Parti = f.PartiNo;
                                upd.Lot = f.LotNo;
                                Hesaplanan.Add(upd);
                                f.Miktar = 0;
                                _kalanMiktar = 0;
                            }
                            else if (f.Miktar > _kalanMiktar) {
                                H_FireMiktar = _kalanMiktar;
                                var upd = u.Clone();
                                upd.FireMiktar = H_FireMiktar;
                                upd.Parti = f.PartiNo;
                                upd.Lot = f.LotNo;
                                Hesaplanan.Add(upd);
                                f.Miktar = f.Miktar - _kalanMiktar;
                                _kalanMiktar = 0;
                            }
                            else if (f.Miktar < _kalanMiktar) {
                                H_FireMiktar = f.Miktar;
                                var upd = u.Clone();
                                upd.FireMiktar = H_FireMiktar;
                                upd.Parti = f.PartiNo;
                                upd.Lot = f.LotNo;
                                Hesaplanan.Add(upd);
                                _kalanMiktar = _kalanMiktar - f.Miktar;
                                f.Miktar = 0;
                            }
                        }

                    }
                    if (_kalanMiktar > 0) {
                        foreach (var fk in MalKabulStok) {
                            if (_kalanMiktar <= 0) {
                                break;
                            }
                            if (u.StokKodu == fk.StokKodu && u.Birim == fk.Birimi && fk.Miktar > 0) {
                                if (fk.Miktar == _kalanMiktar) {
                                    H_FireMiktar = u.FireMiktar;
                                    var upd = u.Clone();
                                    upd.FireMiktar = H_FireMiktar;
                                    upd.Parti = fk.PartiNo;
                                    upd.Lot = fk.LotNo;
                                    Hesaplanan.Add(upd);
                                    fk.Miktar = 0;
                                    _kalanMiktar = 0;
                                }
                                else if (fk.Miktar > _kalanMiktar) {
                                    H_FireMiktar = _kalanMiktar;
                                    var upd = u.Clone();
                                    upd.FireMiktar = H_FireMiktar;
                                    upd.Parti = fk.PartiNo;
                                    upd.Lot = fk.LotNo;
                                    Hesaplanan.Add(upd);
                                    fk.Miktar = fk.Miktar - _kalanMiktar;
                                    _kalanMiktar = 0;
                                }
                                else if (fk.Miktar < _kalanMiktar) {
                                    H_FireMiktar = fk.Miktar;
                                    var upd = u.Clone();
                                    upd.FireMiktar = H_FireMiktar;
                                    upd.Parti = fk.PartiNo;
                                    upd.Lot = fk.LotNo;
                                    Hesaplanan.Add(upd);
                                    _kalanMiktar = _kalanMiktar - fk.Miktar;
                                    fk.Miktar = 0;
                                }
                            }
                        }

                    }
                    if (_kalanMiktar > 0) {
                        foreach (var fk in MalKabulStok) {
                            if (_kalanMiktar <= 0) {
                                break;
                            }
                            if (u.StokKodu == fk.StokKodu && u.Birim == fk.Birimi) {
                                H_FireMiktar = _kalanMiktar;
                                var upd = u.Clone();
                                upd.FireMiktar = H_FireMiktar;
                                upd.Parti = fk.PartiNo;
                                upd.Lot = fk.LotNo;
                                Hesaplanan.Add(upd);
                                _kalanMiktar = 0;
                            }
                        }
                    }
                    if (_kalanMiktar > 0) {
                        foreach (var fk in MalKabulStok) {
                            if (_kalanMiktar <= 0) {
                                break;
                            }
                            if (u.StokKodu == fk.StokKodu) {
                                H_FireMiktar = _kalanMiktar;
                                var upd = u.Clone();
                                upd.FireMiktar = H_FireMiktar;
                                upd.Parti = fk.PartiNo;
                                upd.Lot = fk.LotNo;
                                Hesaplanan.Add(upd);
                                _kalanMiktar = 0;
                            }
                        }
                    }

                }

            }
            return Hesaplanan;
        }

    }
}
