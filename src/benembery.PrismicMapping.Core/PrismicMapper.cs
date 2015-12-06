using System.Collections.Generic;
using System.Linq;
using prismic;

namespace benembery.PrismicMapping.Core
{
    public static class PrismicMapper
    {
        private static readonly List<object> _cache = new List<object>();

        public static T Map<T>(Document source) where T : new()
        {
            var ctx = GetContext<T>();

            return Map(source, ctx);
        }

        public static T Map<T>(Document source, string documentName) where T : new()
        {
            var ctx = GetContext<T>(documentName);

            return Map(source, ctx);
        }

        private static T Map<T>(Document source, PrismicMappingContext<T> ctx) where T : new()
        {
            var dest = new T();

            foreach (var property in ctx.DestinationMappings)
            {
                property.Value.SetValue(source, dest, property.Key, ctx);
            }

            return dest;
        }

        private static PrismicMappingContext<T> GetContext<T>(string documentName = null) where T: new ()
        {
            var ctx = GetContextFromCache<T>(documentName);

            if (ctx != null)
                return ctx;

            ctx = new PrismicMappingContext<T>(documentName);
            CacheContext(ctx);

            return ctx;

        }

        private static PrismicMappingContext<T> GetContextFromCache<T>(string documentName = null) where T: new ()
        {
            var docsOfType = _cache.OfType<PrismicMappingContext<T>>();

            return !string.IsNullOrWhiteSpace(documentName) 
                ? docsOfType.FirstOrDefault(x => x.DocumentType == documentName) 
                : docsOfType.FirstOrDefault();
        }

        private static void CacheContext<T>(PrismicMappingContext<T> ctx) where T :new()
        {
            _cache.Add(ctx);
        }
    }
}