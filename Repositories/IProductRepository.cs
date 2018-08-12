using System.Collections.Generic;
using PriceDepo.Models;

namespace PriceDepo.Repositories
{
	public interface IProductRepository : ICrudRepository<Product, string>
	{
		IEnumerable<Product> GetByManufacturerId(string manufacturerId);
	}
}