using My.Business.Dependency.Unit;
using My.Business.Service.Ayarlar;
using My.Business.Service.Depolar;
using My.Business.Service.Geneller;
using My.Business.Service.IstasyonAciklamalar;
using My.Business.Service.IstasyonBakimlar;
using My.Business.Service.IstasyonKartlar;
using My.Business.Service.IstasyonTakipler;
using My.Business.Service.Kullanicilar;
using My.Business.Service.MailAyarlar;
using My.Business.Service.MesajLar;
using My.Business.Service.OperasyonKartlar;
using My.Business.Service.Personeller;
using My.Business.Service.ReceteGruplar;
using My.Business.Service.ReceteIstasyonGruplar;
using My.Business.Service.ReceteIstasyonlar;
using My.Business.Service.Receteler;
using My.Business.Service.ReceteStoklar;
using My.Business.Service.Siparisler;
using My.Business.Service.SmsAyarlar;
using My.Business.Service.Templer;
using My.Business.Service.UretimAciklamalar;
using My.Business.Service.UretimEmirler;
using My.Business.Service.UretimIstasyonlar;
using My.Business.Service.UretimKontroller;
using My.Business.Service.UretimOperasyonlar;
using My.Business.Service.UretimStoklar;
using My.Business.Service.UretimTalepler;
using My.Core;
using My.Core.Logger;
using System.Data;
using Unity;

namespace My.Business
{
    public class DatabaseFactoryPro
    {
        private readonly IDbConnection connection;
        private readonly IUnityContainer container;

        public DatabaseFactoryPro(string anaKey, string databaseName = "Uretim")
        {
            Settings = DbConnectionSettings.GetSetting(databaseName, anaKey);
            var con = DbConnectionSettings.GetConnectionByString(Settings.ConnectionString);
            connection = con;
            container = new UnityPro(connection).GetContainer();
        }
        public DatabaseModel Settings { get; set; }
        public ILogManager MyLogger => container.Resolve<ILogManager>();
        public IAyarService Ayarlar => container.Resolve<IAyarService>();
        public IAyarSayacService AyarSayaclar => container.Resolve<IAyarSayacService>();
        public IGenelService GenelServis => container.Resolve<IGenelService>();
        public IReceteAnaService ReceteAna => container.Resolve<IReceteAnaService>();
        public IReceteDetayService ReceteDatay => container.Resolve<IReceteDetayService>();
        public IReceteStokService ReceteStok => container.Resolve<IReceteStokService>();
        public IReceteGrupService ReceteGrup => container.Resolve<IReceteGrupService>();
        public IReceteGrupDetayService ReceteGrupDetay => container.Resolve<IReceteGrupDetayService>();
        public ISiparisService Siparis => container.Resolve<ISiparisService>();
        public ISiparisHareketService SiparisHareket => container.Resolve<ISiparisHareketService>();
        public ISiparisHareketDetayService SiparisHareketDetay => container.Resolve<ISiparisHareketDetayService>();
  
        public IOperasyonKartiService OperasyonKarti => container.Resolve<IOperasyonKartiService>();
        public IIstasyonKartiService IstasyonKarti => container.Resolve<IIstasyonKartiService>();
        public IReceteOperasyonService ReceteOperasyon => container.Resolve<IReceteOperasyonService>();
        public IReceteIstasyonService ReceteIstasyon => container.Resolve<IReceteIstasyonService>();
        public IReceteIstasyonCariService ReceteIstasyonCari => container.Resolve<IReceteIstasyonCariService>();
        public IUretimEmriService UretimEmri => container.Resolve<IUretimEmriService>();
        public IUretimStokService UretimStok => container.Resolve<IUretimStokService>();
        public IUretimStokFisService UretimStokFis => container.Resolve<IUretimStokFisService>();
        public IUretimOperasyonService UretimOperasyon => container.Resolve<IUretimOperasyonService>();
        public IUretimOperasyonHareketService UretimOperasyonHareket => container.Resolve<IUretimOperasyonHareketService>();
        public IUretimOperasyonHareketDetayService UretimOperasyonHareketDetay => container.Resolve<IUretimOperasyonHareketDetayService>();
        public IUretimIstasyonService UretimIstasyon => container.Resolve<IUretimIstasyonService>();
        public IUretimIstasyonHareketService UretimIstasyonHareket => container.Resolve<IUretimIstasyonHareketService>();
        public IPersonelService Personel => container.Resolve<IPersonelService>();
        public IMailAyarService MailSettings => container.Resolve<IMailAyarService>();
        public IUretimTalepService UretimTalep => container.Resolve<IUretimTalepService>();
        public IUretimTalepHareketService UretimTalepHareket => container.Resolve<IUretimTalepHareketService>();
        public IReceteyeBagliIstasyonService ReceteyeBagliIstasyon => container.Resolve<IReceteyeBagliIstasyonService>();
        public IIstasyonAciklamaService IstasyonAciklama => container.Resolve<IIstasyonAciklamaService>();
        public IAciklamaKodService AciklamaKod => container.Resolve<IAciklamaKodService>();
        public IAciklamaDegerService AciklamaDeger => container.Resolve<IAciklamaDegerService>();
        public IIstasyonTakipHareketService IstasyonTakipHareket => container.Resolve<IIstasyonTakipHareketService>();
        public IIstasyonTakipHareketLogService IstasyonTakipHareketLog => container.Resolve<IIstasyonTakipHareketLogService>();
        public IIstasyonTakipStokHareketService IstasyonTakipStokHareket => container.Resolve<IIstasyonTakipStokHareketService>();
        public IIstasyonTakipHareketDetayService IstasyonTakipHareketDetay => container.Resolve<IIstasyonTakipHareketDetayService>();
        public IMesajlarService Mesajlar => container.Resolve<IMesajlarService>();
        public IReceteStokRenkBedenService ReceteStokRenkBeden => container.Resolve<IReceteStokRenkBedenService>();
        public IIstasyonBakimService IstasyonBakim => container.Resolve<IIstasyonBakimService>();
        public IIstasyonBakimParcaService IstasyonBakimParca => container.Resolve<IIstasyonBakimParcaService>();
        public ISmsAyarService SmsAyar => container.Resolve<ISmsAyarService>();
        public IReceteIstasyonGrupKodService ReceteIstasyonGrupKodlar => container.Resolve<IReceteIstasyonGrupKodService>();
        public IReceteIstasyonGrupIstasyonService ReceteIstasyonGrupIstasyonlar => container.Resolve<IReceteIstasyonGrupIstasyonService>();
        public IReceteIstasyonGrupOperasyonService ReceteIstasyonGrupOperasyonlar => container.Resolve<IReceteIstasyonGrupOperasyonService>();
        public IUretimKontrolService UretimKontroller => container.Resolve<IUretimKontrolService>(); 
        public IKullaniciService Kullanicilar => container.Resolve<IKullaniciService>();
        public IDepoService Depolar => container.Resolve<IDepoService>();
        public IIstasyonTakipStokHareketDetayService IstasyonTakipStokHareketDetay => container.Resolve<IIstasyonTakipStokHareketDetayService>();
        public ITempSiparisUretimMiktarService TempSiparisUretimMiktar => container.Resolve<ITempSiparisUretimMiktarService>();
        public ITempMikroStokService TempMikroStok => container.Resolve<ITempMikroStokService>();
        public IUretimStokFisHareketService UretimStokFisHareket => container.Resolve<IUretimStokFisHareketService>();



    }
}