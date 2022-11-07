using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParserControl.Interfaces;
using ParserControl.Modules.State;
using ParserControl.Modules.ViewController;
using ParserControl.Modules.ViewController.Aliexpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParserControl.SearchProducts.SearchTask.Aliexpress
{
    public class SearchAliexpressTask : ISearchTask
    {
        private ChromeDriver _chromeDriver;
        private List<Product> _products;
        private readonly CancellationTokenSource _searchTaskToken = new CancellationTokenSource();
        private int _countSearchResults;

        public SearchAliexpressTask(List<Product> products, int countProducts, ChromeDriver chromeDriver)
        {
            _products = products;
            _countSearchResults = countProducts;
            _chromeDriver = chromeDriver;

            Script(_searchTaskToken);
        }

        public void Script(CancellationTokenSource cancellationToken)
        {
            try
            {
                UiHomePageAliexpress uiHomePage = GetUiParser(typeof(UiHomePageAliexpress)) != null ?
                    (UiHomePageAliexpress)GetUiParser(typeof(UiHomePageAliexpress)) : null;

                if (!SearchQuery(uiHomePage)) throw new Exception($"Failed to complete the search query");

                var resultsPage = GetCountSearch();
                var countPage = 0;
                var countProduct = 0;
                while (_countSearchResults != countProduct && resultsPage != 0)
                {
                    countPage++;
                    if (cancellationToken.Token.IsCancellationRequested) return;
                    if (IsExistProduct(countPage)) continue;

                    if (IsRangePrice(countPage))
                    {
                        countProduct++;
                        _products.Add(GetSearchResult(countPage));
                    }

                    if (countPage >= resultsPage && NextPage(uiHomePage))
                    {
                        resultsPage = GetCountSearch();
                        countPage = 0;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool IsRangePrice(int item)
        {
            var price = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[4]/div[2]/div[2]/div/div/div/div[{item}]/div/div/a/div[3]/div[2]/div[1]")).Text;
            double.TryParse(string.Join("", price.Where(c => char.IsDigit(c) || c == ',')), out var value);          
            return value >= AppState.Instance.PriсeMin && value <= AppState.Instance.PriсeMax;
        }

        private bool NextPage(UiHomePageAliexpress uiHomePage)
        {
            try
            {
                if (!uiHomePage.NextPageButton.IsPressPossible) return false;
                uiHomePage.NextPageButton.Press();
                Thread.Sleep(2200);
                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        private int GetCountSearch()
        {
            int count = 0;
            try
            {
                while (true)
                {
                    count++;
                    var photo = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[4]/div[2]/div[2]/div/div/div/div[{count}]/div/div/a"));
                }
            }
            catch (Exception ex)
            {
                return count - 1;
            }
        }

        /// <summary>
        /// Проверка существует ли продукт в листе продуктов
        /// </summary>
        private bool IsExistProduct(int item)
        {
            var id = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[4]/div[2]/div[2]/div/div/div/div[{item}]/div/div/a")).GetAttribute("href");
            var countProduct = _products.Where(x => x.Id == id).Count();
            return countProduct != 0;
        }

        private Product GetSearchResult(int item)
        {
            //outerHTML
            string photo;
            try {  photo = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[4]/div[2]/div[2]/div/div/div/div[{item}]/div/a/div[1]/div[1]/div/div/img")).GetAttribute("src"); }
            catch {  photo = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[4]/div[2]/div[2]/div/div/div/div[{item}]/div/a/div[1]/div[1]/div/div[1]/img")).GetAttribute("src"); }
            var name = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[4]/div[2]/div[2]/div/div/div/div[{item}]/div/div/a/div[1]/div[1]")).Text;
            var price = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[4]/div[2]/div[2]/div/div/div/div[{item}]/div/div/a/div[3]/div[2]/div[1]")).Text;
            var link = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[4]/div[2]/div[2]/div/div/div/div[{item}]/div/div/a")).GetAttribute("href");
            var shopName = "aliexpress";
            var product = new Product(photo, name, price, link, shopName);
            return product;
        }

        //Ввод в поиск поискового запроса
        private bool SearchQuery(UiHomePageAliexpress uiHomePage)
        {
            try
            {
                if (CheckSearchQueryDone(uiHomePage)) return true;

                uiHomePage.SearchWindow.DataInput(AppState.Instance.SearchQuery);
                Thread.Sleep(1000);
                uiHomePage.SearchWindow.PressEnter();
                //uiHomePage.SearchButton.Press();
                Thread.Sleep(3000);
                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        /// <summary>
        /// Проверка на то что поиск возможно уже был выполнен
        /// </summary>
        public bool CheckSearchQueryDone(UiHomePageAliexpress uiHomePage)
        {
            return uiHomePage.SearchWindow.Text.ToLower() == AppState.Instance.SearchQuery.ToLower();
        }

        /// <summary>
        /// Проверка загружены ли результаты поиска
        /// </summary>
        public bool CheckLoadSearchQuery(string xPathCheck, string oldPageHTML, int timeMilliseconds = 0)
        {
            var timeStart = DateTime.Now;

            do
            {
                try
                {
                    Thread.Sleep(100);
                    if (_chromeDriver.FindElement(By.XPath(xPathCheck)).GetAttribute("outerHTML") != oldPageHTML) return true;
                }
                catch (Exception ex)
                {

                }
            }
            while ((DateTime.Now - timeStart).TotalMilliseconds < timeMilliseconds);

            return false;
        }

        //Забираем нужное нам окно, если его сейчас видно
        private UiParser GetUiParser(Type typeUiParser)
        {
            foreach (var uiParser in AppState.Instance.UiParserList)
            {
                if (uiParser.GetType() == typeUiParser) return uiParser;
            }

            return null;
        }

        public void Dispose()
        {
            _searchTaskToken?.Cancel();
        }
    }
}
