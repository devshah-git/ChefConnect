using System;
namespace ChefConnect.Entities
{
	public class ChefCuisines
	{
		//public int ChefCuisinesId { get; set; }\
		
		public AppUser Chef { get; set; }

		public Cuisines Cuisine { get; set; }

	    public string ChefId { get; set; }

		public int CuisineId { get; set; }
		
	}
}

