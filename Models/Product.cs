using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PriceDepo.Models
{
	[BsonIgnoreExtraElements]
	public class Product : IIdentifiable<string>
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string Name { get; set; }
		public string[] Categories { get; set; } = new string[0];
		public string Barcode { get; set; }
		public int? ManufacturerId { get; set; }

		public bool Equals(Product other)
		{
			return Id == other.Id
				&& Name == other.Name
				&& Categories == other.Categories
				&& Barcode == other.Barcode
				&& ManufacturerId == other.ManufacturerId;
		}
	}
}
