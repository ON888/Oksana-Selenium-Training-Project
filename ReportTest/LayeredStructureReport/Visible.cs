using OpenQA.Selenium;
using ReportTest.LayeredStructure.PageObjects;
using ReportTest.LayeredStructureReport.BusinessLogic;

namespace ReportTest.LayeredStructureReport
{
    public class Visible : BasePage
    {
        public Visible(ApplicationContext context) : base(context)
        {
        }

        public bool IsVisible(By locator)
        {
            return drv.FindElements(locator).Count > 0;
        }
    }
}