using System;
using System.Reflection;
using System.Web;
using prismic;

namespace benembery.PrismicMapping.Core
{
    public class PrismicDateAttribute : PrismicFieldAttribute
    {
        public PrismicDateAttribute(string name = null)
            : base(name)
        {
        }

        public override object GetValue(Document document, string documentName, PropertyInfo propertyInfo)
        {
            var date = document.GetDate(GetFieldName(documentName, propertyInfo));

            if (date != null)
                return date.Value;

            if (propertyInfo.PropertyType == typeof (DateTime))
                return DateTime.MinValue;

            return null;
        }
    }
}
