using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using PriceDepo.Models;

namespace PriceDepo.Repositories.Mongo
{
	public class MongoProductRepository : AbstractMongoCrudRepository<Product, string>, IProductRepository
	{
		public const string TABLE_NAME = "products";

		public MongoProductRepository(IMongoDatabase database) : base(database)
		{
		}

		protected override string GetTableName()
		{
			return TABLE_NAME;
		}

		public IEnumerable<Product> GetByManufacturerId(string manufacturerId) {
			var filter = Builders<Product>.Filter.Eq(nameof(Product.ManufacturerId), manufacturerId);
			return FindWithCursor(filter, null, null);
		}
	}
}