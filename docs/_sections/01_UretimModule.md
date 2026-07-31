## Modul: UretimModule

UretimV4 (WinForms masaustu ERP) icindeki **UretimModule** klasoru, uretim emirlerinin (is emri) olusturulmasi, baslatilmasi, takip edilmesi ve operasyon bazli uretim girisinin yonlendirilmesinden sorumludur. Klasor 4 formdan olusur: uretim emirlerinin listelendigi ve filtrelendigi `FrmUretimEmriListesi`, tek bir uretim emrinin acilis/duzenleme + uretim baslatma ekrani `FrmUretimEmriED`, operasyonlari kart (kanban) gorunumunde gosteren canli takip ekrani `FrmOperasyonTakip`, ve secilen operasyonun bekleyen/uretimdeki satirlarini grid olarak listeleyip uretim girisi veya istasyona gonderme islemine kopru kuran `FrmOperasyonTakipDetaylar`.

Modulun is mantigi cogunlukla `UretimEmriManager` (kayit/sil/baslat orkestrasyonu) ve `UretimTakipManagerV2` (canli takip sorgulari) uzerinden yurur. Tum miktar/akis hesaplari `Uretim_MiktarGuncelle`, `Uretim_PlanlananGuncelle`, `Uretim_DurumGuncelle` ve `Uretim_SonrakiIstasyonaGonder` stored procedure'lari ile yapilir; bu prosedurler operasyon Sira'sina gore miktarlari yukari toplar, bir sonraki operasyonun planlananina tasir ve istasyon gruplama acikken otomatik UretimIstasyon kaydi olusturur.

> Not (tum formlar icin): Base form siniflari (`MyFrmKayitFull`, `MyFrmListe`, `MyFrmSadeFull` — `My.Kontrol.Formlar` namespace'i) derlenmis bir kutuphanededir, kaynak kod bu projede yoktur. Bu nedenle base butonlar (BtnKaydet/BtnSil/BtnKapat/BtnYeni/BtnDuzenle/BtnAra/BtnTemizle/BtnYazdir/navigasyon) ve Enter=Kaydet / Esc=Kapat gibi global kisayollar kodla DOGRULANAMADI; Designer dosyalarinda sadece bu butonlarin gorunum/konum ayarlari mevcuttur. F-tusu kisayolu (ShortcutKeys) hicbir formda tanimli degildir.

---

### Uretim Emri Listesi (`FrmUretimEmriListesi.cs` / `.Designer.cs`)
**Ne ise yarar:** Uretim emirlerini (is emri) tarih, kod, cari, tur, recete ve duruma gore filtreleyip listeler. Ust grid uretim emrini, alt grid o emre ait operasyonlari gosterir. Durum sekmesi butonlari (Hepsi/Beklemede/Uretimde/Hazir) ile hizli filtre yapilir. Yeni uretim emri olusturmaya ve secili emri duzenlemeye giris noktasidir.
**Once ne olmali (onkosul):** Sistemde siparis ve recete tanimlari olmali (Turu/Durumu/ReceteAdi combolari `GenelService.GrupListesi` ile UretimEmri tablosundan beslenir). Form `SiparisKodundanFiltrele=true` ve `SiparisKodu` set edilerek de acilabilir (siparis ekranindan tek bir siparise drill-down).
**Sonra ne olur:** Satira cift tiklayinca (SecimIcinAcilmadiysa) `FrmUretimEmriED` acilir (`IdGuid` ile edit). Yeni kayit butonlari `FrmUretimEmriED`'yi `UretimTuru="Siparis"` veya `"Recete"` ile acar. Yazdir secili emri DevExpress rapor (`UretimEmriListesi`) olarak basar. Bu form dogrudan tablo degistirmez (salt okuma + navigasyon); kayit/silme islemleri ED formunda yapilir.
**Butonlar & kisayollar:**
- `BtnAra` — `Bagla()` ile filtreye gore `UretimEmri` listesini ceker (Frm_Load sonunda `PerformClick` ile otomatik aranir).
- `BtnTemizle` — tum filtre alanlarini (kod, ad, cari, tarihler, combolar) sifirlar.
- `BtnYazdir` — secili emir icin `Yazdir()`: `_list` DataSet'e cevrilip `ds.Yaz("UretimEmriListesi", false)` ile basilir.
- `BtnEkleSiparisden` — `FrmUretimEmriED` (UretimTuru="Siparis") acar.
- `BtnEkleReceteden` — `FrmUretimEmriED` (UretimTuru="Recete") acar.
- `BtnDurumuHepsi` / `BtnDurumuBeklemede` / `BtnDurumuUretimde` / `BtnDurumuHazir` — `DurumuAyarla(...)` ile `durumu` filtresini set edip butonu renklendirir, sonra `BtnAra.PerformClick()`. (Acilis varsayilani "Uretimde".)
- Grid cift tiklama/Enter (`MyView1_MyEventDoubleClickEnter`) — secim modunda satir secip kapatir; normal modda `FrmUretimEmriED` acar.
**Cagirdigi katmanlar:**
- Service: `IUretimEmriService.SelectListWhere(sor)` — filtreli uretim emri listesi.
- Service: `IUretimOperasyonService.SelectListWhere("where UrId=...")` — secili emrin operasyonlari (alt grid, `MyView1_FocusedRowChanged` ve ilk satirda).
- Service: `IGenelService.GrupListesi("UretimEmri","Turu"/"Durumu"/"ReceteAdi")` — combo kaynaklari.
- Manager: `SiparisManager.GetSiparis(itm.Id)` — Yazdir oncesi siparis modeli (yazdirmada dogrulama amacli cagrilir).
- Manager: `UretimEmriManager.UretimDurumGuncelle(id)` — `DurumGuncelle` metodu (bu formda buton bagli degil; `exec Uretim_DurumGuncelle` calistirir).
- SQL: filtre WHERE'leri string ile uretilir (SiparisKodu/SiparisCariKodu/SiparisCariUnvani LIKE, Turu/Durumu/ReceteAdi, Baslangic/Bitis tarih araliklari).
**Istasyon sirasiyla iliskisi:** Dogrudan yok; sadece listeleme. Operasyon Sira mantigi ED ve takip formlarinda devreye girer.
**Notlar:** `CmbDurumu` ve `myLabel7` (Durumu) Designer'da `Visible=false` — durum filtresi artik sekme butonlariyla yonetiliyor. `BtnDizayn` gizli. Alt operasyon gridinin grid yerlesim adi "UretimEmriListesi".

---

### Uretim Emri Acilis/Duzenleme (`FrmUretimEmriED.cs` / `.Designer.cs`)
**Ne ise yarar:** Tek bir uretim emrinin (is emrinin) olusturuldugu/duzenlendigi ana ekran. Siparis veya receteden operasyonlari, sarf stoklarini ve baslatilmis operasyon hareketlerini (3 sekmeli grid: Operasyonlar / Stoklar / Istasyon-Hareketler) yonetir. "Uretimi Kaydet ve Baslat" ile uretim akisini fiilen baslatir. Istasyon gruplama secimini de burada yapar.
**Once ne olmali (onkosul):** `IdGuid` (mevcut emir) veya `SipId` set edilmis olmali; ikisi de bossa form acilisinda `FrmSiparisListesi` secim modunda acilir ve bir siparis secilmek ZORUNDADIR (secilmezse form kapanir). Secilen siparise zaten bir UretimEmri varsa edit, yoksa yeni acilis yapilir. Yeni acilista operasyonlar siparis hareketleri x recete operasyonlarindan otomatik uretilir (`OperasyonlarOlustur`).
**Sonra ne olur:**
- **Kaydet**: `UretimEmriManager.UretimEmriKaydetBySiparis` tek transaction'da `UretimEmri` (InsertOrUpdate), eski `UretimOperasyon` ve `UretimStok` kayitlarini siler, yenilerini yazar, ardindan `exec Uretim_MiktarGuncelle` calistirir. Kayit sonrasi `ActionAktar` (liste yenileme) tetiklenir ve edit'e gecilir.
- **Uretimi Kaydet ve Baslat**: once `Kaydet`, sonra `UretimBaslat()` Sira<=1 operasyonlar icin `UretimOperasyonHareket` olusturup `UretimOperasyonHareketKaydet` ile yazar — bu da `exec Uretim_MiktarGuncelle` + `exec Uretim_PlanlananGuncelle` (sonuncu icinde `Uretim_SonrakiIstasyonaGonder`) calistirir. Form kapanir.
- **Sil**: baslatilmis hareket varsa engellenir; yoksa `UretimEmriManager.UretimEmriSil` ana+operasyon+stok siler, `exec Uretim_MiktarGuncelle` ve `Siparis.Durumu='YeniKayit'` yapar.
- **Baslatilmis Uretimleri Sil / Uretime Ait Tum Kayitlari Sil**: ilgili hareket/istasyon/takip tablolarini temizler.
- Cikis (Frm_FormClosing): yeni kayit edilip uretim baslatilmamissa kapanis engellenir / sorulur.
**Butonlar & kisayollar:**
- `BtnKaydet` — `Kaydet()` (base buton; Text Designer'da yok).
- `BtnSil` — `Sil()`.
- `BtnUretimeBasla` ("Üretimi Kaydet \r\nve Başlat") — `KaydetBaslat()` -> kayit + `UretimBaslat()`.
- `BtnUretimiSil` ("Başlatılmış \r\nÜretimleri Sil") — `OperasyonHareketSil()` (her hareket icin once `UretimOperasyonHareketKayitVarmi` kontrolu).
- `BtnUretimeAitTumKayitlariSil` ("Üretime Ait \r\nTüm Kayıtlari Sil") — `OperasyonaAitTumKayitlariSil()` -> `UretimeBagliTumHareketleriSil`.
- `BtnCariTemizle` — cari kodu/unvani textlerini temizler.
- `TxtSiparisKodu` (button edit) — Frm_Load'da `TxtIsEmriNo.ButtonClick` bagli (siparis kodu button click ayrica `BaglaSiparisden` ile yonetilir).
- `TxtIsEmriNo` (button edit) `ButtonClick` — `EvrakNoAl()` ile `GenelService.GetEvrakNo("UretimEmri")` cagirir (varsa once degistirme onayi sorar).
- `TxtIstasyonGrubu` (button edit) `ButtonClick` — `FrmReceteIstasyonGrupIstasyonEslestir` secim modunda acar, secileni `TxtIstasyonGrubu`'na yazar.
- `TxtCariKodu` (button edit) `ButtonClick` — `FrmMikroCariListesi` secim modunda acar, MikroCari secip kod+unvani doldurur.
- `ChcKapandi` — emrin kapandi durumu (kaydedilirken modele aktarilir).
- Grid satir stili (`GridView_RowStyle`) — Durumu "Hazir" yesil, "Uretimde" mor renklenir.
**Cagirdigi katmanlar:**
- Manager: `UretimEmriManager.GetUretimSiparisNew(sipId)` — yeni acilis modeli (siparis hareketleri, recete/operasyon modelleri, sarf stoklari fire yuzdesiyle hesaplanir).
- Manager: `UretimEmriManager.GetUretimSiparisEdit(urId)` — mevcut emrin tum alt verilerini (operasyon, hareket view'i, stok, recete/operasyon modelleri) yukler.
- Manager: `UretimEmriManager.UretimEmriKaydetBySiparis(mdl, yenikayit)` — transaction'li kayit + `Uretim_MiktarGuncelle`.
- Manager: `UretimEmriManager.UretimOperasyonHareketKaydet(list)` — Sira<=1 hareketleri yazar + `Uretim_MiktarGuncelle` + `Uretim_PlanlananGuncelle`.
- Manager: `UretimEmriManager.UretimEmriSil / UretimOperasyonHareketSil / UretimOperasyonHareketKayitVarmi / UretimeBagliTumHareketleriSil`.
- Service: `IReceteAnaService.SelectFind(id)` — istasyon gruplama kullanan recete kontrolu (`TextLeriKontrolEt`: gruplama varsa tek recete + istasyon grubu zorunlu).
- Service: `IGenelService.GetEvrakNo("UretimEmri")` — is emri no.
- SQL/Prosedur: `Uretim_MiktarGuncelle` (kayit/sil/hareket sonrasi), `Uretim_PlanlananGuncelle` (baslat sonrasi, icinde `Uretim_SonrakiIstasyonaGonder`).
**Istasyon sirasiyla iliskisi:** Cekirdek. Operasyonlar `Sira` ile uretilir; baslatma yalnizca Sira<=1 operasyonlar icin hareket acar. `Uretim_PlanlananGuncelle` Sira N uretimini Sira N+1 planlananina tasir. `IstasyonGruplamaKullan=1` recetelerde istasyon grubu (TxtIstasyonGrubu) secilirse `Uretim_SonrakiIstasyonaGonder` her operasyona TEK `UretimIstasyon` olusturur.
**Notlar:** `BtnUretimeBasla/BtnUretimiSil/BtnUretimeAitTumKayitlariSil` click event'leri Designer'da bagli (kod icinde tekrar baglama yorum satirina alinmis — cift tetiklemeyi onlemek icin). Uyari etiketi: "Istasyon Gruplama Tek Ürün Üretim Emrinde Kullanılabilir". UretimTuru combosu: Siparis / Recete / MikroSiparis.

---

### Operasyon Takip (Kart/Kanban) (`FrmOperasyonTakip.cs` / `.Designer.cs`)
**Ne ise yarar:** Tum aktif uretimi operasyon kodu bazinda canli kart (kanban) gorunumunde gosterir. Her operasyon karti (`OperasyonCardControlV2`) o operasyonun Bekleyen ve Uretimdeki is satirlarini ozetler. 60 saniyede bir otomatik yenilenir. Saha/yonetim icin "neyin nerede oldugu" panosu.
**Once ne olmali (onkosul):** En az bir uretim emri baslatilmis (UretimOperasyonHareket kaydi olusmus), durumu "Hazir" olmayan, kapanmamis (Ur.Kapandi=0) ve KalanMiktar>0 olan operasyonlar bulunmali — aksi halde kart cikmaz.
**Sonra ne olur:** Bu form veri degistirmez. Bir karttan Bekleyen detayina tiklayinca `Ortak.UretimTakipBekleyenAcV2(operasyon)` -> `FrmOperasyonTakipDetaylar` (Durumu="Beklemede"); Uretimde detayina tiklayinca `Ortak.UretimTakipUretimdeAcV2(operasyon, Action)` -> `FrmOperasyonTakipDetaylar` (Durumu="Uretimde"). `BtnTumOperasyonlar` ise `FrmOperasyonTakipDetaylar`'i `TumunuBagla=true` ile acar.
**Butonlar & kisayollar:**
- `BtnYenile` ("Yenile") — `Bagla()` ile takip listesini yeniden ceker.
- `BtnTumOperasyonlar` ("Tum\r\nOperasyonlar") — `FrmOperasyonTakipDetaylar {TumunuBagla=true}` acar.
- `TimerYenile` (60000 ms) — `BtnYenile.PerformClick()` ile otomatik yenileme.
- (Kart icindeki tiklamalar `OperasyonCardControlV2` icinde; detay acma yukaridaki Ortak metotlariyla.)
**Cagirdigi katmanlar:**
- Manager: `UretimTakipManagerV2.GetTakipList()` — operasyon kodu + recete + siparis bazinda Planlanan/Uretim/Islemdeki/Fire/Iptal/Kalan miktarlarini SUM'layan sorgu (UretimOperasyonHareket -> UretimOperasyon -> UretimEmri -> Siparis JOIN; `Durumu<>'Hazir' AND Kapandi=0 AND KalanMiktar>0`).
- UI: `OperasyonCardControlV2` — her operasyon icin kart; satirlari Bekleyen (Planlanan-Islemdeki>0) ve Uretimde (Islemdeki>0) olarak ayirir.
- Ortak: `UretimTakipBekleyenAcV2 / UretimTakipUretimdeAcV2` — detay formunu Durumu ile acar.
**Istasyon sirasiyla iliskisi:** Dolayli; kartlar operasyon (Sira) bazli toplam miktarlari yansitir, ancak bu ekran Sira gecislerini tetiklemez.
**Notlar:** Designer'da 4 ornek kart tasarim zamani placeholder; calismada `PnlOrta.Controls.Clear()` sonrasi operasyon koduna gore dinamik kart eklenir. `BtnKaydet` gizli (bu salt-izleme formu).

---

### Operasyon Takip Detaylar (`FrmOperasyonTakipDetaylar.cs` / `.Designer.cs`)
**Ne ise yarar:** Secilen operasyonun (veya tum operasyonlarin) is satirlarini grid olarak listeler ve uretim girisi / istasyona gonderme / detay hareket goruntuleme islemlerine kopru kurar. Durumu="Uretimde" iken uretim girisi, Durumu="Beklemede" iken istasyona gonderme akisi acilir.
**Once ne olmali (onkosul):** `FrmOperasyonTakip` (veya baska bir ekran) tarafindan `Operasyon` + `Durumu` set edilerek acilmali; ya da `TumunuBagla=true` ile tum operasyonlar getirilir; ya da `SipIdDenBagla=true` + `SipId` ile siparise filtrelenir. Satirin `Id`'si bos olmamali (operasyon baslatilmamis satirda islem yapilamaz — "bir onceki operasyondan islem yapiniz" uyarisi).
**Sonra ne olur:**
- Durumu="Uretimde" + Uretim Girisi: once `FrmUretimIstasyonHareketSec` (secim modu, UrOHDId=secili Id) ile istasyon hareketi secilir, sonra `FrmUretimIstasyonUretimGir` acilir; orada uretim girilince istasyon/operasyon hareket detay tablolari ve (prosedurler araciligiyla) UretimOperasyon/UretimEmri/Siparis miktar+durumlari guncellenir.
- Durumu="Beklemede" + Istasyona Gonder: `FrmUretimIstasyonED` (OperasyonTuru=IstasyonEkle, OprId=secili Id) acilir; islem sonrasi `Action` (kart yenileme) tetiklenir.
- Detay Hareketler: `FrmUretimOperasyonHareketDetayList` (DetayGoster=true, DetayId=secili Id) acilir.
- Her islem sonrasi `this.Close()` ile detay formu kapanir.
**Butonlar & kisayollar:**
- `BtnUretimGirisi` — Designer Text'i "Üretim /İstasyon"; Frm_Load'da Durumu="Uretimde" iken "Uretim Giris", "Beklemede" iken "Istasyona Gonder" olarak degisir. `BtnUretimGirisi_Click` Durumu'ya gore uretim girisi veya istasyona gonderme akisini calistirir.
- Sag tik (context menu, `ContexMenuyeEkle`) ogeleri:
  - "Üretim Girişi" (Durumu="Uretimde") — `UretimGirisi` (hareket sec -> uretim gir).
  - "Istasyona Gonder" (Durumu="Beklemede") — `UretimeGonder_Operasyon` (`FrmUretimIstasyonED` IstasyonEkle).
  - "Detay Hareketler" (her durumda) — `Detaylar` (`FrmUretimOperasyonHareketDetayList`).
- Grid cift tik/Enter (`MyView1_MyEventDoubleClickEnter`) — secili satiri alir (govde bos; islem context menu/buton ile).
**Cagirdigi katmanlar:**
- Manager: `UretimTakipManagerV2.GetTakipDetayList(operasyon, durumu, sipid)` — Durumu="Uretimde" iken `UretimOperasyonHareketDetay` (Islemdeki>0 & Kalan>0), aksi halde `UretimOperasyonHareket` (Kalan>0) satirlarini Siparis/Operasyon ile JOIN'leyip getirir.
- Manager: `UretimTakipManagerV2.GetTakipDetayListAll()` — `TumunuBagla` icin KalanMiktar>0 olan tum operasyonlarin hareketleri.
- UI kopru formlari: `FrmUretimIstasyonHareketSec`, `FrmUretimIstasyonUretimGir`, `FrmUretimIstasyonED`, `FrmUretimOperasyonHareketDetayList` (asil uretim girisi/istasyon islemleri bu formlarda yapilir; miktar guncellemeleri `Uretim_MiktarGuncelle`/`Uretim_PlanlananGuncelle` araciligiyla olur).
**Istasyon sirasiyla iliskisi:** Dogrudan. "Beklemede" satir, bir onceki Sira operasyonu uretim yapilmadan baslatilmamis olabilir; bu yuzden `Id` bos satirda islem engellenir ve onceki operasyondan ilerlemesi istenir. Uretim girisi/istasyona gonderme, Sira N+1 operasyonunun planlanan miktarini besler.
**Notlar:** Grid yerlesim adi "ReceteEkleDetaylar" (Designer kalintisi). Form basligi acilista "Üretim Takip Detay = Durumu : ... - Operasyon : ..." olarak set edilir.
