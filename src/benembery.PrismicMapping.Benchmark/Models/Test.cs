using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using benembery.PrismicMapping.Core;
using prismic.fragments;

namespace benembery.PrismicMapping.Benchmark.Models
{
    [PrismicDocument("test")]
    internal class TestMappingClass
    {
        [PrismicUid]
        public string Id { get; set; }

        [PrismicTextField]
        public string Text { get; set; }

        [PrismicTextField("text_named")]
        public string NamedText { get; set; }

        [PrismicStructuredTextField]
        public StructuredText StructuredText { get; set; }

        [PrismicStructuredTextField("structured_text_named")]
        public StructuredText NamedStructuredText { get; set; }

        [PrismicDate]
        public DateTime DateTime { get; set; }

        [PrismicDate("datetime_named")]
        public DateTime NamedDateTime { get; set; }

        [PrismicDate]
        public DateTime? DateTimeNullable { get; set; }

        [PrismicDate("datetime_nullable_named")]
        public DateTime? NamedDateTimeNullable { get; set; }

        
        public TestMappingClass()
        {
            
        }
    }

    internal class TestChildPropertyMappingClass
    {
        [PrismicTextField]
        public string ChildText { get; set; }

        [PrismicTextField("child_text_named")]
        public string ChildNamedText { get; set; }
    }
}