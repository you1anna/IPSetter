
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Keys = OpenQA.Selenium.Keys;

namespace RouterTestFramework
{
    public static class Pages
    {
        public static string _currentIP;
        public static string _finalIP;

        public static HomePage HomePage
        {
            get
            {
                var homePage = new HomePage();
                PageFactory.InitElements(Browser.Driver, homePage);
                return homePage;
            }
        }
        public static AdvPage AdvPage
        {
            get
            {
                var advPage = new AdvPage();
                PageFactory.InitElements(Browser.Driver, advPage);
                return advPage;
            }
        }

        public static PortPage PortPage
        {
            get
            {
                var portPage = new PortPage();
                PageFactory.InitElements(Browser.Driver, portPage);
                return portPage;
            }
        }
        public static ActionPage ActionPage
        {
            get
            {
                var actionPage = new ActionPage();
                PageFactory.InitElements(Browser.Driver, actionPage);
                return actionPage;
            }
        }
    }

    public class ActionPage
    {
        public string getUrl()
        {
            string url = Browser.getUrl();
            return url;       
        }
        public void editIPField(string ip)
        {
            var select = Browser.Driver.FindElement(By.Name("fwin_action"));
            select.Click();
            var option = select.FindElements(By.TagName("option"))[0];
            option.SendKeys(Keys.Tab);
            var ipField1 = Browser.Driver.FindElement(By.Name("lan_start_ip4"));
            Pages._currentIP = ipField1.GetAttribute("value");
            Pages._currentIP = "192.168.0." + Pages._currentIP;
            ipField1.Click();
            ipField1.Clear();
            ipField1.SendKeys(ip);

            var applyBtn = Browser.Driver.FindElement(By.Name("Apply"));
            applyBtn.Click();

            //Actions action = new Actions((IWebDriver)Browser.Driver);
            //var field = action.MoveToElement(ipField1);
            //field.Click().Perform();
            //field.SendKeys(ip);
            //ipField3.Clear();
        }

        public void getNewIPFromWebpage()
        {
            var result = Browser.Driver.FindElement(By.XPath("/html/body/form/div[3]/table/tbody/tr[5]/td/table/tbody/tr[2]/td[6]/b"));
            Pages._finalIP = result.Text;
        }
    }
    public class AdvPage
    {
        public static string advUrl = "http://192.168.0.1/adv_index.htm";

        public void GoTo()
        {
            Browser.GoTo(advUrl);
        }
        public void waitClickAdvancedSetup()
        {
            IWebElement element = Browser.Driver.FindElement(By.XPath("//div[@id='advanced_sub']"));
            var executor = (IJavaScriptExecutor)Browser.Driver;
            executor.ExecuteScript("arguments[0].style.display='block';", element);

            Browser.Driver.FindElement(By.LinkText("Port Forwarding / Port Triggering")).Click();

            /* WebDriverWait wait = new WebDriverWait((IWebDriver)Browser.Driver, TimeSpan.FromSeconds(5));
            var advLink = wait.Until(d =>
            {
                return Browser.Driver.FindElement(By.XPath("//div[@id='advanced_bt']//span[.='Advanced Setup']"));
            });
            */  

        }
        public void clickPortForward()
        {
            var button = Browser.findDynamicElement(By.XPath("//dt[@id='forwarding_triggering']/a/b/span"), 10);
            button.Click();
        }
    }

        public class PortPage
        {
            public static string advUrl = "http://admin:password@192.168.0.1/FW_forward.htm";

            [FindsBy(How = How.Name, Using = "in_sel")]
            private IWebElement selectRadio;

            [FindsBy(How = How.ClassName, Using = "edit_bt")]
            private IWebElement editButton;

            public void GoTo()
            {
                Browser.GoTo(advUrl);
            }

            public void selectRadioBtn()
            {        
                selectRadio.Click();
            }

            public void clickEdit()
            {
                editButton.Click();
            }
        }
        public class HomePage
        {
            public static string Url = "http://192.168.0.1";
            private static string PageTitle = "NETGEAR Router DGN2200v3";

            [FindsBy(How = How.CssSelector, Using = "html body form div#labels div#advanced_label.label_unclick b span")]
            private IWebElement advancedLink;

            public void GoTo()
            {
                Browser.GoTo(Url);
            }

            public bool IsAt()
            {
                return Browser.Title == PageTitle;
            }

            public void ClickAdvanced()
            {
                advancedLink.Click();
            }
        }

}
