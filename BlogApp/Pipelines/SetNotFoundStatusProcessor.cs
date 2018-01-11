using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Pipelines.Response.GetPageItem;
using Sitecore.Pipelines.HttpRequest;

namespace BlogApp.Pipelines
{
	public class SetNotFoundStatusProcessor : HttpRequestProcessor
	{
		public override void Process(HttpRequestArgs args)
		{
			//if (Context.Items["Is404Page"] != null && Context.Items["Is404Page"].ToString() == "true")
			//{
			//	HttpContext.Current.Response.StatusCode = 404;
			//	HttpContext.Current.Response.TrySkipIisCustomErrors = true;
			//}
		}
	}
}