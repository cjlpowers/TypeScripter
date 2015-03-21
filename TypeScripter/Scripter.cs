using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using TypeScripter.TypeScript;

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

		private TypeReader Reader
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
				{ typeof(System.Object), TsPrimitive.Any }
			};
			this.Reader = new TypeReader();
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

		public Scripter WithTypeReader(TypeReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");
			this.Reader = reader;
			return this;
		}

		public Scripter AddType(Type type)
		{
			GenerateType(type);
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
			return AddTypes(this.Reader.GetTypes(assembly));
		}
		
		protected virtual TsType GenerateType(Type type)
		{
			// see if we have already processed the type
			TsType tsType;
			if (this.TypeLookup.TryGetValue(type, out tsType))
				return tsType;

			if (type.IsGenericType)
			{
				// TODO
				return TsPrimitive.Any;
			}

			if (type.IsPrimitive)
			{
				if (type == typeof(bool))
					return TsPrimitive.Boolean;
				else if (type == typeof(int) || type == typeof(float) || type == typeof(double))
					return TsPrimitive.Number;
				else if (type == typeof(string))
					return TsPrimitive.String;
				else if (type == typeof(void))
					return TsPrimitive.Void;
				else
					return TsPrimitive.Any;
			}
			else if (type.FullName.StartsWith("System"))
				return TsPrimitive.Any;
			else if (type.IsClass)
				return GenerateInterface(type);
			else if (type.IsInterface)
				return GenerateInterface(type);
			else
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
				tsInterface.Properties.Add(new TsProperty(GetName(property), GenerateType(property.PropertyType)));

			// process methods
			foreach (var method in this.Reader.GetMethods(type))
			{
				var returnType = GenerateType(method.ReturnType);
				var parameters = this.Reader.GetParameters(method);
				var tsFunction = new TsFunction(GetName(method));
				tsFunction.ReturnType = returnType;
				foreach (var param in parameters.Select(x => new TsParameter(GetName(x), GenerateType(x.ParameterType))))
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

		#region Savings

		public Scripter WithFormatter(TsFormatter writer)
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
