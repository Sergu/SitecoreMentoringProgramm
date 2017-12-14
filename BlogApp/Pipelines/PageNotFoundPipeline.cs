using System.Net;
using System.Web;
using InfrastructureModule.Helpers;
using Sitecore;
using Sitecore.Pipelines.HttpRequest;

namespace BlogApp.Pipelines
{
	public class PageNotFoundPipeline : HttpRequestProcessor
	{
		public override void Process(HttpRequestArgs args)
		{
			var filepath = HttpContext.Current.Server.MapPath(args.Url.FilePath);

			if (IsValidItem() || args.LocalPath.StartsWith("/sitecore") || System.IO.File.Exists(filepath))
			{
				return;
			}

			Context.Item = SitecoreHelper.GetNotFoundPageItem();
			if (Context.Item != null)
			{
				Context.Items["Is404Page"] = "true";
			}
		}

		private bool IsValidItem()
		{
			if (Context.Item == null || Context.Item.Versions.Count == 0 || Context.Item.Visualization.Layout == null)
			{
				return false;
			}
			return true;
		}
	}
}