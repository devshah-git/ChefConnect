



using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace TestLib
{

    [TestFixture]
    public class Tests
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            ChromeOptions options = new ChromeOptions();

            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
        }

        //Test case for Home Page Login Load Verification
        //Test-1
        [Test]
        public void tC01HomePageLoginLoadVerification()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(978, 960);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        //Test case for Home Page Register for Customer
        //Test-2
        [Test]
        public void tC02HomePageRegisterForCustomer()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.FindElement(By.LinkText("Register")).Click();
            driver.FindElement(By.CssSelector(".btn-customer")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("ktest100");
            driver.FindElement(By.Id("Name")).SendKeys("KandarpPatel");
            driver.FindElement(By.Id("Email")).SendKeys("kandarppatel3635@gmail.com");
            driver.FindElement(By.Id("PhoneNumber")).SendKeys("4379716453");
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.Id("ConfirmPassword")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        //Test case for Home Page Register for Chef
        //Test-3
        [Test]
        public void tC03HomePageRegisterForChef()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
            driver.FindElement(By.LinkText("Register")).Click();
            driver.FindElement(By.CssSelector(".btn-chef")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Dhyey1");
            driver.FindElement(By.Id("Name")).SendKeys("Dhyeyjash2");
            driver.FindElement(By.Id("Email")).Click();
            driver.FindElement(By.Id("Email")).SendKeys("dhyeyjash267@gmail.com");
            //driver.FindElement(By.Id("DateOfBirth")).Click();
            driver.FindElement(By.Id("DateOfBirth")).SendKeys("06-06-2001");
            //driver.FindElement(By.Id("PhoneNumber")).Click();
            driver.FindElement(By.Id("PhoneNumber")).SendKeys("4378771234");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.Id("ConfirmPassword")).Click();
            driver.FindElement(By.Id("ConfirmPassword")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }


        //Test case for Invalid User Registration
        //Test-4
        [Test]
        public void tC04InvalidUserRegistration()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 976);
            driver.FindElement(By.LinkText("Register")).Click();
            driver.FindElement(By.CssSelector(".btn-chef")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("AyushC");
            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).SendKeys("Dev");
            driver.FindElement(By.Id("Email")).Click();
            driver.FindElement(By.Id("Email")).SendKeys("dev@gmail.com");
            //driver.FindElement(By.Id("DateOfBirth")).Click();
            
            driver.FindElement(By.Id("DateOfBirth")).SendKeys("07-27-2002");
            //driver.FindElement(By.Id("PhoneNumber")).Click();
            driver.FindElement(By.Id("PhoneNumber")).SendKeys("4379716240");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.Id("ConfirmPassword")).SendKeys("Dev@300702");
            //driver.FindElement(By.CssSelector(".form-outline:nth-child(1) > .text-danger")).Click();
            //driver.FindElement(By.CssSelector(".form-outline:nth-child(1) > .text-danger")).Click();
            //{
            //    var element = driver.FindElement(By.CssSelector(".form-outline:nth-child(1) > .text-danger"));
            //    Actions builder = new Actions(driver);
            //    builder.DoubleClick(element).Perform();
            //}
            //driver.FindElement(By.CssSelector(".form-outline:nth-child(1) > .text-danger")).Click();
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        //Test case for Valid Chef Login
        //Test-5
        [Test]
        public void tC05ValidChefLogin()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1936, 1096);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("ktest100");
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        //Test case for Valid Customer Login
        //Test-6
        [Test]
        public void tC06ValidCustomerLogin()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 976);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        //Test case for Recipe Search By Customer
        //Test-7
        [Test]
        public void tC07RecipeSearchByCustomer()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 976);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.LinkText("Search")).Click();
            driver.FindElement(By.Name("search")).Click();
            driver.FindElement(By.Name("search")).SendKeys("Poutine");
            driver.FindElement(By.CssSelector(".form-search > button")).Click();
        }

        //Test case for Cuisine Search By Customer
        //Test-8
        [Test]
        public void tC08CuisineSearchByCustomer()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 976);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.LinkText("Search")).Click();
            driver.FindElement(By.LinkText("Turkish")).Click();
        }

        //Test case for Add Recipe To Cart By Customer
        //Test-9
        [Test]
        public void tC09AddRecipeToCartByCustomer()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 976);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.CssSelector("#recipeCarousel-other .btn")).Click();
        }

        //Test case for Customer View Cart
        //Test-10
        [Test]
        public void tC10CustomerViewCart()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 976);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.Id("dropdownMenuButton")).Click();
            driver.FindElement(By.LinkText("Cart")).Click();
        }

        //Test case for Customer Update Cart Quantity
        //Test-11
        [Test]
        public void tC11CustomerUpdateCartQuantity()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 976);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.Id("dropdownMenuButton")).Click();
            driver.FindElement(By.LinkText("Cart")).Click();
            driver.FindElement(By.Id("GuestQuantity")).Click();
            driver.FindElement(By.Id("GuestQuantity")).SendKeys("2");
            driver.FindElement(By.CssSelector(".cart-container:nth-child(4) .btn:nth-child(11)")).Click();
        }

        //Test case for Customer Remove Item From Cart
        //Test-12
        [Test]
        public void tC12CustomerRemoveItemFromCart()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 976);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.Id("dropdownMenuButton")).Click();
            driver.FindElement(By.LinkText("Cart")).Click();
            driver.FindElement(By.LinkText("Remove")).Click();
        }

        //Test case for Customer Checkout Cart Items
        //Test-13
        [Test]
        public void tC13CustomerCheckoutCartItems()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 976);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.CssSelector("#recipeCarousel-other .btn")).Click();
            //driver.FindElement(By.Id("OrderDate")).Click();
            driver.FindElement(By.Id("OrderDate")).SendKeys("04-03-2023");
            driver.FindElement(By.Id("GuestQuantity")).Click();
            driver.FindElement(By.Id("GuestQuantity")).SendKeys("2");
            driver.FindElement(By.CssSelector(".btn:nth-child(11)")).Click();
            driver.FindElement(By.LinkText("Proceed to Checkout")).Click();
        }


        //Test case for Customer Update Profile
        //Test-14
        [Test]
        public void tC14CustomerUpdateProfile()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 976);
            //driver.FindElement(By.CssSelector(".container-fluid")).Click();
            //driver.FindElement(By.LinkText("Register")).Click();
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.Id("dropdownMenuButton")).Click();
            driver.FindElement(By.LinkText("Update Profile")).Click();
            driver.FindElement(By.Id("ActiveUser_Email")).Click();
            driver.FindElement(By.Id("ActiveUser_Email")).SendKeys("rkumar@gmail.com");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }

        //Test case for Customer Logout
        //Test-15
        [Test]
        public void tC15LogOutCustomer()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 976);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300701");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.LinkText("Logout")).Click();
        }

        //Test case for Chef Report Review
        //Test-16
        [Test]
        public void tC16ChefReportReview()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 976);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("ktest");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300702");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.LinkText("My Reviews")).Click();
            driver.FindElement(By.LinkText("Report")).Click();
        }


        //Test case for Customer Add Review
        //Test-17
        [Test]
        public void tC17CustomerAddReview()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 1080);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300701");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.LinkText("Orders")).Click();
            driver.FindElement(By.LinkText("Show Past Orders")).Click();
            driver.FindElement(By.CssSelector(".button-cart")).Click();
            driver.FindElement(By.Id("NewReview_ReviewDescription")).Click();
            driver.FindElement(By.Id("NewReview_ReviewDescription")).SendKeys("Nice work");
            driver.FindElement(By.Id("NewReview_Ratings")).Click();
            driver.FindElement(By.Id("NewReview_Ratings")).SendKeys("2");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
        }


        //Customer removes a booking
        //Test-18
        [Test]
        public void tC18CustomerCancelBooking()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 1080);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Rajesh_Kumar");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).SendKeys("Dev@300701");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.LinkText("Orders")).Click();
            {
                var element = driver.FindElement(By.LinkText("Show Upcoming Orders"));
                //Actions builder = new Actions(driver);
                //builder.MoveToElement(element).Perform();
            }
            //driver.Close();
            
        }


        //Admin approves a review
        //Test-19
        [Test]
        public void tC19AdminApproveReview()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 1080);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Admin");
            driver.FindElement(By.Id("Password")).SendKeys("Sesame123#");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.LinkText("Reviews")).Click();
            driver.FindElement(By.LinkText("Approve")).Click();
        }

        //admin deletes a review
        //Test-20
        [Test]
        public void tC20AdminDeleteReview()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 1080);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("UserName")).Click();
            driver.FindElement(By.Id("UserName")).SendKeys("Admin");
            driver.FindElement(By.Id("Password")).SendKeys("Sesame123#");
            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            driver.FindElement(By.LinkText("Reviews")).Click();
            driver.FindElement(By.LinkText("Delete")).Click();
        }
    }
}