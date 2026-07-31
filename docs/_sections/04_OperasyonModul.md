## Modul: OperasyonModul

OperasyonModul, uretim akisinin temel yapi tasi olan "operasyon" tanimlarinin (orn. Kesim, Kaynak, Boya, Montaj) kart bazli yonetimini saglar. Klasorde tek bir ekran vardir: `FrmOperasyonKartlari`. Bu ekran operasyon kodu/adini ve her operasyona atanmis "varsayilan istasyon" bilgisini tutar. Operasyon kartlari, recete tarafinda operasyon sirasi (ReceteOperasyon) ve istasyon eslestirme (ReceteIstasyon / ReceteIstasyonGrupOperasyon) kurulurken referans liste olarak kullanilir; dolayisiyla miktar/akis motorunun OPERASYON-Sira bazli calismasinin sozluk/temel veri katmanidir. Ekran iki modda calisir: (1) ana menuden ShowDialog ile acilan klasik CRUD karti, (2) recete operasyon ekleme ekranindan `SecimIcinAcildi=true` ile acilan secim diyalogu.

### Operasyon Kartlari (`FrmOperasyonKartlari.cs` / `FrmOperasyonKartlari.Designer.cs`)
**Ne ise yarar:** Operasyon kartlarinin (OperasyonKodu, OperasyonAdi ve VarsayilanIstasyonKodu/Adi) eklenmesi, duzenlenmesi, silinmesi ve listelenmesi. Ayrica recete operasyon editorunden cagrildiginda operasyon secim diyalogu olarak gorev yapar.
**Once ne olmali (onkosul):** Istasyon kartlari onceden tanimlanmis olmali; cunku formun acilisinda `IstasyonKarti` listesi cekilip "Varsayilan Istasyon Kodu/Adi" lookup'larina (CmbIstasyonKodu/CmbIstasyonAdi) baglaniyor. Ekran ana menuden `FromAc.OperasyonKartlari()` ile `ShowDialog` olarak acilir. Veri tabani baglantisi (`Ortak.DbPro`) hazir olmali.
**Sonra ne olur:**
- Kaydet: `OperasyonKarti` tablosuna kayit eklenir/guncellenir (`InsertOrUpdate`). Eger duzenleme sirasinda OperasyonKodu degistiyse ayni transaction icinde su tablolarda eski kod yeni kodla (ve OperasyonAdi ile) toplu UPDATE edilir: `IstasyonKarti` (Operasyon, OperasyonAdi), `ReceteOperasyon`, `ReceteIstasyon`, `ReceteIstasyonGrupOperasyon`, `UretimOperasyon`, `IstasyonTakipHareket`. Liste yeniden yuklenir (`Bagla()`), text alanlar temizlenir.
- Sil: Secili `OperasyonKarti` satiri `OperasyonKarti` tablosundan silinir (`_srv.Delete`), liste yenilenir.
- Secim modunda (SecimIcinAcildi): satira cift tiklayinca/Enter ile secilen operasyon `SecilenRow/SecilenKod/SecilenId` ile cagiran ekrana (orn. `FrmReceteOperasyonED`) doner ve form kapanir; orada ReceteOperasyon satiri olarak eklenir.
- Ekran disari (kaydet/sil oldugunda) `KayitEdildi=true` set eder.
**Butonlar & kisayollar:**
- `Kaydet (BtnKaydet)` — `Kaydet()` cagirir: text dogrulamasi (OperasyonKodu bos olamaz), modele aktarim, `OperasyonManager.Kaydet`. (Base form: alt buton seridinde, genelde Enter/F-tusu base davranisi.)
- `Yeni (BtnYeni)` — `YeniKayit=true` yapar, text alanlari temizler (`TemizleText`).
- `Duzenle (BtnDuzenle)` — Grid'de secili satiri (`MyGetCurrentItem<OperasyonKarti>`) klonlayip text alanlara aktarir (`AktarTextlere`), `YeniKayit=false`.
- `Sil (BtnSil)` — Onay sorar ("Kaydi silmek istiyormusunuz.."), secili kayit yoksa uyarir, sonra `Sil()`.
- `Kapat (BtnKapat)` — Base form: formu kapatir.
- `Yazdir (BtnYazdir)` — Base formdan gelir (bu ekranda ozel kod baglanmamis).
- Navigasyon `BtnIlk / BtnOnceki / BtnSonraki / BtnSon` — Base form kayit gezinme butonlari (BindingSource `bs` uzerinde).
- Grid cift tiklama / Enter (`myView1.MyEventDoubleClickEnter`) — Secim modundaysa satiri secip kapatir; degilse `BtnDuzenle.PerformClick()` tetikler.
- Lookup `CmbIstasyonKodu/CmbIstasyonAdi` Leave — biri secilince digeri otomatik doldurulur (kod<->ad esleme), `AcilisBittimi` bayragiyla recursion engellenir.
**Cagirdigi katmanlar:**
- Manager/Service: `OperasyonManager.Kaydet(OperasyonKarti, bool yenikayit)` — yeni/guncelleme ayrimi yapar, `KodVarmi` ile ayni OperasyonKodu kontrolu yapar, transaction acar, `Service.InsertOrUpdate` ile ana kaydi yazar, kod degisikliginde bagimli 6 tabloyu UPDATE eder, commit eder.
- Manager/Service: `OperasyonManager.KodVarmi<T>(...)` — tablo adi/anahtar kolonu reflection ile bulup `Select count(*)` ile ayni kodun varligini denetler ("Aynı OperasyonKodu Kodla Kayıt Var").
- Manager/Service: `IOperasyonKartiService` (= `OperasyonKartiService : BaseService<OperasyonKarti>`) metotlari: `SelectListWhere(" Order By OperasyonKodu")` (liste), `SelectFind(Id)` (eski kaydi bulma), `InsertOrUpdate`, `Delete`, `GetConnection`, `Execute`, `Query<int>`.
- Manager/Service: `IIstasyonKartiService.SelectListWhere(" Order By IstasyonKodu ")` — varsayilan istasyon lookup verisi.
- Manager/Service: `IGenelService` (`_srvGenel`) — enjekte edilmis fakat bu ekranda aktif kullanim yok.
- DataAccess: `OperasyonKartiDal : BaseDal<OperasyonKarti>` — `[Table("OperasyonKarti")]` POCO uzerinden Dapper CRUD.
- SQL/Prosedur: Stored procedure cagrilmaz. Kod degisikliginde inline UPDATE'ler (yukarida listelenen 6 tablo). Sema: `OperasyonKartiCreates.OperasyonKartiCreate()` -> OperasyonKarti tablosu (OperasyonKodu varchar(50), OperasyonAdi varchar(150), VarsayilanIstasyonKodu varchar(50), VarsayilanIstasyonAdi varchar(150)).
- API: - (UretimV4 dogrudan DB'ye baglanir, API kullanmaz.)
**Istasyon sirasiyla iliskisi:** Operasyon kartlari, miktar/akis motorunun OPERASYON-Sira bazli isleyisinde temel sozluktur. Recetede operasyon sirasi (ReceteOperasyon.Sira) ve operasyon->istasyon eslestirmeleri (ReceteIstasyon, ReceteIstasyonGrupOperasyon) bu kartlardaki OperasyonKodu uzerine kurulur. Her operasyon icin tanimlanan "VarsayilanIstasyonKodu", recete operasyon ekleme sirasinda otomatik istasyon eslestirmesi onerisinde kullanilir (bkz. FrmReceteOperasyonED). OperasyonKodu degisince ReceteIstasyonGrupOperasyon ve UretimOperasyon dahil tum akis tablolari guncellenerek sira/akis butunlugu korunur.
**Notlar:**
- OperasyonManager.Kaydet'teki bagimli tablo UPDATE'leri string interpolation ile kuruldugu icin SQL injection ve OperasyonAdi/Kodu icindeki tek tirnak riski tasir (gercek davranis; spekulasyon degil, koddan).
- Kaydetme hatasinda transaction `trs.Dispose()` ile birakiliyor ancak bazi dallarda explicit `Rollback` cagrilmiyor (finally'de yine Dispose var).
- Form base sinifi `MyFrmKayit` (My.Kontrol.Formlar, harici DLL) — alt buton seridi, navigasyon, `bs` BindingSource, `IdGuid/YeniKayit/KayitEdildi/AcilisBittimi/SecimIcinAcildi/Secildi/SecilenKod/SecilenRow/SecilenId` ve `MesajHata/MesajSor/MesajBilgi/MesajBilgi` mesaj yardimcilarini saglar; bu ekranda Enter=Kaydet/Esc=Kapat gibi kisayollar Designer'da acikca tanimlanmamis (base formdan gelir).
- `BtnDuzenle.Click` event'i `BtnDegistir_Click` metoduna baglidir (isim farki sadece kozmetik).
- Grid yerlesimi `myGrid1.MyGridKayitAdi = "OperasyonKartlariListesi"` adiyla saklanir/yuklenir; "Id" kolonu gizlenir (`SutunGizle`).
