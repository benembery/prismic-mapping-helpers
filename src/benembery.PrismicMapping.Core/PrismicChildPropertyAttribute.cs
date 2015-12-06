using System;
using System.Reflection;
using prismic;

namespace benembery.PrismicMapping.Core
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public abstract class PrismicChildPropertyAttribute : PrismicFieldAttribute
    {

        protected PrismicChildPropertyAttribute()
        {
        }

        protected override object GetValue(Document document, string documentName, PropertyInfo property)
        {
            return GetValue(document, documentName);
        }

        protected abstract object GetValue(Document document, string documentType);
        
        protected virtual T GetValue<T>(Document document, string documentType) where T : new()
        {
            return PrismicMapper.Map<T>(document, documentType);
        }
    }
}