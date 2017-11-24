using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.Links;
using InfrastructureModule.Services.Implementations.Navigation;
using InfrastructureModule.Models.Components.Navigation;

namespace OutOfWebRootAppTests.Services
{
	public class NavigationServiceTests
	{
		[TestCase("/sitecore")]
		[TestCase("/sitecore/content/")]
		[TestCase("/sitecore/system")]
		[TestCase("/sitecore/templates")]
		public void IsFakeDbExist(string path)
		{
			using (var db = new Db())
			{
				var item = db.GetItem(path);
				Assert.IsNotNull(item);
			}
		}

		[Test]
		public void ThrowException_IfContextItemIsNull()
		{
			var service = new NavigationService();

			Item contextItem = null;
			Assert.Throws<NullReferenceException>(() => service.GetNavigationModel(contextItem));
		}

		[Test]
		public void ReturnItemsThatMarkedAs_ShowInNavigation()
		{
			var service = new NavigationService();

			var homeItemId = ID.NewID;
			var item1Id = ID.NewID;
			var item1Subitem = ID.NewID;
			var item2Id = ID.NewID;
			var item3Id = ID.NewID;
			var templateId = ID.NewID;

			using (var db = new Db
			{
				new DbTemplate("baseTemplate", templateId)
				{
					{ new DbField("ShowInNavigation") {Type = "checkbox"}, "1" }
				},

				new DbItem("home", homeItemId, templateId) // marked
				{
					new DbItem("item1", item1Id, templateId) // marked
					{
						new DbItem("item1SubItem", item1Subitem, templateId) // marked but shouldn't be shown, cause of the dep
					},
					new DbItem("item2", item2Id),
					new DbItem("item3", item3Id, templateId), // marked
				},
				
			})
			{
				var contextItem = db.GetItem(homeItemId);
				var result = service.GetNavigationModel(contextItem);

				var expectedResult = new NavigationModel()
				{
					NavigationItems = new List<NavigationItem>()
					{
						new NavigationItem() {Title = "home", Url = LinkManager.GetItemUrl(db.GetItem(homeItemId))},
						new NavigationItem() {Title = "item1", Url = LinkManager.GetItemUrl(db.GetItem(item1Id))},
						new NavigationItem() {Title = "item3", Url = LinkManager.GetItemUrl(db.GetItem(item3Id))}
					}
				};

				Assert.AreEqual(result.NavigationItems.Count(), expectedResult.NavigationItems.Count());
				CollectionAssert.AreEqual(result.NavigationItems, expectedResult.NavigationItems);
			}
		}

		[Test]
		public void ReturnName_IfItemHaveNoTitleField()
		{
			var service = new NavigationService();

			var homeItemId = ID.NewID;
			var item1Id = ID.NewID;
			var item1Subitem = ID.NewID;
			var item2Id = ID.NewID;
			var item3Id = ID.NewID;
			var showInNavigationTemplateId = ID.NewID;
			var titleTemplateId = ID.NewID;
			var mixedTemplateId = ID.NewID;

			using (var db = new Db
			{
				new DbTemplate("ShowInNavigationTemplate", showInNavigationTemplateId)
				{
					{ new DbField("ShowInNavigation") {Type = "checkbox"}, "1" }
				},
				new DbTemplate("TitleTemplate", titleTemplateId)
				{
					{ new DbField("Title"), "DefaultTitle" }
				},

				new DbTemplate("MixedBaseTemplate", mixedTemplateId)
				{
					BaseIDs = new [] { showInNavigationTemplateId, titleTemplateId}
				},

				new DbItem("home", homeItemId, showInNavigationTemplateId)					// Name
				{
					new DbItem("item1", item1Id, showInNavigationTemplateId)	// Name
					{
						new DbItem("item1SubItem", item1Subitem, showInNavigationTemplateId) // marked but shouldn't be shown, cause of the dep
					},
					new DbItem("item2", item2Id, showInNavigationTemplateId),				// Name
					new DbItem("item3", item3Id, mixedTemplateId)	// Title field
				},
			})
			{
				var contextItem = db.GetItem(homeItemId);
				var result = service.GetNavigationModel(contextItem);

				var expectedResult = new NavigationModel()
				{
					NavigationItems = new List<NavigationItem>()
					{
						new NavigationItem() {Title = "home", Url = LinkManager.GetItemUrl(db.GetItem(homeItemId))},
						new NavigationItem() {Title = "item1", Url = LinkManager.GetItemUrl(db.GetItem(item1Id))},
						new NavigationItem() {Title = "item2", Url = LinkManager.GetItemUrl(db.GetItem(item2Id))},
						new NavigationItem() {Title = "DefaultTitle", Url = LinkManager.GetItemUrl(db.GetItem(item3Id))}
					}
				};

				Assert.AreEqual(result.NavigationItems.Count(), expectedResult.NavigationItems.Count());
				CollectionAssert.AreEqual(result.NavigationItems, expectedResult.NavigationItems);
			}
		}

	}
}
