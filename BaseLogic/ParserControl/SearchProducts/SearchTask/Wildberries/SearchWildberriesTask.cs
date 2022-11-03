using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParserControl.Interfaces;
using ParserControl.Modules.State;
using ParserControl.Modules.ViewController;
using ParserControl.Modules.ViewController.Wildberries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParserControl.SearchProducts.SearchTask.Wildberries
{
    public class SearchWildberriesTask : ISearchTask
    {
        private ChromeDriver _chromeDriver;
        private List<Product> _products;
        private readonly CancellationTokenSource _searchTaskToken = new CancellationTokenSource();
        private int _countSearchResults;

        public SearchWildberriesTask(List<Product> products, int countProducts, ChromeDriver chromeDriver)
        {
            _products = products;
            _countSearchResults = countProducts;
            _chromeDriver = chromeDriver;

            Script(_searchTaskToken);
            //Task.Factory.StartNew(() => Script(_searchTaskToken));
        }

        public void Script(CancellationTokenSource cancellationToken)
        {
            try
            {
                UiHomePageWildberries uiHomePage = GetUiParser(typeof(UiHomePageWildberries)) != null ?
                    (UiHomePageWildberries)GetUiParser(typeof(UiHomePageWildberries)) : null;

                if(!SearchQuery(uiHomePage)) throw new Exception($"Failed to complete the search query");                

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
            string price = "";
            try { price = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/main/div[2]/div/div[2]/div/div[1]/div/div[3]/div[5]/div/div/div[{item}]/div/a/div[2]/div[1]/span/ins")).Text; }
            catch (Exception ex) { price = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/main/div[2]/div/div[2]/div/div[1]/div/div[3]/div[5]/div/div/div[{item}]/div/a/div[2]/div[1]/span/span")).Text; }
            double.TryParse(string.Join("", price.Where(c => char.IsDigit(c) || c == ',')), out var value);
            return value >= AppState.Instance.PriсeMin && value <= AppState.Instance.PriсeMax;
        }

        private bool NextPage(UiHomePageWildberries uiHomePage)
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
                    var photo = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/main/div[2]/div/div[2]/div/div[1]/div/div[3]/div[5]/div/div/div[{count}]/div/a"));
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
            var id = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/main/div[2]/div/div[2]/div/div[1]/div/div[3]/div[5]/div/div/div[{item}]/div/a")).GetAttribute("href");
            var countProduct = _products.Where(x => x.Id == id).Count();
            return countProduct != 0;
        }

        private Product GetSearchResult(int item)
        {
            //outerHTML
            var photo = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/main/div[2]/div/div[2]/div/div[1]/div/div[3]/div[5]/div/div/div[{item}]/div/a/div[1]/div[2]/img")).GetAttribute("src");
            var name = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/main/div[2]/div/div[2]/div/div[1]/div/div[3]/div[5]/div/div/div[{item}]/div/a/div[2]/div[2]/span")).Text;
            
            string price = "";
            try { price =  _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/main/div[2]/div/div[2]/div/div[1]/div/div[3]/div[5]/div/div/div[{item}]/div/a/div[2]/div[1]/span/ins")).Text; }
            catch (Exception ex) { price =  _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/main/div[2]/div/div[2]/div/div[1]/div/div[3]/div[5]/div/div/div[{item}]/div/a/div[2]/div[1]/span/span")).Text; }
            
            var link = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/main/div[2]/div/div[2]/div/div[1]/div/div[3]/div[5]/div/div/div[{item}]/div/a")).GetAttribute("href");
            var shopName = "wildberries";
            var product = new Product(photo, name, price, link, shopName);
            return product;
        }

        //Ввод в поиск поискового запроса
        private bool SearchQuery(UiHomePageWildberries uiHomePage)
        {
            try
            {
                //var oldPageHTML = _chromeDriver.FindElement(By.XPath("/html/body/div[1]/main/div[2]/div/div")).GetAttribute("outerHTML");
                
                if(CheckSearchQueryDone(uiHomePage)) return true;

                uiHomePage.SearchWindow.DataInput(AppState.Instance.SearchQuery);
                Thread.Sleep(1000);
                uiHomePage.SearchButton.Press();
                Thread.Sleep(3000);
                return true;

                //Thread.Sleep(4000);
                //var test = _chromeDriver.ExecuteScript("return document.querySelector('script[src*=\"common.js\"]')");
                //var test = _chromeDriver.ExecuteScript("return Boolean(document.querySelector('script[src*=\"//ocllfmhjhfmogablefmibmjcodggknml/js/common.js\"]'))");
                //var test = _chromeDriver.ExecuteScript("return document.readyState");
                //var test = _chromeDriver.ExecuteScript("return jQuery.active == 0;");
                //var test = _chromeDriver.ExecuteScript("return window.getSelection()");

                //if (CheckLoadSearchQuery("/html/body/div[1]/main/div[2]/div/div", oldPageHTML, 4000)) return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        /// <summary>
        /// Проверка на то что поиск возможно уже был выполнен
        /// </summary>
        public bool CheckSearchQueryDone(UiHomePageWildberries uiHomePage)
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
                    if(_chromeDriver.FindElement(By.XPath(xPathCheck)).GetAttribute("outerHTML") != oldPageHTML) return true;
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
