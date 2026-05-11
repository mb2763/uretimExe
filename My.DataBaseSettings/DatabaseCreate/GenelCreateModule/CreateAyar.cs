using My.Core.Data;
using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate {
    public static class CreateAyar {

        private const string _tabloAdi = "Ayar";
        public static void AyarCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
            list.AddMalKabulData(); 
            list.AddIstasyonUretimData(); 
            list.AddMikroEntData();
            list.AddGenelData();
        }
        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Modul", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Grup", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.VarChar(500), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Deger", TableCreate.VarChar(50), ""));
        }
        static string ListToSql(this List<Ayar> lis) {

            string sql = " ";
            foreach (var itm in lis) {
                sql += $@" IF NOT EXISTS (SELECT 1 FROM {_tabloAdi}  WHERE Id = '{itm.Id}') 
                    begin INSERT [dbo].[{_tabloAdi}] (Id, Modul ,Grup ,Kodu ,Aciklama ,Deger) 
                    VALUES ( '{itm.Id}', N'{itm.Modul}' ,N'{itm.Grup}' ,N'{itm.Kodu}' ,N'{itm.Aciklama}' ,'{itm.Deger}' ) end ; " + "\r";
            }
            // string sqlMaster = $@"     SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] ON  " + sql + @" SET IDENTITY_INSERT [dbo].[" + _tabloAdi + @"] OFF";
            string sqlMaster = $@"    " + sql + @"  ";
            return sqlMaster;
        }

        static void AddMalKabulData(this List<string> list) {
            List<Ayar> lis = new List<Ayar>();
            string modul = "DepoTakip";
            string grup = "MalKabul";
            /**/
            lis.Add(new Ayar(Guid.Parse("F70C1056-C416-11EC-B9C8-00155D67607E"), modul, grup) { Kodu = "İrsaliyeyiAlsin", Deger = "1", Aciklama = "Depo Mal Kabul İçin irsaliyeleri Kontrol etsin" });

            lis.Add(new Ayar(Guid.Parse("F70C1057-C416-11EC-B9C8-00155D67607E"), modul, grup) { Kodu = "FaturayiAlsin", Deger = "1", Aciklama = "Depo Mal Kabul İçin Faturalari Kontrol etsin" });

            lis.Add(new Ayar(Guid.Parse("F70C1058-C416-11EC-B9C8-00155D67607E"), modul, grup) { Kodu = "KaliteKontrolZorunlu", Deger = "1", Aciklama = "Depo Mal Kabulunden önce KaliteKontrol Zorunlumu" });

            lis.Add(new Ayar(Guid.Parse("F70C1059-C416-11EC-B9C8-00155D67607E"), modul, grup) { Kodu = "RafTakibiZorunlu", Deger = "1", Aciklama = "Depo Urunler Stoklarda Tanımlanan bolum ve rafa Koymak zorunlumu" });
            /**/
            string sql = ListToSql(lis);
            list.Add(sql);
        }
        static void AddGenelData(this List<string> list) {
            List<Ayar> lis = new List<Ayar>();
            string modul = "Genel";
            string grup = "Genel";
            /**/
            lis.Add(new Ayar(Guid.Parse("08DBB814-A129-95CA-1291-F8796402AE75"), modul, grup) { Kodu = "PlKapat", Deger = "1", Aciklama = "" });

         
            /**/
            string sql = ListToSql(lis);
            list.Add(sql);
        }
        static void AddIstasyonUretimData(this List<string> list) {
            List<Ayar> lis = new List<Ayar>();
            string modul = "IstasyonUretim";
            string grup = "Istasyon";
            /**/
            lis.Add(new Ayar(Guid.Parse("08DB4640-6A71-6342-1291-F84FEC06A61A"), modul, grup) { Kodu = "UrunBilgiGoster", Deger = "1", Aciklama = "" });

            lis.Add(new Ayar(Guid.Parse("08DB4640-6A71-90BE-1291-F84FEC06A61D"), modul, grup) { Kodu = "AciklamaGoster", Deger = "0", Aciklama = "" });

            lis.Add(new Ayar(Guid.Parse("08DB4645-3985-2D7A-1291-F8433C04A4FB"), modul, grup) { Kodu = "MiktarGoster", Deger = "1", Aciklama = "" });

            lis.Add(new Ayar(Guid.Parse("08DB4645-3985-A2B2-1291-F8433C04A4FD"), modul, grup) { Kodu = "PartiLotGoster", Deger = "1", Aciklama = "" });

            lis.Add(new Ayar(Guid.Parse("08DB8386-8021-20C9-1291-F8537806A080"), modul, grup) { Kodu = "MalKabulKullan", Deger = "0", Aciklama = "" });

            lis.Add(new Ayar(Guid.Parse("08DBA87B-DBA6-618D-1291-F868500256AA"), modul, grup) { Kodu = "HatadaKodYerineAciklamaKullan", Deger = "0", Aciklama = "" });
          
            lis.Add(new Ayar(Guid.Parse("08DBA87B-DBA6-618D-1291-F868500256AB"), modul, grup) { Kodu = "MamulFiresiKullanma", Deger = "0", Aciklama = "" });
            /**/
            lis.Add(new Ayar(Guid.Parse("08DBEDAE-01D7-1C6E-1291-F856FC05A630"), modul, grup) { Kodu = "OlcumDegeri1", Deger = "Olcum Degeri 1", Aciklama = "" });
         
            lis.Add(new Ayar(Guid.Parse("08DBEDAE-01D7-1C6E-1291-F856FC05A631"), modul, grup) { Kodu = "OlcumDegeri2", Deger = "Olcum Degeri 2", Aciklama = "" });
            
            lis.Add(new Ayar(Guid.Parse("08DBEDAE-01D7-1C6E-1291-F856FC05A632"), modul, grup) { Kodu = "OlcumDegeri3", Deger = "Olcum Degeri 3", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBEDAE-01D7-1C6E-1291-F856FC05A633"), modul, grup) { Kodu = "OlcumDegeri4", Deger = "Olcum Degeri 4", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBEDAE-01D7-1C6E-1291-F856FC05A634"), modul, grup) { Kodu = "OlcumDegeri5", Deger = "Olcum Degeri 5", Aciklama = "" });
            /**/
            string sql = ListToSql(lis);
            list.Add(sql);
             
        }

        //private static void name(this List<string> list) {
        //    List<Ayar> lis = new List<Ayar>();   string modul = "";  string grup = "";
        //    /**/
        //    lis.Add(new Ayar(Guid.Parse(""), modul, grup) { Kodu = "", Deger = "0", Aciklama = "" }); 
        //    lis.Add(new Ayar(Guid.Parse(""), modul, grup) { Kodu = "", Deger = "0", Aciklama = "" }); 
        //    /**/
        //    string sql = ListToSql(lis);  list.Add(sql);
        //}

        static void AddMikroEntData(this List<string> list) {
            List<Ayar> lis = new List<Ayar>();
            string modul = "MikroEntegre";
            string grup = "GENEL";
            /**/
           
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-0F6F-6FBA-1291-F8796402AE10"), modul, grup) { Kodu = "FirmaNo", Deger = "0", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-0F71-38B4-1291-F8796402AE11"), modul, grup) { Kodu = "KullaniciKodu", Deger = "995", Aciklama = "" });

            grup = "FisTuru";
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-1E0B-7325-1291-F8796402AE1B"), modul, grup) { Kodu = "UretimUrunGirisFisi", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-1E0B-4BFA-1291-F8796402AE1A"), modul, grup) { Kodu = "UretimStokCikisFisi", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-1E0B-9A1B-1291-F8796402AE1D"), modul, grup) { Kodu = "UretimUrunFireCikisFisi", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-1E0B-9A1B-1291-F8796402AE1C"), modul, grup) { Kodu = "UretimStokFireCikisFisi", Deger = "", Aciklama = "" });
            
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-1E0B-9A1B-1291-F8796402AE1E"), modul, grup) { Kodu = "SarfCikisFisi", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-1E0B-9A1B-1291-F8796402AE1F"), modul, grup) { Kodu = "FireGirisFisi", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-1E0B-9A1B-1291-F8796402AE19"), modul, grup) { Kodu = "HizliUretimFisi", Deger = "", Aciklama = "" });

            grup = "DepoSevkFisi";
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-2C73-A5D9-1291-F8796402AE2E"), modul, grup) { Kodu = "CikisDepo", Deger = "1", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-2C73-CCEC-1291-F8796402AE2F"), modul, grup) { Kodu = "GirisDepo", Deger = "1", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-2C73-CCEC-1291-F8796402AE30"), modul, grup) { Kodu = "EvrakSeri", Deger = "SVK", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-2C73-F416-1291-F8796402AE31"), modul, grup) { Kodu = "GiderKodu", Deger = "GENEL", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-2C73-F416-1291-F8796402AE32"), modul, grup) { Kodu = "ProjeKodu", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-2C74-1B34-1291-F8796402AE33"), modul, grup) { Kodu = "SrmMerkezi", Deger = "", Aciklama = "" });

            grup = "SarfCikisFisi";
           
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-4863-497D-1291-F8796402AE39"), modul, grup) { Kodu = "DepoKodu", Deger = "1", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-4863-497D-1291-F8796402AE3A"), modul, grup) { Kodu = "EvrakSeri", Deger = "SRF", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-4863-7058-1291-F8796402AE3B"), modul, grup) { Kodu = "GiderKodu", Deger = "GENEL", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-4863-976B-1291-F8796402AE3C"), modul, grup) { Kodu = "ProjeKodu", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-4863-BE78-1291-F8796402AE3D"), modul, grup) { Kodu = "SrmMerkezi", Deger = "", Aciklama = "" });
           
            grup = "FireGirisFisi";
            
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-6046-8CEF-1291-F8796402AE43"), modul, grup) { Kodu = "DepoKodu", Deger = "1", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-6046-B3DC-1291-F8796402AE44"), modul, grup) { Kodu = "EvrakSeri", Deger = "FIG", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-6046-DAE5-1291-F8796402AE45"), modul, grup) { Kodu = "GiderKodu", Deger = "GENEL", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-6047-025B-1291-F8796402AE46"), modul, grup) { Kodu = "ProjeKodu", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-6047-025B-1291-F8796402AE47"), modul, grup) { Kodu = "SrmMerkezi", Deger = "", Aciklama = "" });
          

            grup = "UretimUrunGirisFisi";
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-8B02-31C2-1291-F8796402AE4C"), modul, grup) { Kodu = "DepoKodu", Deger = "1", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-8B02-31C2-1291-F8796402AE4D"), modul, grup) { Kodu = "EvrakSeri", Deger = "URGF", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-8B02-589B-1291-F8796402AE4E"), modul, grup) { Kodu = "GiderKodu", Deger = "GENEL", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-8B02-7FB6-1291-F8796402AE4F"), modul, grup) { Kodu = "ProjeKodu", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-8B02-7FB6-1291-F8796402AE50"), modul, grup) { Kodu = "SrmMerkezi", Deger = "", Aciklama = "" });
            
            grup = "UretimUrunFireCikisFisi";
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-9B69-A6D6-1291-F8796402AE56"), modul, grup) { Kodu = "DepoKodu", Deger = "1", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-9B69-CDFC-1291-F8796402AE57"), modul, grup) { Kodu = "EvrakSeri", Deger = "URFG", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-9B69-F4D7-1291-F8796402AE58"), modul, grup) { Kodu = "GiderKodu", Deger = "GENEL", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-9B69-F4D7-1291-F8796402AE59"), modul, grup) { Kodu = "ProjeKodu", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-9B6A-1BE4-1291-F8796402AE5A"), modul, grup) { Kodu = "SrmMerkezi", Deger = "", Aciklama = "" });
           
            grup = "UretimStokCikisFisi";
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-BA65-8506-1291-F8796402AE60"), modul, grup) { Kodu = "DepoKodu", Deger = "1", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-BA65-8506-1291-F8796402AE61"), modul, grup) { Kodu = "EvrakSeri", Deger = "URSC", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-BA65-AC18-1291-F8796402AE62"), modul, grup) { Kodu = "GiderKodu", Deger = "GENEL", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-BA65-D326-1291-F8796402AE63"), modul, grup) { Kodu = "ProjeKodu", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-BA65-D326-1291-F8796402AE64"), modul, grup) { Kodu = "SrmMerkezi", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-BA65-FA35-1291-F8796402AE65"), modul, grup) { Kodu = "STANDARTMALIYET", Deger = "1", Aciklama = "Stantart 1-ise Receteye bakmaz" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-BA66-4C1E-1291-F8796402AE66"), modul, grup) { Kodu = "RECETEMALIYET", Deger = "0", Aciklama = "" });
           
            grup = "UretimStokFireCikisFisi";
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-CE96-3A27-1291-F8796402AE6A"), modul, grup) { Kodu = "DepoKodu", Deger = "1", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-CE96-6137-1291-F8796402AE6B"), modul, grup) { Kodu = "EvrakSeri", Deger = "URFC", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-CE96-6137-1291-F8796402AE6C"), modul, grup) { Kodu = "GiderKodu", Deger = "GENEL", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-CE96-8846-1291-F8796402AE6D"), modul, grup) { Kodu = "ProjeKodu", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-CE96-8846-1291-F8796402AE6E"), modul, grup) { Kodu = "SrmMerkezi", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-CE96-AF54-1291-F8796402AE6F"), modul, grup) { Kodu = "STANDARTMALIYET", Deger = "1", Aciklama = "Stantart 1-ise Receteye bakmaz" });
            lis.Add(new Ayar(Guid.Parse("08DBB7E1-CE96-D664-1291-F8796402AE70"), modul, grup) { Kodu = "RECETEMALIYET", Deger = "0", Aciklama = "" });
            
            grup = "HizliUretimFisi";
            lis.Add(new Ayar(Guid.Parse("08DBC396-EABB-3B3C-1291-F833480288C0"), modul, grup) { Kodu = "GirisDepoKodu", Deger = "1", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBC396-EABB-3B3C-1291-F833480288C1"), modul, grup) { Kodu = "CikisDepoKodu", Deger = "1", Aciklama = "" });
            //   lis.Add(new Ayar(Guid.Parse("08DBC396-EABB-3B3C-1291-F833480288CA"), modul, grup) { Kodu = "DepoKodu", Deger = "1", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBC396-EABB-3B3C-1291-F833480288CB"), modul, grup) { Kodu = "EvrakSeri", Deger = "URFC", Aciklama = "" });
            //  lis.Add(new Ayar(Guid.Parse("08DBC396-EABB-3B3C-1291-F833480288CC"), modul, grup) { Kodu = "GiderKodu", Deger = "GENEL", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBC396-EABB-3B3C-1291-F833480288CD"), modul, grup) { Kodu = "ProjeKodu", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBC396-EABB-625B-1291-F833480288CE"), modul, grup) { Kodu = "SrmMerkezi", Deger = "", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBC396-EABB-3B3C-1291-F833480288C2"), modul, grup) { Kodu = "GirisGiderKodu", Deger = "GENEL", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse("08DBC396-EABB-3B3C-1291-F833480288C3"), modul, grup) { Kodu = "CikisGiderKodu", Deger = "GENEL", Aciklama = "" });


            /* 
            lis.Add(new Ayar(Guid.Parse(""), modul, grup) { Kodu = "", Deger = "0", Aciklama = "" });
            lis.Add(new Ayar(Guid.Parse(""), modul, grup) { Kodu = "", Deger = "0", Aciklama = "" }); 
             */

            /**/
            string sql = ListToSql(lis);
            list.Add(sql); 
        }


   

        class Ayar {
            public Ayar(Guid? id, string modul, string grup) {
                Id = id;
                Modul = modul;
                Grup = grup;
            }
            public Guid? Id { get; set; }
            public string Modul { get; set; }
            public string Grup { get; set; }
            public string Kodu { get; set; }
            public string Aciklama { get; set; }
            public string Deger { get; set; }
        }
    }
}
