using System;
using benembery.prismic.Mapping.Core;
using benembery.prismic.Mapping.Core.Fragments;
using prismic.fragments;

namespace benembery.prismic.Mapping.Tests.Benchmark
{
	[Document("test")]
	internal class TestMappingClass
	{
		[Uid]
		public string Id { get; set; }

		[TextField()]
		public string Text { get; set; }

		[TextField("text_named")]
		public string NamedText { get; set; }

		[StructuredTextField]
		public StructuredText StructuredText { get; set; }

		[StructuredTextField("structured_text_named")]
		public StructuredText NamedStructuredText { get; set; }

		[Date]
		public DateTime DateTime { get; set; }

		[Date("datetime_named")]
		public DateTime NamedDateTime { get; set; }

		[Date]
		public DateTime? DateTimeNullable { get; set; }

		[Date("datetime_nullable_named")]
		public DateTime? NamedDateTimeNullable { get; set; }


		public TestMappingClass()
		{

		}
	}
}