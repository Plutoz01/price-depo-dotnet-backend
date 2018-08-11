namespace PriceDepo.Models
{
	public interface IIdentifiable<TIdentifier>
	{
		TIdentifier Id { get; }
	}
}