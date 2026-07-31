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
