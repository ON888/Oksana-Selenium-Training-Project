using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WOPickUpTest
{
    public class Tests
    {
        IWebDriver drvChrome;

        [SetUp]
        public void Setup()
        {
            //Login to Ona Requestor LT instance using Environment variables

            drvChrome = new ChromeDriver(); // create instance of the Chrome browser
            drvChrome.Navigate().GoToUrl(Environment.GetEnvironmentVariable("CEURL")); //Navigate to CE instance
            drvChrome.FindElement(By.Id("username")).SendKeys(Environment.GetEnvironmentVariable("CEusername")); //enter username from env variables
            drvChrome.FindElement(By.Id("password")).SendKeys(Environment.GetEnvironmentVariable("CEpassword")); //enter password from env variables
            drvChrome.FindElement(By.CssSelector("input.btn.login-submit-button")).Click(); //SendKeys(Keys.Enter);
            Thread.Sleep(4000);
        }

        [TearDown]
        public void TearDown()
        {
            drvChrome.Quit(); //close browser
        }


        [Test]
        public void TestLogin()
        {
            //User verification

            var user = drvChrome.FindElement(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop")).GetAttribute("text");
            Assert.That(user, Is.EqualTo("Oksana Nahibina"));

        }
        [Test]
        public void TestWOflow()
        {
            //Navigate to WO List page
            drvChrome.Navigate().GoToUrl("https://onarequestorlt.corrigo-qa.com/corpnet/workorder/workorderlist.aspx");
            
            // Preserve WO# with New Status that is going to be Picked Up to return to t later
            var woNumber = drvChrome.FindElement(By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a")).Text;

            //Click on New WO and open Quick View dialog
            drvChrome.FindElement(By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a")).Click();

            // Action Pick Up
            drvChrome.FindElement(By.XPath("//div[@data-role='woqvdialog']/div[@class='modal-body']/form/div/div/span[@title='Pick-Up']")).Click();

            //Activate Comment area
            drvChrome.FindElement(By.XPath("//form[@class='corrigo-form']/div/textarea")).Click();

            // Type a Comment
            drvChrome.FindElement(By.XPath("//form[@class='corrigo-form']/div/textarea")).SendKeys("Pick-Up Comment");

            //Close WO Quick View dialog
            drvChrome.FindElement(By.XPath("//div[@data-role='woactionpickupeditdialog']/div/div/button[@class='btn btn-primary id-btn-save']")).Click();
            
            //Check in the Activity log that Action "Picked Up" is displayed
            var action = drvChrome.FindElement(By.XPath("//div[@id='WoQvActivityLogGrid']/div[@class='k-grid k-widget k-reorderable']/table/tbody/tr/td[@data-column='ActionTitle'][contains(text(),'Picked Up')]"));
            Assert.That(action.Text, Is.EqualTo("Picked Up"));

            //Check in the Activity log that Comment is displayed
            var comment = drvChrome.FindElement(By.XPath("//div[@id='WoQvActivityLogGrid']/div[@class='k-grid k-widget k-reorderable']/table/tbody/tr/td[@data-column='Comment'][contains(text(),'Pick-Up Comment')]"));
            Assert.That(comment.Text, Is.EqualTo("Pick-Up Comment"));

            //Closing the Quick View dialog
            drvChrome.FindElement(By.XPath("//button[@class='close btn-dismiss']")).Click();

            //Check WO Staus is Opened on WO list page
            var woStatus=drvChrome.FindElement(By.XPath("//td[@data-column='Number']/a[contains(text(), "+ woNumber+ ")]/../../td[@class='gc-WoListGrid-WOStatus left type-grid_string '][contains(text(), 'Open')]"));

            Assert.That(woStatus.Text, Is.EqualTo("Open"));


            Thread.Sleep(4000);
        }
    }
}