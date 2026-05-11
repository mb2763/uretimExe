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
using My.Core.Logger;
using My.DataAccess.Ayarlar;
using My.DataAccess.Geneller;
using My.DataAccess.IstasyonAciklamalar;
using My.DataAccess.IstasyonBakimlar;
using My.DataAccess.IstasyonKartlar;
using My.DataAccess.IstasyonTakipler;
using My.DataAccess.Kullanicilar;
using My.DataAccess.MailAyarlar;
using My.DataAccess.MesajLar;
using My.DataAccess.OperasyonKartlar;
using My.DataAccess.Personeller;
using My.DataAccess.ReceteGruplar;
using My.DataAccess.ReceteIstasyonGruplar;
using My.DataAccess.ReceteIstasyonlar;
using My.DataAccess.Receteler;
using My.DataAccess.ReceteStoklar;
using My.DataAccess.Siparisler;
using My.DataAccess.SmsAyarlar;
using My.DataAccess.UretimAciklamalar;
using My.DataAccess.UretimEmirler;
using My.DataAccess.UretimIstasyonlar;
using My.DataAccess.UretimKontroller;
using My.DataAccess.UretimOperasyonlar;
using My.DataAccess.UretimStoklar;
using My.DataAccess.UretimTalepler;
using System.Data;
using Unity;
using Unity.Injection;

namespace My.Business.Dependency.Unit
{
    public class UnityPro
    {
        private readonly IDbConnection connection;
        private readonly IUnityContainer container;
        public UnityPro(IDbConnection _connection)
        {
            container = new UnityContainer();
            connection = _connection;
            Ayarla();
        }
        public IUnityContainer GetContainer()
        {
            return container;
        }
        private void Ayarla()
        {
            container.RegisterType<ILogManager, FileLogger>();
            DataAccess();
            Business();
            BusinessCore();
        }
        public void BusinessCore() {
        ILogManager log = container.Resolve<ILogManager>(); 
        container.RegisterType<IDepoService, DepoService>(TypeLifetime.Scoped, new InjectionConstructor(connection, log)); 
        container.RegisterType<ITempSiparisUretimMiktarService, TempSiparisUretimMiktarService>(TypeLifetime.Scoped, new InjectionConstructor(connection, log)); 
        container.RegisterType<IIstasyonTakipStokHareketDetayService, IstasyonTakipStokHareketDetayService>(TypeLifetime.Scoped, new InjectionConstructor(connection, log)); 
        container.RegisterType<ITempMikroStokService, TempMikroStokService>(TypeLifetime.Scoped, new InjectionConstructor(connection, log)); 
        container.RegisterType<IUretimStokFisHareketService, UretimStokFisHareketService>(TypeLifetime.Scoped, new InjectionConstructor(connection, log)); 
        }
        public void DataAccess()
        { 
            container.RegisterType<IGenelDal, GenelDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IAyarDal, AyarDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IAyarSayacDal, AyarSayacDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteAnaDal, ReceteAnaDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteDetayDal, ReceteDetayDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteGrupDal, ReceteGrupDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteGrupDetayDal, ReceteGrupDetayDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteStokDal, ReceteStokDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<ISiparisDal, SiparisDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<ISiparisHareketDal, SiparisHareketDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<ISiparisHareketDetayDal, SiparisHareketDetayDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IKullaniciDal, KullaniciDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IOperasyonKartiDal, OperasyonKartiDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IIstasyonKartiDal, IstasyonKartiDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteOperasyonDal, ReceteOperasyonDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteIstasyonDal, ReceteIstasyonDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteIstasyonCariDal, ReceteIstasyonCariDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IUretimEmriDal, UretimEmriDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IUretimStokDal, UretimStokDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IUretimStokFisDal, UretimStokFisDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IUretimOperasyonDal, UretimOperasyonDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IUretimOperasyonHareketDal, UretimOperasyonHareketDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IUretimOperasyonHareketDetayDal, UretimOperasyonHareketDetayDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IUretimIstasyonDal, UretimIstasyonDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IUretimIstasyonHareketDal, UretimIstasyonHareketDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IPersonelDal, PersonelDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IMailAyarDal, MailAyarDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IUretimTalepDal, UretimTalepDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IUretimTalepHareketDal, UretimTalepHareketDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteyeBagliIstasyonDal, ReceteyeBagliIstasyonDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IIstasyonAciklamaDal, IstasyonAciklamaDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IAciklamaKodDal, AciklamaKodDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IAciklamaDegerDal, AciklamaDegerDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IIstasyonTakipHareketDal, IstasyonTakipHareketDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IIstasyonTakipHareketLogDal, IstasyonTakipHareketLogDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IIstasyonTakipStokHareketDal, IstasyonTakipStokHareketDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IIstasyonTakipHareketDetayDal, IstasyonTakipHareketDetayDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IMesajlarDal, MesajlarDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteStokRenkBedenDal, ReceteStokRenkBedenDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IIstasyonBakimDal, IstasyonBakimDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IIstasyonBakimParcaDal, IstasyonBakimParcaDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<ISmsAyarDal, SmsAyarDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteIstasyonGrupKodDal, ReceteIstasyonGrupKodDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteIstasyonGrupIstasyonDal, ReceteIstasyonGrupIstasyonDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IReceteIstasyonGrupOperasyonDal, ReceteIstasyonGrupOperasyonDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));
            container.RegisterType<IUretimKontrolDal, UretimKontrolDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));

        }

        public void Business()
        {
            container.RegisterType<IGenelService, GenelService>();
            container.RegisterType<IAyarService, AyarService>();
            container.RegisterType<IAyarSayacService, AyarSayacService>();
            container.RegisterType<IReceteAnaService, ReceteAnaService>();
            container.RegisterType<IReceteDetayService, ReceteDetayService>();
            container.RegisterType<IReceteGrupDetayService, ReceteGrupDetayService>();
            container.RegisterType<IReceteGrupService, ReceteGrupService>();
            container.RegisterType<IReceteStokService, ReceteStokService>();
            container.RegisterType<ISiparisService, SiparisService>();
            container.RegisterType<ISiparisHareketService, SiparisHareketService>();
            container.RegisterType<ISiparisHareketDetayService, SiparisHareketDetayService>();
            container.RegisterType<IKullaniciService, KullaniciService>();
            container.RegisterType<IOperasyonKartiService, OperasyonKartiService>();
            container.RegisterType<IIstasyonKartiService, IstasyonKartiService>();
            container.RegisterType<IReceteOperasyonService, ReceteOperasyonService>();
            container.RegisterType<IReceteIstasyonService, ReceteIstasyonService>();
            container.RegisterType<IReceteIstasyonCariService, ReceteIstasyonCariService>();
            container.RegisterType<IUretimEmriService, UretimEmriService>();
            container.RegisterType<IUretimStokService, UretimStokService>();
            container.RegisterType<IUretimStokFisService, UretimStokFisService>();
            container.RegisterType<IUretimOperasyonService, UretimOperasyonService>();
            container.RegisterType<IUretimOperasyonHareketService, UretimOperasyonHareketService>();
            container.RegisterType<IUretimOperasyonHareketDetayService, UretimOperasyonHareketDetayService>();
            container.RegisterType<IUretimIstasyonService, UretimIstasyonService>();
            container.RegisterType<IUretimIstasyonHareketService, UretimIstasyonHareketService>();
            container.RegisterType<IPersonelService, PersonelService>();
            container.RegisterType<IMailAyarService, MailAyarService>();
            container.RegisterType<IUretimTalepService, UretimTalepService>();
            container.RegisterType<IUretimTalepHareketService, UretimTalepHareketService>();
            container.RegisterType<IReceteyeBagliIstasyonService, ReceteyeBagliIstasyonService>();
            container.RegisterType<IIstasyonAciklamaService, IstasyonAciklamaService>();
            container.RegisterType<IAciklamaKodService, AciklamaKodService>();
            container.RegisterType<IAciklamaDegerService, AciklamaDegerService>();
            container.RegisterType<IIstasyonTakipHareketService, IstasyonTakipHareketService>();
            container.RegisterType<IIstasyonTakipHareketLogService, IstasyonTakipHareketLogService>();
            container.RegisterType<IIstasyonTakipStokHareketService, IstasyonTakipStokHareketService>();
            container.RegisterType<IIstasyonTakipHareketDetayService, IstasyonTakipHareketDetayService>();
            container.RegisterType<IReceteStokRenkBedenService, ReceteStokRenkBedenService>();
            container.RegisterType<IMesajlarService, MesajlarService>();
            container.RegisterType<IIstasyonBakimService, IstasyonBakimService>();
            container.RegisterType<IIstasyonBakimParcaService, IstasyonBakimParcaService>();
            container.RegisterType<ISmsAyarService, SmsAyarService>();
            container.RegisterType<IReceteIstasyonGrupKodService, ReceteIstasyonGrupKodService>();
            container.RegisterType<IReceteIstasyonGrupIstasyonService, ReceteIstasyonGrupIstasyonService>();
            container.RegisterType<IReceteIstasyonGrupOperasyonService, ReceteIstasyonGrupOperasyonService>();
            container.RegisterType<IUretimKontrolService, UretimKontrolService>();

        }
    
    }
}




