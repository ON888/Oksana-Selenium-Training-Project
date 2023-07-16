using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using OksanaTests.LayeredStructure.BusinessLogic;

namespace OksanaTests.LayeredStructure
{
    public class PickupWOTest

    {
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        { 
            app = new CEApplication();  
                
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() 
        {
            app.CloseApp();       
        }
        private CEApplication app;

        [Test]
        public void PickupWO()
        {
            const string commentText = "Pick-Up Comment Auto";
            
            //var text = app.VerifyWOStatusChangedToOpen();

            app.Login();
            app.OpenWOListPage();
            app.PickUpWO(commentText);

            var ret = app.VerifyWOStatusWithComment();

            //Check in the Activity log that Action "Picked Up" is displayed with Comment

            Assert.Multiple(() =>
            {
                Assert.That(ret.Item1, Is.EqualTo("Picked Up"));
                Assert.That(ret.Item2, Is.EqualTo(commentText));
            });
            
            app.CloseWOWindow();
            

            Assert.That(app.VerifyWOStatusChangedToOpen(), Is.EqualTo("Open"));
        }
    }
}
