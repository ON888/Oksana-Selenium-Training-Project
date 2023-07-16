using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
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


        //By wolink;
        //IWebElement woLinkElement;
        string woNumber;

        public CEApplication()
        {

            var opt = new ChromeOptions();
            opt.AddArguments("start-maximized");

            var drv = new ChromeDriver(opt);
            _wait = new WebDriverWait(drv, TimeSpan.FromSeconds(5)); // explicit waits: wait object creation

            _context = new ApplicationContext()
            {
                drv = drv,
                baseUrl = Environment.GetEnvironmentVariable("ENT_QA_BASE_URL"),
                password = Environment.GetEnvironmentVariable("ENT_QA_PASS"),
                username = Environment.GetEnvironmentVariable("ENT_QA_USER"),
                company = Environment.GetEnvironmentVariable("ENT_QA_COMPANY")!

            };  
                      

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
        public CEApplication Login()
        {
            _loginPage.LoginwithDefaultUser();
            return this;
        }



        public CEApplication OpenWOListPage()
        {
            _woListPage.Open();
            return this;    
        }

        public CEApplication PickUpWO(string _commentText)
        {
            woNumber = _woPage.PickUp();
            
            return this;    
        }

        public Tuple <string, string> VerifyWOStatusWithComment()  
        {
            return _woPage.VerifyComment();
        }

        public string VerifyWOStatusChangedToOpen()
        {
            
            return _woListPage.StatusChangeToOpen(woNumber);
            
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


