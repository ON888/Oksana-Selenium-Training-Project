using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ReportTest
{
    public class FileUploadTest
    {
        IWebDriver drv;
        WebDriverWait wait;

        readonly string baseUrl = Environment.GetEnvironmentVariable("ENT_QA_BASE_URL");
        readonly string password = Environment.GetEnvironmentVariable("ENT_QA_PASS");
        readonly string username = Environment.GetEnvironmentVariable("ENT_QA_USER");
        readonly string company = Environment.GetEnvironmentVariable("ENT_QA_COMPANY");

        [SetUp]
        public void Setup()
        {
            drv = new ChromeDriver(); // create instance of the Chrome browser

            wait = new WebDriverWait(drv, TimeSpan.FromSeconds(10)); // explicit waits: wait object creation

            drv.Navigate().GoToUrl(baseUrl + "/CorpNet/Login.aspx");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("username"))).SendKeys($"{username}"); //wait and enter username from env variables

            drv.FindElement(By.Id("password")).SendKeys($"{password}"); //enter password from env variables
            drv.FindElement(By.Name("_companyText")).SendKeys($"{company}");
            drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click(); //SendKeys(Keys.Enter);

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop")));// wait uuser element is visible

            drv.Manage().Window.Maximize();
        }

        [TearDown]
        public void Teardown()
        {
            drv.Quit(); //close browser

        }

        [Test]
        public void UploadFileTest()
        {
            drv.Navigate().GoToUrl(baseUrl + "/corpnet/employee/myprofile.aspx");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".page-actions-wrapper span.arrow"))).Click();
            drv.FindElement(By.CssSelector("li[data-action=MainFpoQvArea_changeImage] a")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".modal .modal-footer .id-btn-save")));


            //Upload file
            var path = Path.Combine(Environment.CurrentDirectory, @"Resources", "A2.jpg");
            drv.FindElement(By.CssSelector("input[type=file]")).SendKeys(path);


            drv.FindElement(By.CssSelector(".modal .modal-footer .id-btn-save")).Click();
        }

    }
}
