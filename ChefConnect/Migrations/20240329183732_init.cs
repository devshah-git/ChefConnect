using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChefConnect.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "Cuisines",
                schema: "Identity",
                columns: table => new
                {
                    CuisinesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CuisineName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuisines", x => x.CuisinesId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlots",
                schema: "Identity",
                columns: table => new
                {
                    TimeSlotsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeSlot = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlots", x => x.TimeSlotsId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "Identity",
                columns: table => new
                {
                    AddressesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AptNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressesId);
                    table.ForeignKey(
                        name: "FK_Addresses_User_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChefCuisines",
                schema: "Identity",
                columns: table => new
                {
                    ChefId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CuisineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChefCuisines", x => new { x.ChefId, x.CuisineId });
                    table.ForeignKey(
                        name: "FK_ChefCuisines_Cuisines_CuisineId",
                        column: x => x.CuisineId,
                        principalSchema: "Identity",
                        principalTable: "Cuisines",
                        principalColumn: "CuisinesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChefCuisines_User_ChefId",
                        column: x => x.ChefId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChefRecipes",
                schema: "Identity",
                columns: table => new
                {
                    ChefRecipesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipeDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipeImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    NumberOfPeople = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    PricePerExtraPerson = table.Column<double>(type: "float", nullable: false),
                    CuisineId = table.Column<int>(type: "int", nullable: false),
                    ChefId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChefRecipes", x => x.ChefRecipesId);
                    table.ForeignKey(
                        name: "FK_ChefRecipes_Cuisines_CuisineId",
                        column: x => x.CuisineId,
                        principalSchema: "Identity",
                        principalTable: "Cuisines",
                        principalColumn: "CuisinesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChefRecipes_User_ChefId",
                        column: x => x.ChefId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                schema: "Identity",
                columns: table => new
                {
                    PaymentMethodsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameOnCard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardCvv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardExpiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.PaymentMethodsId);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_User_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "Identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                schema: "Identity",
                columns: table => new
                {
                    ReviewsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ratings = table.Column<int>(type: "int", nullable: false),
                    ChefId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    chefRecipeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewsId);
                    table.ForeignKey(
                        name: "FK_Reviews_ChefRecipes_chefRecipeId",
                        column: x => x.chefRecipeId,
                        principalSchema: "Identity",
                        principalTable: "ChefRecipes",
                        principalColumn: "ChefRecipesId");
                    table.ForeignKey(
                        name: "FK_Reviews_User_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCartItems",
                schema: "Identity",
                columns: table => new
                {
                    UserCartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    GuestQuantity = table.Column<int>(type: "int", nullable: true),
                    TimeSlotId = table.Column<int>(type: "int", nullable: false),
                    RecipeTotal = table.Column<double>(type: "float", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCartItems", x => x.UserCartItemId);
                    table.ForeignKey(
                        name: "FK_UserCartItems_ChefRecipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "Identity",
                        principalTable: "ChefRecipes",
                        principalColumn: "ChefRecipesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCartItems_TimeSlots_TimeSlotId",
                        column: x => x.TimeSlotId,
                        principalSchema: "Identity",
                        principalTable: "TimeSlots",
                        principalColumn: "TimeSlotsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                schema: "Identity",
                columns: table => new
                {
                    OrderDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderSubTotal = table.Column<double>(type: "float", nullable: false),
                    OrderTax = table.Column<double>(type: "float", nullable: false),
                    Charges = table.Column<double>(type: "float", nullable: false),
                    OrderTotal = table.Column<double>(type: "float", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    paymentMethodId = table.Column<int>(type: "int", nullable: false),
                    addressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailsId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Addresses_addressId",
                        column: x => x.addressId,
                        principalSchema: "Identity",
                        principalTable: "Addresses",
                        principalColumn: "AddressesId");
                    table.ForeignKey(
                        name: "FK_OrderDetails_PaymentMethods_paymentMethodId",
                        column: x => x.paymentMethodId,
                        principalSchema: "Identity",
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodsId");
                    table.ForeignKey(
                        name: "FK_OrderDetails_User_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderRecipes",
                schema: "Identity",
                columns: table => new
                {
                    OrderDetailsId = table.Column<int>(type: "int", nullable: false),
                    ChefRecipesId = table.Column<int>(type: "int", nullable: false),
                    GuestQuantity = table.Column<int>(type: "int", nullable: false),
                    TimeSlotId = table.Column<int>(type: "int", nullable: false),
                    RecipeTotal = table.Column<double>(type: "float", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRecipes", x => new { x.OrderDetailsId, x.ChefRecipesId });
                    table.ForeignKey(
                        name: "FK_OrderRecipes_ChefRecipes_ChefRecipesId",
                        column: x => x.ChefRecipesId,
                        principalSchema: "Identity",
                        principalTable: "ChefRecipes",
                        principalColumn: "ChefRecipesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderRecipes_OrderDetails_OrderDetailsId",
                        column: x => x.OrderDetailsId,
                        principalSchema: "Identity",
                        principalTable: "OrderDetails",
                        principalColumn: "OrderDetailsId");
                    table.ForeignKey(
                        name: "FK_OrderRecipes_TimeSlots_TimeSlotId",
                        column: x => x.TimeSlotId,
                        principalSchema: "Identity",
                        principalTable: "TimeSlots",
                        principalColumn: "TimeSlotsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Cuisines",
                columns: new[] { "CuisinesId", "CuisineName" },
                values: new object[,]
                {
                    { 1, "Italian" },
                    { 2, "Mexican" },
                    { 3, "Japanese" },
                    { 4, "Chinese" },
                    { 5, "Indian" },
                    { 6, "French" },
                    { 7, "Thai" },
                    { 8, "Spanish" },
                    { 9, "Greek" },
                    { 10, "Turkish" },
                    { 11, "Korean" },
                    { 12, "Vietnamese" },
                    { 13, "Lebanese" },
                    { 14, "Brazilian" },
                    { 15, "Mediterranean" },
                    { 16, "German" },
                    { 17, "British" },
                    { 18, "Russian" },
                    { 19, "American" },
                    { 20, "Caribbean" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Chef", "CHEF" },
                    { "2", null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "TimeSlots",
                columns: new[] { "TimeSlotsId", "TimeSlot" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 0, 0, 0, 0) },
                    { 2, new TimeSpan(0, 0, 30, 0, 0) },
                    { 3, new TimeSpan(0, 1, 0, 0, 0) },
                    { 4, new TimeSpan(0, 1, 30, 0, 0) },
                    { 5, new TimeSpan(0, 2, 0, 0, 0) },
                    { 6, new TimeSpan(0, 2, 30, 0, 0) },
                    { 7, new TimeSpan(0, 3, 0, 0, 0) },
                    { 8, new TimeSpan(0, 3, 30, 0, 0) },
                    { 9, new TimeSpan(0, 4, 0, 0, 0) },
                    { 10, new TimeSpan(0, 4, 30, 0, 0) },
                    { 11, new TimeSpan(0, 5, 0, 0, 0) },
                    { 12, new TimeSpan(0, 5, 30, 0, 0) },
                    { 13, new TimeSpan(0, 6, 0, 0, 0) },
                    { 14, new TimeSpan(0, 6, 30, 0, 0) },
                    { 15, new TimeSpan(0, 7, 0, 0, 0) },
                    { 16, new TimeSpan(0, 7, 30, 0, 0) },
                    { 17, new TimeSpan(0, 8, 0, 0, 0) },
                    { 18, new TimeSpan(0, 8, 30, 0, 0) },
                    { 19, new TimeSpan(0, 9, 0, 0, 0) },
                    { 20, new TimeSpan(0, 9, 30, 0, 0) },
                    { 21, new TimeSpan(0, 10, 0, 0, 0) },
                    { 22, new TimeSpan(0, 10, 30, 0, 0) },
                    { 23, new TimeSpan(0, 11, 0, 0, 0) },
                    { 24, new TimeSpan(0, 11, 30, 0, 0) },
                    { 25, new TimeSpan(0, 12, 0, 0, 0) },
                    { 26, new TimeSpan(0, 12, 30, 0, 0) },
                    { 27, new TimeSpan(0, 13, 0, 0, 0) },
                    { 28, new TimeSpan(0, 13, 30, 0, 0) },
                    { 29, new TimeSpan(0, 14, 0, 0, 0) },
                    { 30, new TimeSpan(0, 14, 30, 0, 0) },
                    { 31, new TimeSpan(0, 15, 0, 0, 0) },
                    { 32, new TimeSpan(0, 15, 30, 0, 0) },
                    { 33, new TimeSpan(0, 16, 0, 0, 0) },
                    { 34, new TimeSpan(0, 16, 30, 0, 0) },
                    { 35, new TimeSpan(0, 17, 0, 0, 0) },
                    { 36, new TimeSpan(0, 17, 30, 0, 0) },
                    { 37, new TimeSpan(0, 18, 0, 0, 0) },
                    { 38, new TimeSpan(0, 18, 30, 0, 0) },
                    { 39, new TimeSpan(0, 19, 0, 0, 0) },
                    { 40, new TimeSpan(0, 19, 30, 0, 0) },
                    { 41, new TimeSpan(0, 20, 0, 0, 0) },
                    { 42, new TimeSpan(0, 20, 30, 0, 0) },
                    { 43, new TimeSpan(0, 21, 0, 0, 0) },
                    { 44, new TimeSpan(0, 21, 30, 0, 0) },
                    { 45, new TimeSpan(0, 22, 0, 0, 0) },
                    { 46, new TimeSpan(0, 22, 30, 0, 0) },
                    { 47, new TimeSpan(0, 23, 0, 0, 0) },
                    { 48, new TimeSpan(0, 23, 30, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerId",
                schema: "Identity",
                table: "Addresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChefCuisines_CuisineId",
                schema: "Identity",
                table: "ChefCuisines",
                column: "CuisineId");

            migrationBuilder.CreateIndex(
                name: "IX_ChefRecipes_ChefId",
                schema: "Identity",
                table: "ChefRecipes",
                column: "ChefId");

            migrationBuilder.CreateIndex(
                name: "IX_ChefRecipes_CuisineId",
                schema: "Identity",
                table: "ChefRecipes",
                column: "CuisineId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_addressId",
                schema: "Identity",
                table: "OrderDetails",
                column: "addressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CustomerId",
                schema: "Identity",
                table: "OrderDetails",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_paymentMethodId",
                schema: "Identity",
                table: "OrderDetails",
                column: "paymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRecipes_ChefRecipesId",
                schema: "Identity",
                table: "OrderRecipes",
                column: "ChefRecipesId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRecipes_TimeSlotId",
                schema: "Identity",
                table: "OrderRecipes",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_CustomerId",
                schema: "Identity",
                table: "PaymentMethods",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_chefRecipeId",
                schema: "Identity",
                table: "Reviews",
                column: "chefRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomerId",
                schema: "Identity",
                table: "Reviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Identity",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Identity",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Identity",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Identity",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserCartItems_RecipeId",
                schema: "Identity",
                table: "UserCartItems",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCartItems_TimeSlotId",
                schema: "Identity",
                table: "UserCartItems",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "Identity",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "Identity",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Identity",
                table: "UserRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChefCuisines",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "OrderRecipes",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Reviews",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserCartItems",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "OrderDetails",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ChefRecipes",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "TimeSlots",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "PaymentMethods",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Cuisines",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Identity");
        }
    }
}
