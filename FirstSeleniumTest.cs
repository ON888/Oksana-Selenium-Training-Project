using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace Oksana_Selenium_Training_Project
{
    public class Tests
    {
        IWebDriver drvChrome;
        IWebDriver drvFirefox;
        IWebDriver drvEdge;

        [SetUp]
        public void Setup()
        {
            drvChrome = new ChromeDriver(); // create instance of the Chrome browser
            drvFirefox = new FirefoxDriver(); // create instance of the Firefox browser
            drvEdge = new EdgeDriver(); // create instance of the Edge browser
        }

        [TearDown]
        public void TearDown() 
        { 
            drvChrome.Quit(); //close browser
            drvFirefox.Quit(); //close browser
            drvEdge.Quit(); //close browser
        }

        [Test]
        public void Test_Chrome()
        {
            drvChrome.Navigate().GoToUrl("https://google.com");
            drvChrome.FindElement(By.CssSelector("[name=q]")).SendKeys("Selenium");

            //Assert.Pass();
        }
        [Test]
        public void Test_Firefox()
        {
            drvFirefox.Navigate().GoToUrl("https://google.com");
            drvFirefox.FindElement(By.CssSelector("[name=q]")).SendKeys("Selenium");

            //Assert.Pass();
        }
        [Test]
        public void Test_Edge()
        {
            drvEdge.Navigate().GoToUrl("https://google.com");
            drvEdge.FindElement(By.CssSelector("[name=q]")).SendKeys("Selenium");

            //Assert.Pass();
        }
    }
}