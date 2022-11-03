using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParserControl.Interfaces;
using ParserControl.Modules.ViewController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Modules.ViewController.Ozon
{
    [BracedViewPath("https://www.ozon.ru/")]
    public class UiHomePage : UiParser
    {
        private readonly ChromeDriver _driver;

        private IButton _searchButton;
        private IWindowInput _searchWindow;

        public IButton SearchButton => _searchButton;
        public IWindowInput SearchWindow => _searchWindow;

        public UiHomePage(ChromeDriver driver)
        {
            _driver = driver;
            _searchButton = new DefaultButton(_driver);
            _searchWindow = new DefaultWindowInput(_driver);
        }

        public override void Parse()
        {
            _searchButton.ParseButton("/html/body/div[1]/div/div[1]/header/div[1]/div[3]/div/form/button");
            _searchWindow.ParseWindow("/html/body/div[1]/div/div[1]/header/div[1]/div[3]/div/form/div[2]/input[1]");
        }

        public override bool IsVisible()
        {
            return true;
        }

        public override void Dispose()
        {

        }
    }
}
