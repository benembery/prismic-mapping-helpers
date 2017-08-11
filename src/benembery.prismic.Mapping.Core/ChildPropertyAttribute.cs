using System;
using System.Reflection;
using benembery.prismic.Mapping.Core.Fragments;
using prismic;

namespace benembery.prismic.Mapping.Core
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public abstract class ChildPropertyAttribute : FragmentAttribute
    {

        protected ChildPropertyAttribute()
        {
        }

        protected override object GetValue(Document document, string documentName, PropertyInfo property)
        {
            return GetValue(document, documentName);
        }

        protected abstract object GetValue(Document document, string documentType);
        
        protected virtual T GetValue<T>(Document document, string documentType) where T : new()
        {
            return Mapper.Map<T>(document, documentType);
        }
    }
}