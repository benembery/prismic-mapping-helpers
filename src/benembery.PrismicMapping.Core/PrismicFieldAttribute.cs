using System;
using System.Reflection;
using prismic;

namespace benembery.PrismicMapping.Core
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field)]
    public abstract class PrismicFieldAttribute : Attribute
    {
        private readonly string _name;

        protected PrismicFieldAttribute(string name = null)
        {
            _name = name;
        }

        public abstract object GetValue(Document document, string documentName, PropertyInfo property);

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
}