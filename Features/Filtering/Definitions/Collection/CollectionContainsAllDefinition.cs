using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceDepo.Filtering
{
	public class CollectionPropertyContainsAllDefinition<TEntity, TCollectionItem> : IFilterDefinition<TEntity>
	{

		private readonly ICollection<TCollectionItem> _filterValues;
		private readonly Func<TEntity, ICollection<TCollectionItem>> _propertySelector;


		public CollectionPropertyContainsAllDefinition(ICollection<TCollectionItem> filterValues, Func<TEntity, ICollection<TCollectionItem>> propertySelector)
		{
			_filterValues = filterValues;
			_propertySelector = propertySelector;
		}

		public Func<TEntity, bool> ToPredicate()
		{			
			return (TEntity entity) => {
				var propertyValueSet = new HashSet<TCollectionItem>(_propertySelector(entity));
				return _filterValues.All( filterValue => propertyValueSet.Contains(filterValue));
			};
		}
	}
}