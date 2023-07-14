using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using OksanaTests.LayeredStructure.BusinessLogic;
using OksanaTests.LayeredStructure.PageObjects;

namespace OksanaTests.LayeredStructure
{
    public class CEApplication
    {
        //private readonly IWebDriver drv;
        private readonly WebDriverWait _wait;

        private readonly ApplicationContext _context;
        private readonly LoginPage _loginPage;
        private readonly WOPage _woPage;
        private readonly WOListPage _woListPage;


        By wolink;
        IWebElement woLinkElement;
        string woNumber;

        public CEApplication()
        {
            _context = new ApplicationContext();

            var opt = new ChromeOptions();
            opt.AddArguments("start-maximized");

            var drv = new ChromeDriver(opt);
            _wait = new WebDriverWait(drv, TimeSpan.FromSeconds(5)); // explicit waits: wait object creation


            _context.drv = drv;
            _context.baseUrl = Environment.GetEnvironmentVariable("ENT_QA_BASE_URL");
            _context.password = Environment.GetEnvironmentVariable("ENT_QA_PASS");
            _context.username = Environment.GetEnvironmentVariable("ENT_QA_USER");
            _context.company = Environment.GetEnvironmentVariable("ENT_QA_COMPANY");

            _loginPage = new LoginPage(_context);
            _woPage = new WOPage(_context);
            _woListPage = new WOListPage(_context);


            // Preserve WO# with New Status that is going to be Picked Up to return to t later
            
        }

        //private void InitData(out IWebElement woLinkElement, out string woNumber)
        //{
        //   // Preserve WO# with New Status that is going to be Picked Up to return to t later
        //    var wolink = By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a");
        //    woLinkElement = wait.Until(drv => drv.FindElement(wolink));
        //    woNumber = drv.FindElement(wolink).Text;
        //}
        public void Login()
        {
            _loginPage.LoginwithDefaultUser();

        }



        public void OpenWOListPage()
        {
            _woListPage.Open();
        }

        public void PickUpWO(string _commentText)
        {
            _woPage.PickUp();
        }

        public Tuple <string, string> VerifyWOStatusWithComment()  
        {
            return _woPage.VerifyComment();
        }

        public string VerifyWOStatusChangedToOpen()
        {
            return _woListPage.StatusChangeToOpen();
            
        }

        public void CloseWOWindow()
        {
            _woPage.ClosedQVDialog();
        }

        public void CloseApp()
        {
            _context.drv.Quit(); //close browser
        }

    }
}


