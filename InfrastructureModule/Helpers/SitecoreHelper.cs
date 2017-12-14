using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.StringExtensions;

namespace InfrastructureModule.Helpers
{
	public class SitecoreHelper
	{
		public static Item GetHomeItem()
		{
			return Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);
		}

		public static Item GetSearchItem()
		{
			var homeItemPath = Sitecore.Context.Site.StartPath;
			var searchItem = Sitecore.Context.Database.SelectItems($"fast:{homeItemPath}/*[@@templatekey='{SitecoreHardcode.SearchItemTemplateName}']").First();

			return searchItem;
		}

		public static Item GetPostsItem()
		{
			var homeItemPath = Sitecore.Context.Site.StartPath;
			var postsItem = Sitecore.Context.Database.SelectItems($"fast:{homeItemPath}/*[@@templatekey='{SitecoreHardcode.PostsItemTemplateName}']").First();

			return postsItem;
		}

		public static Item GetPostTagtreeRootItem()
		{
			var i = SitecoreHelper.GetSiteSettingItem();
			var postTagTreeRootItemId = i.Fields["PostTagTreeRootItemId"].Value;

			if (postTagTreeRootItemId.IsNullOrEmpty())
			{
				throw new NullReferenceException();
			}

			var id = new ID(postTagTreeRootItemId);
			return Sitecore.Context.Database.GetItem(id);
		}

		public static Item GetPostCategoryrootItemId()
		{
			var postCategoryrootItemId = SitecoreHelper.GetSiteSettingItem().Fields["PostCategoryRootItemId"].Value;

			if (postCategoryrootItemId.IsNullOrEmpty())
			{
				throw new NullReferenceException();
			}

			var id = new ID(postCategoryrootItemId);
			return Sitecore.Context.Database.GetItem(id);
		}

		public static Item GetSiteSettingItem()
		{
			var homeItemPath = Sitecore.Context.Site.ContentStartPath;
			var siteSettingTemplateName = SitecoreHardcode.SiteSettingTemplateName;
			if (siteSettingTemplateName.IsNullOrEmpty())
			{
				throw new NullReferenceException();
			}

			return Sitecore.Context.Database.SelectSingleItem($"fast:{homeItemPath}/*[@@templatekey='{siteSettingTemplateName}']");
		}

		public static Item GetNotFoundPageItem()
		{
			var cont = Sitecore.Context.Database;
			var homeItemPath = Sitecore.Context.Site.StartPath;
			var notFoundPageTemplateName = SitecoreHardcode.NotFoundPageTemplateName;
			if (notFoundPageTemplateName.IsNullOrEmpty())
			{
				throw new NullReferenceException();
			}

			return Sitecore.Context.Database.SelectSingleItem($"fast:{homeItemPath}/*[@@templatekey='{notFoundPageTemplateName}']");
		}

		public static Item GetEmailSettingsItem()
		{
			var homeItemPath = Sitecore.Context.Site.ContentStartPath;
			var emailSettingsTemplateName = SitecoreHardcode.EmailSettingsTemplateName;
			var emailSettingsItem =
				Sitecore.Context.Database.SelectSingleItem(
					$"fast:{homeItemPath}/local storage//*[@@templatekey='{emailSettingsTemplateName}']");

			return emailSettingsItem;
		}

		public static Item[] GetAllSitesRootItems()
		{
			var siteTemplateName = SitecoreHardcode.SiteTemplateName;
			var result = Sitecore.Context.Database.SelectItems($"fast:/sitecore/content/*[@@templatekey='{siteTemplateName}']/home/posts");
			return result;
		}
	}
}