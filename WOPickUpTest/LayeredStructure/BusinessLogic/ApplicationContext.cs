using OpenQA.Selenium;

namespace OksanaTests.LayeredStructure.BusinessLogic
{
    public record ApplicationContext
    {
        public IWebDriver drv { get; set; }        
        public string baseUrl { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        public string company { get; set; }


    }
}
