using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using InfrastructureModule.Models.Emailing;
using Sitecore.Data.Items;

namespace InfrastructureModule.Services.Interfaces.Email
{
	public interface IEmailService
	{
		void SendFreshPostsByEmail(Item emailSettings, FreshPostsResult freshPosts);
		void SendEmail(string emailFrom, string emailTo, string fromPassword, string subject, string body);
	}
}
