using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using benembery.prismic.Mapping.Core;
using benembery.prismic.Mapping.Core.Fragments;
using FluentAssertions;
using prismic;
using prismic.fragments;

namespace benembery.prismic.Mapping.Tests
{
	public class PrismicMappingTestFixture : IClassFixture<PrismicSetupFixture>, IDisposable
    {
        private string _docType = "mapping-test-document";

        private Document _document;

        private IList<Document> _results;

        public PrismicMappingTestFixture(PrismicSetupFixture fixture)
        {
                
            _document = fixture.Document;
            _results = fixture.Results;
        }

        //[SetUp]
        public void SetUp()
        {
        }

        [Fact]
        public void Map_throws_Exception_when_destination_does_not_have_DocumentAttribute()
        {
            Action action = () =>
            {
                Mapper.Map<object>(_document);
            };
            
            action.ShouldThrow<Exception>().WithMessage("Prismic DocumentAttribute is required");
        }

        [Fact]
        public void Map_throws_Exception_when_destination_DocumentAttribute_has_no_name()
        {
            Action action = () => Mapper.Map<TestMappingClassWithNoDocumentName>(_document);

            action.ShouldThrow<Exception>().WithMessage("Document name is required");
        }

        [Fact]
        public void Map_correctly_returns_PrismicUid_value()
        {
            var result = Mapper.Map<TestMappingClass>(_document);

            result.Id.Should().Be(_document.Uid);
        }

        [Fact]
        public void Map_correctly_returns_PrismicText_value()
        {
            var result = Mapper.Map<TestMappingClass>(_document);

            result.Text.Should().Be(GetFragment<Text>("text").Value);
        }

        [Fact]
        public void Map_correctly_returns_PrismicText_value_for_named_field()
        {
            var result = Mapper.Map<TestMappingClass>(_document);

            result.NamedText.Should().Be(GetFragment<Text>("text_named").Value);
        }

        [Fact]
        public void Map_correctly_returns_PrismicStructuredText_value()
        {
            var result = Mapper.Map<TestMappingClass>(_document);

            result.StructuredText.getFirstParagraph().Text.Should().Be(GetFragment<StructuredText>("structuredtext").getFirstParagraph().Text);
        }

        [Fact]
        public void Map_correctly_returns_PrismicStructuredText_value_for_named_field()
        {
            var result = Mapper.Map<TestMappingClass>(_document);

            result.NamedStructuredText.getFirstParagraph().Text.Should().Be(GetFragment<StructuredText>("structured_text_named").getFirstParagraph().Text);
        }

        [Fact]
        public void Map_correctly_returns_PrismicDate_value()
        {
            var result = Mapper.Map<TestMappingClass>(_document);

            result.Date.Should().Be(GetFragment<Date>("date").Value);
        }

        [Fact]
        public void Map_correctly_returns_DateTime_MinValue_when_PrismicDate_is_null()
        {
            var doc = _results.FirstOrDefault(x => x.Id == "WKWNzSwAACoAaS5h");

            var result = Mapper.Map<TestMappingClass>(doc);

            result.Date.Should().Be(DateTime.MinValue);
        }

        [Fact]
        public void Map_correctly_returns_PrismicDate_value_for_named_field()
        {
            var result = Mapper.Map<TestMappingClass>(_document);

            result.NamedDate.Should().Be(GetFragment<Date>("date").Value);
        }
        
        [Fact]
        public void Map_correctly_returns_PrismicChildProperty_value()
        {
            var result = Mapper.Map<TestMappingClass>(_document);

            result.Child.ChildText.Should().Be(GetFragment<Text>("childtext").Value);
        }

        [Fact]
        public void Map_correctly_returns_PrismicChildProperty_value_for_named_field()
        {
            var result = Mapper.Map<TestMappingClass>(_document);

            result.Child.ChildNamedText.Should().Be(GetFragment<Text>("text_named").Value);
        }

        private T GetFragment<T>(string fieldName) where T : Fragment
        {
            return _document.Get($"{_docType}.{fieldName}").As<T>();
        }

        [Document("")]
        private class TestMappingClassWithNoDocumentName
        {
        }

        [Document("mapping-test-document")]
        private class TestMappingClass
        {
            [Uid]
            public string Id { get; set; }

            [TextField]
            public string Text { get; set; }

            [TextField("text_named")]
            public string NamedText { get; set; }

            [StructuredTextField]
            public StructuredText StructuredText { get; set; }

            [StructuredTextField("structured_text_named")]
            public StructuredText NamedStructuredText { get; set; }

            [Date]
            public DateTime Date { get; set; }

            [Date("date")]
            public DateTime NamedDate { get; set; }

            [Date]
            public DateTime? DateTimeNullable { get; set; }

            [Date("datetime_nullable_named")]
            public DateTime? NamedDateTimeNullable { get; set; }

            [TestChildProperty]
            public TestChildPropertyMappingClass Child { get; set; }

            public TestMappingClass()
            {
                Child = new TestChildPropertyMappingClass();
            }
        }

        private class TestChildPropertyMappingClass
        {
            [TextField]
            public string ChildText { get; set; }

            [TextField("text_named")]
            public string ChildNamedText { get; set; }
        }

        private class TestChildPropertyAttribute : ChildPropertyAttribute
        {
            protected override object GetValue(Document document, string documentType)
            {
                return GetValue<TestChildPropertyMappingClass>(document, documentType);
            }
        }

        public void Dispose()
        {
        }
    }
}
