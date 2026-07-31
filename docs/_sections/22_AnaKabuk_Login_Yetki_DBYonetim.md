## Modul: Ana Kabuk, Login, Yetki ve DB Yonetimi

UretimV4 (CepPatronERP) iki ayri calistirilabilir uygulamadan olusur: (1) ana WinForms ERP uygulamasi `MyUI` (`CepPatronERP.exe`) ve (2) yardimci DB/yetki yonetim araci `My.DataBaseSettings` (`DatabaseKontrol.exe`). Bu bolum, bu iki uygulamanin kabuk (shell), giris (login), kullanici/yetki yonetimi ve veritabani baglanti/sema yonetimi ekranlarini belgeler. Ana uygulamada `FrmAna` ribbon + MDI ana penceredir; acilirken once otomatik guncelleme kontrolu (`UpdateProgram` -> FTP `update.ceppatron.com`), ardindan `FrmLogin` ile kullanici dogrulamasi, sonra donanim tabanli sistem lisans kontrolu (`OrtakLis.SistemLisansKontrolu` -> `lisans.lic` dosyasi + islemci ID) yapar; basariliysa masaustu acilir, `Ortak.DbPro`/`Ortak.DbMikro` baglanti fabrikalari kurulur, genel/istasyon/mikro ayarlar belege baglanir ve Mikro stok temp tablolari guncellenir. Tum baglanti ayarlari INI dosyalarinda AES anahtariyla ("OzelAnahtar1234%&", `Ortak.GetKey()`) sifreli saklanir; uygulama verileri `UretimV3_FEZA` (programconnection / DbPro), Mikro ERP verileri `MikroDB_V16_FEZA24` (mikroconnection / DbMikro) veritabanindadir. Yardimci `My.DataBaseSettings` uygulamasi ise tarih-tabanli gunluk parola (`FrmLogin` "GSYA_2") ile korunur ve DB baglanti stringi tanimlama, sema olusturma/guncelleme, kullanici CRUD ve modul bazli yetki matrisi yonetimini saglar.

---

### FrmAna - Ana Kabuk (`MyUI\FrmAna.cs` + `FrmAna.Designer.cs`)
**Ne ise yarar:** Ana ERP uygulamasinin ribbon menulu MDI ana penceresidir. Tum modul ekranlarini (siparis, recete, uretim, istasyon, mikro, raporlar, ayarlar, kullanici/personel vb.) ribbon bar butonlari uzerinden acan kabuk formdur. Alt durum cubugunda (status bar) kullanici adi, lisans durumu, bagli firma DB adi ve versiyon bilgisini gosterir. `MyFrmAna` (My.Kontrol kutuphanesindeki base ribbon form) turevidir.
**Once ne olmali (onkosul):** Uygulama exe'si calistirilir; baglanti ayar INI dosyalari (`DbPro`/`DbMikro`) onceden tanimlanmis olmali (yoksa `My.DataBaseSettings` araciyla girilir). Form yuklenirken sirayla: guncelleme kontrolu -> login -> lisans kontrolu yapilir.
**Sonra ne olur:**
- `FrmAna_Load` once `UpdateKontrol()` cagirir; guncelleme indirildiyse `return` ile uygulamadan cikar (yeni surum kurulacaktir).
- Ardindan `FrmLogin` (modal) acilir. Giris basarisizsa `this.Close()` ile uygulama kapanir.
- Giris basariliysa: skin yuklenir (`SkinAdd`), masaustu MDI child acilir (`MasaUstuAc` -> `FrmMasaustu1`), opacity fade-in timer baslar, alt barda kullanici adi yazilir.
- `OrtakLis.SistemLisansKontrolu()` ile lisans dogrulanir: aktifse `Ortak.LisansAktif=true`, alt bar "Lisans = Aktif", Recete Listesi / Recete Grup Takimlar / Kullanicilar butonlari `Enabled=true`; pasifse bu butonlar devre disi birakilir ("Lisans = Pasif").
- `Ortak.DbPro` ve `Ortak.DbMikro` baglanti fabrikalari yeniden olusturulur, pencere maximize edilir, alt barda firma DB adi gosterilir.
- `Ortak.MikroEntAyarlarBagla()`, `GenelAyarlarBagla()`, `IstasyonAyarlarBagla()` ile ayar tablolari belege yuklenir; versiyon yazilir.
- `TempGuncelle()` cagirilir: Mikro stok kategori + stok temp tablolari (`MikroStokKategoriGuncelle`, `MikroStokGuncelle`) Mikro DB'den senkronlanir.
- Form kapatilirken (`FrmAnaV2_FormClosing`) secili skin/palet INI'ye yazilir ve "Programı Kapatmak İstiyormusunuz" onayi sorulur.
**Butonlar & kisayollar:** (Ribbon bar butonlari; her biri ilgili ekrani acar)
- Modul ekranlarini acan generic buton handler `BarBtnlar_Click` — buton `Hint` degerini tag olarak alip `FormAc.FormSec(tag)` cagirir (Siparisler, ReceteUretimler, MikroStokListesi, UretimEmirleri, OperasyonTakip, IstasyonTakip, IstasyonRaporu vb.).
- `BarBtnKullanicilar` — `FrmKullaniciKayit` (kullanici CRUD) acar (lisans aktifse).
- `BarBtnPersonelListesi` — `FrmPersonelKartlari` acar.
- `BtnLisansKayit` — `Ortak.LisansKayit()` -> `FrmLisansGiris` (lisans anahtari girisi).
- `BarBtnMailAyarlari` — `FrmMailSettings`; `BarBtnSmsAyarlari` — `FrmSmsAyarlari`; `BarBtnSmsRapor` — `FrmSmsRapor`.
- `BarBtnMikroEntAyarlari` — `FrmMikroEntAyarlari`; `BarBtnGenelAyarlar` — `FrmGenelAyarlar`; `BarBtnIstasyonUretimAyarlari` — `FrmIstasyonUretimAyarlari`.
- `BarBtnIstasyonBaslatmaHatalari` / `BarBtnIstasyonDurdurmaKodlari` / `BarBtnIstasyonFireSebepleri` — `FrmIstasyonAciklamalari` (modul turune gore).
- `BarBtnReceteAciklama` — `FrmAciklamaKodlar` (ReceteAciklama).
- `BarBtnReceteIstasyonGruplar` / `BarBtnReceteIstasyonGrupOperasyonlar` — recete-istasyon grup tanim ekranlari.
- `BarBtnDbGuncelle` — `Ortak.DatabaseGuncelleUretim()` (sema guncelleme).
- `BarBtnIstasyonHareketLog`, `BarBtnIstasyonBakimList`, `BarBtnMalKabulListe`, `BarBtnOlcuKontrol`, `BarBtnHizliUretim`, `BarBtnStokTuketimRaporu(Detayli)`, `BarBtnReceteKullanilanStok`, `BarBtnReceteGenelRaporu` — ilgili liste/rapor ekranlari (MDI child olarak `.Show()`).
- `btnStokGuncelle` — `FrmStokKodGuncelle` acar.
- `BarBtnAcilMesaj` — `FrmMesajGenel` (acil mesaj).
- Tum Sekmeleri Kapat (`BarUstTumSekmeleriKapat` / sag tik menu) — Masaustu disindaki tum MDI child'lari onayla kapatir.
- Hizli Baslangica Ekle (ribbon sag tik) — aktif ribbon butonunu masaustune kisayol olarak ekler (`frmmasaUstu.ButonEkle`).
- Ribbon QAT'a ekleme menusu gizlenir, yerine "Hızlı Başlangıca Ekle" ve "Tüm Sekmeleri Kapat" eklenir (`RibbonControl1_ShowCustomizationMenu`).
**Cagirdigi katmanlar:**
- Manager/Service: `OrtakLis.SistemLisansKontrolu()` — `lisans.lic` dosyasini islemci ID hash'i (`GetBaseCpuInfo`/MD5 tabanli `Kisa()`) ile karsilastirir; donanim kilitli lisans dogrulamasi.
- Manager/Service: `UpdateProgram.VersiyonKontrol()` — FTP (`update.ceppatron.com/update_uretimv1/`) uzerinden uzak `versiyon.txt` ile yerel surumu kiyaslar; yeni surum varsa `download.zip` indirir (`FtpManager`).
- Manager/Service: `Ortak.DbPro` (`DatabaseFactoryPro`) / `Ortak.DbMikro` (`DatabaseFactoryMikro`) — Unity DI container'la tum servisleri (Ayarlar, Siparis, Recete, Uretim, Istasyon, Kullanicilar, TempMikroStok vb.) cozer.
- Manager/Service: `Ortak.MikroEntAyarlarBagla/GenelAyarlarBagla/IstasyonAyarlarBagla` — `IAyarService.SelectListWhere(" where Modul='...' ")` ile ayar tablolarini yukler; `PlKapat`, `MalKabulKullan` gibi global bayraklari set eder.
- Manager/Service: `ITempMikroStokService.MikroStokKategoriGuncelle / MikroStokGuncelle` — Mikro DB'den stok/kategori temp senkron (MERGE).
- Manager/Service: `FormAc.FormSec(tag)` — string tag'e gore ilgili modul formunu olusturup MDI child olarak acan dispatcher (switch-case).
- SQL/Prosedur: dogrudan prosedur cagrilmaz; temp guncelleme SQL'i Mikro fonksiyonlari `fn_StokCins` kullanir, STOKLAR/STOK_KATEGORILERI/STOK_REYONLARI/STOK_KALITE_KONTROL_TANIMLARI tablolarini NOLOCK ile okur.
- API: yok (FTP guncelleme haricinde dogrudan DB).
**Istasyon sirasiyla iliskisi:** - (Kabuk/menu formu; uretim akisi veya istasyon sirasiyla dogrudan iliskisi yoktur, sadece ilgili ekranlari acar.)
**Notlar:**
- Lisans pasifken yalnizca Recete Listesi/Grup ve Kullanicilar butonlari kapatilir; diger moduller acik kalir.
- Skin/palet secimleri INI `[Theme]` bolumunde `ApplicationSkinName`/`ApplicationSkinPaletteName` olarak saklanir.
- `AktifButon` alani, ribbon highlight degisiminde guncellenir ve "Hizli Baslangica Ekle" islevinde kullanilir.
- Versiyon `versiyon.txt` dosyasindan okunur; FTP guncelleme bu dosyayi karsilastirir.

---

### MyFrmLoginPaneli - Login Panel Taban (`MyUI\AyarVeGenelModul\MyFrmLoginPaneli.cs`)
**Ne ise yarar:** Ana uygulamanin login ekrani (`MyUI.FrmLogin`) icin gorsel/davranis taban sinifidir (base form). Kenarliksiz pencerenin baslik/etiket alanlarindan fareyle suruklenip tasinmasini, ESC ile kapatma / F2 ile giris kisayollarini ve ortak mesaj kutusu yardimcilarini (`MesajHata`, `MesajBilgi`, `MesajSor`, `GetKontrol`) saglar. Kendi basina veri islemi yapmaz; gorsel cerceve ve etkilesim altyapisidir.
**Once ne olmali (onkosul):** Dogrudan acilmaz; `MyUI.FrmLogin` bu siniftan tureyerek kullanir. `FormOlusturuldu` bayragi form `Shown` olunca true olur.
**Sonra ne olur:** Kullanici basligi suruklerse pencere konumu degisir; BtnKapat veya ESC ile form kapanir; F2 ile turetilen formdaki `BtnGiris` tetiklenir.
**Butonlar & kisayollar:**
- `BtnKapat` / `Esc` — formu kapatir (`BtnKapat_Click` -> `this.Close()`).
- `F2` — `BtnGiris.PerformClick()` (turetilen FrmLogin'deki giris butonunu tetikler).
- Baslik/etiket/panel alanlari fare suruklemesi — pencere tasima (`Lbl_Baslik_MouseDown/Move/Up`).
**Cagirdigi katmanlar:**
- Manager/Service: yok (saf UI taban sinifi).
- SQL/Prosedur: yok.
- API: yok.
**Istasyon sirasiyla iliskisi:** -
**Notlar:**
- `MesajHata/MesajBilgi/MesajSor` static yardimcilari ve `GetKontrol(Action)` try/catch sarmalayicisi tum login akisinda kullanilir.
- Asagidaki `MyUI.FrmLogin` bu sinifin turevidir (gercek dogrulama mantigi orada).

---

### FrmLogin (MyUI) - Uygulama Girisi (`MyUI\AyarVeGenelModul\FrmLogin.cs` + `FrmLogin.Designer.cs`)
**Ne ise yarar:** Ana ERP uygulamasinin gercek kullanici giris ekranidir (`MyFrmLoginPaneli` turevi). Kullanici adi/sifre dogrulamasi yapar ve basariliysa global `Ortak.DbPro`/`Ortak.DbMikro` baglanti fabrikalarini kurar. `FrmAna_Load` icinde modal acilir.
**Once ne olmali (onkosul):** Baglanti ayar INI dosyalari tanimli olmali (form load'da `DatabaseFactoryPro/Mikro` kurulur; ayar yoksa hata `GetKontrol` ile yakalanir). Son kullanilan kullanici adi INI `[AYAR] KULLANICI`'dan okunup doldurulur.
**Sonra ne olur:**
- Giris basariliysa `GirisYapildi=true`, kullanici adi INI'ye yazilir, `Ortak.KullaniciAdi` set edilir, form kapanir; kontrol `FrmAna_Load`'a doner (lisans kontrolu + masaustu acma).
- Basarisizsa "Kullanıcı Adı Veya Şifre Geçersiz" uyarisi gosterilir, form acik kalir.
- Ozel durum: `Kullanici` tablosu bossa otomatik `Admin` / sifre "1" (AES sifreli), `Admin=true` ilk kullanici INSERT edilir (`Kontrol()` icinde).
**Butonlar & kisayollar:**
- `BtnGiris` / `F2` — `BtnGiris_Click` -> `Kontrol()` ile dogrulama.
- `BtnKapat` / `LblClose` / `Esc` — formu kapatir (giris yapilmadan kapanirsa uygulama sonlanir).
- `TxtKullanici` Enter — fokusu sifre alanina tasir; `TxtSifre` Enter — `Kontrol()` calistirir.
- `BtnDbPro` / `BtnDbMikro` — DB ayar butonlari (tasarimda gizli, `Visible=false`; icerik comment-out edilmis, kullanilmiyor).
**Cagirdigi katmanlar:**
- Manager/Service: `Ortak.DbPro.Kullanicilar` (`IKullaniciService`) — `Query<int>("select count(*) ...")` (kullanici sayisi), `Insert(...)` (ilk admin), `SelectFirst(k => k.KullaniciAdi == ka)` (dogrulama).
- Manager/Service: `DatabaseFactoryPro` / `DatabaseFactoryMikro` (form load'da kurulur).
- Yardimci: `Ortak.GetKey()` AES anahtari; `string.Sifrele(key)` ile parola sifreleme (DB'deki sifre ile karsilastirma).
- SQL/Prosedur: dogrudan inline `select count(*) as Adet from Kullanici` + servis tabanli CRUD; stored procedure yok.
- API: yok.
**Istasyon sirasiyla iliskisi:** -
**Notlar:**
- Kullanici adi ve sifre `ToUpper()` ile karsilastirilir -> giris buyuk/kucuk harf duyarsiz.
- Sifreler DB'de duz metin degil AES sifreli ("OzelAnahtar1234%&") saklanir.
- Bu form modul bazli yetki (KullaniciIzinler) kontrolu YAPMAZ; sadece kullanici/sifre + `Admin` bayragi anlamlidir. Modul bazli yetki matrisi yardimci `My.DataBaseSettings` araciyla tanimlanir (asagiya bkz).
- Tasarimda `BtnDbPro`/`BtnDbMikro` butonlarinin DB ayar acma kodu yorum satirina alinmis (login ekranindan DB ayari degistirilemez).

---

### FrmLogin (My.DataBaseSettings) - Yardimci Arac Girisi (`My.DataBaseSettings\FrmLogin.cs` + Designer)
**Ne ise yarar:** Yardimci `DatabaseKontrol.exe` (DB/yetki yonetim araci) ve DB paneli (`FrmDataPanel`) acilirken gosterilen koruma ekranidir. Veritabani sema/baglanti islemleri gibi hassas islemlere yetkisiz erisimi engellemek icin tarih tabanli (gunluk degisen) bir parola sorar.
**Once ne olmali (onkosul):** `FrmDataPanel.Frm_Load` (veya araci dogrudan calistirma) bu formu modal acar. Kullanicinin gunun gecerli parolasini bilmesi gerekir.
**Sonra ne olur:**
- Dogru parola girilirse `GirisYapildi=true`, form kapanir; cagiran `FrmDataPanel` `Ortak.GetSettings()` ile DB ayarlarini yukler (panel acilir).
- Yanlissa "Şifre Geçersiz" uyarisi; tekrar denenebilir.
- Cagiran panelde giris yapilmazsa `Application.Exit()` ile arac kapanir.
**Butonlar & kisayollar:**
- `button1` ("Giris") — `Kontrol()` calistirir; `textBox1` Enter ile de tetiklenir.
- `button2` ("Kapat") — formu kapatir (giris yapilmamis sayilir).
**Cagirdigi katmanlar:**
- Manager/Service: yok (parola tamamen istemci tarafinda, koddaki algoritma ile uretilir).
- SQL/Prosedur: yok.
- API: yok.
**Istasyon sirasiyla iliskisi:** -
**Notlar:**
- Parola algoritmasi (`// GSYA_2`): `gun(2hane) + saat(2hane) + (yil-2000)(2hane) + ay(2hane)` formatinda gunluk/saatlik degisen bir kod. Yani parola servis/personel tarafindan bilinen ve gun-saate gore degisen statik bir formul; veritabaninda saklanmaz.
- Bu form ana uygulamadaki `FrmLogin`'den tamamen ayridir (farkli namespace `My.DatabaseSettings`, farkli mantik); karistirmamak gerekir.

---

### FrmKullaniciAyar - Kullanici Ayarlari/CRUD (`My.DataBaseSettings\FrmKullaniciAyar.cs` + Designer)
**Ne ise yarar:** Yardimci aracta uygulamaya giris yapacak kullanicilari (Kullanici tablosu) listeleyen ve ekleme/duzenleme/silme yapan CRUD ekranidir. Her kullanici icin modul bazli yetkileri tanimlamak uzere `FrmKullaniciYetkiler` ekranina kapi acar. (Ana uygulamadaki `FrmKullaniciKayit` ile ayni tabloyu yonetir; bu, yonetici araci muadilidir.)
**Once ne olmali (onkosul):** `FrmDataPanel` -> "Kullanıcı Ayar Ve Yetkiler" butonundan acilir; `Ortak.GetSettings()` ile `Ortak.Connection` (programconnection) kurulmus olmali.
**Sonra ne olur:**
- Kaydet -> `Kullanici` tablosuna IF EXISTS bazli UPDATE/INSERT; grid yenilenir (`Bagla`), alanlar temizlenir.
- Sil -> once `KullaniciIzinler` (yetki kayitlari), sonra `Kullanici` satiri DELETE edilir (cascade el ile); grid yenilenir.
- Yetkiler -> secili kullanici icin `FrmKullaniciYetkiler` modal acilir (modul bazli yetki matrisi).
**Butonlar & kisayollar:**
- `BtnKaydet` — `BtnKaydet_Click`: `RowaAktar()` ile form alanlarini `_mdl`'e yazar, sifreyi `Sifrele(Ortak.ANA_KEY)` ile sifreler, UPDATE/INSERT calistirir.
- `BtnYeni` — `BtnYeni_Click`: yeni bos `Kullanici` olusturur, alanlara aktarir.
- `BtnDegistir` ("Duzenle") — `BtnDegistir_Click`/`Duzenle()`: grid'deki secili kaydi `_mdl`'e klonlar, sifreyi `SifreCoz` ile cozup gosterir.
- `BtnSil` — `BtnSil_Click`: onayla `KullaniciIzinler` + `Kullanici` siler.
- `BtnYetkiler` — `BtnYetkiler_Click`: secili kullanici icin `FrmKullaniciYetkiler` acar (Kullanici + KulIdSi gecirilir).
- `BtnKapat` — formu kapatir.
- Grid cift tik (`dataGridView1_DoubleClick`) — secili kaydi duzenlemeye alir (`Duzenle`).
**Cagirdigi katmanlar:**
- Manager/Service: `Ortak.Connection` (Dapper `IDbConnection`) uzerinden dogrudan inline SQL — `Query<Kullanici>("Select * from Kullanici")`, IF EXISTS UPDATE/INSERT, DELETE.
- Yardimci: `Sifrele(Ortak.ANA_KEY)` / `SifreCoz(Ortak.ANA_KEY)` — parola sifreleme/cozme (ANA_KEY = "OzelAnahtar1234%&").
- Entity: `My.DatabaseSettings.Entites.Kullanici` (Id Guid PK, Adi, Soyadi, KullaniciAdi, Sifre, Admin) + `Clone()`.
- SQL/Prosedur: dogrudan inline SQL (`Kullanici`, `KullaniciIzinler` tablolari); stored procedure yok.
- API: yok.
**Istasyon sirasiyla iliskisi:** -
**Notlar:**
- Grid'de `Id` ve `Sifre` kolonlari gizlenir; tum kolonlar read-only (duzenleme form alanlarindan yapilir).
- Duzenlemede sifre cozulup gosterilir, kaydederken yeniden sifrelenir.
- `ChcAdmin` admin (tam yetki) bayragini set eder; admin ise modul bazli yetki kontrolu pratikte atlanir.
- Silme isleminde `KullaniciIzinler` once silinir (yabanci anahtar/yetim kayit temizligi).

---

### FrmKullaniciYetkiler - Modul Bazli Yetki Matrisi (`My.DataBaseSettings\FrmKullaniciYetkiler.cs` + Designer)
**Ne ise yarar:** Secili bir kullanici icin modul/yetki bazli izinleri (acik/kapali) duzenleyen yetki matrisi ekranidir. `KullaniciYetkiler` (tum yetki tanimlari) ile `KullaniciIzinler` (kullaniciya verilmis izinler) tablolarini birlestirerek bir grid'de gosterir; kullanici "Durum" onay kutularini isaretleyerek izin verir/alir.
**Once ne olmali (onkosul):** `FrmKullaniciAyar` -> "Yetkiler" butonundan, secili `Kullanici` ve `KulIdSi` (Guid) set edilerek acilir. Kullanici secilmemisse "Kullanıcı Seçilmemiş" uyarisi verip form kapanir.
**Sonra ne olur:**
- Kaydet -> grid'deki her satir icin `KullaniciIzinler` tablosuna IF EXISTS UPDATE/INSERT (Durum, KulId, YetId) yapilir; grid yenilenir.
- Sil -> secili kullanicinin ilgili izin kayitlari `KullaniciIzinler`'den DELETE edilir.
- Bu yetkiler, ilgili moduller calistirildiginda erisim kontrolu icin okunur (modul bazli izin matrisi).
**Butonlar & kisayollar:**
- `BtnKaydet` — `BtnKaydet_Click` -> `Kaydet()`: `bs.EndEdit()` sonrasi listedeki tum satirlar icin IF EXISTS UPDATE/INSERT (Durum).
- `BtnSil` — `BtnSil_Click` -> `Sil()`: onayla izin kayitlarini siler.
- `BtnKapat` — formu kapatir.
- Grid "Durum" kolonu — tek duzenlenebilir kolon (onay kutusu); diger kolonlar (Modul, Yetki, Aciklama) read-only.
**Cagirdigi katmanlar:**
- Manager/Service: `Ortak.Connection` (Dapper) uzerinden inline SQL — `KullaniciYetkiler YT LEFT OUTER JOIN KullaniciIzinler IZ ON IZ.YetId=YT.Id AND IZ.KulId=@KulId` ile birlesik liste; IF EXISTS UPDATE/INSERT; DELETE.
- Entity: `KullaniciYetkilerModel` (Durum bool, Modul, Yetki, Aciklama, YetId Guid, KulId Guid).
- SQL/Prosedur: dogrudan inline SQL (`KullaniciYetkiler`, `KullaniciIzinler` tablolari); stored procedure yok.
- API: yok.
**Istasyon sirasiyla iliskisi:** -
**Notlar:**
- `Durum` COALESCE(IZ.Durum, 0) ile gelir: izin kaydi yoksa varsayilan 0 (kapali).
- `KulId`/`YetId` kolonlari grid'de gizlenir; sadece Durum duzenlenebilir.
- Bu matris (KullaniciIzinler), ana uygulamadaki `FrmLogin` tarafindan giris aninda OKUNMAZ; yetki kontrolu modulleri/ilgili formlari calistirirken yapilir (bu form sadece tanimlama yeridir).
- Yetki tanimlari (`KullaniciYetkiler`) once seed/tanimli olmali (aksi halde matris bos gelir).

---

### FrmDataPanel (My.DataBaseSettings\FrmAna.cs) - DB Ayar Kabugu (`My.DataBaseSettings\FrmAna.cs` + `FrmAna.Designer.cs`)
**Ne ise yarar:** Yardimci `DatabaseKontrol.exe` aracinin ana penceresidir (sinif adi `FrmDataPanel`, dosya `FrmAna.cs`). Veritabani baglanti ayarlari, sema olusturma/guncelleme/kontrol ve kullanici/yetki yonetimi islemlerini gruplandiran butonlu bir kabuk formdur.
**Once ne olmali (onkosul):** Arac calistirilir; `Frm_Load` once `FrmLogin` (tarih tabanli parola) acar. Giris yapilmazsa `Application.Exit()`; yapilirsa `Ortak.GetSettings()` ile programconnection baglantisi kurulur.
**Sonra ne olur:** Kullanici ilgili butona basarak: DB baglanti stringi tanimlar, sema olusturur/guncelleyebilir/kontrol eder veya kullanici/yetki ayarlarini acar. Islemler sonucunda `MessageBox` ile bilgilendirme verilir.
**Butonlar & kisayollar:**
- `BtnBaglantiPro` ("Program Bağlantı Ayar") — `FrmDataPaneli(DbProAdi)` acar (programconnection ayar formu).
- `BtnBaglantiMikro` ("Mikro Bağlantı Ayar") — `FrmDataPaneli(DbMikroAdi)` acar (mikroconnection ayar formu).
- `BtnDatabaseKontrol` ("Database Kontrol") — `Ortak.DatabaseControl()`: DB var mi kontrol eder; yoksa onayla Genel + Uretim + DepoKabul sema tablolarini olusturur.
- `BtnDatabaseOlusturUretim` ("Üretim Tablolarını Oluştur") — `Ortak.DatabaseGuncelleUretim()`: Genel + Uretim sema guncelleme.
- `BtnDatabaseOlusturDepoKabul` ("Depo Kabul Tablolarını Oluştur") — `Ortak.DatabaseGuncelleDepoKabul()`: Genel + DepoKabul sema guncelleme.
- `BtnKullaniciAyar` ("Kullanıcı Ayar Ve Yetkiler") — `FrmKullaniciAyar` acar.
- `BtnKapat` ("Kapat") — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `Ortak.GetSettings()` — programconnection ayarini AES anahtariyla cozer, `IDbConnection` kurar.
- Manager/Service: `Ortak.DatabaseControl/DatabaseGuncelleUretim/DatabaseGuncelleDepoKabul` — `DatabaseCreate.GenelCreate`, `UretimCreate`, `DepoKabulCreate` siniflarinin `DatabaseKontrol/DatabaseOlustur/DatabaseGuncelle` metotlarini cagirir (sema DDL).
- SQL/Prosedur: sema olusturma/guncelleme SQL'leri `DatabaseCreate` modulundedir (CREATE/ALTER TABLE vb.); stored procedure cagrisi degil DDL.
- API: yok.
**Istasyon sirasiyla iliskisi:** -
**Notlar:**
- Bu form sema/baglanti yonetimini ana uygulamadan ayrik tutar; uretim ortaminda hassas bir aractir (bu yuzden tarih tabanli parola ile korunur).
- `DatabaseControl` DB yoksa olusturur; `DatabaseGuncelle*` ise mevcut DB'ye eksik tablo/kolon ekler.
- Ana uygulamadaki `BarBtnDbGuncelle` da `Ortak.DatabaseGuncelleUretim()` cagirir (ayni sema guncelleme islemine ikinci giris noktasi).

---

### FrmDataPaneli + MyFrmDataPaneli - DB Baglanti Ayar Paneli (`My.DataBaseSettings\DatabasePanel\FrmDataPaneli.cs` ve `MyFrmDataPaneli.cs`)
**Ne ise yarar:** Tek bir veritabani baglantisinin (programconnection veya mikroconnection) ayarlarini (Database adi, Server/IP, Port, kullanici adi, sifre) gosterip duzenleyen ve sifreli olarak INI'ye kaydeden ayar panelidir. `MyFrmDataPaneli` gorsel/davranis tabani (pencere tasima, kisayollar, ortak mesaj kutulari), `FrmDataPaneli` ise asil okuma/yazma mantigidir.
**Once ne olmali (onkosul):** `FrmDataPanel`'den "Program Bağlantı Ayar" veya "Mikro Bağlantı Ayar" butonuyla, hedef DB adi (`DbProAdi`/`DbMikroAdi`) constructor'a gecirilerek acilir. Yetkili kullanicinin (tarih parolasiyla giris yapmis) erismesi gerekir.
**Sonra ne olur:**
- Form yuklenince ayar klasorleri olusturulur (`KlasorleriOlustur`), mevcut baglanti ayarlari INI'den okunup (`DbConnectionSettings.GetSetting`, AES "OzelAnahtar1234%&") alanlara doldurulur (`IniOku`).
- Kaydet -> alanlardan `DatabaseModel` olusturulur ve `DbConnectionSettings.SaveSetting` ile sifreli INI'ye yazilir; `AyarDegisti=true` set edilir (cagiran formun baglantiyi yeniden kurmasi icin sinyal).
**Butonlar & kisayollar:**
- `BtnKaydet` / `F2` — `BtnKaydet_Click` -> `SaveSettings()` (sifreli kaydet) + `AyarDegisti=true`.
- `BtnKapat` / `Esc` — formu kapatir.
- `btnDatabase` ("Database") — `BtnDatabase_Click`: alanlara ornek/varsayilan deger doldurur (Veritabani, ServerAdi, port 0, sa, bos sifre).
- Baslik/panel fare suruklemesi — pencere tasima.
**Cagirdigi katmanlar:**
- Manager/Service: `DbConnectionSettings.GetSetting(DbAdi, AnaKey)` / `SaveSetting(mdl, AnaKey)` — baglanti ayarini AES anahtariyla INI'den okur/INI'ye sifreli yazar.
- Entity: `DatabaseModel` (Database, Server, UserName, Password, ConnectionString) — `DatabaseModel(DbAdi)`.
- Yardimci: `IniDosyasi` (ayar + database INI dosyalari), `ProgramYolAyarlari` (yol sabitleri).
- SQL/Prosedur: yok (sadece ayar/INI islemleri).
- API: yok.
**Istasyon sirasiyla iliskisi:** -
**Notlar:**
- AES anahtari bu sinifta ayrica sabit tanimli: `const string AnaKey = "OzelAnahtar1234%&"` (genel `Ortak.GetKey()` ile ayni deger).
- `AnaKey`/`AyarIni`/`DatabaseIni` static alanlardir; iki kez instance acilsa bile tek ayar dosyasi kullanilir.
- `MyFrmDataPaneli` parametresiz ve `string dbAdi` parametreli iki constructor sunar; parametreli olan event bagli, asil kullanim budur.
- Sifre alani panelde duz gorunur (UI'da maskeli olabilir, koddan duz okunur); INI'ye sifreli yazilir.
