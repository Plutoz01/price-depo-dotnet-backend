using Microsoft.AspNetCore.Mvc;
using PriceDepo.Models;
using PriceDepo.Repositories;

namespace PriceDepo.Controllers
{
	[Route("api/[controller]")]
	public class ProductController : AbstractCrudController<Product, string>
	{
		public ProductController(IProductRepository productRepository) : base(productRepository)
		{
		}
	}
}