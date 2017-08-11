using benembery.prismic.Mapping.Core.Fragments;

namespace benembery.prismic.Mapping.Tests.Benchmark
{
	internal class TestChildPropertyMappingClass
	{
		[TextField]
		public string ChildText { get; set; }

		[TextField("child_text_named")]
		public string ChildNamedText { get; set; }
	}
}