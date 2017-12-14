using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Web;

namespace OutOfWebrotApp.Extensions
{
	public static class SitecoreContextSwitcherExtension
	{
		//public static SiteInfo GetSite(this Item item)
		//{
		//	var siteInfoList = Factory.GetSiteInfoList();
		//	var cutDictionary = new Dictionary<SiteInfo, string>();
		//	foreach (SiteInfo siteInfo in siteInfoList)
		//	{
		//		if (siteInfo.RootPath.Length > 0 && item.Paths.FullPath.StartsWith(siteInfo.RootPath))
		//		{
		//			cutDictionary.Add(siteInfo, item.Paths.FullPath.Replace(siteInfo.RootPath, ""));
		//		}
		//	}
		//	SiteInfo result = null;
		//	int min = int.MaxValue;
		//	foreach (var pair in cutDictionary)
		//	{
		//		if (pair.Value.Length < min)
		//		{
		//			result = pair.Key;
		//			min = pair.Value.Length;
		//		}
		//	}

		//	return result;
		//}
		public static SiteInfo GetSiteInfo(this Item item)
		{
			var siteInfoList = Factory.GetSiteInfoList();

			SiteInfo currentSiteinfo = null;
			var matchLength = 0;
			foreach (var siteInfo in siteInfoList)
			{
				if (!item.Paths.FullPath.StartsWith(siteInfo.RootPath, StringComparison.OrdinalIgnoreCase) || siteInfo.RootPath.Length <= matchLength)
					continue;
				matchLength = siteInfo.RootPath.Length;
				currentSiteinfo = siteInfo;
			}
			return currentSiteinfo;
		}
	}
}