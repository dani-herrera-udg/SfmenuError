using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Drawing;
using System.Threading.Tasks;
using Xunit;

namespace test
{
    public class UnitTest
    {

        private const int MillisecondsTimeout = 800;  // milisegons extres d'espera en el mode hosted

        private string _appUrl = "https://localhost:5001";
        private readonly Random _random;

        private const int SecondsWait = 20;
        private  IWebDriver _driver;
        private DefaultWait<IWebDriver> _fluentWait;

        [Fact]
        public async Task Test1()
        {

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--lang=us --window-size=1920,1080");
            options.SetLoggingPreference(LogType.Browser, LogLevel.All);
            _driver = new ChromeDriver(".", options);

            _fluentWait = new DefaultWait<IWebDriver>(_driver)
            {
                Timeout = TimeSpan.FromSeconds(SecondsWait),
                PollingInterval = TimeSpan.FromMilliseconds(250)
            };
            _fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            _driver.Manage().Window.Size = new Size(1000, 1000);

            // Selectors
            var blazor_error_selector = By.CssSelector("#blazor-error-ui");
            var go_home_selector = By.CssSelector("#main-menu-go-home");
            var go_counter_selector = By.CssSelector("#main-menu-go-counter");
            var go_license_selector = By.CssSelector(".sf-license-close-btn");
            var main_menu = By.CssSelector("#main-menu-go");

            // Go page
            _driver.Navigate().GoToUrl(_appUrl);
            await Task.Delay(100);

            // Close license
            _fluentWait.Until((w) => w.FindElement(go_license_selector));
            _driver.FindElement(go_license_selector).Click();

            // wait for error div reader
            _fluentWait.Until( (w) => w.FindElement(blazor_error_selector));
            var blazor_error = _driver.FindElement(blazor_error_selector);

            // get main elements
            _fluentWait.Until( (w) => w.FindElement(main_menu));
            var main_menu_go = _driver.FindElement(main_menu);

            while (true)
            {
                // open main menu
                main_menu_go.Click();

                // click go home
                _fluentWait.Until((w) => w.FindElement(go_home_selector));
                var main_menu_go_home = _driver.FindElement(go_home_selector);
                main_menu_go_home.Click();

                // reopen menu
                main_menu_go.Click();

                // click go counter
                _fluentWait.Until((w) => w.FindElement(go_counter_selector));
                var main_menu_go_counter = _driver.FindElement(go_counter_selector);
                main_menu_go_counter.Click();

                // waig for a bit before check the fails
                await Task.Delay(100);

                // check for no error
                Assert.False(blazor_error.Displayed);

            }

        }
    }
}
