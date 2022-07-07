using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace ShortUrl.DekstopTests
{
    public class DekstopTests
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string baseShortUrl = "https://shorturl.nakov.repl.co/api";
        private const string appLocation = @"C:\work\ShortURL-DesktopClient\ShortURL-DesktopClient.exe";

        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void StartApp()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }


        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_SearchUrls()
        {
            // Arrange
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(baseShortUrl);

            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click();

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            var allUrls = driver.FindElementsByAccessibilityId("ListViewSubItem");

            foreach (var searchUrl in allUrls)
            {
                if (searchUrl.Text.EndsWith("dev"))
                {
                    Assert.That(searchUrl.Text, Is.EqualTo("https://selenium.dev"));
                    break;
                }
            }

        }
        [Test]
        public void Test_AddEndSearchUrls()
        {
            // Arrange
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(baseShortUrl);

            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click();

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);


            //Act
          
            Thread.Sleep(3000);

            var addButton = driver.FindElementByAccessibilityId("buttonAdd");
            addButton.Click();

            var textBoxUrl = driver.FindElementByAccessibilityId("textBoxURL");
            textBoxUrl.SendKeys("http://escom.bg" );

            var descriptonCode = driver.FindElementByAccessibilityId("textBoxCode");
            descriptonCode.SendKeys("code" + DateTime.Now.Ticks);

            var buttonCreate = driver.FindElementByAccessibilityId("buttonCreate");
            buttonCreate.Click();

            
            Thread.Sleep(3000);

            var buttonReload = driver.FindElementByAccessibilityId("buttonReload");
            buttonReload.Click();

            Thread.Sleep(5000);
                      
            // Assert
            var searchUrl = textBoxUrl;
            var allUrls = driver.FindElementsByAccessibilityId("ListViewSubItem");
            
            foreach (var url in allUrls)
            {
             if (url.Text.EndsWith("bg"))
                {
                 Assert.That(searchUrl.Text, Is.EqualTo("http://escom.bg"));

                }
            }
            
                       

        }
        

        
    }
}