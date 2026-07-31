## Modul: AyarVeGenelModul

Bu modul, UretimV4 (CepPatronERP / WinForms) masaustu uygulamasinin "cekirdek/altyapi" katmanidir. Iceriginde uygulamaya giris (login), donanim bazli lisans kontrolu, kullanicinin kendi olusturdugu favori dugmelerden olusan masaustu paneli ve uygulamanin tum ayar ekranlari yer alir. Ayar ekranlarinin hemen hepsi tek bir `Ayar` tablosu (Modul + Grup + Kodu + Deger + Aciklama) uzerinde calisir; her ekran sadece `Modul` filtresiyle ayrisir (GenelAyarlar -> `Modul='Genel'`, MikroEntAyarlari -> `Modul='MikroEntegre'`, IstasyonUretimAyarlari -> `Modul='IstasyonUretim'`). Mikro ERP entegrasyonu icin hangi uretim/stok/fire/sarf hareketinin Mikro'da hangi fis turune yazilacagi da bu moduldeki `FrmMikroEntAyarlari` ekraninda belirlenir. Ayrica Mikro'da stok kodu degisince UretimV3 DB'sindeki tum ilgili tablolari toplu guncelleyen `FrmStokKodGuncelle` ile, acil duyuru mesajini tutan `FrmMesajGenel` de burada bulunur.

Modulun cogu ayar formu `My.Kontrol.Formlar.MyFrmKayit` (harici DLL) tabanlidir; ortak alt buton seridi base formdan gelir: `BtnKaydet`, `BtnSil`, `BtnYeni`, `BtnDuzenle`, `BtnYazdir`, `BtnKapat` ve navigasyon dugmeleri `BtnIlk / BtnOnceki / BtnSonraki / BtnSon`. Base form ayrica `MesajHata / MesajBilgi / MesajSor`, `IdGuid`, `YeniKayit`, `KayitEdildi`, `AcilisBittimi`, `SecimIcinAcildi / Secildi / SecilenKod / SecilenId / SecilenRow` (secim modu) ve grid bilesenleri `myGrid1 / myView1 / bs` (BindingSource) uyelerini saglar. Login ekrani ise `MyFrmLoginPaneli` tabanlidir ve kendine ozel kisayollar tasir.

---

### Giris / Login (`FrmLogin.cs`, `MyFrmLoginPaneli.cs`)
**Ne ise yarar:** Uygulamaya kullanici adi + sifre ile giris yapilan ekran. Hic kullanici yoksa otomatik olarak "Admin / 1" admin kullanicisini olusturur. Basarili girise kadar ana ekran (FrmAna) acilmaz.
**Once ne olmali (onkosul):** `FrmAna_Load` icinde once `UpdateProgram.VersiyonKontrol()` (guncelleme kontrolu) gecmeli; sonra `Ortak.GetKey()` ile AES anahtari alinip `Ortak.DbPro` (DatabaseFactoryPro) ve `Ortak.DbMikro` (DatabaseFactoryMikro) baglantilari kurulur. Kullanici adi `Ortak.AyarIni`'den ("AYAR/KULLANICI") on-doldurulur.
**Sonra ne olur:** Giris dogrulanirsa `GirisYapildi=true`, `Ortak.KullaniciAdi` set edilir, kullanici adi tekrar ini dosyasina yazilir ve form kapanir. FrmAna devaminda skin yukler, masaustunu acar, `OrtakLis.SistemLisansKontrolu()` ile lisansi kontrol eder (lisans pasifse Recete/ReceteGrup/Kullanicilar dugmeleri devre disi), ardindan `MikroEntAyarlarBagla / GenelAyarlarBagla / IstasyonAyarlarBagla` ve `TempGuncelle` (Mikro stok temp tablolari) calisir.
**Butonlar & kisayollar:**
- `Giris (F2)` / `BtnGiris` — `Kontrol()` dogrulamasini calistirir; dogruysa giris yapar (Enter sifre kutusunda da ayni isi yapar).
- `Kapat (Esc)` / `BtnKapat` — formu kapatir (giris yapilmamis sayilir, FrmAna kendini kapatir).
- `Enter` (Kullanici kutusunda) — odagi Sifre kutusuna tasir; `Enter` (Sifre kutusunda) — girisi dener.
- `Pro` / `BtnDbPro`, `Mik` / `BtnDbMikro` — DB ayar panelini acmak icin tasarlanmis; kod yorum satirinda, ayrica `Visible=false` (kullanilmiyor).
**Cagirdigi katmanlar:**
- Service: `Ortak.DbPro.Kullanicilar.Query<int>("select count(*) ... Kullanici")` — kullanici sayisini sayar; 0 ise admin ekler (`...Kullanicilar.Insert(new Kullanici{...})`).
- Service: `Ortak.DbPro.Kullanicilar.SelectFirst(k => k.KullaniciAdi == ka)` — kullaniciyi getirir; sifre `ps.Sifrele(_aesMasterKey)` ile karsilastirilir.
- Helper: `Ortak.GetKey()` — AES master anahtari; `Ortak.AyarIni.Oku/Yaz` — ini ayar dosyasi.
- SQL/Prosedur: - ; API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Sifre karsilastirmasi buyuk harfe cevrilerek (`ToUpper()`) yapilir, yani kullanici adi/sifre buyuk-kucuk harf duyarsizdir. `KeyPreview=true` oldugu icin Esc/F2 kisayollari her kontrolde calisir.

---

### Lisans Kaydi (`FrmLisansGiris.cs`, `OrtakLis.cs`)
**Ne ise yarar:** Donanima (islemci ID'si) bagli lisans anahtarini olusturup `lisans.lic` dosyasina yazan ekran. Key bilgisayara ozeldir; uretici tarafindan uretilen "Karsilik" degeri girilince lisans aktiflesir.
**Once ne olmali (onkosul):** `Ortak.LisansKayit()` cagrildiginda (lisans pasifken) acilir. Acilista `OrtakLis.SistemLisansKontrolKey()` ile makinenin islemci seri numarasindan uretilen Key gosterilir; varsa mevcut `lisans.lic` icerigi okunur.
**Sonra ne olur:** Girilen karsilik, `OrtakLis.SistemLisansKeyKontrolu(Key)` ciktisi ile esitse `lisans.lic` dosyasi guncellenir ve `Application.Restart()` ile uygulama yeniden baslatilir. Esit degilse "Lisans Kodu Gecersiz" mesaji.
**Butonlar & kisayollar:**
- `Kaydet` / `button2` — `button2_Click`: girilen karsiligi dogrular, uyarsa dosyaya yazip uygulamayi restart eder.
**Cagirdigi katmanlar:**
- Helper: `OrtakLis.SistemLisansKontrolKey()` — `GetBaseCpuInfo()` (WMI `win32_processor.processorID`) -> `Kisa()` (MD5 + ozel sayisal donusum) ile makine anahtari uretir.
- Helper: `OrtakLis.SistemLisansKeyKontrolu(data)` = `Kisa(data)` — beklenen karsiligi hesaplar.
- Helper: `OrtakLis.SistemLisansKontrolu()` — `lisans.lic` ile makine anahtarini karsilastirip bool doner (FrmAna acilista bunu kullanir).
- SQL/Prosedur: - ; API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Lisans makine basinadir (islemci ID). Lisans pasifken FrmAna'da Recete Listesi, Recete Grup/Takimlar ve Kullanicilar dugmeleri kapali kalir.

---

### Masaustu (Favori Dugmeler Paneli) (`FrmMasaustu.cs`, `MasaUstuButonlar.cs`)
**Ne ise yarar:** Ana ekran (MDI) icinde acilan, kullanicinin kendi favori kisayol dugmelerini tasarlayip yerlestirdigi panel. Dugmeler `FavorilerSettings.json` dosyasinda saklanir (konum, boyut, renk, isim). Dugmeye tiklayinca ilgili modul formu acilir.
**Once ne olmali (onkosul):** Login basarili olduktan sonra `FrmAna.MasaUstuAc()` tek instance halinde acar (`Name="Masaustu"`, MdiParent=FrmAna). Acilista `ButonBagla()` JSON'dan dugme listesini okur, yoksa bos liste ile dosyayi olusturur.
**Sonra ne olur:** Duzenleme modunda yapilan degisiklikler (tasima, isim, renk, ekleme/silme) `AyarKaydet()` ile `FavorilerSettings.json`'a serialize edilir. Normal modda dugmeye tiklaninca `FormAc.FormSec(btn.Tag)` ile ilgili form acilir.
**Butonlar & kisayollar (sag tik / ContextMenuStrip `btnContext`):**
- `Duzenle` — `DuzenleAc()`: duzenleme modunu acar (panel arka plani DarkCyan, dugmeler surukle-birak ile tasinabilir).
- `Kaydet` — `DuzenleKapat()`: duzenleme modunu kapatir ve `AyarKaydet()` ile JSON'a yazar.
- `Iptal` — `DuzenleKapat()`: duzenleme modunu kapatir.
- `YenidenIsimver` — `Interaction.InputBox` ile secili dugmeye yeni isim verir.
- `Renk` — `ColorDialog` ile secili dugmenin arka plan rengini degistirir.
- `Butonu Kaldir` — onay sonrasi secili dugmeyi listeden ve panelden siler.
**Cagirdigi katmanlar:**
- UI yonlendirme: `FormAc.FormSec(tag)` — Tag string'ine gore ilgili modul formunu acar (Siparisler, ReceteUretimEkle, UretimEmirleri, OperasyonTakip, IstasyonTakip, MikroStokListesi, IstasyonRaporu vb.; FrmAna ribbon dugmeleri de ayni metodu kullanir).
- Dosya: `JsonConvert` ile `FavorilerSettings.json` oku/yaz (Newtonsoft).
- SQL/Prosedur: - ; API: -
**Istasyon sirasiyla iliskisi:** Dolayli — masaustu dugmesi `IstasyonTakip` ile saha takip ekranini (FrmUretimIstasyonTakip) acabilir; ancak akis mantigini iceren ekranlar UretimIstasyon modulundedir.
**Notlar:** Yeni favori dugme `FrmMasaustu.ButonEkle(Tag)` ile (disaridan, FrmAna ribbon'undan) eklenir. Dugmeler hareket eşiği 5px ile koruma altinda (kayma olunca tiklama iptal). Sinif adi kodda `FrmMasaustu1`.

---

### Genel Ayarlar (`FrmGenelAyarlar.cs`)
**Ne ise yarar:** `Modul='Genel'` kapsamindaki uygulama genel ayarlarini listeleyip degerlerini duzenler (orn. `PlKapat` plasiyer/PL kapanma davranisi). Sadece mevcut kayitlarin Deger/Aciklama alani guncellenir; yeni kod ekleme/silme aktif degildir.
**Once ne olmali (onkosul):** Veritabani guncellemesi (`Ortak.DatabaseGuncelleUretim` / "DB Guncelle") ile `Ayar` tablosundaki Genel kayitlari olusturulmus olmali. Ribbon: `BarBtnGenelAyarlar` -> `FrmGenelAyarlar.ShowDialog()`.
**Sonra ne olur:** Kaydet sonrasi `Ayar` tablosuna `InsertOrUpdate` yazilir, grid yeniden baglanir ve `Ortak.GenelAyarlarBagla()` (ek olarak `Ortak.MikroEntAyarlarBagla()`) ile RAM'deki ayar cache'i tazelenir (`Ortak.GenelAyarlar`, `Ortak.PlKapat`).
**Butonlar & kisayollar:**
- `BtnKaydet` — `Kaydet()` -> `AktarModele()` -> `_srv.InsertOrUpdate(_mdl)`, sonra `Bagla()` + `Ortak.GenelAyarlarBagla()` + `Ortak.MikroEntAyarlarBagla()`.
- `BtnDuzenle` — `Duzenle()`: gridde secili satiri text kutularina aktarir (kayda cift tik = ayni is).
- `BtnYeni` — `TemizleText()`: text kutularini bosaltir.
- `BtnSil` — onay sorar; ancak `Sil()` govdesi yorumlu (silme fiilen pasif).
- Cift tik / Enter (grid) — secim modunda satiri secip kapatir; degilse Duzenle.
**Cagirdigi katmanlar:**
- Service: `IAyarService` (`Ortak.DbPro.Ayarlar`, BaseService<Ayar>) — `SelectListWhere("where Modul='Genel' Order By Kodu")`, `InsertOrUpdate(Ayar)`.
- Helper: `Ortak.GenelAyarlarBagla()` — `Modul='Genel'` ayarlarini cache'e alir, `PlKapat` flag'ini set eder.
- SQL/Prosedur: dogrudan SP yok (generic DAL). ; API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** `TxtKodu` salt-okunur (Enabled=false); Kodu yeni olusturulamaz, sadece var olan ayarin Degeri/Aciklamasi guncellenir.

---

### Mikro Entegre Ayarlari (`FrmMikroEntAyarlari.cs`)
**Ne ise yarar:** Mikro ERP entegrasyonunun beyni. Iki bolumden olusur: (1) ust "Fisler Genel" panelinde Mikro Firma No, Kullanici Kodu ve her uretim hareketinin (urun girisi, stok cikisi, urun/stok fire, sarf cikisi, fire girisi, hizli uretim) Mikro'da hangi fis turune yazilacaginin secimi; (2) sekmeli alt grid ile `Modul='MikroEntegre'` altindaki tum ayar detaylarinin grup grup duzenlenmesi.
**Once ne olmali (onkosul):** DB Guncelle ile `Ayar` tablosunda `Modul='MikroEntegre'` (GENEL, FisTuru ve diger grup) kayitlari olusturulmus olmali. Ribbon: `BarBtnMikroEntAyarlari` -> `FrmMikroEntAyarlari.ShowDialog()`. Acilista `GenelBagla()` combo'lari `MikroKayitFisTurleri` listeleriyle, sekmeleri `GetAyarFisTurleriList()` grup adlariyla doldurur.
**Sonra ne olur:** "Genel Kaydet" tum genel/fis-turu degerlerini topluca `InsertOrUpdate(_listGenelVeTur)` yapar; alt grid "Kaydet" tek detay kaydini yazar. Her ikisinde de `Ortak.MikroEntAyarlarBagla()` cache'i tazelenir (`Ortak.MikroEntAyarlar`). Bu ayarlar daha sonra Mikro'ya kayit ekranlarinda (FrmMikroyaUretimKaydetV2, FrmMikroyaSarfFireKaydet, FrmHizliUretimEG) `MikroConvertManager` tarafindan okunur.
**Butonlar & kisayollar:**
- `Genel Kaydet` / `BtnGenelKaydet` — `GenelAktar()` ile Firma No/Kullanici Kodu ve tum fis-turu combo degerlerini modele aktarip topluca kaydeder + cache tazeler.
- `BtnKaydet` (alt buton serisi) — secili detay ayar satirini kaydeder + `Ortak.MikroEntAyarlarBagla()`.
- `BtnDuzenle` — gridde secili `Ayar` satirini Kodu/Aciklama/Deger kutularina aktarir.
- `BtnYeni` — text kutularini temizler.
- `Ayarlari Sil` / `BtnAyarlarSil` — onay sonrasi `_srv.Delete(c => c.Modul=='MikroEntegre')` ile tum MikroEntegre ayarlarini sifirlar; sonra DB Guncelle yeniden cagrilmali.
- `BtnSil` — gizli (Visible=false), kullanilmiyor.
- Sekmeler (`TabGrupAdlari`) — sekme degisince `DetayBagla(grup)` ile o grubun ayar satirlari gridde gosterilir.
- Cift tik / Enter (grid) — secim modunda secip kapatir; degilse `BtnDuzenle.PerformClick()`.
**Cagirdigi katmanlar:**
- Service: `IAyarService` — `SelectListWhere("where Modul='MikroEntegre' Order By Modul,Grup,Kodu")`, `InsertOrUpdate(Ayar / List<Ayar>)`, `Delete(c => c.Modul=='MikroEntegre')`.
- Manager: `MikroKayitFisTurleri` — `GetAyarFisTurleriList()` (sekme grup adlari) ve `GetUretimUrunGirisFisiTuruList / GetUretimStokCikisFisiTuruList / GetUretimUrunFireCikisFisiTuruList / GetUretimStokFireCikisFisiTuruList / GetSarfCikisFisiTuruList / GetFireGirisFisiTuruList / GetHizliUretimFisiTuruList` (combo secenekleri); `MikroAyarFisTurleri` enum'u Kodu eslesmesinde kullanilir.
- Helper: `Ortak.MikroEntAyarlarBagla()` — `Modul='MikroEntegre'` ayarlarini RAM cache'e (`Ortak.MikroEntAyarlar`) alir.
- SQL/Prosedur: dogrudan SP yok. ; API: -
**Istasyon sirasiyla iliskisi:** Dolayli — buradaki fis turu ayarlari, istasyon/operasyon sonunda Mikro'ya uretim/stok/fire fisi yazilirken kullanilir (MikroConvertManager).
**Notlar:** Combo'lar `Grup='GENEL'` (FirmaNo, KullaniciKodu) ve `Grup='FisTuru'` kayitlariyla iki yonlu eslestirilir. `TxtKodu` salt-okunur. Tasarimda 10 sabit XtraTabPage var ama sekmeler calistirmada `_listGrupAd` (8 ayar fis turu) ile dinamik doldurulur.

---

### Istasyon Uretim Ayarlari (`FrmIstasyonUretimAyarlari.cs`)
**Ne ise yarar:** `Modul='IstasyonUretim'` kapsamindaki saha/istasyon uretim davranis ayarlarini listeleyip duzenler (orn. `Grup='Istasyon'`, `Kodu='MalKabulKullan'` -> mal kabul ozelliginin acik/kapali olmasi).
**Once ne olmali (onkosul):** DB Guncelle ile `Ayar` tablosunda IstasyonUretim kayitlari olusmus olmali. Ribbon: `BarBtnIstasyonUretimAyarlari` -> `FrmIstasyonUretimAyarlari.ShowDialog()`.
**Sonra ne olur:** Kaydet -> `Ayar.InsertOrUpdate` ardindan grid yenilenir. (Bu ekran kaydetten sonra `Ortak.MikroEntAyarlarBagla()` cagirir; istasyon cache'i `Ortak.IstasyonAyarlarBagla()` ise FrmAna acilista calisir ve `Ortak.MalKabulKullan` flag'ini set eder.)
**Butonlar & kisayollar:**
- `BtnKaydet` — `Kaydet()` -> `_srv.InsertOrUpdate(_mdl)`, sonra `Bagla()` + `Ortak.MikroEntAyarlarBagla()`.
- `BtnDuzenle` — secili satiri kutulara aktarir.
- `BtnYeni` — kutulari temizler.
- `BtnSil` — onay sorar; `Sil()` govdesi yorumlu (silme pasif).
- Cift tik / Enter (grid) — secim modunda secer; degilse Duzenle.
**Cagirdigi katmanlar:**
- Service: `IAyarService` — `SelectListWhere("where Modul='IstasyonUretim' Order By Kodu")`, `InsertOrUpdate(Ayar)`.
- Helper: `Ortak.IstasyonAyarlarBagla()` — `Modul='IstasyonUretim'` ayarlarini cache'ler, `MalKabulKullan` flag'ini set eder (FrmAna acilista).
- SQL/Prosedur: dogrudan SP yok. ; API: -
**Istasyon sirasiyla iliskisi:** Dogrudan — buradaki ayarlar (orn. MalKabulKullan) saha istasyon takip akisini etkiler.
**Notlar:** Kod yapisi GenelAyarlar ile birebir ayni (sadece `ayarModul` farkli). TxtKodu salt-okunur degil ama Kodu degeri modele aktarilmiyor (sadece Aciklama+Deger guncellenir).

---

### SMS Ayarlari (`FrmSmsAyarlari.cs`)
**Ne ise yarar:** SMS gonderim saglayicisi bilgilerini (Kullanici Kodu, Sifre, Baslik/gonderici adi, Gonderim URL, Rapor URL) tutar. Diger ayar ekranlarindan farkli olarak ayri bir `SmsAyar` tablosu/servisi kullanir ve tam CRUD (silme dahil) aktiftir.
**Once ne olmali (onkosul):** DB Guncelle ile `SmsAyar` tablosu olusmus olmali. Ribbon: `BarBtnSmsAyarlari` -> `FrmSmsAyarlari.ShowDialog()`.
**Sonra ne olur:** Kaydet -> `SmsAyar.InsertOrUpdate`; bu degerler SMS gonderimi yapan modul (orn. SMS Rapor / sevkiyat bildirimleri) tarafindan kullanilir.
**Butonlar & kisayollar:**
- `BtnKaydet` — `Kaydet()`: `TextLeriKontrolEt()` (5 alan da zorunlu) -> `_srv.InsertOrUpdate(_mdl)` -> grid yenilenir.
- `BtnDuzenle` — secili satiri kutulara aktarir.
- `BtnYeni` — kutulari temizler.
- `BtnSil` — onay sonrasi `Sil()` -> `_srv.Delete(_mdl)` (gercek silme yapar).
- `myButton1` — Gonderim URL kutusunu temizler; `myButton2` — Rapor URL kutusunu temizler.
- Cift tik / Enter (grid) — secim modunda secer; degilse `BtnDuzenle.PerformClick()`.
**Cagirdigi katmanlar:**
- Service: `ISmsAyarService` (`Ortak.DbPro.SmsAyar`) — `SelectListWhere("")`, `InsertOrUpdate(SmsAyar)`, `Delete(SmsAyar)`.
- SQL/Prosedur: dogrudan SP yok. ; API: SMS gonderim URL'leri (GunderimUrl/RaporUrl) ayar olarak saklanir; cagrilma SMS modulunde.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Bu modulde silmenin gercekten calistigi tek ayar ekrani; zorunlu alan validasyonu da burada yapilir.

---

### Acil Mesaj (`FrmMesajGenel.cs`)
**Ne ise yarar:** Uygulama genelinde gosterilecek tek bir "Acil" duyuru mesajini (`Mesajlar` tablosu, `Modul='Genel'`, `Kodu='Acil'`) zengin metin (RichTextBox) olarak duzenler.
**Once ne olmali (onkosul):** DB Guncelle ile `Mesajlar` tablosunda Genel/Acil kaydi mevcut olmali. Ribbon: `BarBtnAcilMesaj` -> `FrmMesajGenel.ShowDialog()`. Acilista `MesajBagla()` ile mevcut mesaj yuklenir.
**Sonra ne olur:** Kaydet -> `Mesajlar.InsertOrUpdate(msj)` ile guncellenir ve form kapanir. Mesaj, uygulamada acil duyuru gosteren yerlerde okunur.
**Butonlar & kisayollar:**
- `BtnKaydet` — `BtnKaydet_Click`: RichTextBox metnini `msj.Mesaj`'a aktarir, `_srv.InsertOrUpdate(msj)` ve formu kapatir.
- `BtnSil` — gizli (Visible=false).
**Cagirdigi katmanlar:**
- Service: `IMesajlarService` (`Ortak.DbPro.Mesajlar`) — `SelectFirstWhere("where Modul='Genel' and Kodu='Acil'")`, `InsertOrUpdate(Mesajlar)`.
- SQL/Prosedur: dogrudan SP yok. ; API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Tek kayit uzerinde calisir; yeni mesaj olusturmaz, var olan Genel/Acil kaydini gunceller.

---

### Stok Kodu Guncelleme (`FrmStokKodGuncelle.cs`)
**Ne ise yarar:** Bir stok kodunun (eski) sistem genelinde baska bir koda (yeni) toplu olarak degistirilmesini saglar. Mikro tarafinda stok kodu degistiginde UretimV3 (uretim) veritabanindaki tum ilgili tablolarda referansi tek transaction icinde gunceller.
**Once ne olmali (onkosul):** Ribbon: `btnStokGuncelle` -> `FrmStokKodGuncelle.ShowDialog()`. Kullanici eski ve yeni stok kodlarini girer.
**Sonra ne olur:** Onay sonrasi `IUretimStokFisService.StokKoduGuncelle(eski, yeni)` calisir; tek transaction icinde su tablolar UPDATE edilir: `DepoStokHareket`, `UretimStok`, `DepoStok`, `DepoKabulIrsaliyeHareket`, `DepoStokBarkod`, `ReceteDetay (VarsayilanStokKodu)`, `SiparisHareket`, `SiparisHareketDetay`, `UretimStokFisHareket`. Hata olursa rollback yapilir.
**Butonlar & kisayollar:**
- `Kaydet` / `btnKaydet` — `btnKaydet_Click`: "X kodu Y ile degistirilecek, devam?" onayi -> `_srv.StokKoduGuncelle(...)` -> basariliysa "Basariyla Guncellendi".
**Cagirdigi katmanlar:**
- Service: `IUretimStokFisService` (`Ortak.DbPro.UretimStokFis`) — `StokKoduGuncelle(string eskiKod, string yeniKod)`: 9 tabloda transactional UPDATE.
- SQL/Prosedur: inline UPDATE'ler (SP degil), `_dal.Execute(...)` ile tek transaction. ; API: -
**Istasyon sirasiyla iliskisi:** Dolayli — uretim/recete/siparis hareketlerindeki stok kodu referanslarini gunceller; akis sirasini degistirmez.
**Notlar:** Dikkat: stok kodu string interpolation ile sorguya gomulur (SQL injection riski); kullanici tarafindan dikkatli girilmeli. Plain WinForms `Form` tabanli (MyFrmKayit degil).

---

### Yardimci/Altyapi dosyalari (form degil)
- `MasaUstuButonlar.cs` — Masaustu favori dugmesinin POCO modeli (Text, Name, LocationX/Y, SizeX/Y, Icon64, Renk). `FavorilerSettings.json`'a serialize edilir.
- `MyExtextionsObject.cs` (`MyExtentionObject`) — string uzanti metotlari `MesajHata / MesajBilgi / MesajSor` (DevExpress XtraMessageBox), `IsNullOrEmpty` (Guid?/string) ve `ToDataTable<T>` cevirici uzantilari.
- `OrtakLis.cs` — donanim bazli lisans uretim/kontrol yardimcisi (MD5 + WMI islemci ID); FrmLisansGiris ve FrmAna acilis lisans kontrolu bunu kullanir.
- `MyFrmLoginPaneli.cs/.Designer.cs` — FrmLogin'in base formu; Esc=Kapat, F2=Giris kisayollari, surukle-ile-tasi ve giris kontrol kutulari (`TxtKullanici`, `TxtSifre`, `BtnGiris`, `BtnKapat`) burada tanimli.
