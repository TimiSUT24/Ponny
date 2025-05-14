using System;
using HairdresserClassLibrary.Models;

namespace HairdresserClassLibrary.Models
{
	public class Booking
	{
		public int Id { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }

		// Use string for foreign keys to match ApplicationUser's primary key type
		public string CustomerId { get; set; } = null!;
		public ApplicationUser Customer { get; set; } = null!;

		public string HairdresserId { get; set; } = null!;
		public ApplicationUser Hairdresser { get; set; } = null!;

		public int TreatmentId { get; set; }
		public Treatment Treatment { get; set; } = null!;
	}
}
