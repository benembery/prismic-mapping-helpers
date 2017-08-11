using System;
using System.Reflection;
using prismic;

namespace benembery.prismic.Mapping.Core.Fragments
{
    public class DateAttribute : FragmentAttribute
    {
        public DateAttribute(string name = null)
            : base(name)
        {
        }

        protected override object GetValue(Document document, string documentName, PropertyInfo propertyInfo)
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
