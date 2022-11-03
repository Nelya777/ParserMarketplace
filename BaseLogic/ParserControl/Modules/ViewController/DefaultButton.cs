using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using ParserControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParserControl.Modules.ViewController
{
    public class DefaultButton : IButton
    {
        private readonly ChromeDriver _driver;
        private string _parseString;
        private string _text;

        public bool IsVisible { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsPressPossible { get; set; }
        public string Text => _text;
        public string ParseString => _parseString;

        public DefaultButton(ChromeDriver driver)
        {
            _driver = driver;
        }

        public void ParseButton(params string[] parseButtonString)
        {
            foreach (var buttonString in parseButtonString)
            {
                try
                {
                    _driver.FindElement(By.XPath(buttonString));
                    _parseString = buttonString;
                    IsPressPossible = true;
                    _text = GetText();
                    break;
                }
                catch (Exception ex) { }
            }
        }

        public void Press()
        {
            try
            {
                _driver.FindElement(By.XPath(_parseString)).Click();
            }
            catch (ElementClickInterceptedException ex)
            {
                _driver.ExecuteScript("arguments[0].click()", _driver.FindElement(By.XPath(_parseString)));
            }
        }

        private string GetText()
        {
            try
            {
                if (IsPressPossible)
                    return _driver.FindElement(By.XPath(_parseString)).Text;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
