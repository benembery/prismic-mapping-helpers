using System.Reflection;
using prismic;

namespace benembery.PrismicMapping.Core
{
    public class PrismicGroupAttribute : PrismicFieldAttribute
    {
        public PrismicGroupAttribute(string name = null)
            : base(name)
        {
        }

        protected override object GetValue(Document document, string documentName, PropertyInfo property)
        {
            var group = document.GetGroup(GetFieldName(documentName, property));

            return group.GroupDocs;
        }
    }
}