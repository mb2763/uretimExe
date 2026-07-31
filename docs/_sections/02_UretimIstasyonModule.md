## Modul: UretimIstasyonModule

Bu modul, uretim emirlerinin OPERASYON-istasyon kirilimindaki saha takibini ve uretim/fire miktari girisini saglar. Bir uretim emrinin her operasyonu altinda bir veya birden cok istasyon (UretimIstasyon) bulunur; bu modul (a) hangi istasyonda ne kadar planlandi/uretildi/fire verildi seklinde gorsel takip kartlari (FrmUretimIstasyonTakip + IstasyonCardControl), (b) istasyon ve istasyon-hareket listeleri/filtreleri (FrmUretimIstasyonHareketList, FrmUretimIstasyonHareketDetayList), (c) tek bir istasyon hareketine uretim/fire miktari girme ekrani (FrmUretimIstasyonUretimGir) ve (d) bir operasyona istasyon ekleme/degistirme/sonraki operasyona gecirme ekrani (FrmUretimIstasyonED + OperasyonIstasyonControlV2) sunar. Miktar/akis motoru tamamen OPERASYON-Sira bazli SQL prosedurleriyle (Uretim_MiktarGuncelle -> Uretim_PlanlananGuncelle -> Uretim_SonrakiIstasyonaGonder) yurur; bu modulun kayit ekranlari bu prosedurleri tetikler. Modul ofis/yonetim WinForms uygulamasidir (CepPatronERP.exe), DB'ye dogrudan baglanir (Ortak.DbPro = UretimV3_FEZA, Ortak.DbMikro = MikroDB_V16_FEZA24).

### Istasyon Takip (`FrmUretimIstasyonTakip.cs` / `.Designer.cs`)
**Ne ise yarar:** Acik (UrO.KalanMiktar > 0) tum istasyonlarin ozet kartlarini gosterir. Her istasyon kodu icin tek bir kart (IstasyonCardControl) ciker; kartta istasyon kodu/adi, operasyon, fason durumu, planlanan/uretim/fire miktarlari toplanmis olarak yer alir. Saha/yonetim icin "hangi istasyonda ne durumda" panosudur.
**Once ne olmali (onkosul):** Uretim emri ve operasyonlari olusturulmus, ilgili operasyona en az bir istasyon atanmis (UretimIstasyon kaydi var) ve operasyonun KalanMiktar > 0 olmali. Aksi halde kartlar yerine bos placeholder kartlar gosterilir.
**Sonra ne olur:** Bu ekran salt okunur ozet panosudur; veritabanini degistirmez. Bir kartin buyutec ikonuna tiklayinca o istasyonun detayi `FrmUretimIstasyonTakipDetay` ekranina gecer (`Ortak.IstasyonKontrolDetayAc`).
**Butonlar & kisayollar:**
- `Yenile` (BtnYenile) — Listeyi yeniden ceker (`Bagla()` -> `GetTakipList()`).
- Kart buyutec ikonu (IstasyonCardControl.pictureBox2) — Secili istasyonun detayini acar (kisayol yok, mouse click).
- Not: Designer'da BtnKaydet Visible=false (bu ekranda kayit yok).
**Cagirdigi katmanlar:**
- Manager/Service: `UretimIstasyonTakipManager.GetTakipList()` — UretimIstasyon (UrI) + UretimOperasyon (UrO) JOIN, `UrO.KalanMiktar > 0` filtresi; istasyon kodu/operasyon/fason/cari bazinda PlanlananMiktar/UretimMiktari/FireMiktari/IptalMiktari SUM'lar. (IGenelService.Query<UretimIstasyonTakipModel> ile ham SQL).
- SQL/Prosedur: Bu ekranda prosedur tetiklenmez (sadece SELECT).
- API: -
**Istasyon sirasiyla iliskisi:** Kartlar operasyon Sira bilgisiyle gruplanmaz; istasyon kodu bazinda gruplanir. Hangi operasyonda olduklari kartta OperasyonKodu-OperasyonAdi olarak gosterilir.
**Notlar:** Frm_Load'da `_mng = new UretimIstasyonTakipManager(Ortak.DbPro)`. Kart sayisi 9'a tamamlanir (bos kartlar eklenir). `FromAc.IstasyonTakip()` ile MDI child olarak acilir. Manager'da kullanilmayan varyantlar: `GetTakipListUretimKodlu()` (IsEmri/Siparis kodu kirilimli).

### Istasyon Card Control (`IstasyonCardControl.cs` / `.Designer.cs`)
**Ne ise yarar:** FrmUretimIstasyonTakip icindeki tek bir istasyon kartini render eden UserControl. UretimIstasyonTakipModel verisini etiketlere basar (istasyon kodu/adi, operasyon, fason, planlanan/uretim/fire miktari, fason cari kodu/unvani).
**Once ne olmali (onkosul):** `Model` property'si bir UretimIstasyonTakipModel ile set edilmis olmali; null ise tum etiketler gizlenir (bos placeholder kart).
**Sonra ne olur:** Salt gosterim. Buyutec ikonuna tiklayinca `Ortak.IstasyonKontrolDetayAc(IstasyonKodu, Fason)` cagrilir -> `FrmUretimIstasyonTakipDetay` acilir.
**Butonlar & kisayollar:**
- Buyutec ikonu (pictureBox2) — Click: detay ekranini acar; MouseHover: arka plan Maroon; MouseLeave: transparent.
**Cagirdigi katmanlar:**
- Manager/Service: - (veri disaridan Model ile gelir).
- SQL/Prosedur: -
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Namespace `MyUI.MyControl` (dosya UretimIstasyonModule klasorunde). Fason etiketi true->"Evet", false->"Hayir".

### Istasyon Takip Detay (`FrmUretimIstasyonTakipDetay.cs` / `.Designer.cs`)
**Ne ise yarar:** Tek bir istasyon kodu (ve fason ayrimi) icindeki tum acik istasyon hareketlerini grid olarak listeler. Siparis/IsEmri kodu, operasyon, recete, planlanan/uretim/fire/iptal miktarlari ve baslangic tarihiyle satir bazinda detay verir.
**Once ne olmali (onkosul):** `IstasyonKodu` ve `Fason` (0/1) property'leri set edilmis olmali (FrmUretimIstasyonTakip kartindan `Ortak.IstasyonKontrolDetayAc` ile gelir).
**Sonra ne olur:** Salt okunur grid; veritabanini degistirmez.
**Butonlar & kisayollar:**
- MyFrmSadeFull alt bar butonlari (Yazdir/Kopyala/Kapat) - bu formda ozel buton/handler yok; sadece grid gosterimi.
**Cagirdigi katmanlar:**
- Manager/Service: `UretimIstasyonTakipManager.GetTakipDetayList(IstasyonKodu, Fason)` — UrI + UrO + Siparis JOIN, `UrO.KalanMiktar > 0 AND UrI.IstasyonKodu = @kodu AND Coalesce(UrI.Fason,0) = @fason` filtreli detay SELECT.
- SQL/Prosedur: Sadece SELECT.
- API: -
**Istasyon sirasiyla iliskisi:** Detayda her satir bir UretimIstasyon hareketidir; operasyon/recete bilgisi gosterilir.
**Notlar:** BaslangicTarihi sutunu "dd.MM.yyyy HH:mm" formatlanir. Form basligi "IstasyonKodu : <kod>" olur. `SutunGizle()` bos.

### Istasyon Hareket Listesi (`FrmUretimIstasyonHareketList.cs` / `.Designer.cs`)
**Ne ise yarar:** Tum UretimIstasyon (UrI) kayitlarini ust grid'de, secili istasyonun istasyon-hareketlerini (UretimIstasyonHareket) alt grid'de gosteren master-detail liste. Istasyon kodu, recete adi ve tarih araliklarina gore filtrelenir. Ayrica baska ekranlardan "istasyon secimi" icin secim modunda (SecimIcinAcildi) acilir.
**Once ne olmali (onkosul):** UretimIstasyon kayitlari olusmus olmali. Secim modunda (FrmUretimIstasyonUretimGir'den) calismasi icin SecimIcinAcildi=true set edilir.
**Sonra ne olur:** Liste modunda salt okunur (satira cift tiklayinca eski FrmUretimEmriED_V2 kodu commentli, islem yok). Secim modunda secilen UretimIstasyon `SecilenRow` olarak doner ve form kapanir.
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — Filtreleri uygular (`Bagla()`).
- `Temizle` (BtnTemizle) — Filtre alanlarini sifirlar (CmbIstasyonKodu, CmbReceteAdi, 4 tarih).
- Grid cift tik / Enter (myView1.MyEventDoubleClickEnter) — Secim modunda secip kapatir.
- Filtreler: CmbIstasyonKodu, CmbReceteAdi, TxtTarihi1-4 (baslangic >= / <= ve bitis >= / <= tarih araliklari).
**Cagirdigi katmanlar:**
- Manager/Service: `IUretimIstasyonService.GetViewListWhere(where)` (Ortak.DbPro.UretimIstasyon) — UrI view'i (UrO ile JOIN'li) where ile ceker. `IUretimIstasyonHareketService.GetViewListWhere(where)` — secili istasyonun UrIId'sine gore hareketler. `IGenelService.GrupListesi("UretimIstasyon","IstasyonKodu")` ve `("UretimEmri","ReceteAdi")` — filtre combobox'larini doldurur.
- SQL/Prosedur: Sadece SELECT (where 1=1 + dinamik tarih/kod filtreleri).
- API: -
**Istasyon sirasiyla iliskisi:** Master grid UrI, detail grid o istasyonun hareketleri; operasyon Sira ile dogrudan kullanim yok, tarih bazli filtre var.
**Notlar:** Frm_Load'da TxtTarihi1 = bugun-1 ay (varsayilan). SQL string birlestirme ile kurulur (parametresiz). `Frm_Load` icinde BtnTemizle.Click iki kez baglanir (kod tekrari). MyFrmListe turevi.

### Istasyon Hareket Detay Listesi (`FrmUretimIstasyonHareketDetayList.cs` / `.Designer.cs`)
**Ne ise yarar:** Tum UretimIstasyonHareket (uretim/fire giris) kayitlarini tek bir grid'de, istasyon kodu / recete adi / tarih araligina gore listeler. Bu hareketlere yeni kayit eklemenin ve mevcut kaydi acmanin giris noktasidir.
**Once ne olmali (onkosul):** En az bir UretimIstasyon hareketi (uretim girisi) yapilmis olmali; ekleme icin once bir istasyon secilebilir durumda olmali.
**Sonra ne olur:** Satira cift tik/Enter veya Ekle ile `FrmUretimIstasyonUretimGir` acilir; orada kaydet/sil yapilinca `Action=Bagla` ile bu liste yenilenir. UretimGir kaydi `Uretim_MiktarGuncelle` + `Uretim_PlanlananGuncelle` prosedurlerini tetikler (asagidaki ekrana bakiniz).
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — Filtreleri uygular (`Bagla()`).
- `Temizle` (BtnTemizle) — Filtre alanlarini sifirlar.
- `Ekle` (BtnEkle) — Bos `FrmUretimIstasyonUretimGir` acar (yeni uretim girisi).
- Grid cift tik / Enter — Secili UretimIstasyonHareket'i `FrmUretimIstasyonUretimGir` ile duzenlemeye acar (IdGuid = itm.Id).
- Filtreler: CmbIstasyonKodu, CmbReceteAdi, TxtTarihi1/TxtTarihi2 (hareket tarihi UrIH.Tarih araligi).
**Cagirdigi katmanlar:**
- Manager/Service: `IUretimIstasyonHareketService.GetViewListWhere(where)` (Ortak.DbPro.UretimIstasyonHareket) — UrIH view'i (UrI/UrO JOIN'li). `IGenelService.GrupListesi(...)` ile filtre listeleri.
- SQL/Prosedur: Sadece SELECT (kayit isi UretimGir ekraninda).
- API: -
**Istasyon sirasiyla iliskisi:** Hareketler istasyon/operasyon bilgisini tasir; Sira ile dogrudan filtre yok.
**Notlar:** Frm_Load'da TxtTarihi1 = bugun-1 ay. MyFrmListe turevi.

### Istasyon Uretim Kayit Gir (`FrmUretimIstasyonUretimGir.cs` / `.Designer.cs`)
**Ne ise yarar:** Tek bir istasyon hareketine (UretimIstasyon) uretim miktari ve fire miktari girisini yapan ana kayit ekranidir. Ust kisimda istasyon/operasyon/recete/planlanan/mevcut uretim bilgileri (readonly), alt kisimda uretim miktari, fire miktari, tarih, personel kodu/adi girilir. Saha uretim girisinin masaustu karsiligidir.
**Once ne olmali (onkosul):** Bir UretimIstasyon kaydi secilmis olmali. IdGuid bos ve MdlIst null ise ekran acilisinda `FrmUretimIstasyonHareketList` secim modunda acilir ve kullanici bir istasyon secer (secmezse form kapanir). IdGuid dolu ise mevcut hareket duzenlemeye acilir.
**Sonra ne olur:** Kaydet -> `UretimIstasyonManager.UretimIstasyonHareketKaydet(_mdl)` -> UretimIstasyonHareket tablosuna Insert/Update, ardindan `exec Uretim_MiktarGuncelle <UrId>` ve `exec Uretim_PlanlananGuncelle <UrId>` calisir (miktarlar yukari toplanir ve operasyon Sira N -> N+1 planlamasi guncellenir). Basarili olursa Action?.Invoke() ile cagiran liste yenilenir ve form kapanir. Sil benzer sekilde Delete + ayni iki prosedur.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — `Kaydet()`: dogrulama (uretim miktari >= 1), miktar kontrolu (planlanani asamaz), kayit + prosedur tetikleme.
- `Sil` (BtnSil) — Onay sorar, `Sil()` ile kaydi siler + prosedurleri tetikler.
- `Kapat` (BtnKapat) — Formu kapatir (MyFrmKayit alt bar).
- Personel alanlari: CmbPersonelKodu / CmbPersonelAdi (LookupEdit) — biri secilince digeri otomatik dolar (Leave event).
**Cagirdigi katmanlar:**
- Manager/Service: `UretimIstasyonManager.UretimIstasyonHareketKaydet(UretimIstasyonHareket)` — InsertOrUpdate + iki prosedur. `UretimIstasyonManager.UretimIstasyonHareketSil(...)` — Delete + iki prosedur. `UretimIstasyonManager.GetUretimIstasyonHareket(id)` / `GetUretimIstasyon(id)` — duzenleme verisini ceker. `IPersonelService.SelectListWhere()` — personel lookup. `IGenelService` (genel).
- SQL/Prosedur: `Uretim_MiktarGuncelle <UrId>` — UretimIstasyonHareket->UretimIstasyon->UrOHD->UrOH->UrO miktar toplamlarini yukari yazar. `Uretim_PlanlananGuncelle <UrId>` — operasyon Sira N uretimini N+1 planlanana tasir; sonunda `Uretim_SonrakiIstasyonaGonder` cagrilabilir.
- API: -
**Istasyon sirasiyla iliskisi:** Bu girisin sonucu, Uretim_PlanlananGuncelle/SonrakiIstasyonaGonder araciligiyla operasyon Sira'sina gore bir sonraki operasyonun istasyonlarinin planlananina yansir. (SonrakiIstasyonaGonder yalnizca ReceteAna.IstasyonGruplamaKullan=1 iken ReceteIstasyonGrupOperasyon eslemesiyle calisir.)
**Notlar:** Miktar kontrolu: yeni kayitta uretim miktari (PlanlananMiktar - UretimMiktari)'ni asamaz; duzenlemede _editMiktar (onceki deger) hesaba katilir. Kayit yeni mi kontrolu icin Audit alanlari (KayitEden/Degistiren) atanir (mantik ters gibi gorunse de mevcut kod boyle). `FromAc` icinde parametresiz UretimGir acan bir launcher da var.

### Uretim Istasyon Kayit / ED (`FrmUretimIstasyonED.cs` / `.Designer.cs`)
**Ne ise yarar:** Bir operasyon hareketine (UretimOperasyonHareket) bagli istasyonlari TANIMLAMA/DEGISTIRME ve "sonraki operasyona gecirme" ekranidir. Operasyonun bekleyen miktarini istasyonlara planlanan miktar olarak dagitir, fason istasyonlar icin cari secer, istasyon ekler/siler. Ucuncu sekme niteliginde uc mod ile calisir: IstasyonEkle, SonrakiOperasyon, Degistir (OperasyonTuruEnum).
**Once ne olmali (onkosul):** Gecerli bir UretimOperasyonHareket (OprId) olmali. Acilista `UretimEmriManager.GetUretimOperasyonSiparisEdit(OprId)` ile siparis/operasyon modeli (UretimEmriKayitModelSiparis) yuklenir ve `IUretimOperasyonHareketService.GetViewListWhere(UrOH.Id=OprId)` ile Hareket bulunur. Degistir modunda IdGuid (UrOHD) gerekir; SonrakiOperasyon modunda bir onceki operasyonun bilgisinden Sira+1 operasyonu bulunur.
**Sonra ne olur:** Kaydet -> mod'a gore `UretimOperasyonManager.KaydetNormal(...)` veya `KaydetSonraki(...)`. KaydetNormal: UretimOperasyonHareketDetay Insert/Update + o detaya bagli UretimIstasyon kayitlarini sil/yeniden ekle + silinen istasyonlarin IstasyonTakipHareket/Detay/Log/StokHareket kayitlarini temizle + `Uretim_MiktarGuncelle` + `Uretim_PlanlananGuncelle` (transaction icinde). KaydetSonraki: yeni UretimOperasyonHareket Insert + Detay + istasyonlari yeni harekete bagla + eski istasyonlari sil + `Uretim_MiktarGuncelle`. Sonrasinda Action?.Invoke() ile cagiran ekran yenilenir, form kapanir.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — `Kaydet()`: istasyonlari toplar (IstasyonlarAl), planlanan/uretim/fason cari dogrulamalari, miktar tavan kontrolu, mod'a gore KaydetNormal/KaydetSonraki.
- OperasyonIstasyonControlV2 ic butonlari (asagidaki bilesen): Fason Sec / Fason Temizle / Istasyon Ekle / Istasyon Sil.
- MyFrmKayitFull alt bar: Kapat/Yazdir/navigasyon (Ilk/Onceki/Sonraki/Son) - bu formda nav butonlari pasif gorunum.
**Cagirdigi katmanlar:**
- Manager/Service: `UretimOperasyonManager.KaydetNormal(ist, Detay, silinenler)` / `KaydetSonraki(ist, Detay, HareketYeni)` — transaction'li kayit + prosedur. `UretimTakipManagerV2.GetSiparisKalanKontrol(UrId)` — sonraki operasyon planlanan miktari hesabi icin. `UretimEmriManager.GetUretimOperasyonSiparisEdit(OprId)` — model. `IUretimIstasyonService`, `IUretimOperasyonHareketService`, `IUretimOperasyonHareketDetayService`, `IUretimOperasyonService` (SelectFirst/GetViewListWhere). Istasyon secimi icin `FrmIstasyonKartList` (BagliIstasyonSec, RcAId).
- SQL/Prosedur: `Uretim_MiktarGuncelle <UrId>`, `Uretim_PlanlananGuncelle <UrId>` (KaydetNormal); `Uretim_MiktarGuncelle <UrId>` (KaydetSonraki). Silinen istasyonlarda IstasyonTakipHareket / IstasyonTakipHareketDetay / IstasyonTakipHareketLog / IstasyonTakipStokHareket Delete.
- API: -
**Istasyon sirasiyla iliskisi:** Cekirdek ekran. IstasyonlarOlustur, recete modelindeki operasyon (RcOId) ve ona bagli istasyonlari (RcIstId) eslestirerek operasyon icin istasyon hareketleri uretir. SonrakiOperasyon modu, OncekiOperasyon.Sira+1 operasyonu bulup once uretileni-operasyondaolani fark olarak yeni operasyonun PlanlananMiktar'ina yazar (operasyon Sira mantiginin UI tarafindaki yansimasi).
**Notlar:** IstasyonlarAl: IstDurumu dolu (islem gormus) istasyonda planlanan miktar degistirilemez; planlanan < uretim/fire olamaz. Fason istasyonda FasonCariKodu zorunlu. Kaydedilecek istasyon yoksa onay sorar (uretim beklemeye alinir). MyFrmKayitFull turevi; SizeChanged ile ic control yeniden boyutlanir.

### Operasyon Istasyon Control V2 (`OperasyonIstasyonControlV2.cs` / `.Designer.cs`)
**Ne ise yarar:** FrmUretimIstasyonED icinde calisan, bir operasyona ait istasyon hareketlerini duzenlenebilir grid ile gosteren UserControl. Ustte recete kodu/adi, operasyon kodu, miktar; gridde her istasyonun PlanlananMiktar, Fason, Baslangic/Bitis tarihi, FasonCariKodu/Unvani duzenlenir.
**Once ne olmali (onkosul):** FrmUretimIstasyonED tarafindan property'leri (RcOId, ReceteKodu, ReceteAdi, Operasyon, Miktar, oprKayMdl, IstasyonHareketler) set edilip `Bagla()` cagrilmis olmali.
**Sonra ne olur:** Grid uzerindeki degisiklikler IstasyonHareketler listesine yansir; FrmUretimIstasyonED.Kaydet bunlari okur. Fason Sec ile secilen Mikro cari satira yazilir.
**Butonlar & kisayollar:**
- `Fason Sec` (BtnCariEkle) — `FrmMikroCariListesi` secim modunda acar; secilen cariyi satira yazar, Fason=true yapar.
- `Fason Temizle` (BtnCariTemizle) — Satirin fason cari bilgisini temizler, Fason=false.
- `Istasyon Ekle` (BtnIstasyonEkle) — MyIstasyonEkleEvent tetikler; FrmUretimIstasyonED bunu yakalayip `FrmIstasyonKartList` ile yeni istasyon ekler.
- `Istasyon Sil` (BtnIstasyonSil) — Uretim girilmis satir silinemez (uyari); aksi halde MyIstasyonSilEvent tetikler, FrmUretimIstasyonED satiri listeden cikarir.
**Cagirdigi katmanlar:**
- Manager/Service: Dogrudan yok; veri/eventler FrmUretimIstasyonED uzerinden yonetilir. Fason cari secimi icin `FrmMikroCariListesi` (Mikro DB).
- SQL/Prosedur: -
- API: -
**Istasyon sirasiyla iliskisi:** Grid satirlari bir operasyonun istasyonlaridir; eklenen istasyon o operasyona (RcOId) baglanir.
**Notlar:** Namespace `MyUI.UretimOperasyonModule` (dosya UretimIstasyonModule klasorunde). PlanlananMiktar/Fason/Baslangic/Bitis kolonlari yesil baslikli ve editable; FasonCari kolonlari buton edit (ColBtnCariKodu/ColBtnCariUnvani).

### Uretim Istasyon Cari Sec (`FrmUretimIstasyonCariSec.cs` / `.Designer.cs`)
**Ne ise yarar:** Bir istasyona tanimli recete istasyon carilerini (ReceteIstasyonCari) listeleyip secim yaptiran basit secim ekranidir.
**Once ne olmali (onkosul):** `Cariler` (List<ReceteIstasyonCari>) disaridan set edilmis olmali.
**Sonra ne olur:** Satira cift tik/Enter ile secilen cari `SecilenRow`/`SecilenKod` (CariKodu) olarak doner, form kapanir.
**Butonlar & kisayollar:**
- Grid cift tik / Enter (myView1.MyEventDoubleClickEnter) — Secip kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: - (veri Cariler property'siyle gelir).
- SQL/Prosedur: -
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** MyFrmSadeFull turevi. Id/RcAId/RcOId/RcIstId sutunlari gizli.

### Uretim Istasyon Hareket Sec (`FrmUretimIstasyonHareketSec.cs` / `.Designer.cs`)
**Ne ise yarar:** UretimIstasyon kayitlarini ya bir operasyon hareketi detayina (UrOHDId) ya da bir operasyon koduna (OperasyonKodu) gore listeleyip secim yaptiran secim ekranidir.
**Once ne olmali (onkosul):** `UrOHDId` (varsayilan mod) veya `OperasyondanBagla=true` + `OperasyonKodu` set edilmis olmali. Secim modu icin SecimIcinAcildi=true.
**Sonra ne olur:** Satir secilince (cift tik/Enter veya Kaydet) secilen UretimIstasyon `SecilenRow`, `SecilenKod`=IstasyonKodu olarak doner ve form kapanir.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — Secili satiri secip kapatir.
- Grid cift tik / Enter (myView1.MyEventDoubleClickEnter -> MyView1_MyEditAc) — Secip kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IUretimIstasyonService.GetViewListWhere(" where UrI.UrOHDId='...'")` (Bagla) veya `(" where UrO.OperasyonKodu='...'")` (BaglaOperasyondan). Ortak.DbMikro ve IGenelService referanslari tutulur ancak bu formda aktif kullanim sade.
- SQL/Prosedur: Sadece SELECT.
- API: -
**Istasyon sirasiyla iliskisi:** Operasyon koduna gore filtre secenegi ile operasyon-istasyon iliskisini gosterir.
**Notlar:** MyFrmSade turevi. RowIndicator'da satir numarasi gosterimi (gridView1_CustomDrawRowIndicator). IndicatorWidth=30.
