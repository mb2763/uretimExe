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
