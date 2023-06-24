using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace WOPickUpTest
{
    public class OksanaTests
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
            //Login to CE instance using Environment variables

            drv = new ChromeDriver(); // create instance of the Chrome browser

            wait = new WebDriverWait(drv, TimeSpan.FromSeconds(10));

            drv.Navigate().GoToUrl(baseUrl + "/CorpNet/Login.aspx"); //Navigate to CE instance
            //wait.Until(ExpectedConditions.ElementIsVisible(By.Name("#username")));

            drv.FindElement(By.Id("username")).SendKeys($"{username}"); //enter username from env variables
            drv.FindElement(By.Id("password")).SendKeys($"{password}"); //enter password from env variables
            drv.FindElement(By.Name("_companyText")).SendKeys($"{companyName}");
            drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click(); //SendKeys(Keys.Enter);

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop")));// wait uuser element is visible

        }    

        [TearDown]
        public void TearDown()
        {
            drv.Quit(); //close browser
        }

        private bool isVisible(By locator)
        {
            return drv.FindElements(locator).Count > 0;
        }
        private bool IsElementPresent(By locator)
        {
            return drv.FindElements(locator).Count > 0;
        }

        [Test]
        public void TestWOflow()
        {
            //Navigate to WO List page
            drv.Navigate().GoToUrl($"{baseUrl }" + "/CorpNet/workorder/workorderlist.aspx");
            
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#WoListGrid")));
            
                // Preserve WO# with New Status that is going to be Picked Up to return to t later
                var woNumber = drv.FindElement(By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a")).Text;
                var commentText = "Pick-Up Comment Auto";
                //Click on New WO and open Quick View dialog
                drv.FindElement(By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a")).Click();

                // Action Pick Up
                drv.FindElement(By.XPath("//div[@data-role='woqvdialog']/div[@class='modal-body']//*[@title='Pick-Up']")).Click();

                //Activate Comment area
                drv.FindElement(By.XPath("//form[@class='corrigo-form']/div/textarea")).Click();

                // Type a Comment
                drv.FindElement(By.XPath("//form[@class='corrigo-form']/div/textarea")).SendKeys(commentText);

                //Close WO Quick View dialog
                drv.FindElement(By.XPath("//div[@data-role='woactionpickupeditdialog']//button[@class='btn btn-primary id-btn-save']")).Click();

            
            //Check in the Activity log that Action "Picked Up" is displayed
            var action = drv.FindElement(By.XPath("//div[@id='WoQvActivityLogGrid']//tr/td[@data-column='ActionTitle'][contains(text(),'Picked Up')]"));
            Assert.That(action.Text, Is.EqualTo("Picked Up"));

            //Check in the Activity log that Comment is displayed
            var comment = drv.FindElement(By.XPath("//div[@id='WoQvActivityLogGrid']//tr/td[@data-column='Comment'][contains(text(),'Pick-Up')]"));
            Assert.That(comment.Text, Is.EqualTo(commentText));

            //Closing the Quick View dialog
            drv.FindElement(By.XPath("//button[@class='close btn-dismiss']")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#WoListGrid")));

            //Check WO Staus is Opened on WO list page
            var woStatus = drv.FindElement(By.XPath("//td[@data-column='Number']/a[contains(text(), $'{woNumber}')]/../../td[@data-column='WOStatus']"));

            Assert.That(woStatus.Text, Is.EqualTo("Open"));
                       
        }
            
    }
}