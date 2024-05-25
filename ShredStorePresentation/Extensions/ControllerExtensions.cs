using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ShredStorePresentation.Extensions
{
    public static class ControllerExtensions
    {
        public static string ControllerName<T>() where T : Controller
        {
            var name = typeof(T).Name;
            return name.Substring(0, Math.Max(name.LastIndexOf(nameof(Controller),
                                              StringComparison.CurrentCultureIgnoreCase), 0));    
        }

        public static string IndexActionName() => "Index";

        public static string EmptyCartActionName() => "EmptyCart";

        public static List<string> ControllerActionName<T>() where T : Controller
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            var res = asm.GetTypes()
                .Where(type => typeof(T).IsAssignableFrom(type)) //filter controllers
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)) && !method.Name.Contains('_'));

            List<string> actions = new List<string>();
            foreach(var item in res)
            {
                actions.Add(item.Name);
            }

            return actions;

        }


    }
}
