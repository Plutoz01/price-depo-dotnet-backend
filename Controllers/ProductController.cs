using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using PriceDepo.Models;
using PriceDepo.Repositories;
using PriceDepo.Services.Filtering;

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
				.Select((FilterDefinition<Product> filterDef) => filterDef.ToPredicate());

			Func<Product, bool> mergedFilters = (Product product) => filters.All(filterFunc => filterFunc(product));

			return _productRepository.GetAll()
				.Where(mergedFilters)
				.ToArray()
			;
		}

		static FilterDefinition<Product> ConvertFilterDtoToFilterDefinition(FilterDTO dto)
		{
			switch (dto.Field)
			{
				case "name":
					{
						return MapStringOperationToFilterDefinition(dto.Operation, dto.Value, product => product.Name);
					}
				case "barcode":
					{
						return MapStringOperationToFilterDefinition(dto.Operation, dto.Value, product => product.Barcode);
					}
				default:
					throw new ArgumentOutOfRangeException($"Unhandled field: {dto.Field}");
			}
		}

		static FilterDefinition<Product> MapStringOperationToFilterDefinition(FilterOperation operation, string value, Func<Product, string> propertySelector)
		{
			switch (operation)
			{
				case FilterOperation.Contains:
					return new StringPropertyContainsDef<Product>(value, propertySelector);
				case FilterOperation.StartsWith:
					return new StringPropertyStartsWithDef<Product>(value, propertySelector);

				default:
					throw new ArgumentOutOfRangeException($"Unhandled operation: {operation}");
			}

		}

	}

	public class FilterDTO
	{
		public FilterOperation Operation { get; set; }
		public string Field { get; set; }
		public string Value { get; set; }
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