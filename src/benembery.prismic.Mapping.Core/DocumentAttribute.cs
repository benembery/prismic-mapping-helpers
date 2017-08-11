using System;

namespace benembery.prismic.Mapping.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DocumentAttribute : Attribute
    {
        private readonly string _name;

        public DocumentAttribute(string name = null)
        {
            _name = name;
        }

        public string Name { get { return _name; } }
    }
}