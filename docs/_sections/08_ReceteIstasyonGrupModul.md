## Modul: ReceteIstasyonGrupModul

Bu modul, "istasyon gruplama" akisinin tanim/eslestirme altyapisini yonetir. Uretimde her operasyonun hangi istasyonda yapilacagini "grup kodu" uzerinden tanimlamaya yarar. Uc ekrandan olusur: (1) grup kodlarinin sozluk kaydi (`FrmReceteIstasyonGrupKodlari`), (2) bir grup kodu altinda hangi operasyonun hangi istasyona gidecegini esleyen tablo (`FrmReceteIstasyonGrupOperasyonEslestir`), ve (3) belirli bir receteye bir veya daha fazla grup kodu baglamak (`FrmReceteIstasyonGrupIstasyonEslestir`). Bu tanimlar dogrudan veri girisi yapmaz; uretim motorunda `Uretim_SonrakiIstasyonaGonder` prosedurunun `ReceteAna.IstasyonGruplamaKullan = 1` iken hangi operasyona hangi istasyonun (TEK `UretimIstasyon` kaydi) otomatik olusturulacagini belirleyen referans veridir. Eslesme `ReceteIstasyonGrupOperasyon` tablosu uzerinden `GrupKodu = UretimEmri.IstasyonGrupKodu AND OperasyonKodu = UretimOperasyon.OperasyonKodu` JOIN'i ile kurulur.

Uc ekran da `MyFrmKayit` (harici `My.Kontrol.Formlar` DLL'i) base formundan turer. Base form standart alt buton seridini (BtnKaydet, BtnSil, BtnDuzenle, BtnYeni, BtnKapat, BtnYazdir, navigasyon: BtnIlk/BtnOnceki/BtnSonraki/BtnSon) ve secim modu altyapisini (`SecimIcinAcildi`, `Secildi`, `SecilenKod`, `SecilenRow`, `SecilenId`, `IdGuid`, `YeniKayit`, `KayitEdildi`, `AcilisBittimi`) saglar. Buton kisayol tuslari (ShortcutKeys) Designer dosyalarinda tanimli degildir; butonlar Click event'leriyle calisir. Grid icinde cift tiklama/Enter (`MyEventDoubleClickEnter`) secim modunda kaydi secip kapatir, normal modda Duzenle'yi tetikler. Hicbir ekranda BtnYazdir icin handler baglanmamistir (pasif).

### Istasyon Grup Kodlari (`FrmReceteIstasyonGrupKodlari.cs`)
**Ne ise yarar:** Istasyon grup kodlarinin ana sozluk/tanim ekranidir (Kodu, Adi, Aciklama). Diger iki ekran ve recete bu kodlari referans alir. Hem standalone kayit ekrani hem de baska ekranlardan "kod sec" amacli (SecimIcinAcildi) acilan secim ekrani olarak calisir.
**Once ne olmali (onkosul):** Yok. Bagimsiz bir tanim ekranidir; FrmAna ribbon menusunden ("Istasyon-Grup Kodlar") veya diger eslestirme ekranlarindaki grup kodu butonundan acilir.
**Sonra ne olur:** Kaydet -> `ReceteIstasyonGrupKod` tablosuna insert/update; liste yeniden baglanir. Sil -> ilgili satir silinir. Secim modunda cift tik/Enter ile `SecilenKod`/`SecilenRow`/`SecilenId` doldurulur, `Secildi=true` yapilip form kapanir (cagiran ekran kodu alir).
**Butonlar & kisayollar:**
- `Kaydet (BtnKaydet)` — `Kaydet()` cagirir: zorunlu alan (Kodu) kontrolu + `KodVarmi(...)` benzersizlik kontrolu, sonra `InsertOrUpdate`, ardindan `Bagla()`.
- `Sil (BtnSil)` — once `MesajSor("Kaydı silmek istiyormusunuz..")` onayi; secili kayit yoksa uyari; `Sil()` -> `Delete`.
- `Duzenle (BtnDuzenle)` — secili grid satirini klonlayip text kutularina aktarir (`AktarTextlere`).
- `Yeni (BtnYeni)` — alanlari temizler (`TemizleText`), `YeniKayit=true`.
- `Kapat (BtnKapat)` — base form formu kapatir.
- Grid cift tik / Enter (`MyView1_MyEventDoubleClickEnter`) — secim modunda kodu secip kapatir; degilse Duzenle'yi tetikler (`BtnDuzenle.PerformClick()`).
**Cagirdigi katmanlar:**
- Manager/Service: `IReceteIstasyonGrupKodService` (Ortak.DbPro.ReceteIstasyonGrupKodlar) — `SelectListWhere(" Order By Kodu ")` liste, `InsertOrUpdate(_mdl)` kayit, `Delete(_mdl)` sil.
- Service: `ReceteIstasyonGrupKodService.KodVarmi<ReceteIstasyonGrupKod>(_mdl,"Kodu",YeniKayit)` — ayni Kodu ile baska kayit var mi diye `Select count(*)` ile kontrol eder; varsa hata.
- SQL/Prosedur: `ReceteIstasyonGrupKod` tablosu (CRUD). Dolayli olarak `Uretim_SonrakiIstasyonaGonder` bu kodlari (UretimEmri.IstasyonGrupKodu uzerinden) tuketir.
- API: -
**Istasyon sirasiyla iliskisi:** Dolayli. Burada tanimlanan grup kodu, operasyon-istasyon eslestirmesinin (sira) ust anahtaridir; sirayi belirlemez ama hangi istasyon setinin kullanilacagini belirler.
**Notlar:** Entity alanlari: Id (Guid PK), Kodu, Adi, Aciklama. Grid'de Id kolonu gizli. `myGrid1.MyGridKayitAdi = "IstasyonKartlariListesi"` (grid yerlesim kayit adi). Ribbon caption "Istasyon-Grup Kodlar", form basligi "İstasyon Grup Kodları".

### Recete Istasyon-Operasyon Eslestir (`FrmReceteIstasyonGrupOperasyonEslestir.cs`)
**Ne ise yarar:** Bir grup kodu altinda, her operasyon kodunun hangi istasyon koduna/adina karsilik geldigini esleyen tablodur (GrupKodu + OperasyonKodu -> IstasyonKodu/IstasyonAdi). Uretim gruplama motorunun (`Uretim_SonrakiIstasyonaGonder`) operasyon basina TEK `UretimIstasyon` kaydi olusturmak icin okudugu ana referanstir.
**Once ne olmali (onkosul):** En az bir grup kodu (`FrmReceteIstasyonGrupKodlari`'nda) tanimli olmali; eslenecek operasyon kodlari (Operasyon Kartlari) ve istasyon kodlari (Istasyon Kartlari) tanimli olmali. Ekran FrmAna ribbon ("Istasyon-Grup Operasyonlar") veya FrmReceteED'deki "İstasyon-Grup Ayarlar" butonundan acilir.
**Sonra ne olur:** Once grup kodu secilir (TxtGrupKodu butonu), liste o gruba filtrelenir. Yeni/Duzenle ile operasyon ve istasyon secilip Kaydet -> `ReceteIstasyonGrupOperasyon` tablosuna insert/update; liste `Bagla()` ile o grup koduna gore yeniden yuklenir, text alanlari kilitlenir (`TxtLeriKapat`). Sil -> satir silinir.
**Butonlar & kisayollar:**
- `Kaydet (BtnKaydet)` — `Kaydet()`: GrupKodu/OperasyonKodu/IstasyonKodu zorunlu kontrolu, `InsertOrUpdate`, `Bagla()`, `TxtLeriKapat()`. Acilista pasiftir, Yeni/Duzenle ile aktiflesir.
- `Sil (BtnSil)` — `MesajSor` onayi + secim kontrolu, `Sil()` -> `Delete`. Acilista pasif.
- `Duzenle (BtnDuzenle)` — secili satiri klonlar, alanlara aktarir (`AktarTextlere`), alanlari acar (`TxtLeriAc`).
- `Yeni (BtnYeni)` — `TemizleText` (GrupKodu haric temizler), alanlari acar, bos model olusturur.
- `TxtGrupKodu (ButtonEdit butonu)` — `TxtGrupKodu_ButtonClick`: `FrmReceteIstasyonGrupKodlari`'ni secim modunda acar, secilen kodu yazip listeyi yeniler.
- `TxtOperasyonKodu / TxtOperasyonAdi (ButtonEdit butonu)` — `TxtOperasyonKodu_ButtonClick`: `_mdl` null ise islem yapmaz; `FrmOperasyonKartlari`'ni secim modunda acar, secilen `OperasyonKodu`/`OperasyonAdi`'ni yazar.
- `TxtIstasyonKodu / TxtIstasyonAdi (ButtonEdit butonu)` — `TxtIstasyonKodu_ButtonClick`: `_mdl` null ise islem yapmaz; `FrmIstasyonKartList`'i secim modunda acar, secilen `IstasyonKodu`/`IstasyonAdi`'ni yazar.
- `Kapat (BtnKapat)` — formu kapatir.
- Grid cift tik / Enter — secim modunda GrupKodu secip kapatir; degilse Duzenle.
**Cagirdigi katmanlar:**
- Manager/Service: `IReceteIstasyonGrupOperasyonService` (Ortak.DbPro.ReceteIstasyonGrupOperasyonlar) — `SelectListWhere(" where GrupKodu='...' Order By GrupKodu ")` liste, `InsertOrUpdate`, `Delete`.
- Diger formlar: `FrmReceteIstasyonGrupKodlari` (grup kodu secimi), `FrmOperasyonKartlari` (MyUI.UretimOperasyonModule, operasyon secimi), `FrmIstasyonKartList` (MyUI.IstasyonModul, istasyon secimi).
- SQL/Prosedur: `ReceteIstasyonGrupOperasyon` tablosu (CRUD). `Uretim_SonrakiIstasyonaGonder` bu tabloyu `LEFT OUTER JOIN ReceteIstasyonGrupOperasyon GROP ON GROP.GrupKodu = UR.IstasyonGrupKodu AND GROP.OperasyonKodu = URO.OperasyonKodu` ile okuyup her operasyon icin `UretimIstasyon` (IstasyonKodu/IstasyonAdi) kaydi acar.
- API: -
**Istasyon sirasiyla iliskisi:** Dogrudan iliskili ama sirayi KENDISI tutmaz. Designer'daki uyari etiketi: "Siralamayı Operasyondan Alır. Burda eşleşenleri operasyondan kontrol eder." Yani operasyon sirasi `UretimOperasyon`/`ReceteOperasyon` tarafindan belirlenir; bu ekran sadece her operasyona istasyon esler.
**Notlar:** Entity alanlari: Id (PK), GrupKodu, OperasyonKodu, OperasyonAdi, IstasyonKodu, IstasyonAdi. Grid'de Id/OprId/IstId kolonlari gizlenir (gizlenen OprId/IstId kolonlari entity'de yok; SutunGizle cagrisi varsa-yoksa guvenli calisir). `myGrid1.MyGridKayitAdi = "IstasyonOperasyonEslestirListesi"`. Form basligi "Recete Istasyon-Operasyon Eşleştir".

### Receteye Istasyon Grup Sec (`FrmReceteIstasyonGrupIstasyonEslestir.cs`)
**Ne ise yarar:** Belirli bir receteye (RcAId) bir veya daha fazla istasyon grup kodu baglar. `ReceteIstasyonGrupIstasyon` tablosuna (RcAId + GrupKodu) kayit ekler. Boylece o recete uretildiginde hangi grup kodunun (dolayisiyla operasyon-istasyon eslestirmesinin) gecerli olacagi belirlenir.
**Once ne olmali (onkosul):** Recete kayitli olmali. FrmReceteED'de "İstasyon Gruplar" butonu, recete kayitli degilse ("Reçete Kayıt Edilmeden Istasyon Grup Eklenemiyor..") engeller; aksi halde `f.RcAId = _mdl.Recete.Id` atayarak bu formu acar. Eslenecek grup kodunun operasyonlari (`FrmReceteIstasyonGrupOperasyonEslestir`'de) tanimli olmali.
**Sonra ne olur:** Acilista recete bilgisi (`ReceteBagla`) ve mevcut esleme listesi (RcAId'ye gore) yuklenir, text/butonlar kilitlidir. Grup kodu secildiginde `ReceteEslesiyormu(kod)` ile recetenin operasyonlari ile grup operasyonlarinin tam eslesip eslesmedigi dogrulanir; eslesmiyorsa kayit engellenir. Kaydet -> `ReceteIstasyonGrupIstasyon` tablosuna insert/update, liste yeniden baglanir, alanlar kilitlenir. Sil -> esleme satiri silinir.
**Butonlar & kisayollar:**
- `Kaydet (BtnKaydet)` — `Kaydet()`: GrupKodu ve ReceteKodu zorunlu kontrolu, `InsertOrUpdate` (RcAId + GrupKodu), `Bagla()`, `TxtLeriKapat()`. Acilista pasiftir.
- `Sil (BtnSil)` — `MesajSor` onayi + secim kontrolu, `Sil()` -> `Delete`. Acilista pasif.
- `Duzenle (BtnDuzenle)` — secili satiri klonlar, GrupKodu'nu aktarir, alanlari acar (`TxtLeriAc`).
- `Yeni (BtnYeni)` — `TemizleText`, alanlari acar, bos model.
- `TxtGrupKodu (ButtonEdit butonu)` — `TxtGrupKodu_ButtonClick`: `FrmReceteIstasyonGrupKodlari`'ni secim modunda acar; secilen kod icin `ReceteEslesiyormu(kod)` kontrolu yapilir, gecerse kod yazilip liste yenilenir, degilse uyari verip iptal eder.
- `Kapat (BtnKapat)` — formu kapatir.
- Grid cift tik / Enter — secim modunda GrupKodu secip kapatir; degilse Duzenle.
- `TxtReceteKodu`, `TxtReceteAdi` — salt okunur (Enabled=false), acilista receteden doldurulur.
**Cagirdigi katmanlar:**
- Manager/Service: `IReceteIstasyonGrupIstasyonService` (Ortak.DbPro.ReceteIstasyonGrupIstasyonlar) — `SelectListWhere(" where RcAId='...' Order By GrupKodu ")`, `InsertOrUpdate`, `Delete`.
- Manager/Service: `IReceteAnaService` (Ortak.DbPro.ReceteAna) — `SelectFind(RcAId)` ile recete bilgisini (ReceteKodu/ReceteAdi) getirir.
- Manager/Service: `IReceteOperasyonService` (Ortak.DbPro.ReceteOperasyon) — `SelectList(c => c.RcAId==RcAId)` recetenin operasyonlarini getirir (eslesme dogrulamasi icin).
- Manager/Service: `IReceteIstasyonGrupOperasyonService` (Ortak.DbPro.ReceteIstasyonGrupOperasyonlar) — `SelectList(c => c.GrupKodu==kod)` secilen grubun operasyonlarini getirir (eslesme dogrulamasi icin).
- Service: `ReceteEslesiyormu(string kod)` — recetenin her operasyon kodunun, secilen grubun operasyonlari icinde bulunup bulunmadigini kontrol eder; bulunamayan operasyon varsa "Secilen Grup ile Recete Operasyonları eşleşmiyor" hatasi.
- SQL/Prosedur: `ReceteIstasyonGrupIstasyon` tablosu (CRUD). Bu eslesme uretim asamasinda recete bazli grup kodu secimini saglar (UretimEmri.IstasyonGrupKodu uretildiginde bu tanim baz alinir).
- API: -
**Istasyon sirasiyla iliskisi:** Dolayli. Receteyi bir grup koduna bagladigi icin, o recete uretildiginde operasyon-istasyon eslestirmesini (ve dolayisiyla `Uretim_SonrakiIstasyonaGonder`'in istasyon olusturmasini) etkiler. Sirayi tutmaz.
**Notlar:** Entity alanlari: Id (PK), RcAId, GrupKodu. Grid'de Id/RcAId gizli. `myGrid1.MyGridKayitAdi = "IstasyonReceteEslestirListesi"`. Form basligi "Receteye Istasyon Grup Seç" (lblBaslik arka plan Teal). Tek receteye birden fazla grup kodu eklenebilir (liste). Recete-grup operasyon eslesme dogrulamasi, hatali grup secimini engelleyen onemli is kuralidir.
