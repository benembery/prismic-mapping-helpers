using System;
using System.Reflection;
using prismic;

namespace benembery.PrismicMapping.Core
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PrismicChildPropertyAttribute : Attribute
    {
        private readonly string _name;

        public PrismicChildPropertyAttribute(string name = null)
        {
            _name = name;
        }

        public object GetValue<T>(Document document, PrismicMappingContext<T> ctx) where T : new()
        {
            
            return PrismicMapper.Map<T>(document, ctx.DocumentType);
        }

        protected string GetFieldName(string documentName, PropertyInfo property)
        {
            var name = _name;

            if (string.IsNullOrWhiteSpace(name))
            {
                if (property != null)
                    name = property.Name.ToLowerInvariant();
            }

            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            return string.Format("{0}.{1}", documentName, name);
        }

    }

    public class PrismicSliceZoneAttribute : PrismicFieldAttribute
    {
        public PrismicSliceZoneAttribute(string name = null)
            : base(name)
        {
        }

        public override object GetValue(Document document, string documentName, PropertyInfo propertyInfo)
        {
            return document.GetSliceZone(GetFieldName(documentName, propertyInfo)).Slices;
        }
    }

    public class PrismicDateAttribute : PrismicFieldAttribute
    {
        public PrismicDateAttribute(string name = null)
            : base(name)
        {
        }

        public override object GetValue(Document document, string documentName, PropertyInfo propertyInfo)
        {
            return null; //return document.GetDateOrDefault(GetFieldName(documentName, propertyInfo));
        }
    }
}
