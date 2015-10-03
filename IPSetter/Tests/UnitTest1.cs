using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouterTestFramework;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void CanGoToHomePage()
        {
            Pages.HomePage.GoTo();
            Assert.IsTrue(Pages.HomePage.IsAt());
        }

        [TestMethod]
        public void CanGoToPortForwarding()
        {
           Pages.PortPage.GoTo();
           Pages.PortPage.selectRadioBtn();
           Pages.PortPage.clickEdit();
           Browser.timeOut(5);
           
           Pages.ActionPage.editIPField("2");

           //Pages.HomePage.GoTo();
           //Browser.timeOut(5);
           //Browser.WaitForElementVisible();
           //Pages.HomePage.waitClickAdvanced();
           //Pages.AdvPage.GoTo();
           //Pages.AdvPage.waitClickAdvancedSetup();
        }
        

        [TestCleanup]
        public void CleanUp()
        {
            //Browser.Close();
        }
    }
}
