namespace ChefConnect.Entities
{
    public class UserCartItem
    {
        public int UserCartItemId { get; set; }

        public int RecipeId { get; set; }

        public ChefRecipes? ChefRecipe { get; set; }

        public int? GuestQuantity { get; set; }

        public int? TimeSlotId { get; set; }

        public TimeSlots? TimeSlot { get; set; }

        public double? RecipeTotal { get; set; }

        public string? CustomerId { get; set; }

        public DateTime? OrderDate { get; set; }

    }
}
