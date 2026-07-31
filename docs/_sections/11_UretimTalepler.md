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
