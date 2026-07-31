## Modul: Raporlar

UretimV4 (CepPatronERP.exe) masaustu uygulamasinin "Raporlar" klasoru, uretim/recete/istasyon verilerini salt-okunur listeleyen 5 form icerir. Uc alt klasore ayrilir: `IstasyonRaporlari` (operator/istasyon bazli uretim ozeti), `ReceteRaporlari` (recetelerde kullanilan stoklarin listesi + recete genel raporu) ve `UretimRaporlari` (uretimde tuketilen stoklarin toplu/detayli raporu, Mikro stok kategorileriyle birlikte). Tum formlar `My.Kontrol.Formlar.MyFrmListe` taban sinifindan turer; bu sinif ortak liste duzenini (sol filtre tab paneli `tabAra1`, orta `myGrid1` + DevExpress `myView1`, alt buton paneli) ve standart butonlari (`BtnAra`, `BtnTemizle`, `BtnYazdir`, `BtnDizayn`, `BtnKapat`) saglar. Formlarin tamami acilista otomatik `BtnAra.PerformClick()` cagirir; UretimRaporlari formlari ek olarak acilista Mikro stok/kategori temp tablolarini ve uretim stok-hareket detay merge tablosunu gunceller. Hicbiri veri yazmaz/silmez (yalnizca rapor okur); bu yuzden uretim akis prosedurleri (Uretim_MiktarGuncelle / Uretim_PlanlananGuncelle / Uretim_SonrakiIstasyonaGonder) bu modulde cagrilmaz. Raporlar uretim akisinin SONUCUNU goruntuleyen ekranlardir.

### Istasyon Raporu (`MyUI/Raporlar/IstasyonRaporlari/FrmIstasyonRaporu.cs`)
**Ne ise yarar:** Belirli tarih/saat araliginda, istasyon ve personel filtresiyle gerceklesen uretim hareketlerini (mamul giris, fire mamul giris, uretim bitis, uretim iptal) iki gridde gosterir: ust grid gun+stok+istasyon+personel kirilimli hareketler (`myGrid1`), alt grid sadece stok bazli toplamlar (`myGrid2`).
**Once ne olmali (onkosul):** Saha akisinda (TabletV2) istasyon takip hareketleri girilmis ve `IstasyonTakipHareketDetay` tablosuna `Turu` = MamulGiris/FireMamulGiris/UretimBitis/UretimIptal kayitlari islenmis olmali. Personel/Istasyon kartlari tanimli olmali (combo'lari doldurmak icin).
**Sonra ne olur:** Salt-okunur rapor; tabloya/proseure yazma yok. "Ara" sonrasi gridler doldurulur. Satira cift tiklaninca (yalnizca form `SecimIcinAcildi` ile bir secim diyalogu olarak acildiysa) secilen `ReceteKodu`/`Id` geri dondurulup form kapanir; normal acilista cift tik bir sey yapmaz.
**Butonlar & kisayollar:**
- `Ara` (`BtnAra`) — filtreleri uygulayip `Bagla()` ile her iki gridi yeniden doldurur (Designer'da `BtnAra.Click -> BtnAra_Click`). Acilista da otomatik tetiklenir.
- `Temizle` (`BtnTemizle`) — taban formdan gelen filtre temizleme butonu.
- `Yazdir` (`BtnYazdir`) — taban formdan gelen grid yazdirma.
- `Dizayn` (`BtnDizayn`) — grid kolon/yerlesim dizayni.
- `Kapat` (`BtnKapat`) — formu kapatir.
- Tarih1/Tarih2 (`TxtTarihi1`/`TxtTarihi2`), Saat1/Saat2 (`TxtSaat1` vars. "00:00", `TxtSaat2` vars. "23:59"), Istasyon combo (`CmbIstasyon`), Personel combo (`CmbPersonel`) — filtre alanlari (kisayol tanimli degil; Enter ile sonraki kontrole gecer, EnterMoveNextControl=true).
**Cagirdigi katmanlar:**
- Manager/Service: `IstasyonRaporManager.GetIstasyonRapor(string andSorgu)` — hem hareket hem toplam SQL'ini calistirip `IstasyonRaporModel` (Hareketler + Toplamlar) dondurur.
- Service: `IIstasyonKartiService.SelectListWhere("")` — `CmbIstasyon` combo'sunu istasyon kodlariyla doldurur.
- Service: `IPersonelService.SelectListWhere("")` — `CmbPersonel` combo'sunu personel kodlariyla doldurur.
- SQL: `IstasyonRaporHareketModel.GetSelectSqlCode()` — `IstasyonTakipHareketDetay HR` + `IstasyonTakipHareket IST` + `Personel Prs` JOIN; `Turu IN (MamulGiris,FireMamulGiris,UretimBitis,UretimIptal)`; gun/stok/istasyon/personel GROUP BY; UretimMiktar/FireMiktar/IptalMiktar SUM.
- SQL: `IstasyonRaporToplamModel.GetSelectSqlCode()` — ayni filtre, sadece stok bazinda SUM (alt toplam gridi).
- API: -
**Istasyon sirasiyla iliskisi:** Dogrudan istasyon bazli; `CmbIstasyon` ile tek istasyona daraltilabilir. Filtre SQL'i `Ist.IstasyonKodu` ve `HR.KayitEden` (personel) uzerinden calisir. Operasyon-Sira mantigi yok; hareket kaydinin baglandigi istasyon koduna gore raporlar.
**Notlar:** Filtre stringleri parametresiz string-interpolation ile kuruluyor (SQL injection riski). Saat alanlari tarih ile birlestirilerek `CAST(... AS datetime)` araligi olusturur. `myGrid1` kayit adi "IstasyonRaporuHareketList1", `myGrid2` "IstasyonRaporuToplamList1".

### Recete Kullanilan Stok Listesi (`MyUI/Raporlar/ReceteRaporlari/FrmReceteKullanilanStokList.cs`)
**Ne ise yarar:** Tum recete detay satirlarini (her recetede kullanilan stoklar) tek duz listede gosterir; recete kodu/adi veya varsayilan stok kodu/adi ile aranabilir.
**Once ne olmali (onkosul):** Receteler ve recete detaylari (`ReceteAna` / `ReceteDetay`) tanimlanmis olmali.
**Sonra ne olur:** Salt-okunur liste. Satira cift tiklaninca ilgili recete `FrmReceteED` (Recete Edit/Detay formu) ile `IdGuid = itm.RcAId` olarak acilir; orada kayit edilirse (`f.KayitEdildi`) liste tekrar yenilenir (`BtnAra.PerformClick()`) ve odak ayni satira geri konur. Form secim modunda acildiysa (`SecimIcinAcildi`) secilen `ReceteKodu` dondurulup kapanir.
**Butonlar & kisayollar:**
- `Ara` (`BtnAra`) — `Bagla()` ile listeyi `TxtAra` metnine gore filtreler (`EventlerBagla` icinde `BtnAra.Click += BtnAra_Click`).
- `Temizle` / `Yazdir` / `Dizayn` / `Kapat` — taban form (`MyFrmListe`) standart butonlari.
- Cift tik / Enter (grid) — `MyView1_MyEventDoubleClickEnter`: receteyi `FrmReceteED` ile acar veya secim modunda secip kapatir.
- `Ara` filtre kutusu (`TxtAra`) — recete kodu/adi/varsayilan stok kodu/adi icinde LIKE arama.
**Cagirdigi katmanlar:**
- Manager/Service: `ReceteManager.GetReceteKullanilanStokList(string wheresql)` — `ReceteDetay rd LEFT JOIN ReceteAna ra` sorgusuyla `ReceteKullanilanStokModel` listesi dondurur.
- SQL: yukaridaki inline SELECT (`ReceteDetay` + `ReceteAna`); filtre `ra.ReceteKodu / ra.ReceteAdi / VarsayilanStokKodu / VarsayilanStokAdi` uzerinden LIKE.
- Form: `FrmReceteED { IdGuid = RcAId }` — cift tikta recete duzenleme formu acar.
- API: -
**Istasyon sirasiyla iliskisi:** - (recete tanim raporu; uretim/istasyon akisi yok)
**Notlar:** Filtre verildiginde sorgu basina `where 1 = 1` eklenir. `myGrid1` kayit adi "ReceteKullanilanStokList1".

### Recete Raporu (`MyUI/Raporlar/ReceteRaporlari/FrmReceteRaporu.cs`)
**Ne ise yarar:** Recetelerin genel raporu: her recete icin once recete ust satiri, ardindan o recetenin detay (kullanilan stok) satirlari fis-sira/recete-sira duzeninde listelenir; fireli miktar dahil hesaplanir. Entegre (Mikro) stok kodu/adi ile aranabilir.
**Once ne olmali (onkosul):** `ReceteAna` (EntegreStokKodu/Adi dahil) ve `ReceteDetay` kayitlari olmali. Detay satirlarinin cins bilgisi icin `TempMikroStok` temp tablosu dolu olmali (LEFT JOIN; bos olursa cins bos gelir).
**Sonra ne olur:** Salt-okunur. Cift tik davranisi Kullanilan Stok Listesi ile ayni (recete `FrmReceteED` ile acilir veya secim modunda secilir). `RcAId` ve `RcDId` kolonlari gridde gizlenir.
**Butonlar & kisayollar:**
- `Ara` (`BtnAra`) — `Bagla()` ile genel rapor sorgusunu calistirir (Designer'da event yok; `EventlerBagla` icinde `BtnAra.Click += BtnAra_Click`).
- `Temizle` / `Yazdir` / `Dizayn` / `Kapat` — taban form standart butonlari.
- Cift tik / Enter (grid) — `MyView1_MyEventDoubleClickEnter`: `FrmReceteED` acar veya secimi dondurur.
- `Ara` filtre kutusu (`TxtAra`) — `EntegreStokKodu` / `EntegreStokAdi` icinde LIKE arama.
**Cagirdigi katmanlar:**
- Manager/Service: `ReceteManager.GetReceteGenelRaporuList(string wheresql)` — `#TempReceteRaporu` gecici tablosu + `ReceteRaporCursor` cursor'u ile her receteyi gezip ust+detay satirlarini uretir; `FireliMiktar = Miktar * (1 + FireYuzde/100)` hesaplar.
- SQL: cursor tabanli rapor; `ReceteAna` (cursor kaynagi) + `ReceteDetay RD LEFT JOIN TempMikroStok TmpST` (cins bilgisi); sonuc `ReceteRaporuModel`.
- Form: `FrmReceteED { IdGuid = RcAId }` — cift tikta recete duzenleme.
- API: -
**Istasyon sirasiyla iliskisi:** - (recete tanim/maliyet raporu)
**Notlar:** `myView1.MyKurusHane = 4` (miktar 4 ondalik gosterilir). Filtre verildiginde `where 1 = 1` eklenir. `myGrid1` kayit adi "ReceteRaporu1". Cursor + temp table kullandigindan buyuk recete sayisinda yavaslayabilir.

### Uretim Stok Tuketim Raporu (`MyUI/Raporlar/UretimRaporlari/FrmUretimStokTuketimRaporu.cs`)
**Ne ise yarar:** Uretimde tuketilen/giren stoklarin TOPLU (stok bazli, miktarlari SUM'lanmis) raporu. Her stok icin uretim/fire/iptal miktarlari ve recete carpaniyla hesaplanan stok miktarlari; Mikro stok kategori/kalite kontrol/reyon bilgileriyle zenginlestirilmis. Tarih-saat araligi ve cok secimli kategori/kalitekontrol/reyon filtreleri + serbest metin arama.
**Once ne olmali (onkosul):** Saha (TabletV2) uretim akisinda mamul/fire/stok hareketleri girilmis olmali. ONEMLI: form acilista `TempGuncelle()` ile Mikro stok ve kategori temp tablolarini ve `IstasyonTakipStokHareketDetay` merge tablosunu gunceller; bu yuzden Mikro DB (`Ortak.DbMikro`) erisilebilir olmali. Mikro stok kategorileri (kategori/kalitekontrol/reyon) combo'lari bu temp tablodan beslenir.
**Sonra ne olur:** Salt-okunur rapor (uretim akisini degistirmez). Acilis sirasinda yan etki olarak: `TempMikroStokKategori`, `TempMikroStok`, `TempSonGuncelleme` ve `IstasyonTakipStokHareketDetay` tablolari guncellenir/merge edilir (TempGuncelle). "Ara"da grid doldurulur, kayit sayisi status bar'da (`lblKayitSayisi`) gosterilir. Gridde cift tik/odak event'leri devre disi (yorum satiri).
**Butonlar & kisayollar:**
- `Ara` (`BtnAra`) — `Bagla()` ile toplu raporu calistirir (Designer'da `BtnAra.Click -> BtnAra_Click`, TabIndex=0).
- `Temizle` (`BtnTemizle`) / `Yazdir` / `Dizayn` / `Kapat` — taban form standart butonlari.
- Tarih1/2 (`TxtTarihi1`/`TxtTarihi2`), Saat1/2 (`TxtSaat1`="00:00", `TxtSaat2`="23:59") — tarih-saat araligi filtresi.
- Kategori (`CmbKategori`), Kalite.Kont. (`CmbKaliteKontrol`), Reyon (`CmbReyon`) — `MyComboBoxCheck` cok secimli (`;` ayrac) filtreler.
- `Ara` metin kutusu (`TxtAra`) — stok kodu/adi LIKE arama.
**Cagirdigi katmanlar:**
- Service: `IIstasyonTakipStokHareketDetayService.GetListViewInKategoriToplu(sor, sor2)` — iki kollu UNION ALL sorgu: (1) `IstasyonTakipStokHareketDetay ITSHD LEFT JOIN TempMikroStok` stok bazli SUM; (2) `IstasyonTakipHareketDetay ITHD` `Turu='FireStokGiris'` fire stok girisleri. Kategori/kalitekontrol/reyon TMPS uzerinden filtrelenir.
- Service: `IIstasyonTakipStokHareketDetayService.DetaylarGuncelleToplu()` — acilista (TempGuncelle) `IstasyonTakipStokHareketDetay` tablosunu MERGE ile yeniden hesaplar (UrO.Sira=1, Turu UretimBitis/MamulGiris/FireMamulGiris; recete planlanan miktar / siparis miktar carpaniyla StokMiktar/StokFireMiktar/StokIptalMiktar uretir).
- Service: `ITempMikroStokService.MikroStokKategoriGuncelle(mikroDb)` — Mikro STOK_KATEGORILERI/KALITE_KONTROL/REYONLARI -> `TempMikroStokKategori` merge.
- Service: `ITempMikroStokService.MikroStokGuncelle(mikroDb)` — Mikro STOKLAR (+kategori/kalite/reyon/birim) -> `TempMikroStok` merge (3 dakikada bir; TempSonGuncelleme ile kontrol).
- Service: `ITempMikroStokService.GetStokKategoriListStokKategori() / GetStokKategoriListKaliteKontrol() / GetStokReyonList()` — combo'lari `TempMikroStokKategori`'den (Turu'ye gore) doldurur.
- SQL/Prosedur: yukaridaki inline MERGE ve UNION ALL sorgulari (stored procedure degil, servis ici dinamik SQL).
- API: -
**Istasyon sirasiyla iliskisi:** Dolayli: `DetaylarGuncelleToplu` ve stok miktar carpani yalnizca operasyon `UrO.Sira=1` (ilk operasyon) icin hesaplanir — yani stok tuketimi ilk istasyon/operasyon hareketlerine baglanir. Raporun kendisinde istasyon kirilimi yok (toplu).
**Notlar:** `myView1.MyKurusHane = 2`. Filtre stringleri parametresiz interpolation (injection riski). Acilistaki TempGuncelle Mikro DB'ye baglanamazsa hata mesaji gosterir ve rapor verisi gelmez. `myGrid1` kayit adi "UretimStokTuketimRaporuList1".

### Uretim Stok Tuketim Raporu Detayli (`MyUI/Raporlar/UretimRaporlari/FrmUretimStokTuketimRaporuDetayli.cs`)
**Ne ise yarar:** Yukaridaki raporun DETAYLI versiyonu: is emri (siparis) kodu, uretilen mamul stok kodu/adi ve tuketilen stok kirilimiyla gosterir. Toplu rapordan farki, satirlarin is emri + mamul + stok bazinda gruplanmasidir (hangi is emrinde hangi stoktan ne kadar harcandi).
**Once ne olmali (onkosul):** Toplu rapor ile ayni: saha uretim hareketleri girilmis ve Mikro DB erisilebilir olmali. Acilista `TempGuncelle()` ayni sekilde temp ve detay merge tablolarini gunceller.
**Sonra ne olur:** Salt-okunur. Acilista ayni yan etkiler (TempMikroStok/Kategori + IstasyonTakipStokHareketDetay merge). "Ara"da grid doldurulur, kayit sayisi `lblKayitSayisi`'de. Cift tik/odak event'leri devre disi (yorum).
**Butonlar & kisayollar:**
- `Ara` (`BtnAra`) — `Bagla()` ile detayli raporu calistirir (Designer'da `BtnAra.Click -> BtnAra_Click`, TabIndex=0).
- `Temizle` (`BtnTemizle`) / `Yazdir` / `Dizayn` / `Kapat` — taban form standart butonlari.
- Tarih1/2, Saat1/2, Kategori (`CmbKategori`), Kalite.Kont. (`CmbKaliteKontrol`), Reyon (`CmbReyon`) cok secimli filtreler, `Ara` (`TxtAra`) serbest metin.
- `Ara` metni — ana kolda is emri kodu / uretilen stok kodu-adi / stok kodu-adi LIKE; fire (alt) kolda siparis kodu / recete kodu-adi / stok kodu-adi LIKE.
**Cagirdigi katmanlar:**
- Service: `IIstasyonTakipStokHareketDetayService.GetListViewInKategoriDetayli(sor, sor2)` — UNION ALL: (1) `IstasyonTakipStokHareketDetay ITSHD` + `UretimEmri UR` + `TempMikroStok` (is emri/mamul/stok detayli, Tip='Fis'); (2) `IstasyonTakipHareketDetay ITHD` `Turu='FireStokGiris'` (Tip='Fire'). Is emri+mamul+stok+kategori bazinda GROUP BY.
- Service: `IIstasyonTakipStokHareketDetayService.DetaylarGuncelleToplu()` — acilista detay merge tablosunu yeniden hesaplar (toplu rapor ile ayni metot).
- Service: `ITempMikroStokService.MikroStokKategoriGuncelle / MikroStokGuncelle(mikroDb)` — Mikro temp tablolari guncelle.
- Service: `ITempMikroStokService.GetStokKategoriListStokKategori / GetStokKategoriListKaliteKontrol / GetStokReyonList()` — combo doldurma.
- SQL/Prosedur: servis ici inline UNION ALL + MERGE sorgulari (stored procedure degil).
- API: -
**Istasyon sirasiyla iliskisi:** Toplu rapor ile ayni — stok tuketim carpani `UrO.Sira=1` operasyonuna bagli hesaplanir; raporda is emri/mamul kirilimi var ama istasyon-sira kirilimi yok.
**Notlar:** `myView1.MyKurusHane = 2`. `myGrid1` kayit adi "UretimStokTuketimRaporuListDetayli1". Toplu rapordan tek farki cagirilan servis metodu (`GetListViewInKategoriDetayli`) ve arama kapsami; filtre/temp guncelleme mantigi aynidir.
