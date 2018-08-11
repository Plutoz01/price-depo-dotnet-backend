using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using PriceDepo.Models;

namespace PriceDepo.Repositories.Mongo
{
	public class MongoProductCrudRepository : AbstractMongoCrudRepository<Product, string>, IProductRepository
	{
		public const string TABLE_NAME = "products";

		public MongoProductCrudRepository(IMongoDatabase database) : base(database)
		{
		}

		protected override string GetTableName()
		{
			return TABLE_NAME;
		}
	}
}