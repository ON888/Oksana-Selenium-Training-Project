using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LoginTest
{
    public class Tests
    {
        IWebDriver drv;
        

        [SetUp]
        public void Setup()
        {
            drv = new ChromeDriver(); // create instance of the Chrome browser
            drv.Navigate().GoToUrl(Environment.GetEnvironmentVariable("ENT_QA_BASE_URL") +
                    "/CorpNet/Login.aspx"); //Navigate to CE instance
        }

        [TearDown]
        public void TearDown()
        {
            drv.Quit(); //close browser
        }

        [Test]
        public void InvalidUsername()
        {
            //User verification
            drv.FindElement(By.Id("username")).SendKeys("test");
            drv.FindElement(By.Id("password")).SendKeys((Environment.GetEnvironmentVariable("ENT_QA_PASS")));
            drv.FindElement(By.Name("_companyText")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
            drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click(); //SendKeys(Keys.Enter);
            Thread.Sleep(4000);
            var error = drv.FindElement(By.CssSelector("div.login-table-b ul li")).Text;
            var loginPage = drv.FindElement(By.CssSelector("#mainContentWrapper"));

            Assert.Multiple(() =>
            {
                Assert.That(error, Is.EqualTo("Invalid User ID or Password"));
                Assert.That(loginPage, Is.True);
            });
        }
        [Test]
        public void InvalidPass()
        {
            //Password verification
            drv.FindElement(By.Id("username")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_USER"));
            drv.FindElement(By.Id("password")).SendKeys("test");
            drv.FindElement(By.Name("_companyText")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
            drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click(); //SendKeys(Keys.Enter);
            Thread.Sleep(4000);
            var error = drv.FindElement(By.CssSelector("div.login-table-b ul li")).Text;
            var loginPage = drv.FindElement(By.CssSelector("#mainContentWrapper"));

            Assert.Multiple(() =>
            {
                Assert.That(error, Is.EqualTo("Invalid User ID or Password"));
                Assert.That(loginPage, Is.True);
            });
        }

        [Test]
        public void InvalidCompany()
        {
            //Company verification
            drv.FindElement(By.Id("username")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_USER"));
            drv.FindElement(By.Name("_companyText")).SendKeys("test");
            drv.FindElement(By.Id("password")).SendKeys((Environment.GetEnvironmentVariable("ENT_QA_PASS")));
            drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click(); //SendKeys(Keys.Enter);
            Thread.Sleep(4000);
            var error = drv.FindElement(By.CssSelector("div.login-table-b ul li")).Text;
            var loginPage = drv.FindElement(By.CssSelector("#mainContentWrapper"));

            Assert.Multiple(() =>
            {
                Assert.That(error, Is.EqualTo("Invalid User ID or Password"));
                Assert.That(loginPage, Is.True);
            });
        }

        [Test]
        public void ValidTest()
        {
            //Succesful Login User logged verification

            drv.FindElement(By.Id("username")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_USER")); //enter username from env variables
            drv.FindElement(By.Id("password")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_PASS")); //enter password from env variables
            drv.FindElement(By.Name("_companyText"))
            .SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
            drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click(); //SendKeys(Keys.Enter);
            Thread.Sleep(4000);

            var user = drv.FindElement(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop")).GetAttribute("text");        
            var loginPage = drv.FindElement(By.CssSelector("#mainContentWrapper"));

            Assert.Multiple(() =>
            {
                Assert.That(user, Is.EqualTo("ENT_QA_USER"));
                Assert.That(loginPage, Is.False);
            });
        }
    }
}