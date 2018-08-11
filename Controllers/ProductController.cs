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
		public ActionResult<Product[]> GetAll([FromQuery] PaginationParameters pagination)
		{
			return _productRepository.GetAll(pagination.Limit, pagination.Offset).ToArray();
		}

		[HttpGet("{id}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public ActionResult<Product> Get(string id)
		{
			var result = _productRepository.GetById(id);
			if (result == null)
			{
				return NotFound();
			}
			return result;
		}

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public ActionResult<Product> CreateOne([FromBody] Product newProduct)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (newProduct.Id != null)
			{
				return BadRequest("New entity must not have Id");
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

		[HttpPut("{id}")]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public ActionResult<Product> UpdateOne(string id, [FromBody] Product product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (id != product.Id)
			{
				return BadRequest($"mismatching identifiers (path: {id}, body: {product?.Id})");
			}
			if (!_productRepository.IsExists(id))
			{
				return NotFound();
			}
			return _productRepository.Save(product);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public IActionResult Delete(string id)
		{
			if (!_productRepository.IsExists(id))
			{
				return NotFound();
			}
			_productRepository.Remove(id);
			return NoContent();
		}
	}
}