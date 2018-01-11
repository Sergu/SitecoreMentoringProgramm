using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using InfrastructureModule.Models.Components.Navigation;
using InfrastructureModule.Services.Interfaces.Navigation;
using InfrastructureModule.TDS.sitecore.templates.Custom.BaseTemplates.Base;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Extensions;
using Sitecore.Sites;

namespace InfrastructureModule.Services.Implementations.Navigation
{
	public class NavigationService : INavigationService
	{
		private readonly ISitecoreService _sitecoreService;
		public NavigationService()
		{
			var sitecoreService = new SitecoreService(Sitecore.Context.Database);

			if (sitecoreService == null)
				throw new NullReferenceException("sitecoreService is null");
			_sitecoreService = sitecoreService;
		}

		public NavigationModel GetNavigationModel(Item contextItem)
		{
			if (ReferenceEquals(contextItem, null))
			{
				throw new NullReferenceException("contextItem is null");
			}

			var shouldBeProcessedItems = new List<Item>();
			var renderedNavigationItems = new List<NavigationItem>();

			shouldBeProcessedItems.Add(contextItem);
			contextItem.Children.Each(item => shouldBeProcessedItems.Add(item));
			
			foreach (var item in shouldBeProcessedItems)
			{
				var baseNavigationItem = _sitecoreService.GetItem<IBaseNavigationItem>(item.ID.Guid);

				if(baseNavigationItem == null)
					continue;

				var basePageItem = _sitecoreService.GetItem<IBasePage>(item.ID.Guid);

				bool shouldShowInNavigationField = baseNavigationItem.ShowInNavigation;
				var itemUrl = LinkManager.GetItemUrl(item);
				var itemTitle = string.Empty;

				if (basePageItem.Title != null)
				{
					itemTitle = basePageItem.Title;
				}

				if (shouldShowInNavigationField)
				{
					var navigationModel = new NavigationItem()
					{
						Title = itemTitle,
						Url = itemUrl
					};

					renderedNavigationItems.Add(navigationModel);
				}
			}

			return new NavigationModel()
			{
				NavigationItems = renderedNavigationItems
			};
		}
	}
}