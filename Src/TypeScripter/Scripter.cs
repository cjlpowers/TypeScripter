using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using TypeScripter.TypeScript;
using TypeScripter.Readers;

namespace TypeScripter
{
    /// <summary>
    /// A class which generates TypeScript definitions for .NET types
    /// </summary>
    public class Scripter
    {
        #region Properties
        private Dictionary<Type, TsType> TypeLookup
        {
            get;
            set;
        }

        private HashSet<TsType> Types
        {
            get;
            set;
        }

        private Func<Assembly, bool> AssemblyFilter
        {
            get;
            set;
        }

        private Func<Type, bool> TypeFilter
        {
            get;
            set;
        }

        private TypeReader Reader
        {
            get;
            set;
        }

        private TsFormatter Formatter
        {
            get;
            set;
        }
        #endregion

        #region Creation
        /// <summary>
        /// Constructor
        /// </summary>
        public Scripter()
        {
            this.Types = new HashSet<TsType>();

            // Add mappings for primitives 
            this.TypeLookup = new Dictionary<Type, TsType>()
            {
                { typeof(void), TsPrimitive.Void },
                { typeof(object), TsPrimitive.Any },
                { typeof(string), TsPrimitive.String },
                { typeof(bool), TsPrimitive.Boolean },
                { typeof(byte), TsPrimitive.Number },
                { typeof(short), TsPrimitive.Number },
                { typeof(int), TsPrimitive.Number },
                { typeof(long), TsPrimitive.Number },
                { typeof(float), TsPrimitive.Number },
                { typeof(double), TsPrimitive.Number },
            };

            this.RegisterTypeMapping(CreateGenericDictionaryType(), typeof(Dictionary<,>));
            this.RegisterTypeMapping(TsPrimitive.Any, typeof(ValueType));

            // initialize the scripter with default implementations
            this.Reader = new TypeReader();
            this.Formatter = new TsFormatter();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the current scripter output
        /// </summary>
        /// <returns>The scripter output</returns>
        public override string ToString()
        {
            var str = new StringBuilder();
            foreach (var module in this.Modules())
                str.Append(this.Formatter.Format(module));
            return str.ToString();
        }
        #endregion

        #region Operations
        /// <summary>
        /// Configures the scripter to use a particular TypeReader
        /// </summary>
        /// <param name="reader">The type reader</param>
        /// <returns>The scripter</returns>
        public Scripter UsingTypeReader(TypeReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            this.Reader = reader;
            return this;
        }

        /// <summary>
        /// Adds a type to be scripted
        /// </summary>
        /// <param name="tsType">The type</param>
        /// <returns>The scripter</returns>
        public Scripter AddType(TsType tsType)
        {
            this.Types.Add(tsType);
            return this;
        }

        /// <summary>
        /// Adds a type to be scripted
        /// </summary>
        /// <param name="tsType">The type</param>
        /// <param name="type">The .NET type</param>
        /// <returns>The scripter</returns>
        private Scripter AddType(TsType tsType, Type type)
        {
            this.AddType(tsType);
            this.RegisterTypeMapping(tsType, type);
            return this;
        }

        /// <summary>
        /// Registers a type mapping
        /// </summary>
        /// <param name="tsType">The TypeScript type</param>
        /// <param name="type">The native type</param>
        private void RegisterTypeMapping(TsType tsType, Type type)
        {
            this.TypeLookup[type] = tsType;
        }

        /// <summary>
        /// Registers custom type mapping
        /// </summary>
        /// <param name="tsType">The TypeScript type</param>
        /// <param name="type">The native type</param>
        /// <returns></returns>
        public Scripter WithTypeMapping(TsType tsType, Type type)
        {
            if (this.TypeLookup.ContainsKey(type))
            {
                throw new ArgumentException("Mapping for " + type.FullName + " is already defined.", "type");
            }
            this.TypeLookup[type] = tsType;
            return this;
        }

        /// <summary>
        /// Adds a particular type to be scripted
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The scripter</returns>
        public Scripter AddType(Type type)
        {
            this.Resolve(type);
            return this;
        }

        /// <summary>
        /// Adds a set of types to be scripted
        /// </summary>
        /// <param name="types">The types to add</param>
        /// <returns>The scripter</returns>
        public Scripter AddTypes(IEnumerable<Type> types)
        {
            foreach (var type in types)
                this.AddType(type);
            return this;
        }

        /// <summary>
        /// Adds all types from a particular assembly to the scripter.
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>The scripter</returns>
        public Scripter AddTypes(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            return AddTypes(new[] { assembly });
        }

        /// <summary>
        /// Adds all types from a set of assemblies to the scripter.
        /// </summary>
        /// <param name="assemblies">The assemblies</param>
        /// <returns>The scripter</returns>
        public Scripter AddTypes(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException("assemblies");

            foreach (var assembly in assemblies)
                AddTypes(this.Reader.GetTypes(assembly).Select(x => x.AsType()));
            return this;
        }

        /// <summary>
        /// Limits the resolution of types ot the specified assembly.
        /// All types outside of the assembly will be treated as type "any".
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>The scripter</returns>
        public Scripter UsingAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            return this.UsingAssemblies(new[] { assembly });
        }

        /// <summary>
        /// Limits the resolution of types to the specified assemblies.
        /// All types outside of the assemblies will be treated as type "any".
        /// </summary>
        /// <param name="assemblies">The assemblies</param>
        /// <returns>The scripter</returns>
        public Scripter UsingAssemblies(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException("assemblies");

            var assembliesLookup = new HashSet<Assembly>(assemblies);
            return this.UsingAssemblyFilter(x => assembliesLookup.Contains(x));
        }

        /// <summary>
        /// Limits the resolution of types to a particular set of assemblies.
        /// All types outside of the the filtered assemblies will be treated as type "any".
        /// </summary>
        /// <param name="filter">The assembly filter</param>
        /// <returns>The scripter</returns>
        public Scripter UsingAssemblyFilter(Func<Assembly, bool> filter)
        {
            this.AssemblyFilter = filter;
            return this;
        }

        /// <summary>
        /// Limits the resolution of types.
        /// Not matching types will be treated as type "any".
        /// </summary>
        /// <param name="filter">The type filter</param>
        /// <returns>The scripter</returns>
        public Scripter UsingTypeFilter(Func<Type, bool> filter)
        {
            this.TypeFilter = filter;
            return this;
        }
        #endregion

        #region Type Generation
        /// <summary>
        /// Generates a TypeScript interface for a particular CLR type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The resulting TypeScript interface</returns>
        private TsInterface GenerateInterface(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var tsInterface = new TsInterface(GetName(type));
            this.AddType(tsInterface, type);

            // resolve non-inherited interfaces implemented by the type
            var interfaces = typeInfo.BaseType == null ? typeInfo.GetInterfaces() : typeInfo.GetInterfaces().Except(typeInfo.BaseType.GetTypeInfo().GetInterfaces());
            foreach (var interfaceType in interfaces)
                this.AddType(interfaceType);

            if (typeInfo.IsGenericType)
            {
                if (typeInfo.IsGenericTypeDefinition)
                {
                    foreach (var genericArgument in typeInfo.GetGenericArguments())
                    {
                        var tsTypeParameter = new TsTypeParameter(new TsName(genericArgument.Name));
                        var genericArgumentType = genericArgument.GetTypeInfo().GetGenericParameterConstraints().FirstOrDefault();
                        if (genericArgumentType != null)
                        {
                            var tsTypeParameterType = Resolve(genericArgumentType);
                            tsTypeParameter.Extends = tsTypeParameterType.Name;
                        }
                        tsInterface.TypeParameters.Add(tsTypeParameter);
                    }
                }
                else
                {
                    var genericType = type.GetGenericTypeDefinition();
                    var tsGenericType = this.Resolve(genericType);
                }
            }

            // resolve the base class if present
            if (typeInfo.BaseType != null)
            {
                var baseType = this.Resolve(typeInfo.BaseType);
                if (baseType != null && baseType != TsPrimitive.Any)
                    tsInterface.BaseInterfaces.Add(baseType);
            }

            // process fields
            foreach (var field in this.Reader.GetFields(typeInfo))
            {
                var tsProperty = this.Resolve(field);
                if (tsProperty != null)
                    tsInterface.Properties.Add(tsProperty);
            }

            // process properties
            foreach (var property in this.Reader.GetProperties(typeInfo))
            {
                var tsProperty = this.Resolve(property);
                if (tsProperty != null)
                    tsInterface.Properties.Add(tsProperty);
            }

            // process methods
            foreach (var method in this.Reader.GetMethods(typeInfo))
            {
                var tsFunction = this.Resolve(method);
                if (tsFunction != null)
                    tsInterface.Functions.Add(tsFunction);
            }

            return tsInterface;
        }

        /// <summary>
        /// Generates a TypeScript enum for a particular CLR enum type
        /// </summary>
        /// <param name="type">The enum type</param>
        /// <returns>The resulting TypeScrpt enum</returns>
        private TsEnum GenerateEnum(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var names = typeInfo.GetEnumNames();
            var values = typeInfo.GetEnumValues();
            var entries = new Dictionary<string, long?>();
            for (int i = 0; i < values.Length; i++)
                entries.Add(names[i], Convert.ToInt64(values.GetValue(i)));
            var tsEnum = new TsEnum(GetName(type), entries);
            this.AddType(tsEnum, type);
            return tsEnum;
        }

        /// <summary>
        /// Gets the TypeScript type name for a particular type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The TypeScript type name</returns>
        protected virtual TsName GetName(Type type)
        {
            const char genericNameSymbol = '`';
            var typeInfo = type.GetTypeInfo();
            var typeName = type.Name;
            if (typeInfo.IsGenericType)
            {
                if (typeName.Contains(genericNameSymbol))
                    typeName = typeName.Substring(0, typeName.IndexOf(genericNameSymbol));
            }

            return new TsName(typeName, type.Namespace);
        }

        /// <summary>
        /// Gets the TypeScript name for a CLR parameter
        /// </summary>
        /// <param name="parameter">The parameter</param>
        /// <returns>The TypeScript name</returns>
        protected virtual TsName GetName(ParameterInfo parameter)
        {
            return new TsName(parameter.Name);
        }

        /// <summary>
        /// Gets the TypeScript name for a CLR member
        /// </summary>
        /// <param name="member">The member</param>
        /// <returns>The TypeScript name</returns>
        protected virtual TsName GetName(MemberInfo member)
        {
            return new TsName(member.Name);
        }
        #endregion

        #region Type Resolution
        /// <summary>
        /// Resolves a type
        /// </summary>
        /// <param name="type">The type to resolve</param>
        /// <returns>The TypeScript type definition</returns>
        protected TsType Resolve(Type type)
        {
            // see if we have already processed the type
            TsType tsType;
            if (!this.TypeLookup.TryGetValue(type, out tsType))
                tsType = OnResolve(type);

            this.AddType(tsType, type);
            return tsType;
        }

        /// <summary>
        /// Resolves a type
        /// </summary>
        /// <param name="type">The type to resolve</param>
        /// <returns>The TypeScript type definition</returns>
        protected virtual TsType OnResolve(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            TsType tsType;
            if (this.TypeLookup.TryGetValue(type, out tsType))
                return tsType;
            else if (this.AssemblyFilter != null && !this.AssemblyFilter(typeInfo.Assembly)) // should this assembly be considered?
                tsType = TsPrimitive.Any;
            else if (this.TypeFilter != null && !this.TypeFilter(type)) // should this assembly be considered?
                tsType = TsPrimitive.Any;
            else if (type.IsGenericParameter)
                tsType = new TsGenericType(new TsName(type.Name));
            else if (typeInfo.IsGenericType && !typeInfo.IsGenericTypeDefinition)
            {
                var tsGenericTypeDefinition = Resolve(type.GetGenericTypeDefinition());
                TsGenericType tsGenericType = new TsGenericType(tsGenericTypeDefinition.Name);
                foreach (var argument in type.GetTypeInfo().GetGenericArguments())
                {
                    var tsArgType = this.Resolve(argument);
                    tsGenericType.TypeArguments.Add(tsArgType);
                }
                tsType = tsGenericType;
            }
            else if (type.IsArray && type.HasElementType)
            {
                var elementType = this.Resolve(type.GetElementType());
                tsType = new TsArray(elementType, type.GetArrayRank());
            }
            else if (typeInfo.IsEnum)
                tsType = GenerateEnum(type);
            else if (typeInfo.IsAnsiClass)
                tsType = GenerateInterface(type);
            else if (typeInfo.IsInterface)
                tsType = GenerateInterface(type);
            else
                tsType = TsPrimitive.Any;

            return tsType;
        }

        /// <summary>
        /// Resolves a field
        /// </summary>
        /// <param name="field">The field to resolve</param>
        /// <returns></returns>
        protected virtual TsProperty Resolve(FieldInfo field)
        {
            return new TsProperty(GetName(field), Resolve(field.FieldType));
        }

        /// <summary>
        /// Resolves a property
        /// </summary>
        /// <param name="property">The property to resolve</param>
        /// <returns></returns>
        protected virtual TsProperty Resolve(PropertyInfo property)
        {
            TsType propertyType;
            bool optional = false;
            var propertyTypeInfo = property.PropertyType.GetTypeInfo();
            if (propertyTypeInfo.IsGenericType && propertyTypeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var genericArguments = propertyTypeInfo.GetGenericArguments();
                propertyType = this.Resolve(genericArguments[0]);
                optional = true;
            }
            else if (propertyTypeInfo.IsGenericType && !propertyTypeInfo.IsGenericTypeDefinition && propertyTypeInfo.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                // find type used for dictionary values
                var genericArguments = propertyTypeInfo.GetGenericArguments();
                var keyType = genericArguments[0];
                var valueType = genericArguments[1];
                var tsKeyType = this.Resolve(keyType);
                var tsArgType = this.Resolve(valueType);
                var inlineInterfaceType = new TsInterface();
                inlineInterfaceType.IndexerProperties.Add(new TsIndexerProperty(new TsName("key"), tsKeyType, tsArgType));
                propertyType = inlineInterfaceType;
            }
            else
            {
                propertyType = Resolve(property.PropertyType);
            }
            return new TsProperty(GetName(property), propertyType, optional);
        }

        /// <summary>
        /// Resolves a method
        /// </summary>
        /// <param name="method">The method to resolve</param>
        /// <returns>The TypeScript function definition</returns>
        protected virtual TsFunction Resolve(MethodInfo method)
        {
            var returnType = Resolve(method.ReturnType);
            var parameters = this.Reader.GetParameters(method);
            var tsFunction = new TsFunction(GetName(method));
            tsFunction.ReturnType = returnType;
            if (method.IsGenericMethod)
            {
                foreach (var genericArgument in method.GetGenericArguments())
                {
                    var tsTypeParameter = new TsTypeParameter(new TsName(genericArgument.Name));
                    tsFunction.TypeParameters.Add(tsTypeParameter);
                }
            }

            foreach (var param in parameters.Select(x => new TsParameter(GetName(x), Resolve(x.ParameterType))))
                tsFunction.Parameters.Add(param);

            return tsFunction;
        }
        #endregion

        #region Modules
        /// <summary>
        /// Returns the list of modules associated with the current set of resolved types
        /// </summary>
        /// <returns>The list of modules</returns>
        public IEnumerable<TsModule> Modules()
        {
            return this.Types
                .GroupBy(x => x.Name.Namespace)
                .Where(x => !string.IsNullOrEmpty(x.Key))
                .Select(x => new TsModule(new TsName(x.Key), x));
        }
        #endregion

        #region Output
        /// <summary>
        /// Configures the scripter to use a particular formatter
        /// </summary>
        /// <param name="formatter">The formatter</param>
        /// <returns></returns>
        public Scripter UsingFormatter(TsFormatter formatter)
        {
            this.Formatter = formatter;
            return this;
        }

        /// <summary>
        /// Saves the scripter output to a file
        /// </summary>
        /// <param name="file">The file path</param>
        /// <returns></returns>
        public Scripter SaveToFile(string file)
        {
            System.IO.File.WriteAllText(file, this.ToString());
            return this;
        }

        /// <summary>
        /// Saves the scripter output to a directory
        /// </summary>
        /// <param name="directory">The directory path</param>
        /// <returns></returns>
        public Scripter SaveToDirectory(string directory)
        {
            var includeContent = new StringBuilder();
            var includeRef = string.Format("/// <reference path=\"include.ts\" />{0}", Environment.NewLine);
            foreach (var module in this.Modules())
            {
                var fileName = module.Name.FullName + ".d.ts";
                var path = System.IO.Path.Combine(directory, fileName);
                var output = this.Formatter.Format(module);

                System.IO.File.WriteAllText(path, includeRef + Environment.NewLine + output);
                includeContent.AppendFormat("/// <reference path=\"{0}\" />", fileName);
                includeContent.AppendLine();
            }

            System.IO.File.WriteAllText(System.IO.Path.Combine(directory, "include.ts"), includeContent.ToString());

            // write the include file
            return this;
        }
        #endregion

        #region Helper Methods
        private TsType CreateGenericDictionaryType()
        {
            var tsInterface = new TsInterface(new TsName("Dictionary", "System.Collections.Generic"));
            var tsKeyType = new TsInterface(new TsName("TKey"));
            var tsValueType = new TsInterface(new TsName("TValue"));
            tsInterface.TypeParameters.Add(new TsTypeParameter(new TsName("Tkey")));
            tsInterface.TypeParameters.Add(new TsTypeParameter(new TsName("TValue")));
            tsInterface.IndexerProperties.Add(new TsIndexerProperty(new TsName("key"), TsPrimitive.String, tsValueType));
            return tsInterface;
        }
        #endregion
    }
}
