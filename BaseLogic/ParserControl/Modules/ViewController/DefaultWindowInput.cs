using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParserControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParserControl.Modules.ViewController
{
    internal class DefaultWindowInput : IWindowInput
    {
        private readonly ChromeDriver _driver;
        private string _parseString;
        private string _text;

        public bool IsVisible { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsInputPossible { get; set; }
        public string Text => _text;
        public string ParseString => _parseString;

        public DefaultWindowInput(ChromeDriver driver)
        {
            _driver = driver;
        }

        public void ParseWindow(params string[] parseWindowString)
        {
            foreach (var windowString in parseWindowString)
            {
                try
                {
                    _driver.FindElement(By.XPath(windowString));
                    _parseString = windowString;
                    IsInputPossible = true;
                    _text = GetText();
                    break;
                }
                catch (Exception ex) { }
            }
        }

        public void DataInput(string inputString)
        {
            if (IsInputPossible)
            {
                _driver.FindElement(By.XPath(_parseString)).Clear();
                Thread.Sleep(600);
                _driver.FindElement(By.XPath(_parseString)).SendKeys(inputString);
            }
        }

        public void PressEnter()
        {
            try
            {
                _driver.FindElement(By.XPath(_parseString)).SendKeys(Keys.Enter);
            }
            catch (ElementClickInterceptedException ex)
            {
                _driver.ExecuteScript("arguments[0].enter()", _driver.FindElement(By.XPath(_parseString)));
            }
        }

        private string GetText()
        {
            try
            {
                if (IsInputPossible)
                    return _driver.FindElement(By.XPath(_parseString)).GetDomProperty("value");
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
