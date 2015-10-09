using System;
using System.Reflection;

namespace benembery.PrismicMapping.Core
{
    public class PrismicMappingContext<T>
        where T : new()
    {
        private T _destination;
        public Type DestinationType { get; set; }
        public string DocumentType { get; set; }

        public PrismicMappingContext(string documentType = null)
        {
            _destination = new T();
            DestinationType = _destination.GetType();

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

        public T Destination { get { return _destination; } }
    }
}