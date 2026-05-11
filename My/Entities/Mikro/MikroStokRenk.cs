using My.Core.Data;
using System;
using System.Collections.Generic;

namespace My.Entities.Mikro
{
    [Table("STOK_RENK_TANIMLARI")]
    public class MikroStokRenk
    {
        public Guid rnk_Guid { get; set; }
        public short rnk_DBCno { get; set; }
        public int? rnk_SpecRECNo { get; set; }
        public bool? rnk_iptal { get; set; }
        public short? rnk_fileid { get; set; }
        public bool? rnk_hidden { get; set; }
        public bool? rnk_kilitli { get; set; }
        public bool? rnk_degisti { get; set; }
        public int? rnk_CheckSum { get; set; }
        public short? rnk_create_user { get; set; }
        public DateTime rnk_create_date { get; set; }
        public short? rnk_lastup_user { get; set; }
        public DateTime? rnk_lastup_date { get; set; }
        public string rnk_special1 { get; set; }
        public string rnk_special2 { get; set; }
        public string rnk_special3 { get; set; }
        public string rnk_kodu { get; set; }
        public string rnk_ismi { get; set; }
        public string rnk_kirilim_1 { get; set; }
        public string rnk_kirilim_2 { get; set; }
        public string rnk_kirilim_3 { get; set; }
        public string rnk_kirilim_4 { get; set; }
        public string rnk_kirilim_5 { get; set; }
        public string rnk_kirilim_6 { get; set; }
        public string rnk_kirilim_7 { get; set; }
        public string rnk_kirilim_8 { get; set; }
        public string rnk_kirilim_9 { get; set; }
        public string rnk_kirilim_10 { get; set; }
        public string rnk_kirilim_11 { get; set; }
        public string rnk_kirilim_12 { get; set; }
        public string rnk_kirilim_13 { get; set; }
        public string rnk_kirilim_14 { get; set; }
        public string rnk_kirilim_15 { get; set; }
        public string rnk_kirilim_16 { get; set; }
        public string rnk_kirilim_17 { get; set; }
        public string rnk_kirilim_18 { get; set; }
        public string rnk_kirilim_19 { get; set; }
        public string rnk_kirilim_20 { get; set; }
        public string rnk_kirilim_21 { get; set; }
        public string rnk_kirilim_22 { get; set; }
        public string rnk_kirilim_23 { get; set; }
        public string rnk_kirilim_24 { get; set; }
        public string rnk_kirilim_25 { get; set; }
        public string rnk_kirilim_26 { get; set; }
        public string rnk_kirilim_27 { get; set; }
        public string rnk_kirilim_28 { get; set; }
        public string rnk_kirilim_29 { get; set; }
        public string rnk_kirilim_30 { get; set; }
        public string rnk_kirilim_31 { get; set; }
        public string rnk_kirilim_32 { get; set; }
        public string rnk_kirilim_33 { get; set; }
        public string rnk_kirilim_34 { get; set; }
        public string rnk_kirilim_35 { get; set; }
        public string rnk_kirilim_36 { get; set; }
        public string rnk_kirilim_37 { get; set; }
        public string rnk_kirilim_38 { get; set; }
        public string rnk_kirilim_39 { get; set; }
        public string rnk_kirilim_40 { get; set; }
        public string rnk_kirilim_41 { get; set; }
        public string rnk_kirilim_42 { get; set; }
        public string rnk_kirilim_43 { get; set; }
        public string rnk_kirilim_44 { get; set; }
        public string rnk_kirilim_45 { get; set; }
        public string rnk_kirilim_46 { get; set; }
        public string rnk_kirilim_47 { get; set; }
        public string rnk_kirilim_48 { get; set; }
        public string rnk_kirilim_49 { get; set; }
        public string rnk_kirilim_50 { get; set; }
        public string rnk_kirilim_51 { get; set; }
        public string rnk_kirilim_52 { get; set; }
        public string rnk_kirilim_53 { get; set; }
        public string rnk_kirilim_54 { get; set; }
        public string rnk_kirilim_55 { get; set; }
        public string rnk_kirilim_56 { get; set; }
        public string rnk_kirilim_57 { get; set; }
        public string rnk_kirilim_58 { get; set; }
        public string rnk_kirilim_59 { get; set; }
        public string rnk_kirilim_60 { get; set; }

        public List<string> GetRenkler()
        {
            var lis = new List<string>();
            lis.Add("");
            var renk = "";
            for (var i = 1; i <= 60; i++)
            {
                renk = GetPropertyValue("rnk_kirilim_" + i, this);
                if (!string.IsNullOrEmpty(renk)) lis.Add(renk);
            }

            return lis;
        }

        private string GetPropertyValue(string PropName, MikroStokRenk ent)
        {
            return ent.GetType().GetProperty(PropName) == null
                ? ""
                : ent.GetType().GetProperty(PropName).GetValue(ent, null).ToString();
        }
    }
}