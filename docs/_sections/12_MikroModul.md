## Modul: MikroModul

MikroModul, UretimV4 (CepPatronERP) masaustu uygulamasinin Mikro ERP (MikroDB_V16_FEZA24) ile entegrasyon koprusudur. Iki yonlu calisir: (1) Mikro tarafindan veri okuma/secme — Cari, Stok, Siparis ve Recete listeleri (cogu zaman baska formlara "secim penceresi" olarak acilir, `SecimIcinAcildi` bayragiyla); (2) Uretim sonuclarini Mikro'ya geri yazma — is emri uretim/fire/sarf hareketlerini Mikro `STOK_HAREKETLERI` (+ `BEDEN_HAREKETLERI`, `PARTILOT`) tablolarina fis olarak kaydetme, kaydedilen fisleri listeleme/silme. Tum formlar `My.Kontrol` kutuphanesindeki base formlardan turer: liste formlari `MyFrmListe` (arama paneli + grid + alt buton seridi `BtnAra`/`BtnTemizle`/`BtnYazdir`/`BtnDizayn`/`BtnKapat`), kayit/popup formlari `MyFrmKayit` (`BtnKaydet`/`BtnKapat`/`BtnSil` + navigasyon butonlari). Veri erisimi iki ayri DatabaseFactory uzerinden yapilir: `Ortak.DbMikro` (Mikro ERP) ve `Ortak.DbPro` (UretimV3_FEZA uretim DB). Mikro'ya yazma mantigi `MikroKayitManager` (transaction + parti/lot/renk/beden hesaplama) ve `MikroConvertManager` (StokHareketleriModel -> MikroStokHareketleri fis turune gore donusum, evrak seri/sira atama) siniflarinda toplanir; fis turleri kullanici tanimli `MikroEntegre` ayarlarindan (`Ortak.MikroEntAyarlar`) okunur.

### Mikro Cari Listesi (`FrmMikroCariListesi.cs` / `.Designer.cs`)
**Ne ise yarar:** Mikro ERP'deki cari (musteri/tedarikci) kartlarini arar ve listeler. Cogunlukla baska ekranlardan "cari sec" diyalogu olarak acilir (FrmUretimEmriED, FrmIstasyonKartlari, FrmReceteOperasyonED, OperasyonIstasyonControlV2 vb.).
**Once ne olmali (onkosul):** Mikro DB baglantisi (`Ortak.DbMikro`) hazir olmali. Secim modunda acan formun `SecimIcinAcildi = true` set etmesi gerekir.
**Sonra ne olur:** Salt-okunur listeleme; Mikro'da degisiklik yapmaz. Secim modunda satira cift tiklayinca `SecilenKod` (cari_kod), `SecilenRow` (MikroCari) doldurulur, `Secildi=true` yapilip form kapanir; cagiran form bu degerleri okur.
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — arama kriterleriyle listeyi yeniden yukler (`Bagla()`).
- `Temizle` (BtnTemizle) — Kodu/Unvani/Ara metin kutularini temizler.
- `Yazdir`/`Dizayn` (BtnYazdir/BtnDizayn) — grid yazdirma/kolon dizayni (base form ozelligi).
- `Kapat` (BtnKapat) — formu kapatir.
- Grid cift tik / Enter — secim (MyEventDoubleClickEnter).
- Arama kutulari: `Kodu`, `Ünvanı`, `Ara` (kod+unvan birlikte).
**Cagirdigi katmanlar:**
- Service: `IMikroCariService.GetViewListWhere(where)` (`Ortak.DbMikro.Cariler`) — verilen WHERE ile cari view listesini ceker (cari_kod / cari_unvan1 / cari_unvan2 LIKE filtreleri SorguAyarla ile uretilir).
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Filtreler string birlestirme ile uretilir (parametrik degil). `CrGuid` kolonu gizlenir.

### Mikro Stok Listesi (`FrmMikroStokListesi.cs` / `.Designer.cs`)
**Ne ise yarar:** Mikro ERP stok kartlarini ana grup, stok cinsi (Mamul/Hammadde/Yari Mamul vb.) ve kod/ad ile arar ve listeler. Recete tasarim ekranlarinda (FrmReceteED, FrmReceteDetayED, FrmUretimTalepED) stok secimi icin acilir.
**Once ne olmali (onkosul):** `Ortak.DbMikro` ve `Ortak.MikroStokGrubu` ayari hazir olmali. `TumStoklar=true` ise acilista otomatik yukleme yapilmaz (kullanici filtreleyip arar). Secim modunda `SecimIcinAcildi=true`.
**Sonra ne olur:** Salt-okunur listeleme. Secim modunda cift tik/Enter ile `SecilenKod`=StokKodu, `SecilenRow`=MikroStok doldurulur, form kapanir.
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — `Bagla()` ile filtreli listeyi yukler.
- `Temizle` (BtnTemizle) — StokKodu/StokAdi/AnaGrubu/Ara alanlarini temizler.
- `Kapat` / `Yazdir` / `Dizayn` — base form butonlari.
- Grid cift tik / Enter — secim.
- Filtreler: `StokKodu`, `StokAdi`, `AnaGrubu` (combo), `StokCinsi` (combo), `Ara`.
**Cagirdigi katmanlar:**
- Service: `IMikroStokService.GetViewListWhere(where, Ortak.MikroStokGrubu)` (`Ortak.DbMikro.Stoklar`) — stok view listesi (sto_kod/sto_isim/anagrup/sto_cins filtreli).
- Service: `IMikroGenelService.GrupListesi("STOKLAR", Ortak.MikroStokGrubu)` — ana grup combosu icin distinct grup listesi.
- Manager: `MikroStokCinsiManager.GetCinsListFull()` — stok cinsi (kod+ad) listesini doldurur; `GetCinsiKodu(ad)` ile secilen cinsin sto_cins kodu bulunur.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Stok cinsi secilmezse (-2) cins filtresi uygulanmaz.

### Mikro Siparis Listesi (`FrmMikroSiparisListesi.cs` / `.Designer.cs`)
**Ne ise yarar:** Mikro ERP'deki acik/kapali satis siparislerini tarih ve cari ile listeler; secilen siparisin satir hareketlerini alt gridde gosterir. Sag-tik menusunden secili Mikro siparisinden UretimV4 is emri (Siparis/MikroSiparis) olusturulur. UretimV4 tarafinda zaten aktarilmis siparisler yesil/menekse renkle isaretlenir.
**Once ne olmali (onkosul):** `Ortak.DbMikro` ve `Ortak.DbPro` hazir olmali. Is emri olusturmak icin ilgili Mikro stok kodlarina karsilik UretimV4'te `ReceteAna.EntegreStokKodu` eslesen receteler tanimli olmali.
**Sonra ne olur:** Listeleme salt-okunur. Sag-tik "Uretim EmriOlustur/Guncelle" -> `FrmMikroUretimEmriOlustur` acilir; kayit sonrasi UretimV4 `Siparis`/`SiparisHareket`/`SiparisHareketDetay` tablolarina yazilir ve liste `Aktarildi` olarak isaretlenir (geri cagrim `Action=Bagla`).
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — `Bagla()` filtreli liste + ilk siparisin hareketlerini yukler.
- `Temizle` (BtnTemizle) — kod/ad/ara/tarih filtrelerini temizler.
- `Kapalı Siparişler` (ChcSiparisAcikKapali) — isaretli=Kapali, degil=Acik siparisler.
- Tarih filtreleri: `TxtTarihi1` (varsayilan: bugun-1 yil), `TxtTarihi2`.
- Sag-tik menu: `Uretim EmriOlustur/Guncelle` — FrmMikroUretimEmriOlustur acar.
- Grid cift tik/Enter (secim modunda) — `SecilenKod`=CariKodu ile kapanir.
**Cagirdigi katmanlar:**
- Service: `IMikroSiparisService.GetViewListWhere(where)` (`Ortak.DbMikro.Siparisler`) — siparis view (cari kodu/unvani, tarih, acik/kapali filtreli).
- Service: `IMikroSiparisHareketService.GetViewListSeriSira(seri, sira)` (`Ortak.DbMikro.SiparisHareketler`) — secili siparisin satir hareketleri.
- Service: `ISiparisService.SelectListWhere(" where Turu ='MikroSiparis' ")` (`Ortak.DbPro.Siparis`) — UretimV4'te aktarilmis siparisleri tespit edip `Aktarildi=true` isaretler (SiparisKodu = EvrakSeri+EvrakSira eslesmesi).
**Istasyon sirasiyla iliskisi:** Bu ekrandan olusan is emri, FrmMikroUretimEmriOlustur -> FrmUretimEmriED akisina baglanir; istasyon/operasyon yapisi recete uzerinden ileride kurulur.
**Notlar:** `SipGuid` gizlenir. Aktarilmis satirlar RowStyle ile yesil (myView1) gosterilir.

### Mikro Recete Listesi (`FrmMikroReceteListesi.cs` / `.Designer.cs`)
**Ne ise yarar:** Mikro ERP urun recetelerini (URUN_RECETELERI) listeler; secilen recetenin UretimV4 (Pro) karsiligini ve operasyonlarini alt gridlerde gosterir. UretimV4'e aktarilmis receteler yesil isaretlenir. Sag-tik menu ile receteyi UretimV4'e iceri aktarma ve fire yuzdesi guncelleme ekranlari acilir.
**Once ne olmali (onkosul):** `Ortak.DbPro` ve `Ortak.DbMikro` hazir olmali; `MikroReceteManager` (her ikisi ile) kurulu olmali.
**Sonra ne olur:** Listeleme salt-okunur. Sag-tik "Receteyi İçeri Aktar" -> `FrmReceteED` (MikrodanAktar=true) ile UretimV4 recetesi olusturulur; "Recete Fire Yüzde Guncelle" -> `FrmMikroReceteFireGuncelle` acilir.
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — `Bagla()` ile listeyi yukler (rec_iptal=0 + filtre).
- `Temizle` (BtnTemizle) — Ara alanini temizler.
- `comboBox1` durum filtresi: Aktif (varsayilan) / Pasif / Tümü (sto_pasif_fl uzerinden).
- Sag-tik menu: `Receteyi İçeri Aktar`, `Recete Fire Yüzde Guncelle`.
- Grid cift tik/Enter (secim modunda) — `SecilenKod`=ReceteKodu ile kapanir.
**Cagirdigi katmanlar:**
- Manager: `MikroReceteManager.GetMikroReceteList(where)` — URUN_RECETELERI'den (NOLOCK) recete basliklarini ceker (rec_anakod, fn_StokIsmi, fn_StokBirimi, rec_anamiktar, sto_pasif_fl).
- Service: `IReceteAnaService.SelectListWhere()` (`Ortak.DbPro.ReceteAna`) — UretimV4 recetelerini cekip eslesenleri `Aktarildi` isaretler; secilen recetenin Pro karsiligini gosterir.
- Service: `IReceteOperasyonService.SelectList(c => c.RcAId == rcaid)` (`Ortak.DbPro.ReceteOperasyon`) — secilen Pro recetesinin operasyonlari.
**Istasyon sirasiyla iliskisi:** Pro recetesinin operasyonlari (Sira bazli) bu ekranda goruntulenir; uretim akisinin temelini olusturan operasyon sirasi burada izlenebilir.
**Notlar:** Aktarilmis receteler GridView_RowStyle ile yesil. Pro grid kolonlari (Id, RcAGuid, RcAId, RcOGuid) gizlenir.

### Mikro Uretim Emri Olustur (`FrmMikroUretimEmriOlustur.cs` / `.Designer.cs`)
**Ne ise yarar:** Secilen bir Mikro satis siparisinden, satirlardaki stok kodlarina karsilik gelen UretimV4 recetelerini bularak otomatik bir UretimV4 is emri (Siparis turu="MikroSiparis") olusturur. Ust gridde Mikro siparis basligi, alt gridde Mikro siparis hareketleri gosterilir.
**Once ne olmali (onkosul):** FrmMikroSiparisListesi'nden bir siparis secilip `SipGuid` set edilmis olmali. Her Mikro stok kodu icin `ReceteAna.EntegreStokKodu` eslesen bir UretimV4 recetesi tanimli olmali — yoksa "Ürüne ait Reçete Bulunamadı" hatasi verir ve form kapanir.
**Sonra ne olur:** `Kaydet` ile UretimV4 `Siparis` + `SiparisHareket` + `SiparisHareketDetay` tablolarina yazilir (SiparisManager.SiparisKaydet transaction icinde eski hareket/detaylari silip yenisini insert eder). Ardindan `Action?.Invoke()` (cagiran liste yenilenir) ve `FrmUretimEmriED` (UretimTuru="MikroSiparis") acilir; bu form kapanir.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — `SiparisManager.SiparisKaydet(_mdl, YeniKayit)` cagirir; basarili olursa is emri duzenleme formuna gecer.
- `Kapat` (BtnKapat) — base form ile kapatir.
**Cagirdigi katmanlar:**
- Service: `IMikroSiparisService.GetViewListWhere(" where Sip.SipGuid =...")` — Mikro siparis basligini ceker.
- Service: `IMikroSiparisHareketService.GetViewListSeriSira(seri, sira)` — Mikro siparis satirlarini ceker.
- Manager: `SiparisManager.GetSiparis()` — bos SiparisKayitModel; `SiparisManager.SiparisKaydet(mdl, yenikayit)` — UretimV4 siparisini transaction'la kaydeder.
- Manager: `ReceteManager.GetReceteKayit(rcAId)` — eslesen recetenin tam kaydini (detaylar dahil) getirir; ilk detaydan hareket stok bilgisi, tum detaylardan SiparisHareketDetay olusturulur.
- DAL: `_dbPro.ReceteAna.SelectFirst(c => c.EntegreStokKodu == sipH.StokKodu)` — stok koduna gore recete eslestirme.
**Istasyon sirasiyla iliskisi:** Olusan is emri sonradan FrmUretimEmriED'de operasyon/istasyon yapisiyla devam eder; recetenin operasyon sirasi uretim akisinin temelidir.
**Notlar:** Her Mikro siparis satiri icin tek recete secilir (`ReceteTekSec`). Hareketler `EntKayitSeri/Sira/Guid` ile Mikro siparis satirina baglanir (geri izlenebilirlik).

### Mikro Recete Fire Yüzde Güncelle (`FrmMikroReceteFireGuncelle.cs` / `.Designer.cs`)
**Ne ise yarar:** Mikro recetesindeki tuketim kalemlerinin fire yuzdelerini, UretimV4'e aktarilmis recetenin detaylarina (ReceteDetay.FireYuzde) toplu olarak gunceller. Ust gridde Mikro recete detaylari (referans), alt gridde duzenlenebilir Pro recete detaylari gosterilir.
**Once ne olmali (onkosul):** FrmMikroReceteListesi sag-tik menusunden acilmis ve `MikroReceteKodu` + `MikrodanAktar=true` set edilmis olmali. Recete daha once UretimV4'e aktarilmis olmali (ReceteDetay kayitlari icin).
**Sonra ne olur:** `Kaydet` ile `ReceteManager.ReceteDetayFireYuzdeGuncelle(receteDetaylar)` cagrilir; UretimV4 `ReceteDetay` tablosundaki fire yuzdeleri guncellenir, mesaj verilip form kapanir.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — fire yuzde guncellemeyi yazar.
- Alt grid duzenlenebilir kolonlar: `ReceteSira`, `Miktar`, `Ebat`, `Gram`, `Olcu`, `FireYuzde` (yesil baslikli).
- `Stok Cinsi` combo, `Recete Grubu` combo, entegre stok kodu/adi/birim/model kodu alanlari (cogu salt bilgi).
**Cagirdigi katmanlar:**
- Manager: `ReceteManager.GetReceteKayit()` — bos recete kayit modeli; `GetReceteDetayByReceteKodu(receteKodu)` — Pro recete detaylari; `ReceteDetayFireYuzdeGuncelle(list)` — fire yuzdelerini gunceller.
- Manager: `MikroReceteManager.GetMikroReceteList(where)` — Mikro recete basligi; `GetMikroReceteHareketler(receteKodu)` — Mikro tuketim kalemleri (rec_fireyuzde dahil).
- Service: `IMikroStokService.GetViewListWhere(...)` — entegre stok bilgisi/cinsi; `IGenelService.GrupListesi("ReceteAna","Grubu")` — recete grup combosu.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Mikro detay sirasi (ReceteSira) ile Pro detay sirasi eslestirilerek fire yuzdesi tasinr. EvrakNoAl ile gerekirse yeni recete kodu uretilir.

### Mikroya Uretim Kaydet V2 (`FrmMikroyaUretimKaydetV2.cs` / `.Designer.cs`)
**Ne ise yarar:** Modulun ana yazma ekrani. Tamamlanmis bir UretimV4 is emrinin sonuclarini (uretilen urun + fire + sarf edilen stoklar + stok firesi) Mikro ERP'ye stok hareketi fisleri olarak kaydeder. Ust gridde is emri hareketleri (uretim/fire miktarlari), alt gridde kullanilan/cikan stoklar gosterilir.
**Once ne olmali (onkosul):** Is emri uretim girisleri tamamlanmis olmali (UretimIstasyonHareket -> ... miktarlar toplanmis). FrmSiparisListesi'nden `SipId` set edilerek acilir. `Ortak.MikroEntAyarlar` (MikroEntegre fis turu/maliyet ayarlari) ve `Ortak.IstasyonAyarlarBagla()`, `Ortak.MalKabulKullan`, `Ortak.PlKapat` ayarlari yuklenir. Daha once aktarilmissa (`Siparis.Ent`) kullaniciya eski fisleri silmesi gerektigi uyarisi cikar.
**Sonra ne olur:** `Kaydet` butonu, ayarlara gore urun girisi / urun fire cikisi / stok cikisi / stok fire cikisi listelerini StokHareketleriModel olarak olusturur, maliyet hesaplar (standart/recete maliyet), fis turune gore (StokVirman/UretimHareket/UretimdenGiris/SayimDepoGiris/UretimeCikis/SarfDepoCikis/FireCikis) `MikroConvertManager.Convert...` ile MikroStokHareketleri'ne donusturur ve `MikroKayitManager.StokHareketKaydet` ile Mikro `STOK_HAREKETLERI` (+ BEDEN_HAREKETLERI + PARTILOT + StokDepoRaf) tablolarina transaction icinde yazar. Basariliysa `SiparisManager.SiparisEntGuncelle` ile UretimV4 `Siparis`/`SiparisHareket` tablolarina Ent=1 + evrak seri/sira islenir; Kaydet butonu pasiflesir.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — tum fisleri Mikro'ya yazar (yukaridaki akis).
- `Yazdir` (BtnYazdir) — `BaglaKaliteYazdir()` ile kalite raporunu (KaliteRapor REPX) yazdirir.
- `myButton1` (gizli/yardimci) — mal kabul hesaplama onizleme formunu (`FrmMikroMalKabulHesaplama`) acar.
- `Kapat` (BtnKapat) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager: `SiparisManager.GetSiparis(SipId)` — is emri (Siparis + hareketler); `SiparisEntGuncelle(...)` — Ent + evrak seri/sira yazar; `GetKaliteYazdir(SipId)` — kalite rapor datasi.
- Manager: `ReceteManager.GetRafOmru(rcAId)` — urun raf omru (parti son kullanma tarihi icin).
- Manager: `MikroConvertManager.SetUretimUrunGirisFisiAyar / SetUretimUrunFireCikisFisiAyar / SetUretimStokCikisFisiAyar / SetUretimStokFireCikisFisiAyar` (depo/fis ayarlari) ve `ConvertStokVirmanFisi / ConvertUretimHareketFisi / ConvertUretimdenGirisFisi / ConvertSayimDepoGiris / ConvertUretimeCikisFisi / ConvertSarfDepoCikis / ConvertFireCikis` (fis turune gore MikroStokHareketleri + evrak seri/sira uretimi).
- Manager: `MikroKayitManager.GetMikroStokMaliyetListWhere(where)` — stok standart/recete maliyeti; `StokHareketKaydet(lisMikro, depoRaf)` — Mikro fis kaydi (transaction; parti/lot/renk-beden hesaplama dahil).
- Service: `IIstasyonTakipStokHareketService.GetViewListKullanimWherePartiLot / GetViewListKullanimWhereMalKabul / GetViewListKullanimMalKabulFis` — kullanilan/cikan stoklar; `IIstasyonTakipHareketDetayService.GetViewListStokFire(...)` — stok fireleri; `IIstasyonTakipStokHareketDetayService.DetaylarGuncelleBySipId(SipId)` — kayit oncesi detay senkronu; `ITempMikroStokService.SelectListWhere(...)` — birim katsayilari (Birim2/3/4, Katsayi) ile birim donusumu.
- Yardimci sinif: `MikroyaKaydetMalKabulHesaplama` / `MikroyaKaydetMalKabulFireHesaplama` — `Ortak.MalKabulKullan` aktifse parti/lot bazinda mal kabul fisine gore stok/fire dagitimi yapar.
- SQL/Prosedur: dolayli olarak fis kaydi sirasinda STOKLAR (sto_detay_takip/renk/beden), PARTILOT, BEDEN_HAREKETLERI; maliyet icin `fn_by_Stok_Son5_Giris_Fiyati`, `fn_StokIsmi`, `fn_StokBirimi` Mikro fonksiyonlari.
**Istasyon sirasiyla iliskisi:** Bu ekran uretim akisinin SONUNDADIR — istasyon takip hareketleriyle (uretim girisi, sarf, fire, mal kabul) biriken miktarlar burada Mikro'ya stok hareketi olarak aktarilir. Olcum/akis motoru (Uretim_MiktarGuncelle/PlanlananGuncelle/SonrakiIstasyonaGonder) is emri tarafinda calismis, miktarlar UretimIstasyonHareket->...->UrO seviyesine toplanmistir; burada yalnizca okunup fise donusturulur.
**Notlar:** Urun fiyati = (stok cikis tutar + fire cikis tutar) / toplam urun miktari olarak hesaplanip urun giris/fire fislerine yazilir. Birim donusumu TempMikroStok katsayilariyla yapilir (birimpntr 1-4). `.txt` uzantili eski V1 ayni isimli formun yedegidir (aktif degil; bu V2 dosyasi kullanilir).

### Mikroya Sarf Fire Kaydet (`FrmMikroyaSarfFireKaydet.cs` / `.Designer.cs`)
**Ne ise yarar:** Istasyon takip hareket detaylarindan dogan ara sarf cikis ve fire giris fislerini (IstasyonTakipHareketDetay) Mikro ERP'ye stok hareketi olarak kaydeder. Tam is emri kapatmadan, istasyon bazinda sarf/fire aktarimi icin kullanilir.
**Once ne olmali (onkosul):** FrmIstasyonFisList ekranindan aktarilacak `FisList` (List<IstasyonTakipHareketDetay>) doldurularak acilmis olmali. Acilista `DahaOnceKayitEdilmismi()` ile EntCode'u olan kayitlarin Mikro'da zaten var olup olmadigi kontrol edilir — varsa Kaydet butonu gizlenir (mukerrer aktarim engeli).
**Sonra ne olur:** `Kaydet` ile her detay, turune gore (SarfCikisFisi / FireGirisFisi) ve ayarlardaki fis turune gore (StokVirman / SarfDepoCikis / FireCikis) StokHareketleriModel'e cevrilir, `MikroConvertManager.Convert...` ile MikroStokHareketleri olusturulur, `MikroKayitManager.StokHareketKaydet(lisMikro)` ile Mikro'ya yazilir. Basariliysa her FisList kaydina `Ent=true`, `EntSeri/EntSira/EntDate` yazilip `IIstasyonTakipHareketDetayService.InsertOrUpdate(FisList)` ile UretimV4'e geri kaydedilir.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — sarf/fire fislerini Mikro'ya aktarir ve UretimV4 detaylarini Ent isaretler.
- `Kapat` (BtnKapat) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager: `MikroConvertManager.SetSarfCikisFisiTuruAyar / SetFireGirisFisiTuruAyar` (depo/ayar); `ConvertStokVirmanFisi / ConvertSarfDepoCikis` (fis donusumu).
- Manager: `MikroKayitManager.StokHareketIdKayitEdilmismi(id)` (mukerrer kontrol); `StokHareketKaydet(lisMikro)` (Mikro kayit).
- Service: `IIstasyonTakipHareketDetayService.InsertOrUpdate(FisList)` (`Ortak.DbPro.IstasyonTakipHareketDetay`) — Ent bilgilerini geri yazar.
**Istasyon sirasiyla iliskisi:** Saha akisinda istasyon bazli sarf/fire kayitlari (IstasyonTakipHareketDetay, Turu=SarfCikisFisi/FireGirisFisi) bu ekranda Mikro'ya aktarilir; is emri tam kapanmadan ara aktarim saglar.
**Notlar:** Her detaya benzersiz `EntCode` (Guid) atanip Mikro fis `sth_Guid` olarak kullanilir; bu sayede mukerrer aktarim tespiti yapilir.

### Mikro Mal Kabul Hesaplama (`FrmMikroMalKabulHesaplama.cs` / `.Designer.cs`)
**Ne ise yarar:** Mal kabul fisleri ile istasyon kullanim hareketlerini eslestirip, hangi stok/parti/lot'tan ne kadar dusulecegini hesaplayan onizleme/dogrulama ekranidir (FrmMikroyaUretimKaydetV2 icindeki MalKabul hesaplamasinin gorsel kontrolu). 4 grid: mal kabul fisi, istasyon hareket, hesaplanan, kalan.
**Once ne olmali (onkosul):** Acan form (FrmMikroyaUretimKaydetV2.myButton1) `MalKabulFis`, `IstasyonHareket`, `StokFireListPartili` listelerini doldurmali. `Ortak.MalKabulKullan` senaryosu icin anlamlidir.
**Sonra ne olur:** Salt hesaplama/onizleme; DB'ye yazmaz. `myButton1` -> `hsp.Convert()` (hesaplanan dagitim) ve `hsp.GetKalanList()` (artan mal kabul miktarlari) gridlere basilir.
**Butonlar & kisayollar:**
- `myButton1` — hesaplamayi calistirip hesaplanan + kalan listeleri gosterir.
**Cagirdigi katmanlar:**
- Yardimci sinif: `MikroyaKaydetMalKabulHesaplama.Convert()` — partili fireleri dusup mal kabul fisine gore istasyon hareketlerini parti/lot bazinda dagitir; `GetKalanList()` — kullanilmayan mal kabul kalanlari.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Bu ekran dogrudan menude degil, Mikroya Uretim Kaydet V2 icinden yardimci olarak acilir. Asil kayit FrmMikroyaUretimKaydetV2'de yapilir.

### Mikro Uretim Kaydedilen Fişler (`FrmMikroUretimKaydedilenFisler.cs` / `.Designer.cs`)
**Ne ise yarar:** Bir is emri (BelgeNo/SipId) icin Mikro'ya daha once aktarilmis stok hareketi fislerini listeler ve secilenleri Mikro'dan silmeyi saglar. Yanlis/tekrar aktarim sonrasi temizlik ve yeniden aktarim icin kullanilir.
**Once ne olmali (onkosul):** FrmSiparisListesi'nden `BelgeNo` ve `SipId` set edilerek acilmis olmali; o is emrine ait Mikro fisleri var olmali.
**Sonra ne olur:** `Sil` ile secili (`Sec=true`) fisler `MikroKayitManager.DeleteMikroAktarilanFisBySeriSira` ile Mikro `STOK_HAREKETLERI` (+ BEDEN_HAREKETLERI + PARTILOT) tablolarindan silinir. Tum fisler silindiyse (`fisKaldimi=false`) `SiparisManager.SiparisEntGuncelle(SipId,"","","","",0,0)` ile UretimV4'teki entegrasyon bayraklari (Ent=0, Kapandi=0) temizlenir; liste yeniden yuklenir.
**Butonlar & kisayollar:**
- `Sil` (BtnSil) — secili fisleri Mikro'dan siler (onay sorar).
- `Kapat` (BtnKapat) — formu kapatir (`BtnKapat_Click_1`).
- Grid `Sec` kolonu — silinecek fisleri isaretlemek icin duzenlenebilir checkbox.
**Cagirdigi katmanlar:**
- Manager: `MikroKayitManager.GetMikroAktarilanFisByBelgeNo(belgeNo)` — belge no'ya gore aktarilmis fisleri ceker; `DeleteMikroAktarilanFisBySeriSira(seri, sira)` — fisi (ve bagli beden/parti hareketlerini) transaction'la siler.
- Manager: `SiparisManager.SiparisEntGuncelle(...)` — fis kalmayinca UretimV4 entegrasyon bayraklarini sifirlar.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Mal kabul fisi turu (sth_tip=2, sth_cins=6, sth_evraktip=2) listeden haric tutulur. Silme parti/lot ozelkod3='AKT' ve belge_no eslesmesine dayanir (yalnizca bu uygulamanin actigi fisler).
