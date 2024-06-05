using System;
using System.ComponentModel.DataAnnotations;
namespace ChefConnect.Entities
{
	public class PaymentMethods
	{
		public int PaymentMethodsId { get; set; }
		
		public string PaymentType { get; set; }

		public string NameOnCard { get; set; }

		public string CardNumber { get; set; }

		public string CardCvv { get; set; }

		public DateTime CardExpiry { get; set; }

		public string CustomerId { get; set; }

		public ICollection<OrderDetails>? OrderDetails { get; set; }

		public AppUser? Customer { get; set; }
    }
}

