using System;
namespace ChefConnect.Entities
{
	public class Cuisines
	{
		public int CuisinesId { get; set; }

		public string CuisineName { get; set; }

		public ICollection<ChefRecipes> Recipes { get; set; }

		public ICollection<ChefCuisines> ChefCuisines { get; set; }	

		
	}
}

