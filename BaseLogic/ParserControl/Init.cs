using ParserControl.Interfaces;
using ParserControl.Modules.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl
{
    public class Init
    {
        private static readonly List<IModule> _modules = new List<IModule>();

        public static void InitModules()
        {
            var executingAssemblyTypes = Assembly.GetExecutingAssembly().GetTypes();

            var phoneStateType = executingAssemblyTypes.FirstOrDefault(x => x.IsClass &&
                                                                            x.GetInterface(nameof(IAppState)) != null);
            var moduleTypes = executingAssemblyTypes.Where(x => x.IsClass &&
                                                                x.GetInterface(nameof(IModule)) != null);


            (Activator.CreateInstance(phoneStateType) as IAppState).InitAppState();

            foreach (var moduleType in moduleTypes)
                _modules.Add((Activator.CreateInstance(moduleType) as IModule).InitModule());
        }

        public static object CloseModules()
        {
            if(AppState.Instance?.Modules == null) return new object();

            foreach (var modul in AppState.Instance?.Modules)
                modul.Dispose();

            AppState.Instance?.SearchProducts?.Dispose();

            return new object();
    }
    }
}
