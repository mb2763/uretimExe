using My.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Entities.Mikro {
    [Table("BEDEN_HAREKETLERI")]
    public partial class MikroBedenHareketleri {
        [Key]
        public  Guid BdnHar_Guid { get; set; }
        public short BdnHar_DBCno { get; set; }
        public int BdnHar_Spec_Rec_no { get; set; }
        public bool BdnHar_iptal { get; set; }
        public short BdnHar_fileid { get; set; }
        public bool BdnHar_hidden { get; set; }
        public bool BdnHar_kilitli { get; set; }
        public bool BdnHar_degisti { get; set; }
        public int BdnHar_checksum { get; set; }
        public short BdnHar_create_user { get; set; }
        public DateTime? BdnHar_create_date { get; set; }
        public short BdnHar_lastup_user { get; set; }
        public DateTime? BdnHar_lastup_date { get; set; }
        public string BdnHar_special1 { get; set; }
        public string BdnHar_special2 { get; set; }
        public string BdnHar_special3 { get; set; }
        public byte BdnHar_Tipi { get; set; }
        public Guid? BdnHar_Har_uid { get; set; }
        public short BdnHar_BedenNo { get; set; }
        public double? BdnHar_HarGor { get; set; }
        public double? BdnHar_KnsIsGor { get; set; }
        public double? BdnHar_KnsFat { get; set; }
        public double? BdnHar_TesMik { get; set; }
        public double? BdnHar_rezervasyon_miktari { get; set; }
        public double? BdnHar_rezerveden_teslim_edilen { get; set; }


        public static string GetInsertSqlCode() {
            string sql = @"  IF EXISTS
  (SELECT * FROM  BEDEN_HAREKETLERI  WHERE BdnHar_Guid = @BdnHar_Guid )
    UPDATE  BEDEN_HAREKETLERI SET
    BdnHar_DBCno                     = @BdnHar_DBCno                       ,
    BdnHar_Spec_Rec_no               = @BdnHar_Spec_Rec_no                 ,
    BdnHar_iptal                     = @BdnHar_iptal                       ,
    BdnHar_fileid                    = @BdnHar_fileid                      ,
    BdnHar_hidden                    = @BdnHar_hidden                      ,
    BdnHar_kilitli                   = @BdnHar_kilitli                     ,
    BdnHar_degisti                   = @BdnHar_degisti                     ,
    BdnHar_checksum                  = @BdnHar_checksum                    ,
    BdnHar_create_user               = @BdnHar_create_user                 ,
    BdnHar_create_date               = @BdnHar_create_date                 ,
    BdnHar_lastup_user               = @BdnHar_lastup_user                 ,
    BdnHar_lastup_date               = @BdnHar_lastup_date                 ,
    BdnHar_special1                  = @BdnHar_special1                    ,
    BdnHar_special2                  = @BdnHar_special2                    ,
    BdnHar_special3                  = @BdnHar_special3                    ,
    BdnHar_Tipi                      = @BdnHar_Tipi                        ,
    BdnHar_Har_uid                   = @BdnHar_Har_uid                     ,
    BdnHar_BedenNo                   = @BdnHar_BedenNo                     ,
    BdnHar_HarGor                    = @BdnHar_HarGor                      ,
    BdnHar_KnsIsGor                  = @BdnHar_KnsIsGor                    ,
    BdnHar_KnsFat                    = @BdnHar_KnsFat                      ,
    BdnHar_TesMik                    = @BdnHar_TesMik                      ,
    BdnHar_rezervasyon_miktari       = @BdnHar_rezervasyon_miktari         ,
    BdnHar_rezerveden_teslim_edilen  = @BdnHar_rezerveden_teslim_edilen      
    WHERE BdnHar_Guid = @BdnHar_Guid
  ELSE
    INSERT INTO  BEDEN_HAREKETLERI  (
        BdnHar_Guid                             ,
        BdnHar_DBCno                            ,
        BdnHar_Spec_Rec_no                      ,
        BdnHar_iptal                            ,
        BdnHar_fileid                           ,
        BdnHar_hidden                           ,
        BdnHar_kilitli                          ,
        BdnHar_degisti                          ,
        BdnHar_checksum                         ,
        BdnHar_create_user                      ,
        BdnHar_create_date                      ,
        BdnHar_lastup_user                      ,
        BdnHar_lastup_date                      ,
        BdnHar_special1                         ,
        BdnHar_special2                         ,
        BdnHar_special3                         ,
        BdnHar_Tipi                             ,
        BdnHar_Har_uid                          ,
        BdnHar_BedenNo                          ,
        BdnHar_HarGor                           ,
        BdnHar_KnsIsGor                         ,
        BdnHar_KnsFat                           ,
        BdnHar_TesMik                           ,
        BdnHar_rezervasyon_miktari              ,
        BdnHar_rezerveden_teslim_edilen                          
     )
    VALUES(
        @BdnHar_Guid                             ,
        @BdnHar_DBCno                            ,
        @BdnHar_Spec_Rec_no                      ,
        @BdnHar_iptal                            ,
        @BdnHar_fileid                           ,
        @BdnHar_hidden                           ,
        @BdnHar_kilitli                          ,
        @BdnHar_degisti                          ,
        @BdnHar_checksum                         ,
        @BdnHar_create_user                      ,
        @BdnHar_create_date                      ,
        @BdnHar_lastup_user                      ,
        @BdnHar_lastup_date                      ,
        @BdnHar_special1                         ,
        @BdnHar_special2                         ,
        @BdnHar_special3                         ,
        @BdnHar_Tipi                             ,
        @BdnHar_Har_uid                          ,
        @BdnHar_BedenNo                          ,
        @BdnHar_HarGor                           ,
        @BdnHar_KnsIsGor                         ,
        @BdnHar_KnsFat                           ,
        @BdnHar_TesMik                           ,
        @BdnHar_rezervasyon_miktari              ,
        @BdnHar_rezerveden_teslim_edilen                        
      );";
            return sql;
        }



    }
}
