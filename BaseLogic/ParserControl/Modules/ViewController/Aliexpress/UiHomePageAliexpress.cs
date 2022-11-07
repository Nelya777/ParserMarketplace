using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParserControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Modules.ViewController.Aliexpress
{
    [BracedViewPath("https://aliexpress.ru/")]
    public class UiHomePageAliexpress : UiParser
    {
        private readonly ChromeDriver _driver;

        private IButton _searchButton;
        private IButton _activeTabButton;
        private IButton _nextPageButton;
        private IWindowInput _searchWindow;

        public IButton SearchButton => _searchButton;
        public IButton ActiveTabButton => _activeTabButton;
        public IButton NextPageButton => _nextPageButton;
        public IWindowInput SearchWindow => _searchWindow;
        public UiHomePageAliexpress(ChromeDriver driver)
        {
            _driver = driver;
            _searchButton = new DefaultButton(_driver);
            _activeTabButton = new DefaultButton(_driver);
            _nextPageButton = new DefaultButton(_driver);
            _searchWindow = new DefaultWindowInput(_driver);
        }

        public override void Parse()
        {
            string parseButtonStr1 = "/html/body/div[1]/div/div[2]/div/div/div[2]/div/form/button";
            string parseButtonStr2 = "/html/body/div[1]/div/div[2]/div/div/div/div[2]/div[1]/div[1]/form/button";
            string parseButtonStr3 = "/html/body/div[1]/div/div[3]/div/div/div/div/div/div[2]/div/div/div/div[2]/div[2]/div/label";
            _searchButton.ParseButton(parseButtonStr1, parseButtonStr2, parseButtonStr3 );

            string parseWindowStr1 = "/html/body/div[1]/div/div[2]/div/div/div[2]/div/form/div/div/input";
            string parseWindowStr2 = "/html/body/div[1]/div/div[2]/div/div/div/div[2]/div[1]/div[1]/form/div/div/input";
            string parseWindowStr3 = "//*[@id=\"searchInput\"]";
            _searchWindow.ParseWindow(parseWindowStr1, parseWindowStr2, parseWindowStr3);

            //Костыль для определения кнопки перехода на следующую страницу
            for (int i = 0; i < 10; i++)
            {
                _nextPageButton.ParseButton($"/html/body/div[1]/div/div[3]/div[1]/div[2]/div/div[4]/button[{(1 + i)}]");
                if (_nextPageButton.Text == "След. стр. ") break;
            }
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
