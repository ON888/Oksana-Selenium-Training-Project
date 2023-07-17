using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using ReportTest.LayeredStructureReport.BusinessLogic;

namespace ReportTest.LayeredStructure.PageObjects
{
    public class LoginPage : BasePage
    {
        private By usernameId = By.Name("username");
        private By passwordId = By.Id("password");
        private By companyId = By.Name("_companyText");
        private By loginBtn = By.CssSelector("input.btn.login-submit-button");
        private By menuUser = By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop");


        public LoginPage(ApplicationContext context) : base(context)
        {
        }

        public void LoginwithDefaultUser()
        {
          //Navigate to CE instance and login.

            drv.Navigate().GoToUrl(context.baseUrl + "/CorpNet/Login.aspx");
            wait.Until(ExpectedConditions.ElementToBeClickable(usernameId)).SendKeys(context.username); //wait and enter username from env variables
            drv.FindElement(passwordId).SendKeys(context.password);
            drv.FindElement(companyId).SendKeys(context.company);
            drv.FindElement(loginBtn).Click();

          // wait uuser element is visible
            wait.Until(
                ExpectedConditions.ElementIsVisible(menuUser));
        }
    }
}
