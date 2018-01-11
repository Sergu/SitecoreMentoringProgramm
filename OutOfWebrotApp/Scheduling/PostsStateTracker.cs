using System;
using System.Collections.Generic;
using System.Net.Mail;
using Glass.Mapper.Sc;
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
		//private readonly ISearchService _searchService;
		//private readonly IEmailService _emailService;

		//public PostsStateTracker(ISearchService searchService, IEmailService emailService)
		//{
		//	_searchService = searchService;
		//	_emailService = emailService;
		//}

		public void FreshPostsDispatchingByEmail(Item[] items, CommandItem command, ScheduleItem schedule)
		{
			using (new DatabaseSwitcher(Sitecore.Configuration.Factory.GetDatabase("master")))
			{
				var db = Sitecore.Context.Database;
				var sitecoreService = new SitecoreService(db);
				var _emailService = new EmailService();
				var _searchService = new SearchService(new SearchEngineService());

				//Log.Info($"PostStateTracker SendNewPosts command:{command.Name}, {DateTime.Now}", this);

				var savedContextSite = Context.Site;
				var sitesRootItems = SitecoreHelper.GetAllSitesRootItems();

				foreach (var rootSiteItem in sitesRootItems)
				{
					var temp = rootSiteItem.GetSiteInfo();
					var newContextSite = new Sitecore.Sites.SiteContext(temp);
					if (newContextSite.HostName != savedContextSite.HostName)
					{
						Context.Site = newContextSite;
					}
					var currentSiteSettingItem = SitecoreHelper.GetSiteSettingItem(sitecoreService);
					var currentSiteEmailSetting = SitecoreHelper.GetEmailSettingsItem();
					var hourTimeInterval = int.Parse(currentSiteEmailSetting.Fields["HourTimeInterval"].Value);
					var fromDate = DateTime.Now.AddHours(-hourTimeInterval);
					var indexName = currentSiteSettingItem.PostIndex;

					var result = _searchService.GetFreshPostsSinceDate(indexName, fromDate);

					_emailService.SendFreshPostsByEmail(currentSiteEmailSetting, result);
					Context.Site = savedContextSite;
				}
			}
		}
	}
}