using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using ReportTest.LayeredStructureReport.BusinessLogic;
using System.Collections.ObjectModel;

namespace ReportTest.LayeredStructureReport
{
    public class ReportOnlyTest : Visible
    {
        private Application app;

        public ReportOnlyTest(ApplicationContext context) : base(context)
        {
        }

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

        [Test]
        public void ReportTest()
        {
            app.Login();

            var handles = app.GetWindowHandles();
            var currentHandle = app.GetCurrentHandle();

            var reportTitle = app.ReportListPage();

            var count = app.GetWindowHandles().Count();

            app.GererateReport();

            handles = app.GetWindowHandles();

            wait.Until(drv => app.GetWindowHandles().Count > count);

            app.SwitchToNewTab(handles, count);
            //app.CheckReportTitle();

            Assert.That(IsVisible(By.XPath($"//span[contains(text(),'{reportTitle}')]")), Is.True);

            Assert.That(app.CheckReportTitle(), Is.EqualTo(reportTitle));

            

            app.SwitchToReportListPageTab(currentHandle);
            app.CloseReport();

            app.CloseApp();
        }



    }
}
