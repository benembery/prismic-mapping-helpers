using System;
using prismic;

namespace benembery.PrismicMapping.Core
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public abstract class PrismicChildPropertyAttribute : Attribute
    {
        private readonly string _name;

        protected PrismicChildPropertyAttribute(string name = null)
        {
            _name = name;
        }

        public abstract object GetValue(Document document, string documentType);

        protected virtual T GetValue<T>(Document document, string documentType) where T : new()
        {
            return PrismicMapper.Map<T>(document, documentType);
        }
    }
}