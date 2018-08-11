using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using PriceDepo.Models;

namespace PriceDepo.Repositories.Mongo
{
	public abstract class AbstractMongoCrudRepository<TEntity, TIdentifier> : ICrudRepository<TEntity, TIdentifier> where TEntity : IIdentifiable<TIdentifier>
	{
		protected abstract string GetTableName();

		protected readonly IMongoCollection<TEntity> _collection;

		public AbstractMongoCrudRepository(IMongoDatabase database)
		{
			_collection = database.GetCollection<TEntity>(GetTableName());
		}

		protected FilterDefinition<TEntity> EmptyFilter
		{
			get
			{
				return Builders<TEntity>.Filter.Empty;
			}
		}

		protected IEnumerable<TEntity> IterateCursor(IAsyncCursor<TEntity> cursor)
		{
			foreach (var entity in cursor.ToEnumerable())
			{
				yield return entity;
			}
		}

		protected FilterDefinition<TEntity> GetIdFilter(TIdentifier id)
		{
			return Builders<TEntity>.Filter.Eq(nameof(IIdentifiable<TIdentifier>.Id), id);
		}

		public long Count()
		{
			return _collection.CountDocuments(EmptyFilter);
		}

		public IEnumerable<TEntity> GetAll(int? limit, int? offset)
		{
			var cursor = _collection.Find(EmptyFilter)
				.Skip(offset)
				.Limit(limit)
				.ToCursor();
			return IterateCursor(cursor);
		}

		public TEntity GetById(TIdentifier id)
		{
			return _collection.Find(GetIdFilter(id)).FirstOrDefault();
		}

		public bool IsExists(TIdentifier id)
		{
			return _collection.CountDocuments(GetIdFilter(id), new CountOptions(){ Limit = 1 }) > 0;
		}

		public void Remove(TIdentifier id)
		{
			_collection.DeleteOne(GetIdFilter(id));
		}

		public void Remove(TEntity removable)
		{
			Remove(removable.Id);
		}

		public void RemoveAll(IEnumerable<TIdentifier> removableIds)
		{
			var filters = removableIds
				.Select(id => GetIdFilter(id))
				.Aggregate((acc, item) => acc & item);

			_collection.DeleteMany(filters);
		}

		public void RemoveAll(IEnumerable<TEntity> removables)
		{
			var ids = removables.Select(entity => entity.Id);
			RemoveAll(ids);
		}

		public TEntity Save(TEntity entityToSave)
		{
			if (entityToSave.Id == null)
			{
				_collection.InsertOne(entityToSave);

			}
			else
			{
				_collection.FindOneAndReplace(GetIdFilter(entityToSave.Id), entityToSave);
			}
			return entityToSave;

		}

		public IEnumerable<TEntity> Save(IEnumerable<TEntity> entitiesToSave)
		{
			_collection.InsertMany(entitiesToSave);
			return entitiesToSave;
		}
	}
}