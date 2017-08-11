using System.Collections.Generic;
using System.Reflection;
using prismic;
using prismic.fragments;

namespace benembery.prismic.Mapping.Core.Fragments
{
    public class StructuredTextFieldAttribute : FragmentAttribute
    {
        public StructuredTextFieldAttribute(string name = null)
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