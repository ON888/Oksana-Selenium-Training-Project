using DocumentFormat.OpenXml.VariantTypes;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace LoginTest
{
    public class Login
    {
        IWebDriver drv;
        WebDriverWait wait;

        public string baseUrl = Environment.GetEnvironmentVariable("ENT_QA_BASE_URL");
        public string password = Environment.GetEnvironmentVariable("ENT_QA_PASS");
        public string username = Environment.GetEnvironmentVariable("ENT_QA_USER");
        public string companyName = Environment.GetEnvironmentVariable("ENT_QA_COMPANY");

        [SetUp]
        public void Setup()
        {
            drv = new ChromeDriver(); // create instance of the Chrome browser
            //drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            wait = new WebDriverWait(drv, TimeSpan.FromSeconds(10));

            drv.Navigate().GoToUrl(baseUrl + "/CorpNet/Login.aspx"); //Navigate to CE instance

            Assert.True(isVisible(By.CssSelector(".login-page-header")));

        }

        [TearDown]
        public void TearDown()
        {
            drv.Quit(); //close browser
        }
        private bool IsElementPresent(By locator)
        {
            return drv.FindElements(locator).Count > 0;
        }

        private bool isVisible(By locator)
        {
            return drv.FindElements(locator).Count > 0;
        }

        [Test]
        public void isElementPresent ()
        {
            drv.Navigate().GoToUrl(baseUrl + "/corpnet/common/dashboard.aspx");
            
            if (IsElementPresent(By.Id("username")))// check if username filed is present on the page and then fill in data
            {

                drv.FindElement(By.Id("username")).SendKeys($"{username}"); //enter username from env variables
                drv.FindElement(By.Id("password")).SendKeys($"{password}"); //enter password from env variables
                drv.FindElement(By.Name("_companyText")).SendKeys($"{companyName}");
                drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click(); //SendKeys(Keys.Enter);

            }
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop")));// wait uuser element is visible
            
            if (wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("username"))))
            {
                drv.FindElement(By.CssSelector(".menu-secondary .menu-user")).Click();
                drv.FindElement(By.CssSelector("li.menu-logout a")).Click();
            }
        }


        [TestCase("test", "test", "QA Requestor Summer")]
        //[TestCase($"{username}", "test", $"{company}")]
        //[TestCase($"{username}", $"{company}", "test")]
        public void InvalidCredentialsLogin(string username1, string password1, string company1)
        {
            //Invalid credentials provided; Error message received verification

            bool loginPage = IsElementPresent(By.CssSelector(".login-page-header"));

            if (loginPage)
            {

                drv.FindElement(By.Id("username")).SendKeys(username1);
                drv.FindElement(By.Id("password")).SendKeys(password1);
                drv.FindElement(By.Name("_companyText")).SendKeys(company1);
                drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click(); //SendKeys(Keys.Enter);
            }

            var error = drv.FindElement(By.CssSelector("div.login-table-b ul li")).Text;

            Assert.Multiple(() =>
            {
                Assert.That(error, Is.EqualTo("Invalid User ID or Password"));
                Assert.That(loginPage, Is.True);
            });
        }
        
        [Test]
        public void ValidCredentialsLogin()
        {
            //Succesful Login User logged verification

            bool loginPage = IsElementPresent(By.CssSelector(".login-page-header")); // 

            if (loginPage) //check if login page header page is present
            {
                drv.FindElement(By.Id("username")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_USER")); //enter username from env variables
                drv.FindElement(By.Id("password")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_PASS")); //enter password from env variables
                drv.FindElement(By.Name("_companyText")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
                drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click(); //SendKeys(Keys.Enter);
            }
            var user = drv.FindElement(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop"));        
            
            Assert.Multiple(() =>
            {
                Assert.That(isVisible(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop")), Is.True);
                Assert.That(user.Text, Is.EqualTo("Quality Assurance"));
            });
        }
        
    }
}