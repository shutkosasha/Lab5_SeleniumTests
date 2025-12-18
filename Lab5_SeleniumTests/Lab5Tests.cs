using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Lab5_SeleniumTests
{
    public class Lab5Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string BaseUrl = "https://the-internet.herokuapp.com/";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                driver?.Quit();
            }
            catch { }
            finally
            {
                driver?.Dispose();
                driver = null;
            }
        }

        [Test]
        public void ForgotPassword_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl + "forgot_password");

         
            var emailInput = driver.FindElement(By.Id("email"));
            Assert.IsTrue(emailInput.Displayed, "Поле email не відображається");

            emailInput.SendKeys("test@test.com");

       
            driver.FindElement(By.Id("form_submit")).Click();

        
            var message = driver.FindElement(By.TagName("h1")).Text;
            Assert.That(message, Is.EqualTo("Internal Server Error"));
        }

        [Test]
        public void HorizontalSlider_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl + "horizontal_slider");

            var slider = driver.FindElement(By.TagName("input"));
            var valueLabel = driver.FindElement(By.Id("range"));

            string initialValue = valueLabel.Text;

        
            slider.SendKeys(Keys.ArrowRight);
            slider.SendKeys(Keys.ArrowRight);
            slider.SendKeys(Keys.ArrowRight);

            string newValue = valueLabel.Text;

            Assert.AreNotEqual(initialValue, newValue, "Значення слайдера не змінилось");
        }

        [Test]
        public void Dropdown_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl + "dropdown");

            var dropdown = new SelectElement(driver.FindElement(By.Id("dropdown")));
            dropdown.SelectByValue("1");

            Assert.That(dropdown.SelectedOption.Text, Is.EqualTo("Option 1"));
        }

        [Test]
        public void Typos_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl + "typos");

            var text = driver.FindElement(By.CssSelector(".example p:nth-of-type(2)")).Text;

            Assert.IsNotEmpty(text, "Текст відсутній");
        }

        [Test]
        public void EntryAd_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl + "entry_ad");

            wait.Until(d => d.FindElement(By.ClassName("modal")).Displayed);

            var modalTitle = driver.FindElement(By.CssSelector(".modal-title h3")).Text;
            Assert.That(modalTitle, Is.EqualTo("THIS IS A MODAL WINDOW"));

            driver.FindElement(By.CssSelector(".modal-footer p")).Click();
        }

        [Test]
        public void FileDownload_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl + "download");

            var fileLink = driver.FindElement(By.CssSelector(".example a"));
            Assert.IsTrue(fileLink.Displayed, "Посилання для завантаження не знайдено");
        }

        [Test]
        public void BasicAuth_Test()
        {
            driver.Navigate().GoToUrl("https://admin:admin@the-internet.herokuapp.com/basic_auth");

            var message = driver.FindElement(By.TagName("p")).Text;
            Assert.IsTrue(message.Contains("Congratulations"), "Авторизація не пройшла");
        }

        [Test]
        public void DynamicLoading_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl + "dynamic_loading/1");

            driver.FindElement(By.CssSelector("#start button")).Click();

            var hello = wait.Until(d =>
            {
                var el = d.FindElement(By.CssSelector("#finish h4"));
                return string.IsNullOrWhiteSpace(el.Text) ? null : el;
            });

            Assert.That(hello.Text, Is.EqualTo("Hello World!"));
        }

        [Test]
        public void ContextMenu_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl + "context_menu");

            var box = driver.FindElement(By.Id("hot-spot"));

            Actions actions = new Actions(driver);
            actions.ContextClick(box).Perform();

            IAlert alert = driver.SwitchTo().Alert();
            Assert.That(alert.Text, Is.EqualTo("You selected a context menu"));

            alert.Accept();
        }

        [Test]
        public void RedirectLink_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl + "redirector");

            driver.FindElement(By.Id("redirect")).Click();

            Assert.IsTrue(driver.Url.Contains("status_codes"), "Редірект не відбувся");
        }
    }
}
