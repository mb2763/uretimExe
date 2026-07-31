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
