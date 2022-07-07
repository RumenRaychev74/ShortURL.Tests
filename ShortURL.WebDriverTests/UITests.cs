using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace ShortURL.WebDriverTests
{
    public class UITests
    {
        private const string url = "https://shorturl.nakov.repl.co";
        private WebDriver driver;

        [OneTimeSetUp]
        public void OpenBrowser()
        {
            this.driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [OneTimeTearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }
        [Test]
        public void Test_CheckUrlsList()
        {
            //Arrenge
            driver.Navigate().GoToUrl(url);
            var contactLink = driver.FindElement(By.LinkText("Short URLs"));

            //Act
            contactLink.Click();

            //Assert
            var linkName = driver.FindElement(By.CssSelector(".urls > tbody:nth-child(2) > tr:nth-child(1) > td:nth-child(1) > a:nth-child(1)"));
            Assert.AreEqual("https://nakov.com", linkName.Text);


        }
        [Test]
        public void Test_AddUrl_ValidData()
        {
            //Arrenge
            driver.Navigate().GoToUrl(url);
            var headerLink = driver.FindElement(By.LinkText("Add URL"));
            headerLink.Click();
            var textBoxUrl = driver.FindElement(By.Id("url"));
            var textBoxCode = driver.FindElement(By.Id("code"));
            var buttonCreate = driver.FindElement(By.CssSelector("form button"));

            //Act
            var newUrl = "http://testurl" + DateTime.Now.Ticks + ".com/";
            var newCode =  "code" + DateTime.Now.Ticks;
            textBoxUrl.SendKeys(newUrl);
            textBoxCode.SendKeys(newCode);
            buttonCreate.Click();

            //Assert
            var newUrlLink = driver.FindElement(By.LinkText(newUrl));
            Assert.AreEqual(newUrl, newUrlLink.GetAttribute("href"));
        }
        [Test]
        public void Test_AddUrl_InvalidData()
        {
            //Arrenge
            driver.Navigate().GoToUrl(url);
            var headerLink = driver.FindElement(By.LinkText("Add URL"));
            headerLink.Click();
            var textBoxUrl = driver.FindElement(By.Id("url"));
            var textBoxCode = driver.FindElement(By.Id("code"));
            var buttonCreate = driver.FindElement(By.CssSelector("form button"));

            //Act
            var newUrl = "invalid Url";
            var newCode = "invalid code";
            textBoxUrl.SendKeys(newUrl);
            textBoxCode.SendKeys(newCode);
            buttonCreate.Click();

            //Assert
            var divErr = driver.FindElement(By.CssSelector(".err"));
            Assert.AreEqual("Invalid URL!",divErr.Text);
        }
        [Test]
        public void Test_Visit_InvalidUrl()
        {
            //Arrenge

            //Act
            driver.Navigate().GoToUrl("http://shorturl.nakov.repl.co/go/invalid536524");

            //Assert
            var errMsg = driver.FindElement(By.CssSelector(".err"));
            Assert.AreEqual("Cannot navigate to given short URL", errMsg.Text);
        }
        [Test]
        public void Test_VisitUrl()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);

            var shortURLs_button = driver.FindElement(By.LinkText("Short URLs"));
            shortURLs_button.Click();
            //Act
            var firstVisitors = driver.FindElement(By.CssSelector("body > main > table > tbody > tr:nth-child(1) > td:nth-child(4)")).Text;
            var numberFirstVisitors = firstVisitors;
            var shortUrl = driver.FindElement(By.ClassName("shorturl"));
            shortUrl.Click();
            //Assert
            driver.SwitchTo().Window(driver.WindowHandles[0]);
            Thread.Sleep(3000);
            var lastVisitors = driver.FindElement(By.CssSelector("body > main > table > tbody > tr:nth-child(1) > td:nth-child(4)")).Text;
            var numberLastVisitors = lastVisitors;
            Thread.Sleep(2000);
            Assert.That(int.Parse(numberLastVisitors), Is.GreaterThan(int.Parse(numberFirstVisitors)));
        }
      
    }
}