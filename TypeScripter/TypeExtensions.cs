using System;
using System.Reflection;

namespace TypeScripter
{
    internal static class TypeExtensions
    {
        public static bool IsGenericType(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().IsGenericType;
#else
            return type.IsGenericType;
#endif
        }

        public static Type BaseType(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
        }

        public static bool IsEnum(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        public static bool IsPublic(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().IsPublic;
#else
            return type.IsPublic;
#endif
        }

        public static bool IsInterface(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().IsInterface;
#else
            return type.IsInterface;
#endif
        }

        public static Assembly Assembly(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().Assembly;
#else
            return type.Assembly;
#endif
        }

        public static string[] GetEnumNames(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().GetEnumNames();
#else
            return type.GetEnumNames();
#endif
        }

        public static Array GetEnumValues(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().GetEnumValues();
#else
            return type.GetEnumValues();
#endif
        }

        public static Type[] GetGenericArguments(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().GetGenericArguments();
#else
            return type.GetGenericArguments();
#endif
        }

        public static bool IsGenericTypeDefinition(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().IsGenericTypeDefinition;
#else
            return type.IsGenericTypeDefinition;
#endif
        }

        public static bool IsAnsiClass(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().IsAnsiClass;
#else
            return type.IsAnsiClass;
#endif
        }

        public static Type[] GetInterfaces(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().GetInterfaces();
#else
            return type.GetInterfaces();
#endif
        }

        public static FieldInfo[] GetFields(this Type type, BindingFlags bindingFlags)
        {
#if DOTNET
            return type.GetTypeInfo().GetFields(bindingFlags);
#else
            return type.GetFields(bindingFlags);
#endif
        }

        public static PropertyInfo[] GetProperties(this Type type, BindingFlags bindingFlags)
        {
#if DOTNET
            return type.GetTypeInfo().GetProperties(bindingFlags);
#else
            return type.GetProperties(bindingFlags);
#endif
        }

        public static MethodInfo[] GetMethods(this Type type, BindingFlags bindingFlags)
        {
#if DOTNET
            return type.GetTypeInfo().GetMethods(bindingFlags);
#else
            return type.GetMethods(bindingFlags);
#endif
        }

        public static Type[] GetGenericParameterConstraints(this Type type)
        {
#if DOTNET
            return type.GetTypeInfo().GetGenericParameterConstraints();
#else
            return type.GetGenericParameterConstraints();
#endif
        }

        



    }
}