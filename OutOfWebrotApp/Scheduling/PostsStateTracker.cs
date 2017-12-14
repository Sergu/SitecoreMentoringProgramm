using System;
using System.Collections.Generic;
using System.Net.Mail;
using InfrastructureModule.Helpers;
using InfrastructureModule.Models.Emailing;
using InfrastructureModule.Models.Pages.Post;
using InfrastructureModule.Services.Implementations.Email;
using InfrastructureModule.Services.Implementations.Search;
using InfrastructureModule.Services.Interfaces.Email;
using InfrastructureModule.Services.Interfaces.Search;
using OutOfWebrotApp.Extensions;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Tasks;

namespace OutOfWebrotApp.Scheduling
{
	public class PostsStateTracker
	{
		private readonly ISearchService _searchService;

		public PostsStateTracker(ISearchService searchService)
		{
			_searchService = searchService;
		}

		public void FreshPostsDispatchingByEmail(Item[] items, CommandItem command, ScheduleItem schedule)
		{
			Log.Info($"PostStateTracker SendNewPosts command:{command.Name}, {DateTime.Now}", this);

			var c = Sitecore.Context.Item;
		}

		public void FreshPostsDispatchingByEmail()
		{
			Log.Info($"Static PostStateTracker SendNewPosts, {DateTime.Now}", this);

			var fromDate = DateTime.Now.AddMonths(-6);
			var savedContextSite = Context.Site;
			var sitesRootItems = SitecoreHelper.GetAllSitesRootItems();
			IEmailService emailService = new EmailService();

			foreach (var rootSiteItem in sitesRootItems)
			{
				var temp = rootSiteItem.GetSiteInfo();
				var newContextSite = new Sitecore.Sites.SiteContext(temp);
				Context.Site = newContextSite;
				var currentSiteSettingItem = SitecoreHelper.GetSiteSettingItem();
				var currentSiteEmailSetting = SitecoreHelper.GetEmailSettingsItem();
				var indexName = currentSiteSettingItem.Fields["PostIndex"].Value;

				var result = _searchService.GetFreshPostsSinceDate(indexName, fromDate);

				emailService.SendFreshPostsByEmail(currentSiteEmailSetting, result);
			}

			Context.Site = savedContextSite;
		}
	}
}