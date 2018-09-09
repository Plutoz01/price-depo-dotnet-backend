using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using PriceDepo.Models;
using PriceDepo.Filtering;
using System.Text.RegularExpressions;

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

		protected FilterDefinition<TEntity> GetMultipleIdsFilter(IEnumerable<TIdentifier> ids)
		{
			return Builders<TEntity>.Filter.In(nameof(IIdentifiable<TIdentifier>.Id), ids);
		}

		protected IEnumerable<TEntity> FindWithCursor(FilterDefinition<TEntity> filter, int? limit = null, int? offset = null)
		{
			var cursor = _collection.Find(filter)
				.Skip(offset)
				.Limit(limit)
				.ToCursor();
			return IterateCursor(cursor);
		}

		public long Count()
		{
			return _collection.CountDocuments(EmptyFilter);
		}

		public IEnumerable<TEntity> GetAll(int? limit = null, int? offset = null)
		{
			return FindWithCursor(EmptyFilter, limit, offset);
		}

		public TEntity GetById(TIdentifier id)
		{
			return _collection.Find(GetIdFilter(id)).FirstOrDefault();
		}

		public bool IsExists(TIdentifier id)
		{
			return _collection.CountDocuments(GetIdFilter(id), new CountOptions() { Limit = 1 }) > 0;
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
			_collection.DeleteMany(GetMultipleIdsFilter(removableIds));
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

		// TODO: do not use DTO in repository, instead process it in Controller layer
		public IEnumerable<TEntity> Filter(IEnumerable<FilterDTO> filters)
		{
			var builder = Builders<TEntity>.Filter;

			var mongoFilters = filters.Select(dto =>
			{
				switch (dto.Operation)
				{
					case FilterOperation.Equals:
						return builder.Eq(dto.Field, dto.Value);

					case FilterOperation.Contains:
						return builder.Regex(dto.Field, new BsonRegularExpression($"/{ Regex.Escape(dto.Value) }/i"));

					case FilterOperation.StartsWith:
						return builder.Regex(dto.Field, new BsonRegularExpression($"/^{ Regex.Escape(dto.Value) }/i"));

					case FilterOperation.CollectionContainsAll:
						{
							// TODO: move json parsing logic to controller layer
							var jsonArray = dto.Value as Newtonsoft.Json.Linq.JArray;
							string[] filterValues = jsonArray == null || jsonArray.HasValues ? jsonArray.Values<string>().ToArray() : Array.Empty<string>();
							return builder.All(dto.Field, filterValues);
						}

					default:
						throw new ArgumentOutOfRangeException($"Unhandled operation: {dto.Operation}");
				}
			}).Aggregate((mongoFilter1, mongoFilter2) => mongoFilter1 & mongoFilter2);

			return FindWithCursor(mongoFilters);
		}
	}
}