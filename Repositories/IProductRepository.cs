using PriceDepo.Models;

namespace PriceDepo.Repositories
{
	public interface IProductRepository : ICrudRepository<Product, string>
	{
	}
}