using System.Collections.Generic;
using PriceDepo.Models;
using PriceDepo.Filtering;

namespace PriceDepo.Repositories
{
	public interface IProductRepository : ICrudRepository<Product, string>
	{
		IEnumerable<Product> GetByManufacturerId(string manufacturerId);

		IEnumerable<Product> Filter(IEnumerable<FilterDTO> filters);

	}
}