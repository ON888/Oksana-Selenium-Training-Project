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
    public class WOPage : BasePage
    {
        string commentText = "Pick-Up Comment Auto";
        
        public WOPage(ApplicationContext context) : base(context)
        {
        
        }

        public string PickUp()
        {
            //PickUp WO Quick View dialog with a comment
            By wolink = By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a");
                     
            IWebElement woLinkElement = wait.Until(drv => drv.FindElement(wolink));
            string woNumber = drv.FindElement(wolink).Text;

            drv.FindElement(wolink).Click();

            // Action Pick Up with wait for element to be clickable
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@data-role='woqvdialog']//span[@title='Pick-Up']"))).Click();

            //Activate Comment area
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//form[@class='corrigo-form']/div/textarea"))).Click();

            // Type a Comment
            drv.FindElement(By.XPath("//form[@class='corrigo-form']/div/textarea")).SendKeys(commentText);

            //Close WO Quick View dialog
            drv.FindElement(By.XPath("//div[@data-role='woactionpickupeditdialog']//button[@class='btn btn-primary id-btn-save']")).Click();
            return woNumber;

        }
        public Tuple<string, string> VerifyComment()
        {
            var table = drv.FindElement(By.CssSelector("#WoQvActivityLogGrid tbody"));
            wait.Until(ExpectedConditions.StalenessOf(table));// wait untill table is stable/updated

            var action = drv.FindElement(By.XPath("//div[@id='WoQvActivityLogGrid']//tr[1]/td[@data-column='ActionTitle']"));
            var comment = drv.FindElement(By.XPath("//div[@id='WoQvActivityLogGrid']//tr[1]/td[@data-column='Comment']"));
            
            return Tuple.Create(action.Text, comment.Text);
        }

        public void ClosedQVDialog()
        {
            //Closing the Quick View dialog
            drv.FindElement(By.XPath("//button[@class='close btn-dismiss']")).Click();
        }
    }
}
