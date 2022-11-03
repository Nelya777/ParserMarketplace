using System.Linq;
using System.Reflection;
using ParserControl.Interfaces;
using ParserControl.Modules.ViewController.Ozon;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using ParserControl.Modules.State;
using ParserControl.Modules.ViewController.Wildberries;
using ParserControl.Modules.ViewController.Aliexpress;

namespace ParserControl.Modules.ViewController
{
    class UiFactory
    {
        private readonly Dictionary<Type, IStringViewPath[]> _factoryTypesDictionary = new Dictionary<Type, IStringViewPath[]>();
        private readonly Type[] _uiParserConstructorTypes = new[] { typeof(ChromeDriver) };
        internal readonly Dictionary<string, UiParser> UiParserStatesDictImpl = new Dictionary<string, UiParser>();

        public UiFactory()
        {
            Register(typeof(UiHomePageWildberries));
            Register(typeof(UiHomePageAliexpress));
        }

        public void Register(Type UiParseType, IStringViewPath[] viewPaths = null)
        {
            if (!typeof(UiParser).IsAssignableFrom(UiParseType))
                throw new Exception($"Wrong type register");

            var attrPaths = new List<IStringViewPath>();
            var bracedViewPathAttributes = UiParseType.GetCustomAttributes<BracedViewPathAttribute>().ToList();
            bracedViewPathAttributes.ForEach(attr => attrPaths.Add(new BracedViewPath(attr.Path)));
            var resPaths = attrPaths.Distinct(new StringViewPathComparer()).ToArray();
            if (viewPaths != null)
                resPaths = attrPaths.Concat(viewPaths).Distinct(new StringViewPathComparer()).ToArray();

            if (!resPaths.Any() || resPaths.Any(path => string.IsNullOrWhiteSpace(path.Path)))
                throw new Exception($"Wrong path register");

            _factoryTypesDictionary.Add(UiParseType, resPaths);
        }

        public void Parse(Dictionary<string, ChromeDriver> driverDict)
        {
            foreach (var kvTypePath in _factoryTypesDictionary)
            {
                var type = kvTypePath.Key;
                var paths = kvTypePath.Value;

                foreach (var path in paths)
                {
                    var uniqId = type.Name + path.Path;
                    var driver = (ChromeDriver)(driverDict.Where(driverChrome => driverChrome.Value.Url.ToString().Contains(path.Path))?.First().Value);
                    if (driver != null)
                    {
                        if (!UiParserStatesDictImpl.TryGetValue(uniqId, out var view))
                        {
                            var constructor = type.GetConstructor(_uiParserConstructorTypes);
                            if (constructor == null)
                            {
                                throw new Exception($"No suitable constructor");
                            }

                            view = (UiParser)constructor.Invoke(new object[] { driver });
                            AppState.Instance.UiParserList.Add(view);
                        }

                        if (view is UiParser uiParser)
                        {
                            if (uiParser.IsVisible())
                            {
                                if (!UiParserStatesDictImpl.ContainsKey(uniqId))
                                {
                                    UiParserStatesDictImpl.Add(uniqId, view);
                                }
                                else
                                {

                                }
                                uiParser.Parse();
                            }
                            else
                            {
                                var removed = UiParserStatesDictImpl.Remove(uniqId);
                            }
                        }
                    }
                    else
                    {
                        var removed = UiParserStatesDictImpl.Remove(uniqId);
                    }

                }
            }
        }
    }
}
