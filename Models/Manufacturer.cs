using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PriceDepo.Models
{
	[BsonIgnoreExtraElements]
	public class Manufacturer : IIdentifiable<string>
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string Name { get; set; }

		public bool Equals(Manufacturer other)
		{
			return Id == other.Id
				&& Name == other.Name;
		}
	}
}
