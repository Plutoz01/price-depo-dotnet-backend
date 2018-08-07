using System;
using System.Collections.Generic;
using System.Linq;
using PriceDepo.Models;

namespace PriceDepo.Repositories.InMemoryCrud
{

	public abstract class AbstractInMemoryCrudRepository<TEntity, TIdentifier> : ICrudRepository<TEntity, TIdentifier> where TEntity : IIdentifiable<TIdentifier>
	{

		private readonly IDictionary<TIdentifier, TEntity> entities = new Dictionary<TIdentifier, TEntity>();

		public long Count()
		{
			return entities.LongCount();
		}

		public TEntity GetById(TIdentifier id)
		{
			return entities[id];
		}

		public IEnumerable<TEntity> GetAll(int? limit, int? offset)
		{
			return entities.Values
				.Skip(offset ?? 0)
				.Take(limit ?? 100);
		}

		public void Remove(TIdentifier id)
		{
			entities.Remove(id);
		}

		public void Remove(TEntity removable)
		{
			if (removable != null)
			{
				Remove(removable.Id);
			}
		}

		public void RemoveAll(IEnumerable<TIdentifier> removableIds)
		{
			foreach (var id in removableIds)
			{
				Remove(id);
			}
		}

		public void RemoveAll(IEnumerable<TEntity> removables)
		{
			RemoveAll(removables.Select(entity => entity.Id));
		}

		public TEntity Save(TEntity entityToSave)
		{
			entities[entityToSave.Id] = entityToSave;
			return entityToSave;
		}

		public IEnumerable<TEntity> Save(IEnumerable<TEntity> entitiesToSave)
		{
			return entitiesToSave.Select(Save).ToList();
		}

		public bool IsExists(TIdentifier id)
		{
			return entities.ContainsKey(id);
		}
	}
}