using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using InfrastructureModule.Models.Emailing;
using InfrastructureModule.Models.Pages.Post;
using InfrastructureModule.Services.Interfaces.Email;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Web;

namespace InfrastructureModule.Services.Implementations.Email
{
	public class EmailService : IEmailService
	{
		public void SendFreshPostsByEmail(Item emailSettings, FreshPostsResult freshPosts)
		{
			var id = emailSettings.Fields["ReportEmailTemplate"].Value;
			var emailReportItemId = new ID(id);
			var emailReportItem = Sitecore.Context.Database.GetItem(emailReportItemId);
			var emailPostTemlateItemId = new ID(emailSettings.Fields["NewPostEmailTemplate"].Value);
			var emailPostTemplateItem = Sitecore.Context.Database.GetItem(emailPostTemlateItemId);
			var emailToSendReports = emailSettings.Fields["EmailToSendReports"].Value;
			var emailFrom = emailSettings.Fields["EmailFrom"].Value;
			var fromPassword = emailSettings.Fields["FromPassword"].Value;
			var emailReportBodyTemplate = emailReportItem.Fields["Body"].Value;
			var emailReportSubjectTemplate = emailReportItem.Fields["Subject"].Value;

			if (freshPosts.IsSuccesful && freshPosts.SearchResult.Any())
			{
				var emailBody = string.Empty;
				foreach (var post in freshPosts.SearchResult)
				{
					var postBody = CreateEmailPostTemplateString(emailPostTemplateItem, post);
					emailBody += postBody;
				}

				emailReportBodyTemplate = emailReportBodyTemplate.Replace("[content]", emailBody);

			}
			else
			{
				emailReportBodyTemplate = emailReportBodyTemplate.Replace("[content]", "No one post have been added since last mail");
			}

			SendEmail(emailFrom, emailToSendReports, fromPassword, emailReportSubjectTemplate,  emailReportBodyTemplate);
		}

		public void SendCreatedPostByEmail(Item emailSettings, SiteInfo siteInfo,Item postItem)
		{
			var postModel = ConvertPostItemIntoPostModel(postItem, siteInfo);
			var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");

			var emailPostTemlateItemId = new ID(emailSettings.Fields["NewPostEmailTemplate"].Value);
			var emailPostTemplateItem = masterDb.GetItem(emailPostTemlateItemId);
			var emailToSendReports = emailSettings.Fields["EmailToSendReports"].Value;
			var emailFrom = emailSettings.Fields["EmailFrom"].Value;
			var fromPassword = emailSettings.Fields["FromPassword"].Value;

			var emailSubject = "New item have been created";
			var emailbody = CreateEmailPostTemplateString(emailPostTemplateItem, postModel);

			SendEmail(emailFrom, emailToSendReports, fromPassword, emailSubject, emailbody);
		}

		public void SendEmail(string emailFrom, string emailTo, string fromPassword, string subject, string body)
		{
			var fromAddress = new MailAddress(emailFrom, "From Name");
			var toAddress = new MailAddress(emailTo, "To Name");

			var smtp = new SmtpClient
			{
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
			};
			using (var message = new MailMessage(fromAddress, toAddress)
			{
				Subject = subject,
				Body = body,
				IsBodyHtml = true
			})
			{
				smtp.Send(message);
			}
		}

		private string CreateEmailPostTemplateString(Item emailPostTemplateItem, PostItemModel post)
		{
			var authorTemplate = emailPostTemplateItem.Fields["Author"].Value;
			var nameTemplate = emailPostTemplateItem.Fields["Name"].Value;
			var postTitleTemplate = emailPostTemplateItem.Fields["PostTitle"].Value;
			var linkTemplate = emailPostTemplateItem.Fields["Link"].Value;
			var dateTemplate = emailPostTemplateItem.Fields["Date"].Value;

			authorTemplate = authorTemplate.Replace("[content]", post.Author);
			nameTemplate = nameTemplate.Replace("[content]", post.Title);
			postTitleTemplate = postTitleTemplate.Replace("[content]", post.Subtitle);
			linkTemplate = linkTemplate.Replace("[content]", post.Url);
			dateTemplate = dateTemplate.Replace("[content]", post.Date.ToString());

			return String.Concat(authorTemplate, nameTemplate, postTitleTemplate, linkTemplate, dateTemplate);
		}

		private PostItemModel ConvertPostItemIntoPostModel(Item postItem, SiteInfo siteInfo)
		{
			var savedContextSite = Context.Site;
			var newContextSite = new Sitecore.Sites.SiteContext(siteInfo);
			Context.Site = newContextSite;
			var resultItem = postItem;
			MultilistField postTagsField = resultItem.Fields["Tags"];
			LookupField category = resultItem.Fields["Category"];

			//var newContextSite = new Sitecore.Sites.SiteContext();
			var post = new PostItemModel()
			{
				Body = resultItem.Fields["Body"].Value,
				Subtitle = resultItem.Fields["Subtitle"].Value,
				Title = resultItem.Fields["Title"].Value,
				Url = LinkManager.GetItemUrl(resultItem),
				Author = resultItem.Fields["Author"].Value,
				Date = (new DateField(resultItem.Fields["Date"])).DateTime,
				Tags = postTagsField.Count != 0 ? postTagsField.GetItems().Select(i => i.Fields["Value"].Value) : new List<string>(),
				Category = category.TargetItem != null ? category.TargetItem.Fields["Value"].Value : null,
			};

			Context.Site = savedContextSite;

			return post;
		}
	}
}
