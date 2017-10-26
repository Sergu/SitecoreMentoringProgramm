using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;

namespace OutOfWebrotApp.Services.Interfaces.Publishing
{
	public interface IPublishingService
	{
		void PublishItemToWebDatabase(Item item, bool deepPusblishing);
	}
}
