using System;

namespace benembery.PrismicMapping.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PrismicDocumentAttribute : Attribute
    {
        private readonly string _name;

        public PrismicDocumentAttribute(string name = null)
        {
            _name = name;
        }

        public string Name { get { return _name; } }
    }
}