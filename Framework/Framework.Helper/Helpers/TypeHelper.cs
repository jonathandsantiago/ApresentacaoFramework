using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Helper.Helpers
{
    public static class TypeHelper
    {
        private static List<Func<object, Type>> realTypeResolvers;
        private static List<Func<object, string, bool>> checkerPropProxies;

        static TypeHelper()
        {
            realTypeResolvers = new List<Func<object, Type>>();
            checkerPropProxies = new List<Func<object, string, bool>>();
        }

        public static void AddRealTypeResolver(Func<object, Type> resolver)
        {
            realTypeResolvers.Add(resolver);
        }

        public static void AddCheckerPropProxies(Func<object, string, bool> checker)
        {
            checkerPropProxies.Add(checker);
        }

        public static object GetPropValue(object objeto, string nome)
        {
            PropertyInfo prop = GetProperties(objeto.GetType()).FirstOrDefault(c => c.Name == nome);

            if (prop == null) return null;

            return prop.GetValue(objeto, null);
        }

        public static IList<PropertyInfo> GetProperties(Type tipo)
        {
            return tipo.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
        }
    }
}