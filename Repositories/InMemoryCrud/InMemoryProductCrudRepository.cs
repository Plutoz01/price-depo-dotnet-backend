using System;
using System.Collections.Generic;
using PriceDepo.Models;

namespace PriceDepo.Repositories.InMemoryCrud
{

	public class InMemoryProductCrudRepository : AbstractInMemoryCrudRepository<Product, string>, IProductRepository
	{
		public InMemoryProductCrudRepository() : base() {
		}

		private static readonly List<Product> dummyProducts = new List<Product>()
		{
			new Product() { id = "1", name = "Product1", barcode = "111111", manufacturerId = 1 },
			new Product() { id = "2", name = "Product2", barcode = "222222", manufacturerId = 1 },
			new Product() { id = "3", name = "Product3", barcode = "333333", manufacturerId = 1 },
			new Product() { id = "4", name = "Product4", barcode = "444444", manufacturerId = 2 }
		};

		public static InMemoryProductCrudRepository WithDummyData()
		{
			var result = new InMemoryProductCrudRepository();
			result.Save(dummyProducts);
			return result;
		}

	}
}