using System;
using System.ComponentModel.DataAnnotations;

namespace PriceDepo.Models
{
	public class PaginationParameters
	{
		public const int DEFAULT_LIMIT_SIZE = 10;
		public const int MAX_LIMIT_SIZE = 100;

		[Range(1,MAX_LIMIT_SIZE)]
		public int Limit { get; set; } = DEFAULT_LIMIT_SIZE;
		[Range(0, Int32.MaxValue)]
		public int Offset { get; set; } = 0;
	}
}
