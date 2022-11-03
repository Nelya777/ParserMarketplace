using OpenQA.Selenium.Chrome;
using ParserControl.Interfaces;
using ParserControl.Modules.ViewController;
using ParserControl.Modules.ViewController.Ozon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Modules.State
{
    public class AppState : IAppState
    {
        private string _searchQuery;
        private List<IWebResource> _webResources = new List<IWebResource>();
        public long? _priсeMin;
        public long? _priсeMax;

        private List<IModule> _modules = new List<IModule>();
        private Dictionary<string, ChromeDriver> _chromeDriverDict = new Dictionary<string, ChromeDriver>();
        private List<UiParser> _uiParserList = new List<UiParser>();
        private SearchProducts.SearchProducts searchProducts;
        private static AppState _state;

        ////////////////////////
        ///////КЛИЕНТСКИЕ///////
        ////////////////////////
        public string SearchQuery
        {
            get => _searchQuery;
            set => _searchQuery = value;
        }
        public List<IWebResource> WebResources => _webResources;
        public List<Product> Products => SearchProducts != null ? SearchProducts.Products : null;
        public long? PriсeMin
        {
            get => _priсeMin;
            set => _priсeMin = value;
        }
        public long? PriсeMax
        {
            get => _priсeMax;
            set => _priсeMax = value;
        }

        /////////////////////////
        ////////СЛУЖЕБНЫЕ////////
        /////////////////////////
        public List<UiParser> UiParserList => _uiParserList;
        public static AppState Instance => _state;
        public Dictionary<string, ChromeDriver> ChromeDriverDict => _chromeDriverDict;

        public SearchProducts.SearchProducts SearchProducts
        {
            get => searchProducts;
            set => searchProducts = value;
        }

        public List<IModule> Modules => _modules;

        public AppState()
        {

        }

        public IAppState InitAppState()
        {
            _state = _state == null ? this : _state;
            return this;
        }

        public void Dispose()
        {

        }
    }
}
