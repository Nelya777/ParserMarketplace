using OpenQA.Selenium.Chrome;
using ParserControl.Interfaces;
using ParserControl.Modules.State;
using ParserControl.Modules.ViewController;
using ParserControl.SearchProducts.SearchTask.Aliexpress;
using ParserControl.SearchProducts.SearchTask.Ozon;
using ParserControl.SearchProducts.SearchTask.Wildberries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.SearchProducts
{
    public class SearchProducts : IDisposable
    {
        private List<Product> _products = new List<Product>();
        private ISearchTask _searchTask;
        private const int COUNT_SEARCH_RESULTS = 8;
        private int _lastCountProduct;

        //public List<Product> Products => _products;
        public List<Product> Products => _products;

        public SearchProducts()
        {
            AppState.Instance.SearchProducts = this;
        }

        public void OnComandSearch(object empty)
        {
            try
            {
                CheckSearchConditons();
                _lastCountProduct = Products.Count;
                foreach (var webResource in AppState.Instance.WebResources)
                {
                    switch (webResource)
                    {
                        case WebResourceAliexpress obj:
                            _searchTask = new SearchAliexpressTask(Products, COUNT_SEARCH_RESULTS,
                                (ChromeDriver)(AppState.Instance.ChromeDriverDict.Where(x => x.Key == webResource.Name).First().Value));
                            break;
                        case WebResourceWildberries obj:
                            _searchTask = new SearchWildberriesTask(Products, _lastCountProduct + COUNT_SEARCH_RESULTS - Products.Count,
                                (ChromeDriver)(AppState.Instance.ChromeDriverDict.Where(x => x.Key == webResource.Name).First().Value));
                            break;
                    }

                    if (Products.Count >= _lastCountProduct + COUNT_SEARCH_RESULTS) return;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void OnComandSortMin()
        {
            try
            {
                _products = _products.OrderBy(product =>
                {
                    double.TryParse(string.Join("", product.Price.Where(c => char.IsDigit(c) || c == ',')), out var value);
                    return value;
                }).ToList();
            }
            catch (Exception ex)
            {

            }
        }

        public void OnComandSortMax()
        {
            try
            {
                _products = _products.OrderByDescending(product =>
                {
                    double.TryParse(string.Join("", product.Price.Where(c => char.IsDigit(c) || c == ',')), out var value);
                    return value;
                }).ToList();
            }
            catch (Exception ex)
            {

            }
        }

        public void CheckSearchConditons()
        {
            if (AppState.Instance.WebResources.Count == 0) throw new Exception($"Web resources for search not specified!");
            if (AppState.Instance.SearchQuery == null) throw new Exception($"Search query not specified!");
            if (AppState.Instance.PriсeMin == null) throw new Exception($"Priсe min not specified!");
            if (AppState.Instance.PriсeMax == null) throw new Exception($"Priсe max not specified!");
        }

        public void Dispose()
        {
            _searchTask.Dispose();
        }
    }
}
