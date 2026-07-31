# UretimV4 (CepPatronERP) — Kapsamli Sayfa/Islem Analizi

WinForms masaustu ERP. Ofis/yonetim: uretim emri, recete, istasyon, operasyon, mal kabul, kontrol, raporlama. DB'ye dogrudan baglanir (UretimV3_FEZA + MikroDB_V16_FEZA24).

> Bu dokuman otomatik analizle uretilmistir. Her sayfa/islem icin: ne yapar, oncesinde ne olmali (onkosul), sonrasinda ne olur, butonlar & kisayollar, cagirdigi manager/servis/SQL/endpoint, istasyon sirasiyla iliskisi.

## Icindekiler
- Istasyon Sirasi Nasil Olusur?
- Modul: UretimModule
- Modul: UretimIstasyonModule
- Modul: UretimOperasyonModule
- Modul: OperasyonModul
- Modul: IstasyonModul
- Modul: IstasyonHareketlerModul
- Modul: ReceteModul
- Modul: ReceteIstasyonGrupModul
- Modul: MalKabul
- Modul: UretimKontroller
- Modul: UretimTalepler
- Modul: MikroModul
- Modul: SiparisModule
- Modul: KullaniciModul
- Modul: PersonelModul
- Modul: AyarVeGenelModul
- Modul: MailModul
- Modul: SmsModule
- Modul: Aciklamalar
- Modul: Raporlar
- Modul: HizliUretimModule
- Modul: Ana Kabuk, Login, Yetki ve DB Yonetimi

---

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
## Modul: OperasyonModul

OperasyonModul, uretim akisinin temel yapi tasi olan "operasyon" tanimlarinin (orn. Kesim, Kaynak, Boya, Montaj) kart bazli yonetimini saglar. Klasorde tek bir ekran vardir: `FrmOperasyonKartlari`. Bu ekran operasyon kodu/adini ve her operasyona atanmis "varsayilan istasyon" bilgisini tutar. Operasyon kartlari, recete tarafinda operasyon sirasi (ReceteOperasyon) ve istasyon eslestirme (ReceteIstasyon / ReceteIstasyonGrupOperasyon) kurulurken referans liste olarak kullanilir; dolayisiyla miktar/akis motorunun OPERASYON-Sira bazli calismasinin sozluk/temel veri katmanidir. Ekran iki modda calisir: (1) ana menuden ShowDialog ile acilan klasik CRUD karti, (2) recete operasyon ekleme ekranindan `SecimIcinAcildi=true` ile acilan secim diyalogu.

### Operasyon Kartlari (`FrmOperasyonKartlari.cs` / `FrmOperasyonKartlari.Designer.cs`)
**Ne ise yarar:** Operasyon kartlarinin (OperasyonKodu, OperasyonAdi ve VarsayilanIstasyonKodu/Adi) eklenmesi, duzenlenmesi, silinmesi ve listelenmesi. Ayrica recete operasyon editorunden cagrildiginda operasyon secim diyalogu olarak gorev yapar.
**Once ne olmali (onkosul):** Istasyon kartlari onceden tanimlanmis olmali; cunku formun acilisinda `IstasyonKarti` listesi cekilip "Varsayilan Istasyon Kodu/Adi" lookup'larina (CmbIstasyonKodu/CmbIstasyonAdi) baglaniyor. Ekran ana menuden `FromAc.OperasyonKartlari()` ile `ShowDialog` olarak acilir. Veri tabani baglantisi (`Ortak.DbPro`) hazir olmali.
**Sonra ne olur:**
- Kaydet: `OperasyonKarti` tablosuna kayit eklenir/guncellenir (`InsertOrUpdate`). Eger duzenleme sirasinda OperasyonKodu degistiyse ayni transaction icinde su tablolarda eski kod yeni kodla (ve OperasyonAdi ile) toplu UPDATE edilir: `IstasyonKarti` (Operasyon, OperasyonAdi), `ReceteOperasyon`, `ReceteIstasyon`, `ReceteIstasyonGrupOperasyon`, `UretimOperasyon`, `IstasyonTakipHareket`. Liste yeniden yuklenir (`Bagla()`), text alanlar temizlenir.
- Sil: Secili `OperasyonKarti` satiri `OperasyonKarti` tablosundan silinir (`_srv.Delete`), liste yenilenir.
- Secim modunda (SecimIcinAcildi): satira cift tiklayinca/Enter ile secilen operasyon `SecilenRow/SecilenKod/SecilenId` ile cagiran ekrana (orn. `FrmReceteOperasyonED`) doner ve form kapanir; orada ReceteOperasyon satiri olarak eklenir.
- Ekran disari (kaydet/sil oldugunda) `KayitEdildi=true` set eder.
**Butonlar & kisayollar:**
- `Kaydet (BtnKaydet)` — `Kaydet()` cagirir: text dogrulamasi (OperasyonKodu bos olamaz), modele aktarim, `OperasyonManager.Kaydet`. (Base form: alt buton seridinde, genelde Enter/F-tusu base davranisi.)
- `Yeni (BtnYeni)` — `YeniKayit=true` yapar, text alanlari temizler (`TemizleText`).
- `Duzenle (BtnDuzenle)` — Grid'de secili satiri (`MyGetCurrentItem<OperasyonKarti>`) klonlayip text alanlara aktarir (`AktarTextlere`), `YeniKayit=false`.
- `Sil (BtnSil)` — Onay sorar ("Kaydi silmek istiyormusunuz.."), secili kayit yoksa uyarir, sonra `Sil()`.
- `Kapat (BtnKapat)` — Base form: formu kapatir.
- `Yazdir (BtnYazdir)` — Base formdan gelir (bu ekranda ozel kod baglanmamis).
- Navigasyon `BtnIlk / BtnOnceki / BtnSonraki / BtnSon` — Base form kayit gezinme butonlari (BindingSource `bs` uzerinde).
- Grid cift tiklama / Enter (`myView1.MyEventDoubleClickEnter`) — Secim modundaysa satiri secip kapatir; degilse `BtnDuzenle.PerformClick()` tetikler.
- Lookup `CmbIstasyonKodu/CmbIstasyonAdi` Leave — biri secilince digeri otomatik doldurulur (kod<->ad esleme), `AcilisBittimi` bayragiyla recursion engellenir.
**Cagirdigi katmanlar:**
- Manager/Service: `OperasyonManager.Kaydet(OperasyonKarti, bool yenikayit)` — yeni/guncelleme ayrimi yapar, `KodVarmi` ile ayni OperasyonKodu kontrolu yapar, transaction acar, `Service.InsertOrUpdate` ile ana kaydi yazar, kod degisikliginde bagimli 6 tabloyu UPDATE eder, commit eder.
- Manager/Service: `OperasyonManager.KodVarmi<T>(...)` — tablo adi/anahtar kolonu reflection ile bulup `Select count(*)` ile ayni kodun varligini denetler ("Aynı OperasyonKodu Kodla Kayıt Var").
- Manager/Service: `IOperasyonKartiService` (= `OperasyonKartiService : BaseService<OperasyonKarti>`) metotlari: `SelectListWhere(" Order By OperasyonKodu")` (liste), `SelectFind(Id)` (eski kaydi bulma), `InsertOrUpdate`, `Delete`, `GetConnection`, `Execute`, `Query<int>`.
- Manager/Service: `IIstasyonKartiService.SelectListWhere(" Order By IstasyonKodu ")` — varsayilan istasyon lookup verisi.
- Manager/Service: `IGenelService` (`_srvGenel`) — enjekte edilmis fakat bu ekranda aktif kullanim yok.
- DataAccess: `OperasyonKartiDal : BaseDal<OperasyonKarti>` — `[Table("OperasyonKarti")]` POCO uzerinden Dapper CRUD.
- SQL/Prosedur: Stored procedure cagrilmaz. Kod degisikliginde inline UPDATE'ler (yukarida listelenen 6 tablo). Sema: `OperasyonKartiCreates.OperasyonKartiCreate()` -> OperasyonKarti tablosu (OperasyonKodu varchar(50), OperasyonAdi varchar(150), VarsayilanIstasyonKodu varchar(50), VarsayilanIstasyonAdi varchar(150)).
- API: - (UretimV4 dogrudan DB'ye baglanir, API kullanmaz.)
**Istasyon sirasiyla iliskisi:** Operasyon kartlari, miktar/akis motorunun OPERASYON-Sira bazli isleyisinde temel sozluktur. Recetede operasyon sirasi (ReceteOperasyon.Sira) ve operasyon->istasyon eslestirmeleri (ReceteIstasyon, ReceteIstasyonGrupOperasyon) bu kartlardaki OperasyonKodu uzerine kurulur. Her operasyon icin tanimlanan "VarsayilanIstasyonKodu", recete operasyon ekleme sirasinda otomatik istasyon eslestirmesi onerisinde kullanilir (bkz. FrmReceteOperasyonED). OperasyonKodu degisince ReceteIstasyonGrupOperasyon ve UretimOperasyon dahil tum akis tablolari guncellenerek sira/akis butunlugu korunur.
**Notlar:**
- OperasyonManager.Kaydet'teki bagimli tablo UPDATE'leri string interpolation ile kuruldugu icin SQL injection ve OperasyonAdi/Kodu icindeki tek tirnak riski tasir (gercek davranis; spekulasyon degil, koddan).
- Kaydetme hatasinda transaction `trs.Dispose()` ile birakiliyor ancak bazi dallarda explicit `Rollback` cagrilmiyor (finally'de yine Dispose var).
- Form base sinifi `MyFrmKayit` (My.Kontrol.Formlar, harici DLL) — alt buton seridi, navigasyon, `bs` BindingSource, `IdGuid/YeniKayit/KayitEdildi/AcilisBittimi/SecimIcinAcildi/Secildi/SecilenKod/SecilenRow/SecilenId` ve `MesajHata/MesajSor/MesajBilgi/MesajBilgi` mesaj yardimcilarini saglar; bu ekranda Enter=Kaydet/Esc=Kapat gibi kisayollar Designer'da acikca tanimlanmamis (base formdan gelir).
- `BtnDuzenle.Click` event'i `BtnDegistir_Click` metoduna baglidir (isim farki sadece kozmetik).
- Grid yerlesimi `myGrid1.MyGridKayitAdi = "OperasyonKartlariListesi"` adiyla saklanir/yuklenir; "Id" kolonu gizlenir (`SutunGizle`).
## Modul: IstasyonModul

IstasyonModul, uretim akisinin temel yapi taslarindan biri olan **istasyon kartlarini** (uretim duraklari/operasyon noktalari), istasyonlara ait **bakim/ariza kayitlarini** ve saha tabletinde operatorun karsisina cikan **acilir aciklama listelerini** (baslatma hatasi, durdurma kodu, fire sebebi) yonetir. Tum veriler Uretim DB'sinde (UretimV3_FEZA) tutulur: `IstasyonKarti`, `IstasyonBakim`, `IstasyonBakimParca`, `IstasyonAciklama` tablolari. Istasyon kartlari bir **Operasyon** koduna baglanir; bu baglanti, recete-istasyon esleme (ReceteIstasyonGrupOperasyon) ve uretim akisinin (UretimIstasyon olusturma) merkezindedir. Istasyon kart kodu degistirildiginde modul, kodu uretim zincirindeki tum bagli tablolara (ReceteIstasyon, ReceteIstasyonGrupOperasyon, UretimIstasyon, IstasyonTakipHareket, IstasyonKontrol) tek bir transaction icinde yayar. Modulde 6 form vardir: 3'u ana sayfa (Kart Listesi, Kart Kaydi, Bakim Listesi) ve 3'u yardimci/popup (Bakim Ekle, Bakim Parca Ekle, Aciklama Tanimlari). Formlar `My.Kontrol.Formlar` namespace'indeki base siniflardan turer: `MyFrmListe` (arama panelli liste ekranlari), `MyFrmKayit` (ust bilgi + grid'li kayit ekranlari, alt buton seridi Kaydet/Sil/Yeni/Duzenle/Kapat icerir) ve `MyFrmSade` (sade popup, Kaydet/Kapat). Grid'lerde cift tiklama/Enter ortak `MyEventDoubleClickEnter` olayini tetikler (`SecimIcinAcildi` ise satiri secip kapatir, degilse Duzenle'yi calistirir).

### Istasyon Kart Listesi (`FrmIstasyonKartList.cs` / `FrmIstasyonKartList.Designer.cs`)
**Ne ise yarar:** Istasyon kartlarini operasyona gore filtreleyip listeleyen, esas olarak **secim diyalogu** olarak kullanilan liste ekrani. Recete-istasyon grubu operasyon eslestirme ve uretim istasyonu duzenleme ekranlarindan "istasyon sec" amaciyla acilir. `MyFrmListe` turevi (sol arama paneli + grid).
**Once ne olmali (onkosul):** Istasyon kartlari (`IstasyonKarti`) ve operasyon kartlari (`OperasyonKarti`) tanimli olmali. Secim modunda acan ekran (orn. `FrmReceteIstasyonGrupOperasyonEslestir`, `FrmUretimIstasyonED`) `SecimIcinAcildi=true`, opsiyonel `Aranan` (operasyon kodu) ve `RcAId` (recete ana Id) atar.
**Sonra ne olur:** Veri tabaninda DEGISIKLIK YAPMAZ (salt okuma/secim). Satira cift tiklaninca `SecilenKod=IstasyonKodu`, `SecilenRow=satir`, `Secildi=true` set edilip form kapanir; cagiran ekran secilen istasyonu alir.
**Butonlar & kisayollar:**
- `Ara` (`BtnAra`) — `Bagla()` cagirir; operasyon filtresine gore listeyi yeniden ceker.
- `Temizle` (`BtnTemizle`) — `TemizleText()`; operasyon arama kutusunu bosaltir.
- `Tümü` (`ChcTumu` onay kutusu) — isaretli ise tum istasyonlar; isaretsiz ve `RcAId` verilmisse yalnizca o receteye bagli istasyonlar (`ReceteyeBagliIstasyon.RcIId` ile eslesen) gosterilir.
- `Operasyon` (`CmbOperasyon` lookup) — operasyon koduna gore filtre.
- Grid cift tiklama / Enter — secim modunda satiri secip kapatir (`MyView1_MyEventDoubleClickEnter`).
- `Kapat` (`BtnKapat`, base) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IIstasyonKartiService.SelectListWhere(sor)` — operasyon filtresine gore istasyon kartlarini ceker (`_srv`, `Ortak.DbPro.IstasyonKarti`).
- Manager/Service: `IReceteyeBagliIstasyonService.SelectListWhere("where RcAId=...")` — `RcAId` verildiyse receteye bagli istasyon Id listesini ceker (`_srvBIst`), "Tümü" isaretsizken filtre icin kullanilir.
- Manager/Service: `IOperasyonKartiService.SelectListWhere(" Order By OperasyonKodu")` — operasyon combo'sunu doldurur (`_srvOpr`).
- SQL/Prosedur: Dinamik `where 1=1 AND Operasyon ='<deger>` (SorguAyarla) — NOT: tek tirnak kapanisi eksik, basit string birlestirme.
- API: -
**Istasyon sirasiyla iliskisi:** Dolayli. Secilen istasyon, recete operasyon-grup eslestirmesinde veya uretim istasyonu duzenlemede kullanilir; bu da uretim akisinin (operasyon Sira sirasi -> UretimIstasyon) kurulmasini etkiler.
**Notlar:** Namespace `MyUI.IstasyonModul`. `BagliIstasyonSec` bayragi tanimli ama mantikta dogrudan kullanilmiyor (cagiran taraf set ediyor). Grid duzeni `IstasyonKartlariSelectListesi` adiyla saklanir (`MyGridKayitAdi`). `Id` sutunu gizlenir.

### Istasyon Kartlari (Kayit) (`FrmIstasyonKartlari.cs` / `FrmIstasyonKartlari.Designer.cs`)
**Ne ise yarar:** Istasyon kartlarinin tam CRUD ekrani (ekle/duzenle/sil). Her istasyon bir operasyona, opsiyonel olarak bir fason cariye baglanir; kalite kontrol, yazdirilmali, fason bayraklari tutulur. `MyFrmKayit` turevi (ust GroupControl form + alt grid).
**Once ne olmali (onkosul):** Operasyon kartlari (`OperasyonKarti`) tanimli olmali (operasyon kodu/adi combo'lari icin). Fason cari secimi icin Mikro DB'de cari kartlari (`MikroCari`) bulunmali.
**Sonra ne olur:** Kaydet -> `IstasyonManager.Kaydet()` tek transaction'da: (1) `IstasyonKarti` tablosuna InsertOrUpdate; (2) **istasyon kodu degistirilmisse** ayni transaction icinde `ReceteIstasyon`, `ReceteIstasyonGrupOperasyon`, `UretimIstasyon`, `IstasyonTakipHareket`, `IstasyonKontrol` tablolarindaki eski koda ait IstasyonKodu/IstasyonAdi alanlarini yeni degerle gunceller. Sil -> `IIstasyonKartiService.Delete()`. Islem sonrasi liste `Bagla()` ile yenilenir; form acik kalir.
**Butonlar & kisayollar:**
- `Kaydet` (`BtnKaydet`, base) — `Kaydet()` -> dogrulama (kod zorunlu) -> `AktarModele()` -> `IstasyonManager.Kaydet(mdl, YeniKayit)` -> `Bagla()`.
- `Sil` (`BtnSil`, base) — onay sorar, secili kayit yoksa uyarir, `Sil()` -> `_srv.Delete(_mdl)`.
- `Yeni` (`BtnYeni`, base) — `YeniKayit=true`, `TemizleText()` ile formu temizler.
- `Düzenle` (`BtnDuzenle`, base) — secili grid satirini klonlayip forma yukler (`AktarTextlere`), `YeniKayit=false`.
- `Bagla` (`BtnBagla`) — listeyi yeniden baglar (`Bagla()` + sutun gizle + grid yerlesimi yukle).
- Fason Cari Kodu/Adi (`TxtFasonCariKodu`/`TxtFasonCariAdi`) buton tiklamasi — `FrmMikroCariListesi` secim diyalogunu acar, secilen carinin kodu/unvani alanlara aktarilir.
- `Operasyon` / `Operasyon Adi` (`CmbOperasyon`/`CmbOperasyonAdi` lookup) — biri secilince digeri otomatik dolar (Leave olaylari).
- `Fason` / `KaliteKontrol` / `Yazdırılmalı` (`ChcFason`/`ChcKaliteKontrol`/`myChcYazdir`) — istasyon ozellik bayraklari.
- Grid cift tiklama / Enter — secim modunda satiri secip kapatir, degilse `BtnDuzenle.PerformClick()`.
- `Kapat` (`BtnKapat`, base) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IstasyonManager.Kaydet(IstasyonKarti mdl, bool yenikayit)` — kod tekillik kontrolu (`KodVarmi`) + transaction'li kayit + kod degisikligini bagli uretim tablolarina yayma.
- Manager/Service: `IstasyonManager.KodVarmi(...)` — `Select count(*) From IstasyonKarti ...` ile ayni IstasyonKodu var mi kontrolu.
- Manager/Service: `IIstasyonKartiService` — `SelectListWhere`, `SelectFind`, `InsertOrUpdate`, `Delete`, `Execute` (DAL temel metotlari).
- Manager/Service: `IOperasyonKartiService.SelectListWhere(" Order By OperasyonKodu")` — operasyon combo'lari.
- SQL/Prosedur: `UPDATE ReceteIstasyon / ReceteIstasyonGrupOperasyon / UretimIstasyon / IstasyonTakipHareket SET IstasyonKodu, IstasyonAdi ...` ve `UPDATE IstasyonKontrol SET IstasyonKodu ...` — istasyon kodu yeniden adlandirma yayilimi.
- API: -
**Istasyon sirasiyla iliskisi:** Dogrudan ve kritik. Istasyon-operasyon eslesmesini burada kurulan kart belirler; uretim akisi operasyon `Sira`'sina gore ilerlerken `ReceteIstasyonGrupOperasyon` (GrupKodu+OperasyonKodu->IstasyonKodu) ile her operasyona TEK `UretimIstasyon` olusturulur. Kart silme/yeniden adlandirma bu zinciri etkiledigi icin Manager kod degisimini tum bagli tablolara senkronlar.
**Notlar:** Namespace `MyUI.UretimIstasyonModule` (dosya IstasyonModul klasorunde olsa da). Grid duzeni `IstasyonKartlariListesi`. `AcilisBittimi` bayragi combo Leave olaylarinin acilis sirasinda tetiklenmesini engeller. `BtnDegistir_Click`/`BtnYeni_Click` event'leri `EventlerBagla()` icinde kod ile baglanir (Designer'da degil).

### Istasyon Aciklamalari (`FrmIstasyonAciklamalari.cs` / `FrmIstasyonAciklamalari.Designer.cs`)
**Ne ise yarar:** Saha tabletinde operatorun secebilecegi **kodlu aciklama listelerini** yonetir; tek form, `AciklamaModulTuru` enum'una gore 3 farkli amac icin acilir: **Istasyon Baslatma Hatasi**, **Istasyon Durdurma Kodu**, **Istasyon Fire Sebebi**. Her kayit kod+deger, opsiyonel personel gorevi, SMS gonderim bayragi ve SMS sablon kodu icerir. `MyFrmKayit` turevi.
**Once ne olmali (onkosul):** Form, `AciklamaModulTuru` set edilerek acilmali (`FrmAna` menusunden: `IstasyonBaslatmaHata` / `IstasyonDurdurmaKodu` / `IstasyonFireSebep`). Personel gorevi combo'su icin Personel tablosunda Gorevi degerleri bulunmali.
**Sonra ne olur:** Kaydet -> `IIstasyonAciklamaService.InsertOrUpdate(mdl)`; `Modul` alani enum'un string karsiligi olarak yazilir. Sil -> `IIstasyonAciklamaService.Delete(mdl)`. Liste yalnizca o modul turunun kayitlarini gosterir (`where Modul='<tur>' Order By Kodu`). Bu kayitlar saha tabletinde (TabletV2) durdurma/baslatma-hata/fire ekranlarinda secim listesi olarak okunur.
**Butonlar & kisayollar:**
- `Kaydet` (`BtnKaydet`, base) — `Kaydet()` -> kod zorunlu dogrulamasi -> `AktarModele()` (Modul=enum.ToString()) -> `InsertOrUpdate` -> `Bagla()`.
- `Sil` (`BtnSil`, base) — onay sorar, `Sil()` -> `_srv.Delete(_mdl)` -> `Bagla()`.
- `Yeni` (`BtnYeni`, base) — `TemizleText()`.
- `Düzenle` (`BtnDuzenle`, base) — secili satiri klonlayip forma yukler.
- `SmsGonder` (`ChcSmsGonder`) — bu aciklama secilince SMS gonderilsin mi.
- `SmsKodu` (`TxtSmsKodu`) — SMS sablon kodu; `@IstasyonKodu` -> istasyon kodu, `@Personel` -> aktif kullanici ile degistirilir (form ustundeki bilgi etiketleri myLabel5/myLabel6).
- `Per.Görevi` (`CmbGorevi`) — bu aciklamaya/SMS'e bagli personel gorevi.
- Grid cift tiklama / Enter — secim modunda satiri secip kapatir, degilse `BtnDuzenle.PerformClick()`.
- `Kapat` (`BtnKapat`, base) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IIstasyonAciklamaService.SelectListWhere`, `InsertOrUpdate`, `Delete` (`_srv`, `Ortak.DbPro.IstasyonAciklama`) — aciklama CRUD.
- Manager/Service: `IGenelService.GrupListesi("Personel", "Gorevi")` (`_srvGenel`, `Ortak.DbPro.GenelServis`) — gorevi combo'su icin distinct deger listesi.
- SQL/Prosedur: `where Modul='<IstasyonAciklamaModulTuru>' Order By Kodu` — liste filtresi.
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Manager kullanmaz, dogrudan service ile InsertOrUpdate yapar (Kart/Bakim'in aksine). `AciklamaModulTuru` enum degerleri: `IstasyonBaslatmaHata`, `IstasyonDurdurmaKodu`, `IstasyonFireSebep` (`My.Entities.IstasyonAciklamalar`). Form basligi ve `lblBaslik` calismada enum adina gore set edilir (Designer'daki "FrmIstasyonBaslatmaHatalari" override edilir). Grid duzeni `IstasyonAciklamalariListesi1`; `Id` ve `Modul` sutunlari gizlenir.

### Istasyon Bakim Listesi (`FrmIstasyonBakimList.cs` / `FrmIstasyonBakimList.Designer.cs`)
**Ne ise yarar:** Istasyonlara ait bakim/ariza kayitlarini istasyon ve tarih araligina gore listeleyen ana ekran; ust grid bakim baslik kayitlarini, alt grid secili bakimin **degisen parcalarini** gosterir (master-detay). Buradan yeni bakim eklenir veya mevcut bakim duzenlenir. `MyFrmListe` turevi (master-detay split grid).
**Once ne olmali (onkosul):** Istasyon kartlari (`IstasyonKarti`) tanimli olmali (istasyon filtre combo'su icin). Form `FrmAna`'dan MDI cocuk olarak `Show()` ile acilir.
**Sonra ne olur:** Bu ekran kayit yapmaz; `Ekle`/cift tiklama -> `FrmIstasyonBakimEkle` popup'i acar. Popup `KayitEdildi=true` ile kapanirsa `BtnAra` tetiklenir ve liste tazelenir, onceki satir secimi korunur. Ust grid satiri degisince alt parca grid'i `BaglaDetay()` ile yeniden yuklenir.
**Butonlar & kisayollar:**
- `Ara` (`BtnAra`) — `Bagla()`; istasyon kodu + tarih1/tarih2 araligina gore bakim listesini ceker.
- `Ekle` (`BtnEkle`) — bos `FrmIstasyonBakimEkle` acar (yeni bakim).
- `Istasyon` (`CmbIstasyon` combo) — istasyon koduna gore filtre.
- `Tarihi` (`TxtTarihi1`/`TxtTarihi2` tarih kutulari) — bakim tarihi alt/ust siniri (CAST DATE karsilastirmasi).
- Ust grid cift tiklama / Enter (`MyView1_MyEventDoubleClickEnter`) — secili bakimi `FrmIstasyonBakimEkle { IdGuid = itm.Id }` ile duzenlemeye acar.
- Ust grid satir degisimi (`MyView1_FocusedRowChanged`) — alt parca grid'ini gunceller.
- `Kapat` (`BtnKapat`, base) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IIstasyonBakimService.SelectListWhere(sor)` (`_srv`, `Ortak.DbPro.IstasyonBakim`) — bakim baslik listesi (istasyon+tarih filtreli).
- Manager/Service: `IIstasyonBakimParcaService.SelectListWhere(" where IstBakId='...'")` (`_srvParca`) — secili bakimin parca detaylari.
- Manager/Service: `IIstasyonKartiService.SelectListWhere("")` (`_srvIst`) — istasyon filtre combo'sunu doldurur.
- SQL/Prosedur: Dinamik `where 1=1 and IstasyonKodu='...' AND CAST(coalesce(Tarih,'1901-01-01') AS DATE) >= / <= CAST('...' AS DATE)` — istasyon + tarih araligi filtresi.
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Tarih kutulari Designer'da "24.05.2023" sabit baslangic degeriyle gelir. Ust grid duzeni `IstasyonBakimList`, alt grid `IstasyonBakimParcaList`. `BtnEkle_Click` event'i Designer'da baglanir.

### Istasyon Bakim Ekle (`FrmIstasyonBakimEkle.cs` / `FrmIstasyonBakimEkle.Designer.cs`)
**Ne ise yarar:** Tek bir bakim/ariza kaydini (istasyon, tarih, personel, islem turu, aciklama) ve ona ait degisen parca listesini olusturma/duzenleme popup'i. `MyFrmKayit` turevi; alt sekmede parca grid'i (Ekle/Sil panel butonlari) bulunur.
**Once ne olmali (onkosul):** Istasyon kartlari (`IstasyonKarti`) tanimli olmali. `FrmIstasyonBakimList`'ten yeni icin bos, duzenleme icin `IdGuid` set edilerek acilir.
**Sonra ne olur:** Kaydet -> `IstasyonBakimManager.Kaydet(mdl, parcalar)` tek transaction'da: (1) `IstasyonBakim` baslik InsertOrUpdate; (2) bu bakima ait eski `IstasyonBakimParca` kayitlarini sil; (3) gecerli parca listesini InsertOrUpdate. Audit alanlari (KayitEden/KayitTarihi yeni kayitta, Degistiren/DegistirmeTarihi her zaman) `Ortak.KullaniciAdi` ile doldurulur. Sil -> `IstasyonBakimManager.Sil(mdl)` (baslik + tum parcalar). Basariliysa `ActionAktar` callback'i tetiklenir, `KayitEdildi=true`, form kapanir; liste ekrani tazelenir.
**Butonlar & kisayollar:**
- `Kaydet` (`BtnKaydet`, base) — `Kaydet()` -> dogrulama (istasyon kodu zorunlu, tarih bossa now) -> `AktarModele()` -> `Manager.Kaydet()` -> kapan.
- `Sil` (`BtnSil`, base) — onay sorar, `Manager.Sil()` -> kapan.
- `Ekle` (`BtnStokEkle`, sag panel) — `FrmIstasyonBakimParcaEkle` popup'i (yeni parca) acar; donen parca listeye eklenir ve grid yenilenir.
- `Sil` (`BtnStokSil`, sag panel) — onay sorar, secili parcayi listeden cikarir (DB kaydi Kaydet'te transaction ile guncellenir).
- `IstasyonKodu` (`CmbIstasyonKodu` combo) — bakim yapilan istasyon.
- `Personel` / `Bakım/Islem Turu` / `Açıklaması` / `Tarihi` (`TxtPersonel`/`TxtIslemTuru`/`TxtAciklama`/`TxtTarih`) — bakim baslik alanlari.
- `Kapat` (`BtnKapat`, base) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IstasyonBakimManager.Kaydet(IstasyonBakim mdl, List<IstasyonBakimParca> parcalar)` — transaction'li baslik+parca kaydi (eski parcalari silip yeniden yazar).
- Manager/Service: `IstasyonBakimManager.Sil(IstasyonBakim mdl)` — transaction'li baslik+parca silme.
- Manager/Service: `IIstasyonBakimService.SelectFirst(c => c.Id == IdGuid)` (`_mng.IstBakimService`) — duzenlemede baslik yukleme.
- Manager/Service: `IIstasyonBakimParcaService.SelectList(c => c.IstBakId == IdGuid)` (`_mng.IstParcaService`) — duzenlemede parca yukleme.
- Manager/Service: `IIstasyonKartiService.SelectListWhere("")` (`_mng.IstKartService`) — istasyon combo'sunu doldurur.
- SQL/Prosedur: Manager icinde `IstasyonBakim` InsertOrUpdate + `IstasyonBakimParca` Delete (IstBakId) + InsertOrUpdate (Dapper DAL uretir).
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Manager `DatabaseFactoryMikro` da alir (`_dbMikro`) ancak bu formda kullanilmaz (yalniz Pro DB'ye yazar). Parca grid'inde Parca/ParcaNo/EvrakNo/Aciklama/Garanti sutunlari inline duzenlenebilir (`SutunReadOnlyKapat`). `Id`/`IstBakId` gizlenir. Grid duzeni `IstasyonBakimEkleParcalar`. `ActionAktar` callback'i opsiyonel; cagiran liste ekrani genelde `KayitEdildi` bayragiyla yenileme yapar.

### Istasyon Bakim Parca Ekle (`FrmIstasyonBakimParcaEkle.cs` / `FrmIstasyonBakimParcaEkle.Designer.cs`)
**Ne ise yarar:** Bir bakim kaydina eklenecek tek bir degisen parcanin bilgilerini (parca adi, parca no, evrak no, aciklama, garanti) girmek icin kullanilan sade popup. `MyFrmSade` turevi.
**Once ne olmali (onkosul):** `FrmIstasyonBakimEkle` icinden `Parca` ozelligi (yeni `IstasyonBakimParca`, Id atanmis) ve `YeniKayit=true` set edilerek acilir.
**Sonra ne olur:** Kaydet -> parca adi bos degilse `AktarModele()` ile gelen `Parca` nesnesini doldurur, `KayitEdildi=true` set edip kapanir. DB'ye dogrudan YAZMAZ; donen parca cagiran `FrmIstasyonBakimEkle`'nin bellek listesine eklenir ve esas kayit oradaki Kaydet ile transaction icinde DB'ye yazilir.
**Butonlar & kisayollar:**
- `Kaydet` (`BtnKaydet`, base) — Parca bos ise uyarir; degilse `AktarModele()`, `KayitEdildi=true`, kapan.
- `Parça` / `Parça No` / `Evrak No` / `Açıklama` (`TxtParca`/`TxtParcaNo`/`TxtEvrakNo`/`TxtAciklama`) — parca alanlari.
- `Garanti` (`ChcGaranti`) — garanti kapsaminda mi.
- `Kapat` (`BtnKapat`, base) — kaydetmeden kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: - (servis/Manager cagrisi yok; yalniz in-memory nesne doldurur)
- SQL/Prosedur: -
- API: -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** En basit form. Persist islemi tamamen cagiran `FrmIstasyonBakimEkle` + `IstasyonBakimManager.Kaydet()` sorumlulugunda. Tek zorunlu alan `TxtParca`.
## Modul: IstasyonHareketlerModul

Bu modul, sahada (TabletV2) acilan istasyon takip kayitlarinin ofis/yonetim tarafindan **listelenmesi, izlenmesi, duzeltilmesi ve Mikro ERP'ye fis aktarimini** kapsar. Ureticinin kendisi burada uretim girisi yapmaz; uretim girisi tablette `IstasyonTakipPage` akisinda yapilir ve buraya yalniz **sonuc kayitlari** (IstasyonTakipHareket = istasyon basligi/sarisi, IstasyonTakipHareketDetay = her mamul/fire/iptal hareketi, IstasyonTakipStokHareket = sarf edilen recete stoklari, IstasyonTakipHareketLog = baslat/durdur/bitir log'lari) yansir. Modul WinForms (UretimV4 / `CepPatronERP.exe`) icindedir, formlar `Frm*` deseni ile yazilmistir ve DB'ye dogrudan baglanir (`Ortak.DbPro` = Uretim DB, `Ortak.DbMikro` = Mikro DB). Tum servisler `IstasyonHareketManager` veya dogrudan `Ortak.DbPro.<Servis>` uzerinden cagrilir. Akis/miktar motoru OPERASYON-Sira bazlidir: bir detayda miktar duzeltilirse `Uretim_MiktarGuncelle` ve `Uretim_PlanlananGuncelle` prosedurleri yeniden cagrilir ve miktarlar UretimIstasyonHareket -> UretimIstasyon -> UrOHD/UrOH/UrO zincirinde yukari toplanir, bir sonraki operasyon Sira'sinin planlanani guncellenir.

Bu modulde 6 form vardir: 4 liste formu (Hareketler, Bekleyenler, Detaylar, Fis Listesi, Log Listesi) ve 1 kayit/duzenleme formu (Detay Guncelle).

---

### Istasyon Hareketler (`FrmIstasyonHareketler.cs` / `.Designer.cs`)
**Ne ise yarar:** Acilmis istasyon takip basliklarini (IstasyonTakipHareket) listeler. Ust grid baslik kayitlari, alt sol grid o baslica ait hareket detaylari (IstasyonTakipHareketDetay), alt sag grid ilgili UretimIstasyon'a (UrIId) ait sarf stok hareketleri (IstasyonTakipStokHareket). Istasyon, durum (Aktif/Beklemede/Durduruldu/Bitti), is emri (siparis) kodu ve tarih araligina gore filtrelenir.
**Once ne olmali (onkosul):** Tablette ilgili istasyonda en az bir takip kaydi baslatilmis olmali (UretimIstasyon + IstasyonTakipHareket olusmus olmali). Form `SiparisKodundanFiltrele=true` + `SiparisKodu` ile baska bir ekrandan (siparis/is emri) tek is emrine kilitli acilabilir.
**Sonra ne olur:** Bu form yalniz okuma/goruntulemedir; veri degistirmez. Bir detay satirina cift tiklamak (yalnizca `SecimIcinAcildi` modunda) secim dondurur. Detay duzeltmesi bu formdan degil, FrmIstasyonHareketDetaylar -> FrmIstasyonHareketDetayED uzerinden yapilir.
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `Bagla()` cagirir, filtreye gore ust gridi doldurur. Form acilisinda `BtnAra.PerformClick()` otomatik calisir.
- `BtnTemizle` ("Temizle") — Tarih (son 1 ay) / istasyon / is emri kodu filtrelerini sifirlar.
- `BtnDurumuHepsi` ("Hepsi") — durum filtresini bos birakir, otomatik arar.
- `BtnDurumuAktif` ("Aktif") — Durumu=Aktif filtreler (acilista varsayilan secili).
- `BtnDurumuBeklemede` ("Beklemede") — Durumu=Beklemede filtreler (Designer'da `Visible=false`, gizli).
- `BtnDurumuDurduruldu` ("Durduruldu") — Durumu=Durduruldu filtreler.
- `BtnDurumuBitti` ("Bitti") — Durumu=Bitti filtreler.
- `CmbIstasyon` (combo) — istasyon kodu filtresi (IstasyonKarti listesinden dolar).
- `TxtKodu` (Is Emri Kodu), `TxtTarihi1/TxtTarihi2` — like ve tarih araligi filtreleri.
- Grid cift tik / Enter (`MyEventDoubleClickEnter`) — secim modunda kayit dondur.
- `BtnKapat` (base) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager/Service: `IstasyonHareketManager(Ortak.DbPro, Ortak.DbMikro)` — modul servislerini toplar.
- Service: `IstHareketService.GetViewListWhere(where)` — IstasyonTakipHareket view'ini filtreyle ceker (ust grid).
- Service: `IstHareketDetayService.GetViewListWhere(" where IstHD.IstHrId='...'")` — secili baslica ait detaylar (alt sol grid).
- Service: `IstHareketStokService.GetStokHareketByUrIId(UrIId)` — secili UretimIstasyon'un sarf stok hareketleri (alt sag grid).
- Service: `IstKartService.SelectListWhere("")` — istasyon combo'sunu doldurur.
- SQL/Prosedur: yok (yalniz select).
- API: yok (WinForms, dogrudan DB).
**Istasyon sirasiyla iliskisi:** Durumu=Aktif satirlar o anda calisilan, Bitti satirlar bir sonraki istasyona sevk edilmis/tamamlanmis operasyonlardir. Alt sag grid UrIId ile o operasyonun sarf recete stoklarini gosterir; operasyon Sira mantigini dogrudan degistirmez (okuma).
**Notlar:** `Ortak.PlKapat=true` ise Parti/Lot kolonlari gizlenir. SorguAyarla string concatenation ile yazilmistir (parametrik degil). "SiparisKodu" kolonu UI'da "IsEmriKodu" olarak gosterilir.

---

### Istasyon Bekleyenler (`FrmIstasyonBekleyenler.cs` / `.Designer.cs`)
**Ne ise yarar:** Henuz hicbir istasyon takip kaydi (IstasyonTakipHareket) olusturulmamis, yani sahada baslatilmayi bekleyen UretimIstasyon kuyrugunu gosterir. Her satir: kalan miktar, planlanan/uretilen/fire/iptal miktarlari, recete, operasyon, siparis (is emri), teslim tarihi, fason bilgisi vb.
**Once ne olmali (onkosul):** Uretim emri + receteden UretimOperasyon/UretimIstasyon kayitlari uretilmis (KalanMiktar > 0) ama o UretimIstasyon icin henuz IstasyonTakipHareket acilmamis olmali.
**Sonra ne olur:** Salt okuma listesidir. Cift tik islevleri kod icinde yorum satirina alinmis; herhangi bir kayit/degisiklik tetiklemez. Operatorun tablette hangi isleri baslatmasi gerektigini planlama amaclidir.
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `Bagla()` cagirir; form acilisinda da otomatik `Bagla()` calisir.
- `BtnTemizle` ("Temizle") — Designer'da var ancak bu formda click event'i baglanmamis (etkin degil).
- `CmbIstasyon` (combo) — istasyon kodu filtresi (`UrI.IstasyonKodu`).
- `TxtKodu` ("Is Emri Kodu") — siparis kodu like filtresi (`Sip.SiparisKodu`).
- Grid cift tik / Enter (`MyEventDoubleClickEnter`) — aktif islev yok (yorumlu).
- `BtnKapat` (base) — kapatir.
**Cagirdigi katmanlar:**
- Manager: `IstasyonHareketManager.GetBekleyenler(whereAnd)` — su SQL'i calistirir: `UretimIstasyon UrI` + `UretimOperasyon UrO` + `Siparis` + `SiparisHareket` LEFT JOIN, `IstasyonTakipHareket TH` LEFT JOIN ile `WHERE UrO.KalanMiktar > 0 AND TH.Id IS NULL` (yani henuz takip acilmamis); UrI bazinda planlanan/uretilen/fire/iptal miktarlarini gruplayip toplar. (Benzer `GetBekleyenByIstKodu(Istasyon)` metodu da ayni mantikla tek istasyon icindir.)
- Service: `IstKartService.SelectListWhere("")` — istasyon combo'su.
- SQL/Prosedur: inline SELECT (ad yok), prosedur cagirmaz.
- API: yok.
**Istasyon sirasiyla iliskisi:** "Bekleyen" = `IstasyonTakipHareket` henuz yok; bir operasyonun Sira'sina ait UretimIstasyon olusmus ama saha basinda is alinmamistir. Tablette baslatildiginda bu satir listeden dusup FrmIstasyonHareketler'e tasinir.
**Notlar:** Tarih filtreleri (`SorguAyarlaTrh`) tamamen yorum satiridir, etkisizdir. PlKapat'ta Parti/Lot gizlenir.

---

### Istasyon Hareket Detaylar (`FrmIstasyonHareketDetaylar.cs` / `.Designer.cs`)
**Ne ise yarar:** Tum istasyon hareket **detaylarini** (IstasyonTakipHareketDetay) tek listede gosterir (mamul giris, fire mamul giris, uretim bitis, fire stok giris, uretim iptal turleri). Sarf/Fire **fis** turleri (SarfCikisFisi, FireGirisFisi) bu listeden HARIC tutulur (onlar FrmIstasyonFisList'te). Detaya cift tiklanarak miktar/fire/iptal duzeltme ekrani acilir.
**Once ne olmali (onkosul):** Tablette uretim girisi yapilmis ve IstasyonTakipHareketDetay kayitlari olusmus olmali.
**Sonra ne olur:** Cift tik -> `FrmIstasyonHareketDetayED` acilir; orada kaydedilirse miktarlar guncellenir ve dönülünce `Bagla()` ile liste tazelenir.
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `Bagla()`.
- `BtnTemizle` ("Temizle") — tarih (son 1 ay), saat (00:00:00 / 23:59:59), istasyon, is emri kodu filtrelerini sifirlar.
- `CmbIstasyon` (combo) — `IstHr.IstasyonKodu` filtresi.
- `CmbTuru` (combo) — `IstHD.Turu` filtresi; SarfCikisFisi ve FireGirisFisi turleri listeye eklenmez.
- `TxtKodu` ("Is Emri Kodu") — `IstHr.SiparisKodu` like.
- `TxtTarihi1/2` + `TxtSaat1/2` — `IstHD.Tarih` datetime araligi.
- Grid cift tik / Enter (`MyEventDoubleClickEnter`) — secim modunda secim dondurur; degilse `FrmIstasyonHareketDetayED` acar.
- `BtnKapat` (base) — kapatir.
**Cagirdigi katmanlar:**
- Service: `Ortak.DbPro.IstasyonTakipHareketDetay.GetViewListWhere(where)` — detay view'i (SarfCikisFisi/FireGirisFisi haric SQL ile filtrelenir).
- Service: `Ortak.DbPro.IstasyonKarti.SelectListWhere("")` — istasyon combo.
- Service: `Ortak.DbPro.GenelServis.GrupListesi("IstasyonTakipHareketDetay","Turu")` — Turu combo degerleri.
- Acilan form: `FrmIstasyonHareketDetayED` (Model=secili detay).
- SQL/Prosedur: dogrudan prosedur cagirmaz (duzeltme FrmIstasyonHareketDetayED'de yapilir).
- API: yok.
**Istasyon sirasiyla iliskisi:** Detay turleri (MamulGiris/UretimBitis/UretimIptal/FireMamulGiris) miktar motorunun yukari toplama girdileridir; burada yapilan duzeltme bir sonraki operasyon Sira'sinin planlananini etkiler (ED formunda prosedur tetiklenir).
**Notlar:** Sarf/Fire fisleri kasıtlı olarak haric. Saat alanlari TimeSpanEdit.

---

### Istasyon Hareket Detay Guncelle (`FrmIstasyonHareketDetayED.cs` / `.Designer.cs`)
**Ne ise yarar:** Tek bir IstasyonTakipHareketDetay kaydinin Miktar / FireMiktar / IptalMiktar degerlerini duzeltir (ofiste hatali girilen miktari elle duzeltme). Stok/Recete/Operasyon kod-ad alanlari salt okunur gosterilir.
**Once ne olmali (onkosul):** FrmIstasyonHareketDetaylar listesinde bir detay satiri secilip cift tiklanmis (`Model` set edilmis) olmali.
**Sonra ne olur (kaydet sonrasi):** `IstasyonHareketManager.SaveIstasyonTakipHareketDetayUpdate(Model)` calisir; tek transaction icinde su zincir isler ve form kapanir:
1. IstasyonTakipHareketDetay update (detay miktarlari yazilir),
2. `UretimIstasyonHareket` ayni Id ile UretimMiktari/FireMiktari/IptalMiktari guncellenir,
3. `IstasyonTakipHareket` baslik miktarlari (`GetMiktarFireUpdateSqlCode`) yeniden hesaplanir (detaylardan MamulGiris/FireMamulGiris/UretimBitis/UretimIptal toplanir; KalanMiktar = Planlanan - (Uretim+Iptal+Fire)),
4. UrIId bazinda iptal miktari stok hareketlerine oranlanir (`GetIptalUpdateSqlCodeByUrIId`),
5. `exec [Uretim_MiktarGuncelle] '<UrId>'`,
6. `exec [Uretim_PlanlananGuncelle] '<UrId>'`.
**Butonlar & kisayollar:**
- `BtnKaydet` ("Kaydet") — `AktarRowa()` + `Kaydet()` (yukaridaki update zincirini calistirir, sonra `this.Close()`).
- `BtnKapat` (base) — kaydetmeden kapatir.
- `BtnSil` (base) — `Visible=false`, gizli (silme yok).
- `BtnYeni/BtnDuzenle/BtnYazdir/BtnIlk/BtnOnceki/BtnSonraki/BtnSon` (base nav) — bu formda islevsiz/gizli, tek kayit duzenleme.
- Duzenlenebilir alanlar: `TxtMiktar`, `TxtFireMiktar`, `TxtIptalMiktar` (sayi). Diger alanlar ReadOnly.
**Cagirdigi katmanlar:**
- Manager: `IstasyonHareketManager.SaveIstasyonTakipHareketDetayUpdate(IstasyonTakipHareketDetay)` — yukaridaki 6 adimli transaction.
- SQL/Prosedur: `Uretim_MiktarGuncelle` — miktarlari UretimIstasyonHareket->UretimIstasyon->UrOHD/UrOH/UrO zincirinde yukari toplar. `Uretim_PlanlananGuncelle` — operasyon Sira N uretimini Sira N+1 planlanina tasir, sonunda `Uretim_SonrakiIstasyonaGonder` cagrilir (ReceteAna.IstasyonGruplamaKullan=1 ise yeni UretimIstasyon olusturur).
- SQL kod (entity static): `IstasyonTakipHareketDetay.GetUpdateSqlCode()`, `IstasyonTakipHareket.GetMiktarFireUpdateSqlCode(IstHrId)`, `IstasyonTakipHareket.GetIptalUpdateSqlCodeByUrIId(UrIId)`.
- API: yok.
**Istasyon sirasiyla iliskisi:** Bu form akis motorunu **tetikleyen tek yazma noktasidir**. Bir detay miktari degistiginde `Uretim_PlanlananGuncelle` -> `Uretim_SonrakiIstasyonaGonder` zinciriyle bir sonraki operasyon Sira'sinin planlanani/istasyon kaydi yeniden hesaplanir.
**Notlar:** `MyFrmKayit` turevidir (kayit formu base'i). Transaction'da hata olursa rollback + ErrorResult; basaride form kapanir.

---

### Istasyon Fis Listesi (`FrmIstasyonFisList.cs` / `.Designer.cs`)
**Ne ise yarar:** Yalniz **SarfCikisFisi** ve **FireGirisFisi** turundeki istasyon hareket detaylarini (Mikro'ya gonderilecek sarf cikisi / fire girisi fisleri) listeler ve secilenleri Mikro ERP'ye aktarir. "Mikroya Aktarildi" durumuna gore (Tumu / Aktarilan / Bekleyen) filtrelenir.
**Once ne olmali (onkosul):** Tablette/uretim girisinde sarf veya fire fisi turunde IstasyonTakipHareketDetay kayitlari olusmus olmali. Aktarim oncesi Mikro entegre ayarlari (`Ortak.MikroEntAyarlar` icinde fis turu ayarlari) tanimli olmali.
**Sonra ne olur (Mikroya Gonder sonrasi):** Secili fisler `FrmMikroyaSarfFireKaydet`e tasinir; orada Mikro `STOK_HAREKETLERI` kayitlari uretilip yazilir, basarida detay kayitlari `Ent=true`, `EntSeri/EntSira/EntDate` ile guncellenir (artik "Aktarilan" sekmesinde gorunur).
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `Bagla()`; acilista `AktarimAyarla("Bekleyen")` + otomatik `BtnAra.PerformClick()`.
- `BtnTemizle` ("Temizle") — tarih filtrelerini bosaltir.
- `BtnAktarildiTumu` ("Tumu") — Ent filtresi yok.
- `BtnAktarildiAktarilan` ("Aktarilan") — `coalesce(Ent,0)=1`.
- `BtnAktarildiBekleyen` ("Bekleyen") — `coalesce(Ent,0)=0` (acilista varsayilan).
- `TxtTarihi1/2` — `IstHD.Tarih` araligi.
- `Sec` kolonu — grid'de editlenebilir checkbox (aktarilacak satirlari isaretleme).
- Sag tik context menu: **"Mikroya Gonder"** (`ToolStripMenuItem`) — secili (`Sec=true`) fisleri toplayip `FrmMikroyaSarfFireKaydet` acar.
- Grid cift tik / Enter (`MyEventDoubleClickEnter`) — secim modunda secim dondurur; degilse islevsiz.
- `BtnKapat` (base) — kapatir.
**Cagirdigi katmanlar:**
- Service: `Ortak.DbPro.IstasyonTakipHareketDetay.GetViewListWhere(where)` — yalniz `Turu=SarfCikisFisi OR Turu=FireGirisFisi` + Ent durumu filtresi.
- Acilan form: `FrmMikroyaSarfFireKaydet` (MikroModul) — `FisList` ile.
- `FrmMikroyaSarfFireKaydet` icinde: `MikroKayitManager` (`StokHareketIdKayitEdilmismi`, `StokHareketKaydet`), `MikroConvertManager` (`SetSarfCikisFisiTuruAyar`, `SetFireGirisFisiTuruAyar`, `ConvertStokVirmanFisi`, `ConvertSarfDepoCikis`), `IstasyonTakipHareketDetay` servisinin `InsertOrUpdate(FisList)` ile Ent guncellemesi.
- SQL/Prosedur: dogrudan prosedur cagirmaz; aktarim Mikro tarafinda StokHareketKaydet ile yapilir.
- API: yok.
**Istasyon sirasiyla iliskisi:** -  (Fis aktarimi miktar/Sira motorunu degil, Mikro ERP stok hareketlerini etkiler.)
**Notlar:** Cift aktarimi engellemek icin `FrmMikroyaSarfFireKaydet` once `StokHareketIdKayitEdilmismi` ile onceden aktarilmis kayit kontrolu yapar; varsa Kaydet butonu gizlenir. `Sec` kolonu acilista ReadOnly kapatilip edit acilir.

---

### Istasyon Hareket Log Listesi (`FrmIstasyonHareketLogList.cs` / `.Designer.cs`)
**Ne ise yarar:** Saha akisindaki baslat/durdur/bitir/sevk gibi olaylarin log kayitlarini (IstasyonTakipHareketLog) listeler. Operasyon, istasyon ve tarih-saat araligina gore filtrelenir. Denetim/izleme amaclidir.
**Once ne olmali (onkosul):** Tablette istasyon takip akisi calismis ve log kayitlari (`IstasyonTakipHareketLog`) olusmus olmali.
**Sonra ne olur:** Salt okuma; veri degistirmez. Cift tik islevi (uretim emri editorune gitme) kod icinde yorumludur, aktif degildir.
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `Bagla()`; form acilisinda da `Bagla()` otomatik.
- `BtnTemizle` ("Temizle") — tarih bos, saat 00:00:00 / 23:59:59 yapar.
- `CmbOperasyon` (MyLookupEdit) — `TH.OperasyonKodu` filtresi (OperasyonKarti listesinden).
- `CmbIstasyon` (MyLookupEdit) — `TH.IstasyonKodu` filtresi (IstasyonKarti listesinden).
- `TxtTarihi1/2` + `TxtSaat1/2` — `LG.Tarih` datetime araligi.
- Grid cift tik / Enter (`MyEventDoubleClickEnter`) — secim modunda secim dondurur; degilse islevsiz (yorumlu).
- `BtnKapat` (base) — kapatir.
**Cagirdigi katmanlar:**
- Service: `Ortak.DbPro.IstasyonTakipHareketLog.GetViewListWhere(where)` — log view'i.
- Service: `Ortak.DbPro.OperasyonKarti.SelectListWhere(" Order By OperasyonKodu")` — operasyon combo.
- Service: `Ortak.DbPro.IstasyonKarti.SelectListWhere(" Order By IstasyonKodu")` — istasyon combo.
- SQL/Prosedur: yok (yalniz select).
- API: yok.
**Istasyon sirasiyla iliskisi:** Log kayitlari saha akisindaki adimlarin (baslat/durdur/bitir/sonraki istasyona sevk) zaman damgali izini tutar; Sira/akis motorunu degistirmez.
**Notlar:** `IstasyonTakipHareketLogService` ayni zamanda `IstasyonHareketManager.IstHareketLogService` olarak da erisilebilir ama bu form servisi dogrudan `Ortak.DbPro` uzerinden alir.

---

#### Modul geneli notlar
- Tum liste formlari `MyFrmListe`, kayit formu (`FrmIstasyonHareketDetayED`, `FrmMikroyaSarfFireKaydet`) `MyFrmKayit` base sinifindan turer (base siniflar `My.Kontrol.Formlar` namespace'inde, harici kontrol kutuphanesinde). `BtnAra/BtnTemizle/BtnKapat/BtnYazdir/BtnDizayn` ve kayit formundaki `BtnKaydet/BtnSil/BtnYeni/BtnIlk/BtnOnceki/BtnSonraki/BtnSon` base'ten gelir.
- Grid cift tik ve Enter, base'in `MyView.MyEventDoubleClickEnter` event'i ile ayni davranisi tetikler (cift tik = Enter).
- Filtre butonlarinin secili gorunumu `MyButton.FilterButonRenklendir(true/false)` ile renklendirilir.
- Tek **yazma** noktasi `FrmIstasyonHareketDetayED` (miktar duzeltme + prosedur zinciri) ve `FrmIstasyonFisList` -> `FrmMikroyaSarfFireKaydet` (Mikro'ya fis aktarimi). Diger formlar salt okuma/listeleme.
## Modul: ReceteModul

Recete (urun agaci / urun receteleri) modulu, uretilecek mamulun hangi hammadde/yari-mamul stoklardan (Detaylar -> Stoklar), hangi operasyonlardan (Operasyonlar -> Istasyonlar -> Cariler) ve hangi siralamayla olusacagini tanimlar. Bu modulde tanimlanan veriler dogrudan uretim akisinin temelidir: ReceteOperasyon.Sira degeri, uretim girisinde calisan `Uretim_PlanlananGuncelle` prosedurunun "operasyon Sira N -> Sira N+1" tasimasinin kaynagidir; `ReceteAna.IstasyonGruplamaKullan=1` ise `Uretim_SonrakiIstasyonaGonder` prosedurunun calisma sartidir. Modul WinForms (DevExpress) ekranlarindan olusur: Recete Listesi (ana giris), Recete Ekle/Duzenle (kart), Recete Detay Ekle, Recete Stok Sec (renk/beden), Recete Operasyon Ekle/Duzenle, Recete Aciklamalari, iki Maliyet ekrani (Genel ve Stoklar), Recete Sec (secim diyalogu), Recete Grup Listesi ve Recete Grup Ekle/Duzenle. Veriler `UretimV3_FEZA` DB'sindeki ReceteAna / ReceteDetay / ReceteStok / ReceteStokRenkBeden / ReceteyeBagliIstasyon / ReceteOperasyon / ReceteIstasyon / ReceteIstasyonCari / ReceteGrup / ReceteGrupDetay tablolarinda tutulur; maliyet ekranlari ayrica Mikro DB'sindeki STOKLAR ve URUN_RECETELERI tablolarina baglanir.

Tum kayit ekranlari `MyFrmKayit`, liste/secim ekranlari `MyFrmListe`, sade diyaloglar `MyFrmSade` / `XtraForm` base sinifindan turer. Base siniflar (My.Kontrol DLL) standart alt buton seridi saglar: `BtnKaydet` (Kaydet), `BtnKapat` (Kapat/Esc), `BtnSil`, `BtnYazdir`, `BtnDuzenle`, navigasyon butonlari (BtnIlk/BtnOnceki/BtnSonraki/BtnSon) ve listede `BtnAra` / `BtnTemizle`. Grid'lerde satira cift tiklama veya Enter, `MyEventDoubleClickEnter` olayini tetikler (genelde duzenleme/secim). Designer dosyalarinda ayrica tanimlanmis F-tusu (ShortcutKeys) bulunmamaktadir; kisayollar base form davranisi ve grid Enter/cift-tik ile sinirlidir.

---

### Recete Listesi (`FrmReceteListesi.cs` / `FrmReceteListesi.Designer.cs`)
**Ne ise yarar:** Tum recetelerin arandigi/listelendigi ana ekran. Ust grid recete basliklari (ReceteAna), alt iki grid secili recetenin Detaylari (ReceteDetay) ve Operasyonlarini (ReceteOperasyon) gosterir. Ayni form `SecimIcinAcildi=true` ile baska ekranlardan recete secmek icin de acilir.
**Once ne olmali (onkosul):** Mikro stok cinsi tanimlari okunabilmeli (BaglaStokCinsi). Liste filtresiz/filtreli acilir, otomatik arar (Frm_Load -> Bagla).
**Sonra ne olur:** Bu ekran veriyi sadece okur; degisiklik alt ekranlardan yapilir. Satira cift-tik/Enter -> secim modunda secili receteyi dondurur ve kapanir; degilse `FrmReceteED` (duzenleme) acilir, kayit sonrasi liste yenilenir.
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — filtre alanlarina (Kodu/Adi/Grup/Stok Cinsi/Ara) gore listeyi yeniden ceker (Bagla -> SorguAyarla LIKE/eq sorgusu).
- `Temizle` (BtnTemizle) — Kodu/Adi/Grup filtre alanlarini bosaltir.
- `Recete Ekle` (BtnReceteEkle) — bos `FrmReceteED` acar (yeni recete).
- `Operasyon Ekle / Duzenle` (BtnOperasyonEkle) — secili recetenin `FrmReceteOperasyonED` ekranini acar; recete secili degilse uyarir.
- `Operasyon Kopyala` (BtnOperasyonKopyala) — kaynak receteyi sectirir (`FrmReceteListesi` secim modu), kaynak operasyonlari hedef receteye kolonlamak icin `FrmReceteOperasyonED`'i Kolonlanacak=true ile acar.
- `Stok Maliyet` (BtnStokMaliyet) — secili recete icin `FrmReceteMaliyetStoklar` acar.
- Grid cift-tik / Enter (myView1) — secim modunda receteyi dondurur, degilse `FrmReceteED` acar.
- `Kapat` (BtnKapat) — formu kapatir.
**Cagirdigi katmanlar:**
- Service: `IReceteAnaService.GetListWhere(where)` — receteleri (StokCinsiAdi dahil view) filtreyle getirir.
- Service: `IReceteDetayService.SelectList(c => c.RcAId == id)` — secili recetenin detaylarini getirir.
- Service: `IReceteOperasyonService.SelectList(c => c.RcAId == id)` — secili recetenin operasyonlarini getirir.
- Service: `IGenelService.GrupListesi("ReceteAna","Grubu")` — grup filtre comboboxunu doldurur.
- Manager: `MikroStokCinsiManager.GetCinsListFull()` — Mikro stok cinsi listesi (Mamul vb. filtre).
**Istasyon sirasiyla iliskisi:** Operasyon gridinde ReceteOperasyon.Sira goruntulenir; bu sira uretim akisinin (Sira N -> N+1) belirleyicisidir. Burada salt okuma.
**Notlar:** `ContexMenuyeEkle` (Uretim Emri Olustur context menu) kod icinde yorumlanmis, aktif degil. SorguAyarla string birlestirmeli LIKE filtresi kullanir.

---

### Recete Ekle / Duzenle (`FrmReceteED.cs` / `FrmReceteED.Designer.cs`)
**Ne ise yarar:** Bir recetenin ana bilgileri (kod, ad, grup, entegre Mikro stok, raf omru, bayraklar) ile Detaylarini ve her detayin alternatif Stoklarini (renk/beden), ayrica Bagli Istasyonlarini tanimlamak/duzenlemek icin ana kart ekrani. Mikrodan recete ice aktarma da burada yapilir.
**Once ne olmali (onkosul):** Listeden recete secilmis (IdGuid dolu -> duzenleme) ya da yeni kayit (IdGuid bos). `MikrodanAktar=true` ise gecerli `MikroReceteKodu` set edilmis olmali.
**Sonra ne olur:** Kaydet -> transaction ile ReceteAna upsert + ReceteDetay/ReceteStok/ReceteyeBagliIstasyon/ReceteStokRenkBeden tablolari silinip yeniden yazilir (ReceteManager.ReceteKaydet). Ayni EntegreStokKodu baska recetede varsa kayit engellenir. Kayit sonrasi `KayitEdildi=true` set edilir, form veriyi yeniden yukler; cagiran liste yenilenir.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — TextLeriKontrolEt + AktarModele + Entegre stok kodu tekillik kontrolu + ReceteKaydet.
- `Sil` (BtnSil) — once `ReceteSilKontrol` (Recete_Sil_Kontrol proc) ile bagli hareket var mi bakar, yoksa ReceteSil (operasyon bagliysa silmez).
- `Kopyala Ctr+D` (BtnDuzenle -> BtnDegistir_Click) — gecerli receteyi yeni Id'lerle kopyalar (ReceteKodu bosaltilir, yeni kayit olur).
- `Detay Ekle` (BtnDetayEkle) — `FrmReceteDetayED` yeni detay ekler (Sira=receteSira+1).
- `Detay Sil` (BtnDetaySil) — secili detayi ve ona bagli stok/renk-beden satirlarini listeden cikarir.
- `Stok Ekle` (BtnStokEkle) — secili detaya Mikro stok secip (`FrmMikroStokListesi`) `FrmReceteStokSec` ile renk/beden tanimlar.
- `Stok Sil` (BtnStokSil) — secili ReceteStok ve renk/beden satirlarini cikarir.
- `Aciklamalar` (BtnAciklamalar) — kayitli recete icin `FrmReceteAciklamalar` acar (kaydedilmemisse uyarir).
- `Operasyon Ekle / Duzenle` (BtnOperasyonEkle1) — kayitli recete icin `FrmReceteOperasyonED` acar.
- `Operasyon Kopyala` (BtnOperasyonKopyala1) — kaynak receteyi secip operasyonlari bu receteye kolonlar.
- `Stok Maliyet` (BtnStokMaliyet) — `FrmReceteMaliyetGenel` acar (gecerli model klonuyla).
- `Istasyon Ekle` / `Istasyon Sil` (BtnIstasyonEkle/BtnIstasyonSil, "Bagli Istasyonlar" sekmesi) — ReceteyeBagliIstasyon listesine istasyon karti ekler/cikarir.
- `Istasyon Gruplar` (BtnIstasyonGruplar) — `FrmReceteIstasyonGrupIstasyonEslestir` (grup-istasyon eslestirme) acar.
- `Istasyon-Grup Ayarlar` (BtnIstasyonGrupAyarlar) — `FrmReceteIstasyonGrupOperasyonEslestir` acar.
- `Stok Bilgi ^ Recete Aktar` (BtnRecetAdiAktar) — Entegre stok kodu/adini ReceteKodu/ReceteAdi alanlarina kopyalar.
- TxtReceteKodu buton — `GetEvrakNo("Recete")` ile otomatik recete kodu uretir.
- TxtEntegreStokKodu/Adi/Birim/ModelKodu buton — `FrmMikroStokListesi` secimi (entegre stok ata + cinsi getir).
- Sekmeler: `Detaylar/Stoklar` (tabNavigationPage1), `Bagli Istasyonlar` (tabNavigationPage2).
- Bayraklar: `Haziri Sonraki Istasyona Gonder (Grupsuz)`, `Istasyon Gruplama Kullan`, `Aparat Zorunlu`, `Olcum Kullan (Zorunlu)` checkbox'lari.
**Cagirdigi katmanlar:**
- Manager: `ReceteManager.GetReceteKayit(id)` — recete + detay + stok + bagli istasyon + renk/beden modelini doldurur.
- Manager: `ReceteManager.ReceteKaydet(model, yenikayit)` — transaction ile tum alt tablolari sil-yeniden yaz.
- Manager: `ReceteManager.ReceteStokKoduDahaonceGirilmismi(id, entegreStokKodu)` — ayni entegre stok koduyla baska recete var mi kontrol eder.
- Manager: `ReceteManager.ReceteSilKontrol(id)` / `ReceteManager.ReceteSil(model)` — Recete_Sil_Kontrol proc + iliskili tablolari sil.
- Manager: `MikroReceteManager.GetMikroReceteList(where)` / `GetMikroReceteHareketler(receteKodu)` — Mikrodan aktarmada URUN_RECETELERI'nden recete basligi ve satirlarini ceker.
- Service: `IMikroStokService.GetViewListWhere(...)` — entegre stok cinsini bulur (StokDanCinsiBagla).
- Service: `IGenelService.GrupListesi/GetEvrakNo` — grup combobox + recete evrak no.
- SQL/Prosedur: `Recete_Sil_Kontrol` — receteye bagli hareket adedini dondurur.
**Istasyon sirasiyla iliskisi:** `IstasyonGruplamaKullan` bayragi burada set edilir; =1 ise uretim akisinda `Uretim_SonrakiIstasyonaGonder` calisir. Detay ReceteSira (urun agaci satir sirasi) ve operasyon sirasi (ayri ekranda) buradaki receteye baglidir.
**Notlar:** Detay gridinde satir ici duzenlenebilen kolonlar yesil baslikli (ReceteSira, Miktar, Ebat, Gram, Olcu, FireYuzde). RafOmru gun bazli. Mikrodan aktarilan detaylar StokTuru="Sabit", Cinsi="MikroRecete" olarak gelir.

---

### Recete Detay Ekle / Duzenle (`FrmReceteDetayED.cs` / `FrmReceteDetayED.Designer.cs`)
**Ne ise yarar:** Recetenin tek bir detay (urun agaci satiri) bilgilerini girer: cinsi (kategori), birim, miktar, fire %, recete sira, varsayilan stok (kod/ad), renk/beden, operasyon maliyeti, stok turu (Stok/Grup/Sabit/Tumu), stok ana grup, aciklama ve bayraklar (StokKullan, SiparisdeGosterme).
**Once ne olmali (onkosul):** `FrmReceteED`'den cagrilir; yeni icin `Detay=new ReceteDetay()` ve `Sira` set edilir, duzenleme icin mevcut `Detay` gecirilir.
**Sonra ne olur:** Tamam -> AktarModele ile `Detay` nesnesi guncellenir, `KayitEdildi=true` ile kapanir; cagiran `FrmReceteED` detayi listesine ekler/gunceller (DB'ye yazma yok, kayit ana ekranin Kaydet'inde olur).
**Butonlar & kisayollar:**
- `Tamam` (BtnTamam) — zorunlu alan kontrolleri (Cinsi, Miktar>0, stok turune gore ana grup/varsayilan stok kodu) + AktarModele + kapat.
- VarsayilanStokKodu/Adi buton (TxtVarsayilanStokKodu1/Adi1) — `FrmMikroStokListesi` ile Mikro stok secer, birim + renk/beden listelerini yukler.
- StokTuru secimi: `Stok` / `Grup` / `Sabit` / `Tumu` radio (RadStok1/RadGrup1/RadSabit1/RadTumu1).
- `Kapat` — diyalogu iptal eder.
**Cagirdigi katmanlar:**
- Service: `IMikroStokService.GetStokKategoriler()` — Cinsi comboboxunu doldurur (kategoriler).
- Service: `IMikroStokService.GetRenkByStokKodu(kod)` / `GetBedenByStokKodu(kod)` — renk/beden comboboxlari.
- Service: `IMikroGenelService.GrupListesi("STOKLAR", Ortak.MikroStokGrubu)` — Stok Ana Grup comboboxu.
**Istasyon sirasiyla iliskisi:** ReceteSira (detay/urun agaci sirasi) burada girilir; operasyon sirasindan farklidir.
**Notlar:** StokTuru "Grup" ise StokAnaGrup, "Sabit" ise VarsayilanStokKodu zorunludur. Yeni kayitta StokKullan ve SiparisdeGosterme varsayilan true gelir.

---

### Recete Stok Sec (renk/beden) (`FrmReceteStokSec.cs` / `FrmReceteStokSec.Designer.cs`)
**Ne ise yarar:** Bir ReceteStok satiri icin stok kodu/ad, renk, beden, ebat, gram, olcu bilgilerini ve coklu renk/beden secim listesini (ReceteStokRenkBeden) belirler.
**Once ne olmali (onkosul):** `FrmReceteED` -> Stok Ekle (yeni: `MikroStok` gecirilir) veya stok satirina cift-tik (Edit=true, mevcut `ReceteStok` + secili renk/beden listesi gecirilir).
**Sonra ne olur:** Kaydet -> `ReceteStok` guncellenir, `RenkBedenListSecilen` (Sec=true olanlar) doldurulur, `Secildi=true` ile kapanir; cagiran ekran model listelerine yansitir (DB'ye yazma ana ekranda).
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — alanlari `ReceteStok`'a aktarir, secili beden satirlarini RenkBedenListSecilen'e ekler, kapatir.
- Beden grid (myView2) — Sec ve Miktar kolonlari duzenlenebilir.
- `Kapat` — iptal.
**Cagirdigi katmanlar:**
- Service: `IMikroStokService.GetBedenByStokKodu(kod)` — beden listesi.
- Service: `IReceteStokRenkBedenService.SelectListWhere(where)` — mevcut renk/beden kayitlarini (RcAId/RcDId/RcSTId) getirir.
- Yardimci: `ReceteStokRenkBeden.GetBeden(receteStok, beden)` — eksik bedenler icin satir uretir.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Edit modunda StokKodu kolonu gizlenir; Bedenler Mikro stoktan, secili olanlar mevcut kayitlardan birlesir.

---

### Recete Operasyon Ekle / Duzenle (`FrmReceteOperasyonED.cs` / `FrmReceteOperasyonED.Designer.cs`)
**Ne ise yarar:** Bir recetenin operasyonlarini (ReceteOperasyon: Sira, uretim sure, maliyet, kullanilan aparat, olcum min/max 1-5), her operasyona bagli istasyonlari (ReceteIstasyon: fason, maliyet) ve her istasyona bagli carileri (ReceteIstasyonCari) tanimlar. Uretim akisinin operasyon sirasini belirleyen kritik ekran.
**Once ne olmali (onkosul):** `FrmReceteListesi` veya `FrmReceteED` uzerinden gecerli recete IdGuid'i ile acilir. Kopyalama icin `Kolonlanacak=true` + `KolonRecete` (hedef recete) set edilir.
**Sonra ne olur:** Kaydet -> sira kontrolu (1'den baslayan tekil ardisik) sonrasi `ReceteOperasyonManager.OperasyonKaydet` transaction ile ReceteOperasyon/ReceteIstasyon/ReceteIstasyonCari tablolarini sil-yeniden yazar; duzenlemede ayrica `ReceteOperasyon_Tablo_AdGuncelle` ve `ReceteIstasyon_Tablo_AdGuncelle` proclari calistirilir. KayitEdildi=true ile kapanir.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — TextLeriKontrolEt + AktarModele + OperasyonSiraKontrol + OperasyonKaydet.
- `Sil` (BtnSil) — tum operasyonlar icin Operasyon_Sil_Kontrol, hepsi temizse OperasyonSil.
- `Operasyon Ekle` (BtnOperasyonEkle) — `FrmOperasyonKartlari` secimi; operasyonu Sira artarak ekler, operasyon kartinin varsayilan istasyonu ve ona bagli istasyonlari da otomatik ReceteIstasyon olarak ekler.
- `Operasyon Sil` (BtnOperasyonSil) — Operasyon_Sil_Kontrol sonrasi operasyonu, bagli istasyon ve carilerini listeden cikarir, siralari yeniden numaralandirir.
- `Istasyon Ekle` (BtnIstasyonEkle) — secili operasyona `FrmIstasyonKartlari` ile istasyon ekler (operasyon koduna filtreli).
- `Istasyon Sil` (BtnIstasyonSil) — Istasyon_Sil_Kontrol sonrasi istasyonu ve bagli carileri cikarir.
- `Cari Ekle` (BtnCariEkle) — secili istasyona `FrmMikroCariListesi` ile cari ekler (ReceteIstasyonCari).
- `Cari Sil` (BtnCariSil) — secili cariyi cikarir.
- `Kapat` — kapat.
**Cagirdigi katmanlar:**
- Manager: `ReceteOperasyonManager.GetOperasyonKayitEdit(rcAId)` — recete + operasyonlar + istasyonlar + cariler modelini doldurur.
- Manager: `ReceteOperasyonManager.OperasyonKaydet(model, yenikayit)` — transaction sil-yaz + tablo adi guncelleme proclari.
- Manager: `ReceteOperasyonManager.OperasyonSil/OperasyonSilKontrol/IstasyonSilKontrol` — silme ve bagli hareket kontrolleri.
- Service: `IIstasyonKartiService.SelectFirst/SelectList(...)` — operasyon karti varsayilan istasyonu ve bagli istasyonlari.
- Service: `IGenelService.GrupListesi("ReceteAna","Grubu")` — grup combobox.
- SQL/Prosedur: `Operasyon_Sil_Kontrol`, `Istasyon_Sil_Kontrol` — bagli hareket adedi kontrolu. `ReceteOperasyon_Tablo_AdGuncelle`, `ReceteIstasyon_Tablo_AdGuncelle` — kayit sonrasi ad/iliski guncelleme.
**Istasyon sirasiyla iliskisi:** DOGRUDAN. Operasyon.Sira burada girilir/kontrol edilir (1'den baslayan, tekil, ardisik); bu deger uretim girisinde `Uretim_PlanlananGuncelle` prosedurunun "operasyon Sira N uretimini Sira N+1 planlananina tasi" mantiginin temelidir. IstasyonGruplamaKullan=0 senaryosunda ReceteIstasyon (operasyon-istasyon) eslesmeleri kullanilir.
**Notlar:** OperasyonSiraKontrol Sira=1 zorunlulugu ve tekil ardisik sira sartini denetler. Operasyon ekleme, operasyon kartinin VarsayilanIstasyonKodu ve operasyona bagli istasyonlari otomatik getirir.

---

### Recete Aciklamalari (`FrmReceteAciklamalar.cs` / `FrmReceteAciklamalar.Designer.cs`)
**Ne ise yarar:** Bir receteye serbest aciklama satirlari (AciklamaDeger) atar; "ReceteAciklama" modulune tanimli aciklama kod sablonlarini (AciklamaKod) baz alir.
**Once ne olmali (onkosul):** Kayitli bir recete (`Recete.Id` dolu) gerekir; `FrmReceteED` -> Aciklamalar butonundan acilir.
**Sonra ne olur:** Kaydet -> mevcut receteye ait AciklamaDeger kayitlari silinip yeniden yazilir (degerService.Delete + InsertOrUpdate). Sil -> receteye bagli aciklamalari siler.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — degerleri sil-yeniden yaz.
- `Sil` (BtnSil) — receteye bagli aciklamalari sil.
- Grid (myView1) — Sira ve Deger1 kolonlari duzenlenebilir.
**Cagirdigi katmanlar:**
- Service: `IAciklamaDegerService.SelectListWhere(where)` / `Delete` / `InsertOrUpdate` — receteye ait aciklama degerleri.
- Service: `IAciklamaKodService.SelectListWhere(where)` — "ReceteAciklama" modulu kod sablonlari (deger yoksa sablondan uretir).
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Deger yoksa AciklamaKod listesinden satirlar uretilip EntId=Recete.Id ile baglanir. Kaydette null Deger1/2/3 bos string'e cevrilir.

---

### Recete Maliyet (Genel) (`FrmReceteMaliyetGenel.cs` / `FrmReceteMaliyetGenel.Designer.cs`)
**Ne ise yarar:** Recetenin stok maliyeti (varsayilan stoklarin Mikro standart maliyeti x miktar) + operasyon maliyeti toplamini hesaplar ve genel maliyeti gosterir; yazdirilabilir.
**Once ne olmali (onkosul):** `FrmReceteED` -> Stok Maliyet ile acilir; `Model` (ReceteKayitModel klonu) set edilmis olmali.
**Sonra ne olur:** Bagla -> stok ve operasyon maliyetleri hesaplanir, ekranda gosterilir (DB'ye yazma yok). Yazdir -> DataSet'e cevirip rapor basar.
**Butonlar & kisayollar:**
- `Bagla` (BtnBagla) — maliyetleri yeniden hesaplar.
- `Yazdir` (BtnYazdir) — "ReceteMaliyet" raporunu (StokMaliyet/OperasyonMaliyet/ToplamMaliyet tablolari) basar.
- `Kapat` (BtnKapat) — kapatir.
**Cagirdigi katmanlar:**
- Manager: `MikroReceteManager.GetStokStandartMaliyetler(stokKodlari)` — Mikro STOKLAR.sto_standartmaliyet degerleri.
- Service: `_db.ReceteOperasyon.SelectList(c => c.RcAId == id)` — operasyon maliyetleri (MaliyetFiyat toplami).
- Yazdirma: `DataSet.Yaz("ReceteMaliyet", false)` — rapor cikti.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Sadece VarsayilanStokKodu dolu detaylar maliyete dahil edilir.

---

### Recete Maliyet (Stoklar) (`FrmReceteMaliyetStoklar.cs` / `FrmReceteMaliyetStoklar.Designer.cs`)
**Ne ise yarar:** Recetedeki stoklarin farkli maliyet bazlarini (Son Alis, Son 5 Alis Ortalama, Standart Maliyet, Son Sayim Giris, Devir Giris) miktar x fiyat olarak ayri ayri hesaplar ve toplamlarini gosterir; yazdirilabilir.
**Once ne olmali (onkosul):** `FrmReceteListesi` -> Stok Maliyet veya `FrmReceteED` ile acilir; `IdGuid` (recete) set edilmis olmali (model DB'den taze cekilir).
**Sonra ne olur:** Bagla -> recete detaylari + Mikro fiyatlar birlestirilip maliyet kolonlari hesaplanir (DB'ye yazma yok). Yazdir -> rapor basar.
**Butonlar & kisayollar:**
- `Bagla` (BtnBagla) — receteyi cekip maliyetleri hesaplar.
- `Yazdir` (BtnYazdir) — "ReceteStoklarMaliyet" raporunu (StokMaliyet/ToplamMaliyet) basar.
- `Kapat` (BtnKapat) — kapatir.
**Cagirdigi katmanlar:**
- Manager: `ReceteManager.GetReceteKayit(IdGuid)` — recete modelini getirir.
- Manager: `MikroReceteManager.GetStokSonAlisSatisFiyatlar(stokKodlari)` — Mikro fn_by_Stok_Son5_Giris_Fiyati ve sto_standartmaliyet uzerinden 5 maliyet bazi.
- Yazdirma: `DataSet.Yaz("ReceteStoklarMaliyet", false)`.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Maliyet bazlari `fn_by_Stok_Son5_Giris_Fiyati(sto_kod,NULL,n)` fonksiyonunun n=0..3 parametreleriyle gelir.

---

### Recete Sec (`FrmReceteSec.cs` / `FrmReceteSec.Designer.cs`)
**Ne ise yarar:** Sade recete secim/arama diyalogu (tek grid). Baska ekranlarda recete secmek icin kullanilir; secim modunda secili receteyi dondurur, normal acilista `FrmReceteED` ile duzenleme yapar.
**Once ne olmali (onkosul):** Secim modunda `SecimIcinAcildi=true` ile acilir. Form acilisinda filtresiz arar.
**Sonra ne olur:** Cift-tik/Enter -> secim modunda SecilenRow/SecilenKod/SecilenId set edilip Secildi=true ile kapanir; degilse `FrmReceteED` acilir.
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — Kodu/Adi/Grup/Ara filtreleriyle yeniden listeler.
- `Temizle` (BtnTemizle) — filtreleri bosaltir.
- Grid cift-tik / Enter (myView1) — sec veya duzenle.
- `Kapat` — kapat.
**Cagirdigi katmanlar:**
- Service: `IReceteAnaService.SelectListWhere(where)` — receteleri listeler.
- Service: `IGenelService.GrupListesi("ReceteAna","Grubu")` — grup combobox.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** `FrmReceteListesi`'nin sadelestirilmis muadili; detay/operasyon gridleri yoktur.

---

### Recete Grup Listesi (`FrmReceteGrupListesi.cs` / `FrmReceteGrupListesi.Designer.cs`)
**Ne ise yarar:** Recete gruplarini (ReceteGrup = birden cok receteyi takim halinde toplayan ust kayit) listeler; secili grubun icindeki receteleri alt gridde gosterir. Secim modunda grup dondurur.
**Once ne olmali (onkosul):** Acilista grup filtreleri ve grup comboboxu yuklenir, liste cekilir.
**Sonra ne olur:** Cift-tik/Enter -> secim modunda grubu dondurur; degilse `FrmReceteGrupED` (grup kart) acilir. Grup degisiminde alt grid grubun receteleriyle dolar.
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — Kodu/Aciklama/Grup filtreleriyle listeler.
- `Temizle` (BtnTemizle) — filtreleri bosaltir.
- `Grup/Takim Ekle` (BtnGrupTakimEkle) — bos `FrmReceteGrupED` acar.
- Grid cift-tik / Enter (myView1) — sec veya `FrmReceteGrupED` ile duzenle.
- `Kapat` — kapat.
**Cagirdigi katmanlar:**
- Service: `IReceteGrupService.SelectListWhere(where)` — grup listesi.
- Service: `IReceteGrupDetayService.SelectList(c => c.RcGId == id)` — gruptaki recete id'leri.
- Service: `IReceteAnaService.SelectListWhere(" where Id IN(...)")` — gruba bagli recete basliklari.
- Service: `IGenelService.GrupListesi("ReceteAna","Grubu")` — grup combobox.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Grup, recetelerin uzerinde bir "takim" katmanidir; uretim akisini dogrudan etkilemez.

---

### Recete Grup Ekle / Duzenle (`FrmReceteGrupED.cs` / `FrmReceteGrupED.Designer.cs`)
**Ne ise yarar:** Bir recete grubunun kodu, grubu, aciklamasi ile gruba dahil receteleri (ReceteGrupDetay: recete + miktar + aciklama) tanimlar.
**Once ne olmali (onkosul):** `FrmReceteGrupListesi` -> Grup/Takim Ekle (yeni) veya satira cift-tik (IdGuid dolu -> duzenleme).
**Sonra ne olur:** Kaydet -> `ReceteGrupManager.ReceteGrupKaydet` transaction ile ReceteGrup upsert + ReceteGrupDetay sil-yeniden yaz; KayitEdildi=true ile kapanir, liste yenilenir. Sil -> grup ve detaylarini siler.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — TextLeriKontrolEt (kod bossa GetEvrakNo) + AktarModele + ReceteGrupKaydet.
- `Sil` (BtnSil) — onayla sonra grubu sil.
- `Detay Ekle` (BtnDetayEkle) — `FrmReceteListesi` secimiyle gruba recete ekler (Miktar=1).
- `Detay Sil` (BtnDetaySil) — secili recete detayini cikarir.
- TxtReceteKodu buton — `GetEvrakNo("Recete")` ile grup kodu uretir.
- Grid (myView1) — Miktar ve Aciklama kolonlari duzenlenebilir.
- `Kapat` — kapat.
**Cagirdigi katmanlar:**
- Manager: `ReceteGrupManager.GetReceteKayit(id)` — grup + detaylar modeli.
- Manager: `ReceteGrupManager.ReceteGrupKaydet(model, yenikayit)` / `ReceteGrupSil(model)` — transaction sil-yaz / sil.
- Service: `IGenelService.GrupListesi("ReceteGrup","Grubu")` — grup combobox; `GetEvrakNo("Recete")` — kod.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** ReceteGrupDetay her satiri bir receteyi ve takimdaki miktarini tutar.

---

### Yardimci dosya (`ReceteStokRenkBeden.cs`)
**Ne ise yarar:** `MyUI.ReceteModule` namespace altinda bos govdeli dosya (UI mantigi icermez). Renk/beden entity'si (`My.Entities.ReceteStoklar.ReceteStokRenkBeden`) `FrmReceteStokSec`'te kullanilir; bu dosyanin kendisi aktif kod barindirmaz.
**Once ne olmali (onkosul):** -
**Sonra ne olur:** -
**Butonlar & kisayollar:** -
**Cagirdigi katmanlar:** -
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Pratikte islevsiz/placeholder dosya.
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
## Modul: MalKabul

MalKabul (Mal Kabul) modulu, bir uretim emrine (UretimEmri) bagli olarak uretilen/giren mamul-malzeme stok fislerini (UretimStokFis) listeler ve goruntuler. Fis basligi uretim emri + 1. operasyonun istasyon bilgisiyle (UretimEmri.Sira=1 -> UretimOperasyon -> UretimIstasyon) eslestirilir; fis satirlari ise UretimStokFisHareket tablosundan gelir. Modul iki formdan olusur: liste formu (`FrmMalKabulListe`) ve fis goruntuleme/edit formu (`FrmMalKabulED`). Bu modulun ekranlarinda fis ASLA bu formlardan **kaydedilmez** (FrmMalKabulED.Kaydet() govdesi yorum satiri, BtnKaydet `Visible=false`, AktarModele() bos); fisler tablet/uretim akisi (IstasyonTakip + IstasyonSevk) tarafindan olusturulur. Bu modul sadece okuma, yazdirma ve **silme** (hem UretimStokFis/UretimStokFisHareket/DepoStokHareket hem de Mikro'ya aktarilan fisin geri silinmesi) islemlerini yapar. Fis ayrica Siparis listesinden sag-tik ("Mal Kabul Fisi Ac") ile SipId uzerinden de acilabilir.

### Mal Kabul Liste (`MyUI/MalKabul/FrmMalKabulListe.cs`, `FrmMalKabulListe.Designer.cs`)
**Ne ise yarar:** Uretim stok fislerini (UretimStokFis) tarih araligi ve istasyon kodu/adi filtresiyle listeler. Bir satira cift tiklayinca o fisin detayini (FrmMalKabulED) acar. Form, FrmAna ana menusunden (`BarBtnMalKabulListe_ItemClick`) MDI child olarak acilir; ayrica baska formlardan "secim modu" (SecimIcinAcildi) ile cagrilabilir.
**Once ne olmali (onkosul):** Sistemde uretim akisi tarafindan olusturulmus UretimStokFis kayitlari bulunmali (uretim girisi -> istasyon sevk akisi fisi yaratir). Form acilir acilmaz Frk_Load icinde tarih filtresi 01.01.<yil> olarak set edilir ve `BtnAra.PerformClick()` ile otomatik arama yapilir; ek bir on kosul yoktur.
**Sonra ne olur:** Kayit/silme yapmaz; sadece okuma. Cift tik -> normal modda `FrmMalKabulED` (IdGuid=secili fis) dialog acar; secim modunda (SecimIcinAcildi) secilen fisin EvrakNo'sunu SecilenKod'a, satiri SecilenRow'a yazip Secildi=true ile formu kapatir (cagiran forma fis secimi doner). Yazdir butonu liste verisini DataSet'e cevirip "MalKabul" yazdirma sablonuyla yazdirir.
**Butonlar & kisayollar:**
- `BtnAra` — `BtnAra_Click` -> `Bagla()`; SorguAyarla()+SorguAyarlaTrh() ile WHERE kurar, `_srv.GetFisList(sor)` cagirir, gridi doldurur. (Form acilista PerformClick ile otomatik tetiklenir.)
- `BtnTemizle` — `BtnTemizle_Click`; TxtAra'yi bosaltir, TxtTarihi1'i 01.01.<yil> yapar, TxtTarihi2'yi bosaltir (arama filtrelerini sifirlar).
- `BtnYazdir` — `BtnYazdir_Click` -> `Yazdir()`; secili satir varsa `list`'i "Hareketler" DataTable'ina cevirip "MalKabul" sablonuyla `ds.Yaz("MalKabul", false)` ile yazdirir.
- `BtnDizayn` — base liste formundan gelir (yazdirma dizayn duzenleme); MalKabul kodunda ozel Click handler bagli degil.
- `BtnKapat` — base form kapatma butonu (My.Kontrol.Formlar / MyFrmListe).
- Grid: `myView1.MyEventDoubleClickEnter` (cift tik / Enter) -> `MyView1_MyEventDoubleClickEnter` -> secim modunda fis sec, normal modda FrmMalKabulED ac.
- `TxtAra` (Ara), `TxtTarihi1` (>= tarih), `TxtTarihi2` (<= tarih) — filtre giris alanlari; EnterMoveNextControl=true (Enter ile sonraki kontrole gecis).
- Not: BtnAra/BtnTemizle/BtnYazdir/BtnDizayn/BtnKapat MyFrmListe base formundan gelir; Text ve ShortcutKeys degerleri proje kaynaginda/resx'te tanimli degil (DevExpress image'li, base formda yonetilir).
**Cagirdigi katmanlar:**
- Service: `IUretimStokFisService.GetFisList(string sor)` (`UretimStokFisService.GetFisList`) — `UretimStokFisModel.GetSelectSqlCode(sor)` SQL'ini calistirir, List<UretimStokFisModel> doner.
- SQL: `UretimStokFisModel.GetSelectSqlCode` — `UretimStokFis UF LEFT JOIN UretimEmri UR (UF.UrId=UR.Id) LEFT JOIN UretimOperasyon UrO (UrO.UrId=Ur.Id AND UrO.Sira=1) LEFT JOIN UretimIstasyon UrI (UrI.UrOId=UrO.Id)` ile fis + IsEmriNo + SiparisKodu(IsEmriKodu) + 1. operasyon istasyon kodu/adi getirir. Filtre: istasyon kodu/adi LIKE (TxtAra), `UF.Durumu` (durumu alani, varsayilan bos), `UF.Tarih` araligi.
- Yazdirma: `DataSet.Yaz("MalKabul", false)` (My.Kontrol.Yazdirma uzantisi) — "MalKabul" yazdirma sablonuyla rapor uretir.
- API: -
**Istasyon sirasiyla iliskisi:** Fis basligindaki istasyon kodu/adi, uretim emrinin **1. operasyonunun** (UretimOperasyon.Sira=1) istasyonu (UretimIstasyon) uzerinden eslestirilir; SorguAyarla'daki arama da bu istasyon kodu/adina gore filtreler. SutunCaptionDegistir ile "SiparisKodu" basligi "IsEmriKodu" olarak gosterilir.
**Notlar:** `durumu` alani sabit "" (bos) — durum filtresi pratikte calismaz (UF.Durumu filtresi hep atlanir). Arama girdileri SQL'e dogrudan string interpolasyon ile gomulu (parametresiz; SQL injection riski). SutunGizle ile Id ve UrId kolonlari gizlenir, Tarih dd.MM.yyyy formatlanir.

### Mal Kabul Fisi (`MyUI/MalKabul/FrmMalKabulED.cs`, `FrmMalKabulED.Designer.cs`)
**Ne ise yarar:** Tek bir uretim stok fisini (UretimStokFis) ve satirlarini (UretimStokFisHareket) salt-okunur goruntuler; fisi (ve Mikro'ya aktarilan karsiligini) silmeyi saglar. Baslik alanlari: Is Emri No/Kodu, Tarih, Istasyon Kodu/Adi, EvrakNo, BelgeNo, Durumu. Form basligi "Mal Kabul Fisi". Iki sekilde acilir: (1) liste cift tikindan IdGuid ile, (2) Siparis listesinden SiparisIdDenAra=true + SipId ile.
**Once ne olmali (onkosul):** Goruntulenecek fisin var olmasi gerekir. IdGuid (liste cift tik) ya da SipId (Siparis listesi "Mal Kabul Fisi Ac") set edilmis olmali. SiparisIdDenAra=true ise `GetFisFirst(" where Ur.SipId='...' ")` ile siparis id'sine bagli ilk fis bulunur ve IdGuid ona set edilir.
**Sonra ne olur:** Kaydetme yapmaz (Kaydet() govdesi yorumda, BtnKaydet Visible=false). Silme: BtnSil -> `Sil()`; once `_srv.FisSil(IdGuid)` ile UretimStokFis + UretimStokFisHareket + DepoStokHareket satirlarini (FisId uzerinden, tek transaction) siler, sonra `_mngMikroKayit.DeleteMikroAktarilanFisByBelgeNo(_mdl.EvrakNo)` ile Mikro tarafinda ayni EvrakNo'lu STOK_HAREKETLERI + BEDEN_HAREKETLERI (ve aktarilan fis kaydi) silinir, KayitEdildi=true yapilip form kapatilir.
**Butonlar & kisayollar:**
- `BtnSil` — `BtnSil_Click` -> `Sil()`; "Kaydi silmek istiyormusunuz.." onayi (MesajSor) sonrasi yerel fisi + Mikro aktarilan fisi siler, formu kapatir.
- `BtnKaydet` — base form kaydet butonu, ANCAK `Visible=false` (kaydetme bu formda devre disi); Kaydet() metodu bos/yorumda.
- `BtnYeni` / `BtnDuzenle` / `BtnYazdir` / `BtnKapat` — base kayit formundan (MyFrmKayit) gelir; MalKabul kodunda ozel Click handler bagli degil (BtnYazdir bu formda kodla bos).
- `BtnIlk` / `BtnOnceki` / `BtnSonraki` / `BtnSon` — base form kayit navigasyon butonlari (ilk/onceki/sonraki/son kayda gecis); MyFrmKayit base davranisi.
- Baslik/satir alanlari: TxtIsEmriNo, TxtIsEmriKodu, TxtIstasyonKodu, TxtIstasyonAdi, TxtEvrakNo, TxtBelgeNo, TxtDurumu, TxtTarih — AktarTextlere() ile doldurulur (goruntuleme; AktarModele() bos oldugu icin geri yazma yok).
- Not: BtnSil/BtnKaydet/BtnYeni/BtnDuzenle/BtnYazdir/BtnKapat ve navigasyon butonlari MyFrmKayit base formundan gelir; Text ve ShortcutKeys degerleri proje kaynaginda/resx'te yok (base formda yonetilir). Enter ile alanlar arasi gecis (EnterMoveNextControl) aktiftir.
**Cagirdigi katmanlar:**
- Service: `IUretimStokFisService.GetFis(Guid id)` — `UretimStokFisModel.GetSelectSqlCode(" where UF.Id='...' ")` ile tek fis getirir.
- Service: `IUretimStokFisService.GetFisFirst(string sor)` — SipId'ye bagli ilk fisi getirir (SiparisIdDenAra modunda).
- Service: `IUretimStokFisService.GetStokHareketByFisId(Guid? fisId)` — `UretimStokFisHareket.GetSelectSqlCodeByFisId(fisId)` ile fis satirlarini getirir, gride baglar.
- Service: `IUretimStokFisService.FisSil(Guid? fisId)` — tek transaction icinde `DELETE FROM UretimStokFis WHERE Id=...`, `DELETE FROM UretimStokFisHareket WHERE FisId=...`, `DELETE FROM DepoStokHareket WHERE FisId=...`.
- Manager: `MikroKayitManager.DeleteMikroAktarilanFisByBelgeNo(string belgeNo)` — Mikro DB'de ayni belge no'lu BEDEN_HAREKETLERI ve aktarilan STOK_HAREKETLERI fisini (MikroStokHareketAktarilanFisler.GetDeleteSqlCodeByBelgeNo) tek transaction'da siler.
- SQL/Prosedur: dogrudan stored procedure cagrilmaz; islemler entity'lerin GetSelectSqlCode / DELETE SQL'leri ile yapilir. (Uretim_MiktarGuncelle vb. prosedurler bu modulden tetiklenmez.)
- API: -
**Istasyon sirasiyla iliskisi:** Goruntulenen fis basligindaki Istasyon Kodu/Adi, uretim emrinin 1. operasyonunun (UretimOperasyon.Sira=1) UretimIstasyon kaydindan gelir (GetSelectSqlCode JOIN'i). Grid satirlarinda GirisCikis/Sira alanlari gizlenir; satir akisi/istasyon sevkiyle dogrudan etkilesim yoktur (sadece okuma+silme).
**Notlar:** Sil() icinde Mikro silme sonucu kontrolu ters mantikla yazilmis: `if (rs2.Success) { MesajHata(rs2.Message); return; }` — Mikro silme BASARILI olunca hata mesaji gosterip return ediyor (form kapanmadan once); bu olasi bir bug. AktarModele() ve TextLeriKontrolEt() govdeleri tamamen yorum/bos; Kaydet() bu formda hicbir sey yapmaz. Grid `MyGridKayitAdi="SiparisKayit1"` (Siparis formundan kopyalanmis adlandirma). Form Designer'da Text="FrmMalKabulED" olsa da lblBaslik.Text="Mal Kabul Fisi" gosterilir.
## Modul: UretimKontroller

UretimV4 (CepPatronERP masaustu ERP) icindeki UretimKontroller modulu, saha operatorlerinin tablet uzerinden girdigi olcum/kalite kontrol kayitlarini ofis tarafinda goruntulemeye ve raporlamaya yarayan tek bir salt-okunur liste ekranindan olusur. Olcum kayitlari (UretimKontrol tablosu, Turu='OlcumGiris') asil olarak WebUretim TabletV2 projesindeki istasyon takip akisinda (Pop_IstasyonAktifIslemOlcumGiris.razor -> UretimKontrolService.SaveUretimKontrol) olusturulur; bu modulde herhangi bir kayit ekleme/silme/duzeltme yoktur. Ekran, ortak iki SQL veritabanindan UretimV3_FEZA icindeki UretimKontrol tablosunu (UretimEmri ve Personel ile JOIN ederek) okur. Modulun ana islevi: tarih ve serbest metin filtresiyle olcum kontrol kayitlarini listelemek, tolerans disi (HataliGiris) kayitlari gormek ve listeyi yazdirmak.

### Olcu Kontrol Listesi (`FrmOlcuKontrolListesi.cs` / `FrmOlcuKontrolListesi.Designer.cs`)
**Ne ise yarar:** Uretim sirasinda girilen olcum kontrol kayitlarini (Turu='OlcumGiris') bir grid uzerinde listeler. Her satirda istasyon, stok, olcum degeri (OlcumDegeri/2/3), tolerans araliklari (Min/Max), hatali giris bayragi, is emri no/kodu, kaydeden personel ve tarih gosterilir. Salt-okunur bir izleme/rapor ekranidir; veri girisi yapilmaz.
**Once ne olmali (onkosul):** Olcum kayitlarinin onceden olusmus olmasi gerekir. Bu kayitlar UretimV4'te degil, WebUretim TabletV2 saha akisinda olusur: operator istasyonu baslatip aktif islemde Olcum Girisi popup'ini (Pop_IstasyonAktifIslemOlcumGiris.razor) acar, olculen degerleri girer ve kaydeder. Kayit UretimKontrolService.SaveUretimKontrol ile UretimKontrol tablosuna Turu='OlcumGiris', UrId/UrIId/IstHrId, IstasyonKodu/Adi, StokKodu/Adi, OlcumDegeri ve tolerans araliklari (OlcumDegeriMin/Max...) ile yazilir; deger tolerans disindaysa HataliGiris=true isaretlenir. Ayrica ilgili is emrinin (UretimEmri) ve kaydeden personelin (Personel) DB'de bulunmasi gerekir (JOIN icin).
**Sonra ne olur:** Ekran salt-okunurdur; hicbir tabloyu yazmaz/degistirmez ve hicbir stored procedure cagirmaz. Yapilabilecek tek "sonra" islemi: secili kayit varken Yazdir ile listenin DevExpress rapor cikti uretimine gonderilmesidir (ds.Yaz("UretimKontrol", false)). Cift tiklama yalnizca form baska bir formdan "secim icin" acildiysa (SecimIcinAcildi=true) secilen kaydi geri dondurup formu kapatir; normal acilista cift tiklamanin etkisi yoktur.
**Butonlar & kisayollar:**
- `Ara (BtnAra)` — Filtreleri uygulayip gridi yeniden doldurur (BtnAra_Click -> Bagla()). Form acilisinda otomatik bir kez tetiklenir (Frm_Load icinde BtnAra.PerformClick()).
- `Temizle (BtnTemizle)` — Arama kutusunu bosaltir, baslangic tarihini (TxtTarihi1) yilbasina (01.01.<yil>) ceker, bitis tarihini (TxtTarihi2) bosaltir (BtnTemizle_Click). Not: temizledikten sonra otomatik arama yapmaz; yeniden Ara'ya basmak gerekir.
- `Yazdir (BtnYazdir)` — Secili kayit varsa listeyi (list) "Hareketler" tablosu olarak bir DataSet'e koyup "UretimKontrol" rapor sablonuyla yazdirir (BtnYazdir_Click -> Yazdir()).
- `Dizayn (BtnDizayn)` — Base form (MyFrmListe) uzerinden gelen rapor/grid dizayn butonu; bu formda ozel bir Click handler baglanmamistir (base davranis).
- `Kapat (BtnKapat)` — Base form (MyFrmListe) uzerinden gelen formu kapatma butonu; bu formda ozel handler yok (base davranis).
- `Cift tiklama / Enter (myView1.MyEventDoubleClickEnter)` — Secim modunda (SecimIcinAcildi) secili UretimKontrol satirini SecilenRow'a atar, Secildi=true yapar ve formu kapatir; normal modda islem yapmaz.
- Arama alanlari: `TxtAra` (serbest metin: IstasyonKodu/IstasyonAdi/IsEmriNo/SiparisKodu icinde LIKE), `TxtTarihi1` (baslangic tarihi >=), `TxtTarihi2` (bitis tarihi <=).
- Not: Buton Text/ShortcutKeys (Enter=Kaydet, Esc=Kapat vb.) tanimlari bu formda degil, harici base form MyFrmListe (My.Kontrol.Formlar, DLL) icindedir; .Designer.cs'de yalnizca gorunum/renk ve Click baglamalari vardir.
**Cagirdigi katmanlar:**
- Service: `IUretimKontrolService.GetViewListWhere(string whereSql)` (UretimKontrolService) — UretimKontrol tablosunu UretimEmri (UrId) ve Personel (KayitEden=Personel.Kodu) ile LEFT JOIN ederek IsEmriNo, IsEmriKodu (SiparisKodu) ve Kullanici (Adi+Soyadi) ek alanlariyla birlikte verilen WHERE kosuluna gore okur; Dapper Query<UretimKontrol> ile calisir, sonucu SuccessDataResult/ErrorDataResult olarak doner.
- Service (base): `IGenelService` (Ortak.DbPro.GenelServis) — formda alani tanimli ancak bu ekranda dogrudan cagrilmaz (genel yardimci servis referansi).
- DataAccess: `UretimKontrolDal : BaseDal<UretimKontrol>` — tablo erisimi icin Dapper tabanli DAL; bu ekranda yalnizca okuma (Query) yolu kullanilir.
- SQL/Prosedur: Yok. Ekran herhangi bir stored procedure cagirmaz (Uretim_MiktarGuncelle / Uretim_PlanlananGuncelle / Uretim_SonrakiIstasyonaGonder / Uretim_DurumGuncelle ile iliskisi yoktur). Tek SQL, GetViewListWhere icindeki dinamik SELECT + WHERE sorgusudur.
- Yazdirma: `ds.Yaz("UretimKontrol", false)` (My.Kontrol.Yazdirma) — list -> DataTable("Hareketler") -> DataSet("UretimKontrolDS") rapor cikti motoru.
- API: Yok (UretimV4 masaustu, DB'ye dogrudan baglanir; API kullanmaz).
**Istasyon sirasiyla iliskisi:** Dogrudan miktar/akis motoruna katki vermez; istasyon sirasini (operasyon Sira N -> N+1) etkilemez. Yalnizca kaydi istasyon bazinda gosterir (IstasyonKodu/IstasyonAdi) ve her olcum satiri bir UretimIstasyonHareket'e (IstHrId) ile UretimIstasyon'a (UrIId) ve is emrine (UrId) baglidir. Tolerans disi (HataliGiris) olcumler bu ekranda gorulse de istasyon sevkini/durumunu burada degistirmez; o akis TabletV2 tarafinda yurutulur.
**Notlar:**
- Filtre sabit olarak `UK.Turu='OlcumGiris'` ile dusunulmus olsa da (SorguAyarla icinde t1 degiskenine atanip hemen TxtAra.Text ile ezilmesi nedeniyle) bu kosul sorguya EKLENMEZ; pratikte tum UretimKontrol turleri listelenir. Bu, koddaki bir hata/olu satirdir (string t1 = "...OlcumGiris..."; hemen ardindan t1 = TxtAra.Text.Trim()).
- Form acilisinda baslangic tarihi her zaman icinde bulunulan yilin 01 Ocak'ina set edilir; bitis tarihi bos gelir (ust sinir yok).
- Arama sorgulari parametresiz string interpolasyonu ile kurulur (LIKE '%...%' ve CAST tarih kosullari) - SQL injection acisindan korunmasizdir; analiz notu olarak belirtilmistir.
- Gizlenen kolonlar: Id, UrId, UrIId, IstHrId (SutunGizle); Tarih kolonu "dd.MM.yyyy HH:mm" formatlidir. Grid yerlesimi MyGridKayitAdi="OlcuKontrolListesi" / MyGridKayitAdi GridYerlesimYukle() ile kullaniciya ozel saklanip yuklenir.
- Entity (UretimKontrol) 5 olcum degeri (OlcumDegeri..5) ve 5 tolerans cifti (Min/Max..5) tasir; .cs entity'sinde OlcumDegeri4/5 alanlari yokken (Ignore'lu IsEmriNo/Kullanici/IsEmriKodu var) tablo create dosyasinda (UretimKontrolCreates.cs) OlcumDegeri4/OlcumDegeri5 kolonlari da olusturulur.
## Modul: UretimTalepler

Uretim Talepleri modulu, ofis/yonetim tarafindan bir istasyona uretilmesi istenen mamul/recete kalemlerini "uretim talebi" evraki olarak girmek ve listelemek icin kullanilir. Modul iki formdan olusur: liste ekrani (`FrmUretimTalepList`) ve kayit/duzenleme ekrani (`FrmUretimTalepED`). Veriler `UretimTalep` (evrak basligi) ve `UretimTalepHareket` (kalem satirlari) tablolarinda tutulur; her satirda hedef istasyon (IstasyonKodu/IstasyonAdi), stok/recete (StokKodu/StokAdi), miktar, birim, parti ve lot bilgileri yer alir. Stok kalemleri Recete Listesi (`FrmReceteListesi`) uzerinden secilir. Modul, bu sahadaki diger uretim/akis motoruna (Uretim_MiktarGuncelle, Uretim_PlanlananGuncelle vb. prosedurler) DOGRUDAN baglanmaz; sadece bir talep/evrak kaydi olusturur. Kayit, `UretimTalepManager` uzerinden tek transaction icinde yapilir (basligi InsertOrUpdate, hareketleri sil-yeniden yaz). Formlar `My.Kontrol.Formlar` icindeki ortak temel siniflardan turer (liste: `MyFrmListe`, kayit: `MyFrmKayit`) ve bu temel siniflarin standart butonlarini (Kaydet/Kapat/Sil/Yazdir/navigasyon) miras alir.

### Uretim Talep Liste (`MyUI/UretimTalepler/FrmUretimTalepList.cs` + `.Designer.cs`)
**Ne ise yarar:** Girilmis uretim talep evraklarini tarih araligina gore listeler (ust grid: `myGrid1`/`myView1`) ve secili evrakin kalem satirlarini alt gridde (`myGrid2`/`myView2`) gosterir. Buradan yeni talep eklenir veya mevcut talep cift tiklayarak/Enter ile duzenlenir. Form ayrica baska bir ekrandan "secim icin" acildiginda (SecimIcinAcildi) talep evrakini secip geri dondurebilir.
**Once ne olmali (onkosul):** Uygulamaya giris yapilmis ve veritabani baglantisi (`Ortak.DbPro`) hazir olmali. Listede evrak gorunmesi icin onceden en az bir uretim talebi kaydedilmis olmali (yoksa liste bos gelir).
**Sonra ne olur:** Bu ekran salt okuma/listelemedir; dogrudan tablo degistirmez. "Ur.Talep Ekle" veya cift tiklama ile `FrmUretimTalepED` acilir; orada yapilan kaydet/sil islemi `UretimTalep` ve `UretimTalepHareket` tablolarini gunceller, sonra `ActionAktar = Bagla` callback'i ile liste yeniden yuklenir. Secim modunda (`SecimIcinAcildi`) cift tik/Enter -> `SecilenKod = EvrakNo`, `SecilenRow`, `Secildi=true` set edilip form kapanir.
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `BtnAra_Click` -> `Bagla()`: tarih filtreleriyle (`TxtTarihi1`/`TxtTarihi2`) sorgu kurup `UretimTalep` listesini yeniden ceker.
- `BtnTemizle` ("Temizle") — `BtnTemizle_Click`: `TxtTarihi1` ve `TxtTarihi2` tarih filtre kutularini bosaltir (listeyi otomatik yenilemez).
- `BtnEkleUretimTalep` (Text: "Ur.Talep Ekle") — `BtnEkleUretimTalep_Click`: yeni kayit icin `FrmUretimTalepED` acar (`ActionAktar = Bagla`), `ShowDialog()`.
- `BtnKapat` (temel `MyFrmListe` butonu) — formu kapatir.
- `BtnYazdir` / `BtnDizayn` (temel `MyFrmListe` butonlari, Designer'da tanimli) — bu formda ozel click handler baglanmamis; grid yazdirma/dizayn temel davranisi.
- Grid cift tik / Enter — `MyView1_MyEventDoubleClickEnter`: secim modunda evraki dondurur, normal modda `FrmUretimTalepED` (duzenleme) acar.
- Ust gridde satir degisimi — `MyView1_FocusedRowChanged` -> `BaglaHareket(itm.UrtTlpId)`: secili evrakin hareketlerini alt gride yukler.
- `TxtTarihi1`/`TxtTarihi2` (MyDateEdit) — tarih araligi filtresi; `Frm_Load`'da `TxtTarihi1` bugun-7 gune set edilir.
**Cagirdigi katmanlar:**
- Manager/Service: `IUretimTalepService.SelectListWhere(string)` (`_srv`, = `Ortak.DbPro.UretimTalep`) — `where 1=1` + tarih sartiyla `UretimTalep` kayitlarini ceker.
- Manager/Service: `IUretimTalepHareketService.SelectList(c => c.UrtTlpId == urtTlpId)` (`_srvHareket`) — secili evrakin `UretimTalepHareket` satirlarini ceker.
- Manager/Service: `UretimTalepManager` (`_mng`, `Frm_Load`'da kurulur) — bu formda olusturulur ancak metotlari dogrudan cagrilmaz (kayit/sil islemleri ED formuna devredilir).
- Manager/Service: `IGenelService` (`_srvGenel`) — alanı tanimli, bu formda aktif kullanilmiyor.
- SQL/Prosedur: dogrudan stored procedure cagrilmaz; sorgu BaseService/DAL uzerinden parametrik `WHERE` ile uretilir (filtre: `CAST(coalesce(Tarih,'1901-01-01') AS DATE)` araligi).
- API: -
**Istasyon sirasiyla iliskisi:** Dolayli. Hareket satirlarinda hedef `IstasyonKodu`/`IstasyonAdi` gorunur; ancak bu liste ekrani operasyon-sira/akis motorunu (Uretim_MiktarGuncelle, Uretim_SonrakiIstasyonaGonder) tetiklemez, yalnizca talep evraklarini gosterir.
**Notlar:** `SorguAyarla()` icindeki kod/cari/durum filtreleri tamamen yorum satiri; aktif filtre yalnizca tarih araligi (`SorguAyarlaTrh`). `SutunGizle`/`SutunGizle2` ile teknik kolonlar (UrtTlpId, Ent, EntId, EntKodu, EntKodu2, EntTarih, UrtTlpHrId) gizlenir. Grid yerlesimleri `MyGridKayitAdi` ile saklanir ("UretimTalepListesi", "UretimTalepListesiDetaylar"). `Turu = "Siparis"` alani tanimli fakat aktif sorguda kullanilmiyor.

### Uretim Talep Kayit (`MyUI/UretimTalepler/FrmUretimTalepED.cs` + `.Designer.cs`)
**Ne ise yarar:** Tek bir uretim talep evrakini olusturmak/duzenlemek icin kullanilir. Ust kisimda evrak basligi (Tarih, Evrak No, Aciklama); alt kisimda kalem grid'i (`myGrid1`/`myView1`) ile her satir icin hedef istasyon (lookup), stok/recete, miktar, birim, aciklama, parti, lot girilir. Kalemler Recete Listesinden secilerek eklenir.
**Once ne olmali (onkosul):** `FrmUretimTalepList` uzerinden "Ur.Talep Ekle" (yeni) veya cift tik/Enter (`IdGuid` dolu = duzenleme) ile acilmali. Istasyon lookup'lari icin `IstasyonKarti` kayitlari (`Ortak.DbPro.IstasyonKarti`) tanimli olmali. Recete secimi icin `ReceteAna` kayitlari (Recete modulu) tanimli olmali. Kaydet/Sil butonlarinin aktif olmasi icin lisans aktif olmali (`Ortak.LisansAktif`).
**Sonra ne olur:** "Kaydet" -> dogrulama (`TextLeriKontrolEt`) + modele aktarim (`AktarModele`) sonrasi `UretimTalepManager.Kaydet(_mdl, Hareketler)` ile TEK transaction icinde: `UretimTalep` basligi InsertOrUpdate edilir, ardindan o evraka ait tum `UretimTalepHareket` satirlari `Delete` edilip yeniden `InsertOrUpdate` yapilir (sil-yaz deseni). Basarili olursa `KayitEdildi=true`, `ActionAktar?.Invoke()` (liste yenilenir) ve form kapanir. "Sil" -> `UretimTalepManager.Sil(_mdl)` ile baslik ve tum hareketler tek transaction'da silinir. Bu modul, kayittan sonra herhangi bir akis prosedurunu (Uretim_MiktarGuncelle vb.) cagirmaz.
**Butonlar & kisayollar:**
- `BtnKaydet` (temel `MyFrmKayit` butonu) — `BtnKaydet_Click` -> `Kaydet()`: dogrula, aktar, `_mng.Kaydet(...)`.
- `BtnSil` (temel `MyFrmKayit` butonu) — `BtnSil_Click` -> `Sil()`: onay sorar, `_mng.Sil(...)`.
- `BtnStokSec` (Text: "F5") — `BtnStokSec_Click` -> `StokSecReceteden()`: Recete Listesinden kalem secip yeni `UretimTalepHareket` ekler. Klavye kisayolu: **F5** (`Frm_KeyDown` -> `BtnStokSec.PerformClick()`).
- `BtnStokSil` (Text: "F8") — `BtnStokSil_Click` -> `StokSil()`: secili kalem satirini listeden cikarir (onay sorar). Klavye kisayolu: **F8** (`Frm_KeyDown` -> `BtnStokSil.PerformClick()`).
- `TxtSiparisKodu` (Evrak No) editor butonu — `TxtSiparisKodu_ButtonClick`: dolu ise onay sorduktan sonra `EvrakNoAl()` ile yeni evrak no uretir.
- `BtnKapat` (temel `MyFrmKayit` butonu) — formu kapatir.
- `BtnYazdir` / `BtnYeni` / `BtnDuzenle` / navigasyon butonlari `BtnIlk`/`BtnOnceki`/`BtnSonraki`/`BtnSon` (temel `MyFrmKayit`, Designer'da tanimli) — bu formda ozel click handler baglanmamis; temel form davranislari.
- Grid hucre editoru acilinca — `MyView1_ShownEditor`: TextEdit ise mevcut metni otomatik secer (SelectAll).
- Istasyon lookup degisince — `colCmbIstasyonKodu_EditValueChanged` / `colCmbIstasyonAdi_EditValueChanged`: secilen `IstasyonKarti`'na gore satirin IstasyonKodu ve IstasyonAdi hucrelerini eslestirir.
**Cagirdigi katmanlar:**
- Manager/Service: `UretimTalepManager.GetTalepNew()` — bos `UretimTalep` dondurur (yeni kayit).
- Manager/Service: `UretimTalepManager.GetTalep(Guid?)` -> `UretimTalepService.SelectFirst(c => c.UrtTlpId == id)` — duzenlemede evrak basligini ceker.
- Manager/Service: `UretimTalepManager.GetTalepHareketler(Guid?)` -> `UretimTalepHareketService.SelectList(c => c.UrtTlpId == ...)` — evrakin kalem satirlarini ceker.
- Manager/Service: `UretimTalepManager.Kaydet(UretimTalep, List<UretimTalepHareket>)` — tek transaction: `UretimTalep.InsertOrUpdate` + `UretimTalepHareket.Delete(by UrtTlpId)` + `UretimTalepHareket.InsertOrUpdate(list)`; hata olursa rollback.
- Manager/Service: `UretimTalepManager.Sil(UretimTalep)` — tek transaction: `UretimTalep.Delete` + `UretimTalepHareket.Delete(by UrtTlpId)`.
- Manager/Service: `IIstasyonKartiService.SelectListWhere("")` (`_srvIstasyon`) — istasyon lookup veri kaynagini (`bsCari`) doldurur.
- Manager/Service: `IGenelService.GetEvrakNo("UretimTalep")` (`_srvGenel`) -> `GenelDal.GetEvrakNo` — `AyarSayac` tablosundan "UretimTalep" sayacini okuyup formatlar (BasinaEkle + sifir dolgu/BasamakSayisi) ve sayaci 1 artirir.
- Manager/Service: `IMikroStokService` (`_srvCari = Ortak.DbMikro.Stoklar`) — alanı tanimli; kalem secimi recete uzerinden yapildigi icin aktif yol kullanmiyor (`StokTekSec__` metodu FrmMikroStokListesi ile stok secer ama hicbir butona bagli degil = olu kod).
- UI cagrisi: `FrmReceteListesi` (SecimIcinAcildi) — `StokSecReceteden()` icinde recete secilir; secilen `ReceteAna`'dan ReceteKodu->StokKodu, ReceteAdi->StokAdi, EntegreBirim->Birimi ile yeni hareket olusturulur.
- SQL/Prosedur: Dogrudan stored procedure cagrilmaz. `GetEvrakNo` icindeki inline SQL (AyarSayac okuma/guncelleme) tek SQL etkilesimidir.
- API: -
**Istasyon sirasiyla iliskisi:** Her kalem satirinda hedef `IstasyonKodu`/`IstasyonAdi` lookup ile secilir; bu, talebin hangi istasyonda uretilecegini belirtir. Ancak modul operasyon-sira tabanli akis motorunu (Uretim_PlanlananGuncelle / Uretim_SonrakiIstasyonaGonder) calistirmaz; sadece istasyon bazli talep kaydi tutar.
**Notlar:** Dogrulama (`TextLeriKontrolEt`): Evrak No bossa otomatik `EvrakNoAl()` cagrilir, Tarih bossa simdiki zaman atanir; her kalemde IstasyonKodu bos olamaz ve Miktar > 0 olmali, aksi halde hata mesaji. `AktarModele` icinde Id'ler `MyGuid.NewGuid()` ile uretilir ve audit alanlari (Kullanici, KayitEden=`Ortak.KullaniciAdi`, KayitTarihi) doldurulur; ayrica baslik IstasyonKodu/IstasyonAdi tum satirlara kopyalanir (NOT: baslikta IstasyonKodu/IstasyonAdi'yi text'lere set eden bir kontrol yok, bu yuzden `itm.IstasyonKodu = _mdl.IstasyonKodu` satir bazinda lookup ile girilen degeri baslik degeriyle (genelde bos) ezme riski tasir - bilinen dikkat noktasi). Grid kolonlarindan yalnizca Miktar, Aciklama, Parti, Lot editlenebilir (yesil baslikli); IstasyonKodu/IstasyonAdi lookup ile editlenir; StokKodu/StokAdi/Birimi salt okunur. `colMiktar` formatı "n4" (4 ondalik). `bs` (BaseForm BindingSource) Designer'da grid DataSource olarak atanmis gorunse de kodda grid `Hareketler` listesine baglaniyor.
## Modul: MikroModul

MikroModul, UretimV4 (CepPatronERP) masaustu uygulamasinin Mikro ERP (MikroDB_V16_FEZA24) ile entegrasyon koprusudur. Iki yonlu calisir: (1) Mikro tarafindan veri okuma/secme — Cari, Stok, Siparis ve Recete listeleri (cogu zaman baska formlara "secim penceresi" olarak acilir, `SecimIcinAcildi` bayragiyla); (2) Uretim sonuclarini Mikro'ya geri yazma — is emri uretim/fire/sarf hareketlerini Mikro `STOK_HAREKETLERI` (+ `BEDEN_HAREKETLERI`, `PARTILOT`) tablolarina fis olarak kaydetme, kaydedilen fisleri listeleme/silme. Tum formlar `My.Kontrol` kutuphanesindeki base formlardan turer: liste formlari `MyFrmListe` (arama paneli + grid + alt buton seridi `BtnAra`/`BtnTemizle`/`BtnYazdir`/`BtnDizayn`/`BtnKapat`), kayit/popup formlari `MyFrmKayit` (`BtnKaydet`/`BtnKapat`/`BtnSil` + navigasyon butonlari). Veri erisimi iki ayri DatabaseFactory uzerinden yapilir: `Ortak.DbMikro` (Mikro ERP) ve `Ortak.DbPro` (UretimV3_FEZA uretim DB). Mikro'ya yazma mantigi `MikroKayitManager` (transaction + parti/lot/renk/beden hesaplama) ve `MikroConvertManager` (StokHareketleriModel -> MikroStokHareketleri fis turune gore donusum, evrak seri/sira atama) siniflarinda toplanir; fis turleri kullanici tanimli `MikroEntegre` ayarlarindan (`Ortak.MikroEntAyarlar`) okunur.

### Mikro Cari Listesi (`FrmMikroCariListesi.cs` / `.Designer.cs`)
**Ne ise yarar:** Mikro ERP'deki cari (musteri/tedarikci) kartlarini arar ve listeler. Cogunlukla baska ekranlardan "cari sec" diyalogu olarak acilir (FrmUretimEmriED, FrmIstasyonKartlari, FrmReceteOperasyonED, OperasyonIstasyonControlV2 vb.).
**Once ne olmali (onkosul):** Mikro DB baglantisi (`Ortak.DbMikro`) hazir olmali. Secim modunda acan formun `SecimIcinAcildi = true` set etmesi gerekir.
**Sonra ne olur:** Salt-okunur listeleme; Mikro'da degisiklik yapmaz. Secim modunda satira cift tiklayinca `SecilenKod` (cari_kod), `SecilenRow` (MikroCari) doldurulur, `Secildi=true` yapilip form kapanir; cagiran form bu degerleri okur.
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — arama kriterleriyle listeyi yeniden yukler (`Bagla()`).
- `Temizle` (BtnTemizle) — Kodu/Unvani/Ara metin kutularini temizler.
- `Yazdir`/`Dizayn` (BtnYazdir/BtnDizayn) — grid yazdirma/kolon dizayni (base form ozelligi).
- `Kapat` (BtnKapat) — formu kapatir.
- Grid cift tik / Enter — secim (MyEventDoubleClickEnter).
- Arama kutulari: `Kodu`, `Ünvanı`, `Ara` (kod+unvan birlikte).
**Cagirdigi katmanlar:**
- Service: `IMikroCariService.GetViewListWhere(where)` (`Ortak.DbMikro.Cariler`) — verilen WHERE ile cari view listesini ceker (cari_kod / cari_unvan1 / cari_unvan2 LIKE filtreleri SorguAyarla ile uretilir).
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Filtreler string birlestirme ile uretilir (parametrik degil). `CrGuid` kolonu gizlenir.

### Mikro Stok Listesi (`FrmMikroStokListesi.cs` / `.Designer.cs`)
**Ne ise yarar:** Mikro ERP stok kartlarini ana grup, stok cinsi (Mamul/Hammadde/Yari Mamul vb.) ve kod/ad ile arar ve listeler. Recete tasarim ekranlarinda (FrmReceteED, FrmReceteDetayED, FrmUretimTalepED) stok secimi icin acilir.
**Once ne olmali (onkosul):** `Ortak.DbMikro` ve `Ortak.MikroStokGrubu` ayari hazir olmali. `TumStoklar=true` ise acilista otomatik yukleme yapilmaz (kullanici filtreleyip arar). Secim modunda `SecimIcinAcildi=true`.
**Sonra ne olur:** Salt-okunur listeleme. Secim modunda cift tik/Enter ile `SecilenKod`=StokKodu, `SecilenRow`=MikroStok doldurulur, form kapanir.
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — `Bagla()` ile filtreli listeyi yukler.
- `Temizle` (BtnTemizle) — StokKodu/StokAdi/AnaGrubu/Ara alanlarini temizler.
- `Kapat` / `Yazdir` / `Dizayn` — base form butonlari.
- Grid cift tik / Enter — secim.
- Filtreler: `StokKodu`, `StokAdi`, `AnaGrubu` (combo), `StokCinsi` (combo), `Ara`.
**Cagirdigi katmanlar:**
- Service: `IMikroStokService.GetViewListWhere(where, Ortak.MikroStokGrubu)` (`Ortak.DbMikro.Stoklar`) — stok view listesi (sto_kod/sto_isim/anagrup/sto_cins filtreli).
- Service: `IMikroGenelService.GrupListesi("STOKLAR", Ortak.MikroStokGrubu)` — ana grup combosu icin distinct grup listesi.
- Manager: `MikroStokCinsiManager.GetCinsListFull()` — stok cinsi (kod+ad) listesini doldurur; `GetCinsiKodu(ad)` ile secilen cinsin sto_cins kodu bulunur.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Stok cinsi secilmezse (-2) cins filtresi uygulanmaz.

### Mikro Siparis Listesi (`FrmMikroSiparisListesi.cs` / `.Designer.cs`)
**Ne ise yarar:** Mikro ERP'deki acik/kapali satis siparislerini tarih ve cari ile listeler; secilen siparisin satir hareketlerini alt gridde gosterir. Sag-tik menusunden secili Mikro siparisinden UretimV4 is emri (Siparis/MikroSiparis) olusturulur. UretimV4 tarafinda zaten aktarilmis siparisler yesil/menekse renkle isaretlenir.
**Once ne olmali (onkosul):** `Ortak.DbMikro` ve `Ortak.DbPro` hazir olmali. Is emri olusturmak icin ilgili Mikro stok kodlarina karsilik UretimV4'te `ReceteAna.EntegreStokKodu` eslesen receteler tanimli olmali.
**Sonra ne olur:** Listeleme salt-okunur. Sag-tik "Uretim EmriOlustur/Guncelle" -> `FrmMikroUretimEmriOlustur` acilir; kayit sonrasi UretimV4 `Siparis`/`SiparisHareket`/`SiparisHareketDetay` tablolarina yazilir ve liste `Aktarildi` olarak isaretlenir (geri cagrim `Action=Bagla`).
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — `Bagla()` filtreli liste + ilk siparisin hareketlerini yukler.
- `Temizle` (BtnTemizle) — kod/ad/ara/tarih filtrelerini temizler.
- `Kapalı Siparişler` (ChcSiparisAcikKapali) — isaretli=Kapali, degil=Acik siparisler.
- Tarih filtreleri: `TxtTarihi1` (varsayilan: bugun-1 yil), `TxtTarihi2`.
- Sag-tik menu: `Uretim EmriOlustur/Guncelle` — FrmMikroUretimEmriOlustur acar.
- Grid cift tik/Enter (secim modunda) — `SecilenKod`=CariKodu ile kapanir.
**Cagirdigi katmanlar:**
- Service: `IMikroSiparisService.GetViewListWhere(where)` (`Ortak.DbMikro.Siparisler`) — siparis view (cari kodu/unvani, tarih, acik/kapali filtreli).
- Service: `IMikroSiparisHareketService.GetViewListSeriSira(seri, sira)` (`Ortak.DbMikro.SiparisHareketler`) — secili siparisin satir hareketleri.
- Service: `ISiparisService.SelectListWhere(" where Turu ='MikroSiparis' ")` (`Ortak.DbPro.Siparis`) — UretimV4'te aktarilmis siparisleri tespit edip `Aktarildi=true` isaretler (SiparisKodu = EvrakSeri+EvrakSira eslesmesi).
**Istasyon sirasiyla iliskisi:** Bu ekrandan olusan is emri, FrmMikroUretimEmriOlustur -> FrmUretimEmriED akisina baglanir; istasyon/operasyon yapisi recete uzerinden ileride kurulur.
**Notlar:** `SipGuid` gizlenir. Aktarilmis satirlar RowStyle ile yesil (myView1) gosterilir.

### Mikro Recete Listesi (`FrmMikroReceteListesi.cs` / `.Designer.cs`)
**Ne ise yarar:** Mikro ERP urun recetelerini (URUN_RECETELERI) listeler; secilen recetenin UretimV4 (Pro) karsiligini ve operasyonlarini alt gridlerde gosterir. UretimV4'e aktarilmis receteler yesil isaretlenir. Sag-tik menu ile receteyi UretimV4'e iceri aktarma ve fire yuzdesi guncelleme ekranlari acilir.
**Once ne olmali (onkosul):** `Ortak.DbPro` ve `Ortak.DbMikro` hazir olmali; `MikroReceteManager` (her ikisi ile) kurulu olmali.
**Sonra ne olur:** Listeleme salt-okunur. Sag-tik "Receteyi İçeri Aktar" -> `FrmReceteED` (MikrodanAktar=true) ile UretimV4 recetesi olusturulur; "Recete Fire Yüzde Guncelle" -> `FrmMikroReceteFireGuncelle` acilir.
**Butonlar & kisayollar:**
- `Ara` (BtnAra) — `Bagla()` ile listeyi yukler (rec_iptal=0 + filtre).
- `Temizle` (BtnTemizle) — Ara alanini temizler.
- `comboBox1` durum filtresi: Aktif (varsayilan) / Pasif / Tümü (sto_pasif_fl uzerinden).
- Sag-tik menu: `Receteyi İçeri Aktar`, `Recete Fire Yüzde Guncelle`.
- Grid cift tik/Enter (secim modunda) — `SecilenKod`=ReceteKodu ile kapanir.
**Cagirdigi katmanlar:**
- Manager: `MikroReceteManager.GetMikroReceteList(where)` — URUN_RECETELERI'den (NOLOCK) recete basliklarini ceker (rec_anakod, fn_StokIsmi, fn_StokBirimi, rec_anamiktar, sto_pasif_fl).
- Service: `IReceteAnaService.SelectListWhere()` (`Ortak.DbPro.ReceteAna`) — UretimV4 recetelerini cekip eslesenleri `Aktarildi` isaretler; secilen recetenin Pro karsiligini gosterir.
- Service: `IReceteOperasyonService.SelectList(c => c.RcAId == rcaid)` (`Ortak.DbPro.ReceteOperasyon`) — secilen Pro recetesinin operasyonlari.
**Istasyon sirasiyla iliskisi:** Pro recetesinin operasyonlari (Sira bazli) bu ekranda goruntulenir; uretim akisinin temelini olusturan operasyon sirasi burada izlenebilir.
**Notlar:** Aktarilmis receteler GridView_RowStyle ile yesil. Pro grid kolonlari (Id, RcAGuid, RcAId, RcOGuid) gizlenir.

### Mikro Uretim Emri Olustur (`FrmMikroUretimEmriOlustur.cs` / `.Designer.cs`)
**Ne ise yarar:** Secilen bir Mikro satis siparisinden, satirlardaki stok kodlarina karsilik gelen UretimV4 recetelerini bularak otomatik bir UretimV4 is emri (Siparis turu="MikroSiparis") olusturur. Ust gridde Mikro siparis basligi, alt gridde Mikro siparis hareketleri gosterilir.
**Once ne olmali (onkosul):** FrmMikroSiparisListesi'nden bir siparis secilip `SipGuid` set edilmis olmali. Her Mikro stok kodu icin `ReceteAna.EntegreStokKodu` eslesen bir UretimV4 recetesi tanimli olmali — yoksa "Ürüne ait Reçete Bulunamadı" hatasi verir ve form kapanir.
**Sonra ne olur:** `Kaydet` ile UretimV4 `Siparis` + `SiparisHareket` + `SiparisHareketDetay` tablolarina yazilir (SiparisManager.SiparisKaydet transaction icinde eski hareket/detaylari silip yenisini insert eder). Ardindan `Action?.Invoke()` (cagiran liste yenilenir) ve `FrmUretimEmriED` (UretimTuru="MikroSiparis") acilir; bu form kapanir.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — `SiparisManager.SiparisKaydet(_mdl, YeniKayit)` cagirir; basarili olursa is emri duzenleme formuna gecer.
- `Kapat` (BtnKapat) — base form ile kapatir.
**Cagirdigi katmanlar:**
- Service: `IMikroSiparisService.GetViewListWhere(" where Sip.SipGuid =...")` — Mikro siparis basligini ceker.
- Service: `IMikroSiparisHareketService.GetViewListSeriSira(seri, sira)` — Mikro siparis satirlarini ceker.
- Manager: `SiparisManager.GetSiparis()` — bos SiparisKayitModel; `SiparisManager.SiparisKaydet(mdl, yenikayit)` — UretimV4 siparisini transaction'la kaydeder.
- Manager: `ReceteManager.GetReceteKayit(rcAId)` — eslesen recetenin tam kaydini (detaylar dahil) getirir; ilk detaydan hareket stok bilgisi, tum detaylardan SiparisHareketDetay olusturulur.
- DAL: `_dbPro.ReceteAna.SelectFirst(c => c.EntegreStokKodu == sipH.StokKodu)` — stok koduna gore recete eslestirme.
**Istasyon sirasiyla iliskisi:** Olusan is emri sonradan FrmUretimEmriED'de operasyon/istasyon yapisiyla devam eder; recetenin operasyon sirasi uretim akisinin temelidir.
**Notlar:** Her Mikro siparis satiri icin tek recete secilir (`ReceteTekSec`). Hareketler `EntKayitSeri/Sira/Guid` ile Mikro siparis satirina baglanir (geri izlenebilirlik).

### Mikro Recete Fire Yüzde Güncelle (`FrmMikroReceteFireGuncelle.cs` / `.Designer.cs`)
**Ne ise yarar:** Mikro recetesindeki tuketim kalemlerinin fire yuzdelerini, UretimV4'e aktarilmis recetenin detaylarina (ReceteDetay.FireYuzde) toplu olarak gunceller. Ust gridde Mikro recete detaylari (referans), alt gridde duzenlenebilir Pro recete detaylari gosterilir.
**Once ne olmali (onkosul):** FrmMikroReceteListesi sag-tik menusunden acilmis ve `MikroReceteKodu` + `MikrodanAktar=true` set edilmis olmali. Recete daha once UretimV4'e aktarilmis olmali (ReceteDetay kayitlari icin).
**Sonra ne olur:** `Kaydet` ile `ReceteManager.ReceteDetayFireYuzdeGuncelle(receteDetaylar)` cagrilir; UretimV4 `ReceteDetay` tablosundaki fire yuzdeleri guncellenir, mesaj verilip form kapanir.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — fire yuzde guncellemeyi yazar.
- Alt grid duzenlenebilir kolonlar: `ReceteSira`, `Miktar`, `Ebat`, `Gram`, `Olcu`, `FireYuzde` (yesil baslikli).
- `Stok Cinsi` combo, `Recete Grubu` combo, entegre stok kodu/adi/birim/model kodu alanlari (cogu salt bilgi).
**Cagirdigi katmanlar:**
- Manager: `ReceteManager.GetReceteKayit()` — bos recete kayit modeli; `GetReceteDetayByReceteKodu(receteKodu)` — Pro recete detaylari; `ReceteDetayFireYuzdeGuncelle(list)` — fire yuzdelerini gunceller.
- Manager: `MikroReceteManager.GetMikroReceteList(where)` — Mikro recete basligi; `GetMikroReceteHareketler(receteKodu)` — Mikro tuketim kalemleri (rec_fireyuzde dahil).
- Service: `IMikroStokService.GetViewListWhere(...)` — entegre stok bilgisi/cinsi; `IGenelService.GrupListesi("ReceteAna","Grubu")` — recete grup combosu.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Mikro detay sirasi (ReceteSira) ile Pro detay sirasi eslestirilerek fire yuzdesi tasinr. EvrakNoAl ile gerekirse yeni recete kodu uretilir.

### Mikroya Uretim Kaydet V2 (`FrmMikroyaUretimKaydetV2.cs` / `.Designer.cs`)
**Ne ise yarar:** Modulun ana yazma ekrani. Tamamlanmis bir UretimV4 is emrinin sonuclarini (uretilen urun + fire + sarf edilen stoklar + stok firesi) Mikro ERP'ye stok hareketi fisleri olarak kaydeder. Ust gridde is emri hareketleri (uretim/fire miktarlari), alt gridde kullanilan/cikan stoklar gosterilir.
**Once ne olmali (onkosul):** Is emri uretim girisleri tamamlanmis olmali (UretimIstasyonHareket -> ... miktarlar toplanmis). FrmSiparisListesi'nden `SipId` set edilerek acilir. `Ortak.MikroEntAyarlar` (MikroEntegre fis turu/maliyet ayarlari) ve `Ortak.IstasyonAyarlarBagla()`, `Ortak.MalKabulKullan`, `Ortak.PlKapat` ayarlari yuklenir. Daha once aktarilmissa (`Siparis.Ent`) kullaniciya eski fisleri silmesi gerektigi uyarisi cikar.
**Sonra ne olur:** `Kaydet` butonu, ayarlara gore urun girisi / urun fire cikisi / stok cikisi / stok fire cikisi listelerini StokHareketleriModel olarak olusturur, maliyet hesaplar (standart/recete maliyet), fis turune gore (StokVirman/UretimHareket/UretimdenGiris/SayimDepoGiris/UretimeCikis/SarfDepoCikis/FireCikis) `MikroConvertManager.Convert...` ile MikroStokHareketleri'ne donusturur ve `MikroKayitManager.StokHareketKaydet` ile Mikro `STOK_HAREKETLERI` (+ BEDEN_HAREKETLERI + PARTILOT + StokDepoRaf) tablolarina transaction icinde yazar. Basariliysa `SiparisManager.SiparisEntGuncelle` ile UretimV4 `Siparis`/`SiparisHareket` tablolarina Ent=1 + evrak seri/sira islenir; Kaydet butonu pasiflesir.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — tum fisleri Mikro'ya yazar (yukaridaki akis).
- `Yazdir` (BtnYazdir) — `BaglaKaliteYazdir()` ile kalite raporunu (KaliteRapor REPX) yazdirir.
- `myButton1` (gizli/yardimci) — mal kabul hesaplama onizleme formunu (`FrmMikroMalKabulHesaplama`) acar.
- `Kapat` (BtnKapat) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager: `SiparisManager.GetSiparis(SipId)` — is emri (Siparis + hareketler); `SiparisEntGuncelle(...)` — Ent + evrak seri/sira yazar; `GetKaliteYazdir(SipId)` — kalite rapor datasi.
- Manager: `ReceteManager.GetRafOmru(rcAId)` — urun raf omru (parti son kullanma tarihi icin).
- Manager: `MikroConvertManager.SetUretimUrunGirisFisiAyar / SetUretimUrunFireCikisFisiAyar / SetUretimStokCikisFisiAyar / SetUretimStokFireCikisFisiAyar` (depo/fis ayarlari) ve `ConvertStokVirmanFisi / ConvertUretimHareketFisi / ConvertUretimdenGirisFisi / ConvertSayimDepoGiris / ConvertUretimeCikisFisi / ConvertSarfDepoCikis / ConvertFireCikis` (fis turune gore MikroStokHareketleri + evrak seri/sira uretimi).
- Manager: `MikroKayitManager.GetMikroStokMaliyetListWhere(where)` — stok standart/recete maliyeti; `StokHareketKaydet(lisMikro, depoRaf)` — Mikro fis kaydi (transaction; parti/lot/renk-beden hesaplama dahil).
- Service: `IIstasyonTakipStokHareketService.GetViewListKullanimWherePartiLot / GetViewListKullanimWhereMalKabul / GetViewListKullanimMalKabulFis` — kullanilan/cikan stoklar; `IIstasyonTakipHareketDetayService.GetViewListStokFire(...)` — stok fireleri; `IIstasyonTakipStokHareketDetayService.DetaylarGuncelleBySipId(SipId)` — kayit oncesi detay senkronu; `ITempMikroStokService.SelectListWhere(...)` — birim katsayilari (Birim2/3/4, Katsayi) ile birim donusumu.
- Yardimci sinif: `MikroyaKaydetMalKabulHesaplama` / `MikroyaKaydetMalKabulFireHesaplama` — `Ortak.MalKabulKullan` aktifse parti/lot bazinda mal kabul fisine gore stok/fire dagitimi yapar.
- SQL/Prosedur: dolayli olarak fis kaydi sirasinda STOKLAR (sto_detay_takip/renk/beden), PARTILOT, BEDEN_HAREKETLERI; maliyet icin `fn_by_Stok_Son5_Giris_Fiyati`, `fn_StokIsmi`, `fn_StokBirimi` Mikro fonksiyonlari.
**Istasyon sirasiyla iliskisi:** Bu ekran uretim akisinin SONUNDADIR — istasyon takip hareketleriyle (uretim girisi, sarf, fire, mal kabul) biriken miktarlar burada Mikro'ya stok hareketi olarak aktarilir. Olcum/akis motoru (Uretim_MiktarGuncelle/PlanlananGuncelle/SonrakiIstasyonaGonder) is emri tarafinda calismis, miktarlar UretimIstasyonHareket->...->UrO seviyesine toplanmistir; burada yalnizca okunup fise donusturulur.
**Notlar:** Urun fiyati = (stok cikis tutar + fire cikis tutar) / toplam urun miktari olarak hesaplanip urun giris/fire fislerine yazilir. Birim donusumu TempMikroStok katsayilariyla yapilir (birimpntr 1-4). `.txt` uzantili eski V1 ayni isimli formun yedegidir (aktif degil; bu V2 dosyasi kullanilir).

### Mikroya Sarf Fire Kaydet (`FrmMikroyaSarfFireKaydet.cs` / `.Designer.cs`)
**Ne ise yarar:** Istasyon takip hareket detaylarindan dogan ara sarf cikis ve fire giris fislerini (IstasyonTakipHareketDetay) Mikro ERP'ye stok hareketi olarak kaydeder. Tam is emri kapatmadan, istasyon bazinda sarf/fire aktarimi icin kullanilir.
**Once ne olmali (onkosul):** FrmIstasyonFisList ekranindan aktarilacak `FisList` (List<IstasyonTakipHareketDetay>) doldurularak acilmis olmali. Acilista `DahaOnceKayitEdilmismi()` ile EntCode'u olan kayitlarin Mikro'da zaten var olup olmadigi kontrol edilir — varsa Kaydet butonu gizlenir (mukerrer aktarim engeli).
**Sonra ne olur:** `Kaydet` ile her detay, turune gore (SarfCikisFisi / FireGirisFisi) ve ayarlardaki fis turune gore (StokVirman / SarfDepoCikis / FireCikis) StokHareketleriModel'e cevrilir, `MikroConvertManager.Convert...` ile MikroStokHareketleri olusturulur, `MikroKayitManager.StokHareketKaydet(lisMikro)` ile Mikro'ya yazilir. Basariliysa her FisList kaydina `Ent=true`, `EntSeri/EntSira/EntDate` yazilip `IIstasyonTakipHareketDetayService.InsertOrUpdate(FisList)` ile UretimV4'e geri kaydedilir.
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — sarf/fire fislerini Mikro'ya aktarir ve UretimV4 detaylarini Ent isaretler.
- `Kapat` (BtnKapat) — formu kapatir.
**Cagirdigi katmanlar:**
- Manager: `MikroConvertManager.SetSarfCikisFisiTuruAyar / SetFireGirisFisiTuruAyar` (depo/ayar); `ConvertStokVirmanFisi / ConvertSarfDepoCikis` (fis donusumu).
- Manager: `MikroKayitManager.StokHareketIdKayitEdilmismi(id)` (mukerrer kontrol); `StokHareketKaydet(lisMikro)` (Mikro kayit).
- Service: `IIstasyonTakipHareketDetayService.InsertOrUpdate(FisList)` (`Ortak.DbPro.IstasyonTakipHareketDetay`) — Ent bilgilerini geri yazar.
**Istasyon sirasiyla iliskisi:** Saha akisinda istasyon bazli sarf/fire kayitlari (IstasyonTakipHareketDetay, Turu=SarfCikisFisi/FireGirisFisi) bu ekranda Mikro'ya aktarilir; is emri tam kapanmadan ara aktarim saglar.
**Notlar:** Her detaya benzersiz `EntCode` (Guid) atanip Mikro fis `sth_Guid` olarak kullanilir; bu sayede mukerrer aktarim tespiti yapilir.

### Mikro Mal Kabul Hesaplama (`FrmMikroMalKabulHesaplama.cs` / `.Designer.cs`)
**Ne ise yarar:** Mal kabul fisleri ile istasyon kullanim hareketlerini eslestirip, hangi stok/parti/lot'tan ne kadar dusulecegini hesaplayan onizleme/dogrulama ekranidir (FrmMikroyaUretimKaydetV2 icindeki MalKabul hesaplamasinin gorsel kontrolu). 4 grid: mal kabul fisi, istasyon hareket, hesaplanan, kalan.
**Once ne olmali (onkosul):** Acan form (FrmMikroyaUretimKaydetV2.myButton1) `MalKabulFis`, `IstasyonHareket`, `StokFireListPartili` listelerini doldurmali. `Ortak.MalKabulKullan` senaryosu icin anlamlidir.
**Sonra ne olur:** Salt hesaplama/onizleme; DB'ye yazmaz. `myButton1` -> `hsp.Convert()` (hesaplanan dagitim) ve `hsp.GetKalanList()` (artan mal kabul miktarlari) gridlere basilir.
**Butonlar & kisayollar:**
- `myButton1` — hesaplamayi calistirip hesaplanan + kalan listeleri gosterir.
**Cagirdigi katmanlar:**
- Yardimci sinif: `MikroyaKaydetMalKabulHesaplama.Convert()` — partili fireleri dusup mal kabul fisine gore istasyon hareketlerini parti/lot bazinda dagitir; `GetKalanList()` — kullanilmayan mal kabul kalanlari.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Bu ekran dogrudan menude degil, Mikroya Uretim Kaydet V2 icinden yardimci olarak acilir. Asil kayit FrmMikroyaUretimKaydetV2'de yapilir.

### Mikro Uretim Kaydedilen Fişler (`FrmMikroUretimKaydedilenFisler.cs` / `.Designer.cs`)
**Ne ise yarar:** Bir is emri (BelgeNo/SipId) icin Mikro'ya daha once aktarilmis stok hareketi fislerini listeler ve secilenleri Mikro'dan silmeyi saglar. Yanlis/tekrar aktarim sonrasi temizlik ve yeniden aktarim icin kullanilir.
**Once ne olmali (onkosul):** FrmSiparisListesi'nden `BelgeNo` ve `SipId` set edilerek acilmis olmali; o is emrine ait Mikro fisleri var olmali.
**Sonra ne olur:** `Sil` ile secili (`Sec=true`) fisler `MikroKayitManager.DeleteMikroAktarilanFisBySeriSira` ile Mikro `STOK_HAREKETLERI` (+ BEDEN_HAREKETLERI + PARTILOT) tablolarindan silinir. Tum fisler silindiyse (`fisKaldimi=false`) `SiparisManager.SiparisEntGuncelle(SipId,"","","","",0,0)` ile UretimV4'teki entegrasyon bayraklari (Ent=0, Kapandi=0) temizlenir; liste yeniden yuklenir.
**Butonlar & kisayollar:**
- `Sil` (BtnSil) — secili fisleri Mikro'dan siler (onay sorar).
- `Kapat` (BtnKapat) — formu kapatir (`BtnKapat_Click_1`).
- Grid `Sec` kolonu — silinecek fisleri isaretlemek icin duzenlenebilir checkbox.
**Cagirdigi katmanlar:**
- Manager: `MikroKayitManager.GetMikroAktarilanFisByBelgeNo(belgeNo)` — belge no'ya gore aktarilmis fisleri ceker; `DeleteMikroAktarilanFisBySeriSira(seri, sira)` — fisi (ve bagli beden/parti hareketlerini) transaction'la siler.
- Manager: `SiparisManager.SiparisEntGuncelle(...)` — fis kalmayinca UretimV4 entegrasyon bayraklarini sifirlar.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Mal kabul fisi turu (sth_tip=2, sth_cins=6, sth_evraktip=2) listeden haric tutulur. Silme parti/lot ozelkod3='AKT' ve belge_no eslesmesine dayanir (yalnizca bu uygulamanin actigi fisler).
## Modul: SiparisModule

UretimV4 (CepPatronERP.exe, WinForms) masaustu uygulamasinin "Siparis / Recete Uretim" giris ve takip modulu. Bu modul, hem musteri SIPARISI hem de dahili RECETE URETIM emirlerinin (her ikisi de ayni `Siparis` tablosunda `Turu` alaniyla ayristirilir) baslangic noktasidir: siparis basligi (cari, tarih, teslim) + receteler (`SiparisHareket`) + recete malzeme detaylari (`SiparisHareketDetay`) burada olusturulur. Liste ekrani uzerinden bu siparisten URETIM EMRI olusturulur/baslatilir (`FrmUretimEmriED`), ilk istasyonlara gonderilir, uretilen miktarlar geri toplanir (TempSiparisUretimMiktar), Mikro ERP'ye uretim fisi gonderilir ve iptal edilen miktarlar depoya iade edilir. Yani saha akisinin (TabletV2'deki IstasyonTakip) "kaynak" tarafidir; istasyon-sira motoru bu modulun urettigi `UretimEmri/UretimOperasyon/UretimIstasyon` kayitlari uzerinde calisir.

Tum formlar harici `My.Kontrol` kutuphanesindeki base formlardan turer: liste formlari `MyFrmListe` (BtnAra/BtnTemizle/BtnYazdir/BtnDizayn/BtnKapat + `SecimIcinAcildi/Secildi/SecilenRow` secim altyapisi + grid cift-tik = `MyEventDoubleClickEnter`), kayit formlari `MyFrmKayit` / `MyFrmKayitFull` (BtnKaydet/BtnSil/BtnYazdir/BtnKapat + `YeniKayit/KayitEdildi/IdGuid/AcilisBittimi`). Base form davranisi geneldir: kapat butonu/Esc formu kapatir, kaydet butonu kaydeder; bu modulde ayrica F5/F6/F7/F8 recete kisayollari forma ozel eklenmistir.

### Siparis / Recete Uretim Listesi (`FrmSiparisListesi.cs`, `FrmSiparisListesi.Designer.cs`)
**Ne ise yarar:** Siparis ve Recete uretim emirlerinin ana liste ekranidir. Ust gridde siparisler (`Siparis`), alt gridde secili siparisin hareketleri (`SiparisHareket`, kalan miktarli view) listelenir. `Turu` alani ("Siparis" / "Recete" / "MikroSiparis") forma disaridan set edilerek ayni form 3 farkli baslik/davranisla acilir (Recete ise baslik "Recete Üretim Listesi", buton "Üretim Ekle"; kolon basliklari "İş Emri Tarihi/Kodu" olur). Sag-tik context menusu uzerinden uretim emri olusturma/baslatma, ilk istasyona gonderme, miktar guncelleme, Mikroya uretim gonderme, iptal iadesi gibi tum orkestrasyon islemleri buradan tetiklenir.
**Once ne olmali (onkosul):** Lisans/kullanici girisi yapilmis (`Ortak.KullaniciAdi`, `Ortak.DbPro`, `Ortak.DbMikro` dolu), Mikro stok grubu ayarli (`Ortak.MikroStokGrubu`). Listede islem yapmak icin once `FrmSiparisEd`/`FrmSiparisReceteEd` ile siparis kayitli olmali.
**Sonra ne olur:** Form acilisinda otomatik `BtnAra.PerformClick()` ile veri yuklenir. Secilen islemlere gore: yeni `UretimEmri/UretimOperasyon/UretimIstasyon` kayitlari olusur (FrmUretimEmriED), `IstasyonTakipHareket` ilk istasyonlara dusurulur (FrmOperasyonTakipDetaylar), `SiparisHareket.UretimMiktari/FireMiktari/IptalMiktari` guncellenir (TempSiparisUretimMiktar MERGE), Mikro ERP'ye uretim fisi yazilir, ya da iptal iadesi depoya islenir. Cift-tik secimde (`SecimIcinAcildi`) `SecilenKod=SiparisKodu` doner.
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `Bagla()`; filtre + tarih kriterlerine gore `Siparis` listesini yeniler.
- `BtnTemizle` ("Temizle") — filtre kutularini temizler (tarih1 = 01.01.<yil>).
- `BtnEkleSiparis` ("Siparis Ekle" / Recete'de "Üretim Ekle") — Turu'ye gore `FrmSiparisEd` veya `FrmSiparisReceteEd` yeni kayit acar.
- `BtnUretimEmriOlustur` ("Üretime Emri") — secili siparis icin `UretimEmriEmriOlusturBaslat(itm, false)`.
- `BtnYazdir` ("Yazdir") — `Yazdir()`; secili siparisi tek DataSet'te (Siparis+Hareketler+Detaylar) "Siparis" dizaynina basar.
- `BtnDizayn` ("YazdirTek / Ctrl+D") — `Yazdir2()`; her hareketi ayri sayfa olarak yazdirir (YaziciAyar ile adet/onizleme).
- `BtnKapat` (base / Esc) — formu kapatir.
- Durum filtre butonlari: `BtnDurumuHepsi`, `BtnDurumuYeniKayit`, `BtnDurumuBeklemede`, `BtnDurumuUretimde`, `BtnDurumuHazir` — `DurumuAyarla(...)` + `BtnAra.PerformClick()` (secili buton renklenir `FilterButonRenklendir`).
- Kapandi filtre butonlari: `BtnKapandiTumu`, `BtnKapandiAcik`, `BtnKapandiKapandi` — `KapandiAyarla(...)` + arama (Açık = `coalesce(Kapandi,0)=0`).
- Grid cift-tik / Enter (`MyView1_MyEventDoubleClickEnter`) — secim modunda secip kapatir; degilse Turu'ye gore `FrmSiparisEd`/`FrmSiparisReceteEd` duzenleme acar.
- Sag-tik context menu (`ContexMenuyeEkle`) ogeleri: "Uretimi Emri (Olustur / Oto Başlat)" -> `ConIsEmriOlusturBaslat` (OtoBaslat=true); "Uretim Emri (Olustur / Guncelle)" -> `ConIsEmriOlusturV2` (OtoBaslat=false); "Operasyon (İlk İstasyonlara Gönder)" -> `ConOperasyonIstasyonBaslat`; "Üretilen Adetleri Güncelle" -> `ConUretimAdetleriGuncelle`; "Mikroya Uretim Gonder" -> `ConMikroyaUretimGonderYeni`; "Mikroya Aktarilan Uretim Fisleri" -> `ConMikroyaGonderilenFisler`; "Mal Kabul Fisi" -> `ConMalKabulFisAc`; "Iptal Edilenleri Depo Kontrole GeriAktar" -> `ConIptalFisiniDepoyaAktar`; "Git" alt menusu -> Üretim Emri / Üretim Operasyonlar / Istasyon Hareketler / Istasyon Bekleyenler / Istasyon Hareket Detaylar formlarini SiparisKodu ile filtreli acar.
**Cagirdigi katmanlar:**
- Service: `ISiparisService.SelectListWhere(where)` — siparis listesi.
- Service: `ISiparisHareketService.GetViewListKalanMiktarliWhere(" where SH.SipId='...'")` — secili siparisin kalan-miktarli hareket gorunumu (alt grid).
- Service: `IGenelService.GrupListesi("Siparis","Durumu")` — durum combosu icin grup degerleri.
- Service: `ITempMikroStokService.SelectFirst(StokKodu)` — parti takipli urun kontrolu (TakipTip 1/2).
- Manager: `SiparisManager.GetSiparis(id)` — yazdirma icin Siparis+Hareketler+Detaylar getirir.
- Manager: `SiparisManager.GetTempSiparisUretimMiktarBySipId(sipId, kullanici)` — uretilen adetleri TempSiparisUretimMiktar uzerinden hesaplar ve `SiparisHareket`e MERGE eder (adet guncelleme + Mikroya/iptale gonderme onkosulu).
- Manager: `SiparisManager.GetOperasyonKoduByRcAId(rcaId)` — RcAId icin Sira=1 ilk operasyon kodu (ilk istasyona gonderme icin).
- Acilan formlar: `FrmUretimEmriED` (uretim emri olustur/baslat), `FrmOperasyonTakipDetaylar` (ilk istasyona gonder, Durumu="Beklemede"), `FrmMikroyaUretimKaydetV2` (Mikroya uretim fisi), `FrmMikroUretimKaydedilenFisler`, `FrmMalKabulED`, `FrmUretimIptalleriDepoyaAktar`, ve Git menusu formlari (FrmUretimEmriListesi, FrmUretimOperasyonList, FrmIstasyonHareketler, FrmIstasyonBekleyenler, FrmIstasyonHareketDetaylar).
- SQL/Prosedur: dolayli olarak TempSiparisUretimMiktar inline T-SQL (IstasyonTakipHareket -> UretimIstasyon -> UretimOperasyon -> UretimEmri toplama + SiparisHareket MERGE).
**Istasyon sirasiyla iliskisi:** Modulun istasyon-sira motoruyla ana baglanti noktasidir. "Operasyon (İlk İstasyonlara Gönder)" her benzersiz operasyon (RcAId -> Sira=1 OperasyonKodu) icin FrmOperasyonTakipDetaylar acarak isi ilk istasyonun bekleyenler kuyruguna (`IstasyonTakipHareket` Beklemede) koyar. "Üretilen Adetleri Güncelle" ise istasyon hareketlerini operasyon-sira bazinda yukari toplayip siparise yansitir. Mikroya gonderme ve iptal iadesi yalnizca Durumu="Hazir" (tum siralar bitmis) siparislerde calisir.
**Notlar:** `Ortak.PlKapat` true ise tum gridlerde Parti/Lot kolonlari gizlenir ve parti-takip zorunlulugu atlanir. SorguAyarla string interpolasyonla WHERE kurar (parametrik degil). `Yazdir2/Yazdir` yazdirma adi sabit "Siparis"tir.

### Siparis Kayit (`FrmSiparisEd.cs`, `FrmSiparisEd.Designer.cs`)
**Ne ise yarar:** Musteri SIPARISI basligi ve recete satirlarinin girildigi/duzenlendigi kayit formu (`MyFrmKayitFull`). Cari secimi (kod/unvan otomatik doldurma, Mikro cari listesinden), siparis kodu otomatik evrak no, tarih/teslim tarihi, kargo, e-posta, aciklama girilir; grid'e recete grubu veya tekil recete eklenir, her recete satirinin malzeme detaylari (`SiparisHareketDetay`) `SiparisPanelControl` ile ayarlanir.
**Once ne olmali (onkosul):** Receteler (`ReceteAna`/`ReceteGrup`) onceden tanimlanmis olmali. Mikro stoklari (`Ortak.MikroStokGrubu`) ve cari listesi erisilebilir olmali. Duzenleme icin `IdGuid` set edilir (yeni kayit icin bos).
**Sonra ne olur:** Kaydette `SiparisManager.SiparisKaydet(mdl, yeniKayit)` cagrilir; transaction icinde `Siparis` upsert + ilgili `SiparisHareket` ve `SiparisHareketDetay` once silinip yeniden yazilir. Durumu bos ise "YeniKayit" set edilir, `Miktar` (hareket toplami) ve `Notu` (recete adlari) hesaplanir. Kayit sonrasi `ActionAktar` (liste yenileme) tetiklenir; e-posta varsa sorulup mail gonderilir. Sipariste uretim baslatilmissa (`SipariseBagliUretimVarmi`) form salt-okunur olur.
**Butonlar & kisayollar:**
- `BtnKaydet` ("Kaydet") — parti-takip kontrolu sonrasi `Kaydet()`.
- `BtnSil` ("Sil") — `SiparisSilKontrol` sonrasi `Sil()`.
- `BtnReceteGrupSec` ("Recete Grup Seç" / F5) — `ReceteGrupSec()`; cari+siparis kodu kontrolu ile recete grubu satirlarini ekler.
- `BtnReceteTekSec` / `BtnReceteSec` ("Recete Seç" / F6) — `ReceteTekSec()`; tek recete ekler.
- `BtnReceteDegistir` ("Recete Değiştir" / F7) — `ReceteDegistir()`; secili satirin recetesini degistirir.
- `BtnReceteSil` ("Recete Sil" / F8) — `ReceteSil()`; secili recete satirini ve detaylarini listeden cikarir.
- `BtnYazdir` ("Yazdir") — `Yazdir()` (DataSet -> "Siparis" dizayni).
- `BtnYazdirTek` — `YazdirTek()` (hareket basina ayri yazdirma).
- `BtnMailPdfDesing` — `YazdirMail()`; "SiparisMail" dizaynini hazirlar.
- `BtnMailGonder` — `MailGonder(false)`; `FrmFisMailGonder` ile e-posta gonderir.
- `TxtSiparisKodu` buton (`TxtSiparisKodu_ButtonClick`) — `EvrakNoAl()` ile yeni siparis kodu uretir.
- Cari kutulari `Leave` — secilen Mikro cariden Unvan/Kargo/Email otomatik doldurur.
- Grid cift-tik (`MyView1_MyEventDoubleClickEnter`) — `RecetePanelBagla()` ile secili recete satirinin malzeme paneli acilir.
- `BtnKapat` / Esc — kapatir.
**Cagirdigi katmanlar:**
- Manager: `SiparisManager.GetSiparis()/GetSiparis(id)` — bos/dolu kayit modeli.
- Manager: `SiparisManager.SiparisKaydet(mdl, yeniKayit)` — transaction'li upsert (Siparis + Hareket + Detay sil/ekle).
- Manager: `SiparisManager.SiparisSil(mdl)` / `SiparisSilKontrol(id)` (proc `Siparis_Sil_Kontrol`) — bagli hareket kontrolu + silme.
- Manager: `SiparisManager.SipariseBagliUretimVarmi(sipid)` — `UretimEmri`de SipId var mi (salt-okunur kararina).
- Manager: `ReceteManager.GetReceteKayit(id)` ve `ReceteGrupManager.GetReceteKayit(id)` — recete/recete grubu detaylari.
- Service: `IGenelService.GetEvrakNo("Siparis")` — siparis kodu uretimi.
- Service: `IMikroCariService.GetViewListWhere(...)` — cari lookup datasi.
- Service: `ITempMikroStokService.SelectFirst(StokKodu)` — parti takip kontrolu.
- Service: `Ortak.DbMikro.Stoklar.GetViewListWhere/GetRenkListWhere` — stok/renk listeleri (panel icin).
- Acilan formlar: `FrmReceteGrupListesi`, `FrmReceteSec`, `FrmFisMailGonder`, `SiparisPanelControl`.
**Istasyon sirasiyla iliskisi:** Dolayli. Burada secilen recete (`RcAId`) ileride uretim emri ve operasyon-sira zincirini belirler; ancak bu form yalnizca siparis/recete verisini hazirlar, istasyona is dusurmez. Uretim baslamissa form kilitlenir (degistirilemez).
**Notlar:** `MyFrmKayitFull` turevi. Parti takipli (`TempMikroStok.TakipTip` 1/2) uründe parti no zorunlu (PlKapat haricinde). Recete satirinda `SiparisdeGosterme=false` detay varsa satir "YeniKayit" olarak isaretlenir ve panelle ayarlanmadan kayit engellenir.

### Recete Uretim Kayit (`FrmSiparisReceteEd.cs`, `FrmSiparisReceteEd.Designer.cs`)
**Ne ise yarar:** Dahili RECETE URETIM emri (Turu="Recete") kayit formu. `FrmSiparisEd` ile cok benzer ama: evrak no "Uretim" serisinden alinir, cari opsiyonel (buton ile Mikro cari listesinden secilir / temizlenir), gridde Beden duzenlenebilir ve kayit oncesi parti/lot/beden takip kurallari daha siki uygulanir (parti-lot daha once girilmis mi kontrolu dahil).
**Once ne olmali (onkosul):** Receteler tanimli, Mikro stok/renk listeleri erisilebilir. Duzenleme icin `IdGuid`. Uretim baslamamis olmali (baslamissa salt-okunur).
**Sonra ne olur:** `SiparisManager.SiparisKaydet` ile ayni `Siparis/SiparisHareket/SiparisHareketDetay` tablolarina Turu="Recete" olarak yazar. Kayit oncesi her hareket icin parti takibi (TakipTip 1/2), beden takibi (RbTakipTip=2 veya TakipTip=3) ve `MikroKayitManager.PartiLotDataOnceGirilmismi` ile parti-lot tekrar kontrolu yapilir; Lot bos/0 ise 1'e cekilir. Kayit sonrasi `ActionAktar` ile liste yenilenir.
**Butonlar & kisayollar:**
- `BtnKaydet` ("Kaydet") — parti/beden/parti-lot kontrolleri sonrasi `Kaydet()`.
- `BtnSil` ("Sil") — `SiparisSilKontrol` sonrasi `Sil()`.
- `BtnReceteGrupSec` (F5) — `ReceteGrupSec()`.
- `BtnReceteTekSec` / `BtnReceteSec` (F6) — `ReceteTekSec()`.
- `BtnReceteDegistir` (F7) — `ReceteDegistir()` (mevcut miktari korur).
- `BtnReceteSil` (F8) — `ReceteSil()`.
- `BtnYazdir` / `BtnYazdirTek` — `Yazdir()` / `YazdirTek()` ("Siparis" dizayni).
- `TxtCariKodu` buton (`TxtCariKodu_ButtonClick`) — `FrmMikroCariListesi` secimi.
- `BtnCariTemizle` — cari kod/unvan temizler.
- `TxtSiparisKodu` buton — `EvrakNoAl()` ("Uretim" serisi).
- Grid cift-tik — `RecetePanelBagla()` (malzeme paneli).
- `BtnKapat` / Esc — kapatir.
**Cagirdigi katmanlar:**
- Manager: `SiparisManager.GetSiparis/SiparisKaydet/SiparisSil/SiparisSilKontrol/SipariseBagliUretimVarmi` (FrmSiparisEd ile ayni).
- Manager: `ReceteManager.GetReceteKayit` / `ReceteGrupManager.GetReceteKayit` — recete(grup) detaylari.
- Manager: `MikroKayitManager.PartiLotDataOnceGirilmismi(stokKodu, parti, lot)` — parti-lot daha once kayitli mi.
- Service: `IGenelService.GetEvrakNo("Uretim")` — uretim kodu.
- Service: `ITempMikroStokService.SelectFirst(StokKodu)` — TakipTip/RbTakipTip okuma.
- Service: `Ortak.DbMikro.Stoklar.GetViewListWhere/GetRenkListWhere`.
- Acilan formlar: `FrmReceteSec`, `FrmReceteGrupListesi`, `FrmMikroCariListesi`, `SiparisPanelControl`.
**Istasyon sirasiyla iliskisi:** `FrmSiparisEd` ile ayni; recete secimi ileri uretim operasyon-sira zincirini tanimlar, form dogrudan istasyona is dusurmez.
**Notlar:** `CheckForIllegalCrossThreadCalls=false` set edilir. Beden kolonu duzenlenebilir acilir (FrmSiparisEd'de Beden duzenlenmez). Hareket yoksa kayit engellenir ("Hareket Bulunamadı").

### Siparis Hareket Seç (`FrmSiparisHareketSec.cs`, `FrmSiparisHareketSec.Designer.cs`)
**Ne ise yarar:** Baska ekranlardan cagrilan SECIM (lookup) listesi: siparis hareketlerini (`SiparisHareketModel` view) filtreleyip tekini secip dondurur. Tek grid uzerinde cari kodu/unvan/siparis kodu/durum/kapandi/tarih filtreleriyle calisir.
**Once ne olmali (onkosul):** `SecimIcinAcildi=true` ile acilmis olmali (secim modu). Cagiran ekran dondurulen `SecilenRow`u (`SiparisHareketModel`) kullanir.
**Sonra ne olur:** Cift-tik / Enter ile secili hareket `SecilenRow`a atanir, `Secildi=true` yapilip form kapanir (tablo degistirmez, salt okuma). Secim modu degilse hareket cift-tikinda islem yapilmaz.
**Butonlar & kisayollar:**
- `BtnAra` ("Ara") — `Bagla()`; filtreyle hareket listesini yeniler.
- `BtnTemizle` ("Temizle") — filtre kutularini sifirlar.
- `CmbDurumu` / `CmbKapandi` — durum ve acik/kapandi filtresi.
- Grid cift-tik / Enter (`MyView1_MyEventDoubleClickEnter`) — secip dondurur.
- `BtnKapat` / Esc — secmeden kapatir.
**Cagirdigi katmanlar:**
- Service: `ISiparisHareketService.GetViewListWhere(where)` — hareket view listesi (Siparis JOIN'li, alias S.).
- Service: `IGenelService.GrupListesi("Siparis","Durumu")` — durum combosu.
- Manager: `SiparisManager` (Frm_Load'da olusturulur, dogrudan metot cagrisi yok).
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Salt secim formudur; veri yazmaz. Filtreler string interpolasyonludur (parametrik degil). Tarih1 varsayilani bir ay once.

### Uretim Iptalleri Depoya Aktar (`FrmUretimIptalleriDepoyaAktar.cs`, `FrmUretimIptalleriDepoyaAktar.Designer.cs`)
**Ne ise yarar:** Uretim sirasinda IPTAL edilen miktarlara karsilik gelen tuketilmis hammadde/malzemeyi yerel depoya geri (iade) fisi olarak aktaran kayit formu (`MyFrmKayit`). Ust kisimda siparis bilgisi + hareketler, ortada kullanilan stok hareketleri, altta otomatik hesaplanan iade satirlari (`DepoStokHareket`) gosterilir; kullanici parti/lot/miktar/tarihleri duzenleyip kaydeder.
**Once ne olmali (onkosul):** Liste ekranindan "Iptal Edilenleri Depo Kontrole GeriAktar" ile `SipId` set edilerek acilir; siparis Durumu="Hazir" olmali (liste once `GetTempSiparisUretimMiktarBySipId` ile miktarlari gunceller). Hareketlerde `IptalMiktari>0` ve `Miktar > (UretimMiktari+FireMiktari)` olan satirlar bulunmali, aksi halde "Aktarılacak Kayıt Bulunamadı" uyarisi cikar.
**Sonra ne olur:** `AktarilacakAyarla()` iade miktarini `Carpan * (Miktar-(Uretim+Fire))` formuluyle hesaplar; her satir depoda kayitli mi (`StGuid`) dogrulanir. Kaydette FisSeri="IA", FisTuru=IslemTuru="UretimIade" olan `DepoStokFis` + `DepoStokHareket` listesi `DepoStokFisManager.KaydetDepoStokFis` ile yazilir; ardindan `SiparisManager.SiparisIptalEntGuncelle(sipId, seri, sira)` ile `Siparis.EntIptal=1, EntIptalSeri/Sira` set edilir. Kaydet butonu sonrasi devre disi birakilir.
**Butonlar & kisayollar:**
- `BtnKaydet` ("Kaydet") — depoda kayitli olmayan stok kontrolu sonrasi `Kaydet()` (grid editorunu kapatip fisi yazar).
- `BtnYazdir` (base) — yazdirma (form ozelinde handler bagli degil).
- `BtnKapat` / Esc — kapatir.
- Alt grid (myView3) duzenlenebilir kolonlar: GirisMiktar, PartiNo, LotNo, UretimTarihi, SonKulTarihi (yesil baslikli).
**Cagirdigi katmanlar:**
- Manager: `SiparisManager.GetSiparis(SipId)` — siparis+hareket basligi.
- Manager: `SiparisManager.SiparisIptalEntGuncelle(sipId, seri, sira)` — `Siparis.EntIptal/EntIptalSeri/EntIptalSira` set eder.
- Manager: `DepoStokFisManager.GetDepoStok(list)` — stoklarin depo karsiligi (StGuid/Birim) eslenir.
- Manager: `DepoStokFisManager.FisNoAl("UretimIade","IA")` — yeni iade fis no.
- Manager: `DepoStokFisManager.KaydetDepoStokFis(fis, list, yenikayit, false)` — `DepoStokFis`+`DepoStokHareket` kaydeder (giris fisi, cikis=false).
- Service: `IIstasyonTakipStokHareketService.GetViewListKullanimWherePartiLot(" and HRD.SipId='...'")` — bu siparise ait kullanilan stok (parti/lot/carpan) hareketleri.
**Istasyon sirasiyla iliskisi:** Akisin SON tarafindadir: istasyon hareketlerinde olusan iptal miktarlarinin hammadde karsiligini depoya iade eder; istasyon-sira motorunu calistirmaz, sadece sonuclarini (IstasyonTakipStokHareketDetay/kullanilan stok) okur.
**Notlar:** Yalnizca Durumu="Hazir" siparislerde anlamlidir (liste tarafinda zorlanir). Depoda olmayan stok varsa hata mesaji verilir ama akis devam edebilir (return etmeden Kaydet cagrilir).

### Yardimci UserControl'ler (`SiparisControl.cs`, `SiparisPanelControl.cs`)
**Ne ise yarar:** `SiparisPanelControl` (namespace MyUI.MyControl) bir recete satirinin malzeme detaylarini duzenlemek icin dinamik panel olusturur; her `ReceteDetay` (SiparisdeGosterme=false) icin bir `SiparisControl` (stok kodu/adi/renk/beden/miktar/aciklama editorleri) uretir. `SiparisControl` stok seciminde renk/beden listelerini recete kisitina veya Mikro stok renk/beden tanimina gore doldurur. Bu panel `FrmSiparisEd` ve `FrmSiparisReceteEd` icinde `PanelDetay`e gomulu calisir.
**Once ne olmali (onkosul):** Cagiran kayit formu `Hareket`, `Detaylar`, `Recete` (ReceteKayitModel), `StoklarAll`, `StokRenklerAll` ve `KayitAction` set etmis olmali.
**Sonra ne olur:** "Tamam" (`BtnTamam_Click`) -> `Aktar()` her `SiparisControl`u `SiparisHareketDetay` modeline yazar (stok kodu bos ise hata), ilk satirdan Hareket.Renk/Beden alir, sonra `KayitAction` (formun `PanelGeriAktar`i) cagrilir; bu da degisiklikleri `_mdl.Hareketler/_mdl.Detaylar`a geri isler ve gridi yeniler.
**Butonlar & kisayollar:**
- `BtnTamam` ("Tamam") — `Aktar()` + `KayitAction.Invoke()`.
- Stok kodu/adi combolari `Leave` — secilen Mikro stoktan birim/renk/beden otomatik doldurma.
**Cagirdigi katmanlar:**
- Service: `Ortak.DbMikro.Stoklar.GetViewListWhere(" where S.sto_kod in (...)")` / `GetRenkListWhere` / `GetBedenListWhere` — panel acilisinda yalnizca recetedeki stok kodlarini ceker.
- Veri: `Recete.ReceteStoklar`, `Recete.ReceteStokRenkBedenler`, `Recete.ReceteDetaylar` — renk/beden kisitlamalari.
**Istasyon sirasiyla iliskisi:** -
**Notlar:** Saf UI yardimcilaridir, DB'ye yazmaz; sadece okuma yapip kayit formunun in-memory modelini gunceller. `SiparisControl`te kullanilmayan `RenkBagla___/BedenBagla___` eski metotlar mevcut.
## Modul: KullaniciModul

UretimV4 (CepPatronERP) masaustu uygulamasinin kullanici yonetim modulu. Tek bir formdan (`FrmKullaniciKayit`) olusur ve uygulamaya giris yapacak kullanicilarin tanimlandigi yer/silindigi/duzenlendigi CRUD ekranidir. Yonettigi kayitlar `UretimV3_FEZA` veritabanindaki `Kullanici` tablosuna yazilir (`[Table("Kullanici")]`). Bu kayitlar dogrudan uretim/istasyon akisini etkilemez; uygulama acilisindaki `FrmLogin` ekraninda kullanici adi/sifre dogrulamasi ve `Admin` yetkisi icin kullanilir. Sifreler veritabaninda duz metin degil, AES anahtari (`Ortak.GetKey()` -> "OzelAnahtar1234%&") ile sifrelenmis olarak (`string.Sifrele/SifreCoz`) saklanir.

Form `MyFrmKayit` (My.Kontrol.Formlar kutuphanesindeki base liste+kayit formu) turevidir; standart alt bar butonlari (Kaydet, Yeni, Duzenle, Sil, Yazdir, Kapat ve kayit gezinme butonlari Ilk/Onceki/Sonraki/Son) base'den gelir. Form `SecimIcinAcildi` bayragi ile "secim modu"nda da acilabilir; bu modda grid'de bir satira cift tiklayinca/Enter'a basinca kullanici secilir ve cagiran forma `SecilenKod` (KullaniciAdi) + `SecilenRow` (Kullanici nesnesi) dondurulur.

### Kullanici Kaydi (`FrmKullaniciKayit.cs` / `FrmKullaniciKayit.Designer.cs`)
**Ne ise yarar:** Uygulamaya giris yapacak kullanicilari listeler ve ekleme/duzenleme/silme islemlerini yapar. Ust kisimda form alanlari (KullaniciAdi, Sifre, Adi, Soyadi, Admin onay kutusu), alt kisimda mevcut kullanicilarin grid listesi bulunur.
**Once ne olmali (onkosul):** Uygulamaya giris yapilmis olmali (`FrmLogin` -> `Ortak.DbPro` baglanti fabrikasi kurulmus olmali). Form, ana ekrandan (`FrmAna`) "Kullanicilar" menu/bar butonu (`BarBtnKullanicilar_ItemClick`) tiklaninca `new FrmKullaniciKayit().ShowDialog()` ile acilir. Pratikte yalnizca Admin kullanicilarin erismesi beklenir (yeni kullanici/yetki tanimi).
**Sonra ne olur:**
- Kaydet -> `Kullanici` tablosuna INSERT (yeni Id ile) veya UPDATE (mevcut Id) yapilir; grid yeniden yuklenir (`Bagla()`) ve form alanlari temizlenir (`Temizle()`).
- Sil -> secili satir `Kullanici` tablosundan DELETE edilir; grid yeniden yuklenir, alanlar temizlenir.
- Bu kayitlar daha sonra `FrmLogin.Kontrol()` tarafindan giris dogrulamasinda okunur (KullaniciAdi + sifrelenmis Sifre karsilastirmasi). Baska bir ekrana otomatik gecis yoktur; islem ayni form icinde kalir.
- Secim modunda (`SecimIcinAcildi=true`): kayit yapilmaz; satir secilince `Secildi=true` set edilip form kapanir, cagiran forma deger doner.
**Butonlar & kisayollar:**
- `BtnKaydet` ("Kaydet", alt bar) — `BtnKaydet_Click`: form alanlarini `_mdl` Kullanici nesnesine yazar, sifreyi `TxtSifre.Text.Sifrele(Ortak.GetKey())` ile sifreler, `_srv.InsertOrUpdate(_mdl)` cagirir. Base formda genelde Enter/F2 ile tetiklenir.
- `BtnSil` ("Sil", alt bar) — `BtnSil_Click`: "Kaydı silmek istiyormusunuz" onayi (`MesajSor`) sonrasi grid'deki secili kullaniciyi `_srv.Delete(data)` ile siler.
- `BtnDuzenle` ("Duzenle", alt bar) — `BtnDegistir_Click`: grid'deki secili kullaniciyi `_mdl`'e klonlar, form alanlarina doldurur (sifre cozulerek `SifreCoz` gosterilir).
- `BtnYeni` ("Yeni", alt bar) — `BtnYeni_Click`: `Temizle()` cagirir; yeni bos `Kullanici` olusturur, tum alanlari sifirlar.
- `BtnKapat` ("Kapat", alt bar) — base form: formu kapatir (genelde Esc).
- `BtnYazdir` ("Yazdir", alt bar) — base form yazdirma (bu formda ozel kod yok; grid yazdirma).
- `BtnIlk` / `BtnOnceki` / `BtnSonraki` / `BtnSon` — base form kayit gezinme butonlari.
- Grid satirina cift tiklama / Enter (`myView1.MyEventDoubleClickEnter` -> `MyView1_MyEventDoubleClickEnter`): secim modundaysa kaydi secip kapatir; degilse secili kaydi form alanlarina doldurur (duzenleme).
- `ChcAdmin` ("Admin" onay kutusu) — kullanicinin admin (tam yetki) olup olmadigini belirler.
**Cagirdigi katmanlar:**
- Service: `IKullaniciService _srv = Ortak.DbPro.Kullanicilar` — `KullaniciService : BaseService<Kullanici>` (ozel metodu yoktur, tum davranis base'den gelir).
  - `_srv.SelectListWhere()` — tum kullanicilari getirir (grid kaynagi, `Bagla()` icinde).
  - `_srv.InsertOrUpdate(_mdl)` — Id varsa UPDATE yoksa INSERT (Kaydet).
  - `_srv.Delete(data)` — secili kullaniciyi siler.
- Service (kullanilmiyor ama enjekte edilmis): `IGenelService _srvGenel = Ortak.DbPro.GenelServis`.
- DataAccess: `KullaniciDal : BaseDal<Kullanici>` (Dapper, ozel sorgu yok; CRUD base'den).
- Entity: `My.Entities.Kullanicilar.Kullanici` — alanlar: `Id (Guid, PK)`, `Adi`, `Soyadi`, `KullaniciAdi`, `Sifre (sifrelenmis)`, `Admin (bool)`; `Clone()` metodu (MemberwiseClone).
- Yardimci: `Ortak.GetKey()` AES anahtari; `string.Sifrele(key)` / `string.SifreCoz(key)` uzanti metotlari (My.Core) ile sifre sifreleme/cozme.
- SQL/Prosedur: yok (saf CRUD; stored procedure cagrilmaz).
- API: yok (UretimV4 DB'ye dogrudan baglanir, API kullanmaz).
**Istasyon sirasiyla iliskisi:** - (Kullanici tablosu uretim miktar/akis motoruyla, operasyon sirasiyla veya istasyon gruplamasiyla iliskili degildir; yalnizca uygulama girisi/yetkilendirme icindir.)
**Notlar:**
- Grid'de `Id` ve `Sifre` kolonlari gizlenir (`SutunGizle("Id")`, `SutunGizle("Sifre")`); sifre listede gosterilmez.
- Form alanlarina kayit yuklenirken sifre cozulur (`SifreCoz`), kaydedilirken yeniden sifrelenir (`Sifrele`); boylece duzenlemede gercek sifre gosterilir.
- Ilk kullanici otomatik olusturma KullaniciModul'de degil, `FrmLogin.Kontrol()` icinde yapilir: `Kullanici` tablosu bossa otomatik "Admin" / sifre "1" (sifrelenmis), Admin=true bir kayit eklenir.
- Giris dogrulamasi (`FrmLogin`) kullanici adini ve sifreyi `ToUpper()` yaparak karsilastirir; bu nedenle kullanici adi/sifre buyuk-kucuk harf duyarsizdir.
- Grid yerlesimi `myGrid1.MyGridKayitAdi = "KullanciKayitListesi"` adiyla saklanir/yuklenir (`GridYerlesimYukle`).
- Tum alanlar `MyMaxLength = 75` (max 75 karakter), `EnterMoveNextControl = true` (Enter ile sonraki alana gecis).
- Hata durumlarinda kullaniciya `MesajHata(rs.Message)` ile mesaj gosterilir; islem yapilmaz.
## Modul: PersonelModul

PersonelModul, uretim sahasinda ve ofiste calisan personel kartlarinin tek bir ekrandan yonetildigi (CRUD) moduldur. Tek bir form icerir: `FrmPersonelKartlari`. Burada tanimlanan personel; istasyon takibi (IstasyonPersoneli + IstasyonKodu eslestirmesi), yetki/admin bayraklari, gorev bazli SMS gonderimi (BAKIM / PLANLAMA gorevleri) ve sifre (MD5 hash) yonetimi icin kullanilir. Kayitlar dogrudan `UretimV3_FEZA` veritabanindaki `Personel` tablosuna Dapper ile yazilir. Kullanici giris/sifre yonetiminden (FrmKullaniciKayit) ayri bir kavramdir; bu modul daha cok saha personeli ve gorev/SMS eslestirmesi icindir. Modulun istasyon-iliskili tarafi: bir personel `IstasyonPersoneli=1` ve `IstasyonKodu` ile bir istasyona baglandiginda, tablet (TabletV2) istasyon takip akisinda o istasyonda kimlerin calisabilecegini/etiketlenecegini belirler.

### Personel Kaydi (`FrmPersonelKartlari.cs` / `FrmPersonelKartlari.Designer.cs`)
**Ne ise yarar:** Personel kartlari listesini gosterir ve yeni personel ekleme, var olani duzenleme, silme islemlerini yapar. Bir personelin Kodu, Adi, Soyadi, Grup, Gorevi, CepTel, Sifre alanlari ile Yetkili / Admin / IstasyonPersoneli / SmsGonder bayraklarini ve istasyon eslestirmesini (IstasyonKodu) yonetir.
**Once ne olmali (onkosul):** FrmAna (ana ekran) acik olmali; Ribbon -> "Personel Kartlari" (BarBtnPersonelListesi, Id=118) butonuna tiklanmali. Form `ShowDialog()` ile modal acilir. Sayac uretmek icin `AyarSayac` tablosunda "Personel" kodlu satir, grup/gorev combolari icin `Personel` tablosunda Grup/Gorevi degerleri, istasyon combosu icin `IstasyonKarti` kayitlari hazir olmalidir (bunlar form yuklenirken otomatik cekilir).
**Sonra ne olur:** Kaydet sonrasi `Personel` tablosuna INSERT (yeni) ya da UPDATE (mevcut) yapilir; sifre girilmisse `GetInsOrUpdCode()` SQL'i (sifre dahil), girilmemisse `GetUpdCodeSifresiz()` SQL'i (sifre haric) calistirilir. Islem sonrasi grid `Bagla()` ile yeniden yuklenir, form alanlari `Temizle()` ile sifirlanir ve kayit grubu (GrpKayit) tekrar pasiflesir. Silme sonrasi ilgili satir `Personel` tablosundan kaldirilir. Ayri bir ekrana gidilmez; ayni form acik kalir. Form secim modunda acilmissa (SecimIcinAcildi) cift tiklama/Enter ile secilen personel geri dondurulup form kapanir.
**Butonlar & kisayollar:**
- `BtnYeni` (Yeni) — `BtnYeni_Click`: `Temizle()` cagirip kayit grubunu (GrpKayit) aktiflestirir, yeni bos kayit girisi hazirlar (YeniKayit=true).
- `BtnKaydet` (Kaydet) — `BtnKaydet_Click`: Form alanlarini `_mdl` (Personel) nesnesine aktarir; Kodu bossa `EvrakNoAl()` ile otomatik kod uretir; CepTel maskesini ("(000) 000-0000") temizler; `KodVarmi()` ile ayni Kodda kayit kontrolu yapar; yeni kayitta sifre zorunlulugunu denetler; sonra `Kaydet()` calisir.
- `BtnSil` (Sil) — `BtnSil_Click`: "Kaydi silmek istiyormusunuz" onayi (MesajSor) sonrasi secili gridteki personeli `_srv.Delete(data)` ile siler.
- `BtnDuzenle` (Duzenle) — `BtnDegistir_Click`: Gridte secili personeli `AktarTextlere(data)` ile form alanlarina doldurur, duzenleme moduna gecer (GrpKayit aktif, YeniKayit=false).
- `BtnKapat` (Kapat) — base form (MyFrmKayit) davranisi; formu kapatir (genel Esc=Kapat kalibi). Bu modulde ozel event baglanmamis.
- `BtnYazdir` (Yazdir) — Designer'da tanimli ancak bu formda Click event'i baglanmamis (kullanilmiyor).
- Navigasyon butonlari `BtnIlk` / `BtnOnceki` / `BtnSonraki` / `BtnSon` — base form (MyFrmKayit) gezinme butonlari; bu formda ozel event baglanmamis.
- Grid cift tiklama / Enter — `myView1_MyEventDoubleClickEnter`: Secim modunda (SecimIcinAcildi) satiri secip formu kapatir; normal modda `AktarTextlere()` ile satiri forma doldurur (duzenleme).
- Form kontrol kisayollari: `CepTel` alaninda maske "(000) 000-0000"; `Sifre` alani PasswordChar='*'; tum kayit alanlarinda EnterMoveNextControl=true (Enter ile bir sonraki kontrole gecis); gridde EnterMoveNextColumn=true. Base MyFrmKayit kalibi geregi Enter=Kaydet / Esc=Kapat genel davranislari uygulanir (ozel ShortcutKeys/ToolStrip menu ogesi bu formda tanimli degil).
**Cagirdigi katmanlar:**
- Manager/Service: `IPersonelService.SelectListWhere()` (PersonelService -> BaseService) — `Personel` tablosundaki tum personeli listeler, grid'e baglar (`Bagla()`).
- Manager/Service: `IPersonelService.PersonelKaydet(Personel, yenikayitmi)` — once `KodVarmi(Kodu)` ile mukerrer kod denetimi, sonra `Personel.GetInsOrUpdCode()` SQL'ini (Id varsa UPDATE / yoksa INSERT, sifre dahil) Dapper Execute ile calistirir.
- Manager/Service: `IPersonelService.PersonelKaydetSifresiz(Personel, yenikayitmi)` — sifre bos birakildiginda `Personel.GetUpdCodeSifresiz()` SQL'i ile sadece UPDATE yapar, mevcut sifreyi degistirmez.
- Manager/Service: `IPersonelService.Sifrele64(metin)` — sifreyi "mikroconnect" tuzu ile birlestirip MD5 hash uretir; kaydetmeden once `_mdl.Sifre` bu degerle doldurulur.
- Manager/Service: `IPersonelService.Delete(Personel)` (BaseService) — secili personeli `Personel` tablosundan siler.
- Manager/Service: `IGenelService.GetEvrakNo("Personel")` (GenelService -> GenelDal.GetEvrakNo) — `AyarSayac` tablosundan "Personel" sayacini okuyup BasinaEkle + BasamakSayisi ile formatlanmis yeni kod uretir ve Verilecek degerini +1 artirir (Kodu bossa cagrilir).
- Manager/Service: `IGenelService.GrupListesi("Personel","Grup")` ve `GrupListesi("Personel","Gorevi")` (GenelDal.GrupListesi) — `Select Grup/Gorevi as Kodu From Personel group by ...` ile mevcut benzersiz Grup ve Gorevi degerlerini cekip CmbGrup / CmbGorevi combolarina baglar (basa bos satir eklenir).
- Manager/Service: `IIstasyonKartiService.SelectListWhere(" Order By IstasyonKodu ")` — istasyon kartlarini cekip CmbIstasyonKodu (MyLookupEdit) lookup'ina IstasyonKodu degeriyle baglar.
- Manager/Service: `IGenelService.Query<int>(sql, entity)` — formdaki `KodVarmi()` mukerrer kod kontrolu icin `Select count(*) From Personel where Kodu=@Kodu` benzeri sorguyu calistirir (ClassExtensions.GetClassTableName / GetClassColumnNameKey ile tablo ve PK adi dinamik alinir).
- SQL/Prosedur: `Personel.GetInsOrUpdCode()` — IF EXISTS(Id) UPDATE ELSE INSERT; tum alanlari (Kodu, Adi, Soyadi, Grup, Gorevi, Yetkili, Admin, Sifre, IstasyonPersoneli, IstasyonKodu, CepTel, SmsGonder) yazar.
- SQL/Prosedur: `Personel.GetUpdCodeSifresiz()` — Sifre haric tum alanlari UPDATE eder (WHERE Id=@Id).
- SQL/Prosedur: `GenelDal.GetEvrakNo` (inline T-SQL, AyarSayac uzerinde) — yeni Personel kodu uretir.
- API: - (UretimV4 masaustu; bu form dogrudan DB'ye baglanir, API kullanmaz)
**Istasyon sirasiyla iliskisi:** Dogrudan operasyon-Sira akis motoruyla (Uretim_MiktarGuncelle / Uretim_PlanlananGuncelle / Uretim_SonrakiIstasyonaGonder) bu form etkilesmez. Ancak burada `IstasyonPersoneli=1` ve `IstasyonKodu` ile bir istasyona baglanan personel, tablet (TabletV2) istasyon takip akisinda (IstasyonTakipPage baslat/durdur/bitir) o istasyonda calisan personeli temsil eder; istasyon eslestirmesi bu formdaki CmbIstasyonKodu (IstasyonKarti listesi) ile yapilir.
**Notlar:**
- Grid'de `Id` ve `Sifre` kolonlari gizlenir (`SutunGizle`). Grid yerlesimi `MyGridKayitAdi="PersonelKartlariListesi"` adiyla kaydedilir/yuklenir (`GridYerlesimYukle`).
- Kodu "Admin"/"admin" olan kayitta `TxtKodu` ve `TxtAdi` alanlari salt-okunur yapilir (admin kullanicisinin kodu/adi degistirilemez).
- Sifre kurali: Guncellemede sifre alani bos birakilirsa sifre degismez (form uzerindeki label1: "Guncellemede Sifre Girilmezse Sifreyi Degistirmez"); yeni kayitta sifre zorunludur ("Lutfen sifre Giriniz").
- SMS kurali (form uzerindeki label2): SMS gonderilmesi icin Gorevi alanina Bakimci icin "BAKIM", Planlamaci icin "PLANLAMA" yazilmali; ChcSmsGonder isaretli olmali. CepTel maske disindaki "-" ve bosluklar kaydedilmeden once temizlenir.
- Form base sinifi `MyFrmKayit` (My.Kontrol.Formlar, derlenmis DLL) — pnlAltBtn / BtnKaydet / BtnSil / BtnYeni / BtnDuzenle / BtnKapat / navigasyon butonlari ve genel Enter/Esc kalibi base'den gelir; bu form sadece ilgili Click event'lerini `EventlerBagla()` icinde baglar.
- Formun bilinen bir bug'i: form icindeki `KodVarmi()` mukerrer kod bulundugunda `new ErrorResult(...)` nesnesini olusturur ama `return` etmez (satir 245), bu yuzden formdaki kontrol mukerrer kodu engellemez; gercek engelleme servisteki `PersonelService.KodVarmi` ile `PersonelKaydet/PersonelKaydetSifresiz` icinde yapilir.
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
## Modul: Aciklamalar

Aciklamalar modulu, uretim genelinde kullanilan **kodlu serbest aciklama/etiket listelerini** (kod + deger ciftleri) tek bir merkezi tabloda (`AciklamaKod`, Uretim DB / UretimV3_FEZA) yonetir. Klasorde tek bir form vardir: `FrmAciklamaKodlar`. Bu form generic bir CRUD ekranidir; hangi amac icin acildigini disaridan set edilen `AciklamaModulTuru` enum'u belirler (`ReceteAciklama`, `OperasyonAciklama`). Form acilinca yalnizca o modul turune ait kayitlari listeler (`where Modul='<tur>' Order By Sira,Kodu`), kod/deger ekle-duzenle-sil islemleri yapar ve `Sira` alaniyla siralama tutar. Bu kodlar dogrudan uretim akisini veya istasyon hareketlerini DEGISTIRMEZ; tanim/sozluk niteligindedir. Uretildikleri yerlerde (orn. recete bazinda aciklama secimi yapan `FrmReceteAciklamalar`) acilir liste kaynagi olarak okunurlar. Form `My.Kontrol.Formlar.MyFrmKayit` base sinifindan turer (ust GroupControl form bolgesi + alt grid + base alt buton seridi Kaydet/Sil/Yeni/Duzenle/Yazdir/Kapat ve gezinme butonlari). Grid'de cift tiklama/Enter ortak `MyEventDoubleClickEnter` olayini tetikler: form secim modunda (`SecimIcinAcildi`) acildiysa satiri secip kapatir, aksi halde Duzenle butonunu calistirir.

### Aciklama Kodlar (`FrmAciklamaKodlar.cs` / `FrmAciklamaKodlar.Designer.cs`)
**Ne ise yarar:** `AciklamaKod` tablosundaki kodlu aciklama tanimlarinin tam CRUD ekranidir (Kodu + Deger/`Deger1` + `Sira`). Tek form, `AciklamaModulTuru` enum degerine gore farkli amaclarla (su an menuden yalnizca **ReceteAciklama**; enum'da ayrica **OperasyonAciklama** tanimli) acilir ve sadece o modul turune ait kayitlari yonetir. Pencere basligi ve ust baslik etiketi (`lblBaslik`) enum'un string karsiligi ile doldurulur.
**Once ne olmali (onkosul):** Form `AciklamaModulTuru` set edilerek acilmali. Ana menude (`FrmAna`) "Recete Aciklamalar" (`BarBtnReceteAciklama`, Caption="Recete Aciklamalar", Id=137) tiklanir; handler `BarBtnReceteAciklama_ItemClick` formu olusturup `AciklamaModulTuru = AciklamaModulTuru.ReceteAciklama` atar ve `ShowDialog()` ile acar. `AciklamaKod` tablosu DB'de mevcut olmali (sema `AciklamaKodCreates.AciklamaKodCreate` ile uretilir).
**Sonra ne olur:** Kaydet -> secili modul turu `Modul` alanina string olarak yazilarak `IAciklamaKodService.InsertOrUpdate(_mdl)` cagrilir; basarili ise `KayitEdildi=true` set edilip liste `Bagla()` ile yeniden cekilir (form acik kalir). Sil -> `IAciklamaKodService.Delete(_mdl)`; basarili ise liste yenilenir. Degisen tablo: `AciklamaKod`. Bu kayitlar daha sonra tuketici ekranlarda (orn. `MyUI\ReceteModul\FrmReceteAciklamalar.cs` icinde `kodService.SelectListWhere(" where Modul='ReceteAciklama' Order By Sira,Kodu")`) acilir aciklama listesi kaynagi olarak okunur. Hicbir stored procedure veya uretim/istasyon tablosu tetiklenmez.
**Butonlar & kisayollar:**
- `Kaydet` (`BtnKaydet`, base) — `BtnKaydet_Click` -> `Kaydet()`: `TextLeriKontrolEt()` (Kodu zorunlu) -> `AktarModele()` (Id bos ise yeni Guid, `Modul`=enum.ToString()) -> `_srv.InsertOrUpdate(_mdl)` -> `Bagla()`. (ardindan tekrar `Bagla()`).
- `Sil` (`BtnSil`, base) — `BtnSil_Click`: "Kaydı silmek istiyormusunuz.." onayi sorar; secili kayit yoksa (`_mdl.Id` bos) bilgi mesaji verir; `Sil()` -> `_srv.Delete(_mdl)` -> `Bagla()`.
- `Yeni` (`BtnYeni`, base) — `BtnYeni_Click` -> `TemizleText()`: yeni bos `AciklamaKod`, `IdGuid=Guid.Empty`, Kodu/Adi kutularini bosaltir.
- `Düzenle` (`BtnDuzenle`, base) — `BtnDegistir_Click`: secili grid satirini (`myView1.MyGetCurrentItem<AciklamaKod>()`) klonlayip (`_mdl = itm.Clone()`) forma yukler (`AktarTextlere`); klonlama acilis sirasinda `AcilisBittimi=false/true` ile cevrelenir.
- Grid cift tiklama / Enter (`myView1.MyEventDoubleClickEnter` -> `MyView1_MyEventDoubleClickEnter`) — `SecimIcinAcildi` ise satiri secip (`SecilenKod`, `SecilenRow`, `SecilenId`, `Secildi=true`) formu kapatir; degilse `BtnDuzenle.PerformClick()`.
- `Kodu` (`TxtKodu`, MyTextEdit, MaxLength 75) — kod girisi; bos olamaz (`TextLeriKontrolEt`).
- `Deger` (`TxtAdi`, MyTextEdit, MaxLength 75) — `Deger1` alanina karsilik gelir.
- Grid'de `Sira` sutunu duzenlenebilir (`SutunEditAc("Sira")` + `SutunReadOnlyKapat("Sira")`); listeleme `Sira, Kodu` sirasina gore yapilir.
- `Yazdır` (`BtnYazdir`, base) — base form butonu, bu formda ozel olay baglanmamis (varsayilan base davranis).
- `Kapat` (`BtnKapat`, base) — formu kapatir (base davranis).
- Gezinme butonlari `BtnIlk` / `BtnOnceki` / `BtnSonraki` / `BtnSon` (base) — base kayit gezinme seridi; bu formda ozel olay baglanmamis.
**Cagirdigi katmanlar:**
- Manager/Service: `IAciklamaKodService` (`Ortak.DbPro.AciklamaKod` uzerinden) — `SelectListWhere($"where Modul='{AciklamaModulTuru}' Order By Sira,Kodu ")`, `InsertOrUpdate(AciklamaKod)`, `Delete(AciklamaKod)`. Servis kendi metodu eklemez; `BaseService<AciklamaKod>` ve `IBaseService<AciklamaKod>` temel CRUD'unu kullanir.
- DataAccess: `AciklamaKodDal : BaseDal<AciklamaKod>` — Dapper tabanli generic DAL (`AciklamaKod` tablosu, `[Table("AciklamaKod")]`, PK `Id` Guid).
- Entity: `AciklamaKod` (alanlar: `Id`, `Modul`, `Kodu`, `Deger1`, `Deger2`, `Deger3`, `Sira`; `Clone()` metodu). Grid'de `Id`, `Modul`, `Deger2`, `Deger3` sutunlari gizlenir (`SutunGizle`).
- Enum: `AciklamaModulTuru { ReceteAciklama, OperasyonAciklama }` (My.Entities.UretimAciklamalar).
- SQL/Prosedur: Stored procedure cagrilmaz. Sema `AciklamaKodCreates.AciklamaKodCreate` (My.DataBaseSettings) ile olusturulur. Tek SQL, generic DAL'in urettigi SELECT/INSERT/UPDATE/DELETE.
- API: - (UretimV4 masaustu; API kullanmaz).
**Istasyon sirasiyla iliskisi:** - (Dogrudan iliskisi yoktur. Kayitlar miktar/akis motorunu, operasyon Sira'sini veya UretimIstasyon olusturmayi etkilemez; yalnizca tanim/sozluk verisidir ve tuketici ekranlarda acilir liste olarak okunur.)
**Notlar:** Namespace `MyUI.Aciklamalar`. Grid duzeni `AciklamalariKodListesi1` adiyla saklanir (`MyGridKayitAdi`), `myGrid1.GridYerlesimYukle()` ile yuklenir. Event'ler Designer yerine kod ile baglanir (`EventlerBagla()`). `Kaydet()` icinde zaten `Bagla()` cagrilirken `BtnKaydet_Click` bir kez daha `Bagla()` cagirir (cift yenileme). Designer'da base butonlar icin ShortcutKeys/F-tuslari atanmamistir; Enter=Kaydet / Esc=Kapat gibi kisayollar varsa `MyFrmKayit` base sinifindan gelir (base kaynak bu repoda yok, harici My.Kontrol DLL'inde). `AciklamaModulTuru` enum'unda `OperasyonAciklama` degeri tanimli olsa da `FrmAna` menusunde bu turu acan bir buton bulunamadi (su an yalnizca `ReceteAciklama` baglanmis). Bu form ile `MyUI\IstasyonModul\FrmIstasyonAciklamalari` (farkli tablo/enum: `IstasyonAciklama` + `IstasyonAciklamaModulTuru`) karistirmamak gerekir; isim benzer, veri kaynagi farklidir.
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
## Modul: HizliUretimModule

HizliUretimModule, UretimV4 (CepPatronERP) masaustu uygulamasinda tek bir ekrandan "tek tikla" uretim girisi yapmak icin kullanilir. Klasorde tek form vardir: `FrmHizliUretimEG`. Normal uretim akisi (Siparis -> Uretim Emri -> Istasyon/Operasyon hareketleri -> Mikro'ya kaydetme, bkz. SiparisModule / UretimModule / MikroModul) yerine bu ekran, secilen bir recetenin tek seferde hem urun girisini hem de recete detaylarindaki (hammadde/sarf) stoklarin cikisini Mikro ERP'ye yazar. Yani is emri/istasyon zinciri olmadan, recete + miktar bilgisiyle dogrudan Mikro `STOK_HAREKETLERI`'ne bir fis uretir. Form `My.Kontrol.Formlar` kutuphanesindeki `MyFrmKayit` base formundan turer; alt buton seridi (Kaydet/Yeni/Duzenle/Sil/Yazdir/Kapat + Ilk/Onceki/Sonraki/Son gezinme butonlari) base'den gelir, ancak bu ekranda fiilen yalnizca `BtnKaydet` ve `BtnKapat` islevseldir (`BtnSil.Visible=false`). Veri iki ayri DatabaseFactory ile islenir: `Ortak.DbPro` (UretimV3_FEZA uretim DB — recete okuma) ve `Ortak.DbMikro` (Mikro ERP — depo listesi + fis yazma). Fis turu ve depo/seri/gider/proje gibi tum parametreler kullanici tanimli `MikroEntegre` ayarlarindan (`Ortak.MikroEntAyarlar`, Grup = `HizliUretimFisi`) okunur.

### Hizli Uretim Kayit (`FrmHizliUretimEG.cs` / `.Designer.cs`)
**Ne ise yarar:** Bir recete (UretimV4 `ReceteAna`) ve miktar secilerek tek seferde Mikro ERP'ye uretim fisi yazar: secilen recete kodunun mamul **giris** hareketi + recetenin tum detaylarindaki (`ReceteDetay.VarsayilanStokKodu`) hammadde/sarf stoklarin **cikis** hareketi olusturulur. Cikis miktarlari recete detay miktari x girilen mamul miktari olarak hesaplanir; her detayda `FireYuzde` varsa fire payi eklenir. Istasyon/operasyon adimlari, is emri ve siparis baglantisi yoktur — adi ustunde "hizli" tek-ekran giristir.
**Once ne olmali (onkosul):**
- Mikro entegrasyon ayarlari tanimli olmali (`FrmMikroEntAyarlari` ekraninda Modul=`MikroEntegre`, Grup=`HizliUretimFisi` icin `EvrakSeri`, `GirisDepoKodu`, `CikisDepoKodu`, `GirisGiderKodu`, `CikisGiderKodu` vb. ve Grup=`FisTuru` icin `HizliUretimFisiTuru` = `StokVirmanFisi` veya `UretimHareketFisi`; ayrica Grup=`GENEL` icin `FirmaNo`/`KullaniciKodu`). Bunlar `Ortak.MikroEntAyarlar` cache'inden okunur.
- Mikro DB (`Ortak.DbMikro`) ve uretim DB (`Ortak.DbPro`) baglantilari hazir olmali.
- Stok kodu alanindan bir recete secilmis olmali (`rcaId` dolu olmali); aksi halde Kaydet'te recete bulunamaz.
- Form acilista, ayarlarda tanimli `GirisDepoKodu`'na karsilik gelen depo Giris Depo alanina otomatik secilir; depo listesi Mikro `DEPOLAR` tablosundan yuklenir.
- `Ortak.PlKapat` ayarina gore PartiNo/LotNo alanlari gizlenir/gosterilir (PlKapat aktifse parti/lot gizlenir).
**Sonra ne olur:**
- Kaydet'te recete `Ortak.DbPro` (UretimV3_FEZA) `ReceteAna`/`ReceteDetay` tablolarindan okunur (yazma yapilmaz; bu DB salt okunur kullanilir).
- Olusturulan `StokHareketleriModel` listesi secili fis turune gore `MikroStokHareketleri` kayitlarina cevrilir ve Mikro ERP `STOK_HAREKETLERI` tablosuna yazilir (parti/lot takipli stoklarda `PARTILOT`, renk/beden takipli stoklarda `BEDEN_HAREKETLERI` kayitlari da otomatik eklenir). Tum yazma tek transaction icinde yapilir.
- `HizliUretimFisiTuru = StokVirmanFisi` ise virman fisi (sth_cins=3, sth_evraktip=6), `UretimHareketFisi` ise uretim hareket fisi (sth_cins=7, sth_evraktip=7) olarak yazilir; tanimsiz/baska deger gelirse varsayilan olarak StokVirmanFisi kullanilir.
- Basarili kayitta "KayıtEdildi" bilgi mesaji gosterilir ve `BtnKaydet` pasiflestirilir (ayni fisin tekrar yazilmasini engellemek icin). Hata olursa `MesajHata` ile uyari verilir, kayit yapilmaz.
- Not: Kayit sonrasi UretimV4 tarafinda hicbir tabloya yazma/guncelleme yoktur (kodda siparis/evrak guncelleme bloklari yorum satiri olarak birakilmis).
**Butonlar & kisayollar:**
- `Kaydet` (BtnKaydet) — `Kaydet()` metodunu cagirir; fisi olusturup Mikro'ya yazar. (base'de Enter = Kaydet davranisi gelir.)
- `Kapat` (BtnKapat) — formu kapatir. (base'de Esc = Kapat.)
- `Sil` (BtnSil) — gizli (`Visible=false`), bu ekranda kullanilmaz.
- `Yeni` / `Duzenle` / `Yazdir` ve gezinme butonlari (Ilk/Onceki/Sonraki/Son) — base `MyFrmKayit`'ten gelir; bu ekranda anlamli bir islevi yoktur (kayit gezme/listeleme mantigi bu form icin kurulmamis).
- `StokKodu` alani button-click (TxtStokKodu butonu) — `FrmReceteListesi`'ni secim modunda (Maximized) acar; recete secilince StokKodu/StokAdi/Birim doldurulur ve `rcaId` set edilir.
- `StokAdi` alani button-click (TxtStokAdi butonu) — yukaridakiyle ayni davranis (recete secim penceresini acar).
- Alanlar: `StokKodu`, `StokAdi`, `Birim` (TxtBirim1, secilen recetenin `EntegreBirim` degeri), `Miktar` (TxtMiktar, >0 zorunlu), `Giris Depo` (TxtDepoNoGiris lookup), `PartiNo` (TxtPartiNo, PlKapat'a gore gizli), `Lot` (TxtLotNo, PlKapat'a gore gizli).
**Cagirdigi katmanlar:**
- Manager: `ReceteManager.GetReceteKayit(Guid? rcaId)` (`Ortak.DbPro`) — secili recetenin ana + detay (+ stok/istasyon/renk-beden) kayitlarini `ReceteKayitModel` olarak getirir; Kaydet, bunun `ReceteDetaylar` listesini cikis hareketleri icin kullanir.
- Manager: `MikroConvertManager.SetHizliUretimFisiUrunGirisAyar(sth, ayarlar, depoGir)` — mamul giris hareketine ayarlardan giris depo (ekrandan secilen depo override eder), evrak seri, gider/proje/sorumluluk merkezi alanlarini set eder (`GirisDepoKodu`/`GirisGiderKodu`).
- Manager: `MikroConvertManager.SetHizliUretimFisiStokCikisAyar(sth2, ayarlar)` — her recete detayindan olusan cikis hareketine ayarlardan cikis depo/gider (`CikisDepoKodu`/`CikisGiderKodu`) ve seri bilgisini set eder.
- Manager: `MikroConvertManager.ConvertStokVirmanFisi(lisStokFisi, ayarlar)` — StokHareketleriModel listesini virman fisine (sth_cins=3, sth_evraktip=6) cevirir; `GetStokVirmanEvrakSira(seri)` ile siradaki evrak no'yu hesaplar, firma/kullanici kodunu ayarlardan alir, depo 0 ise 1'e cevirir.
- Manager: `MikroConvertManager.ConvertUretimHareketFisi(lisStokFisi, ayarlar)` — alternatif fis turu; uretim hareket fisine (sth_cins=7, sth_evraktip=7) cevirir; `GetUretimHareketFisiEvrakSira(seri)` ile evrak sirasi hesaplar.
- Manager: `MikroKayitManager.StokHareketKaydet(lisMikro)` — Mikro `STOK_HAREKETLERI`'ne yazar (transaction). Stok kartinin `sto_detay_takip` / `sto_renkDetayli` / `sto_bedenli_takip` degerlerine bakarak gerektiginde `PARTILOT` (parti/lot) ve `BEDEN_HAREKETLERI` (renk/beden) kayitlarini da uretir; parti no daha once girilmisse hata doner.
- Helper: `MikroKayitFisTurleri.GetHizliUretimFisiTuru(ayarDegeri)` — `FisTuru/HizliUretimFisiTuru` ayar metnini `MikroFisGirisTurleri` enum'una cevirir (yalnizca `StokVirmanFisi` veya `UretimHareketFisi`).
- Service: `IMikroGenelService.GetMikroDepoListesi(" order by dep_no")` (`Ortak.DbMikro.GenelServis`) — Giris Depo lookup'i icin Mikro `DEPOLAR` tablosundan (`dep_no`, `dep_adi`) depo listesini ceker.
- SQL/Prosedur: Dogrudan saklı prosedur cagrilmaz. Kullanilan sorgular ham SQL'dir: recete okuma `ReceteAna`/`ReceteDetay` uzerinden (QueryBuilder SelectFirst/SelectList); evrak sira hesaplama `SELECT ISNULL(MAX(sth_evrakno_sira),0) ... FROM STOK_HAREKETLERI`; stok takip tipi `SELECT sto_detay_takip, sto_renkDetayli, sto_bedenli_takip ... FROM STOKLAR`; yazma `STOK_HAREKETLERI` / `PARTILOT` / `BEDEN_HAREKETLERI` insert.
- API: -
**Istasyon sirasiyla iliskisi:** Yoktur. Bu ekran istasyon/operasyon zincirini (UretimIstasyon, IstasyonHareketler, ReceteyeBagliIstasyon) tamamen atlar; secilen recetenin `GetReceteKayit` ile gelen `ReceteyeBagliIstasyonlar` koleksiyonu okunsa da Kaydet mantiginda kullanilmaz. Yalnizca recetenin malzeme listesi (`ReceteDetaylar`) reçeteden uretim sarfini hesaplamak icin kullanilir. Yani "hizli uretim" = istasyon bazli takip yapmadan, recete + miktardan dogrudan stok giris/cikis fisi.
**Notlar:**
- Form ana menuden `FrmAna.BarBtnHizliUretim_ItemClick` ile modal (`ShowDialog`) acilir; parametre/onkosul gerektirmez.
- `Miktar <= 0` ise kayit engellenir ("Lütfen Miktar Giriniz").
- Cikis miktari hesabi: `gec_miktar = ReceteDetay.Miktar + (Miktar * FireYuzde/100)`, sonra `sth_miktar = gec_miktar * mamulMiktari`. (Fire yuzdesi recete detay birim miktarina uygulanir, sonra mamul miktariyla carpilir.)
- Tum hareketlerde `sth_special2 = "HZL"` (hizli uretim isareti); `StokHareketKaydet` ayrica `sth_special3 = "AKT"` (aktarildi) damgasini ekler. Giris hareketi `sth_tip=0`, cikis hareketleri `sth_tip=1`.
- PartiNo/Lot yalnizca mamul giris hareketine (`sth_parti_kodu`, `sth_lot_no`) yazilir; recete detay (cikis) hareketlerinde parti/lot bos birakilir. Parti/lot takipli stokta `StokHareketKaydet` lot no bos ise 1'e tamamlar ve mukerrer parti kontrolu yapar.
- `_mikroStokService` ve `_mngMikroKayit` alanlari ile renk/beden bagla metotlari (RenkBagla/BedenBagla) tanimli ama tamamen yorum satirinda; ekranda renk/beden secimi suanda devre disi (renk/beden hareketi yine de stok kartinin takip tipine gore StokHareketKaydet icinde otomatik islenir — fakat bu ekrandan Renk/Beden degeri gonderilmedigi icin pratikte renk/beden hareketi olusmaz).
- Belge no bos (`sth_belge_no = ""`) ve tarih alanlari `DateTime.Now` olarak set edilir; tutar/fiyat bilgisi gonderilmez (sth_tutar = Fiyat(0) * miktar = 0).
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
