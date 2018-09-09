using System;

namespace PriceDepo.Filtering {
	public class StringPropertyContainsDefinition<T> : IFilterDefinition<T>
	{
		private readonly string _filterValue;
		private readonly Func<T,string> _propertySelector;

		public StringPropertyContainsDefinition(string filterValue, Func<T,string> propertySelector) {
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
}