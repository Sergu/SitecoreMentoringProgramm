using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfrastructureModule.Models.Pages.Post;

namespace InfrastructureModule.Models.Emailing
{
	public class FreshPostsResult
	{
		public FreshPostsResult()
		{
			SearchResult = new List<PostItemModel>();
		}

		public IList<PostItemModel> SearchResult { get; set; }
		public bool IsSuccesful { get; set; }
		public string NoResultMessage { get; set; }
	}
}
