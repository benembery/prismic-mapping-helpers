using System.CodeDom;
using System.Linq;
using System.Reflection;
using prismic;

namespace benembery.PrismicMapping.Core
{
    public static class PrismicMapper
    {
        public static T Map<T>(Document source) where T : new()
        {
            var ctx = new PrismicMappingContext<T>();

            return Map(source, ctx);
        }

        public static T Map<T>(Document source, string documentName) where T : new()
        {
            var ctx = new PrismicMappingContext<T>(documentName);

            return Map(source, ctx);
        }

        private static T Map<T>(Document source, PrismicMappingContext<T> ctx) where T : new()
        {
            var destProperties = ctx.DestinationType.GetProperties();

            foreach (var property in destProperties)
            {
                property.MapProperty(source, ctx);
            }

            return ctx.Destination;
        }

        private static void MapProperty<T>(this PropertyInfo property, Document source, PrismicMappingContext<T> ctx)
            where T : new()
        {
            var attributes = property.GetCustomAttributes().ToList();

            //if(!attributes.Any())
            //    return;

            var child = attributes.OfType<PrismicChildPropertyAttribute>().FirstOrDefault();

            if (child != null)
            {
                property.SetValue(ctx.Destination, child.GetValue(source, ctx.DocumentType));
                return;
            }
            
            var field = attributes.OfType<PrismicFieldAttribute>().FirstOrDefault();

            if (field == null)
                return;

            property.SetValue(ctx.Destination, field.GetValue(source, ctx.DocumentType, property));
        }
    }
}