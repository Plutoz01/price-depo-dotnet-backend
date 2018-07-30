using System;

namespace PriceDepo.Models
{
	public class Product : IIdentifiable<string>
	{
		public string id { get; set; }
		public string name { get; set; }
		public string barcode { get; set; }
		public int manufacturerId { get; set; }

		public bool Equals(Product other)
		{
			return id == other.id
				&& name == other.name
				&& barcode == other.barcode
				&& manufacturerId == other.manufacturerId;
		}
	}
}
