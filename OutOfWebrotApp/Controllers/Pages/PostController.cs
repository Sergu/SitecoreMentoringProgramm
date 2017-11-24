using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using InfrastructureModule.Helpers;
using InfrastructureModule.Models.Pages.Post;
using InfrastructureModule.Services.Interfaces.Publishing;

namespace OutOfWebrotApp.Controllers.Pages
{
    public class PostController : Controller
    {
	    private readonly IPublishingService _publishService;

	    public PostController(IPublishingService publishService)
	    {
		    _publishService = publishService;
	    }
        // GET: Post
        public ActionResult Index()
        {
	        var contextItem = RenderingContext.Current.Rendering.Item;

	        if (contextItem == null)
	        {
		        if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
		        {
			        return View("~/Views/Errors/ExperienceEditorError.cshtml");
		        }

		        return View("~/Views/Pages/Post/Post.cshtml", new PostItemModel());
	        }

	        MultilistField postTagsField = contextItem.Fields["Tags"];
	        LookupField category = contextItem.Fields["Category"];

			var post = new PostItemModel()
	        {
		        Body = contextItem.Fields["Body"].Value,
		        Subtitle = contextItem.Fields["Subtitle"].Value,
		        Title = contextItem.Fields["Title"].Value,
		        Url = LinkManager.GetItemUrl(contextItem),
		        Author = contextItem.Fields["Author"].Value,
		        Date = (new DateField(contextItem.Fields["Date"])).DateTime,
				Tags = postTagsField.Count != 0 ? postTagsField.GetItems().Select(i => i.Fields["Value"].Value) : new List<string>(),
				Category = category.TargetItem != null ? category.TargetItem.Fields["Value"].Value : null,
	        };

	        var layoutGuid = RenderingContext.CurrentOrNull.Rendering.LayoutId;
	        var layoutItem = Sitecore.Context.Database.GetItem(new ID(layoutGuid));
	        var layoutName = layoutItem.Name;

	        if (layoutName == "PrintLayout")
	        {
		        return View("~/Views/Pages/Post/PostPrintView.cshtml", post);
			}

	        return View("~/Views/Pages/Post/Post.cshtml", post);
        }

	    public ActionResult Comments()
	    {
			var contextItem = RenderingContext.Current.Rendering.Item;

		    if (contextItem == null)
		    {
			    if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
			    {
				    return View("~/Views/Errors/ExperienceEditorError.cshtml");
			    }

			    return View("~/Views/Pages/Post/Post.cshtml", new PostItemModel());
		    }

			var commentSearchQuerry = $"fast:{contextItem.Paths.FullPath}//*[@@templatekey='comment']";

		    var commentItems = Sitecore.Context.Database.SelectItems(commentSearchQuerry);
		    var comments = new List<Comment>();

		    foreach (var commentItem in commentItems)
		    {
			    DateField date = commentItem.Fields["Date"];
			    var comment = new Comment()
			    {
				    Author = commentItem.Fields["Author"].Value,
				    Date = date.DateTime,
				    Text = commentItem.Fields["Text"].Value
			    };
			    comments.Add(comment);
		    }

			var orderedComments = comments.OrderByDescending(c => c.Date);

		    return View("~/Views/Components/Comments/Comment.cshtml", orderedComments);
	    }

		[HttpGet]
	    public ActionResult GetCommentPostForm()
	    {
			return View("~/Views/Components/Comments/CommentPostForm.cshtml");
		}

	    [HttpPost]
		[ValidateInput(false)]
	    public ActionResult GetCommentPostForm(Comment comment)
	    {
		    Database masterDatabase = Sitecore.Data.Database.GetDatabase("master");
		    var contextItem = RenderingContext.Current.ContextItem;
		    var masterContextItem = masterDatabase.GetItem(contextItem.ID);
		    var templatePath = SitecoreHelper.GetSiteSettingItem().Fields["CommentTemplateId"].Value;
			var templateItem = masterDatabase.GetTemplate(templatePath);

		    using (new Sitecore.SecurityModel.SecurityDisabler())
		    {
				var commentItem = masterContextItem.Add("Comment", new TemplateID(templateItem.ID));
			    if (commentItem != null)
			    {
				    commentItem.Editing.BeginEdit();
				    commentItem["Text"] = comment.Text;
				    commentItem["Author"] = "John Black";
				    commentItem["Date"] = DateUtil.ToIsoDate(DateTime.Now);
				    commentItem.Editing.EndEdit();

					_publishService.PublishItemToWebDatabase(commentItem, false);
			    }
			}

		    return Redirect(LinkManager.GetItemUrl(contextItem));
	    }

	    public ActionResult CheckContextItemOnNull(Item item)
	    {
			if (item == null)
			{
				if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
				{
					return View("~/Views/Errors/ExperienceEditorError.cshtml");
				}

				return View("~/Views/Pages/Post/Post.cshtml", new PostItemModel());
			}

		    return null;
	    }

	}
}