using System;
using System.Collections.Generic;
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
        
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void Map_throws_Exception_when_destination_does_not_have_DocumentAttribute()
        {
            Action action = () =>
            {
                PrismicMapper.Map<object>(GetMockDocument("test", new Dictionary<string, Fragment>()));
            };

            action.ShouldThrow<Exception>().WithMessage("Prismic DocumentAttribute is required");
        }

        [Test]
        public void Map_throws_Exception_when_destination_DocumentAttribute_has_no_name()
        {
            Action action = () => PrismicMapper.Map<TestMappingClassWithNoDocumentName>(GetMockDocument("test", new Dictionary<string, Fragment>()));

            action.ShouldThrow<Exception>().WithMessage("Document name is required");
        }

        [Test]
        public void Map_correctly_returns_PrismicUid_value()
        {
            var document = GetMockDocument("test", new Dictionary<string, Fragment>());
            var result = PrismicMapper.Map<TestMappingClass>(document);

            result.Id.Should().Be(document.Uid);
        }

        [Test]
        public void Map_correctly_returns_PrismicText_value()
        {
            var fragment = new prismic.fragments.Text(NAuto.GetRandomString(15));
            var fragments = new Dictionary<string, Fragment>
            {
                {"test.text", fragment}
            };
            var document = GetMockDocument("test", fragments);
            var result = PrismicMapper.Map<TestMappingClass>(document);

            result.Text.Should().Be(fragment.Value);
        }

        [Test]
        public void Map_correctly_returns_PrismicText_value_for_named_field()
        {
            var fragment = new prismic.fragments.Text(NAuto.GetRandomString(15));
            var fragments = new Dictionary<string, Fragment>
            {
                {"test.text_named", fragment}
            };
            var document = GetMockDocument("test", fragments);
            var result = PrismicMapper.Map<TestMappingClass>(document);

            result.NamedText.Should().Be(fragment.Value);
        }

        [Test]
        public void Map_correctly_returns_PrismicStructuredText_value()
        {
            var fragment = new StructuredText(new List<StructuredText.Block>
            {
                new StructuredText.Paragraph(NAuto.GetRandomString(15), new List<StructuredText.Span>(), "test")
            });
            var fragments = new Dictionary<string, Fragment>
            {
                {"test.structuredtext", fragment}
            };
            var document = GetMockDocument("test", fragments);
            var result = PrismicMapper.Map<TestMappingClass>(document);

            result.StructuredText.getFirstParagraph().Text.Should().Be(fragment.getFirstParagraph().Text);
        }

        [Test]
        public void Map_correctly_returns_PrismicStructuredText_value_for_named_field()
        {
            var fragment = new StructuredText(new List<StructuredText.Block>
            {
                new StructuredText.Paragraph(NAuto.GetRandomString(15), new List<StructuredText.Span>(), "test")
            });

            var fragments = new Dictionary<string, Fragment>
            {
                {"test.structured_text_named", fragment}
            };
            var document = GetMockDocument("test", fragments);
            var result = PrismicMapper.Map<TestMappingClass>(document);

            result.NamedStructuredText.getFirstParagraph().Text.Should().Be(fragment.getFirstParagraph().Text);
        }

        [Test]
        public void Map_correctly_returns_PrismicDate_value()
        {
            var fragment = new Date(DateTime.Now);
            var fragments = new Dictionary<string, Fragment>
            {
                {"test.datetime", fragment}
            };
            var document = GetMockDocument("test", fragments);
            var result = PrismicMapper.Map<TestMappingClass>(document);

            result.DateTime.Should().Be(fragment.Value);
        }

        [Test]
        public void Map_correctly_returns_DateTime_MinValue_when_PrismicDate_is_null()
        {
            var fragments = new Dictionary<string, Fragment>
            {
            };
            var document = GetMockDocument("test", fragments);
            var result = PrismicMapper.Map<TestMappingClass>(document);

            result.DateTime.Should().Be(DateTime.MinValue);
        }

        [Test]
        public void Map_correctly_returns_PrismicDate_value_for_named_field()
        {
            var fragment = new Date(DateTime.Now);
            var fragments = new Dictionary<string, Fragment>
            {
                {"test.datetime_named", fragment}
            };
            var document = GetMockDocument("test", fragments);
            var result = PrismicMapper.Map<TestMappingClass>(document);

            result.NamedDateTime.Should().Be(fragment.Value);
        }


        [Test]
        public void Map_correctly_returns_PrismicChildProperty_value()
        {
            var fragment = new prismic.fragments.Text(NAuto.GetRandomString(15));
            var fragments = new Dictionary<string, Fragment>
            {
                {"test.childtext", fragment}
            };
            var document = GetMockDocument("test", fragments);
            var result = PrismicMapper.Map<TestMappingClass>(document);

            result.Child.ChildText.Should().Be(fragment.Value);
        }

        [Test]
        public void Map_correctly_returns_PrismicChildProperty_value_for_named_field()
        {
            var fragment = new prismic.fragments.Text(NAuto.GetRandomString(15));
            var fragments = new Dictionary<string, Fragment>
            {
                {"test.child_text_named", fragment}
            };
            var document = GetMockDocument("test", fragments);
            var result = PrismicMapper.Map<TestMappingClass>(document);

            result.Child.ChildNamedText.Should().Be(fragment.Value);
        }

        private Document GetMockDocument(string type, IDictionary<string, Fragment> fragments, ISet<string> tags = null)
        {
            tags = tags ?? new SortedSet<string>();
            var rand = NAuto.GetRandomString(6, 50, CharacterSetType.Anything, Spaces.None, Casing.Any);


            return new Document(rand, rand, type, "http://example.com", tags, new List<string>(), fragments);
        }

        [PrismicDocument("")]
        private class TestMappingClassWithNoDocumentName
        {
        }

        [PrismicDocument("test")]
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
            public DateTime DateTime { get; set; }

            [PrismicDate("datetime_named")]
            public DateTime NamedDateTime { get; set; }

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

            [PrismicTextField("child_text_named")]
            public string ChildNamedText { get; set; }
        }

        private class TestChildPropertyAttribute : PrismicChildPropertyAttribute
        {
            public override object GetValue(Document document, string documentType)
            {
                return GetValue<TestChildPropertyMappingClass>(document, documentType);
            }
        }
    }
}
