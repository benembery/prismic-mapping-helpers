using System;
using System.Collections.Generic;
using prismic;
using prismic.fragments;

namespace benembery.prismic.Mapping.Tests.Benchmark
{
	internal class Fixture
	{
		public static Document GetDocument()
		{
			var fragments = GenerateMockDocumentFragments();
			return GetMockDocument("test", fragments);
		}

		private static Document GetMockDocument(string type, IDictionary<string, Fragment> fragments, ISet<string> tags = null)
		{
			tags = tags ?? new SortedSet<string>();
			var docuementId = "Asdadadaal23049823n";

			return new Document(docuementId, docuementId, type, "http://example.com", tags, new List<string>(), fragments);
		}

		private static Dictionary<string, Fragment> GenerateMockDocumentFragments()
		{

			return new Dictionary<string, Fragment>
			{
				{
					"test.structured", new StructuredText(new List<StructuredText.Block>
					{
						new StructuredText.Paragraph(Lorem, new List<StructuredText.Span>(), "test")
					})
				},
				{
					"test.structured_text_named", new StructuredText(new List<StructuredText.Block>
					{
						new StructuredText.Paragraph(Lorem, new List<StructuredText.Span>(), "test")
					})
				},
				{"test.datetime", new Date(DateTime.Now)},
				{"test.datetime_named", new Date(DateTime.Now)},
				{"test.childtext", new Text(Lorem)},
				{"test.child_text_named", new Text(Lorem)}
			};
		}

		private static string Lorem =
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus urna tellus, rutrum quis faucibus at, feugiat vel ipsum. Donec auctor sollicitudin augue, eget ultricies enim commodo in. Nam eu commodo nulla.";
	}
}