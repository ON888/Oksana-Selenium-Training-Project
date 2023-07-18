using NUnit.Framework.Internal.Execution;
using OpenQA.Selenium;
using ReportTest.LayeredStructure.PageObjects;
using ReportTest.LayeredStructureReport.BusinessLogic;
using SeleniumExtras.WaitHelpers;


namespace ReportTest.LayeredStructureReport.PageObjects
{
    public class ReportListPage : BasePage
    {
        public ReportListPage(ApplicationContext context) : base(context)
        {
           
        }

        public string OpenReportListPage()
        {
            drv.Navigate().GoToUrl(context.baseUrl + "/CorpNet/report/reportlist.aspx");
            //wait until link in the first row is clickable

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#ReportsListGrid")));

            var reportlink = drv.FindElement(By.XPath("//tbody/tr[1]/td[@data-column='DisplayAs']/a"));

            var reportTitle = reportlink.Text;

            wait.Until(ExpectedConditions.ElementToBeClickable(reportlink)).Click();

            return reportTitle;
        }

        public void GenerateFirstReport()
        {
            var reportDialog = By.XPath("//*[@data-role='generatereportdialog']");

            wait.Until(drv => drv.FindElement(reportDialog));

            var toggle = By.XPath("//div[@class='widget-row showHeaders']//span[@class='k-switch-container km-switch-container']");
            drv.FindElement(toggle).Click();

            //Click generate Report button
            drv.FindElement(By.XPath("//button[@class='btn btn-primary id-generate-button']")).Click();
        }

        public string GetReportTitle()
        {
            // get to find span with role navgation that includes Report name
            var report = drv.FindElement(By.XPath("//title"));


            wait.Until(drv => drv.FindElement(By.XPath("//title")));
            var reportText= report.GetAttribute("text");
            
            return reportText;
        }

        public void CloseGenerateReportDialog()
        {
            drv.FindElement(By.XPath("//button[@class='btn btn-default id-btn-close']")).Click();
        }

    }
}
