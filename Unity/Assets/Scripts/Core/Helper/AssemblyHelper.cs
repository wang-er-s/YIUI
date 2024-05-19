using System;
using System.Collections.Generic;
using System.Reflection;

namespace ET
{
    public static class AssemblyHelper
    {
        public static Dictionary<string, Type> GetAssemblyTypes(params Assembly[] args)
        {
            Dictionary<string, Type> types = new Dictionary<string, Type>();

            foreach (Assembly ass in args)
            {
                foreach (Type type in ass.GetTypes())
                {
                    types[type.FullName] = type;
                }
            }

            return types;
        }

#if UNITY_EDITOR
        [StaticField]
        public static Dictionary<string, Assembly> name2Assembly = new Dictionary<string, Assembly>();

        static AssemblyHelper()
        {
            foreach (Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                name2Assembly.Add(assembly.GetName().Name, assembly);
            }
        }
        
        public static Assembly GetAssembly(string name)
        {
            name2Assembly.TryGetValue(name, out var result);
            return result;
        }
#endif
    }
}