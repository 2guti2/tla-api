using System;
using System.Linq;
using System.Reflection;
using TeLoArreglo.Logic.Common.Media;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Logic.Common
{
    public class GlobalFactory
    {
        public static object User(string className)
        {
            var assembly = Assembly.GetAssembly(typeof(User));

            return CreateInstance(assembly, className);
        }

        public static object MediaManager(string className)
        {
            var assembly = Assembly.GetAssembly(typeof(MediaManager));

            return CreateInstance(assembly, className + "Manager");
        }

        private static object CreateInstance(Assembly assembly, string className)
        {
            var type = assembly.GetTypes().First(t => t.Name == className);

            return Activator.CreateInstance(type);
        }
    }
}
