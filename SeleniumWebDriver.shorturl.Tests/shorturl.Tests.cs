using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Support.UI;
using System.ComponentModel;
using NUnit.Framework;

namespace shorturl.Tests
{
    public class SeleniumTests
    {
        private WebDriver driver;
        //private const string baseUrl = "https://shorturl.damiant4.repl.co/";
        private const string baseUrl = "https://ffc0118f-7304-4d78-aa9e-c1e737c17d20-00-2xqb128crqg88.riker.replit.dev/";
        private readonly string dateTime;

        [SetUp]
        public void OpenBrouser()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Url = baseUrl;

        }

        [TearDown]
        public void CloseBrouser()
        {
            driver.Dispose();
        }


        [Test]
        public void Test_Shorturl_Check_Title()
        {
            var pageTitle = driver.Title;

            Assert.That("URL Shortener", Is.EqualTo(pageTitle));
        }

        [Test]
        public void Test_Shorturl_Table_Top_Left_Cell_By_XPath()
        {
            //Act
            var linckShortURL = driver.FindElement(By.XPath("//a[contains(.,'Short URLs')]"));
            linckShortURL.Click();
            var pageTitle = driver.Title;
            var FirstTableCell = driver.FindElement(By.XPath("//th[contains(.,'Original URL')]"));

            //Assert
            Assert.That("Short URLs", Is.EqualTo(pageTitle));
            Assert.That(FirstTableCell.Text, Is.EqualTo("Original URL"));
        }

        [Test]
        public void Test_Shorturl_Table_Top_Left_Cell_By_CssSelector()
        {
            //Act
            var linckShortURL = driver.FindElement(By.LinkText("Short URLs"));
            linckShortURL.Click();
            var pageTitle = driver.Title;
            var tableHeaderLeftCell = driver.FindElement(By.CssSelector("th:nth-child(1)"));

            //Assert
            Assert.That("Short URLs", Is.EqualTo(pageTitle));
            Assert.That(tableHeaderLeftCell.Text, Is.EqualTo("Original URL"));
        }

        [Test]
        public void Test_Shorturl_Add_URL_Valid()
        // if not cleared the database will fail !!!
        {
            //Act
            var LinckAddURL = driver.FindElement(By.LinkText("Add URL"));
            LinckAddURL.Click();
            var inputURL = driver.FindElement(By.Id("url"));
            inputURL.SendKeys("https://cnn.com");
            var inputShortCode = driver.FindElement(By.Id("code"));
            inputShortCode.SendKeys(Keys.Control + "a");
            inputShortCode.SendKeys("cnn");
            var buttonCreate = driver.FindElement(By.XPath("//button[@type='submit']"));
            buttonCreate.Click();
            var pageTitle = driver.Title;
            var LinckOriginalURL = driver.FindElement(By.XPath("(//a[@href='https://cnn.com'])[1]"));

            //Assert
            Assert.That("Short URLs", Is.EqualTo(pageTitle));
            Assert.That(LinckOriginalURL.Text, Is.EqualTo("https://cnn.com"));
        }

        [Test]
        public void Test_Shorturl_Add_Valid_URL()
        {
            //Act
            var urlToAdd = "http://url" + DateTime.Now.Ticks + ".com";

            var linckAddUrl = driver.FindElement(By.LinkText("Add URL"));
            linckAddUrl.Click();
            var inputAddUrl = driver.FindElement(By.Id("url"));
            inputAddUrl.SendKeys(urlToAdd);
            var buttonCreate = driver.FindElement(By.XPath("//button[@type='submit']"));
            buttonCreate.Click();

            //Assert
            //Asertion for the URL in the Page Source
            Assert.That(driver.PageSource.Contains(urlToAdd));

            //Asertion for the URL in the Table Source
            var tableLastRow = driver.FindElements(By.CssSelector("table > tbody > tr")).Last();
            var tableLastRowFirstCell = tableLastRow.FindElements(By.CssSelector("td")).First();

            Assert.That(tableLastRowFirstCell.Text, Is.EqualTo(urlToAdd));
        }

        [Test]
        public void Test_Shorturl_Add_URL_Invalid()
        {
            //Act
            var LinckAddURL = driver.FindElement(By.LinkText("Add URL"));
            LinckAddURL.Click();
            var inputURL = driver.FindElement(By.Id("url"));
            inputURL.SendKeys("https://www.nbcnews.com/");
            var inputShortCode = driver.FindElement(By.Id("code"));
            inputShortCode.SendKeys(Keys.Control + "a");
            inputShortCode.SendKeys("nbcnews/");
            var buttonCreate = driver.FindElement(By.XPath("//button[@type='submit']"));
            buttonCreate.Click();
            var pageTitle = driver.Title;
            var ErrorMassage = driver.FindElement(By.XPath("//div[@class='err']"));

            //Assert
            Assert.That("Add Short URL", Is.EqualTo(pageTitle));
            Assert.That(ErrorMassage.Text, Is.EqualTo("Short code holds invalid chars!"));
        }

        [Test]
        public void Test_Shorturl_Add_Invalid_URL()
        {
            //Act
            var LinckAddUrl = driver.FindElement(By.LinkText("Add URL"));
            LinckAddUrl.Click();
            var inputUrl = driver.FindElement(By.Id("url"));
            inputUrl.SendKeys("nbcnews");
            var buttonCreate = driver.FindElement(By.XPath("//button[@type='submit']"));
            buttonCreate.Click();
            var lableErrorMassage = driver.FindElement(By.XPath("//div[@class='err']"));

            //Assert
            //Assert.That(lableErrorMassage.Text, Is.EqualTo("Invalid URL!"));
            Assert.That(lableErrorMassage.Displayed);
        }

        [Test]
        public void Test_Shorturl_Visit_Non_Exsisting_URL()
        {
            //Act           
            //driver.Url = "http://shorturl.nakov.repl.co/go/invalid536524";
            driver.Url = "https://ffc0118f-7304-4d78-aa9e-c1e737c17d20-00-2xqb128crqg88.riker.replit.dev/go/invalid536524";
            var lableErrorMassage = driver.FindElement(By.XPath("//div[@class='err']"));
            //Thread.Sleep(1000);
            //Assert
            //Assert.That(lableErrorMassage.Text, Is.EqualTo("Invalid URL"));
            Assert.That(lableErrorMassage.Displayed);
        }

        [Test]
        public void Test_Shorturl_Visits_Check()
        {
            // Arrange
            var LinckShortUrl = driver.FindElement(By.LinkText("Short URLs"));
            LinckShortUrl.Click();

            var tableFirstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();
            var oldCounter = int.Parse(tableFirstRow.FindElements(By.CssSelector("td")).Last().Text);

            var linkToClickCell = tableFirstRow.FindElements(By.CssSelector("td"))[1];
            var linkToClick = linkToClickCell.FindElement(By.TagName("a"));
            linkToClick.Click();

            driver.SwitchTo().Window(driver.WindowHandles[0]);
            driver.Navigate().Refresh();

            tableFirstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();
            var newCounter = int.Parse(tableFirstRow.FindElements(By.CssSelector("td")).Last().Text);

            Assert.That(newCounter, Is.EqualTo(oldCounter + 1));
        }
    }
}