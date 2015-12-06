using System.Collections.Generic;
using System.Reflection;
using prismic;
using prismic.fragments;

namespace benembery.PrismicMapping.Core
{
    public class PrismicStructuredTextFieldAttribute : PrismicFieldAttribute
    {
        public PrismicStructuredTextFieldAttribute(string name = null)
            : base(name)
        {
        }

        protected override object GetValue(Document document, string documentName, PropertyInfo propertyInfo)
        {
            return document.GetStructuredText(GetFieldName(documentName, propertyInfo))
                   ?? new StructuredText(new List<StructuredText.Block>());
        }
    }
}