using System;
using System.Collections.Generic;
using System.Linq;
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

		public ProductController(IProductRepository productRepository) {
			_productRepository = productRepository;
		}
		

		[HttpGet]
		public IEnumerable<Product> GetAll()
		{
			return _productRepository.GetAll();
		}

		[HttpGet("{id}")]
        public Product Get(string id)
        {
            return _productRepository.GetById(id);
        }
	}
}