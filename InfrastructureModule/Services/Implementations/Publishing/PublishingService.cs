using System;
using InfrastructureModule.Services.Interfaces.Publishing;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Publishing;

namespace InfrastructureModule.Services.Implementations.Publishing
{
	public class PublishingService : IPublishingService
	{
		public void PublishItemToWebDatabase(Item item, bool deepPusblishing)
		{
			Assert.IsNotNull(item,"item != null");

			PublishOptions publishOptions = new PublishOptions(item.Database, 
																Database.GetDatabase("web"),
																PublishMode.SingleItem, 
																item.Language, 
																DateTime.Now);
			Publisher publisher = new Publisher(publishOptions);
			publisher.Options.RootItem = item;
			publisher.Options.Deep = deepPusblishing;
			publisher.Publish();
		}
	}
}