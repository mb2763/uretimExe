## Modul: IstasyonHareketlerModul

Bu modul, sahada (TabletV2) acilan istasyon takip kayitlarinin ofis/yonetim tarafindan **listelenmesi, izlenmesi, duzeltilmesi ve Mikro ERP'ye fis aktarimini** kapsar. Ureticinin kendisi burada uretim girisi yapmaz; uretim girisi tablette `IstasyonTakipPage` akisinda yapilir ve buraya yalniz **sonuc kayitlari** (IstasyonTakipHareket = istasyon basligi/sarisi, IstasyonTakipHareketDetay = her mamul/fire/iptal hareketi, IstasyonTakipStokHareket = sarf edilen recete stoklari, IstasyonTakipHareketLog = baslat/durdur/bitir log'lari) yansir. Modul WinForms (UretimV4 / `CepPatronERP.exe`) icindedir, formlar `Frm*` deseni ile yazilmistir ve DB'ye dogrudan baglanir (`Ortak.DbPro` = Uretim DB, `Ortak.DbMikro` = Mikro DB). Tum servisler `IstasyonHareketManager` veya dogrudan `Ortak.DbPro.<Servis>` uzerinden cagrilir. Akis/miktar motoru OPERASYON-Sira bazlidir: bir detayda miktar duzeltilirse `Uretim_MiktarGuncelle` ve `Uretim_PlanlananGuncelle` prosedurleri yeniden cagrilir ve miktarlar UretimIstasyonHareket -> UretimIstasyon -> UrOHD/UrOH/UrO zincirinde yukari toplanir, bir sonraki operasyon Sira'sinin planlanani guncellenir.

Bu modulde 6 form vardir: 4 liste formu (Hareketler, Bekleyenler, Detaylar, Fis Listesi, Log Listesi) ve 1 kayit/duzenleme formu (Detay Guncelle).

---

### Istasyon Hareketler (`FrmIstasyonHareketler.cs` / `.Designer.cs`)
**Ne ise yarar:** Acilmis istasyon takip basliklarini (IstasyonTakipHareket) listeler. Ust grid baslik kayitlari, alt sol grid o baslica ait hareket detaylari (IstasyonTakipHareketDetay), alt sag grid ilgili UretimIstasyon'a (UrIId) ait sarf stok hareketleri (IstasyonTakipStokHareket). Istasyon, durum (Aktif/Beklemede/Durduruldu/Bitti), is emri (siparis) kodu ve tarih araligina gore filtrelenir.
**Once ne olmali (onkosul):** Tablette ilgili istasyonda en az bir takip kaydi baslatilmis olmali (UretimIstasyon + IstasyonTakipHareket olusmus olmali). Form `SiparisKodundanFiltrele=true` + `SiparisKodu` ile baska bir ekrandan (siparis/is emri) tek is emrine kilitli acilabilir.
**Sonra ne olur:** Bu form yalniz okuma/goruntulemedir; veri degistirmez. Bir detay satirina cift tiklamak (yalnizca `SecimIcinAcildi` modunda) secim dondurur. Detay duzeltmesi bu formdan degil, FrmIstasyonHareketDetaylar -> FrmIstasyonHareketDetayED uzerinden yapilir.
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `Bagla()` cagirir, filtreye gore ust gridi doldurur. Form acilisinda `BtnAra.PerformClick()` otomatik calisir.
- `BtnTemizle` ("Temizle") — Tarih (son 1 ay) / istasyon / is emri kodu filtrelerini sifirlar.
- `BtnDurumuHepsi` ("Hepsi") — durum filtresini bos birakir, otomatik arar.
- `BtnDurumuAktif` ("Aktif") — Durumu=Aktif filtreler (acilista varsayilan secili).
- `BtnDurumuBeklemede` ("Beklemede") — Durumu=Beklemede filtreler (Designer'da `Visible=false`, gizli).
- `BtnDurumuDurduruldu` ("Durduruldu") — Durumu=Durduruldu filtreler.
- `BtnDurumuBitti` ("Bitti") — Durumu=Bitti filtreler.
- `CmbIstasyon` (combo) — istasyon kodu filtresi (IstasyonKarti listesinden dolar).
- `TxtKodu` (Is Emri Kodu), `TxtTarihi1/TxtTarihi2` — like ve tarih araligi filtreleri.
- Grid cift tik / Enter (`MyEventDoubleClickEnter`) — secim modunda kayit dondur.
- `BtnKapat` (base) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IstasyonHareketManager(Ortak.DbPro, Ortak.DbMikro)` — modul servislerini toplar.
- Service: `IstHareketService.GetViewListWhere(where)` — IstasyonTakipHareket view'ini filtreyle ceker (ust grid).
- Service: `IstHareketDetayService.GetViewListWhere(" where IstHD.IstHrId='...'")` — secili baslica ait detaylar (alt sol grid).
- Service: `IstHareketStokService.GetStokHareketByUrIId(UrIId)` — secili UretimIstasyon'un sarf stok hareketleri (alt sag grid).
- Service: `IstKartService.SelectListWhere("")` — istasyon combo'sunu doldurur.
- SQL/Prosedur: yok (yalniz select).
- API: yok (WinForms, dogrudan DB).
**Istasyon sirasiyla iliskisi:** Durumu=Aktif satirlar o anda calisilan, Bitti satirlar bir sonraki istasyona sevk edilmis/tamamlanmis operasyonlardir. Alt sag grid UrIId ile o operasyonun sarf recete stoklarini gosterir; operasyon Sira mantigini dogrudan degistirmez (okuma).
**Notlar:** `Ortak.PlKapat=true` ise Parti/Lot kolonlari gizlenir. SorguAyarla string concatenation ile yazilmistir (parametrik degil). "SiparisKodu" kolonu UI'da "IsEmriKodu" olarak gosterilir.

---

### Istasyon Bekleyenler (`FrmIstasyonBekleyenler.cs` / `.Designer.cs`)
**Ne ise yarar:** Henuz hicbir istasyon takip kaydi (IstasyonTakipHareket) olusturulmamis, yani sahada baslatilmayi bekleyen UretimIstasyon kuyrugunu gosterir. Her satir: kalan miktar, planlanan/uretilen/fire/iptal miktarlari, recete, operasyon, siparis (is emri), teslim tarihi, fason bilgisi vb.
**Once ne olmali (onkosul):** Uretim emri + receteden UretimOperasyon/UretimIstasyon kayitlari uretilmis (KalanMiktar > 0) ama o UretimIstasyon icin henuz IstasyonTakipHareket acilmamis olmali.
**Sonra ne olur:** Salt okuma listesidir. Cift tik islevleri kod icinde yorum satirina alinmis; herhangi bir kayit/degisiklik tetiklemez. Operatorun tablette hangi isleri baslatmasi gerektigini planlama amaclidir.
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `Bagla()` cagirir; form acilisinda da otomatik `Bagla()` calisir.
- `BtnTemizle` ("Temizle") — Designer'da var ancak bu formda click event'i baglanmamis (etkin degil).
- `CmbIstasyon` (combo) — istasyon kodu filtresi (`UrI.IstasyonKodu`).
- `TxtKodu` ("Is Emri Kodu") — siparis kodu like filtresi (`Sip.SiparisKodu`).
- Grid cift tik / Enter (`MyEventDoubleClickEnter`) — aktif islev yok (yorumlu).
- `BtnKapat` (base) — kapatir.
**Cagirdigi katmanlar:**
- Manager: `IstasyonHareketManager.GetBekleyenler(whereAnd)` — su SQL'i calistirir: `UretimIstasyon UrI` + `UretimOperasyon UrO` + `Siparis` + `SiparisHareket` LEFT JOIN, `IstasyonTakipHareket TH` LEFT JOIN ile `WHERE UrO.KalanMiktar > 0 AND TH.Id IS NULL` (yani henuz takip acilmamis); UrI bazinda planlanan/uretilen/fire/iptal miktarlarini gruplayip toplar. (Benzer `GetBekleyenByIstKodu(Istasyon)` metodu da ayni mantikla tek istasyon icindir.)
- Service: `IstKartService.SelectListWhere("")` — istasyon combo'su.
- SQL/Prosedur: inline SELECT (ad yok), prosedur cagirmaz.
- API: yok.
**Istasyon sirasiyla iliskisi:** "Bekleyen" = `IstasyonTakipHareket` henuz yok; bir operasyonun Sira'sina ait UretimIstasyon olusmus ama saha basinda is alinmamistir. Tablette baslatildiginda bu satir listeden dusup FrmIstasyonHareketler'e tasinir.
**Notlar:** Tarih filtreleri (`SorguAyarlaTrh`) tamamen yorum satiridir, etkisizdir. PlKapat'ta Parti/Lot gizlenir.

---

### Istasyon Hareket Detaylar (`FrmIstasyonHareketDetaylar.cs` / `.Designer.cs`)
**Ne ise yarar:** Tum istasyon hareket **detaylarini** (IstasyonTakipHareketDetay) tek listede gosterir (mamul giris, fire mamul giris, uretim bitis, fire stok giris, uretim iptal turleri). Sarf/Fire **fis** turleri (SarfCikisFisi, FireGirisFisi) bu listeden HARIC tutulur (onlar FrmIstasyonFisList'te). Detaya cift tiklanarak miktar/fire/iptal duzeltme ekrani acilir.
**Once ne olmali (onkosul):** Tablette uretim girisi yapilmis ve IstasyonTakipHareketDetay kayitlari olusmus olmali.
**Sonra ne olur:** Cift tik -> `FrmIstasyonHareketDetayED` acilir; orada kaydedilirse miktarlar guncellenir ve dönülünce `Bagla()` ile liste tazelenir.
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `Bagla()`.
- `BtnTemizle` ("Temizle") — tarih (son 1 ay), saat (00:00:00 / 23:59:59), istasyon, is emri kodu filtrelerini sifirlar.
- `CmbIstasyon` (combo) — `IstHr.IstasyonKodu` filtresi.
- `CmbTuru` (combo) — `IstHD.Turu` filtresi; SarfCikisFisi ve FireGirisFisi turleri listeye eklenmez.
- `TxtKodu` ("Is Emri Kodu") — `IstHr.SiparisKodu` like.
- `TxtTarihi1/2` + `TxtSaat1/2` — `IstHD.Tarih` datetime araligi.
- Grid cift tik / Enter (`MyEventDoubleClickEnter`) — secim modunda secim dondurur; degilse `FrmIstasyonHareketDetayED` acar.
- `BtnKapat` (base) — kapatir.
**Cagirdigi katmanlar:**
- Service: `Ortak.DbPro.IstasyonTakipHareketDetay.GetViewListWhere(where)` — detay view'i (SarfCikisFisi/FireGirisFisi haric SQL ile filtrelenir).
- Service: `Ortak.DbPro.IstasyonKarti.SelectListWhere("")` — istasyon combo.
- Service: `Ortak.DbPro.GenelServis.GrupListesi("IstasyonTakipHareketDetay","Turu")` — Turu combo degerleri.
- Acilan form: `FrmIstasyonHareketDetayED` (Model=secili detay).
- SQL/Prosedur: dogrudan prosedur cagirmaz (duzeltme FrmIstasyonHareketDetayED'de yapilir).
- API: yok.
**Istasyon sirasiyla iliskisi:** Detay turleri (MamulGiris/UretimBitis/UretimIptal/FireMamulGiris) miktar motorunun yukari toplama girdileridir; burada yapilan duzeltme bir sonraki operasyon Sira'sinin planlananini etkiler (ED formunda prosedur tetiklenir).
**Notlar:** Sarf/Fire fisleri kasıtlı olarak haric. Saat alanlari TimeSpanEdit.

---

### Istasyon Hareket Detay Guncelle (`FrmIstasyonHareketDetayED.cs` / `.Designer.cs`)
**Ne ise yarar:** Tek bir IstasyonTakipHareketDetay kaydinin Miktar / FireMiktar / IptalMiktar degerlerini duzeltir (ofiste hatali girilen miktari elle duzeltme). Stok/Recete/Operasyon kod-ad alanlari salt okunur gosterilir.
**Once ne olmali (onkosul):** FrmIstasyonHareketDetaylar listesinde bir detay satiri secilip cift tiklanmis (`Model` set edilmis) olmali.
**Sonra ne olur (kaydet sonrasi):** `IstasyonHareketManager.SaveIstasyonTakipHareketDetayUpdate(Model)` calisir; tek transaction icinde su zincir isler ve form kapanir:
1. IstasyonTakipHareketDetay update (detay miktarlari yazilir),
2. `UretimIstasyonHareket` ayni Id ile UretimMiktari/FireMiktari/IptalMiktari guncellenir,
3. `IstasyonTakipHareket` baslik miktarlari (`GetMiktarFireUpdateSqlCode`) yeniden hesaplanir (detaylardan MamulGiris/FireMamulGiris/UretimBitis/UretimIptal toplanir; KalanMiktar = Planlanan - (Uretim+Iptal+Fire)),
4. UrIId bazinda iptal miktari stok hareketlerine oranlanir (`GetIptalUpdateSqlCodeByUrIId`),
5. `exec [Uretim_MiktarGuncelle] '<UrId>'`,
6. `exec [Uretim_PlanlananGuncelle] '<UrId>'`.
**Butonlar & kisayollar:**
- `BtnKaydet` ("Kaydet") — `AktarRowa()` + `Kaydet()` (yukaridaki update zincirini calistirir, sonra `this.Close()`).
- `BtnKapat` (base) — kaydetmeden kapatir.
- `BtnSil` (base) — `Visible=false`, gizli (silme yok).
- `BtnYeni/BtnDuzenle/BtnYazdir/BtnIlk/BtnOnceki/BtnSonraki/BtnSon` (base nav) — bu formda islevsiz/gizli, tek kayit duzenleme.
- Duzenlenebilir alanlar: `TxtMiktar`, `TxtFireMiktar`, `TxtIptalMiktar` (sayi). Diger alanlar ReadOnly.
**Cagirdigi katmanlar:**
- Manager: `IstasyonHareketManager.SaveIstasyonTakipHareketDetayUpdate(IstasyonTakipHareketDetay)` — yukaridaki 6 adimli transaction.
- SQL/Prosedur: `Uretim_MiktarGuncelle` — miktarlari UretimIstasyonHareket->UretimIstasyon->UrOHD/UrOH/UrO zincirinde yukari toplar. `Uretim_PlanlananGuncelle` — operasyon Sira N uretimini Sira N+1 planlanina tasir, sonunda `Uretim_SonrakiIstasyonaGonder` cagrilir (ReceteAna.IstasyonGruplamaKullan=1 ise yeni UretimIstasyon olusturur).
- SQL kod (entity static): `IstasyonTakipHareketDetay.GetUpdateSqlCode()`, `IstasyonTakipHareket.GetMiktarFireUpdateSqlCode(IstHrId)`, `IstasyonTakipHareket.GetIptalUpdateSqlCodeByUrIId(UrIId)`.
- API: yok.
**Istasyon sirasiyla iliskisi:** Bu form akis motorunu **tetikleyen tek yazma noktasidir**. Bir detay miktari degistiginde `Uretim_PlanlananGuncelle` -> `Uretim_SonrakiIstasyonaGonder` zinciriyle bir sonraki operasyon Sira'sinin planlanani/istasyon kaydi yeniden hesaplanir.
**Notlar:** `MyFrmKayit` turevidir (kayit formu base'i). Transaction'da hata olursa rollback + ErrorResult; basaride form kapanir.

---

### Istasyon Fis Listesi (`FrmIstasyonFisList.cs` / `.Designer.cs`)
**Ne ise yarar:** Yalniz **SarfCikisFisi** ve **FireGirisFisi** turundeki istasyon hareket detaylarini (Mikro'ya gonderilecek sarf cikisi / fire girisi fisleri) listeler ve secilenleri Mikro ERP'ye aktarir. "Mikroya Aktarildi" durumuna gore (Tumu / Aktarilan / Bekleyen) filtrelenir.
**Once ne olmali (onkosul):** Tablette/uretim girisinde sarf veya fire fisi turunde IstasyonTakipHareketDetay kayitlari olusmus olmali. Aktarim oncesi Mikro entegre ayarlari (`Ortak.MikroEntAyarlar` icinde fis turu ayarlari) tanimli olmali.
**Sonra ne olur (Mikroya Gonder sonrasi):** Secili fisler `FrmMikroyaSarfFireKaydet`e tasinir; orada Mikro `STOK_HAREKETLERI` kayitlari uretilip yazilir, basarida detay kayitlari `Ent=true`, `EntSeri/EntSira/EntDate` ile guncellenir (artik "Aktarilan" sekmesinde gorunur).
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `Bagla()`; acilista `AktarimAyarla("Bekleyen")` + otomatik `BtnAra.PerformClick()`.
- `BtnTemizle` ("Temizle") — tarih filtrelerini bosaltir.
- `BtnAktarildiTumu` ("Tumu") — Ent filtresi yok.
- `BtnAktarildiAktarilan` ("Aktarilan") — `coalesce(Ent,0)=1`.
- `BtnAktarildiBekleyen` ("Bekleyen") — `coalesce(Ent,0)=0` (acilista varsayilan).
- `TxtTarihi1/2` — `IstHD.Tarih` araligi.
- `Sec` kolonu — grid'de editlenebilir checkbox (aktarilacak satirlari isaretleme).
- Sag tik context menu: **"Mikroya Gonder"** (`ToolStripMenuItem`) — secili (`Sec=true`) fisleri toplayip `FrmMikroyaSarfFireKaydet` acar.
- Grid cift tik / Enter (`MyEventDoubleClickEnter`) — secim modunda secim dondurur; degilse islevsiz.
- `BtnKapat` (base) — kapatir.
**Cagirdigi katmanlar:**
- Service: `Ortak.DbPro.IstasyonTakipHareketDetay.GetViewListWhere(where)` — yalniz `Turu=SarfCikisFisi OR Turu=FireGirisFisi` + Ent durumu filtresi.
- Acilan form: `FrmMikroyaSarfFireKaydet` (MikroModul) — `FisList` ile.
- `FrmMikroyaSarfFireKaydet` icinde: `MikroKayitManager` (`StokHareketIdKayitEdilmismi`, `StokHareketKaydet`), `MikroConvertManager` (`SetSarfCikisFisiTuruAyar`, `SetFireGirisFisiTuruAyar`, `ConvertStokVirmanFisi`, `ConvertSarfDepoCikis`), `IstasyonTakipHareketDetay` servisinin `InsertOrUpdate(FisList)` ile Ent guncellemesi.
- SQL/Prosedur: dogrudan prosedur cagirmaz; aktarim Mikro tarafinda StokHareketKaydet ile yapilir.
- API: yok.
**Istasyon sirasiyla iliskisi:** -  (Fis aktarimi miktar/Sira motorunu degil, Mikro ERP stok hareketlerini etkiler.)
**Notlar:** Cift aktarimi engellemek icin `FrmMikroyaSarfFireKaydet` once `StokHareketIdKayitEdilmismi` ile onceden aktarilmis kayit kontrolu yapar; varsa Kaydet butonu gizlenir. `Sec` kolonu acilista ReadOnly kapatilip edit acilir.

---

### Istasyon Hareket Log Listesi (`FrmIstasyonHareketLogList.cs` / `.Designer.cs`)
**Ne ise yarar:** Saha akisindaki baslat/durdur/bitir/sevk gibi olaylarin log kayitlarini (IstasyonTakipHareketLog) listeler. Operasyon, istasyon ve tarih-saat araligina gore filtrelenir. Denetim/izleme amaclidir.
**Once ne olmali (onkosul):** Tablette istasyon takip akisi calismis ve log kayitlari (`IstasyonTakipHareketLog`) olusmus olmali.
**Sonra ne olur:** Salt okuma; veri degistirmez. Cift tik islevi (uretim emri editorune gitme) kod icinde yorumludur, aktif degildir.
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `Bagla()`; form acilisinda da `Bagla()` otomatik.
- `BtnTemizle` ("Temizle") — tarih bos, saat 00:00:00 / 23:59:59 yapar.
- `CmbOperasyon` (MyLookupEdit) — `TH.OperasyonKodu` filtresi (OperasyonKarti listesinden).
- `CmbIstasyon` (MyLookupEdit) — `TH.IstasyonKodu` filtresi (IstasyonKarti listesinden).
- `TxtTarihi1/2` + `TxtSaat1/2` — `LG.Tarih` datetime araligi.
- Grid cift tik / Enter (`MyEventDoubleClickEnter`) — secim modunda secim dondurur; degilse islevsiz (yorumlu).
- `BtnKapat` (base) — kapatir.
**Cagirdigi katmanlar:**
- Service: `Ortak.DbPro.IstasyonTakipHareketLog.GetViewListWhere(where)` — log view'i.
- Service: `Ortak.DbPro.OperasyonKarti.SelectListWhere(" Order By OperasyonKodu")` — operasyon combo.
- Service: `Ortak.DbPro.IstasyonKarti.SelectListWhere(" Order By IstasyonKodu")` — istasyon combo.
- SQL/Prosedur: yok (yalniz select).
- API: yok.
**Istasyon sirasiyla iliskisi:** Log kayitlari saha akisindaki adimlarin (baslat/durdur/bitir/sonraki istasyona sevk) zaman damgali izini tutar; Sira/akis motorunu degistirmez.
**Notlar:** `IstasyonTakipHareketLogService` ayni zamanda `IstasyonHareketManager.IstHareketLogService` olarak da erisilebilir ama bu form servisi dogrudan `Ortak.DbPro` uzerinden alir.

---

#### Modul geneli notlar
- Tum liste formlari `MyFrmListe`, kayit formu (`FrmIstasyonHareketDetayED`, `FrmMikroyaSarfFireKaydet`) `MyFrmKayit` base sinifindan turer (base siniflar `My.Kontrol.Formlar` namespace'inde, harici kontrol kutuphanesinde). `BtnAra/BtnTemizle/BtnKapat/BtnYazdir/BtnDizayn` ve kayit formundaki `BtnKaydet/BtnSil/BtnYeni/BtnIlk/BtnOnceki/BtnSonraki/BtnSon` base'ten gelir.
- Grid cift tik ve Enter, base'in `MyView.MyEventDoubleClickEnter` event'i ile ayni davranisi tetikler (cift tik = Enter).
- Filtre butonlarinin secili gorunumu `MyButton.FilterButonRenklendir(true/false)` ile renklendirilir.
- Tek **yazma** noktasi `FrmIstasyonHareketDetayED` (miktar duzeltme + prosedur zinciri) ve `FrmIstasyonFisList` -> `FrmMikroyaSarfFireKaydet` (Mikro'ya fis aktarimi). Diger formlar salt okuma/listeleme.
