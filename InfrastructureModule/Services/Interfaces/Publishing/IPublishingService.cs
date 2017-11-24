using Sitecore.Data.Items;

namespace InfrastructureModule.Services.Interfaces.Publishing
{
	public interface IPublishingService
	{
		void PublishItemToWebDatabase(Item item, bool deepPusblishing);
	}
}
