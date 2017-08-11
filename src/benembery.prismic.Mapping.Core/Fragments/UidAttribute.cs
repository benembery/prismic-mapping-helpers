using System.Reflection;
using prismic;

namespace benembery.prismic.Mapping.Core.Fragments
{
    public class UidAttribute : FragmentAttribute
    {
        protected override object GetValue(Document document, string documentName, PropertyInfo property)
        {
            return document.Uid;
        }
    }
}