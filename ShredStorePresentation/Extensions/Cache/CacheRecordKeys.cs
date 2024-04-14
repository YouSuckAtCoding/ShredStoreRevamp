namespace ShredStorePresentation.Extensions.Cache
{
    public class CacheRecordKeys
    {
        private readonly IConfiguration _config;

        public CacheRecordKeys(IConfiguration config)
        {
            _config = config;
        }

        public string GetProductCacheKey()
        {
            string key = _config.GetValue<string>("Cache:productCache")!;
            return key;
        }


    }
}
