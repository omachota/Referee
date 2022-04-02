using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Referee.Infrastructure
{
	public class GithubAssets
	{
		[JsonProperty("assets")] public List<Asset> Assets { get; set; }
	}

	public class Asset
	{
		[JsonProperty("browser_download_url")] public Uri BrowserDownloadUrl { get; set; }
	}
}
