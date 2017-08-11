using System;
using System.Collections.Generic;
using System.Linq;
using prismic;

namespace benembery.prismic.Mapping.Tests
{
	public class PrismicSetupFixture : IDisposable
	{
		public  Document Document { get; }
		public IList<Document> Results { get; }
        
		public PrismicSetupFixture()
		{
			try
			{
				var p = Api.Get("https://primsic-mapping-integration-tests.prismic.io/api").Result;

				var response = p.Form("everything").Ref(p.Master).Submit().Result;

				if (!response.Results.Any())
					throw new Exception("No documents in response.");

				Results = response.Results;
				Document = response.Results.First(x => x.Id == "WKVdFSwAACgAaFfw");
			}
			catch (Exception e)
			{
				throw new Exception("Could not get document from prismic", e);
			}
		}

		public void Dispose()
		{
		}


	}
}