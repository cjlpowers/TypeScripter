using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using TypeScripter.TypeScript;
using TypeScripter.Readers;
using TypeScripter.Resolvers;

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


		private ITypeResolver Resolver
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
		public Scripter()
		{
			this.TypeLookup = new Dictionary<Type, TsType>()
			{
				{ typeof(void), TsPrimitive.Void },
                { typeof(object), TsPrimitive.Any },
                { typeof(string), TsPrimitive.String },
				{ typeof(bool), TsPrimitive.Boolean },
				{ typeof(int), TsPrimitive.Number },
				{ typeof(float), TsPrimitive.Number },
			};
			this.Reader = new DefaultTypeReader();
			this.Resolver = new DefaultTypeResolver();
			this.Writer = new TsFormatter();
		}
		#endregion

		#region Methods
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

		public Scripter AddTypes(IEnumerable<Type> types)
		{
			foreach (var type in types)
				this.AddType(type);
			return this;
		}

		public Scripter AddTypes(Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			return AddTypes(new [] { assembly });
		}

		public Scripter AddTypes(IEnumerable<Assembly> assemblies)
		{
			if (assemblies == null)
				throw new ArgumentNullException("assemblies");

			this.UsingAssemblies(assemblies);
			foreach (var assembly in assemblies)
				AddTypes(this.Reader.GetTypes(assembly));
			return this;
		}

		public Scripter UsingAssembly(Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");
			return this.UsingAssemblies(new[] { assembly });
		}

		public Scripter UsingAssemblies(IEnumerable<Assembly> assemblies)
		{
			if (assemblies == null)
				throw new ArgumentNullException("assemblies");

			var assembliesLookup = new HashSet<Assembly>(assemblies);
			return this.UsingAssemblyFilter(x => assembliesLookup.Contains(x));
		}

		public Scripter UsingAssemblyFilter(Func<Assembly, bool> filter)
		{
			this.AssemblyFilter = filter;
			return this;
		}

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

			if (type.IsArray && type.HasElementType) 
			{
				var elementType = this.Resolve(type.GetElementType());
				return new TsArray(elementType, type.GetArrayRank());
			}

			// resolve the type
			tsType = this.Resolver.Resolve(type);
			if (tsType != null)
				return tsType;

			if (type.IsGenericType)
			{
				// TODO
				return TsPrimitive.Any;
			}
			else if (type.IsAnsiClass)
				return GenerateInterface(type);
			else if (type.IsInterface)
				return GenerateInterface(type);

			return TsPrimitive.Any;
		}
		#endregion

		#region Type Generation
		private TsInterface GenerateInterface(Type type)
		{
			var tsInterface = new TsInterface(GetName(type));
			this.TypeLookup.Add(type, tsInterface);

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

		protected virtual TsName GetName(Type type)
		{
			return new TsName(type.Name, type.Namespace);
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
