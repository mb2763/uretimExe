using My.Core;
using My.Core.Data;
using System; 

namespace My.Entities.Mikro {

    [Table("PARTILOT")]
    public class MikroPartiLot {

        [Key]
        public Guid? pl_Guid { get; set; }
        public short pl_DBCno { get; set; }
        public int pl_SpecRECno { get; set; }
        public bool pl_iptal { get; set; }
        public short pl_fileid { get; set; }
        public bool pl_hidden { get; set; }
        public bool pl_kilitli { get; set; }
        public bool pl_degisti { get; set; }
        public int pl_checksum { get; set; }
        public short? pl_create_user { get; set; }
        public DateTime pl_create_date { get; set; }
        public short? pl_lastup_user { get; set; }
        public DateTime? pl_lastup_date { get; set; }
        public string pl_ozelkod1 { get; set; }
        public string pl_ozelkod2 { get; set; }
        public string pl_ozelkod3 { get; set; }
        public string pl_partikodu { get; set; }
        public int? pl_lotno { get; set; }
        public string pl_stokkodu { get; set; }
        public string pl_aciklama { get; set; }
        public double pl_olckalkdeg_deg1 { get; set; }
        public double pl_olckalkdeg_deg2 { get; set; }
        public double pl_olckalkdeg_deg3 { get; set; }
        public double pl_olckalkdeg_deg4 { get; set; }
        public double pl_olckalkdeg_deg5 { get; set; }
        public double pl_olckalkdeg_deg6 { get; set; }
        public double pl_olckalkdeg_deg7 { get; set; }
        public double pl_olckalkdeg_deg8 { get; set; }
        public double pl_olckalkdeg_deg9 { get; set; }
        public double pl_olckalkdeg_deg10 { get; set; }
        public string pl_olckalkdeg_aciklama1 { get; set; }
        public string pl_olckalkdeg_aciklama2 { get; set; }
        public string pl_olckalkdeg_aciklama3 { get; set; }
        public string pl_olckalkdeg_aciklama4 { get; set; }
        public string pl_olckalkdeg_aciklama5 { get; set; }
        public string pl_olckalkdeg_aciklama6 { get; set; }
        public string pl_olckalkdeg_aciklama7 { get; set; }
        public string pl_olckalkdeg_aciklama8 { get; set; }
        public string pl_olckalkdeg_aciklama9 { get; set; }
        public string pl_olckalkdeg_aciklama10 { get; set; }
        public DateTime? pl_son_kullanim_tar { get; set; }
        public double pl_DaraliKilo { get; set; }
        public double pl_SafiKilo { get; set; }
        public double pl_En { get; set; }
        public double pl_Boy { get; set; }
        public double pl_Yukseklik { get; set; }
        public double pl_OzgulAgirlik { get; set; }
        public string pl_kod1 { get; set; }
        public string pl_kod2 { get; set; }
        public string pl_kod3 { get; set; }
        public string pl_kod4 { get; set; }
        public string pl_kod5 { get; set; }
        public string pl_kod6 { get; set; }
        public string pl_kod7 { get; set; }
        public string pl_kod8 { get; set; }
        public string pl_kod9 { get; set; }
        public string pl_kod10 { get; set; }
        public DateTime? pl_uretim_tar { get; set; }     
        public MikroPartiLot() {           
            pl_partikodu = "";
            pl_lotno = 0;
            pl_stokkodu = "";
            pl_aciklama = "";
            pl_create_user = 995;
            pl_lastup_user = 995;
            pl_create_date = DateTime.Now;
            pl_lastup_date = pl_create_date;
            pl_uretim_tar = pl_create_date.Date;
            pl_son_kullanim_tar = pl_create_date.Date.AddYears(1); 
            /**/
            pl_Guid = MyGuid.NewGuid();
            pl_fileid = 153;
            Temizle();
        }

        private void Temizle() { 
            pl_DBCno = 0;
            pl_SpecRECno = 0;
            pl_iptal = false; 
            pl_hidden = false;
            pl_kilitli = false;
            pl_degisti = false;
            pl_checksum = 0 ; 
            pl_ozelkod1 =""  ;
            pl_ozelkod2 =""  ;
            pl_ozelkod3 =""  ; 
            pl_olckalkdeg_deg1 = 0 ;
            pl_olckalkdeg_deg2 = 0 ;
            pl_olckalkdeg_deg3 = 0 ;
            pl_olckalkdeg_deg4 = 0 ;
            pl_olckalkdeg_deg5 = 0 ;
            pl_olckalkdeg_deg6 = 0 ;
            pl_olckalkdeg_deg7 = 0 ;
            pl_olckalkdeg_deg8 = 0 ;
            pl_olckalkdeg_deg9 = 0 ;
            pl_olckalkdeg_deg10 =0  ;
            pl_olckalkdeg_aciklama1 = "" ;
            pl_olckalkdeg_aciklama2 = "" ;
            pl_olckalkdeg_aciklama3 = "" ;
            pl_olckalkdeg_aciklama4 = "" ;
            pl_olckalkdeg_aciklama5 = "" ;
            pl_olckalkdeg_aciklama6 = "" ;
            pl_olckalkdeg_aciklama7 = "" ;
            pl_olckalkdeg_aciklama8 = "" ;
            pl_olckalkdeg_aciklama9 = "" ;
            pl_olckalkdeg_aciklama10 = "" ; 
            pl_DaraliKilo =  0;
            pl_SafiKilo =  0;
            pl_En = 0 ;
            pl_Boy =  0;
            pl_Yukseklik = 0 ;
            pl_OzgulAgirlik = 0 ;
            pl_kod1 = "" ;
            pl_kod2 = "" ;
            pl_kod3 = "" ;
            pl_kod4 = "" ;
            pl_kod5 = "" ;
            pl_kod6 = "" ;
            pl_kod7 = "" ;
            pl_kod8 = "" ;
            pl_kod9 = "" ;
            pl_kod10 ="" ; 
        }


        public static string GetInsertSqlCode() {
             
            string sql = @"  IF EXISTS
  (SELECT * FROM  PARTILOT  WHERE pl_partikodu = @pl_partikodu and pl_lotno = @pl_lotno and pl_stokkodu=@pl_stokkodu)
    UPDATE  PARTILOT SET
 pl_DBCno                = @pl_DBCno                ,
 pl_SpecRECno            = @pl_SpecRECno            ,
 pl_iptal                = @pl_iptal                ,
 pl_fileid               = @pl_fileid               ,
 pl_hidden               = @pl_hidden               ,
 pl_kilitli              = @pl_kilitli              ,
 pl_degisti              = @pl_degisti              ,
 pl_checksum             = @pl_checksum             ,
 pl_create_user          = @pl_create_user          ,
 pl_create_date          = @pl_create_date          ,
 pl_lastup_user          = @pl_lastup_user          ,
 pl_lastup_date          = @pl_lastup_date          ,
 pl_ozelkod1             = @pl_ozelkod1             ,
 pl_ozelkod2             = @pl_ozelkod2             ,
 pl_ozelkod3             = @pl_ozelkod3             ,
 pl_partikodu            = @pl_partikodu            ,
 pl_lotno                = @pl_lotno                ,
 pl_stokkodu             = @pl_stokkodu             ,
 pl_aciklama             = @pl_aciklama             ,
 pl_olckalkdeg_deg1      = @pl_olckalkdeg_deg1      ,
 pl_olckalkdeg_deg2      = @pl_olckalkdeg_deg2      ,
 pl_olckalkdeg_deg3      = @pl_olckalkdeg_deg3      ,
 pl_olckalkdeg_deg4      = @pl_olckalkdeg_deg4      ,
 pl_olckalkdeg_deg5      = @pl_olckalkdeg_deg5      ,
 pl_olckalkdeg_deg6      = @pl_olckalkdeg_deg6      ,
 pl_olckalkdeg_deg7      = @pl_olckalkdeg_deg7      ,
 pl_olckalkdeg_deg8      = @pl_olckalkdeg_deg8      ,
 pl_olckalkdeg_deg9      = @pl_olckalkdeg_deg9      ,
 pl_olckalkdeg_deg10     = @pl_olckalkdeg_deg10     ,
 pl_olckalkdeg_aciklama1 = @pl_olckalkdeg_aciklama1 ,
 pl_olckalkdeg_aciklama2 = @pl_olckalkdeg_aciklama2 ,
 pl_olckalkdeg_aciklama3 = @pl_olckalkdeg_aciklama3 ,
 pl_olckalkdeg_aciklama4 = @pl_olckalkdeg_aciklama4 ,
 pl_olckalkdeg_aciklama5 = @pl_olckalkdeg_aciklama5 ,
 pl_olckalkdeg_aciklama6 = @pl_olckalkdeg_aciklama6 ,
 pl_olckalkdeg_aciklama7 = @pl_olckalkdeg_aciklama7 ,
 pl_olckalkdeg_aciklama8 = @pl_olckalkdeg_aciklama8 ,
 pl_olckalkdeg_aciklama9 = @pl_olckalkdeg_aciklama9 ,
 pl_olckalkdeg_aciklama10= @pl_olckalkdeg_aciklama10, 
 pl_DaraliKilo           = @pl_DaraliKilo           ,
 pl_SafiKilo             = @pl_SafiKilo             ,
 pl_En                   = @pl_En                   ,
 pl_Boy                  = @pl_Boy                  ,
 pl_Yukseklik            = @pl_Yukseklik            ,
 pl_OzgulAgirlik         = @pl_OzgulAgirlik         ,
 pl_kod1                 = @pl_kod1                 ,
 pl_kod2                 = @pl_kod2                 ,
 pl_kod3                 = @pl_kod3                 ,
 pl_kod4                 = @pl_kod4                 ,
 pl_kod5                 = @pl_kod5                 ,
 pl_kod6                 = @pl_kod6                 ,
 pl_kod7                 = @pl_kod7                 ,
 pl_kod8                 = @pl_kod8                 ,
 pl_kod9                 = @pl_kod9                 ,
 pl_kod10                = @pl_kod10                ,
 pl_son_kullanim_tar     = @pl_son_kullanim_tar     ,
 pl_uretim_tar           = @pl_uretim_tar            

    WHERE  pl_partikodu = @pl_partikodu and pl_lotno = @pl_lotno and pl_stokkodu=@pl_stokkodu
  ELSE
    INSERT INTO  PARTILOT  (
 pl_Guid                 ,
 pl_DBCno                ,
 pl_SpecRECno            ,
 pl_iptal                ,
 pl_fileid               ,
 pl_hidden               ,
 pl_kilitli              ,
 pl_degisti              ,
 pl_checksum             ,
 pl_create_user          ,
 pl_create_date          ,
 pl_lastup_user          ,
 pl_lastup_date          ,
 pl_ozelkod1             ,
 pl_ozelkod2             ,
 pl_ozelkod3             ,
 pl_partikodu            ,
 pl_lotno                ,
 pl_stokkodu             ,
 pl_aciklama             ,
 pl_olckalkdeg_deg1      ,
 pl_olckalkdeg_deg2      ,
 pl_olckalkdeg_deg3      ,
 pl_olckalkdeg_deg4      ,
 pl_olckalkdeg_deg5      ,
 pl_olckalkdeg_deg6      ,
 pl_olckalkdeg_deg7      ,
 pl_olckalkdeg_deg8      ,
 pl_olckalkdeg_deg9      ,
 pl_olckalkdeg_deg10     ,
 pl_olckalkdeg_aciklama1 ,
 pl_olckalkdeg_aciklama2 ,
 pl_olckalkdeg_aciklama3 ,
 pl_olckalkdeg_aciklama4 ,
 pl_olckalkdeg_aciklama5 ,
 pl_olckalkdeg_aciklama6 ,
 pl_olckalkdeg_aciklama7 ,
 pl_olckalkdeg_aciklama8 ,
 pl_olckalkdeg_aciklama9 ,
 pl_olckalkdeg_aciklama10, 
 pl_DaraliKilo           ,
 pl_SafiKilo             ,
 pl_En                   ,
 pl_Boy                  ,
 pl_Yukseklik            ,
 pl_OzgulAgirlik         ,
 pl_kod1                 ,
 pl_kod2                 ,
 pl_kod3                 ,
 pl_kod4                 ,
 pl_kod5                 ,
 pl_kod6                 ,
 pl_kod7                 ,
 pl_kod8                 ,
 pl_kod9                 ,
 pl_kod10                ,
 pl_son_kullanim_tar     ,
 pl_uretim_tar           )
    VALUES(
 @pl_Guid                 ,
 @pl_DBCno                ,
 @pl_SpecRECno            ,
 @pl_iptal                ,
 @pl_fileid               ,
 @pl_hidden               ,
 @pl_kilitli              ,
 @pl_degisti              ,
 @pl_checksum             ,
 @pl_create_user          ,
 @pl_create_date          ,
 @pl_lastup_user          ,
 @pl_lastup_date          ,
 @pl_ozelkod1             ,
 @pl_ozelkod2             ,
 @pl_ozelkod3             ,
 @pl_partikodu            ,
 @pl_lotno                ,
 @pl_stokkodu             ,
 @pl_aciklama             ,
 @pl_olckalkdeg_deg1      ,
 @pl_olckalkdeg_deg2      ,
 @pl_olckalkdeg_deg3      ,
 @pl_olckalkdeg_deg4      ,
 @pl_olckalkdeg_deg5      ,
 @pl_olckalkdeg_deg6      ,
 @pl_olckalkdeg_deg7      ,
 @pl_olckalkdeg_deg8      ,
 @pl_olckalkdeg_deg9      ,
 @pl_olckalkdeg_deg10     ,
 @pl_olckalkdeg_aciklama1 ,
 @pl_olckalkdeg_aciklama2 ,
 @pl_olckalkdeg_aciklama3 ,
 @pl_olckalkdeg_aciklama4 ,
 @pl_olckalkdeg_aciklama5 ,
 @pl_olckalkdeg_aciklama6 ,
 @pl_olckalkdeg_aciklama7 ,
 @pl_olckalkdeg_aciklama8 ,
 @pl_olckalkdeg_aciklama9 ,
 @pl_olckalkdeg_aciklama10, 
 @pl_DaraliKilo           ,
 @pl_SafiKilo             ,
 @pl_En                   ,
 @pl_Boy                  ,
 @pl_Yukseklik            ,
 @pl_OzgulAgirlik         ,
 @pl_kod1                 ,
 @pl_kod2                 ,
 @pl_kod3                 ,
 @pl_kod4                 ,
 @pl_kod5                 ,
 @pl_kod6                 ,
 @pl_kod7                 ,
 @pl_kod8                 ,
 @pl_kod9                 ,
 @pl_kod10                ,
 @pl_son_kullanim_tar     ,
 @pl_uretim_tar           );";
            return sql;
        }

    }
}
