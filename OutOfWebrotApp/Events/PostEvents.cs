using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfrastructureModule.Helpers;
using InfrastructureModule.Services.Implementations.Email;
using OutOfWebrotApp.Extensions;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.StringExtensions;

namespace OutOfWebrotApp.Events
{
	public class PostEvents
	{
		public void OnItemAdded(object sender, EventArgs args)
		{
			var addedItem = Event.ExtractParameter(args, 0) as Item;
			var db = Sitecore.Configuration.Factory.GetDatabase(addedItem.Database.Name);

			if (addedItem != null && db != null)
			{
				var emailService = new EmailService();

				var getAllPostsItemsQuery = $"/sitecore/content/*[@@templatekey='{SitecoreHardcode.SiteTemplateName}']/Home/*[@@templatekey='{SitecoreHardcode.PostsItemTemplateName}']";
				var postsItems = db.SelectItems(getAllPostsItemsQuery);
				var isAddedItemIsPostItem = postsItems.Where(p => p.ID == addedItem.ParentID).ToList().Any();

				if (addedItem != null && isAddedItemIsPostItem)
				{
					var globalEmailConfig = SitecoreHelper.GetGlobalEmailSettingsItem();
					var siteInfo = addedItem.GetSiteInfo();
					emailService.SendCreatedPostByEmail(globalEmailConfig, siteInfo, addedItem);
				}
			}
		}
	}
}