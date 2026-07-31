## Modul: IstasyonModul

IstasyonModul, uretim akisinin temel yapi taslarindan biri olan **istasyon kartlarini** (uretim duraklari/operasyon noktalari), istasyonlara ait **bakim/ariza kayitlarini** ve saha tabletinde operatorun karsisina cikan **acilir aciklama listelerini** (baslatma hatasi, durdurma kodu, fire sebebi) yonetir. Tum veriler Uretim DB'sinde (UretimV3_FEZA) tutulur: `IstasyonKarti`, `IstasyonBakim`, `IstasyonBakimParca`, `IstasyonAciklama` tablolari. Istasyon kartlari bir **Operasyon** koduna baglanir; bu baglanti, recete-istasyon esleme (ReceteIstasyonGrupOperasyon) ve uretim akisinin (UretimIstasyon olusturma) merkezindedir. Istasyon kart kodu degistirildiginde modul, kodu uretim zincirindeki tum bagli tablolara (ReceteIstasyon, ReceteIstasyonGrupOperasyon, UretimIstasyon, IstasyonTakipHareket, IstasyonKontrol) tek bir transaction icinde yayar. Modulde 6 form vardir: 3'u ana sayfa (Kart Listesi, Kart Kaydi, Bakim Listesi) ve 3'u yardimci/popup (Bakim Ekle, Bakim Parca Ekle, Aciklama Tanimlari). Formlar `My.Kontrol.Formlar` namespace'indeki base siniflardan turer: `MyFrmListe` (arama panelli liste ekranlari), `MyFrmKayit` (ust bilgi + grid'li kayit ekranlari, alt buton seridi Kaydet/Sil/Yeni/Duzenle/Kapat icerir) ve `MyFrmSade` (sade popup, Kaydet/Kapat). Grid'lerde cift tiklama/Enter ortak `MyEventDoubleClickEnter` olayini tetikler (`SecimIcinAcildi` ise satiri secip kapatir, degilse Duzenle'yi calistirir).

### Istasyon Kart Listesi (`FrmIstasyonKartList.cs` / `FrmIstasyonKartList.Designer.cs`)
**Ne ise yarar:** Istasyon kartlarini operasyona gore filtreleyip listeleyen, esas olarak **secim diyalogu** olarak kullanilan liste ekrani. Recete-istasyon grubu operasyon eslestirme ve uretim istasyonu duzenleme ekranlarindan "istasyon sec" amaciyla acilir. `MyFrmListe` turevi (sol arama paneli + grid).
**Once ne olmali (onkosul):** Istasyon kartlari (`IstasyonKarti`) ve operasyon kartlari (`OperasyonKarti`) tanimli olmali. Secim modunda acan ekran (orn. `FrmReceteIstasyonGrupOperasyonEslestir`, `FrmUretimIstasyonED`) `SecimIcinAcildi=true`, opsiyonel `Aranan` (operasyon kodu) ve `RcAId` (recete ana Id) atar.
**Sonra ne olur:** Veri tabaninda DEGISIKLIK YAPMAZ (salt okuma/secim). Satira cift tiklaninca `SecilenKod=IstasyonKodu`, `SecilenRow=satir`, `Secildi=true` set edilip form kapanir; cagiran ekran secilen istasyonu alir.
**Butonlar & kisayollar:**
- `Ara` (`BtnAra`) — `Bagla()` cagirir; operasyon filtresine gore listeyi yeniden ceker.
- `Temizle` (`BtnTemizle`) — `TemizleText()`; operasyon arama kutusunu bosaltir.
- `Tümü` (`ChcTumu` onay kutusu) — isaretli ise tum istasyonlar; isaretsiz ve `RcAId` verilmisse yalnizca o receteye bagli istasyonlar (`ReceteyeBagliIstasyon.RcIId` ile eslesen) gosterilir.
- `Operasyon` (`CmbOperasyon` lookup) — operasyon koduna gore filtre.
- Grid cift tiklama / Enter — secim modunda satiri secip kapatir (`MyView1_MyEventDoubleClickEnter`).
- `Kapat` (`BtnKapat`, base) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IIstasyonKartiService.SelectListWhere(sor)` — operasyon filtresine gore istasyon kartlarini ceker (`_srv`, `Ortak.DbPro.IstasyonKarti`).
- Manager/Service: `IReceteyeBagliIstasyonService.SelectListWhere("where RcAId=...")` — `RcAId` verildiyse receteye bagli istasyon Id listesini ceker (`_srvBIst`), "Tümü" isaretsizken filtre icin kullanilir.
- Manager/Service: `IOperasyonKartiService.SelectListWhere(" Order By OperasyonKodu")` — operasyon combo'sunu doldurur (`_srvOpr`).
- SQL/Prosedur: Dinamik `where 1=1 AND Operasyon ='<deger>` (SorguAyarla) — NOT: tek tirnak kapanisi eksik, basit string birlestirme.
- API: -
**Istasyon sirasiyla iliskisi:** Dolayli. Secilen istasyon, recete operasyon-grup eslestirmesinde veya uretim istasyonu duzenlemede kullanilir; bu da uretim akisinin (operasyon Sira sirasi -> UretimIstasyon) kurulmasini etkiler.
**Notlar:** Namespace `MyUI.IstasyonModul`. `BagliIstasyonSec` bayragi tanimli ama mantikta dogrudan kullanilmiyor (cagiran taraf set ediyor). Grid duzeni `IstasyonKartlariSelectListesi` adiyla saklanir (`MyGridKayitAdi`). `Id` sutunu gizlenir.

### Istasyon Kartlari (Kayit) (`FrmIstasyonKartlari.cs` / `FrmIstasyonKartlari.Designer.cs`)
**Ne ise yarar:** Istasyon kartlarinin tam CRUD ekrani (ekle/duzenle/sil). Her istasyon bir operasyona, opsiyonel olarak bir fason cariye baglanir; kalite kontrol, yazdirilmali, fason bayraklari tutulur. `MyFrmKayit` turevi (ust GroupControl form + alt grid).
**Once ne olmali (onkosul):** Operasyon kartlari (`OperasyonKarti`) tanimli olmali (operasyon kodu/adi combo'lari icin). Fason cari secimi icin Mikro DB'de cari kartlari (`MikroCari`) bulunmali.
**Sonra ne olur:** Kaydet -> `IstasyonManager.Kaydet()` tek transaction'da: (1) `IstasyonKarti` tablosuna InsertOrUpdate; (2) **istasyon kodu degistirilmisse** ayni transaction icinde `ReceteIstasyon`, `ReceteIstasyonGrupOperasyon`, `UretimIstasyon`, `IstasyonTakipHareket`, `IstasyonKontrol` tablolarindaki eski koda ait IstasyonKodu/IstasyonAdi alanlarini yeni degerle gunceller. Sil -> `IIstasyonKartiService.Delete()`. Islem sonrasi liste `Bagla()` ile yenilenir; form acik kalir.
**Butonlar & kisayollar:**
- `Kaydet` (`BtnKaydet`, base) — `Kaydet()` -> dogrulama (kod zorunlu) -> `AktarModele()` -> `IstasyonManager.Kaydet(mdl, YeniKayit)` -> `Bagla()`.
- `Sil` (`BtnSil`, base) — onay sorar, secili kayit yoksa uyarir, `Sil()` -> `_srv.Delete(_mdl)`.
- `Yeni` (`BtnYeni`, base) — `YeniKayit=true`, `TemizleText()` ile formu temizler.
- `Düzenle` (`BtnDuzenle`, base) — secili grid satirini klonlayip forma yukler (`AktarTextlere`), `YeniKayit=false`.
- `Bagla` (`BtnBagla`) — listeyi yeniden baglar (`Bagla()` + sutun gizle + grid yerlesimi yukle).
- Fason Cari Kodu/Adi (`TxtFasonCariKodu`/`TxtFasonCariAdi`) buton tiklamasi — `FrmMikroCariListesi` secim diyalogunu acar, secilen carinin kodu/unvani alanlara aktarilir.
- `Operasyon` / `Operasyon Adi` (`CmbOperasyon`/`CmbOperasyonAdi` lookup) — biri secilince digeri otomatik dolar (Leave olaylari).
- `Fason` / `KaliteKontrol` / `Yazdırılmalı` (`ChcFason`/`ChcKaliteKontrol`/`myChcYazdir`) — istasyon ozellik bayraklari.
- Grid cift tiklama / Enter — secim modunda satiri secip kapatir, degilse `BtnDuzenle.PerformClick()`.
- `Kapat` (`BtnKapat`, base) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IstasyonManager.Kaydet(IstasyonKarti mdl, bool yenikayit)` — kod tekillik kontrolu (`KodVarmi`) + transaction'li kayit + kod degisikligini bagli uretim tablolarina yayma.
- Manager/Service: `IstasyonManager.KodVarmi(...)` — `Select count(*) From IstasyonKarti ...` ile ayni IstasyonKodu var mi kontrolu.
- Manager/Service: `IIstasyonKartiService` — `SelectListWhere`, `SelectFind`, `InsertOrUpdate`, `Delete`, `Execute` (DAL temel metotlari).
- Manager/Service: `IOperasyonKartiService.SelectListWhere(" Order By OperasyonKodu")` — operasyon combo'lari.
- SQL/Prosedur: `UPDATE ReceteIstasyon / ReceteIstasyonGrupOperasyon / UretimIstasyon / IstasyonTakipHareket SET IstasyonKodu, IstasyonAdi ...` ve `UPDATE IstasyonKontrol SET IstasyonKodu ...` — istasyon kodu yeniden adlandirma yayilimi.
- API: -
**Istasyon sirasiyla iliskisi:** Dogrudan ve kritik. Istasyon-operasyon eslesmesini burada kurulan kart belirler; uretim akisi operasyon `Sira`'sina gore ilerlerken `ReceteIstasyonGrupOperasyon` (GrupKodu+OperasyonKodu->IstasyonKodu) ile her operasyona TEK `UretimIstasyon` olusturulur. Kart silme/yeniden adlandirma bu zinciri etkiledigi icin Manager kod degisimini tum bagli tablolara senkronlar.
**Notlar:** Namespace `MyUI.UretimIstasyonModule` (dosya IstasyonModul klasorunde olsa da). Grid duzeni `IstasyonKartlariListesi`. `AcilisBittimi` bayragi combo Leave olaylarinin acilis sirasinda tetiklenmesini engeller. `BtnDegistir_Click`/`BtnYeni_Click` event'leri `EventlerBagla()` icinde kod ile baglanir (Designer'da degil).

### Istasyon Aciklamalari (`FrmIstasyonAciklamalari.cs` / `FrmIstasyonAciklamalari.Designer.cs`)
**Ne ise yarar:** Saha tabletinde operatorun secebilecegi **kodlu aciklama listelerini** yonetir; tek form, `AciklamaModulTuru` enum'una gore 3 farkli amac icin acilir: **Istasyon Baslatma Hatasi**, **Istasyon Durdurma Kodu**, **Istasyon Fire Sebebi**. Her kayit kod+deger, opsiyonel personel gorevi, SMS gonderim bayragi ve SMS sablon kodu icerir. `MyFrmKayit` turevi.
**Once ne olmali (onkosul):** Form, `AciklamaModulTuru` set edilerek acilmali (`FrmAna` menusunden: `IstasyonBaslatmaHata` / `IstasyonDurdurmaKodu` / `IstasyonFireSebep`). Personel gorevi combo'su icin Personel tablosunda Gorevi degerleri bulunmali.
**Sonra ne olur:** Kaydet -> `IIstasyonAciklamaService.InsertOrUpdate(mdl)`; `Modul` alani enum'un string karsiligi olarak yazilir. Sil -> `IIstasyonAciklamaService.Delete(mdl)`. Liste yalnizca o modul turunun kayitlarini gosterir (`where Modul='<tur>' Order By Kodu`). Bu kayitlar saha tabletinde (TabletV2) durdurma/baslatma-hata/fire ekranlarinda secim listesi olarak okunur.
**Butonlar & kisayollar:**
- `Kaydet` (`BtnKaydet`, base) — `Kaydet()` -> kod zorunlu dogrulamasi -> `AktarModele()` (Modul=enum.ToString()) -> `InsertOrUpdate` -> `Bagla()`.
- `Sil` (`BtnSil`, base) — onay sorar, `Sil()` -> `_srv.Delete(_mdl)` -> `Bagla()`.
- `Yeni` (`BtnYeni`, base) — `TemizleText()`.
- `Düzenle` (`BtnDuzenle`, base) — secili satiri klonlayip forma yukler.
- `SmsGonder` (`ChcSmsGonder`) — bu aciklama secilince SMS gonderilsin mi.
- `SmsKodu` (`TxtSmsKodu`) — SMS sablon kodu; `@IstasyonKodu` -> istasyon kodu, `@Personel` -> aktif kullanici ile degistirilir (form ustundeki bilgi etiketleri myLabel5/myLabel6).
- `Per.Görevi` (`CmbGorevi`) — bu aciklamaya/SMS'e bagli personel gorevi.
- Grid cift tiklama / Enter — secim modunda satiri secip kapatir, degilse `BtnDuzenle.PerformClick()`.
- `Kapat` (`BtnKapat`, base) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IIstasyonAciklamaService.SelectListWhere`, `InsertOrUpdate`, `Delete` (`_srv`, `Ortak.DbPro.IstasyonAciklama`) — aciklama CRUD.
- Manager/Service: `IGenelService.GrupListesi("Personel", "Gorevi")` (`_srvGenel`, `Ortak.DbPro.GenelServis`) — gorevi combo'su icin distinct deger listesi.
- SQL/Prosedur: `where Modul='<IstasyonAciklamaModulTuru>' Order By Kodu` — liste filtresi.
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Manager kullanmaz, dogrudan service ile InsertOrUpdate yapar (Kart/Bakim'in aksine). `AciklamaModulTuru` enum degerleri: `IstasyonBaslatmaHata`, `IstasyonDurdurmaKodu`, `IstasyonFireSebep` (`My.Entities.IstasyonAciklamalar`). Form basligi ve `lblBaslik` calismada enum adina gore set edilir (Designer'daki "FrmIstasyonBaslatmaHatalari" override edilir). Grid duzeni `IstasyonAciklamalariListesi1`; `Id` ve `Modul` sutunlari gizlenir.

### Istasyon Bakim Listesi (`FrmIstasyonBakimList.cs` / `FrmIstasyonBakimList.Designer.cs`)
**Ne ise yarar:** Istasyonlara ait bakim/ariza kayitlarini istasyon ve tarih araligina gore listeleyen ana ekran; ust grid bakim baslik kayitlarini, alt grid secili bakimin **degisen parcalarini** gosterir (master-detay). Buradan yeni bakim eklenir veya mevcut bakim duzenlenir. `MyFrmListe` turevi (master-detay split grid).
**Once ne olmali (onkosul):** Istasyon kartlari (`IstasyonKarti`) tanimli olmali (istasyon filtre combo'su icin). Form `FrmAna`'dan MDI cocuk olarak `Show()` ile acilir.
**Sonra ne olur:** Bu ekran kayit yapmaz; `Ekle`/cift tiklama -> `FrmIstasyonBakimEkle` popup'i acar. Popup `KayitEdildi=true` ile kapanirsa `BtnAra` tetiklenir ve liste tazelenir, onceki satir secimi korunur. Ust grid satiri degisince alt parca grid'i `BaglaDetay()` ile yeniden yuklenir.
**Butonlar & kisayollar:**
- `Ara` (`BtnAra`) — `Bagla()`; istasyon kodu + tarih1/tarih2 araligina gore bakim listesini ceker.
- `Ekle` (`BtnEkle`) — bos `FrmIstasyonBakimEkle` acar (yeni bakim).
- `Istasyon` (`CmbIstasyon` combo) — istasyon koduna gore filtre.
- `Tarihi` (`TxtTarihi1`/`TxtTarihi2` tarih kutulari) — bakim tarihi alt/ust siniri (CAST DATE karsilastirmasi).
- Ust grid cift tiklama / Enter (`MyView1_MyEventDoubleClickEnter`) — secili bakimi `FrmIstasyonBakimEkle { IdGuid = itm.Id }` ile duzenlemeye acar.
- Ust grid satir degisimi (`MyView1_FocusedRowChanged`) — alt parca grid'ini gunceller.
- `Kapat` (`BtnKapat`, base) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IIstasyonBakimService.SelectListWhere(sor)` (`_srv`, `Ortak.DbPro.IstasyonBakim`) — bakim baslik listesi (istasyon+tarih filtreli).
- Manager/Service: `IIstasyonBakimParcaService.SelectListWhere(" where IstBakId='...'")` (`_srvParca`) — secili bakimin parca detaylari.
- Manager/Service: `IIstasyonKartiService.SelectListWhere("")` (`_srvIst`) — istasyon filtre combo'sunu doldurur.
- SQL/Prosedur: Dinamik `where 1=1 and IstasyonKodu='...' AND CAST(coalesce(Tarih,'1901-01-01') AS DATE) >= / <= CAST('...' AS DATE)` — istasyon + tarih araligi filtresi.
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Tarih kutulari Designer'da "24.05.2023" sabit baslangic degeriyle gelir. Ust grid duzeni `IstasyonBakimList`, alt grid `IstasyonBakimParcaList`. `BtnEkle_Click` event'i Designer'da baglanir.

### Istasyon Bakim Ekle (`FrmIstasyonBakimEkle.cs` / `FrmIstasyonBakimEkle.Designer.cs`)
**Ne ise yarar:** Tek bir bakim/ariza kaydini (istasyon, tarih, personel, islem turu, aciklama) ve ona ait degisen parca listesini olusturma/duzenleme popup'i. `MyFrmKayit` turevi; alt sekmede parca grid'i (Ekle/Sil panel butonlari) bulunur.
**Once ne olmali (onkosul):** Istasyon kartlari (`IstasyonKarti`) tanimli olmali. `FrmIstasyonBakimList`'ten yeni icin bos, duzenleme icin `IdGuid` set edilerek acilir.
**Sonra ne olur:** Kaydet -> `IstasyonBakimManager.Kaydet(mdl, parcalar)` tek transaction'da: (1) `IstasyonBakim` baslik InsertOrUpdate; (2) bu bakima ait eski `IstasyonBakimParca` kayitlarini sil; (3) gecerli parca listesini InsertOrUpdate. Audit alanlari (KayitEden/KayitTarihi yeni kayitta, Degistiren/DegistirmeTarihi her zaman) `Ortak.KullaniciAdi` ile doldurulur. Sil -> `IstasyonBakimManager.Sil(mdl)` (baslik + tum parcalar). Basariliysa `ActionAktar` callback'i tetiklenir, `KayitEdildi=true`, form kapanir; liste ekrani tazelenir.
**Butonlar & kisayollar:**
- `Kaydet` (`BtnKaydet`, base) — `Kaydet()` -> dogrulama (istasyon kodu zorunlu, tarih bossa now) -> `AktarModele()` -> `Manager.Kaydet()` -> kapan.
- `Sil` (`BtnSil`, base) — onay sorar, `Manager.Sil()` -> kapan.
- `Ekle` (`BtnStokEkle`, sag panel) — `FrmIstasyonBakimParcaEkle` popup'i (yeni parca) acar; donen parca listeye eklenir ve grid yenilenir.
- `Sil` (`BtnStokSil`, sag panel) — onay sorar, secili parcayi listeden cikarir (DB kaydi Kaydet'te transaction ile guncellenir).
- `IstasyonKodu` (`CmbIstasyonKodu` combo) — bakim yapilan istasyon.
- `Personel` / `Bakım/Islem Turu` / `Açıklaması` / `Tarihi` (`TxtPersonel`/`TxtIslemTuru`/`TxtAciklama`/`TxtTarih`) — bakim baslik alanlari.
- `Kapat` (`BtnKapat`, base) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IstasyonBakimManager.Kaydet(IstasyonBakim mdl, List<IstasyonBakimParca> parcalar)` — transaction'li baslik+parca kaydi (eski parcalari silip yeniden yazar).
- Manager/Service: `IstasyonBakimManager.Sil(IstasyonBakim mdl)` — transaction'li baslik+parca silme.
- Manager/Service: `IIstasyonBakimService.SelectFirst(c => c.Id == IdGuid)` (`_mng.IstBakimService`) — duzenlemede baslik yukleme.
- Manager/Service: `IIstasyonBakimParcaService.SelectList(c => c.IstBakId == IdGuid)` (`_mng.IstParcaService`) — duzenlemede parca yukleme.
- Manager/Service: `IIstasyonKartiService.SelectListWhere("")` (`_mng.IstKartService`) — istasyon combo'sunu doldurur.
- SQL/Prosedur: Manager icinde `IstasyonBakim` InsertOrUpdate + `IstasyonBakimParca` Delete (IstBakId) + InsertOrUpdate (Dapper DAL uretir).
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Manager `DatabaseFactoryMikro` da alir (`_dbMikro`) ancak bu formda kullanilmaz (yalniz Pro DB'ye yazar). Parca grid'inde Parca/ParcaNo/EvrakNo/Aciklama/Garanti sutunlari inline duzenlenebilir (`SutunReadOnlyKapat`). `Id`/`IstBakId` gizlenir. Grid duzeni `IstasyonBakimEkleParcalar`. `ActionAktar` callback'i opsiyonel; cagiran liste ekrani genelde `KayitEdildi` bayragiyla yenileme yapar.

### Istasyon Bakim Parca Ekle (`FrmIstasyonBakimParcaEkle.cs` / `FrmIstasyonBakimParcaEkle.Designer.cs`)
**Ne ise yarar:** Bir bakim kaydina eklenecek tek bir degisen parcanin bilgilerini (parca adi, parca no, evrak no, aciklama, garanti) girmek icin kullanilan sade popup. `MyFrmSade` turevi.
**Once ne olmali (onkosul):** `FrmIstasyonBakimEkle` icinden `Parca` ozelligi (yeni `IstasyonBakimParca`, Id atanmis) ve `YeniKayit=true` set edilerek acilir.
**Sonra ne olur:** Kaydet -> parca adi bos degilse `AktarModele()` ile gelen `Parca` nesnesini doldurur, `KayitEdildi=true` set edip kapanir. DB'ye dogrudan YAZMAZ; donen parca cagiran `FrmIstasyonBakimEkle`'nin bellek listesine eklenir ve esas kayit oradaki Kaydet ile transaction icinde DB'ye yazilir.
**Butonlar & kisayollar:**
- `Kaydet` (`BtnKaydet`, base) — Parca bos ise uyarir; degilse `AktarModele()`, `KayitEdildi=true`, kapan.
- `Parça` / `Parça No` / `Evrak No` / `Açıklama` (`TxtParca`/`TxtParcaNo`/`TxtEvrakNo`/`TxtAciklama`) — parca alanlari.
- `Garanti` (`ChcGaranti`) — garanti kapsaminda mi.
- `Kapat` (`BtnKapat`, base) — kaydetmeden kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: - (servis/Manager cagrisi yok; yalniz in-memory nesne doldurur)
- SQL/Prosedur: -
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** En basit form. Persist islemi tamamen cagiran `FrmIstasyonBakimEkle` + `IstasyonBakimManager.Kaydet()` sorumlulugunda. Tek zorunlu alan `TxtParca`.
