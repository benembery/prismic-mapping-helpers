using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amido.NAuto;
using Amido.NAuto.Randomizers;
using benembery.PrismicMapping.Core;
using FluentAssertions;
using NUnit.Framework;
using prismic;
using prismic.fragments;

namespace benembery.PrismicMapping.Tests.Core
{
    [TestFixture]
    public class PrismicMappingTestFixture
    {
        private Document _document;
        private string _docType = "mapping-test-document";
        private IList<Document> _results;

        [OneTimeSetUp]
        public async Task FixtureSetUp()
        {
            try
            {
                var p = await Api.Get("https://primsic-mapping-integration-tests.prismic.io/api");

                var response = await p.Form("everything")
                    .Ref(p.Master)
                    .Submit();
                
                if (!response.Results.Any())
                    throw new Exception("No documents in response.");

                _results = response.Results;
                _document = response.Results.First(x => x.Id == "WKVdFSwAACgAaFfw");
            }
            catch (Exception e)
            {
                throw new AssertionException("Could not get document from prismic", e);
            }
        }
        
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void Map_throws_Exception_when_destination_does_not_have_DocumentAttribute()
        {
            Action action = () =>
            {
                PrismicMapper.Map<object>(_document);
            };

            action.ShouldThrow<Exception>().WithMessage("Prismic DocumentAttribute is required");
        }

        [Test]
        public void Map_throws_Exception_when_destination_DocumentAttribute_has_no_name()
        {
            Action action = () => PrismicMapper.Map<TestMappingClassWithNoDocumentName>(_document);

            action.ShouldThrow<Exception>().WithMessage("Document name is required");
        }

        [Test]
        public void Map_correctly_returns_PrismicUid_value()
        {
            var result = PrismicMapper.Map<TestMappingClass>(_document);

            result.Id.Should().Be(_document.Uid);
        }

        [Test]
        public void Map_correctly_returns_PrismicText_value()
        {
            var result = PrismicMapper.Map<TestMappingClass>(_document);

            result.Text.Should().Be(GetFragment<Text>("text").Value);
        }

        [Test]
        public void Map_correctly_returns_PrismicText_value_for_named_field()
        {
            var result = PrismicMapper.Map<TestMappingClass>(_document);

            result.NamedText.Should().Be(GetFragment<Text>("text_named").Value);
        }

        [Test]
        public void Map_correctly_returns_PrismicStructuredText_value()
        {
            var result = PrismicMapper.Map<TestMappingClass>(_document);

            result.StructuredText.getFirstParagraph().Text.Should().Be(GetFragment<StructuredText>("structuredtext").getFirstParagraph().Text);
        }

        [Test]
        public void Map_correctly_returns_PrismicStructuredText_value_for_named_field()
        {
            var result = PrismicMapper.Map<TestMappingClass>(_document);

            result.NamedStructuredText.getFirstParagraph().Text.Should().Be(GetFragment<StructuredText>("structured_text_named").getFirstParagraph().Text);
        }

        [Test]
        public void Map_correctly_returns_PrismicDate_value()
        {
            var result = PrismicMapper.Map<TestMappingClass>(_document);

            result.Date.Should().Be(GetFragment<Date>("date").Value);
        }

        [Test]
        public void Map_correctly_returns_DateTime_MinValue_when_PrismicDate_is_null()
        {
            var doc = _results.FirstOrDefault(x => x.Id == "WKWNzSwAACoAaS5h");

            var result = PrismicMapper.Map<TestMappingClass>(doc);

            result.Date.Should().Be(DateTime.MinValue);
        }

        [Test]
        public void Map_correctly_returns_PrismicDate_value_for_named_field()
        {
            var result = PrismicMapper.Map<TestMappingClass>(_document);

            result.NamedDate.Should().Be(GetFragment<Date>("date").Value);
        }
        
        [Test]
        public void Map_correctly_returns_PrismicChildProperty_value()
        {
            var result = PrismicMapper.Map<TestMappingClass>(_document);

            result.Child.ChildText.Should().Be(GetFragment<Text>("childtext").Value);
        }

        [Test]
        public void Map_correctly_returns_PrismicChildProperty_value_for_named_field()
        {
            var result = PrismicMapper.Map<TestMappingClass>(_document);

            result.Child.ChildNamedText.Should().Be(GetFragment<Text>("text_named").Value);
        }

        private T GetFragment<T>(string fieldName) where T : Fragment
        {
            return _document.Get($"{_docType}.{fieldName}").As<T>();
        }

        [PrismicDocument("")]
        private class TestMappingClassWithNoDocumentName
        {
        }

        [PrismicDocument("mapping-test-document")]
        private class TestMappingClass
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
            public DateTime Date { get; set; }

            [PrismicDate("date")]
            public DateTime NamedDate { get; set; }

            [PrismicDate]
            public DateTime? DateTimeNullable { get; set; }

            [PrismicDate("datetime_nullable_named")]
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
            [PrismicTextField]
            public string ChildText { get; set; }

            [PrismicTextField("text_named")]
            public string ChildNamedText { get; set; }
        }

        private class TestChildPropertyAttribute : PrismicChildPropertyAttribute
        {
            protected override object GetValue(Document document, string documentType)
            {
                return GetValue<TestChildPropertyMappingClass>(document, documentType);
            }
        }
    }
}
