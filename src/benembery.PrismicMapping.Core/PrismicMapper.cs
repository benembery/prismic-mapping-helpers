using System.Collections.Generic;
using System.Runtime.Caching;
using System.Linq;
using System.Reflection;
using prismic;

namespace benembery.PrismicMapping.Core
{
    public static class PrismicMapper
    {
        private const string keyPrefix =  "PRIMSIC_MAPCTX_";

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
            foreach (var property in ctx.DestinationMappings)
            {
                property.Value.SetValue(property.Key, source, ctx);
            }

            return ctx.Destination;
        }

        private static PrismicMappingContext<T> GetContext<T>(string documentName = null) where T: new ()
        {
            var ctx = GetContextFromCache<T>(documentName);

            if (ctx != null)
                return ctx;

            ctx = new PrismicMappingContext<T>(documentName);
            CacheContext(ctx, documentName);

            return ctx;

        }

        private static PrismicMappingContext<T> GetContextFromCache<T>(string documentName = null) where T: new ()
        {
            var docsOfType = _cache.OfType<PrismicMappingContext<T>>();

            return !string.IsNullOrWhiteSpace(documentName) 
                ? docsOfType.FirstOrDefault(x => x.DocumentType == documentName) 
                : docsOfType.FirstOrDefault();
        }

        private static void CacheContext<T>(PrismicMappingContext<T> ctx, string documentName = null) where T :new()
        {
            _cache.Add(ctx);
        }
    }
}