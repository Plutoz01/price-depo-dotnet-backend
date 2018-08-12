using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using PriceDepo.Models;

namespace PriceDepo.Repositories.Mongo
{
	public class MongoManufacturerRepository : AbstractMongoCrudRepository<Manufacturer, string>, IManufacturerRepository
	{
		public const string TABLE_NAME = "manufacturers";

		public MongoManufacturerRepository(IMongoDatabase database) : base(database)
		{
		}

		protected override string GetTableName()
		{
			return TABLE_NAME;
		}

		
	}
}