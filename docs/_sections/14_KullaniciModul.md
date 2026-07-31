## Modul: KullaniciModul

UretimV4 (CepPatronERP) masaustu uygulamasinin kullanici yonetim modulu. Tek bir formdan (`FrmKullaniciKayit`) olusur ve uygulamaya giris yapacak kullanicilarin tanimlandigi yer/silindigi/duzenlendigi CRUD ekranidir. Yonettigi kayitlar `UretimV3_FEZA` veritabanindaki `Kullanici` tablosuna yazilir (`[Table("Kullanici")]`). Bu kayitlar dogrudan uretim/istasyon akisini etkilemez; uygulama acilisindaki `FrmLogin` ekraninda kullanici adi/sifre dogrulamasi ve `Admin` yetkisi icin kullanilir. Sifreler veritabaninda duz metin degil, AES anahtari (`Ortak.GetKey()` -> "OzelAnahtar1234%&") ile sifrelenmis olarak (`string.Sifrele/SifreCoz`) saklanir.

Form `MyFrmKayit` (My.Kontrol.Formlar kutuphanesindeki base liste+kayit formu) turevidir; standart alt bar butonlari (Kaydet, Yeni, Duzenle, Sil, Yazdir, Kapat ve kayit gezinme butonlari Ilk/Onceki/Sonraki/Son) base'den gelir. Form `SecimIcinAcildi` bayragi ile "secim modu"nda da acilabilir; bu modda grid'de bir satira cift tiklayinca/Enter'a basinca kullanici secilir ve cagiran forma `SecilenKod` (KullaniciAdi) + `SecilenRow` (Kullanici nesnesi) dondurulur.

### Kullanici Kaydi (`FrmKullaniciKayit.cs` / `FrmKullaniciKayit.Designer.cs`)
**Ne ise yarar:** Uygulamaya giris yapacak kullanicilari listeler ve ekleme/duzenleme/silme islemlerini yapar. Ust kisimda form alanlari (KullaniciAdi, Sifre, Adi, Soyadi, Admin onay kutusu), alt kisimda mevcut kullanicilarin grid listesi bulunur.
**Once ne olmali (onkosul):** Uygulamaya giris yapilmis olmali (`FrmLogin` -> `Ortak.DbPro` baglanti fabrikasi kurulmus olmali). Form, ana ekrandan (`FrmAna`) "Kullanicilar" menu/bar butonu (`BarBtnKullanicilar_ItemClick`) tiklaninca `new FrmKullaniciKayit().ShowDialog()` ile acilir. Pratikte yalnizca Admin kullanicilarin erismesi beklenir (yeni kullanici/yetki tanimi).
**Sonra ne olur:**
- Kaydet -> `Kullanici` tablosuna INSERT (yeni Id ile) veya UPDATE (mevcut Id) yapilir; grid yeniden yuklenir (`Bagla()`) ve form alanlari temizlenir (`Temizle()`).
- Sil -> secili satir `Kullanici` tablosundan DELETE edilir; grid yeniden yuklenir, alanlar temizlenir.
- Bu kayitlar daha sonra `FrmLogin.Kontrol()` tarafindan giris dogrulamasinda okunur (KullaniciAdi + sifrelenmis Sifre karsilastirmasi). Baska bir ekrana otomatik gecis yoktur; islem ayni form icinde kalir.
- Secim modunda (`SecimIcinAcildi=true`): kayit yapilmaz; satir secilince `Secildi=true` set edilip form kapanir, cagiran forma deger doner.
**Butonlar & kisayollar:**
- `BtnKaydet` ("Kaydet", alt bar) — `BtnKaydet_Click`: form alanlarini `_mdl` Kullanici nesnesine yazar, sifreyi `TxtSifre.Text.Sifrele(Ortak.GetKey())` ile sifreler, `_srv.InsertOrUpdate(_mdl)` cagirir. Base formda genelde Enter/F2 ile tetiklenir.
- `BtnSil` ("Sil", alt bar) — `BtnSil_Click`: "Kaydı silmek istiyormusunuz" onayi (`MesajSor`) sonrasi grid'deki secili kullaniciyi `_srv.Delete(data)` ile siler.
- `BtnDuzenle` ("Duzenle", alt bar) — `BtnDegistir_Click`: grid'deki secili kullaniciyi `_mdl`'e klonlar, form alanlarina doldurur (sifre cozulerek `SifreCoz` gosterilir).
- `BtnYeni` ("Yeni", alt bar) — `BtnYeni_Click`: `Temizle()` cagirir; yeni bos `Kullanici` olusturur, tum alanlari sifirlar.
- `BtnKapat` ("Kapat", alt bar) — base form: formu kapatir (genelde Esc).
- `BtnYazdir` ("Yazdir", alt bar) — base form yazdirma (bu formda ozel kod yok; grid yazdirma).
- `BtnIlk` / `BtnOnceki` / `BtnSonraki` / `BtnSon` — base form kayit gezinme butonlari.
- Grid satirina cift tiklama / Enter (`myView1.MyEventDoubleClickEnter` -> `MyView1_MyEventDoubleClickEnter`): secim modundaysa kaydi secip kapatir; degilse secili kaydi form alanlarina doldurur (duzenleme).
- `ChcAdmin` ("Admin" onay kutusu) — kullanicinin admin (tam yetki) olup olmadigini belirler.
**Cagirdigi katmanlar:**
- Service: `IKullaniciService _srv = Ortak.DbPro.Kullanicilar` — `KullaniciService : BaseService<Kullanici>` (ozel metodu yoktur, tum davranis base'den gelir).
  - `_srv.SelectListWhere()` — tum kullanicilari getirir (grid kaynagi, `Bagla()` icinde).
  - `_srv.InsertOrUpdate(_mdl)` — Id varsa UPDATE yoksa INSERT (Kaydet).
  - `_srv.Delete(data)` — secili kullaniciyi siler.
- Service (kullanilmiyor ama enjekte edilmis): `IGenelService _srvGenel = Ortak.DbPro.GenelServis`.
- DataAccess: `KullaniciDal : BaseDal<Kullanici>` (Dapper, ozel sorgu yok; CRUD base'den).
- Entity: `My.Entities.Kullanicilar.Kullanici` — alanlar: `Id (Guid, PK)`, `Adi`, `Soyadi`, `KullaniciAdi`, `Sifre (sifrelenmis)`, `Admin (bool)`; `Clone()` metodu (MemberwiseClone).
- Yardimci: `Ortak.GetKey()` AES anahtari; `string.Sifrele(key)` / `string.SifreCoz(key)` uzanti metotlari (My.Core) ile sifre sifreleme/cozme.
- SQL/Prosedur: yok (saf CRUD; stored procedure cagrilmaz).
- API: yok (UretimV4 DB'ye dogrudan baglanir, API kullanmaz).
**Istasyon sirasiyla iliskisi:** - (Kullanici tablosu uretim miktar/akis motoruyla, operasyon sirasiyla veya istasyon gruplamasiyla iliskili degildir; yalnizca uygulama girisi/yetkilendirme icindir.)
**Notlar:**
- Grid'de `Id` ve `Sifre` kolonlari gizlenir (`SutunGizle("Id")`, `SutunGizle("Sifre")`); sifre listede gosterilmez.
- Form alanlarina kayit yuklenirken sifre cozulur (`SifreCoz`), kaydedilirken yeniden sifrelenir (`Sifrele`); boylece duzenlemede gercek sifre gosterilir.
- Ilk kullanici otomatik olusturma KullaniciModul'de degil, `FrmLogin.Kontrol()` icinde yapilir: `Kullanici` tablosu bossa otomatik "Admin" / sifre "1" (sifrelenmis), Admin=true bir kayit eklenir.
- Giris dogrulamasi (`FrmLogin`) kullanici adini ve sifreyi `ToUpper()` yaparak karsilastirir; bu nedenle kullanici adi/sifre buyuk-kucuk harf duyarsizdir.
- Grid yerlesimi `myGrid1.MyGridKayitAdi = "KullanciKayitListesi"` adiyla saklanir/yuklenir (`GridYerlesimYukle`).
- Tum alanlar `MyMaxLength = 75` (max 75 karakter), `EnterMoveNextControl = true` (Enter ile sonraki alana gecis).
- Hata durumlarinda kullaniciya `MesajHata(rs.Message)` ile mesaj gosterilir; islem yapilmaz.
