using System;
namespace RoshedTehran.ViewModels
{
	public class IndexViewModel
	{
		public string? SearchQuery { get; set; }
		public IEnumerable<ResultObject>? Results { get; set; }

	}
}

