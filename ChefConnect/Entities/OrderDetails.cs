using System;
namespace ChefConnect.Entities
{
    public class OrderDetails
    {
        public int OrderDetailsId { get; set; }

        public string? OrderInstructions { get; set; }

        public double OrderSubTotal { get; set; }

        public double OrderTax { get; set; }

        public double Charges { get; set; }

        public double OrderTotal { get; set; }

        public string CustomerId { get; set; }

        public int paymentMethodId { get; set; }

        public int addressId { get; set; }

        public Addresses? Address { get; set; }

        public PaymentMethods? PaymentMethod { get; set; }

        public AppUser? Customer { get; set; }

        //public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public ICollection<OrderRecipes>? OrderRecipes { get; set; }


    }
}

