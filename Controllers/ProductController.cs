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
			var filters = filterDTOs
				.Select(ConvertFilterDtoToFilterDefinition)
				.Select((IFilterDefinition<Product> filterDef) => filterDef.ToPredicate())
				.ToArray();

			Func<Product, bool> mergedFilters = (Product product) => filters.All(filterFunc => filterFunc(product));

			return _productRepository.GetAll()
				.Where(mergedFilters)
				.ToArray()
			;
		}

		static IFilterDefinition<Product> ConvertFilterDtoToFilterDefinition(FilterDTO dto)
		{
			switch (dto.Field)
			{
				case "name":
					return dto.Operation.ToStringFilterDefinition<Product>(dto.Value as string, product => product.Name);
				case "barcode":
					return dto.Operation.ToStringFilterDefinition<Product>(dto.Value as string, product => product.Barcode);
				case "categories": {
					var jsonArray = dto.Value as Newtonsoft.Json.Linq.JArray;
					string[] filterValues = jsonArray == null || jsonArray.HasValues ? jsonArray.Values<string>().ToArray() : Array.Empty<string>();
					return dto.Operation.ToCollectionDefinition<Product, string>(filterValues, product => product.Categories);
				}
					
				default:
					throw new ArgumentOutOfRangeException($"Unhandled field: {dto.Field}");
			}
		}
	}





	// public interface IFieldInfo<TEntity, TValue>
	// {
	// 	ISet<FilterOperation> AllowedOperations { get; }
	// 	string FieldName { get; }

	// 	Func<TEntity, TValue> ValueSelector();
	// }

	// public class ProductNameFieldInfo : IFieldInfo<Product, string>
	// {
	// 	private static readonly ISet<FilterOperation> _allowedOperations = new HashSet<FilterOperation>(){
	// 		FilterOperation.Equals,
	// 		FilterOperation.Contains,
	// 		FilterOperation.BeginsWith
	// 	};

	// 	public ISet<FilterOperation> AllowedOperations => _allowedOperations;

	// 	public string FieldName => nameof(Product.Name);

	// 	public Func<Product, string> ValueSelector()
	// 	{
	// 		return product => product.Name;
	// 	}
	// }

	// public class ProductBarcodeFieldInfo : IFieldInfo<Product, string> {
	// 	private static readonly ISet<FilterOperation> _allowedOperations = new HashSet<FilterOperation>(){
	// 		FilterOperation.Equals,
	// 		FilterOperation.Contains,
	// 		FilterOperation.BeginsWith
	// 	};

	// 	public ISet<FilterOperation> AllowedOperations => _allowedOperations;

	// 	public string FieldName => nameof(Product.Barcode);

	// 	public Func<Product, string> ValueSelector()
	// 	{
	// 		return product => product.Barcode;
	// 	}
	// }

	// public class ProductCategoriesFieldInfo : IFieldInfo<Product, string[]> {
	// 	private static readonly ISet<FilterOperation> _allowedOperations = new HashSet<FilterOperation>(){
	// 		FilterOperation.ArrayContains
	// 	};

	// 	public ISet<FilterOperation> AllowedOperations => _allowedOperations;

	// 	public string FieldName => nameof(Product.Barcode);

	// 	public Func<Product, string[]> ValueSelector()
	// 	{
	// 		return product => product.Categories;
	// 	}
	// }


	// public class ProductFilterDefinition : FilterDefinition<Product> {

	// 	public readonly IFieldInfo<Product,string>[] FilterableFields = { new ProductNameFieldInfo() };
	// 	public Func<Product, bool> ToPredicate()
	// 	{
	// 		throw new NotImplementedException();
	// 	}
	// }



	// public static class PredicateBuilder
	// {
	// 	public static Expression<Func<T, bool>> True<T>() { return f => true; }
	// 	public static Expression<Func<T, bool>> False<T>() { return f => false; }

	// 	public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
	// 														Expression<Func<T, bool>> expr2)
	// 	{
	// 		var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
	// 		return Expression.Lambda<Func<T, bool>>
	// 			  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
	// 	}

	// 	public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
	// 														 Expression<Func<T, bool>> expr2)
	// 	{
	// 		var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
	// 		return Expression.Lambda<Func<T, bool>>
	// 			  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
	// 	}
	// }
}