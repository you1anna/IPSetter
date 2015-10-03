using System;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace RouterTestFramework
{
    public static class Browser
    {
        static IWebDriver webDriver = new FirefoxDriver();

        public static string Title
        {
            get { return webDriver.Title; }
        }

        public static ISearchContext Driver
        {
            get { return webDriver; }
        }

        public static void GoTo(string url)
        {
           webDriver.Url = url;
        }

        public static string getUrl()
        {
            string url = webDriver.Url;
            return url;
        }

        public static void Close()
        {
            webDriver.Close();
        }

        public static IWebElement findDynamicElement(By by, int timeOut)
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOut));
            IWebElement element = webDriver.FindElement(by);

            //IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(by)); 
            return element;
        }

        public static void timeOut(int seconds)
        {
            webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(seconds));
        }

        public static void switchFrame(string frame)
        {
            webDriver.SwitchTo().Frame(frame);
        }

        public static void WaitForElementVisible(By by, int timeOutInSeconds)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(by));
            }
            catch (Exception)
            {
                Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed.Seconds);
            }
            finally
            {
                stopwatch.Stop();
            }
        }
    }
}