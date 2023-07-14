using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public string StatusChangeToOpen() 
            {
                By wolink = By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a");
                IWebElement woLinkElement = wait.Until(drv => drv.FindElement(wolink));
                string woNumber = drv.FindElement(wolink).Text;

                wait.Until(ExpectedConditions.StalenessOf(woLinkElement)); //wait until wo link is stale

                //Check WO Staus is Opened on WO list page
                var woStatus = drv.FindElement(By.XPath("//td[@data-column='Number']/a[contains(text(), '" + woNumber + "')]/../../td[@data-column='WOStatus']"));
                var text = woStatus.Text;
                return text;  
                }
    }
}
