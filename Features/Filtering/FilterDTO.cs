namespace PriceDepo.Filtering
{
	public class FilterDTO
	{
		public FilterOperation Operation { get; set; }
		public string Field { get; set; }
		public dynamic Value { get; set; }
	}
}