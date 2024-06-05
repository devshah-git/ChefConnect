using System;
namespace ChefConnect.Entities
{
	public class TimeSlots
	{
		public int TimeSlotsId { get; set; }

		public TimeSpan TimeSlot { get; set; }

		public ICollection<OrderRecipes>? OrderRecipes { get; set; }
		public ICollection<UserCartItem>? UserCartItems { get; set; }
    }
}

