# Prismic Mapping Helpers
An attribute based mapping helper to populate objects from Prismic Documents

## Nuget
Available on nuget: 

    Install-Package benembery.PrismicMapping.Core


https://www.nuget.org/packages/benembery.PrismicMapping.Core

## Usage

Add attributes to your DTO, you can name the field you want to map in the atrribute `[PrismicTextField("text_named")]` or leave it blank and the mapper will lower case the property name and look for that field name in the prismic document instead.

    [PrismicDocument("your-prismi-document-id")]
    private class ExampleMappingClass
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

        [PrismicDate]
        public DateTime? DateTimeNullable { get; set; }

        public TestMappingClass()
        {
            Child = new TestChildPropertyMappingClass();
        }
    }
    
  
When you need to map the document, call the mapper as follows.
  
    var result = PrismicMapper.Map<ExampleMappingClass>(document);

### Roadmap  
* Add Method `CreateMap` to pre-cache the mapping plans and which will improve first map performance.
* Got a suggestion get in touch on twitter [@benembery](https://twitter.com/benembery)
  
