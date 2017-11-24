using System;
using System.Collections.Generic;
using InfrastructureModule.Models.Components.Navigation;
using InfrastructureModule.Services.Interfaces.Navigation;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Extensions;

namespace InfrastructureModule.Services.Implementations.Navigation
{
	public class NavigationService : INavigationService
	{
		public NavigationModel GetNavigationModel(Item contextItem)
		{

			if (ReferenceEquals(contextItem, null))
			{
				throw new NullReferenceException();
			}

			var shouldBeProcessedItems = new List<Item>();
			var renderedNavigationItems = new List<NavigationItem>();

			shouldBeProcessedItems.Add(contextItem);
			contextItem.Children.Each(item => shouldBeProcessedItems.Add(item));

			foreach (var item in shouldBeProcessedItems)
			{
				CheckboxField showInNavigationField = item.Fields["ShowInNavigation"];
				var itemUrl = LinkManager.GetItemUrl(item);
				var itemTitle = item.Name;

				if (item.Fields["Title"] != null)
				{
					itemTitle = item.Fields["Title"].Value.IsEmptyOrNull() ? item.Name : item.Fields["Title"].Value;
				}

				if ((showInNavigationField != null) && showInNavigationField.Checked)
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