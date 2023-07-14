using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OksanaTests.LayeredStructure.BusinessLogic;

namespace OksanaTests.LayeredStructure.PageObjects
{
    public abstract class BasePage
    {
        public ApplicationContext context;
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
