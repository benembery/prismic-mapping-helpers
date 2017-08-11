using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using benembery.prismic.Mapping.Core.Fragments;

namespace benembery.prismic.Mapping.Core
{
    public class MappingContext<T>
        where T : new()
    {
        private T _destination;
        public Type DestinationType { get; set; }
        public Dictionary<PropertyInfo, FragmentAttribute> DestinationMappings { get; set; }
        public string DocumentType { get; set; }

        public MappingContext(string documentType = null)
        {
            _destination = new T();
            DestinationType = _destination.GetType();
            
            var destinationProperties = DestinationType.GetProperties();
            DestinationMappings = GetPropertyMappings(destinationProperties);

            if (!string.IsNullOrWhiteSpace(documentType))
            {
                DocumentType = documentType;
                return;
            }

            var docType = DestinationType.GetTypeInfo().GetCustomAttribute<DocumentAttribute>();

            if (docType == null)
                throw new Exception("Prismic DocumentAttribute is required");

            if (string.IsNullOrWhiteSpace(docType.Name))
                throw new Exception("Document name is required");
            
            DocumentType = docType.Name;
        }


        private static Dictionary<PropertyInfo, FragmentAttribute> GetPropertyMappings(PropertyInfo[] properties)
        {
            var mappings = new Dictionary<PropertyInfo, FragmentAttribute>();

            foreach (var property in properties)
            {
                var customAttributes = property.GetCustomAttributes().ToList();

                FragmentAttribute attribute = customAttributes.OfType<FragmentAttribute>().FirstOrDefault();

                if(attribute == null)
                    continue;
                
                mappings.Add(property,attribute);
            }

            return mappings;
        } 
    }
}