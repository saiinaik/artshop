using System.Collections.Generic;

namespace art_web_app.Models.ViewModels
{
	public class SubCategoryAndCategoryVM
	{
		public IEnumerable<Category> CategoryList { get; set; }
		public SubCategory SubCategory { get; set; }
		public IEnumerable<string> SubCategoryList { get; set; }
		public string StatusMessage { get; set; }
	}
}
