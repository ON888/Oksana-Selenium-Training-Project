using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace OksanaTests.LayeredStructure
{
    public class PickupWOTest

    {
        private CEApplication app;

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

        [Test]
        public void PickupWO()
        {
            var commentText = "Pick-Up Comment Auto";
            var ret = app.VerifyWOStatusWithComment();
            //var text = app.VerifyWOStatusChangedToOpen();

            app.Login();
            app.OpenWOListPage();
            app.PickUpWO(commentText);
            
            //Check in the Activity log that Action "Picked Up" is displayed with Comment

            Assert.Multiple(() =>
            {
                Assert.That(ret.Item1, Is.EqualTo("Picked Up"));
                Assert.That(ret.Item2, Is.EqualTo(commentText));
            });
            
            app.CloseWOWindow();
            
            app.VerifyWOStatusChangedToOpen();

            Assert.That(app.VerifyWOStatusChangedToOpen(), Is.EqualTo("Open"));
        }
    }
}
