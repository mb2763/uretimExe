## Modul: UretimOperasyonModule

Bu modul, bir uretim emrinin OPERASYON katmanini (recetedeki operasyon adimlarini) izlemek/raporlamak icindir. Veri modeli uc seviyelidir: `UretimOperasyon` (operasyon adimi, Sira bazli) -> `UretimOperasyonHareket` (operasyonun siparis-hatti/parti bazli toplam miktarlari) -> `UretimOperasyonHareketDetay` (her uretim girisi/istasyon hareketi detayi). Modul cogunlukla READ-ONLY listeleme/raporlama yapar; yazma (miktar/akis) ise TabletV2 saha akisi ve `Uretim_MiktarGuncelle / Uretim_PlanlananGuncelle` prosedurleriyle gerceklesir. Modulun tek yazma noktasi `FrmUretimOperasyonList` icindeki "Durum Guncelle" sag-tik komutudur (`Uretim_DurumGuncelle` prosedurunu cagirir). Modul ayrica panodaki (FrmOperasyonTakip) operasyon kartlarini olusturan iki UserControl icerir: `OperasyonCardControlV2` ve `OperasyonCardRowControlV2`.

Tum Frm* formlari `My.Kontrol.Formlar.MyFrmListe` taban sinifindan turer. Taban sinif standart liste iskeletini saglar: sol "Ara" sekmesi (`tabAra1`), uc grid (`myGrid1/2/3`), alt buton seridi (`BtnAra`, `BtnTemizle`, `BtnKapat`, `BtnYazdir`, `BtnDizayn`) ve secim modu altyapisi (`SecimIcinAcildi`, `Secildi`, `SecilenRow`). Liste veri kaynagi her zaman entity'nin `GetSelectSqlCode(where)` ile uretilen view-SQL'idir; servisler sadece `GetViewListWhere(where)` cagirir (Dapper okuma). Tarih filtreleri `TxtTarihi1..4` ile (basl./bitis alt-ust) kurulur, kod filtresi serbest metindir; bu yuzden where-cumleleri string birlestirme ile olusur (parametreli degil).

---

### Operasyonlar Listesi (`FrmUretimOperasyonList.cs` / `FrmUretimOperasyonList.Designer.cs`)
**Ne ise yarar:** Uretim operasyon adimlarini (UretimOperasyon) ust grid'de listeler; secilen operasyonun hareketlerini (UretimOperasyonHareket) orta grid'de, secilen hareketin istasyon-detaylarini (UretimOperasyonHareketDetay) alt grid'de master-detail-detail olarak gosterir. Operasyon/recete/durum/tarih filtreleri ve durum-hizli-filtre butonlari (Hepsi/Beklemede/Uretimde/Hazir) icerir.
**Once ne olmali (onkosul):** Bir siparis/is emri icin uretim emri ve operasyonlari olusturulmus olmali (`UretimEmriManager` ile; `Uretim_PlanlananGuncelle` Sira=1 icin `UretimOperasyonHareket` satirlarini otomatik uretir). Siparisten gelindiyse `SiparisKodundanFiltrele=true` ve `SiparisKodu` doludur.
**Sonra ne olur:** Cogunlukla salt okuma. Tek yazma: ust grid satirinda sag-tik "Durum Guncelle" -> `Uretim_DurumGuncelle` prosedurunu calistirir; bu prosedur `UretimOperasyonHareket.KalanMiktar` ve `UretimOperasyon.KalanMiktar` alanlarini yeniden hesaplar, ardindan `UretimOperasyon.Durumu` (Beklemede/Uretimde/Hazir), `UretimEmri.Durumu` ve `Siparis.Durumu` alanlarini gunceller, ayrica miktari sifir kalan `UretimOperasyonHareketDetay` satirlarini siler. Alt grid'de bir detaya cift tiklayinca `FrmUretimIstasyonED` (Degistir modu) acilir.
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — filtrelerle yeniden sorgular (`Bagla()`); Frm_Load sonunda `BtnAra.PerformClick()` ile otomatik tetiklenir (sayfa acilir acilmaz dolar).
- `Temizle` (BtnTemizle) — CmbDurumu/CmbReceteAdi ve TxtTarihi1..4 alanlarini bosaltir.
- `Hepsi / Beklemede / Uretimde / Hazir` (BtnDurumuHepsi/Beklemede/Uretimde/Hazir) — durum hizli filtresi; `DurumuAyarla(...)` ile `durumu` degiskenini set edip ilgili butonu renklendirir ve `BtnAra.PerformClick()` yapar.
- `Sag-tik > Durum Guncelle` (ContextMenu ogesi "DurumGuncelle") — secili operasyonun `UrId`'si icin `UretimEmriManager.UretimDurumGuncelle()` cagirir, basariliysa "Guncellendi." mesaji verir.
- Ust grid cift-tik/Enter (`MyView1_MyEventDoubleClickEnter`) — secim modunda (`SecimIcinAcildi`) satiri secip formu kapatir; normal modda etkin degil (eski FrmUretimEmriED_V2 cagrisi yorum satiri).
- `Kapat / Yazdir / Dizayn` (BtnKapat/BtnYazdir/BtnDizayn) — MyFrmListe taban davranisi (kapatma, grid yazdirma, grid dizayn/yerlesim).
**Cagirdigi katmanlar:**
- Service: `IUretimOperasyonService.GetViewListWhere(where)` — `select UrO.* from UretimOperasyon UrO left outer join Siparis Sip on Sip.Id=UrO.SipId where ...` (ust grid).
- Service: `IUretimOperasyonHareketService.GetViewListWhere("where UrOId='...'")` — secili operasyonun hareketleri (orta grid).
- Service: `IUretimOperasyonHareketDetayService.GetViewListWhere("where UrOHId='...'")` — secili hareketin detaylari (alt grid).
- Service: `IGenelService.GrupListesi("UretimOperasyon", <kolon>)` — Durumu/OperasyonKodu/ReceteAdi combobox kaynaklari (distinct deger listesi).
- Manager: `UretimEmriManager.UretimDurumGuncelle(Guid? urid)` — transaction icinde `exec [Uretim_DurumGuncelle] '<urid>'` calistirir.
- SQL/Prosedur: `Uretim_DurumGuncelle` — KalanMiktar yeniden hesabi + UretimOperasyon/UretimEmri/Siparis.Durumu guncellemesi + sifir miktarli detay silme.
- API: -
**Istasyon sirasiyla iliskisi:** Operasyon `Sira` alani akisin omurgasidir. `Uretim_PlanlananGuncelle` (bu formdan dogrudan cagrilmaz ama veri bu mantikla olusur) Sira=1 operasyonu icin hareket olusturur ve bir operasyonun uretim miktarini Sira N+1 operasyonun planlananina tasir. Alt grid detayina cift-tik ile acilan `FrmUretimIstasyonED` operasyon-detayinin istasyona giden islemini gosterir/degistirir (myLabel5 notu: "Istasyona Giden Islemi Degistirmek Icin Operasyon Detaya Cift Tiklayin").
**Notlar:** Durum combobox'i ve etiketi gizli (`CmbDurumu.Visible=false`, `myLabel4.Visible=false`); durum filtresi sadece hizli-filtre butonlariyla yapilir. `FocusedRowChanged` ile ust grid satiri degisince orta/alt grid'ler yeniden baglanir. Where filtreleri parametresiz string birlestirmesidir (SQL injection acisindan dikkat). `SiparisFiltreAyarla()` siparis kodunu `TxtKodu`'ya yazip baslangic tarihini yilin 1 Ocak'ina ceker.

---

### Operasyon Hareketler Listesi (`FrmUretimOperasyonHareketList.cs` / `FrmUretimOperasyonHareketList.Designer.cs`)
**Ne ise yarar:** `UretimOperasyonHareket` kayitlarini (operasyon hareketlerinin toplam miktarlari: Planlanan/Islemdeki/Uretim/Fire/Iptal/Kalan) tek bir duz listede gosteren rapor ekranidir. Operasyon/durum/recete ve baslangic/bitis tarih filtreleri icerir.
**Once ne olmali (onkosul):** En az bir uretim emri ve operasyonu olusturulmus, dolayisiyla `UretimOperasyonHareket` satirlari uretilmis olmali.
**Sonra ne olur:** Salt okuma raporu; herhangi bir tablo/prosedur degistirmez. Cift-tik/Enter sadece secim modunda satiri secip formu kapatir (normal modda is yapmaz).
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — filtrelerle `Bagla()` calistirir.
- `Temizle` (BtnTemizle) — combobox ve tarih alanlarini temizler.
- Grid cift-tik/Enter (`MyView1_MyEventDoubleClickEnter`) — secim modunda satiri dondurur; normal modda pasif.
- `Kapat / Yazdir / Dizayn` — MyFrmListe taban davranisi.
**Cagirdigi katmanlar:**
- Service: `IUretimOperasyonHareketService.GetViewListWhere(where)` — `select UrOH.*, UrO.IsEmriNo, UrO.OperasyonKodu, UrO.OperasyonAdi, UrO.ReceteKodu, UrO.ReceteAdi from UretimOperasyonHareket UrOH left outer join UretimOperasyon UrO on UrO.Id=UrOH.UrOId where ...`.
- Service: `IGenelService.GrupListesi("UretimOperasyon", <kolon>)` — Durumu/OperasyonKodu/ReceteAdi combobox kaynaklari.
- Manager: -
- SQL/Prosedur: -
- API: -
**Istasyon sirasiyla iliskisi:** Her hareket satiri bir operasyonun `Sira`'sina baglidir (where filtreleri `UrO.OperasyonKodu` uzerinden, tarih filtreleri `UrOH.BaslangicTarihi/BitisTarihi` uzerinden). Akis motoru (Uretim_MiktarGuncelle/PlanlananGuncelle) bu satirlarin miktar alanlarini gunceller.
**Notlar:** `BtnTemizle.Click += BtnTemizle_Click;` iki kez baglanmis (kucuk bir kod tekrari; islevsel etkisi temizleme isleminin iki kez calismasidir, sonuc ayni). Filtre where'leri parametresiz string birlestirmesidir.

---

### Operasyon Hareket Detay Listesi (`FrmUretimOperasyonHareketDetayList.cs` / `FrmUretimOperasyonHareketDetayList.Designer.cs`)
**Ne ise yarar:** `UretimOperasyonHareketDetay` kayitlarini (her uretim girisinin detayi: tarih, miktarlar) ust grid'de; secilen detaya ait `UretimIstasyon` kayitlarini alt grid'de master-detail gosterir. Iki modda calisir: (1) normal liste/filtreli, (2) belirli bir hareketin detaylarini gosteren odakli mod (`DetayGoster=true` + `DetayId`).
**Once ne olmali (onkosul):** Operasyon hareketleri uzerinde uretim girisi yapilmis olmali (detay/istasyon kayitlari olusmus olmali). Odakli modda cagiran ekran `DetayGoster=true` ve `DetayId`'yi (UrOH.Id) set etmelidir.
**Sonra ne olur:** Salt okuma. Ust grid'de bir detaya cift-tik/Enter -> `FrmUretimIstasyonED` (OperasyonTuru=Degistir, IdGuid=detay.Id, OprId=detay.UrOHId) acilir; istasyon hareketi orada degistirilir (yazma o formda gerceklesir, bu form degil).
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — `Bagla()`; `DetayGoster=true` ise filtreleri yok sayar ve sadece `DetayId` icin sorgular.
- `Temizle` (BtnTemizle) — combobox ve `TxtTarihi1/2` alanlarini temizler.
- Ust grid cift-tik/Enter (`MyView1_MyEventDoubleClickEnter`) — secim modunda satiri secip kapatir; normal modda `FrmUretimIstasyonED` (Degistir) acar.
- `Kapat / Yazdir / Dizayn` — MyFrmListe taban davranisi.
**Cagirdigi katmanlar:**
- Service: `IUretimOperasyonHareketDetayService.GetViewListWhere(where)` — `select UrOHD.*, UrO.IsEmriNo, UrO.OperasyonKodu, UrO.OperasyonAdi, UrO.ReceteKodu, UrO.ReceteAdi from UretimOperasyonHareketDetay UrOHD left outer join UretimOperasyonHareket UrOH on UrOH.Id=UrOHD.UrOHId left outer join UretimOperasyon UrO on UrO.Id=UrOHD.UrOId where ...`. Odakli modda where = `UrOH.Id='<DetayId>'`.
- Service: `IUretimIstasyonService.GetViewListWhere("where UrOHDId='...'")` — secili detayin istasyon kayitlari (alt grid).
- Service: `IGenelService.GrupListesi("UretimOperasyon", <kolon>)` — combobox kaynaklari.
- Manager: -
- SQL/Prosedur: -
- API: -
**Istasyon sirasiyla iliskisi:** Bu ekran operasyon-detayi ile UretimIstasyon arasindaki koprudur; alt grid `UrOHDId` uzerinden istasyon kayitlarini cozer. Cift-tik ile acilan `FrmUretimIstasyonED`, detayin hangi istasyona gittigini/islendigini gosterir.
**Notlar:** `BtnTemizle.Click += BtnTemizle_Click;` yine iki kez baglanmis. Sadece iki tarih filtresi var (`UrOHD.Tarih` alt/ust); diger formlardaki dort tarihten farklidir.

---

### OperasyonCardControlV2 (`OperasyonCardControlV2.cs` / `OperasyonCardControlV2.Designer.cs`)
**Ne ise yarar:** Operasyon takip panosunda (`FrmOperasyonTakip`) tek bir OPERASYON icin kart gosteren UserControl'dur. Iki kolonlu (Bekleyen / Uretimde) bir kart cizer; her kolona o operasyonun is satirlarini (`OperasyonCardRowControlV2`) `FlowLayoutPanel` icine doldurur. Saglik tik menuleriyle detay/uretim girisi ekranlarina hizli gecis saglar.
**Once ne olmali (onkosul):** Cagiran (`FrmOperasyonTakip`) `OperasyonKodu`, `OperasyonAdi`, `Hareketler` (List<UretimTakipModelV2>) ve `Action` (yenile geri cagrisi) property'lerini set edip kontrolu panoya eklemeli. Hareket modelleri `UretimTakipManagerV2` benzeri bir kaynaktan dolu gelir.
**Sonra ne olur:** `Bagla()` her hareket icin: `PlanlananMiktar - IslemdekiMiktar > 0` ise "Beklemede" satiri, `IslemdekiMiktar > 0` ise "Uretimde" satiri uretir. Satir cift-tiklaninca ilgili detay penceresi (`FrmOperasyonTakipDetaylar`) acilir. Menu komutlari ilgili liste/giris formlarini acar.
**Butonlar & kisayollar (sag-tik ContextMenu ogeleri):**
- Bekleyen panel `Detaylar > Istasyona Gonder` (detaylarToolStripMenuItem) — `BekleyenDetaylarAc()` -> `Ortak.UretimTakipBekleyenAcV2(OperasyonKodu)` -> `FrmOperasyonTakipDetaylar` (Durumu="Beklemede").
- Bekleyen panel `Operasyon Hareket Detaylari` (operasyonDetaylariToolStripMenuItem) — `FrmUretimOperasyonHareketDetayList` (ShowDialog).
- Uretimde panel `Direk > Uretim Girisi` (uretimGirisiToolStripMenuItem) — once `FrmUretimIstasyonHareketSec` (secim) sonra secilen istasyon icin `FrmUretimIstasyonUretimGir` acar.
- Uretimde panel `Detaylar > Uretim Girisi` (toolStripMenuItem1) — `UretimdeDetaylarAc()` -> `Ortak.UretimTakipUretimdeAcV2(OperasyonKodu, Action)` -> `FrmOperasyonTakipDetaylar` (Durumu="Uretimde", yenile Action ile).
- Uretimde panel `Istasyon Hareketler/Uretimler` (uretimlerToolStripMenuItem) — `FormAc.FormSec("IstasyonDetaylar")` -> `FrmUretimIstasyonHareketDetayList`.
- Uretimde panel `Operasyon Hareket Detaylari` (operasyonDetaylariToolStripMenuItem1) — `FrmUretimOperasyonHareketDetayList` (ShowDialog).
- Satir cift-tik (kart satirinda) — `OperasyonCardRowControlV2` icindeki `Action` (BekleyenDetaylarAc / UretimdeDetaylarAc) calisir.
**Cagirdigi katmanlar:**
- Helper: `Ortak.UretimTakipBekleyenAcV2 / UretimTakipUretimdeAcV2` — `FrmOperasyonTakipDetaylar`'i ilgili durumla acar.
- Helper: `FormAc.FormSec("IstasyonDetaylar")` — istasyon hareket detay listesini acar.
- Form: `FrmUretimIstasyonHareketSec`, `FrmUretimIstasyonUretimGir`, `FrmUretimOperasyonHareketDetayList`.
- Service/Manager: dogrudan yok (veri disaridan `Hareketler` ile gelir).
- SQL/Prosedur: -
- API: -
**Istasyon sirasiyla iliskisi:** Kart "Uretim Girisi" akisi saha uretim girisini tetikler (FrmUretimIstasyonUretimGir); uretim girisi sonrasinda akis motoru (Uretim_MiktarGuncelle/Uretim_PlanlananGuncelle) miktarlari Sira boyunca yukari/ileri tasir. Kart panoyu `Action` (Bagla yenile) ile guncel tutar.
**Notlar:** Bekleyen miktar = PlanlananMiktar - IslemdekiMiktar; Uretimde miktar = IslemdekiMiktar - UretimMiktari (satir kontrolunde hesaplanir). Renkler durum vurgusu icindir (Bekleyen lacivert, Uretimde yesil).

---

### OperasyonCardRowControlV2 (`OperasyonCardRowControlV2.cs` / `OperasyonCardRowControlV2.Designer.cs`)
**Ne ise yarar:** `OperasyonCardControlV2` kartinin icindeki tek bir is-satirini (Recete adi, Miktar, Siparis Kodu, Cari Unvani) gosteren kucuk UserControl'dur. Tiklaninca secili duruma gecer (tek-secim mantigi), cift-tiklaninca kartin verdigi `Action`'i (detay penceresi) calistirir.
**Once ne olmali (onkosul):** `OperasyonCardControlV2.Bagla()` tarafindan `Model` (UretimTakipModelV2, Turu="Beklemede"/"Uretimde" set edilmis), `Id` ve `Action` verilerek olusturulur.
**Sonra ne olur:** Yuklenince etiketleri doldurur ve `OperasyonCardManagerV2.SecildiEvent`'e abone olur. Tiklaninca `Secildi()` -> statik `OperasyonCardManagerV2.SecildiTetikle(...)` ile diger satirlara "secilmedi" sinyali yayar ve kendini Teal renge boyar. Cift-tik -> `Action.Invoke()` (detay penceresi acar).
**Butonlar & kisayollar:**
- Etiketlere tek tik (LblRecete/LblMiktar/LblSiparisKodu/LblCariUnvani Click) — `Secildi()` (satiri sec/vurgula).
- Etiketlere cift-tik (MouseDoubleClick) — `Action.Invoke()` (kart tarafindan verilen detay acma).
- Enter (kontrol Enter eventi) — `LblRecete_Click` (secim) ile ayni davranis.
**Cagirdigi katmanlar:**
- Helper: `OperasyonCardManagerV2` (statik event yoneticisi) — `SecildiEvent` / `SecildiTetikle` ile satirlar arasi tek-secim koordinasyonu; `RowId` artan kimlik uretir.
- Service/Manager: -
- SQL/Prosedur: -
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Modeli `UretimTakipModelV2`'dir; Turu alanina gore "Uretimde" satirinda kalan = IslemdekiMiktar - UretimMiktari, "Beklemede" satirinda kalan = PlanlananMiktar - IslemdekiMiktar gosterilir.

---

### Yardimci tipler (`OperasyonTuruEnums.cs`, `OperasyonCardRowEventArgs.cs`)
**Ne ise yarar:** Modulun yardimci tip tanimlaridir; ekran degildir.
- `OperasyonTuruEnum` (SonrakiOperasyon, IstasyonEkle, Degistir) — `FrmUretimIstasyonED` acilirken islem turunu belirtmek icin kullanilir (detay cift-tik akislari `Degistir` gonderir).
- `OperasyonCardRowEventArgsV2` + `OperasyonCardManagerV2` — kart satirlari arasi tek-secim icin EventArgs ve statik event-broker; `OperasyonCardRowControlV2` bunlari kullanir.
**Once ne olmali (onkosul):** -
**Sonra ne olur:** -
**Butonlar & kisayollar:** -
**Cagirdigi katmanlar:** -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** `OperasyonCardRowEventArgsV2` ve `OperasyonCardManagerV2` `MyUI.MyControl` namespace'inde (modul klasorunde olsalar da farkli namespace) tanimlidir.
