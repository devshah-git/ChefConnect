using System;
namespace ChefConnect.Entities
{
	public class Addresses
	{
		public int AddressesId { get; set; }

		public string Name { get; set; }

		public string? AptNumber { get; set; }

		public string StreetAddress { get; set; }

		public string City { get; set; }

		public string Province { get; set; }

		public string Country { get; set; }

		public string PostalCode { get; set; }

		public string PhoneNumber { get; set; }

		public string CustomerId { get; set; }

        public ICollection<OrderDetails>? OrderDetails { get; set; }

        public AppUser? Customer { get; set; }

    }
}

