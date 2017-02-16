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

        protected abstract object GetValue(Document document, string documentName, PropertyInfo property);

        public void SetValue<T>(Document source, T destination, PropertyInfo property, PrismicMappingContext<T> context) where T : new()
        {
            property.SetValue(destination, GetValue(source, context.DocumentType, property));
        }

        protected string GetFieldName(string documentName, PropertyInfo property)
        {
            var name = _name;

            if (string.IsNullOrWhiteSpace(name) && property != null)
                name = property.Name.ToLowerInvariant();

            return string.IsNullOrWhiteSpace(name) 
                ? string.Empty 
                : string.Format("{0}.{1}", documentName, name);
        }
    }
}