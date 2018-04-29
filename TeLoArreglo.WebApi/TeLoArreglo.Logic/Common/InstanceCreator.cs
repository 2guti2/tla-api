using System;
using System.Linq;
using System.Reflection;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Logic.Common
{
    public class InstanceCreator
    {
        public static object User(string className)
        {
            var assembly = Assembly.GetAssembly(typeof(User));

            var type = assembly.GetTypes().First(t => t.Name == className);

            return Activator.CreateInstance(type);
        }
    }
}
