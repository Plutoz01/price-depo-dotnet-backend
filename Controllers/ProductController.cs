using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using PriceDepo.Models;
using PriceDepo.Repositories;
using PriceDepo.Filtering;

namespace PriceDepo.Controllers
{
	[Route("api/[controller]")]
	public class ProductController : AbstractCrudController<Product, string>
	{

		private readonly IProductRepository _productRepository;
		public ProductController(IProductRepository productRepository) : base(productRepository)
		{
			_productRepository = productRepository;
		}

		[HttpPost("filter")]

		public ActionResult<Product[]> Filter([FromBody] FilterDTO[] filterDTOs)
		{
			return _productRepository.Filter(filterDTOs).ToArray();
		}
	}
}
