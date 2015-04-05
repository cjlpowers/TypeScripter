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
	public class Scripter
	{
		#region Properties
		private Dictionary<Type, TsType> TypeLookup
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

		private ITypeReader Reader
		{
			get;
			set;
		}

		private TsFormatter Writer
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
			this.Reader = new DefaultTypeReader();
			this.Writer = new TsFormatter();
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
				str.Append(this.Writer.Format(module));
			return str.ToString();
		}
		#endregion

		#region Operations
		public Scripter UsingTypeReader(ITypeReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");
			this.Reader = reader;
			return this;
		}

		public Scripter AddType(Type type)
		{
			this.Resolve(type);
			return this;
		}

		/// <summary>
		/// Adds a set of types to the scripter.
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

			return AddTypes(new [] { assembly });
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
				AddTypes(this.Reader.GetTypes(assembly));
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

		#region Type Resolution
		protected virtual TsType Resolve(Type type)
		{
			// see if we have already processed the type
			TsType tsType;
			if (this.TypeLookup.TryGetValue(type, out tsType))
				return tsType;

			// should this assembly be considered?
			if (this.AssemblyFilter != null && !this.AssemblyFilter(type.Assembly))
				return TsPrimitive.Any;

			// should this assembly be considered?
			if (this.TypeFilter != null && !this.TypeFilter(type))
				return TsPrimitive.Any;

			if (type.IsGenericParameter)
				return new TsGenericType(new TsName(type.Name));
			else if (type.IsGenericType && !type.IsGenericTypeDefinition)
			{
				var tsGenericTypeDefinition = Resolve(type.GetGenericTypeDefinition());
				var tsGenericType = new TsGenericType(tsGenericTypeDefinition.Name);
				foreach (var argument in type.GetGenericArguments())
				{
					var tsArgType = this.Resolve(argument);
					tsGenericType.TypeArguments.Add(tsArgType);
                }
				return tsGenericType;
			}
			else if (type.IsArray && type.HasElementType)
			{
				var elementType = this.Resolve(type.GetElementType());
				return new TsArray(elementType, type.GetArrayRank());
			}
			else if (type.IsEnum)
				tsType = GenerateEnum(type);
			else if (type.IsAnsiClass)
				tsType = GenerateInterface(type);
			else if (type.IsInterface)
				tsType = GenerateInterface(type);
			else
				tsType = TsPrimitive.Any;

			// add the lookup
			if (!this.TypeLookup.ContainsKey(type))
				this.TypeLookup.Add(type, tsType);
			return tsType;
		}
		#endregion

		#region Type Generation
		private TsInterface GenerateInterface(Type type)
		{
			var tsInterface = new TsInterface(GetName(type));
			if (!this.TypeLookup.ContainsKey(type))
				this.TypeLookup.Add(type, tsInterface);

			// resolve interfaces implemented by the type
			foreach (var interfaceType in type.GetInterfaces())
				this.AddType(interfaceType);

			if (type.IsGenericType)
			{
				if (type.IsGenericTypeDefinition)
				{
					foreach (var genericArgument in type.GetGenericArguments())
					{
						var tsTypeParameter = new TsTypeParameter(new TsName(genericArgument.Name));
						var genericArgumentType = genericArgument.GetGenericParameterConstraints().FirstOrDefault();
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
			if (type.BaseType != null)
			{
				var baseType = this.Resolve(type.BaseType);
				if (baseType != null && baseType != TsPrimitive.Any)
					tsInterface.BaseInterfaces.Add(baseType);
			}

			// process properties
			foreach (var property in this.Reader.GetProperties(type))
				tsInterface.Properties.Add(new TsProperty(GetName(property), Resolve(property.PropertyType)));

			// process methods
			foreach (var method in this.Reader.GetMethods(type))
			{
				var returnType = Resolve(method.ReturnType);
				var parameters = this.Reader.GetParameters(method);
				var tsFunction = new TsFunction(GetName(method));
				tsFunction.ReturnType = returnType;
				foreach (var param in parameters.Select(x => new TsParameter(GetName(x), Resolve(x.ParameterType))))
					tsFunction.Parameters.Add(param);
				tsInterface.Functions.Add(tsFunction);
			}

			return tsInterface;
		}

		private TsEnum GenerateEnum(Type type)
		{
			var names = type.GetEnumNames();
			var values = type.GetEnumValues();
			var entries = new Dictionary<string, long?>();
			for (int i = 0; i < values.Length; i++)
				entries.Add(names[i], Convert.ToInt64(values.GetValue(i)));
			var tsEnum = new TsEnum(GetName(type), entries);
			this.TypeLookup.Add(type, tsEnum);
			return tsEnum;
		}

		protected virtual TsName GetName(Type type)
		{
			const char genericNameSymbol = '`';

            var typeName = type.Name;
			if (type.IsGenericType)
			{
				if(typeName.Contains(genericNameSymbol))
					typeName = typeName.Substring(0, typeName.IndexOf(genericNameSymbol));

			}

			return new TsName(typeName, type.Namespace);
		}

		protected virtual TsName GetName(ParameterInfo parameter)
		{
			return new TsName(parameter.Name);
		}

		protected virtual TsName GetName(MemberInfo member)
		{
			return new TsName(member.Name);
		}
		#endregion

		#region Modules
		public IEnumerable<TsModule> Modules()
		{
			return this.TypeLookup.Values
				.GroupBy(x => x.Name.Namespace)
				.Where(x => !string.IsNullOrEmpty(x.Key))
				.Select(x => new TsModule(new TsName(x.Key), x));
		}
		#endregion

		#region Output
		public Scripter UsingFormatter(TsFormatter writer)
		{
			this.Writer = writer;
			return this;
		}

		public Scripter SaveToFile(string path)
		{
			System.IO.File.WriteAllText(path, this.ToString());
			return this;
		}

		public Scripter SaveToDirectory(string directory)
		{
			var includeContent = new StringBuilder();
			var includeRef = string.Format("/// <reference path=\"include.ts\" />{0}", Environment.NewLine);
			foreach (var module in this.Modules())
			{
				var fileName = module.Name.FullName + ".d.ts";
				var path = System.IO.Path.Combine(directory, fileName);
				var output = this.Writer.Format(module);

				System.IO.File.WriteAllText(path, includeRef + Environment.NewLine + output);
				includeContent.AppendFormat("/// <reference path=\"{0}\" />", fileName);
                includeContent.AppendLine();
            }

			System.IO.File.WriteAllText(System.IO.Path.Combine(directory, "include.ts"), includeContent.ToString());

			// write the include file
			return this;
		}
		#endregion
	}
}
