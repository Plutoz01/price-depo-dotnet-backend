using System;
using System.Collections.Generic;

namespace PriceDepo.Filtering
{
	public class CollectionPropertyContainsAnyDefinition<TEntity, TCollectionItem> : IFilterDefinition<TEntity>
	{

		private readonly ISet<TCollectionItem> _filterValues;
		private readonly Func<TEntity, ICollection<TCollectionItem>> _propertySelector;


		public CollectionPropertyContainsAnyDefinition(ICollection<TCollectionItem> filterValues, Func<TEntity, ICollection<TCollectionItem>> propertySelector)
		{
			_filterValues = new HashSet<TCollectionItem>(filterValues);
			_propertySelector = propertySelector;
		}

		public Func<TEntity, bool> ToPredicate()
		{
			return (TEntity entity) => _filterValues.Overlaps(_propertySelector(entity));
		}
	}
}