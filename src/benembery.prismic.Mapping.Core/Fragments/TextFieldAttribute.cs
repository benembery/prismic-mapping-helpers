using System.Reflection;
using prismic;

namespace benembery.prismic.Mapping.Core.Fragments
{
    public class TextFieldAttribute : FragmentAttribute
    {
        public TextFieldAttribute(string name = null)
            : base(name)
        {
        }

        protected override object GetValue(Document document, string documentName, PropertyInfo propertyInfo)
        {
            return document.GetText(GetFieldName(documentName, propertyInfo));
        }
    }
}