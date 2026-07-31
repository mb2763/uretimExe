## Istasyon Sirasi Nasil Olusur?

Bu bolum, UretimV4 (WinForms ERP) + ApiFeza + WebUretim TabletV2 ekosisteminde "bir uretim emrindeki operasyon/istasyon sirasinin nasil olustugunu, ilk adimin nasil acildigini, bir adim bitince sonrakinin nasil tetiklendigini ve bu akisin UI'ya nasil yansidigini" UC PROJE acisindan, koddan kanitli (dosya:satir) olarak belgeler. Ortak veritabani `UretimV3_FEZA`; miktar/akis motoru OPERASYON-Sira bazli SQL prosedurleridir (`Uretim_MiktarGuncelle`, `Uretim_PlanlananGuncelle`, `Uretim_SonrakiIstasyonaGonder`).

### Kavram haritasi (tablolar / entity'ler)

Sira ve akis su zincir uzerinde tasinir:

- **Recete (sablon) tarafi:**
  - `ReceteOperasyon` — operasyon sablonu. `Sira` (int) operasyonun receteyici sirasini tutar. Kanit: `My\Entities\Receteler\ReceteOperasyon.cs:32` (`public int Sira { get; set; }`), `:45` (`RcAId` = baglı recete). Ctor'da varsayilan `Sira=0` (`:16`).
  - `ReceteIstasyon` — bir operasyona bagli istasyon sablonu. `RcOId` ile operasyona, `RcAId` ile receteye baglanir; istasyonun kendi `Sira` alani YOKTUR. Kanit: `My\Entities\ReceteIstasyonlar\ReceteIstasyon.cs:33-34` (`RcAId`, `RcOId`), `:26-29` (`OperasyonKodu/IstasyonKodu`).
  - `ReceteIstasyonGrupOperasyon` — istasyon gruplama eslestirme tablosu: `GrupKodu + OperasyonKodu -> IstasyonKodu/IstasyonAdi`. Kanit: `My\Entities\ReceteIstasyonGruplar\ReceteIstasyonGrupOperasyon.cs:11-15` (`GrupKodu`, `OperasyonKodu`, `IstasyonKodu`, `IstasyonAdi`). Bu tabloda da `Sira` YOKTUR; eslestirme operasyon koduna gore yapilir.

- **Uretim (canli) tarafi:**
  - `UretimOperasyon` — uretim emrine kopyalanan operasyon. `Sira` (int) buradaki gercek akis sirasidir. Kanit: `My\Entities\UretimOperasyonlar\UretimOperasyon.cs:30` (`public int Sira { get; set; }`), `:35-39` (`UrId`, `RcAId`, `RcOId`, `SipId`, `SipHId`).
  - `UretimOperasyonHareket` — operasyonun "baslatilmis" hareketi (motorun isledigi satir). `Sira` alanini tasir (INSERT'lerde `Sira` set edilir; bkz. `_ProcedureListCreates.cs:145-146`).
  - `UretimOperasyonHareketDetay` — bir operasyon hareketinin acilis/giris detayi.
  - `UretimIstasyon` — operasyon hareketine bagli somut istasyon satiri. `Sira` alani YOKTUR; `RcOId`/`RcIstId` ile sablona, `UrOHId`/`UrOHDId` ile harekete baglanir. Kanit: `My\Entities\UretimIstasyonlar\UretimIstasyon.cs:36-43` (`UrId`,`UrOId`,`UrOHId`,`UrOHDId`,`RcAId`,`RcOId`,`RcIstId`,`SipId`). Istasyonda `Sira` olmamasi, "tek operasyon altinda cok istasyon" durumunda akis bozulmasinin temel sebebidir (asagida).

**Ozet:** Sira tek bir yerde, OPERASYON seviyesinde yasar (`ReceteOperasyon.Sira -> UretimOperasyon.Sira -> UretimOperasyonHareket.Sira`). Istasyonlarin kendi sirasi yoktur; istasyonlar daima icinde bulunduklari operasyonun sirasina tabidir.

---

### 1) Uretim emrinde operasyon/istasyon sirasi nasil belirlenir?

**Operasyon sirasi (ReceteOperasyon.Sira -> UretimOperasyon.Sira):**
Uretim emri siparisten olusturulurken, recetenin operasyonlari `UretimOperasyon` satirlarina kopyalanir ve `Sira`, sablondaki `ReceteOperasyon.Sira` degerinden birebir tasinir.

Kanit: `MyUI\UretimModule\FrmUretimEmriED.cs:199-232` (`OperasyonlarOlustur`):
- `:207` `foreach (var oprs in oprmod.Operasyonlar)` — recete operasyonlari uzerinde doner.
- `:218-219` `RcOId = oprs.Id`, `RcAId = oprs.RcAId` — sablon baglantilari.
- `:226` `Sira = oprs.Sira` — **operasyon sirasi sablondan kopyalanir.**

Operasyon listesi (sablon) `UretimEmriManager.GetOperasyon` ile yuklenir: `My\Business\Manager\UretimEmriManager.cs:543` (`_db.ReceteOperasyon.SelectList(c => c.RcAId == rcAId)`) ve istasyonlar `:546` (`_db.ReceteIstasyon.SelectList(c => c.RcAId == rcAId)`).

**Istasyon sirasi (operasyona baglilik):**
Istasyonlarin bagimsiz sirasi yoktur; bir operasyon hareketinin istasyonlari, o operasyonun `RcOId`'sine eslesen `ReceteIstasyon` kayitlarindan uretilir.

Kanit: `MyUI\UretimIstasyonModule\FrmUretimIstasyonED.cs:284-318` (`IstasyonlarOlustur`):
- `:286` `if (opr.Id == opm.RcOId)` — yalnizca gelen operasyon hareketinin operasyonu icin.
- `:290` `if (opr.Id == ist.RcOId)` — istasyon o operasyona bagliysa.
- `:292-316` bu sablon istasyondan bir `UretimIstasyon` satiri kurulur (`RcOId = ist.RcOId`, `RcIstId = ist.Id`).

**Istasyon gruplama (alternatif istasyon belirleme: GrupKodu + OperasyonKodu -> IstasyonKodu):**
Recetede `IstasyonGruplamaKullan = 1` ise, istasyonlar `ReceteIstasyon` yerine `ReceteIstasyonGrupOperasyon` eslestirmesinden gelir. Uretim emrine secilen `IstasyonGrupKodu` (UI'da `TxtIstasyonGrubu`) ile operasyon kodu eslestirilerek istasyon belirlenir. Bu eslestirme SQL motorunda yapilir.

Kanit: `My.DataBaseSettings\DatabaseCreate\UretimCreateModule\_ProcedureListCreates.cs:250` (`Uretim_SonrakiIstasyonaGonder` icindeki JOIN):
```
LEFT OUTER JOIN ReceteIstasyonGrupOperasyon GROP
   ON GROP.GrupKodu = UR.IstasyonGrupKodu AND GROP.OperasyonKodu = URO.OperasyonKodu
```
- `:243-245` secilen alanlar: `UR.IstasyonGrupKodu`, `URO.OperasyonKodu`, `GROP.IstasyonKodu`, `GROP.IstasyonAdi`.
- `:262` `WHERE RCA.IstasyonGruplamaKullan = 1` — bu otomatik istasyon olusturma yalnizca gruplama acik recetelerde calisir.

UI tarafinda grup kodu secimi: `FrmUretimEmriED.cs:421` (`_mdl.UretimEmri.IstasyonGrupKodu = TxtIstasyonGrubu.Text`) ve secim ekrani `:714-724` (`TxtIstasyonGrubu_ButtonClick` -> `FrmReceteIstasyonGrupIstasyonEslestir`).

---

### 2) Uretim emri kaydedilince ilk operasyon/hareket/istasyon nasil yaratilir?

**Adim A — Kayit (operasyon satirlari + miktar motoru):**
`UretimEmriKaydetBySiparis` tek transaction'da: uretim emrini + operasyonlari + stoklari yazar, ardindan `Uretim_MiktarGuncelle` calistirir.
Kanit: `My\Business\Manager\UretimEmriManager.cs:48` (`_db.UretimOperasyon.InsertOrUpdate(mdl.UretimOperasyonlar, trs)`), `:63` (`exec[Uretim_MiktarGuncelle] '...'`).

`Uretim_MiktarGuncelle` icinde sira mantigi: bir sonraki operasyonun planlanan miktari, bir oncekinin (Sira-1) uretim miktarindan turetilir (MERGE).
Kanit: `_ProcedureListCreates.cs:59-65`:
```
USING(SELECT UR.Id, coalesce((SELECT UretimMiktari FROM UretimOperasyon
   WHERE SipHId=UR.SipHId AND Sira = UR.Sira-1), UR.PlanlananMiktar) AS PlanlananMiktar
 FROM dbo.UretimOperasyon UR WHERE UR.Sira > 1 AND UR.UrId=@urid)
```
Yani `Sira>1` operasyonlar, ayni `SipHId` icindeki `Sira-1` operasyonun cikti miktarini "planlanan" olarak alir.

**Adim B — Uretimi baslatma (ilk operasyon hareketi):**
UI'da "Uretime Basla" akisinda yalnizca `Sira <= 1` operasyonlar icin `UretimOperasyonHareket` yaratilir (yani ilk operasyon baslatilir).
Kanit: `FrmUretimEmriED.cs:233-304` (`UretimBaslat`):
- `:241` `foreach (var oprs in _mdl.UretimOperasyonlar.Where(c => c.Sira <= 1))` — **sadece ilk sira basliyor.**
- `:243-257` ilk operasyona ait `UretimOperasyonHareket` olusturulur (`Sira = oprs.Sira`).
- `:293` `_mng.UretimOperasyonHareketKaydet(...)`.

`UretimOperasyonHareketKaydet`, hareketi yazip arkasindan iki motoru cagirir: `Uretim_MiktarGuncelle` + `Uretim_PlanlananGuncelle`.
Kanit: `UretimEmriManager.cs:156` (`exec[Uretim_MiktarGuncelle]`), `:162` (`exec[Uretim_PlanlananGuncelle]`).

**Adim C — Hareket/istasyon detaylarinin SQL ile acilmasi (`Uretim_PlanlananGuncelle`):**
`Uretim_PlanlananGuncelle` iki cursor ile calisir.
Kanit: `_ProcedureListCreates.cs:121-191`.

- **curs (ilk acilis):** Her `UretimOperasyon` icin, hareketi yoksa olusturur.
  - `:135-139` `curs` tanimi + ilk FETCH (`Sira` dahil okunur).
  - `:143-147` `if(@sira=1)` -> hareket yoksa **planlanan miktarli** `UretimOperasyonHareket` INSERT eder (ilk operasyon dolu baslar). `:145-146`'da `Sira`, `RcAId`, `RcOId` aktarilir.
  - `:148-153` `if(@sira>1)` -> hareket yoksa **sifir miktarli** hareket INSERT eder (sonraki operasyonlar bos/bekleyen baslar). `:152`'de tum miktarlar `0`.

  Bu, ilk operasyonun aktif (planlanan>0), sonraki tum operasyonlarin "0 planlanan" bekleyen olarak yaratilmasini saglar.

- **curs2 (sira N -> N+1 zincirleme planlama):** Bir operasyonun uretimi olunca bir sonraki sira operasyonun planlanan miktarini guncelle.
  - `:162-165` `curs2` tanimi + FETCH (`SipHId`, `Sira` dahil).
  - `:169-173` `if(@UretimMiktari > 0)` -> ayni `RcAId` + `SipHId` icinde `Sira = @sira + 1` operasyon hareketinin `PlanlananMiktar`/`KalanMiktar`'i, bir onceki operasyonun uretim miktarina esitlenir.
  - `:174-178` `if(@UretimMiktari <= 0)` -> sonraki sira sifirlanir.

**Adim D — Istasyon olusturma (gruplama acikken otomatik):**
`Uretim_PlanlananGuncelle` sonunda `Uretim_SonrakiIstasyonaGonder` cagrilir; bu prosedur `IstasyonGruplamaKullan=1` recetelerde otomatik olarak `UretimOperasyonHareketDetay` ("Acilis") + `UretimIstasyon` satirlari INSERT eder ve hareketi "islemde" yapar.
Kanit: `_ProcedureListCreates.cs:278-294` (HareketDetay "Acilis" INSERT), `:297-315` (`UretimIstasyon` INSERT, istasyon kodu/adi `ReceteIstasyonGrupOperasyon`'dan gelir), `:317` (`UPDATE UretimOperasyonHareket SET IslemdekiMiktar = @planlananMiktar`).

**Gruplama KAPALI ise istasyon manuel acilir:** Istasyonlar otomatik degil, kullanici tarafindan `FrmUretimIstasyonED` ile (asagida UI bolumu) eklenir. Bu durumda istasyon satirlari `IstasyonlarOlustur` (`FrmUretimIstasyonED.cs:272-346`) ile sablon `ReceteIstasyon`'dan kurulur ve `UretimOperasyonManager.KaydetNormal` ile yazilir.

---

### 3) Bir adim bitince sonraki adim nasil acilir?

Uc farkli mekanizma vardir; hangisinin calistigi receteye/UI'ya gore degisir.

**(a) SQL motorunda zincirleme planlama (Sira N -> N+1):**
Yukarida Adim C/curs2'de aciklandi. Bir operasyonun `UretimMiktari` artinca, ayni `RcAId + SipHId` icindeki `Sira+1` operasyon hareketinin planlanan miktari otomatik dolar.
Kanit: `_ProcedureListCreates.cs:169-173`.
Bu motor her uretim girisinde tetiklenir (cunku girisler `Uretim_MiktarGuncelle` + `Uretim_PlanlananGuncelle` cagirir; orn. `UretimEmriManager.cs:156-163`, `UretimOperasyonManager.cs:80-89`, `UretimIstasyonManager.cs:45-52`).

**(b) Istasyon gruplama otomatik sevki (`Uretim_SonrakiIstasyonaGonder`):**
`IstasyonGruplamaKullan=1` recetelerde, planlanan>0 ve "henuz islemde olmayan" her operasyon hareketi icin (curs) yeni acilis detayi + istasyon satiri yaratir, operasyonun istasyon kodu `ReceteIstasyonGrupOperasyon` eslestirmesinden gelir.
Kanit: `_ProcedureListCreates.cs:225-265` (cursor + JOIN'lar), filtre `:262-265`:
```
WHERE RCA.IstasyonGruplamaKullan = 1
  AND UROH.PlanlananMiktar > 0
  AND UROH.PlanlananMiktar > UROH.IslemdekiMiktar
  AND UROH.UrId = @urId;
```
Boylece (a) sonraki operasyonun planlananini doldurur, (b) bu yeni dolan operasyonu otomatik dogru istasyona "gonderir" (acar).

**(c) Manuel "Sonraki Operasyona Gecir" (WinForms UI):**
Gruplama yokken kullanici, bir operasyon hareketinden sonraki operasyona elle gecirir. `FrmUretimIstasyonED` `SonrakiOperasyon` modunda calisir.
Kanit: `FrmUretimIstasyonED.cs:142-231` (`SonrakiOperasyonBaslat`):
- `:151-157` bu operasyon hareketinin operasyonu (`OncekiOperasyon`) bulunur.
- `:158` `int sira = OncekiOperasyon.Sira + 1;` — **sonraki sira hesabi.**
- `:159` `_srvOperasyon.SelectFirst(c => c.UrId==... && c.SipHId==OncekiOperasyon.SipHId && c.Sira==sira)` — ayni `SipHId` icinde `Sira+1` operasyon.
- `:172-192` sonraki operasyon icin yeni `UretimOperasyonHareket` (`Sira = SonrakiOperasyon.Sira`).
- `:208` `HareketYeni.PlanlananMiktar = oncedenUretilen - operasyondaolan;` — devredilecek miktar.
- Kayit: `KaydetSonraki` -> `UretimOperasyonManager.KaydetSonraki` (`UretimOperasyonManager.cs:109-179`): yeni hareketi Insert eder, detay/istasyonlari yeni harekete bağlar, `Uretim_MiktarGuncelle` calistirir (`:159`).

**(d) TabletV2 istasyon sevki (`IstasyonSevkManager`):**
TabletV2 (Blazor saha tableti) tarafinda "sonraki istasyona gonderme", operasyon sira motoru ile DEGIL, dogrudan `UretimIstasyon.IstasyonKodu` degistirilerek yapilir; is emri elle secilen hedef istasyona tasinir.
Kanit: `WebUretim TabletV2\My\Business\Managers\IstasyonSevkManager.cs:63-90` (`IstasyonSevkKaydet`):
- `:77-79` mevcut takip hareketi hedef istasyona gore guncellenir, `Durumu="Durduruldu"`.
- `:82` `update UretimIstasyon set IstasyonKodu=@IstasyonKodu, IstasyonAdi=@IstasyonAdi where Id=@Id`.
- `:84-88` tek transaction'da: INSERT(IstasyonTakipHareket) + UPDATE(UretimIstasyon) + INSERT(log).

Bekleyen (takip baslamamis) is emri icin: `:43-58` (`IstasyonBekleyenSevkKaydet`) — yalnizca `UretimIstasyon` istasyon kodu UPDATE + log INSERT (`:50-55`). Bu modulde uretim/miktar motoru (`Uretim_*` prosedurleri) cagrilmaz; sadece istasyon atamasi degisir (bkz. TabletV2 docs `_sections\01_IsEmirleri.md:3`, `:55`).

**Onemli fark:** (a)/(b) otomatik sira tabanli akistir; (c) operasyon sira tabanli manuel akistir; (d) sira motoru ile ilgisiz, istasyondan istasyona elle tasimadir.

---

### 4) Tek operasyon altinda cok istasyon olursa akis sorunu ("YOK uyarisi" / kisitlama)

Sira yalnizca OPERASYON seviyesinde tutuldugu, ISTASYON'da `Sira` alani olmadigi (`UretimIstasyon.cs` icinde `Sira` yok) icin, "ayni operasyon altinda birden cok istasyonu sirayla calistirma" otomatik akisi YOKTUR. Sistem bunu su kisitla onler: istasyon gruplama kullanilacaksa uretim emrine TEK recete girilmesini zorunlu kilar.

Kanit: `MyUI\UretimModule\FrmUretimEmriED.cs:471-486` (`TextLeriKontrolEt`):
```
foreach (var itm in _mdl.ReceteModeller) {
  if (itm.Recete.IstasyonGruplamaKullan) {
    if (rcaid == Guid.Empty) rcaid = itm.Recete.Id;
    else if (rcaid != itm.Recete.Id) {
      MesajHata("İstasyon Gruplama Kullanılacaksa Tek Reçete Seçilebilir. Üretime Lütfen Tek Reçete Giriniz.");
      return false;
    }
  }
}
```
Ek olarak `:489-499` gruplama acikken `IstasyonGrupKodu` (grup kodu) bos birakilamaz: `MesajHata("Lütfen İstasyon Grubu Seçiniz.")`.

`Uretim_SonrakiIstasyonaGonder` icindeki `ReceteIstasyonGrupOperasyon` JOIN'i `OperasyonKodu` uzerinden tek istasyon dondurmeye dayanir (`_ProcedureListCreates.cs:250`); ayni grup+operasyon icin birden cok istasyon eslesirse cursor satirlari cogalir ve otomatik akis belirsizlesir. Gruplama KAPALI ve bir operasyona birden cok `ReceteIstasyon` bagliysa, istasyonlar paralel olarak ayni operasyon hareketine baglanir (`IstasyonlarOlustur`, `FrmUretimIstasyonED.cs:288-318`) ancak aralarinda otomatik bir sira/gecis tetiklemesi tanimli degildir; gecis yalnizca OPERASYON degisince (Sira+1) olur. Yani "tek operasyon - cok istasyon" senaryosunda istasyonlar arasi otomatik sevk akisi yoktur; pratikte her istasyon adimi ayri bir OPERASYON olarak modellenmelidir.

---

### 5) Bilinen hata: `_ProcedureListCreates.cs:186` dinamik EXEC (varchar + uniqueidentifier)

`Uretim_PlanlananGuncelle` sonunda `Uretim_SonrakiIstasyonaGonder` cagrilirken dinamik EXEC kullanilmis ve string birlestirme hatalidir.

Kanit: `_ProcedureListCreates.cs:186`:
```
begin EXEC ('EXEC [Uretim_SonrakiIstasyonaGonder] '''+@uridsi+' ;'''); END
```
Hemen altinda `:187` dogru (statik) cagrinin yorum satirina alindigi gorulur:
```
/*  BEGIN  EXEC [Uretim_SonrakiIstasyonaGonder]  @uridsi ; END*/
```

**Sorunlar:**
1. **Tip hatasi (varchar + uniqueidentifier):** `@uridsi` `uniqueidentifier` (`:125`) tipindedir; `'...' + @uridsi` ifadesi string ile uniqueidentifier'i dogrudan `+` ile birlestirmeye calisir. SQL Server bunu implicit cast etmez, "Conversion failed / operand type clash" benzeri hata uretir. Dogru kullanim `CAST(@uridsi AS varchar(36))` olmaliydi.
2. **Tirnak yerlesimi bozuk:** Uretilmek istenen metin muhtemelen `EXEC [Uretim_SonrakiIstasyonaGonder] '<guid>';` olmali; ancak `'''+@uridsi+' ;'''` ifadesi tirnaklari yanlis yerlestirir — kapanis tirnagi GUID'den sonra degil ` ;` ifadesinden sonra gelir, yani olusan komut hem tip olarak hem sozdizimi olarak gecersizdir.

**Etki:** Bu dinamik EXEC calistiginda `Uretim_PlanlananGuncelle` icinden `Uretim_SonrakiIstasyonaGonder` otomatik tetiklenmesi basarisiz olur (hata firlatir veya — cagiran katmanda yutuluyorsa — sessizce calismaz). Yorumdaki statik cagri (`:187`) parametre tipi acisindan dogru olan biçimdir; dinamik EXEC yerine onun kullanilmasi gerekir. Bu satir, "istasyon gruplama otomatik sevki" akisinin (Bolum 3-b) beklenen sekilde calismamasinin dogrudan kaynagidir.

---

### 6) Istasyon sirasi UI'da kullaniciya nasil yansir?

**UretimV4 (WinForms):**

- **Operasyon listesi + sira:** `FrmUretimEmriED` ana grid'i operasyonlari (`UretimOperasyon`) gosterir; `Sira` kolonu gorunur (gizlenen kolonlar yalnizca Id/FK'lardir: `FrmUretimEmriED.cs:326-335`). Operasyon durumu renkle yansir: "Hazir" koyu yesil, "Uretimde" indigo (`:693-712`, `GridView_RowStyle`). Bu `Durumu`, `Uretim_MiktarGuncelle`/`Uretim_DurumGuncelle` icinde hesaplanir (`_ProcedureListCreates.cs:69-75`, `:99-105`: `Hazir`/`Uretimde`/`Beklemede`).
- **Ilk adim kisiti:** Sonraki bir operasyona istasyon eklenmek istendiginde, onceki operasyon baslatilmamissa engellenir: `MyUI\UretimModule\FrmOperasyonTakipDetaylar.cs:157` ve `:252` `MesajBilgi("Operasyon Baslatilmamis İşlem Yapılamaz Bir önceki operasyondan işlem yapınız")`. Bu, sira disi istasyon islemini onler.
- **Istasyon ekrani (`FrmUretimIstasyonED`):** Uc mod ile acilir — `OperasyonTuruEnum` = `SonrakiOperasyon`/`IstasyonEkle`/`Degistir` (`MyUI\UretimOperasyonModule\OperasyonTuruEnums.cs:9-14`). `IstasyonEkle` ile gelinir (`FrmOperasyonTakipDetaylar.cs:164`, `:259`), istasyonlar `IstasyonlarOlustur` ile operasyona bagli sablondan listelenir (`FrmUretimIstasyonED.cs:272-346`). "Sonraki operasyona gecir" sira+1 hesabiyla yapilir (`:158-159`) ve devreden miktar kullaniciya planlanan olarak gosterilir (`:242`, `cnt.Miktar = HareketYeni.PlanlananMiktar`).
- **Miktar/durum geri bildirimi:** Her kayit sonrasi motorlar cagrildigi icin (`Uretim_MiktarGuncelle`/`Uretim_PlanlananGuncelle`) operasyon/istasyon/uretim emri/siparis durumlari ust seviyeye dogru guncellenir (`_ProcedureListCreates.cs:77-84` UretimEmri ve Siparis durumu).

**TabletV2 (Blazor saha tableti):**

- Is emirleri OPERASYON sirasina gore degil, atandiklari `IstasyonKodu`'na gore gruplanip kart panosunda gosterilir; her kart bir istasyondur. Kanit: TabletV2 docs `_sections\01_IsEmirleri.md:22` ("Is emirleri `IstasyonKodu` alanina gore gruplanir; her kart bir istasyonu temsil eder. Operasyon sira/akis motoru burada tetiklenmez").
- Her istasyon kartinda is emirleri durum renkleriyle gosterilir: Aktif (yesil), Durdurulan (kirmizi), Bekleyen (mavi). Kanit: `_sections\01_IsEmirleri.md:26`.
- Kullanici "Sevk" ile is emrini elle baska istasyona tasiyabilir (sira motoru devreye girmez); hedef istasyon combo'dan secilir. Kanit: `_sections\01_IsEmirleri.md:41`, `:55` ve kod `IstasyonSevkManager.cs:63-90`.
- Bu modulde uretim/miktar motoru cagrilmaz; istasyon sirasi UI'da yalnizca "is emrinin su an hangi istasyonda oldugu" olarak yansir, otomatik sonraki-istasyon gecisi (gruplama akisi) WinForms+SQL tarafinda kalir. Kanit: `_sections\01_IsEmirleri.md:3`.

---

### Kanit ozeti (dosya:satir)

- Operasyon sirasi sablonu: `My\Entities\Receteler\ReceteOperasyon.cs:32`, `:45`.
- Istasyon-operasyon baglantisi (sira yok): `My\Entities\ReceteIstasyonlar\ReceteIstasyon.cs:33-34`; `My\Entities\UretimIstasyonlar\UretimIstasyon.cs:36-43`.
- Grup eslestirme: `My\Entities\ReceteIstasyonGruplar\ReceteIstasyonGrupOperasyon.cs:11-15`.
- Uretim operasyon sirasi: `My\Entities\UretimOperasyonlar\UretimOperasyon.cs:30`.
- Sira kopyalama (sablon->uretim): `MyUI\UretimModule\FrmUretimEmriED.cs:218-226`.
- Ilk operasyon baslatma (Sira<=1): `MyUI\UretimModule\FrmUretimEmriED.cs:241-257`.
- Tek-recete kisiti / grup kodu zorunlu: `MyUI\UretimModule\FrmUretimEmriED.cs:474-499`.
- Ilk acilis cursor'u (Sira=1 dolu / Sira>1 sifir): `_ProcedureListCreates.cs:143-153`.
- Sira N->N+1 planlama (curs2): `_ProcedureListCreates.cs:169-178`.
- Sonraki operasyon planlamasi (MERGE Sira-1): `_ProcedureListCreates.cs:59-65`.
- Otomatik istasyon sevki (gruplama) + grup JOIN: `_ProcedureListCreates.cs:250`, `:262-265`, `:278-317`.
- Dinamik EXEC hatasi: `_ProcedureListCreates.cs:186` (yorumlu dogru cagri `:187`).
- Manuel sonraki operasyon (Sira+1) UI: `MyUI\UretimIstasyonModule\FrmUretimIstasyonED.cs:158-159`, `:172-208`.
- KaydetSonraki/KaydetNormal motorlari: `My\Business\Manager\UretimOperasyonManager.cs:80-89`, `:109-167`.
- Motor cagrilari (her giriste): `UretimEmriManager.cs:63`, `:156-163`; `UretimIstasyonManager.cs:45-52`.
- TabletV2 elle istasyon sevki: `WebUretim TabletV2\My\Business\Managers\IstasyonSevkManager.cs:43-58`, `:63-90`.
- TabletV2 UI yansimasi: `WebUretim TabletV2\docs\_sections\01_IsEmirleri.md:3`, `:22`, `:26`, `:41`, `:55`.
