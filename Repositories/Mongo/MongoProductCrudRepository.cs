using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using PriceDepo.Models;

namespace PriceDepo.Repositories.Mongo
{
	public class MongoProductCrudRepository : IProductRepository
	{
		public const string TABLE_NAME = "products";

		private readonly IMongoCollection<Product> _collection;

		public MongoProductCrudRepository(IMongoDatabase database)
		{
			_collection = database.GetCollection<Product>(TABLE_NAME);
		}

		private FilterDefinition<Product> EmptyFilter
		{
			get
			{
				return Builders<Product>.Filter.Empty;
			}
		}

		private IEnumerable<Product> IterateCursor(IAsyncCursor<Product> cursor)
		{
			foreach (var product in cursor.ToEnumerable())
			{
				yield return product;
			}
		}

		public long Count()
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<Product> GetAll()
		{
			var cursor = _collection.Find(EmptyFilter).ToCursor();
			return IterateCursor(cursor);
		}

		public Product GetById(string id)
		{
			var filter = Builders<Product>.Filter.Eq( nameof(Product.id), id);
			return _collection.Find(filter).FirstOrDefault();
		}

		public bool IsExists(string id)
		{
			throw new System.NotImplementedException();
		}

		public void Remove(string id)
		{
			throw new System.NotImplementedException();
		}

		public void Remove(Product removable)
		{
			throw new System.NotImplementedException();
		}

		public void RemoveAll(IEnumerable<string> removableIds)
		{
			throw new System.NotImplementedException();
		}

		public void RemoveAll(IEnumerable<Product> removables)
		{
			throw new System.NotImplementedException();
		}

		public Product Save(Product entityToSave)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<Product> Save(IEnumerable<Product> entitiesToSave)
		{
			throw new System.NotImplementedException();
		}
	}
}