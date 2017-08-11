using System.Reflection;
using prismic;

namespace benembery.prismic.Mapping.Core.Fragments
{
    public class PrismicSliceZoneAttribute : FragmentAttribute
    {
        public PrismicSliceZoneAttribute(string name = null)
            : base(name)
        {
        }

        protected override object GetValue(Document document, string documentName, PropertyInfo propertyInfo)
        {
            return document.GetSliceZone(GetFieldName(documentName, propertyInfo)).Slices;
        }
    }
}