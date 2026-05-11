using My.Business.Service.Depolar;
using My.Business.Service.Geneller;
using My.Business.Service.MikroModul;
using My.Core.Logger;
using My.DataAccess.Geneller;
using My.DataAccess.MikroModul;
using System.Data;
using Unity;
using Unity.Injection;

namespace My.Business.Dependency.Unit
{
    internal class UnityMicro
    {
        private readonly IDbConnection connection;
        private readonly IUnityContainer container;

        public UnityMicro(IDbConnection _connection)
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
        container.RegisterType<IMikroPartiLotService, MikroPartiLotService>(TypeLifetime.Scoped, new InjectionConstructor(connection, log));
        container.RegisterType<IMikroGenelService, MikroGenelService>(TypeLifetime.Scoped, new InjectionConstructor(connection, log));

 
        }
        public void DataAccess()
        {
           //  container.RegisterType<IGenelDal, GenelDal>(TypeLifetime.Scoped, new InjectionConstructor(connection));

            container.RegisterType<IMikroStokDal, MikroStokDal>(TypeLifetime.Scoped,
                new InjectionConstructor(connection));
            container.RegisterType<IMikroCariDal, MikroCariDal>(TypeLifetime.Scoped,
                new InjectionConstructor(connection));
            container.RegisterType<IMikroSiparisDal, MikroSiparisDal>(TypeLifetime.Scoped,
                new InjectionConstructor(connection));
            container.RegisterType<IMikroSiparisHareketDal, MikroSiparisHareketDal>(TypeLifetime.Scoped,
                new InjectionConstructor(connection));
            container.RegisterType<IMikroStokHareketleriDal, MikroStokHareketleriDal>(TypeLifetime.Scoped,
                new InjectionConstructor(connection));
        }

        public void Business()
        {
          //  container.RegisterType<IMikroGenelService, MikroGenelService>();
            container.RegisterType<IMikroStokService, MikroStokService>();
            container.RegisterType<IMikroCariService, MikroCariService>();
            container.RegisterType<IMikroSiparisService, MikroSiparisService>();
            container.RegisterType<IMikroSiparisHareketService, MikroSiparisHareketService>();
            container.RegisterType<IMikroStokHareketleriService, MikroStokHareketleriService>();
        }
    }
}