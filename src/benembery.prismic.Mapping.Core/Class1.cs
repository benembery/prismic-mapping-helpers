using System.Reflection;
using prismic;

namespace benembery.PrismicMapping.Core
{
    public class PrismicSliceZoneAttribute : PrismicFieldAttribute
    {
        public PrismicSliceZoneAttribute(string name = null)
            : base(name)
        {
        }

        public override object GetValue(Document document, string documentName, PropertyInfo propertyInfo)
        {
            return document.GetSliceZone(GetFieldName(documentName, propertyInfo)).Slices;
        }
    }

    public class PrismicDateAttribute : PrismicFieldAttribute
    {
        public PrismicDateAttribute(string name = null)
            : base(name)
        {
        }

        public override object GetValue(Document document, string documentName, PropertyInfo propertyInfo)
        {
            return null; //return document.GetDateOrDefault(GetFieldName(documentName, propertyInfo));
        }
    }
}
