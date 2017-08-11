using prismic;
using System;
using System.Collections.Generic;
using System.Linq;
using benembery.prismic.Mapping.Core;
using StackExchange.Profiling;


namespace benembery.prismic.Mapping.Tests.Benchmark
{
	internal class Program
	{
		private static int iterations = 100000;

		private static void Main(string[] args)
		{
			Console.WriteLine("Starting benchmark for {0} iteration(s)...", iterations);
			var documentFixture = Fixture.GetDocument();


			MiniProfiler.Settings.ProfilerProvider = new SingletonProfilerProvider();
			MiniProfiler.Start();

			using (MiniProfiler.Current.Step("Initial Map"))
			{
				Mapper.Map<TestMappingClass>(documentFixture);
			}

			using (MiniProfiler.Current.Step("Repeat Maps"))
			{
				for (var i = 0; i < iterations; i++)
				{
					Mapper.Map<TestMappingClass>(documentFixture);
				}
			}

			var repeatMaps = MiniProfiler.Current.Root.Children.First(x => x.Name == "Repeat Maps");
			Console.WriteLine(MiniProfiler.Current.RenderPlainText() + "> Average Mapping Time: {0}ms", repeatMaps.DurationMilliseconds.GetValueOrDefault() / iterations);

			MiniProfiler.Stop();
			Console.WriteLine("Press 'Enter' to exit benchmark ...");
			Console.ReadLine();
		}

		private static Document GetMockDocument(string type, IDictionary<string, Fragment> fragments, ISet<string> tags = null)
		{
			tags = tags ?? new SortedSet<string>();
			var docuementId = "Asdadadaal23049823n";

			return new Document(docuementId, docuementId, type, "http://example.com", tags, new List<string>(), fragments);
		}

		
	}
}

