namespace com.b_velop.Slipways.Web.Data
{
    public interface IStoreWrapper
    {
        IServiceStore Services { get; }
        IExtraStore Extras { get; }
        IManufacturerStore Manufacturers { get; }
        ISlipwayStore Slipways { get; }
        IWaterStore Waters { get; }
    }

    public class StoreWrapper : IStoreWrapper
    {
        public IServiceStore Services { get; }
        public IExtraStore Extras { get; }
        public IManufacturerStore Manufacturers { get; }
        public ISlipwayStore Slipways { get; }
        public IWaterStore Waters { get; }

        public StoreWrapper(
            IServiceStore serviceStore,
            IExtraStore extraStore,
            IManufacturerStore manufacturerStore,
            ISlipwayStore slipwayStore,
            IWaterStore waterStore)
        {
            Services = serviceStore;
            Extras = extraStore;
            Manufacturers = manufacturerStore;
            Slipways = slipwayStore;
            Waters = waterStore;
        }
    }
}
