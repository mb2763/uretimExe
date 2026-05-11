using My.Core.Data;
using System;
using System.Collections.Generic;

namespace My.Entities.Mikro {

    [Table("STOK_BEDEN_TANIMLARI")]
    public class MikroStokBeden {

        public Guid bdn_Guid { get; set; }
        public short bdn_DBCno { get; set; }
        public int? bdn_SpecRECNo { get; set; }
        public bool? bdn_iptal { get; set; }
        public short? bdn_fileid { get; set; }
        public bool? bdn_hidden { get; set; }
        public bool? bdn_kilitli { get; set; }
        public bool? bdn_degisti { get; set; }
        public int? bdn_CheckSum { get; set; }
        public short? bdn_create_user { get; set; }
        public DateTime bdn_create_date { get; set; }
        public short? bdn_lastup_user { get; set; }
        public DateTime? bdn_lastup_date { get; set; }
        public string bdn_special1 { get; set; }
        public string bdn_special2 { get; set; }
        public string bdn_special3 { get; set; }
        public string bdn_kodu { get; set; }
        public string bdn_ismi { get; set; }
        public string bdn_kirilim_1 { get; set; }
        public string bdn_kirilim_2 { get; set; }
        public string bdn_kirilim_3 { get; set; }
        public string bdn_kirilim_4 { get; set; }
        public string bdn_kirilim_5 { get; set; }
        public string bdn_kirilim_6 { get; set; }
        public string bdn_kirilim_7 { get; set; }
        public string bdn_kirilim_8 { get; set; }
        public string bdn_kirilim_9 { get; set; }
        public string bdn_kirilim_10 { get; set; }
        public string bdn_kirilim_11 { get; set; }
        public string bdn_kirilim_12 { get; set; }
        public string bdn_kirilim_13 { get; set; }
        public string bdn_kirilim_14 { get; set; }
        public string bdn_kirilim_15 { get; set; }
        public string bdn_kirilim_16 { get; set; }
        public string bdn_kirilim_17 { get; set; }
        public string bdn_kirilim_18 { get; set; }
        public string bdn_kirilim_19 { get; set; }
        public string bdn_kirilim_20 { get; set; }
        public string bdn_kirilim_21 { get; set; }
        public string bdn_kirilim_22 { get; set; }
        public string bdn_kirilim_23 { get; set; }
        public string bdn_kirilim_24 { get; set; }
        public string bdn_kirilim_25 { get; set; }
        public string bdn_kirilim_26 { get; set; }
        public string bdn_kirilim_27 { get; set; }
        public string bdn_kirilim_28 { get; set; }
        public string bdn_kirilim_29 { get; set; }
        public string bdn_kirilim_30 { get; set; }
        public string bdn_kirilim_31 { get; set; }
        public string bdn_kirilim_32 { get; set; }
        public string bdn_kirilim_33 { get; set; }
        public string bdn_kirilim_34 { get; set; }
        public string bdn_kirilim_35 { get; set; }
        public string bdn_kirilim_36 { get; set; }
        public string bdn_kirilim_37 { get; set; }
        public string bdn_kirilim_38 { get; set; }
        public string bdn_kirilim_39 { get; set; }
        public string bdn_kirilim_40 { get; set; }


        public List<string> GetBedenler() {
            var lis = new List<string>();
            lis.Add("");
            var renk = "";
            for (var i = 1; i <= 40; i++) {
                renk = GetPropertyValue("bdn_kirilim_" + i, this);
                if (!string.IsNullOrEmpty(renk)) lis.Add(renk);
            }

            return lis;
        }

        private string GetPropertyValue(string PropName, MikroStokBeden ent) {
            return ent.GetType().GetProperty(PropName) == null
                ? ""
                : ent.GetType().GetProperty(PropName).GetValue(ent, null).ToString();
        }


    }
}
