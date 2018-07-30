namespace PriceDepo.Models
{
	public interface IIdentifiable<TIdentifier>
	{
		TIdentifier id { get; }
	}
}