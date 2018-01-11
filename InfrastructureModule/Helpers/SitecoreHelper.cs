using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using InfrastructureModule.TDS.sitecore.templates.Custom.BaseTemplates.Settings;
using InfrastructureModule.TDS.sitecore.templates.Custom.BlogApp.Pages.NotFound;
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

		public static Item GetPostTagtreeRootItem(ISitecoreService contextService)
		{
			var siteSettingsItem = GetSiteSettingItem(contextService);
			if (siteSettingsItem.PostTagTreeRootItemId == Guid.Empty)
			{
				throw new ArgumentException("postTagTreeRootItem is empty");
			}
			var postTagTreeRootItemId = siteSettingsItem.PostTagTreeRootItemId;
			return Sitecore.Context.Database.GetItem(new ID(postTagTreeRootItemId));
		}

		public static Item GetPostCategoryrootItemId(ISitecoreService sitecoreService)
		{
			var siteSettingsItem = GetSiteSettingItem(sitecoreService);
			if (siteSettingsItem.PostCategoryRootItemId == Guid.Empty)
			{
				throw new ArgumentException("postCategoryrootItemId is empty");
			}

			var postCategoryrootItemId = siteSettingsItem.PostCategoryRootItemId;
			return Sitecore.Context.Database.GetItem(new ID(postCategoryrootItemId));
		}

		public static ISiteSetting GetSiteSettingItem(ISitecoreService sitecoreService)
		{
			var homeItemPath = Sitecore.Context.Site.ContentStartPath;
			var siteSettingTemplateName = SitecoreHardcode.SiteSettingTemplateName;
			if (siteSettingTemplateName.IsNullOrEmpty())
			{
				throw new NullReferenceException();
			}

			var siteSettingsItem = Sitecore.Context.Database.SelectSingleItem($"fast:{homeItemPath}/*[@@templatekey='{siteSettingTemplateName}']");
			return sitecoreService.GetItem<ISiteSetting>(siteSettingsItem.Paths.Path);
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

		public static INotFound GetNotFoundPageTdsItem(ISitecoreService sitecoreService)
		{
			var notFoundPageItem = GetNotFoundPageItem();
			if(notFoundPageItem == null)
				throw new NullReferenceException("notFoundPageItem is null");

			return sitecoreService.GetItem<INotFound>(notFoundPageItem.ID.Guid);
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

		public static Item GetGlobalEmailSettingsItem()
		{
			var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
			var emailSettingsTemplateName = SitecoreHardcode.EmailSettingsTemplateName;
			var localStorageTemplateName = SitecoreHardcode.LocalStorageTemplateName;
			var query = $"/sitecore/content/*[@@templatekey='{localStorageTemplateName}']//*[@@templatekey='{emailSettingsTemplateName}']";
			var result = masterDb.SelectSingleItem(query);
			return result;
		}

		public static Item[] GetAllSitesRootItems()
		{
			var siteTemplateName = SitecoreHardcode.SiteTemplateName;
			var result = Sitecore.Context.Database.SelectItems($"fast:/sitecore/content/*[@@templatekey='{siteTemplateName}']");
			return result;
		}
	}
}