using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PriceDepo.Models;
using PriceDepo.Repositories;

namespace PriceDepo.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductRepository _productRepository;

		public ProductController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		[HttpGet]
		public IEnumerable<Product> GetAll([FromQuery] PaginationParameters pagination)
		{
			return _productRepository.GetAll(pagination.Limit, pagination.Offset);
		}

		[HttpGet("{id}")]
		public Product Get(string id)
		{
			return _productRepository.GetById(id);
		}

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public ActionResult<Product> CreateOne([FromBody] Product newProduct)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return _productRepository.Save(newProduct);
		}

		[HttpPost("batch")]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public ActionResult<Product[]> CreateMany([FromBody] Product[] newProducts)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return _productRepository.Save(newProducts).ToArray();
		}
	}
}