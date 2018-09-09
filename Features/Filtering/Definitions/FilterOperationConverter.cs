using System;
using System.Collections.Generic;

namespace PriceDepo.Filtering
{
	public static class FilterOperationConverter
	{
		public static IFilterDefinition<T> ToStringFilterDefinition<T>(this FilterOperation operation, string filterValue, Func<T, string> propertySelector)
		{
			switch (operation)
			{
				case FilterOperation.Contains:
					return new StringPropertyContainsDefinition<T>(filterValue, propertySelector);
				case FilterOperation.StartsWith:
					return new StringPropertyStartsWithDefiniton<T>(filterValue, propertySelector);
				default:
					throw OnUnhandledOperation(operation);
			}
		}

		public static IFilterDefinition<TEntity> ToCollectionDefinition<TEntity, TCollectionItem>(
			this FilterOperation operation,
			ICollection<TCollectionItem> filterValues, Func<TEntity, ICollection<TCollectionItem>> propertySelector
		)
		{
			switch (operation)
			{
				case FilterOperation.CollectionContainsAny:
					return new CollectionPropertyContainsAnyDefinition<TEntity, TCollectionItem>(filterValues, propertySelector);
				case FilterOperation.CollectionContainsAll:
					return new CollectionPropertyContainsAllDefinition<TEntity, TCollectionItem>(filterValues, propertySelector);
				default:
					throw OnUnhandledOperation(operation);
			}
		}

		static Exception OnUnhandledOperation(FilterOperation operation)
		{
			return new ArgumentOutOfRangeException($"Unhandled operation: {operation}");
		}
	}
}