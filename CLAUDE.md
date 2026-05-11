# CLAUDE.md - UretimV4 (CepPatronERP) Projesi

## Proje Ozeti

Mobilya/uretim isletmeleri icin uretim takip, istasyon yonetimi ve recete sistemi. Mikro ERP entegrasyonuyla calisan Windows Forms tabanli ERP modulu. Lisans korumali (`My.Kontrol`) bir masaustu uygulamasidir; uretim emirleri, istasyon hareketleri, recete yonetimi, mal kabul, kullanici/personel yonetimi ve raporlama kapsar.

Assembly adi: `CepPatronERP.exe`

## Teknoloji Yigini

- **Framework**: .NET Framework 4.6.1 (WinForms, `WinExe`)
- **UI**: DevExpress WinForms 23.2 (XtraBars, XtraGrid, XtraReports, XtraCharts, ...)
- **Veritabani**: SQL Server (Dapper 2.0.90 + ozel `My.Dapper.Core` BaseDal)
- **DI**: Unity 5.9.4 (Container/Abstractions)
- **Logging**: Serilog 2.10.0 + Serilog.Sinks.File 5.0.0
- **JSON**: Newtonsoft.Json 13.0.1
- **Lisans/Kontrol**: `By.Kontrol.23.dll` (DevExpress base form ve key kontrolu)
- **Yardimci DLL'ler**: `My.Dapper.Core.dll`, `My.FtpManager.dll`, `My.Sms.dll`, `My.Kontrol.23.dll`

## Cozum Yapisi

```
UretimV4.sln
├── My/                       # Is mantigi + Veri erisim katmani (Class Library, My.Data.dll)
│   ├── Business/             # Service'ler, Manager'lar, DI (Unity), DatabaseFactory
│   ├── DataAccess/           # Dapper DAL'lari (BaseDal turevi)
│   └── Entities/             # POCO domain modelleri
├── My.DataBaseSettings/      # Veritabani sema/baglanti aracligi (DatabaseKontrol.exe)
│   ├── DatabaseCreate/       # Tablo/index/procedure olusturma scriptleri
│   └── DatabasePanel/        # Baglanti yonetim formlari
├── MyUI/                     # Ana WinForms uygulamasi (CepPatronERP.exe)
│   ├── FrmAna.cs             # Ana ribbon formu (giris, lisans kontrolu, modul acma)
│   └── *Modul/               # Her is alani icin form klasoru
├── Dll/                      # Yerel yardimci DLL'ler (My.Dapper.Core, My.Kontrol.23, My.FtpManager, My.Sms)
├── appsettings.json          # Sablon konfigurasyon (uygulama tarafindan dogrudan kullanilmiyor)
├── *.sql                     # El ile calistirilan SQL scriptleri
└── packages/                 # NuGet paketleri (`nuget restore` sonrasi olusur)
```

## Derleme

### Visual Studio ile (onerilen)
1. `UretimV4.sln` dosyasini Visual Studio 2022 ile ac
2. Solution Explorer'da sag tikla → **Restore NuGet Packages** (packages.config tabanli, otomatik)
3. **Build → Build Solution** (Debug / Any CPU)

### Komut satiri (MSBuild)
```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" `
  D:\Projelerimmm\UretimV4\UretimV4.sln -t:Build -p:Configuration=Debug -v:minimal
```
- Debug build cikti: `MyUI\bin\Debug\CepPatronERP.exe`
- Release build cikti: `MyUI\bin\Release\CepPatronERP.exe`

### Bilinen Derleme Uyarilari
- Cok sayida **MSB3177** ("kismen guvenilen cagiranlara izin vermiyor") uyarisi normaldir, .NET Framework PartialTrust referans modellemesi kaynaklidir.
- **MSB3181** (System.ValueTuple cifte yol) uyarisi NuGet `System.ValueTuple` paketinin .NET 4.7+ ile catismasindan kaynaklanir; derlemeyi etkilemez.

### NuGet Paket Yollari (HintPath uyarisi)
csproj icindeki bazi `HintPath` girisleri tarihsel olarak `..\..\..\..\Nuget\nuget_packages\...` (paylasilan paket havuzu) gibi paylasilan klasorlere isaret eder. Bu yollar tum makinelerde bulunmayabilir. Eger ilk derlemede "metadata file not found" hatasi alirsaniz:
1. Solution klasorunde **`packages\`** klasoru olustur (eger yoksa)
2. Visual Studio'da NuGet Package Restore'u acik tut (varsayilan)
3. Veya `nuget.exe restore UretimV4.sln -PackagesDirectory packages` calistir

## Calistirma

```powershell
& "D:\Projelerimmm\UretimV4\MyUI\bin\Debug\CepPatronERP.exe"
```

Calistirmadan once kontrol:
- **DataBaseSettings.ini**: `MyUI\bin\Debug\Ayarlar\DataBaseSettings.ini` - Uretim + Mikro veritabani baglantilari (sifre URL-encoded saklanir).
- **Lisans anahtari**: `Program.cs` icinde `My.Kontrol.KeyKontrol.SetKey(...)` cagrisi gomulu (sabit). Lisans aktivasyonu ilk girisin ardindan `OrtakLis.SistemLisansKontrolu()` ile sorgulanir.
- **Login**: Ilk acilis `FrmLogin` dialogunu gosterir; basarisiz girise kadar ana form opacity=0 kalir.

## Konfigurasyon Dosyalari

| Dosya | Amac |
|-------|------|
| `MyUI\bin\Debug\Ayarlar\DataBaseSettings.ini` | Uretim + Mikro SQL Server baglanti bilgileri (sifrelenmis) |
| `MyUI\app.config` | DevExpress skin ayarlari + assembly binding redirects |
| `MyUI\bin\Debug\FavorilerSettings.json` | Kullanici favori modul kisayollari |
| `appsettings.json` (kok) | Eski sablon (uygulama dogrudan okumuyor; referans amacli) |

### DataBaseSettings.ini formati
```ini
[BaseCode]
ReplaceConnectionString=Data Source=@Server;Initial Catalog=@Database;User ID=@UserName;Password=@Password;
[Uretim]
Server=192.168.x.x
Database=UretimV3_XXX
UserName=sa
Password=<URL-encoded>
[Mikro]
Server=192.168.x.x
Database=MikroDB_V16_XXX
UserName=sa
Password=<URL-encoded>
```

## Kod Konvansiyonlari

### Adlandirma
- **Turkce tanimlayicilar**: Istasyon, Recete, UretimEmir, UretimTalep, Operasyon, Personel, Kullanici, Depo, Stok, Mikro
- **Form adlandirma kalibi**: `Frm<Modul><Ek>` (Ek: `ED`=Ekleme/Duzenleme, `EG`=Ekleme/Giris, `Listesi/Liste`=liste formu)
- **Siniflar/Metotlar**: PascalCase
- **Ozel alanlar**: alt cizgi oneki (`_TmpMikroStok`)
- **DAL siniflari**: `<Entity>Dal` (BaseDal<T> turevi, Dapper kullanir)
- **Service siniflari**: `I<Ad>Service` + `<Ad>Service`

### Mimari Katmanlar (My projesi)

1. **Entities/** - POCO domain modelleri (`IEntity` arayuzu)
2. **DataAccess/** - Dapper DAL siniflari (`BaseDal<T>` turevi, `My.Dapper.Core`'dan)
3. **Business/Service/** - Service arayuzleri + somut uygulama (DAL kullanir)
4. **Business/Manager/** - Ust duzey orkestrasyon (cok service'i koordine eder; ornegin `UretimTakipManagerV2`, `MikroKayitManager`)
5. **Business/Dependency/Unit/** - Unity container kayitlari (`UnityPro`, `UnityMicro`)
6. **Business/DatabaseFactory{Pro,Mikro}.cs** - Iki ayri DB icin baglanti ve service factory

### MyUI Sayfa/Form Yapisi
- `FrmAna` (Ribbon-based ana form) `MyFrmAna` (`By.Kontrol.23`) sinifindan turetilir
- Her is alani kendi klasorunde: `UretimModule/`, `IstasyonModul/`, `ReceteModul/`, `PersonelModul/`, ...
- `Ortak.cs` - statik global state (DbPro, DbMikro factory'leri, ayarlar, kullanici bilgisi)

## Veritabani Mimarisi

Iki ayri veritabani:

1. **DbPro** (Uretim DB) - Yerel/sirket veritabani: uretim emirleri, receteler, istasyon takipleri, kullanici/personel, mesajlar, mail/sms ayarlari
2. **DbMikro** (Mikro ERP DB) - Mikro Yazilim'in ana ERP veritabani: STOK_HAREKETLERI, STOKLAR, SIPARISLER, vb.

Audit alanlari (Pro tablolarinda): `KayitEden`, `Guncelleyen`, `KayitTarihi`, `GuncellemeTarihi`
PK: Guid

`DatabaseFactoryPro` ve `DatabaseFactoryMikro` Unity DI ile service'leri uretir; `Ortak.DbPro` ve `Ortak.DbMikro` uzerinden global erisilir.

### Sema Yonetimi
`My.DataBaseSettings` projesi (DatabaseKontrol.exe), Uretim veritabaninda tablo, index ve stored procedure'lari sirayla olusturur/gunceller. `Ortak.DatabaseGuncelleUretim()` calistirildiginda:
1. `GenelCreate.DatabaseGuncelle()` - genel tablolar (Ayar, Kullanici, Cari, Stok, Mesaj, Personel, ...)
2. `UretimCreate.DatabaseGuncelle()` - uretim tablolari (IstasyonBakim, Recete, UretimEmir, Operasyon, ...)

## Onemli Moduller (MyUI)

| Modul | Aciklama |
|-------|----------|
| `AyarVeGenelModul/` | Genel/istasyon/Mikro/SMS ayarlari, lisans, login, masaustu |
| `HizliUretimModule/` | Tek ekrandan hizli uretim girisi (FrmHizliUretimEG) |
| `IstasyonModul/` + `UretimIstasyonModule/` | Istasyon tanim ve uretim istasyon takibi |
| `IstasyonHareketlerModul/` | Istasyon hareket detay/log |
| `MalKabul/` | Mal kabul fisi (depo girisi) |
| `MikroModul/` | Mikro stok aktarim/hesaplama formlari |
| `OperasyonModul/` + `UretimOperasyonModule/` | Operasyon kart tanimi ve uretim operasyon |
| `PersonelModul/` | Personel kayit |
| `ReceteModul/` + `ReceteIstasyonGrupModul/` | Recete agaclari, istasyon gruplari |
| `SiparisModule/` | Mikro siparis listesi/detay |
| `UretimModule/` | Uretim emirleri |
| `UretimTalepler/` | Talep onay/red akisi |
| `UretimKontroller/` | Kontrol formlari |
| `KullaniciModul/` | Kullanici CRUD + yetki matrisi |
| `MailModul/` + `SmsModule/` | Bildirim ayarlari ve gonderim |
| `Raporlar/` | DevExpress XtraReports rapor tanimlari |
| `Updates/` | Versiyon kontrolu (`UpdateProgram.VersiyonKontrol()`) |

## DLL Referans Yollari

csproj dosyalarinin **HintPath**'lari su konumlara isaret eder:

- `..\Dll\*.dll` - solution kokunde `Dll/` klasoru (proje icinde mevcut)
- `..\..\..\Dlller\By.Kontrol.23.dll` - `D:\Dlller\By.Kontrol.23.dll` (paylasilan harici klasor; mevcut degilse `MyUI\bin\Debug\` icinden alinabilir, build cache uzerinden cozulur)
- `..\..\packages\*` ve `..\..\..\..\Nuget\nuget_packages\*` - NuGet paket havuzlari (packages.config restore sonrasi olusur)

Ilk temiz build oncesi `Dll/` klasorunde su dosyalarin oldugundan emin ol:
```
My.Dapper.Core.dll
My.FtpManager.dll
My.Kontrol.23.dll
My.Sms.dll
```

## Lisans Kontrolu

- `Program.cs` icinde sabit anahtar: `My.Kontrol.KeyKontrol.SetKey("ldSvAgr7jI7R+E4Yg+gnbzpt+gI4WVn+coNELpxlLjs=")`
- Login sonrasi `OrtakLis.SistemLisansKontrolu()` calisir; pasif lisans durumunda Recete/Kullanici menu butonlari disabled olur
- Lisans aktivasyon formu: `FrmLisansGiris` (Ortak.LisansKayit ile cagrilir)

## SQL Script'leri (Solution Koku)

| Dosya | Amac |
|-------|------|
| `StokAlisSatisOrtalamaFiyatListes.sql` | Stok alis/satis ortalama fiyat listesi raporu |
| `Uretim_SonrakiIstasyonaGonder 2025 01 29 Son.sql` | Bir sonraki istasyona aktarim stored procedure'u (latest revision) |

Bunlar el ile SQL Server Management Studio'da calistirilir.

## Bilinen Sorunlar / Notlar

- **Release build temiz makinede basarisiz olabilir**: Bircok HintPath paylasilan/eski klasorlere isaret ettigi icin (`..\..\..\..\Nuget\nuget_packages\...`). Debug build mevcut `bin\Debug` icindeki paketler sayesinde calismakta. Temiz Release icin oncelikle NuGet restore'u garantile veya HintPath'lari `packages\` altina yonlendir.
- **`Dlller` klasoru**: `D:\Dlller\By.Kontrol.23.dll` paylasilan disk yoluna baglidir. Yeni bir geliştirme makinesinde bu DLL'in mevcut oldugunu garanti et veya `MyUI\bin\Debug\By.Kontrol.23.dll` dosyasini bu konuma kopyala.
- **packages.config tabanli proje**: Modern PackageReference yerine eski paket yonetimi kullanir; NuGet restore'u VS dahilinde otomatiktir, komut satirinda `nuget.exe restore` gerekir (msbuild `-t:Restore` packages.config'i isleme almaz).
- **DevExpress 23.2 lisansi**: Derleme/calistirma icin gecerli bir DevExpress 23.2 yerel kurulumu veya bin'de tum DLL'lerin bulunmasi gerekir.

## Hizli Baslangic Kontrol Listesi

- [ ] Visual Studio 2022 + .NET Framework 4.6.1 hedef paketi kurulu
- [ ] DevExpress 23.2 lisansli kurulum mevcut (veya tum DLL'ler `bin\Debug` icinde)
- [ ] `Dll\` klasorunde 4 yardimci DLL mevcut
- [ ] `D:\Dlller\By.Kontrol.23.dll` mevcut (yoksa kopyala)
- [ ] `MyUI\bin\Debug\Ayarlar\DataBaseSettings.ini` dogru SQL Server bilgileriyle dolu
- [ ] Uretim ve Mikro veritabanlarina erisim acik
- [ ] Solution acildiginda NuGet paket geri yukleme tamamlanmis
- [ ] Debug | Any CPU ile build basarili → `CepPatronERP.exe` calisir
