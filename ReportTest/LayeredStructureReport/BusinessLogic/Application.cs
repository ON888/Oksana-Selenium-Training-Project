using ReportTest.LayeredStructure.PageObjects;
using OpenQA.Selenium.Chrome;
using ReportTest.LayeredStructureReport.PageObjects;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace ReportTest.LayeredStructureReport.BusinessLogic
{
    public class Application
    {
        private readonly ApplicationContext _context;
        private readonly LoginPage _loginPage;
        private readonly ReportListPage _reportPage;

        public Application()
        {
            var drv = new ChromeDriver();

            _context = new ApplicationContext
            {
                drv = drv,
                baseUrl = Environment.GetEnvironmentVariable("ENT_QA_BASE_URL")!,
                username = Environment.GetEnvironmentVariable("ENT_QA_USER")!,
                password = Environment.GetEnvironmentVariable("ENT_QA_PASS")!,
                company = Environment.GetEnvironmentVariable("ENT_QA_COMPANY")!
            };

            _loginPage = new LoginPage(_context);
            _reportPage = new ReportListPage(_context);
        }

        public void CloseApp()
        {
            _context.drv.Quit(); //close browser
        }


        public Application Login()
        {
            _loginPage.LoginwithDefaultUser();
            return this;
        }

        public Application ReportListPage()
        {
            _reportPage.OpenReportListPage();
            return this;
        }

        public Application GererateReport()
        {
            _reportPage.GenerateFirstReport();
            return this;
        }

        public string GetCurrentHandle()
        {
            
            return _context.drv.CurrentWindowHandle;
        }

        public ReadOnlyCollection<string> GetWindowHandles()
        {
            return _context.drv.WindowHandles;
        }

        public Application SwitchToNewTab(ReadOnlyCollection<string> handles, int count)
        {
            //Switching to a new window
            _context.drv.SwitchTo().Window(handles[count]);
            return this;

        }


        public Application SwitchToReportListPageTab(string currentHandle)
        {
            _context.drv.SwitchTo().Window(currentHandle);
            return this;
        }

        public Application CheckReportTitle()
        {
            _reportPage.GetReportTitle();
            return this;
        }

        public Application CloseReport()
        {
            _reportPage.CloseGenerateReportDialog();
            return this;
        }

    }

}
