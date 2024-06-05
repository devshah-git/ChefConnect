using System;
using ChefConnect.Entities;

namespace ChefConnect.Models
{
	public class CustomerViewModel
	{
		public AppUser? ActiveUser { get; set; }

		public List<ChefRecipes>? AllRecipes { get; set; }

		public ChefRecipes? ActiveRecipe { get; set; }

		public Reviews? NewReview { get; set; }

		public OrderDetails? NewOrder { get; set; }

		public List<UserCartItem>? cartList { get; set; }

		public List<TimeSlots>? TimeSlots { get; set; }

		public List<PaymentMethods>? PaymentMethodsList { get; set; }

		public List<Addresses>? addressList { get; set; }

		public List<AppUser>? ChefsList { get; set; }

		public List<Cuisines>? CuisinesList { get; set; }

        public List<OrderRecipes>? UpComingOrders { get; set; }

        public List<OrderRecipes>? PastOrders { get; set; }

		public List<ChefRecipes>? FiveStarRecipeList { get; set; }


    }
}

