using System;
namespace ChefConnect.Entities
{
	public class ChefRecipes
	{
		public int ChefRecipesId { get; set; }

		public string RecipeName { get; set; }

		public string RecipeDescription { get; set; }

		public byte[] RecipeImage { get; set; }

		public int NumberOfPeople { get; set; }

		public double Price { get; set; }

		public double PricePerExtraPerson { get; set; }

        public int CuisineId { get; set; }

		public Cuisines RecipeCuisine { get; set; }

        public string ChefId { get; set; }

		public AppUser? Chef { get; set; }

		public ICollection<OrderRecipes> Orders { get; set; }

		public ICollection<UserCartItem>? UserCartItems { get; set; }

		public ICollection<Reviews>? Reviews { get; set; }
	}
}

