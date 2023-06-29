using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ReportTest
{
    public class ReportTests
    {
        IWebDriver drv;
        WebDriverWait wait;

        string baseUrl = Environment.GetEnvironmentVariable("ENT_QA_BASE_URL");
        string password = Environment.GetEnvironmentVariable("ENT_QA_PASS");
        string username = Environment.GetEnvironmentVariable("ENT_QA_USER");
        string company = Environment.GetEnvironmentVariable("ENT_QA_COMPANY");

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
        public void Report()
        {
            var handles = drv.WindowHandles;
            var currentHandle = drv.CurrentWindowHandle;

            drv.Navigate().GoToUrl(baseUrl + "/CorpNet/report/reportlist.aspx");
            //wait until link in the first row is clickable

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#ReportsListGrid")));
            
            var reportlink = drv.FindElement(By.XPath("//tbody/tr[1]/td[@data-column='DisplayAs']/a"));

            var reportTitle = reportlink.Text;

            wait.Until(ExpectedConditions.ElementToBeClickable(reportlink)).Click();

            var count= drv.WindowHandles.Count();

            var reportDialog = By.XPath("//*[@data-role='generatereportdialog']");
                       
            wait.Until(drv => drv.FindElement(reportDialog));
            
            var toggle = By.XPath("//div[@class='widget-row showHeaders']//span[@class='k-switch-container km-switch-container']");
            drv.FindElement(toggle).Click();

            //Click generate Report button
            drv.FindElement(By.XPath("//button[@class='btn btn-primary id-generate-button']")).Click();
            
            
            wait.Until(drv => drv.WindowHandles.Count > count);
            count = drv.WindowHandles.Count();

            drv.SwitchTo().Window(handles[count]);
            var report = drv.FindElement(By.CssSelector("table.A976b6f32f4ec47b89c3ab60935aab3b632"));

            wait.Until(ExpectedConditions.StalenessOf(report));

            Assert.That(report.Text, Is.EqualTo(reportTitle));

            drv.Close();

            drv.SwitchTo().Window(currentHandle);

            drv.FindElement(By.XPath("//button[@class='btn btn-default id-btn-close']")).Click();
        }
    }
}