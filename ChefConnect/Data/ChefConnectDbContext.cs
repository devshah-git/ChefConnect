using ChefConnect.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MimeKit.Tnef;

namespace ChefConnect.Data;

public class ChefConnectDbContext : IdentityDbContext<AppUser>
{
    public ChefConnectDbContext(DbContextOptions<ChefConnectDbContext> options)
        : base(options)
    {

    }

    public static async Task CreateAdminUser(IServiceProvider serviceProvider)
    {
        UserManager<AppUser> userManager =
            serviceProvider.GetRequiredService<UserManager<AppUser>>();
        RoleManager<IdentityRole> roleManager = serviceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        string username = "admin";
        string password = "Sesame123#";
        string roleName = "Admin";

        // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
        // if username doesn't exist, create it and add it to role
        if (await userManager.FindByNameAsync(username) == null)
        {
            AppUser user = new AppUser { UserName = username };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }



        var customerList = new[]
        {
            new AppUser{UserName = "Rajesh_Kumar", Name = "Rajesh",Id="101"},
            new AppUser{UserName = "Emily_Robinson", Name = "Emily",Id="102"},
            new AppUser{UserName = "Amanpreet_Singh", Name = "Amanpreet",Id="103"},
            new AppUser{UserName = "Sophia_Lee", Name = "Sophia",Id="104"},
            new AppUser{UserName = "Liam_Murphy", Name = "Liam",Id="105"},
            new AppUser{UserName = "Isabella_Gomez", Name = "Isabella",Id="106"},
            new AppUser{UserName = "Rohan_Singh", Name = "Rohan", Id = "107"},
            new AppUser{UserName = "Ava_Chen", Name = "Ava",Id="108"},
            new AppUser{UserName = "Ethan_Wright", Name = "Ethan", Id = "109"},
            new AppUser{UserName = "Maya_Krishnan", Name = "Maya",Id="110"},
            new AppUser{UserName = "Karan_Mehra", Name = "Karan", Id = "111"},
            new AppUser{UserName = "Lily_Anderson", Name = "Lily", Id = "112"},
            new AppUser{UserName = "Samuel_Rodriguez", Name = "Samuel", Id = "113"},
            new AppUser{UserName = "Emma_Choi", Name = "Emma",Id="114"},
            new AppUser{UserName = "Nathan_Smith", Name = "Nathan", Id = "115"},
            new AppUser{UserName = "Jessica_Lin", Name = "Jessica",Id="116"},
            new AppUser{UserName = "Mark_Johnson", Name = "Mark", Id = "117"},
            new AppUser{UserName = "Sophie_Martinez", Name = "Sophie",Id="118"},
            new AppUser{UserName = "Ryan_Kim", Name = "Ryan", Id = "119"},
            new AppUser{UserName = "Olivia_Hansen", Name = "Olivia",Id="120"},
            new AppUser{UserName = "Alisha_Patel", Name = "Alisha", Id = "121"},
            new AppUser{UserName = "Benjamin_Lee", Name = "Benjamin",Id="122"},
            new AppUser{UserName = "Nora_Fernandez", Name = "Nora",Id="123"},
            new AppUser{UserName = "Evan_Wong", Name = "Evan",Id="124"},
            new AppUser{UserName = "Isaac_Kim", Name = "Isaac",Id="125"},
            new AppUser{UserName = "Diana_Murphy", Name = "Diana",Id="126"},
            new AppUser{UserName = "Alex_Chen", Name = "Alex",Id="127"},
            new AppUser{UserName = "Grace_Lee", Name = "Grace",Id="128"},
            new AppUser{UserName = "Mohammed_Ali", Name = "Mohammed",Id="129"},
            new AppUser{UserName = "Hannah_Brooks", Name = "Hannah",Id="130"},
            new AppUser{UserName = "Tyler_Evans", Name = "Tyler",Id="131"},
            new AppUser{UserName = "Sophia_Nguyen", Name = "Sophia",Id="132"},
            new AppUser{UserName = "Lucas_Martinez", Name = "Lucas",Id="133"},
            new AppUser{UserName = "Madison_Wright", Name = "Madison",Id="134"},
            new AppUser{UserName = "Jack_Thompson", Name = "Jack",Id="135"}



        };

        for (int i = 0; i < customerList.Length; i++)
        {
            if (await userManager.FindByNameAsync(customerList[i].UserName) == null)
            {
                var result = await userManager.CreateAsync(customerList[i], "Dev@300702");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerList[i], "Customer");
                }
            }
            //else
            //{
            //    await userManager.UpdateAsync(customerList[i]);
            //}

        }
    }

    // Tables related to Chefs
    public DbSet<ChefRecipes> ChefRecipes { get; set; }
    public DbSet<ChefCuisines> ChefCuisines { get; set; }

    // Tables related to Customer
    public DbSet<Addresses> Addresses { get; set; }
    public DbSet<PaymentMethods> PaymentMethods { get; set; }

    // Other Tables
    public DbSet<Cuisines> Cuisines { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }
    public DbSet<Reviews> Reviews { get; set; }
    public DbSet<TimeSlots> TimeSlots { get; set; }
    public DbSet<OrderRecipes> OrderRecipes { get; set; }
    public DbSet<UserCartItem> UserCartItems { get; set; }

    //public DbSet<RecipeImages> RecipeImages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // One to Many Relationship between Cuisines and ChefRecipe
        builder.Entity<Cuisines>().HasMany(c => c.Recipes).WithOne(r => r.RecipeCuisine).HasForeignKey(r => r.CuisineId).IsRequired();

        // Many to Many Relationship between Chef and Cusinie with linking table ChefCusinies
        builder.Entity<ChefCuisines>().HasKey(cc => new { cc.ChefId, cc.CuisineId });
        builder.Entity<ChefCuisines>().HasOne(cc => cc.Cuisine).WithMany(c => c.ChefCuisines).HasForeignKey(cc => cc.CuisineId).IsRequired();
        builder.Entity<ChefCuisines>().HasOne(cc => cc.Chef).WithMany(c => c.ChefCuisines).HasForeignKey(cc => cc.ChefId).IsRequired();

        // One to Many Relationship between Customer and Reviews
        builder.Entity<Reviews>().HasOne(r => r.Customer).WithMany(c => c.Reviews).HasForeignKey(r => r.CustomerId).IsRequired();

        // One to Many Realtionship bewtween Chef and ChefRecipes
        builder.Entity<ChefRecipes>().HasOne(r => r.Chef).WithMany(c => c.ChefRecipes).HasForeignKey(r => r.ChefId).IsRequired();

        // One to Many Relationship between Customer and Payment Methods
        builder.Entity<PaymentMethods>().HasOne(r => r.Customer).WithMany(c => c.PaymentMethods).HasForeignKey(r => r.CustomerId).IsRequired();

        // Many to Many Realtionship between ChefRecipes and OrderDetails with linking table OrderRecipes
        builder.Entity<OrderRecipes>().HasKey(cc => new { cc.OrderDetailsId, cc.ChefRecipesId });
        builder.Entity<OrderRecipes>().HasOne(cc => cc.ChefRecipes).WithMany(c => c.Orders).HasForeignKey(cc => cc.ChefRecipesId).IsRequired();
        builder.Entity<OrderRecipes>().HasOne(cc => cc.OrderDetails).WithMany(c => c.OrderRecipes).HasForeignKey(cc => cc.OrderDetailsId).IsRequired(false);

        // One to Many Realtionship between Timeslot and OrderRecipes
        builder.Entity<OrderRecipes>().HasOne(r => r.TimeSlot).WithMany(c => c.OrderRecipes).HasForeignKey(r => r.TimeSlotId).IsRequired();

        // One to Many Realtionship between Customer and OrderDetails
        builder.Entity<OrderDetails>().HasOne(r => r.Customer).WithMany(c => c.OrderDetails).HasForeignKey(r => r.CustomerId).IsRequired();

        //one to many relationship between ChefRecipes and usercartitems
        builder.Entity<UserCartItem>().HasOne(r => r.ChefRecipe).WithMany(c => c.UserCartItems).HasForeignKey(r => r.RecipeId).IsRequired();

        //one-to-many relationship between TimeSlots and UserCartItems
        builder.Entity<UserCartItem>().HasOne(r => r.TimeSlot).WithMany(c => c.UserCartItems).HasForeignKey(r => r.TimeSlotId).IsRequired();


        //one-to-many relationship between AppUser and address
        builder.Entity<Addresses>().HasOne(r => r.Customer).WithMany(c => c.Addresses).HasForeignKey(r => r.CustomerId).IsRequired();

        //one-to-many between OrderDetails and Address
        builder.Entity<OrderDetails>().HasOne(r => r.Address).WithMany(c => c.OrderDetails).HasForeignKey(r => r.addressId).IsRequired(false);

        //one-to-many between OrderDetails and PaymentMethods
        builder.Entity<OrderDetails>().HasOne(r => r.PaymentMethod).WithMany(c => c.OrderDetails).HasForeignKey(r => r.paymentMethodId).IsRequired(false);

        //one-to-many between ChefRecipes and Reviews
        builder.Entity<Reviews>().HasOne(r => r.ChefRecipe).WithMany(c => c.Reviews).HasForeignKey(r => r.chefRecipeId).IsRequired(false);


        var cuisines = new[]
        {
            new Cuisines { CuisinesId = 1, CuisineName = "Italian" },
            new Cuisines { CuisinesId = 2, CuisineName = "Mexican" },
            new Cuisines { CuisinesId = 3, CuisineName = "Japanese" },
            new Cuisines { CuisinesId = 4, CuisineName = "Chinese" },
            new Cuisines { CuisinesId = 5, CuisineName = "Indian" },
            new Cuisines { CuisinesId = 6, CuisineName = "French" },
            new Cuisines { CuisinesId = 7, CuisineName = "Thai" },
            new Cuisines { CuisinesId = 8, CuisineName = "Spanish" },
            new Cuisines { CuisinesId = 9, CuisineName = "Greek" },
            new Cuisines { CuisinesId = 10, CuisineName = "Turkish" },
            new Cuisines { CuisinesId = 11, CuisineName = "Korean" },
            new Cuisines { CuisinesId = 12, CuisineName = "Vietnamese" },
            new Cuisines { CuisinesId = 13, CuisineName = "Lebanese" },
            new Cuisines { CuisinesId = 14, CuisineName = "Brazilian" },
            new Cuisines { CuisinesId = 15, CuisineName = "Mediterranean" },
            new Cuisines { CuisinesId = 16, CuisineName = "German" },
            new Cuisines { CuisinesId = 17, CuisineName = "British" },
            new Cuisines { CuisinesId = 18, CuisineName = "Russian" },
            new Cuisines { CuisinesId = 19, CuisineName = "American" },
            new Cuisines { CuisinesId = 20, CuisineName = "Caribbean" }

        };


        var timeSlots = new[]
        {
            new TimeSlots { TimeSlotsId = 1, TimeSlot = new TimeSpan(0, 0, 0) },   // 00:00
            new TimeSlots { TimeSlotsId = 2, TimeSlot = new TimeSpan(0, 30, 0) },  // 00:30
            new TimeSlots { TimeSlotsId = 3, TimeSlot = new TimeSpan(1, 0, 0) },   // 01:00
            new TimeSlots { TimeSlotsId = 4, TimeSlot = new TimeSpan(1, 30, 0) },  // 01:30
            new TimeSlots { TimeSlotsId = 5, TimeSlot = new TimeSpan(2, 0, 0) },   // 02:00
            new TimeSlots { TimeSlotsId = 6, TimeSlot = new TimeSpan(2, 30, 0) },  // 02:30
            new TimeSlots { TimeSlotsId = 7, TimeSlot = new TimeSpan(3, 0, 0) },   // 03:00
            new TimeSlots { TimeSlotsId = 8, TimeSlot = new TimeSpan(3, 30, 0) },  // 03:30
            new TimeSlots { TimeSlotsId = 9, TimeSlot = new TimeSpan(4, 0, 0) },   // 04:00
            new TimeSlots { TimeSlotsId = 10, TimeSlot = new TimeSpan(4, 30, 0) }, // 04:30
            new TimeSlots { TimeSlotsId = 11, TimeSlot = new TimeSpan(5, 0, 0) },  // 05:00
            new TimeSlots { TimeSlotsId = 12, TimeSlot = new TimeSpan(5, 30, 0) }, // 05:30
            new TimeSlots { TimeSlotsId = 13, TimeSlot = new TimeSpan(6, 0, 0) },  // 06:00
            new TimeSlots { TimeSlotsId = 14, TimeSlot = new TimeSpan(6, 30, 0) }, // 06:30
            new TimeSlots { TimeSlotsId = 15, TimeSlot = new TimeSpan(7, 0, 0) },  // 07:00
            new TimeSlots { TimeSlotsId = 16, TimeSlot = new TimeSpan(7, 30, 0) }, // 07:30
            new TimeSlots { TimeSlotsId = 17, TimeSlot = new TimeSpan(8, 0, 0) },  // 08:00
            new TimeSlots { TimeSlotsId = 18, TimeSlot = new TimeSpan(8, 30, 0) }, // 08:30
            new TimeSlots { TimeSlotsId = 19, TimeSlot = new TimeSpan(9, 0, 0) },  // 09:00
            new TimeSlots { TimeSlotsId = 20, TimeSlot = new TimeSpan(9, 30, 0) }, // 09:30
            new TimeSlots { TimeSlotsId = 21, TimeSlot = new TimeSpan(10, 0, 0) }, // 10:00
            new TimeSlots { TimeSlotsId = 22, TimeSlot = new TimeSpan(10, 30, 0) },// 10:30
            new TimeSlots { TimeSlotsId = 23, TimeSlot = new TimeSpan(11, 0, 0) }, // 11:00
            new TimeSlots { TimeSlotsId = 24, TimeSlot = new TimeSpan(11, 30, 0) },// 11:30
            new TimeSlots { TimeSlotsId = 25, TimeSlot = new TimeSpan(12, 0, 0) }, // 12:00 PM
            new TimeSlots { TimeSlotsId = 26, TimeSlot = new TimeSpan(12, 30, 0) },// 12:30 PM
            new TimeSlots { TimeSlotsId = 27, TimeSlot = new TimeSpan(13, 0, 0) }, // 01:00 PM
            new TimeSlots { TimeSlotsId = 28, TimeSlot = new TimeSpan(13, 30, 0) },// 01:30 PM
            new TimeSlots { TimeSlotsId = 29, TimeSlot = new TimeSpan(14, 0, 0) }, // 02:00 PM
            new TimeSlots { TimeSlotsId = 30, TimeSlot = new TimeSpan(14, 30, 0) },// 02:30 PM
            new TimeSlots { TimeSlotsId = 31, TimeSlot = new TimeSpan(15, 0, 0) }, // 03:00 PM
            new TimeSlots { TimeSlotsId = 32, TimeSlot = new TimeSpan(15, 30, 0) },// 03:30 PM
            new TimeSlots { TimeSlotsId = 33, TimeSlot = new TimeSpan(16, 0, 0) }, // 04:00 PM
            new TimeSlots { TimeSlotsId = 34, TimeSlot = new TimeSpan(16, 30, 0) },// 04:30 PM
            new TimeSlots { TimeSlotsId = 35, TimeSlot = new TimeSpan(17, 0, 0) }, // 05:00 PM
            new TimeSlots { TimeSlotsId = 36, TimeSlot = new TimeSpan(17, 30, 0) },// 05:30 PM
            new TimeSlots { TimeSlotsId = 37, TimeSlot = new TimeSpan(18, 0, 0) }, // 06:00 PM
            new TimeSlots { TimeSlotsId = 38, TimeSlot = new TimeSpan(18, 30, 0) },// 06:30 PM
            new TimeSlots { TimeSlotsId = 39, TimeSlot = new TimeSpan(19, 0, 0) }, // 07:00 PM
            new TimeSlots { TimeSlotsId = 40, TimeSlot = new TimeSpan(19, 30, 0) },// 07:30 PM
            new TimeSlots { TimeSlotsId = 41, TimeSlot = new TimeSpan(20, 0, 0) }, // 08:00 PM
            new TimeSlots { TimeSlotsId = 42, TimeSlot = new TimeSpan(20, 30, 0) },// 08:30 PM
            new TimeSlots { TimeSlotsId = 43, TimeSlot = new TimeSpan(21, 0, 0) }, // 09:00 PM
            new TimeSlots { TimeSlotsId = 44, TimeSlot = new TimeSpan(21, 30, 0) },// 09:30 PM
            new TimeSlots { TimeSlotsId = 45, TimeSlot = new TimeSpan(22, 0, 0) }, // 10:00 PM
            new TimeSlots { TimeSlotsId = 46, TimeSlot = new TimeSpan(22, 30, 0) },// 10:30 PM
            new TimeSlots { TimeSlotsId = 47, TimeSlot = new TimeSpan(23, 0, 0) }, // 11:00 PM
            new TimeSlots { TimeSlotsId = 48, TimeSlot = new TimeSpan(23, 30, 0) } // 11:30 PM
        };




        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Identity");
        builder.Entity<AppUser>(entity =>
        {
            entity.ToTable(name: "User");
        });
        builder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "Role");
        });
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles");
        });
        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims");
        });
        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins");
        });
        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaims");
        });
        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens");
        });

        builder.Entity<IdentityRole>().HasData(new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = "1",
                Name = "Chef",
                NormalizedName = "CHEF"
            },
            new IdentityRole
            {
                Id = "2",
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            }
        });

        builder.Entity<Cuisines>().HasData(cuisines);
        //builder.Entity<AppUser>().HasData(CustomerList);
        builder.Entity<TimeSlots>().HasData(timeSlots);
    }
}
