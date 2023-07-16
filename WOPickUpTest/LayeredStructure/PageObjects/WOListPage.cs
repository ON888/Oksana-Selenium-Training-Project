using OksanaTests.LayeredStructure.BusinessLogic;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace OksanaTests.LayeredStructure.PageObjects
{
    public class WOListPage : BasePage
    {
         
        public WOListPage(ApplicationContext context) : base(context)
        {
        }

        public void Open() 
        {
            //Navigate to WO List page
            drv.Navigate().GoToUrl(context.baseUrl + "/CorpNet/workorder/workorderlist.aspx");

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#WoListGrid")));// wait until grid is visible.

        }

        public string StatusChangeToOpen(string woNumber) 
            {
                //Check WO Staus is Opened on WO list page
                var woStatus = drv.FindElement(By.XPath("//td[@data-column='Number']/a[contains(text(), '" + woNumber + "')]/../../td[@data-column='WOStatus']"));
                var text = woStatus.Text;
                return text;  
                }
    }
}
