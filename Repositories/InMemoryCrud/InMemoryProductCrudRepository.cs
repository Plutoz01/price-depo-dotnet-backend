// using System;
// using System.Collections.Generic;
// using System.Linq;
// using PriceDepo.Models;

// namespace PriceDepo.Repositories.InMemoryCrud
// {

// 	public class InMemoryProductCrudRepository : AbstractInMemoryCrudRepository<Product, string>, IProductRepository
// 	{
// 		public InMemoryProductCrudRepository() : base() {
// 		}

// 		private static readonly List<Product> dummyProducts = new List<Product>()
// 		{
// 			new Product() { Id = "1", Name = "Product1", Barcode = "111111", ManufacturerId = "1" },
// 			new Product() { Id = "2", Name = "Product2", Barcode = "222222", ManufacturerId = "1" },
// 			new Product() { Id = "3", Name = "Product3", Barcode = "333333", ManufacturerId = "1" },
// 			new Product() { Id = "4", Name = "Product4", Barcode = "444444", ManufacturerId = "2" }
// 		};

// 		public static InMemoryProductCrudRepository WithDummyData()
// 		{
// 			var result = new InMemoryProductCrudRepository();
// 			result.Save(dummyProducts);
// 			return result;
// 		}

// 		IEnumerable<Product> IProductRepository.GetByManufacturerId(string manufacturerId)
// 		{
// 			return dummyProducts
// 			.Where( p => p.ManufacturerId == manufacturerId );
// 		}
// 	}
// }