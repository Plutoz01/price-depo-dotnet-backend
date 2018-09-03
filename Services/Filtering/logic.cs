using System;
using PriceDepo.Models;

namespace PriceDepo.Services.Filtering
{
	public enum FilterOperation
	{
		Equals,
		Contains,
		ArrayContains,
		StartsWith,
		LargerThan,
		LargerThanOrEqual,
		SmallerThan,
		SmallerThanOrEqual,
		Between
	}

	public interface FilterDefinition<T>
	{
		Func<T, bool> ToPredicate();
	}

	public class StringPropertyContainsDef<T> : FilterDefinition<T>
	{
		private readonly string _filterValue;
		private readonly Func<T,string> _propertySelector;

		public StringPropertyContainsDef(string filterValue, Func<T,string> propertySelector) {
			_filterValue = filterValue.ToLowerInvariant();
			_propertySelector = propertySelector;
		}

		public Func<T, bool> ToPredicate()
		{
			return (T entity) => 
				_propertySelector(entity)
				.ToLowerInvariant()
				.Contains(_filterValue);
		}
	}

	public class StringPropertyStartsWithDef<T> : FilterDefinition<T>
	{
		private readonly string _filterValue;
		private readonly Func<T,string> _propertySelector;

		public StringPropertyStartsWithDef(string filterValue, Func<T,string> propertySelector) {
			_filterValue = filterValue.ToLowerInvariant();
			_propertySelector = propertySelector;
		}

		public Func<T, bool> ToPredicate()
		{
			return (T entity) => 
				_propertySelector(entity)
				.ToLowerInvariant()
				.StartsWith(_filterValue);
		}
	}
}