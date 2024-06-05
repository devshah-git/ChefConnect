using System;
namespace ChefConnect.Entities
{
	public class Reviews
	{
        public enum ReviewStatus
        {
            Reported,
            Clean
        }

		public int ReviewsId { get; set; }

        public string? ReviewDescription { get; set; }

        public int Ratings { get; set; }

        public string ChefId { get; set; }

        public string CustomerId { get; set; }

        public AppUser? Customer { get; set; }

        public ReviewStatus? Status { get; set; } = ReviewStatus.Clean;

        public DateTime? ReviewDate { get; set; } = DateTime.Now;

        public int? chefRecipeId { get; set; }

        public ChefRecipes? ChefRecipe { get; set; }

    }
}

