namespace InfrastructureModule.Models.Components.Navigation
{
	public class NavigationItem 
	{
		public string Title { get; set; }
		public string Url { get; set; }

		public override bool Equals(object obj)
		{
			if (!ReferenceEquals(obj,null) && obj.GetType() == typeof(NavigationItem))
			{
				var item = obj as NavigationItem;

				if (item.Title.Equals(this.Title) && (item.Url.Equals(this.Url)))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}
}