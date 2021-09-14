using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using AccessTools = HarmonyLib.AccessTools;

namespace ADOFAI_GG {
    public static class Initalizer {
        public static void Init() {
            var methods = GetAllTypes(AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes()))
                .Where(type => !(type?.Namespace?.StartsWith("System") ?? true))
                .SelectMany(type => type.GetMethods(AccessTools.all))
                .Where(info => info.IsStatic
                               && info.GetParameters().Length == 0
                               && info.HasMethodBody()
                               && !info.IsConstructor
                               && !info.IsGenericMethod
                               && !info.IsGenericMethodDefinition
                );

            var initalizers = new Action(() => { });
            foreach (var methodInfo in methods) {
                if (methodInfo.GetCustomAttribute<InitAttribute>() != null) {
                    initalizers += (() => methodInfo.Invoke(null, new object[] { }));
                }
            }

            initalizers();
        }
        
        public static void LateInit() {
            var methods = GetAllTypes(AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes()))
                .Where(type => !(type?.Namespace?.StartsWith("System") ?? true))
                .SelectMany(type => type.GetMethods(AccessTools.all))
                .Where(info => info.IsStatic
                               && info.GetParameters().Length == 0
                               && info.HasMethodBody()
                               && !info.IsConstructor
                               && !info.IsGenericMethod
                               && !info.IsGenericMethodDefinition
                );

            var initalizers = new Action(() => { });
            foreach (var methodInfo in methods) {
                if (methodInfo.GetCustomAttribute<LateInitAttribute>() != null) {
                    initalizers += (() => methodInfo.Invoke(null, new object[] { }));
                }
            }

            initalizers();
        }

        private static List<Type> GetAllTypes(IEnumerable<Type> types) {
            var result = new List<Type>();
            foreach (var type in types) {
                result.Add(type);
                foreach (var nested in GetAllNestedTypes(type)) {
                    result.Add(nested);
                }
            }

            return result;
        }
        private static List<Type> GetAllNestedTypes(Type type) {
            var nesteds = type.GetNestedTypes(AccessTools.all);

            var result = new List<Type>();

            void GetNestedTypes(Type t) {
                result.Add(t);
                foreach (var nestedType in t.GetNestedTypes(AccessTools.all)) {
                    GetNestedTypes(nestedType);
                }
            }

            foreach (var nested in nesteds) {
                GetNestedTypes(nested);
            }

            return result;
        }
    }
}