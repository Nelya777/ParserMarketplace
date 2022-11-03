using OpenQA.Selenium.Chrome;
using ParserControl.Interfaces;
using ParserControl.Modules.State;
using ParserControl.Modules.ViewController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Modules.ViewController
{
    public class ViewController : IModule
    {
        private UiFactory _uiFactory;
        private ChromeDriver _driver;

        public ViewController() 
        {
            _uiFactory = new UiFactory();
            CreateChromeDriver();
            AppState.Instance.Modules.Add(this);
        }


        public IModule InitModule()
        {
            try
            {
                Task.Factory.StartNew(() => Thread());
            }
            catch (Exception ex)
            {

            }
            return this;            
        }

        public void Thread()
        {
            try
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(2000);
                    _uiFactory.Parse(AppState.Instance.ChromeDriverDict);                      
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void CreateChromeDriver()
        {
            var option = new ChromeOptions();
            option.AddArgument("start-maximized");
            //option.AddArguments(new List<string>() { "headless" });

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            List<IWebResource> webResourceList = new List<IWebResource>();
            webResourceList.Add(new WebResourceWildberries());
            webResourceList.Add(new WebResourceAliexpress());

            foreach (var weResourse in webResourceList)
            {
                var driver = new ChromeDriver(service, option);
                driver.Url = weResourse.Url;
                AppState.Instance.ChromeDriverDict.Add(weResourse.Name, driver);
            }
        }

        public void Dispose()
        {
            foreach (var driver in AppState.Instance.ChromeDriverDict)
                driver.Value.Quit();

            foreach (var uiParser in AppState.Instance.UiParserList)
                uiParser.Dispose();
        }
    }
}
