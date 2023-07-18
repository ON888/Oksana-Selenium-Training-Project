using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using ReportTest.LayeredStructureReport.BusinessLogic;
using System.Collections.ObjectModel;

namespace ReportTest.LayeredStructureReport
{
    public class ReportOnlyTest 
        //: Visible
    {
       //public ReportOnlyTest(ApplicationContext context) : base(context)
        //{
        //}

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            app = new Application();
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            app.CloseApp();

        }
        private Application app;


        [Test]
        public void ReportTest()
        {
            app.Login();

            app.GetWindowHandles();

            var currentHandle = app.GetCurrentHandle();       
            var count = app.GetWindowHandles().Count();

            var reportTitle = app.ReportListPage();

            app.GererateReport();

            var handles = app.GetWindowHandles();

            //wait.Until(drv => app.GetWindowHandles().Count > count);

            app.SwitchToNewTab(handles, count);

            
            Assert.That(app.IsVisible(By.XPath($"//span[contains(text(),'{reportTitle}')]")), Is.True);
                          
            Assert.True(app.CheckReportTitle().Contains(reportTitle));

            

            app.SwitchToReportListPageTab(currentHandle);
            app.CloseReport();

            app.CloseApp();
        }

        
    }
}
