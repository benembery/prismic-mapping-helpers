using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Profiling;
using Amido.NAuto;
using Amido.NAuto.Randomizers;
using benembery.PrismicMapping.Benchmark.Models;
using benembery.PrismicMapping.Core;
using prismic;
using prismic.fragments;

namespace benembery.PrismicMapping.Benchmark
{
    internal class Program
    {
        private static int iterations = 100;

        private static void Main(string[] args)
        {
            Console.WriteLine("Starting benchmark for {0} iteration(s)...", iterations);
            var fragments = GenerateMockDocumentFragments();
            var mockDoc = GetMockDocument("test", fragments);


            MiniProfiler.Settings.ProfilerProvider = new SingletonProfilerProvider();
            MiniProfiler.Start();
            
            using (MiniProfiler.Current.Step("Initial Map"))
            {
                PrismicMapper.Map<TestMappingClass>(mockDoc);
            }
            
            using (MiniProfiler.Current.Step("Repeat Maps"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    PrismicMapper.Map<TestMappingClass>(mockDoc);
                }
            }

            var repeatMaps = MiniProfiler.Current.Root.Children.First(x => x.Name == "Repeat Maps");
            Console.WriteLine(MiniProfiler.Current.RenderPlainText() +"> Average Mapping Time: {0}ms", repeatMaps.DurationMilliseconds.GetValueOrDefault() / iterations);
            
            MiniProfiler.Stop();
            Console.WriteLine("Press 'Enter' to exit benchmark ...");
            Console.ReadLine();
        }

        private static Document GetMockDocument(string type, IDictionary<string, Fragment> fragments, ISet<string> tags = null)
        {
            tags = tags ?? new SortedSet<string>();
            var rand = NAuto.GetRandomString(6, 50, CharacterSetType.Anything, Spaces.None, Casing.Any);

            return new Document(rand, rand, type, "http://example.com", tags, new List<string>(), fragments);
        }

        private static Dictionary<string, Fragment> GenerateMockDocumentFragments()
        {

            return new Dictionary<string, Fragment>
            {
                {
                    "test.structured", new StructuredText(new List<StructuredText.Block>
                    {
                        new StructuredText.Paragraph(NAuto.GetRandomString(15), new List<StructuredText.Span>(), "test")
                    })
                },
                {
                    "test.structured_text_named", new StructuredText(new List<StructuredText.Block>
                    {
                        new StructuredText.Paragraph(NAuto.GetRandomString(15), new List<StructuredText.Span>(), "test")
                    })
                },
                {"test.datetime", new Date(DateTime.Now)},
                {"test.datetime_named", new Date(DateTime.Now)},
                {"test.childtext", new prismic.fragments.Text(NAuto.GetRandomString(15))},
                {"test.child_text_named", new prismic.fragments.Text(NAuto.GetRandomString(15))}
            };
        }
    }
}
