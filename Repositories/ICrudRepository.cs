using System;
using System.Collections.Generic;
using PriceDepo.Models;

namespace PriceDepo.Repositories
{

	public interface ICrudRepository<TEntity, TIdentifier>
		where TEntity : IIdentifiable<TIdentifier>
	{
		IEnumerable<TEntity> GetAll(int? limit = null, int? offset = null);
		TEntity GetById(TIdentifier id);
		TEntity Save(TEntity entityToSave);
		IEnumerable<TEntity> Save(IEnumerable<TEntity> entitiesToSave);
		void Remove(TIdentifier id);
		void Remove(TEntity removable);
		void RemoveAll(IEnumerable<TIdentifier> removableIds);
		void RemoveAll(IEnumerable<TEntity> removables);
		long Count();
		bool IsExists(TIdentifier id);
	}
}
