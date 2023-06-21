using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WOPickUpTest
{
    public class OksanaTests
    {
        IWebDriver drv;

        [SetUp]
        public void Setup()
        {
            //Login to CE instance using Environment variables

            drv = new ChromeDriver(); // create instance of the Chrome browser
            drv.Navigate().GoToUrl(Environment.GetEnvironmentVariable("ENT_QA_BASE_URL") +
                     "/CorpNet/Login.aspx"); //Navigate to CE instance
            drv.FindElement(By.Id("username")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_USER")); //enter username from env variables
            drv.FindElement(By.Id("password")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_PASS")); //enter password from env variables
            drv.FindElement(By.Name("_companyText"))
            .SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
            drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click(); //SendKeys(Keys.Enter);
            Thread.Sleep(4000);
            //Assert.True(isVisible(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop")));
        }

        [TearDown]
        public void TearDown()
        {
            drv.Quit(); //close browser
        }

        [Test]
        public void TestWOflow()
        {
            //Navigate to WO List page
            drv.Navigate().GoToUrl("https://onarequestorlt.corrigo-qa.com/corpnet/workorder/workorderlist.aspx");
            
            // Preserve WO# with New Status that is going to be Picked Up to return to t later
            var woNumber = drv.FindElement(By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a")).Text;

            //Click on New WO and open Quick View dialog
            drv.FindElement(By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a")).Click();

            // Action Pick Up
            drv.FindElement(By.XPath("//div[@data-role='woqvdialog']/div[@class='modal-body']/form/div/div/span[@title='Pick-Up']")).Click();

            //Activate Comment area
            drv.FindElement(By.XPath("//form[@class='corrigo-form']/div/textarea")).Click();

            // Type a Comment
            drv.FindElement(By.XPath("//form[@class='corrigo-form']/div/textarea")).SendKeys("Pick-Up Comment");

            //Close WO Quick View dialog
            drv.FindElement(By.XPath("//div[@data-role='woactionpickupeditdialog']/div/div/button[@class='btn btn-primary id-btn-save']")).Click();
            
            //Check in the Activity log that Action "Picked Up" is displayed
            var action = drv.FindElement(By.XPath("//div[@id='WoQvActivityLogGrid']/div[@class='k-grid k-widget k-reorderable']/table/tbody/tr/td[@data-column='ActionTitle'][contains(text(),'Picked Up')]"));
            Assert.That(action.Text, Is.EqualTo("Picked Up"));

            //Check in the Activity log that Comment is displayed
            var comment = drv.FindElement(By.XPath("//div[@id='WoQvActivityLogGrid']/div[@class='k-grid k-widget k-reorderable']/table/tbody/tr/td[@data-column='Comment'][contains(text(),'Pick-Up Comment')]"));
            Assert.That(comment.Text, Is.EqualTo("Pick-Up Comment"));

            //Closing the Quick View dialog
            drv.FindElement(By.XPath("//button[@class='close btn-dismiss']")).Click();

            //Check WO Staus is Opened on WO list page
            var woStatus=drv.FindElement(By.XPath("//td[@data-column='Number']/a[contains(text(), "+ woNumber+ ")]/../../td[@class='gc-WoListGrid-WOStatus left type-grid_string '][contains(text(), 'Open')]"));

            Assert.That(woStatus.Text, Is.EqualTo("Open"));


            Thread.Sleep(4000);
        }
    }
}