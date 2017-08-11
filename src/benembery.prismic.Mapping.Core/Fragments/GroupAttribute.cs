using System.Reflection;
using prismic;

namespace benembery.prismic.Mapping.Core.Fragments
{
    public class GroupAttribute : FragmentAttribute
    {
        public GroupAttribute(string name = null)
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