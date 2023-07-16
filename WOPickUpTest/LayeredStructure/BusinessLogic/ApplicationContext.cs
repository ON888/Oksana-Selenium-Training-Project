using OpenQA.Selenium;

namespace OksanaTests.LayeredStructure.BusinessLogic
{
    public record ApplicationContext
    {
        public IWebDriver drv { get; set; } = null!;     
        public string baseUrl { get; set; } = null!;
        public string password { get; set; } = null!;
        public string username { get; set; } = null!;
        public string company { get; set; } = null!;


    }
}
