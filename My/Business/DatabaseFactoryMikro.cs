using My.Business.Dependency.Unit;
using My.Business.Service.Geneller;
using My.Business.Service.MikroModul;
using My.Core;
using My.Core.Logger;
using System.Data;
using Unity;

namespace My.Business
{
    public class DatabaseFactoryMikro
    {
        private readonly IDbConnection connection;
        private readonly IUnityContainer container;

        public DatabaseFactoryMikro(string anaKey, string databaseName = "Mikro")
        {
            Settings = DbConnectionSettings.GetSetting(databaseName, anaKey);
            var con = DbConnectionSettings.GetConnectionByString(Settings.ConnectionString);
            connection = con;
            container = new UnityMicro(connection).GetContainer();
        }

        public DatabaseModel Settings { get; set; }
        public ILogManager MyLogger => container.Resolve<ILogManager>();
        public IMikroGenelService GenelServis => container.Resolve<IMikroGenelService>();
        public IMikroStokService Stoklar => container.Resolve<IMikroStokService>();
        public IMikroCariService Cariler => container.Resolve<IMikroCariService>();
        public IMikroSiparisService Siparisler => container.Resolve<IMikroSiparisService>();
        public IMikroSiparisHareketService SiparisHareketler => container.Resolve<IMikroSiparisHareketService>();
        public IMikroStokHareketleriService StokHareketleri => container.Resolve<IMikroStokHareketleriService>();
        public IMikroPartiLotService PartiLotlar => container.Resolve<IMikroPartiLotService>();
    }
}