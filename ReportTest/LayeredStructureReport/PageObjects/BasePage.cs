using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ReportTest.LayeredStructureReport.BusinessLogic;

namespace ReportTest.LayeredStructure.PageObjects
{
    public abstract class BasePage
    {
        public ApplicationContext context;
        private ApplicationContext context1;
        public readonly IWebDriver drv;
        public readonly WebDriverWait wait;

        protected BasePage (ApplicationContext context)
        {
            this.context = context;
            drv = context.drv;
            wait = new WebDriverWait(drv, TimeSpan.FromSeconds(10));
        }

        
    }
}
