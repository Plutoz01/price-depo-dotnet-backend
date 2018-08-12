using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using PriceDepo.Models;
using PriceDepo.Repositories;

namespace PriceDepo.Controllers
{
	[Route("api/[controller]")]
	public class ManufacturerController : AbstractCrudController<Manufacturer, string>
	{
		private readonly IManufacturerRepository _manufacturerRepository;
		private readonly IProductRepository _productRepository;

		public ManufacturerController(IManufacturerRepository manufacturerRepository, IProductRepository productRepository) : base(manufacturerRepository)
		{
			_manufacturerRepository = manufacturerRepository;
			_productRepository = productRepository;
		}

		[HttpGet("{manufacturerId}/product")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public ActionResult<Product[]> GetByGetProductsOfManufacturer(string manufacturerId)
		{
			if( !_manufacturerRepository.IsExists(manufacturerId) ){
				return NotFound();
			}
			return _productRepository.GetByManufacturerId(manufacturerId).ToArray();
		}
	}
}