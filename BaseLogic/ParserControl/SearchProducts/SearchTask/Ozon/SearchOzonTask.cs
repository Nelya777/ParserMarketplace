using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParserControl.Interfaces;
using ParserControl.Modules.State;
using ParserControl.Modules.ViewController;
using ParserControl.Modules.ViewController.Ozon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.SearchProducts.SearchTask.Ozon
{
    public class SearchOzonTask : ISearchTask
    {
        private ChromeDriver _chromeDriver;
        private List<Product> _products;

        public SearchOzonTask(List<Product> products, ChromeDriver chromeDriver) 
        {
            _products = products;
            _chromeDriver = chromeDriver;
        }

        public void Script()
        {
            try
            {
                UiHomePage uiHomePage = GetUiParser(typeof(UiHomePage)) != null ?
                    (UiHomePage)GetUiParser(typeof(UiHomePage)) : null;

                SearchQuery(uiHomePage);

                //Забираем найденые результаты
                //while (true)
                //{
                //    _products.Add(GetSearchResult());
                //}

                var test = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[5]/div[1]/div[1]/div")).GetAttribute("outerHTML");
                var count = GetCountSearch();
                for (int i = 1; i < count; i++)
                {
                    _products.Add(GetSearchResult(i));
                }
            }
            catch (Exception ex)
            {

            }
        }

        private int GetCountSearch()
        {
            int count = 0;
            try
            {
                while (true)
                {
                    count++;
                    var photo = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[5]/div[1]/div[1]/div/div[{count}]/a"));
                }             
            }
            catch (Exception ex)
            {
                return count - 1;
            }           
        }

        private Product GetSearchResult(int item)
        {
            //outerHTML

            var photo = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[5]/div[1]/div[1]/div/div[{item}]/a/div/div[1]/img")).GetAttribute("src");
            var name = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[5]/div[1]/div[1]/div/div[{item}]/div[1]/a/span/span")).Text;
            var price = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[5]/div[1]/div[1]/div/div[{item}]/div[1]/div[1]/span")).Text;
            var link = _chromeDriver.FindElement(By.XPath($"/html/body/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[5]/div[1]/div[1]/div/div[{item}]/a")).GetAttribute("href");
            var shopName = "Ozon";
            var product = new Product(photo, name, price, link, shopName);
            return product;
        }

        //Ввод в поиск поискового запроса
        private bool SearchQuery(UiHomePage uiHomePage)
        {
            try
            {
                uiHomePage.SearchWindow.DataInput(AppState.Instance.SearchQuery);
                System.Threading.Thread.Sleep(1500);
                uiHomePage.SearchButton.Press();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        //Забираем нужное нам окно, если его сейчас видно
        private UiParser GetUiParser(Type typeUiParser)
        {
            foreach (var uiParser in AppState.Instance.UiParserList)
            {
                if(uiParser.GetType() == typeUiParser) return uiParser;
            }

            return null;
        }

        public void Dispose()
        {

        }
    }
}
