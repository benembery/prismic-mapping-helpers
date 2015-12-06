using System.Reflection;
using prismic;

namespace benembery.PrismicMapping.Core
{
    public class PrismicTextFieldAttribute : PrismicFieldAttribute
    {
        public PrismicTextFieldAttribute(string name = null)
            : base(name)
        {
        }

        protected override object GetValue(Document document, string documentName, PropertyInfo propertyInfo)
        {
            return document.GetText(GetFieldName(documentName, propertyInfo));
        }
    }
}