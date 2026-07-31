## Modul: MailModul

UretimV4 (CepPatronERP WinForms) icindeki mail/e-posta modulu. Uretim DB'sindeki (`UretimV3_FEZA`) `MailSettings` tablosunda saklanan SMTP gonderici hesaplarini yonetir ve bu hesaplar uzerinden e-posta gonderir. Uc formdan olusur: (1) SMTP hesap ayarlarinin CRUD'unu yapan `FrmMailSettings`, (2) ayarlarin gercekten calistigini denemek icin manuel test maili gonderen `FrmMailGonderOrnek`, (3) bir siparis fisini PDF/XLSX olarak uretip ekli e-posta ile cariye gonderen `FrmFisMailGonder`. Tum formlar `MailManager(Ortak.DbPro, Ortak.DbMikro)` uzerinden calisir; ayarlar `_dbPro.MailSettings` (Pro/Uretim DB) tablosundan okunur/yazilir, gercek gonderim `System.Net.Mail.SmtpClient` ile yapilir. SMTP sifresi DB'de simetrik sifreli tutulur (`Sifrele`/`SifreCoz` + `Ortak.GetKey()`). Modul uretim miktar/akis motoruyla (operasyon-sira, istasyon prosedurleri) iliskili degildir; tamamen yardimci/altyapi moduldur.

### Mail Ayarlari (`FrmMailSettings.cs` / `FrmMailSettings.Designer.cs`)
**Ne ise yarar:** SMTP gonderici hesaplarinin (Host, Port, MailAdres, Pass, DisplayName, EnableSsl, varsayilan Konu/Body) listelendigi ve eklenip/guncellenip/silindigi ayar ekranidir. Ust panelde giris alanlari, altta kayitli ayar listesi (grid) vardir.
**Once ne olmali (onkosul):** Uygulamaya giris yapilmis ve `Ortak.DbPro`/`Ortak.DbMikro` baglanti factory'leri hazir olmali. Form, ana menuden `FrmAna.BarBtnMailAyarlari_ItemClick` ile `new FrmMailSettings().ShowDialog()` cagrilarak acilir. `MailSettings` tablosunun olmasi gerekir.
**Sonra ne olur:** Kaydet -> `MailSettings` tablosuna INSERT veya UPDATE (`MailManager.MailSettingsKaydet` -> `_dbPro.MailSettings.InsertOrUpdate`). Sil -> `MailSettings`'ten ilgili Id silinir (`MailManager.MailSettingsSil` -> `_dbPro.MailSettings.Delete`). Her iki islemden sonra grid yeniden baglanir (`Bagla`) ve form alanlari temizlenir (`TemizleText`). Ekran kapaninca cagiran ana menuye donulur.
**Butonlar & kisayollar:**
- `Yeni` (BtnYeni) — `TemizleText()`: form alanlarini bosaltir, yeni bos `MailAyar` olusturur (yeni kayit girisine hazirlar).
- `Kaydet` (BtnKaydet) — `Kaydet()`: zorunlu alan kontrolu (`TextleriKontrolEt`: Host/Port/MailAdres/Pass dolu mu), `AktarRowa()` ile alanlari modele aktarir (sifre `TxtPass.Text.Sifrele(Ortak.GetKey())` ile sifrelenir), `_mng.MailSettingsKaydet` cagirir.
- `Sil` (BtnSil) — once "Kaydı Silmek İstiyormusunuz" onayi (`MesajSor`), onaylanirsa `Sil()` -> `await _mng.MailSettingsSil(_fis.Id)`.
- `Kapat` (BtnKapat) — `this.Close()`.
- `Test Mail` (BtnTestMail) — `new FrmMailGonderOrnek().ShowDialog()`: test maili gonderme formunu acar.
- Grid satirina cift tik / Enter (`MyView1_MyEventDoubleClickEnter`) — secili `MailAyar` satirini klonlar, `AktarTextlere()` ile form alanlarina doldurur (sifre `SifreCoz` ile cozulup gosterilir). Grid tum kolonlari ReadOnly'dir; duzenleme yalnizca ust paneldeki textbox'lardan yapilir.
- Genel kisayol: `.Designer.cs`'de buton ShortcutKeys / form-level Enter=Kaydet, Esc=Kapat tanimi YOK; butonlar yalnizca Click ile calisir. MyTextEdit'lerde `EnterMoveNextControl=true` (Enter ile sonraki alana gecis).
**Cagirdigi katmanlar:**
- Manager/Service: `MailManager.GetMailSettings()` — `_dbPro.MailSettings.SelectListWhere("")` ile tum ayarlari listeler (grid kaynagi).
- Manager/Service: `MailManager.MailSettingsKaydet(MailAyar)` — `_dbPro.MailSettings.InsertOrUpdate(fis)` (Id varsa UPDATE, yoksa INSERT; `MailAyar.GetInsertCode`/`GetUpdateCode` SQL'leri).
- Manager/Service: `MailManager.MailSettingsSil(Guid? id)` — `_dbPro.MailSettings.Delete(c => c.Id == id)`, async.
- Service/DAL: `MailAyarService : BaseService<MailAyar>` / `MailAyarDal : BaseDal<MailAyar>` — `MailSettings` tablosu icin temel CRUD (Manager dogrudan `_dbPro.MailSettings` DAL'i uzerinden cagirir).
- Yardimci: `string.Sifrele(key)` / `string.SifreCoz(key)` (sifre sifrele/coz), `Ortak.GetKey()` (master anahtar), `MesajHata`/`MesajSor` (toast/diyalog uzantilari).
- SQL/Prosedur: Stored procedure kullanmaz; `MailAyar.GetInsertCode()` / `GetUpdateCode()` parametreli SQL stringleri uzerinden Dapper ile calisir. Tablo: `MailSettings`.
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Form veri kaynagi `Ortak.DbPro` (Uretim/Pro DB), `MailManager` ctor'unda `Ortak.DbMikro` da gecilir ama bu ekranda Mikro DB kullanilmaz. Sifre gridde maskelenmez ama TxtPass'ta `PasswordChar='*'`. `MailKodu` (orn. "Şirket Maili") bilgilendirme amacli; gonderimde kullanilan tek/ilk ayar genelde `FirstOrDefault()` ile secilir (diger formlarda).

### Mail Gonder Ornek / Test Mail (`FrmMailGonderOrnek.cs` / `FrmMailGonderOrnek.Designer.cs`)
**Ne ise yarar:** Kayitli SMTP ayarinin gercekten mail gonderebildigini dogrulamak icin manuel/test e-postasi gonderme ekranidir. Alici mail adresi, konu ve mesaj govdesi girilip tek tikla deneme maili atilir (ek dosya yok).
**Once ne olmali (onkosul):** En az bir gecerli `MailSettings` kaydi olmali (Host/Port/MailAdres/Pass/EnableSsl dogru). Form `FrmMailSettings` icindeki `Test Mail` butonundan `ShowDialog` ile acilir. Yuklenince `Bagla()` ayarlari `_list`'e ceker.
**Sonra ne olur:** Gonder -> listenin ilk ayari (`_list.FirstOrDefault()`) ile `MailManager.GonderTek(...)` cagirilir; basariliysa "Mail Gonderildi" bilgi mesaji, hata varsa hata mesaji gosterilir. Veritabaninda degisiklik YAPMAZ (sadece SMTP gonderimi). Kapat ile cagiran ayar ekranina donulur.
**Butonlar & kisayollar:**
- `Gönder` (BtnGonder) — `BtnGonder_Click`: `_mdl = _list.FirstOrDefault()`; ayar yoksa "mail Ayarları bulunamadı" hatasi; varsa `MailManager.GonderTek(_mdl, TxtMail.Text, TxtKonu.Text, TxtBody.Text, null, Ortak.GetKey())` (dosya=null, sadece duz mail). Sonuc toast ile bildirilir.
- `Kapat` (BtnKapat) — `this.Close()`.
- Kisayol: `.Designer.cs`'de ShortcutKeys/F-tusu tanimi YOK. Alanlarda `EnterMoveNextControl=true`.
**Cagirdigi katmanlar:**
- Manager/Service: `MailManager.GetMailSettings()` — yuklemede ayar listesini ceker (`_dbPro.MailSettings.SelectListWhere("")`).
- Manager/Service: `MailManager.GonderTek(MailAyar ayar, string alicimail, string konu, string body, byte[] dosya, string masterKey, string dosyaUzanti="pdf")` (static) — `SmtpClient` ile (Host/Port/EnableSsl, `NetworkCredential(MailAdres, Pass.SifreCoz(masterKey))`) tek alici maili gonderir; `dosya` null oldugundan eksiz gonderim. `SuccessResult`/`ErrorResult` doner.
- Yardimci: `Ortak.GetKey()`, `MesajBilgi`/`MesajHata`.
- SQL/Prosedur: Yalnizca `MailSettings` SELECT (gonderimde DB yazimi yok). Prosedur yok.
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Her zaman ilk ayar (`FirstOrDefault`) kullanilir; birden fazla ayar varsa hangisinin secileceği UI'dan secilemez. Mail body `IsBodyHtml=true` olarak gonderilir.

### Sipariş Mail Gonder (`FrmFisMailGonder.cs` / `FrmFisMailGonder.Designer.cs`)
**Ne ise yarar:** Bir siparis fisini (SiparisKayitModel) DevExpress dizayni ("SiparisMail" REPX) ile PDF veya XLSX'e cevirip, secilen mail adresine ek olarak gonderen ekrandir. Ekran govdesi bir log/RichTextBox'tir; her adim (dosya olusturma, gonderim, sonuc) buraya yazilir. Otomatik gonderim modunu da destekler.
**Once ne olmali (onkosul):** Gonderilecek siparis once hazirlanmis/secilmis olmali. Form siparis ekranindan (`FrmSiparisEd.MailGonder` -> `BtnMailGonder_Click`) doldurulup acilir: cagiran taraf `f.Mdl = _mdl` (siparis+hareket+detay modeli), `f.MailAdres = TxtEmail.Text`, `f.YazdirmaAdi = "SiparisMail"`, `f.OtoGonder` set eder, sonra `ShowDialog()`. Gecerli bir `MailSettings` kaydi (`_setting = ilk ayar`) ve "SiparisMail" dizayni mevcut olmali.
**Sonra ne olur:** Gonderim oncesi mail adresi `MailAddress` ile dogrulanir (`Mailkontrol`). Dosya, `Mdl`'den uretilen 3 tablolu `DataSet` ("Siparis", "Hareketler", "Detaylar") uzerinden `YazdirPdf`/`YazdirXlsx(YazdirmaAdi)` ile byte[]'a render edilir; `MailManager.GonderTek` ile ekli olarak gonderilir. Basari/hata her durumda log'a yazilir; ayrica toast gosterilir. `OtoGonder=true` ise yuklenir yuklenmez PDF gonderilir ve basariliysa `TmrKapat` (2 sn) ile form otomatik kapanir. Bu islem veritabaninda kayit DEGISTIRMEZ (sadece SMTP). Kapaninca cagiran siparis ekranina donulur.
**Butonlar & kisayollar:**
- `Gönder Pdf (Varsayılan)` (BtnGonderPdf) — `BtnGonderPdf_Click` -> `GonderPdf()`: mail kontrol -> `DatasetOlustur()` -> `ds.YazdirPdf(YazdirmaAdi)` -> `MailManager.GonderTek(_setting, mailAdres, _setting.Konu, _setting.Body, dosya, Ortak.GetKey(), "PDF")`.
- `Gönder Xlsx` (BtnGonderXlsx) — `BtnGonderXlsx_Click` -> `GonderXlsx()`: ayni akis ama `ds.YazdirXlsx(YazdirmaAdi)` + ek uzantisi "xlsx".
- `Kapat` (BtnKapat) — `this.Close()`.
- `TmrKapat` (Timer, Interval=2000ms) — `OtoGonder` basarili oldugunda Start edilir; Tick'te `Stop()` + `this.Close()` (formu otomatik kapatir).
- Kisayol: `.Designer.cs`'de ShortcutKeys/F-tusu tanimi YOK; buton metinlerinde "(Varsayılan)" PDF'in onerilen secenek oldugunu belirtir.
**Cagirdigi katmanlar:**
- Manager/Service: `MailManager.GetMailSettings()` — yuklemede ayarlari ceker, `_setting = rs.Data.FirstOrDefault()`.
- Manager/Service: `MailManager.GonderTek(...)` (static) — siparis dosyasini ek olarak SMTP ile gonderir (PDF/XLSX uzantisiyla).
- Yazdirma/Rapor: `DataSet.YazdirPdf(string)` ve `DataSet.YazdirXlsx(string)` (`My.Kontrol.Yazdirma` namespace, harici DevExpress yazdirma kutuphanesi) — verilen dizayn adina ("SiparisMail") gore DataSet'i PDF/XLSX byte[]'a render eder.
- Model/Yardimci: `SiparisKayitModel` (Siparis + List<SiparisHareket> + List<SiparisHareketDetay>); `Mdl.X.ToDataTable("...")` ile DataSet kurulur (`DatasetOlustur`). `Mailkontrol()`/`GetMailAdres()` mail dogrulama; `Logyaz()` RichTextBox log.
- SQL/Prosedur: Yalnizca `MailSettings` SELECT (`GetMailSettings`). Gonderimde DB yazimi/prosedur YOK.
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Cagiran taraf `FrmSiparisEd` (SiparisModule). `_setting` her zaman ilk ayardir. Hata mesajlarinda `Mdl.Siparis.CariKodu` log'a eklenir. PDF varsayilan secenektir. Bu form siparis fisi e-posta gonderimine ozeldir; genel mail gonderimi degildir.
