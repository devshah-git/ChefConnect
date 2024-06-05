	using System;
namespace ChefConnect.Entities
{
	public class OrderRecipes
	{
		public int OrderDetailsId { get; set; }

		public OrderDetails? OrderDetails { get; set; }

		public int ChefRecipesId { get; set; }

		public ChefRecipes? ChefRecipes { get; set; }

		public int GuestQuantity { get; set; }

        public int TimeSlotId { get; set; }	

		public TimeSlots? TimeSlot { get; set; }

		public double RecipeTotal { get; set; }

		public DateTime OrderDate { get; set; }
	}
}

