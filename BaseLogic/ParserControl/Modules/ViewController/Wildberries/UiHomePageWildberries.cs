using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParserControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Modules.ViewController.Wildberries
{
    [BracedViewPath("https://www.wildberries.ru/")]
    public class UiHomePageWildberries : UiParser
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

        public UiHomePageWildberries(ChromeDriver driver)
        {
            _driver = driver;
            _searchButton = new DefaultButton(_driver);
            _activeTabButton = new DefaultButton(_driver);
            _nextPageButton = new DefaultButton(_driver);
            _searchWindow = new DefaultWindowInput(_driver);
        }

        public override void Parse()
        {
            _searchButton.ParseButton("//*[@id=\"applySearchBtn\"]");
            _activeTabButton.ParseButton("/html/body/div[1]/main/div[2]/div/div/div[1]/div[2]/div[2]/div[6]/div/div");
            
            //Костыль для определения кнопки перехода на следующую страницу
            for (int i = 0; i < 10; i++)
            {
                _nextPageButton.ParseButton($"/html/body/div[1]/main/div[2]/div/div/div[1]/div[2]/div[2]/div[6]/div/div/a[{(1+i)}]");
                if (_nextPageButton.Text == "Следующая страница") break;
            }

            _searchWindow.ParseWindow("//*[@id=\"searchInput\"]");
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
