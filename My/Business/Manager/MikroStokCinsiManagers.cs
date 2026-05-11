using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace My.Business.Manager {
    public enum MikroStokCinsiEnum {
        TicariMal,
        IlkMadde,
        AraMamul,
        YariMamul,
        Mamul,
        YanMamul,
        IsletmeMalzemesi,
        TuketimMalzemesi,
        YedekParca,
        AkaryakitStok,
        MontajReçeteliMamul,
        TemelHammadde ,
        Yok
    }

    public class MikroStokCinsi {
         
        public MikroStokCinsiEnum StokCinsi { get; set; } 
        public int Kodu { get; set; } 
        public string Adi { get; set; } 
    }

    public static class MikroStokCinsiManager {

        public static List<MikroStokCinsi> GetCinsListFull() {
            List<MikroStokCinsi> lis = new List<MikroStokCinsi>();
            lis.Add(new MikroStokCinsi() { StokCinsi = MikroStokCinsiEnum.Yok, Kodu = -1, Adi = "" });
            lis.Add( new MikroStokCinsi() { StokCinsi=MikroStokCinsiEnum.TicariMal,Kodu=0,Adi= "Ticari Mal" });
            lis.Add( new MikroStokCinsi() { StokCinsi=MikroStokCinsiEnum.IlkMadde, Kodu=1,Adi= "İlk Madde" });
            lis.Add( new MikroStokCinsi() { StokCinsi=MikroStokCinsiEnum.AraMamul, Kodu=2,Adi= "Ara Mamül" });
            lis.Add( new MikroStokCinsi() { StokCinsi=MikroStokCinsiEnum.YariMamul, Kodu=3,Adi= "Yarı Mamül" });
            lis.Add( new MikroStokCinsi() { StokCinsi=MikroStokCinsiEnum.Mamul, Kodu=4,Adi= "Mamül" });
            lis.Add( new MikroStokCinsi() { StokCinsi=MikroStokCinsiEnum.YanMamul, Kodu=5,Adi= "Yan Mamül" });
            lis.Add( new MikroStokCinsi() { StokCinsi=MikroStokCinsiEnum.IsletmeMalzemesi, Kodu=6,Adi= "İşletme Malzemesi" });
            lis.Add( new MikroStokCinsi() { StokCinsi=MikroStokCinsiEnum.TuketimMalzemesi, Kodu=7,Adi= "Tüketim Malzemesi" });
            lis.Add( new MikroStokCinsi() { StokCinsi=MikroStokCinsiEnum.YedekParca, Kodu=8,Adi= "Yedek Parça" });
            lis.Add( new MikroStokCinsi() { StokCinsi=MikroStokCinsiEnum.AkaryakitStok, Kodu=9,Adi= "Akaryakıt Stok" });
            lis.Add( new MikroStokCinsi() { StokCinsi=MikroStokCinsiEnum.MontajReçeteliMamul, Kodu=10,Adi= "Montaj Reçeteli Mamül" });
            lis.Add( new MikroStokCinsi() { StokCinsi=MikroStokCinsiEnum.TemelHammadde, Kodu=11,Adi= "Temel Hammadde" });  
            return lis;
        }
        public static List<MikroStokCinsi> GetCinsList() {
            List<MikroStokCinsi> lis = new List<MikroStokCinsi>();
            lis.Add(new MikroStokCinsi() { StokCinsi = MikroStokCinsiEnum.Yok, Kodu = -1, Adi = "" }); 
            lis.Add(new MikroStokCinsi() { StokCinsi = MikroStokCinsiEnum.TicariMal, Kodu = 0, Adi = "Ticari Mal" }); 
            lis.Add(new MikroStokCinsi() { StokCinsi = MikroStokCinsiEnum.AraMamul, Kodu = 2, Adi = "Ara Mamül" });
            lis.Add(new MikroStokCinsi() { StokCinsi = MikroStokCinsiEnum.YariMamul, Kodu = 3, Adi = "Yarı Mamül" });
            lis.Add(new MikroStokCinsi() { StokCinsi = MikroStokCinsiEnum.Mamul, Kodu = 4, Adi = "Mamül" });
            lis.Add(new MikroStokCinsi() { StokCinsi = MikroStokCinsiEnum.YanMamul, Kodu = 5, Adi = "Yan Mamül" }); 
            lis.Add(new MikroStokCinsi() { StokCinsi = MikroStokCinsiEnum.TemelHammadde, Kodu = 11, Adi = "Temel Hammadde" });
            return lis;
        }
        /*
         
        0:Ticari Mal 1:İlk Madde 2:Ara Mamül
3:Yarı Mamül 4:Mamül 5:Yan Mamül
6:İşletme Malzemesi 7:Tüketim Malzemesi 8:Yedek Parça
9:Akaryakıt Stok 10:Montaj Reçeteli Mamül 11:Temel Hammadde

         */
    }

}
