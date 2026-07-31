## Modul: SmsModule

UretimV4 (CepPatronERP.exe) masaustu uygulamasinda NetGSM SMS gonderim raporlarini tarih araliginda listeleyen tek formdan olusan kucuk bir modul. Modulun amaci, NetGSM uzerinden gonderilen SMS'lerin teslim/durum raporlarini (NetGSM rapor servisinden) cekip DevExpress grid'de gostermektir. SMS gonderme arayuzu (telefon + mesaj alanlari, "Gonder" butonu) form uzerinde mevcut fakat tasarimda gizlenmis (`Visible=false`) ve ilgili click handler'i (`myButton2_Click`) tamamen yorum satiri oldugu icin SMS gonderme islevi su an pasiftir; form pratikte sadece RAPOR (sorgulama/listeleme) amacli calisir.

Modul, NetGSM kimlik bilgilerini ve servis URL'lerini `SmsAyar` tablosundan okur. Bu ayarlar ayri bir form olan `FrmSmsAyarlari` (AyarVeGenelModul klasoru) uzerinden girilir; bu yuzden onkosul olarak SMS ayarlarinin daha once kaydedilmis olmasi gerekir. SMS gonderim/rapor mantigi `My.Sms.dll` adli harici referansta (`My.Sms` / `My.Sms.NetGsm` namespace'leri; `NetGsmSmsManager`, `SmsSettings` siniflari) bulunur ve kaynak kodu projede yer almaz; davranislar form kullanimindan cikarilmistir.

Onemli not: Bu modul UretimV3_FEZA uretim akisinin (operasyon/istasyon/recete) hicbir parcasi degildir. Uretim miktar/akis motoru (Uretim_MiktarGuncelle, Uretim_PlanlananGuncelle, Uretim_SonrakiIstasyonaGonder) ile hicbir iliskisi yoktur. Sadece `SmsAyar` tablosunu okur ve dis NetGSM HTTP API'sine baglanir; uretim tablolarina dokunmaz.

### SMS Raporu (`MyUI\SmsModule\FrmSmsRapor.cs` + `FrmSmsRapor.Designer.cs`)
**Ne ise yarar:** Iki tarih (baslangic/bitis) arasinda NetGSM uzerinden gonderilmis SMS'lerin durum/teslim raporunu NetGSM rapor servisinden cekip grid'de listeler. (Tasarimda gizli alanlar nedeniyle tek aktif islevi raporlamadir; manuel SMS gonderme ekrandadir ama pasiftir.)
**Once ne olmali (onkosul):**
- SMS ayarlari onceden girilmis olmali: `SmsAyar` tablosunda en az bir kayit bulunmali (KullaniciKodu, Sifre, Baslik, GunderimUrl, RaporUrl). Bu kayit `FrmSmsAyarlari` formundan girilir. Kayit yoksa form acilisinda "Sms Ay,arlari Yapilmamis." bilgi mesaji cikar ve `settings` null kalir (rapor cekilemez).
- Form, ana ekrandan (`FrmAna`) ribbon ogesi `BarBtnSmsRapor` tiklanarak MDI alt penceresi olarak acilir (`f.MdiParent = this; f.Show();`).
**Sonra ne olur:**
- Hicbir veritabani tablosu YAZILMAZ/degismez; islem salt-okunurdur. Sadece dis NetGSM rapor HTTP servisine (RaporUrl) istek atilir ve donen rapor satirlari `myGrid1` grid'ine baglanir (`myGrid1.DataSource = rs.Data`).
- Hata olursa `MesajHata(rs.Message)` ile kullaniciya bildirilir; basari/hata disinda baska ekrana yonlendirme yoktur (form acik kalir).
- Uretim akisi tablolari (UretimIstasyonHareket, UretimIstasyon, ReceteAna vb.) etkilenmez.
**Butonlar & kisayollar:**
- `BtnAra` (taban form butonu, `MyFrmListe`) — Click `BtnAra_Click` -> `GetRapor()` cagirir: `myDateEdit1` (BasTarih) ve `myDateEdit2` (BitTarih) degerleriyle NetGSM rapor servisinden tarih araliginda rapor ceker, grid'e basar.
- `myDateEdit1` (etiket `BasTarih`) — rapor baslangic tarihi (maske `dd.MM.yyyy`, `EnterMoveNextControl=true` -> Enter ile sonraki kontrole gecer).
- `myDateEdit2` (etiket `BitTarih`) — rapor bitis tarihi (maske `dd.MM.yyyy`).
- `myButton2` (Text "Gonder", `Visible=false`) — Click `myButton2_Click` PASIF: handler govdesi tamamen yorum satiri (`smsManager.SmsSend(...)` cagrisi devre disi). Manuel SMS gonderme islevi aktif degil.
- `myTextEdit1` (etiket `Tel`, `Visible=false`, `MaxLength=75`) — manuel SMS icin telefon alani (gizli/pasif).
- `richTextBox2` (Text "Selam", `Visible=false`) — manuel SMS mesaj govdesi (gizli/pasif).
- Taban form butonlari `BtnYazdir`, `BtnDizayn`, `BtnTemizle`, `BtnKapat` (`MyFrmListe`'den miras) — bu formda OZEL Click handler'i baglanmamistir; yalnizca `BtnAra.Click` Designer'da bu forma ozel olarak wire edilmistir. `BtnKapat` taban formun standart pencere kapatma davranisini saglar; `BtnYazdir`/`BtnDizayn` grid yazdirma/kolon dizayni icin taban davranisina baglidir.
- Kisayollar: Designer'da bu forma ozel ShortcutKeys/F-tusu tanimi YOK. Tarih kontrollerinde `EnterMoveNextControl=true`, grid view'da `EnterMoveNextColumn=true` (Enter -> sonraki kolon) ayarlidir. Enter=Kaydet / Esc=Kapat gibi davranislar varsa taban sinif `MyFrmListe`'den (harici DLL) gelir; kaynakta dogrulanamadi.
**Cagirdigi katmanlar:**
- Service: `ISmsAyarService.SelectFirstWhere("")` — `Ortak.DbPro.SmsAyar` uzerinden cagrilir; `SmsAyar` tablosundan ilk kaydi getirir (KullaniciKodu, Sifre, Baslik, GunderimUrl, RaporUrl). `BaglaSmsAyar()` icinde bu degerlerle `SmsSettings` nesnesi olusturulur. (`SmsAyarService : BaseService<SmsAyar>`, DAL: `SmsAyarDal : BaseDal<SmsAyar>`.)
- Manager (harici DLL `My.Sms.dll`): `NetGsmSmsManager.GetRaporByTarih(DateTime bas, DateTime son)` — NetGSM rapor servisine (RaporUrl) HTTP istegi atip tarih araligindaki SMS durum raporunu `IDataResult` olarak dondurur (`rs.Data` grid'e baglanir). Kaynak kodu projede yok; imza ve davranis form kullanimindan cikarilmistir.
- Manager (harici DLL): `NetGsmSmsManager.SmsSend(string tel, string mesaj)` — manuel SMS gonderir; ANCAK cagrisi formda yorum satirinda oldugu icin kullanilmiyor.
- Manager kurucusu (harici DLL): `new NetGsmSmsManager(SmsSettings settings)`, `new SmsSettings(KullaniciKodu, Sifre, Baslik, GunderimUrl, RaporUrl)`.
- SQL/Prosedur: Stored procedure CAGRILMAZ. Yalnizca `SmsAyar` tablosu okunur (SELECT, BaseDal/Dapper uzerinden). Tablo semasi: `My.DataBaseSettings\DatabaseCreate\UretimCreateModule\SmsAyarCreates.cs` (kolonlar: Id PK, KullaniciKodu varchar(50), Sifre varchar(250), Baslik varchar(50), GunderimUrl varchar(250), RaporUrl varchar(250)).
- API: Dis NetGSM REST servisi — varsayilan rapor URL'si `https://api.netgsm.com.tr/sms/report`, gonderim URL'si `https://api.netgsm.com.tr/sms/send/xml` (`SmsAyar` entity varsayilanlari). Bunlar UretimV4 projesinin kendi API'si degil, ucuncu taraf NetGSM HTTP API'sidir.
**Istasyon sirasiyla iliskisi:** - (Yok. Modul uretim operasyon/istasyon akisindan tamamen bagimsizdir.)
**Notlar:**
- Form acilisi (`FrmSmsRapor_Load`): `BaglaSmsAyar()` -> `new NetGsmSmsManager(settings)` -> `GetRapor()` -> `myGrid1.GridYerlesimYukle()`. Yani form aciliminda otomatik olarak (varsayilan tarihlerle) bir rapor cekme denemesi yapilir.
- `BaglaSmsAyar()` icinde ayar kaydi yoksa "Sms Ay,arlari Yapilmamis." (kod icinde yazim hatasi mevcut) mesaji gosterilir; ardindan `smsManager = new NetGsmSmsManager(settings)` settings null iken olusturulur ve `GetRapor()` cagrisi NetGSM'e bos kimlikle gidip hata dondurebilir.
- Designer'da tarih kontrollerinin sabit varsayilan degeri `"26.05.2023"` olarak gomulu (dinamik DateTime.Now ile doldurulmaz); kullanici Ara'dan once tarihleri elle ayarlamalidir.
- `myGrid1.MyGridKayitAdi = "GridAdi_6C9DAB4298944D2BACB8A0C3323FED47"` ile grid kolon yerlesimi kullaniciya ozel saklanir/yuklenir (`GridYerlesimYukle()`).
- Iliskili ayar formu: `MyUI\AyarVeGenelModul\FrmSmsAyarlari.cs` (`MyFrmKayit` turevi) — `SmsAyar` CRUD (BtnKaydet/BtnSil/BtnDuzenle/BtnYeni); `ISmsAyarService.InsertOrUpdate/Delete/SelectListWhere` cagirir. Bu form `FrmAna.BarBtnSmsAyarlari_ItemClick` -> `f.ShowDialog()` ile acilir. Bu modulun (SmsModule klasoru) parcasi degildir ama onkosulu olusturur.
