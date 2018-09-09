using System;

namespace PriceDepo.Filtering
{
	public interface IFilterDefinition<T>
	{
		Func<T, bool> ToPredicate();
	}
}
