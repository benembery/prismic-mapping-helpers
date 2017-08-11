using System.Collections.Generic;
using System.Linq;
using prismic;

namespace benembery.prismic.Mapping.Core
{
    public static class Mapper
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

        private static T Map<T>(Document source, MappingContext<T> ctx) where T : new()
        {
            var dest = new T();

            foreach (var property in ctx.DestinationMappings)
            {
                property.Value.SetValue(source, dest, property.Key, ctx);
            }

            return dest;
        }

        private static MappingContext<T> GetContext<T>(string documentName = null) where T: new ()
        {
            var ctx = GetContextFromCache<T>(documentName);

            if (ctx != null)
                return ctx;

            ctx = new MappingContext<T>(documentName);
            CacheContext(ctx);

            return ctx;

        }

        private static MappingContext<T> GetContextFromCache<T>(string documentName = null) where T: new ()
        {
            var docsOfType = _cache.OfType<MappingContext<T>>();

            return !string.IsNullOrWhiteSpace(documentName) 
                ? docsOfType.FirstOrDefault(x => x.DocumentType == documentName) 
                : docsOfType.FirstOrDefault();
        }

        private static void CacheContext<T>(MappingContext<T> ctx) where T :new()
        {
            _cache.Add(ctx);
        }
    }
}