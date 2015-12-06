using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace benembery.PrismicMapping.Core
{
    public class PrismicMappingContext<T>
        where T : new()
    {
        private T _destination;
        public Type DestinationType { get; set; }
        public Dictionary<PropertyInfo, PrismicFieldAttribute> DestinationMappings { get; set; }
        public string DocumentType { get; set; }

        public PrismicMappingContext(string documentType = null)
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

            var docType = DestinationType.GetCustomAttribute<PrismicDocumentAttribute>();

            if (docType == null)
                throw new Exception("Prismic DocumentAttribute is required");

            if (string.IsNullOrWhiteSpace(docType.Name))
                throw new Exception("Document name is required");
            
            DocumentType = docType.Name;
        }


        private static Dictionary<PropertyInfo, PrismicFieldAttribute> GetPropertyMappings(PropertyInfo[] properties)
        {
            var mappings = new Dictionary<PropertyInfo, PrismicFieldAttribute>();

            foreach (var property in properties)
            {
                var customAttributes = property.GetCustomAttributes().ToList();

                PrismicFieldAttribute attribute = customAttributes.OfType<PrismicFieldAttribute>().FirstOrDefault();

                if(attribute == null)
                    continue;
                
                mappings.Add(property,attribute);
            }

            return mappings;
        } 
    }
}