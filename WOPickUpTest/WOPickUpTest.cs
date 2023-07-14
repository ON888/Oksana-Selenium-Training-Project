using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace OksanaTests
{
    public class OksanaTests
    {
        private IWebDriver drv;
        private WebDriverWait wait;
        readonly string baseUrl = Environment.GetEnvironmentVariable("ENT_QA_BASE_URL");
        readonly string password = Environment.GetEnvironmentVariable("ENT_QA_PASS");
        readonly string username = Environment.GetEnvironmentVariable("ENT_QA_USER");
        readonly string company= Environment.GetEnvironmentVariable("ENT_QA_COMPANY");

        [SetUp]
        public void Setup()
        {
           drv = new ChromeDriver(); // create instance of the Chrome browser

           wait = new WebDriverWait(drv, TimeSpan.FromSeconds(5)); // explicit waits: wait object creation
           
        }
              

        [TearDown]
        public void TearDown()
        {
            drv.Quit(); //close browser
        }

        [Test]
        public void TestWOflow()
        {
            Login();
            OpenWOListPage();
            IWebElement woLinkElement;
            string woNumber;
            
            InitData(out woLinkElement, out woNumber);

            var commentText = "Pick-Up Comment Auto";

            PickUpWO(commentText);

            VerifyWOStatusWithComment(commentText);

            CloseWOWindow();

            VerifyWOStatusChangedToOpen(woLinkElement, woNumber);

        }

        private void InitData(out IWebElement woLinkElement, out string woNumber)
        {
            // Preserve WO# with New Status that is going to be Picked Up to return to t later
            var wolink = By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a");
            woLinkElement = wait.Until(drv => drv.FindElement(wolink));
            woNumber = drv.FindElement(wolink).Text;
        }

        private void Login()
        {
            //Navigate to CE instance and login.

            drv.Navigate().GoToUrl(baseUrl + "/CorpNet/Login.aspx");
            //Selenium will automatically wait 
            //page is loaded and next command will be executed
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("username"))).SendKeys($"{username}"); //wait and enter username from env variables

            drv.FindElement(By.Id("password")).SendKeys($"{password}"); //enter password from env variables
            drv.FindElement(By.Name("_companyText")).SendKeys($"{company}");
            drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click(); //SendKeys(Keys.Enter);

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop")));// wait uuser element is visible
        }

        private void PickUpWO(string commentText)
        {
            //PickUp WO Quick View dialog with a comment
            drv.FindElement(By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a")).Click();

            // Action Pick Up with wait for element to be clickable
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@data-role='woqvdialog']//span[@title='Pick-Up']"))).Click();

            //Activate Comment area
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//form[@class='corrigo-form']/div/textarea"))).Click();

            // Type a Comment
            drv.FindElement(By.XPath("//form[@class='corrigo-form']/div/textarea")).SendKeys(commentText);

            //Close WO Quick View dialog
            drv.FindElement(By.XPath("//div[@data-role='woactionpickupeditdialog']//button[@class='btn btn-primary id-btn-save']")).Click();

        }

        private void OpenWOListPage()
        {
            //Navigate to WO List page
            drv.Navigate().GoToUrl($"{baseUrl}" + "/CorpNet/workorder/workorderlist.aspx");

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#WoListGrid")));// wait until grid is visible.
        }

        private void VerifyWOStatusChangedToOpen(IWebElement woLinkElement, string woNumber)
        {
            wait.Until(ExpectedConditions.StalenessOf(woLinkElement)); //wait until wo link is stale

            //wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#WoListGrid")));
            //wait.Until(ExpectedConditions.StalenessOf(drv.FindElement(By.CssSelector("#WoListGrid tbody"))));
            //wait.Until(drv => drv.FindElement(By.CssSelector("#WoListGrid")));

            //Check WO Staus is Opened on WO list page
            var woStatus = drv.FindElement(By.XPath("//td[@data-column='Number']/a[contains(text(), '" + woNumber + "')]/../../td[@data-column='WOStatus']"));

            Assert.That(woStatus.Text, Is.EqualTo("Open"));
        }

        private void CloseWOWindow()
        {
            //Closing the Quick View dialog
            drv.FindElement(By.XPath("//button[@class='close btn-dismiss']")).Click();
        }

        private void VerifyWOStatusWithComment(string commentText)
        {

            //Check in the Activity log that Action "Picked Up" is displayed
            var table = drv.FindElement(By.CssSelector("#WoQvActivityLogGrid tbody"));
            wait.Until(ExpectedConditions.StalenessOf(table));// wait untill table is stable/updated

            var action = drv.FindElement(By.XPath("//div[@id='WoQvActivityLogGrid']//tr[1]/td[@data-column='ActionTitle']"));
            var comment = drv.FindElement(By.XPath("//div[@id='WoQvActivityLogGrid']//tr[1]/td[@data-column='Comment']"));

            Assert.That(action.Text, Is.EqualTo("Picked Up"));

            //Check in the Activity log that Comment is displayed

            Assert.That(comment.Text, Is.EqualTo(commentText));

        }








    }
}