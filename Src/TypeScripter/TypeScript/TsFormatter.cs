using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	/// <summary>
	/// A class which is responsible for rendering the TypeScript output
	/// </summary>
	public class TsFormatter
	{
		#region Internal Constructs
		private class StringBuilderContext : IDisposable
		{
			StringBuilderContext PriorContext
			{
				get;
				set;
			}

			public StringBuilder StringBuilder
			{
				get;
				set;
			}

			public int IndentLevel
			{
				get;
				set;
			}

			TsFormatter Writer
			{
				get;
				set;
			}

			public StringBuilderContext(TsFormatter writer)
			{
				this.Writer = writer;
				this.PriorContext = writer.Context;
				this.IndentLevel = this.PriorContext != null ? this.PriorContext.IndentLevel : 0;
				this.StringBuilder = new StringBuilder();
				this.Writer.Context = this;
			}

			public override string ToString()
			{
				return this.StringBuilder.ToString();
			}

			void IDisposable.Dispose()
			{
				if (this.PriorContext != null)
				{
					this.Writer.Context = this.PriorContext;
					this.PriorContext = null;
				}
			}
		}

		private class IndentContext : IDisposable
		{
			private TsFormatter mFormatter;
			public IndentContext(TsFormatter formatter)
			{
				this.mFormatter = formatter;
				this.mFormatter.Context.IndentLevel++;
			}

			void IDisposable.Dispose()
			{
				if (this.mFormatter != null)
				{
					this.mFormatter.Context.IndentLevel--;
					this.mFormatter = null;
				}
			}
		}
		#endregion

		#region Properties
		private StringBuilderContext Context
		{
			get;
			set;
		}

        /// <summary>
        /// The mapping of reserved words
        /// </summary>
		public IDictionary<string, string> ReservedWordsMapping
		{
			get;
			private set;
		}
		#endregion

		#region Creation
        /// <summary>
        /// Constructor
        /// </summary>
		public TsFormatter()
		{
			this.Context = new StringBuilderContext(this);
			this.ReservedWordsMapping = new Dictionary<string, string>()
			{
				{"function","_function"}
			};
		}
		#endregion

		#region Writer
        /// <summary>
        /// Formats a module
        /// </summary>
        /// <param name="module">The module</param>
        /// <returns>The string representation of the module</returns>
		public virtual string Format(TsModule module)
		{
			using (var sbc = new StringBuilderContext(this))
			{
				this.WriteIndent();
				this.Write("declare module {0} {{", Format(module.Name));
				this.WriteNewline();
				using (Indent())
				{
					foreach (var type in module.Types.OfType<TsEnum>().OrderBy(x=>x.Name))
						this.Write(this.Format(type));
					foreach (var type in module.Types.OfType<TsInterface>().OrderBy(x=>x.Name))
						this.Write(this.Format(type));
				}
				this.WriteIndent();
				this.Write("}");
				this.WriteNewline();
				return sbc.ToString();
			}
		}

        /// <summary>
        /// Formats an interface
        /// </summary>
        /// <param name="tsInterface">The interface</param>
        /// <returns>The string representation of the interface</returns>
		public virtual string Format(TsInterface tsInterface)
		{
			using (var sbc = new StringBuilderContext(this))
			{
				this.WriteIndent();
				this.Write("interface {0}{1} {2} {{",
					Format(tsInterface.Name),
					Format(tsInterface.TypeParameters),
                    tsInterface.BaseInterfaces.Count > 0 ? string.Format("extends {0}", string.Join(", ", tsInterface.BaseInterfaces.OrderBy(x => x.Name).Select(Format))) : string.Empty);
				this.WriteNewline();
				using (Indent())
				{
                    foreach (var property in tsInterface.Properties.OrderBy(x => x.Name))
						this.Write(this.Format(property));

                    foreach (var function in tsInterface.Functions.OrderBy(x => x.Name))
						this.Write(this.Format(function));
				}
				this.WriteIndent();
				this.Write("}");
				this.WriteNewline();
				this.WriteNewline();
				return sbc.ToString();
			}
		}

        /// <summary>
        /// Formats a property
        /// </summary>
        /// <param name="property">The property</param>
        /// <returns>The string representation of the property</returns>
		public virtual string Format(TsProperty property)
		{
			using (var sbc = new StringBuilderContext(this))
			{
				this.WriteIndent();
				this.Write("{0}: {1};", Format(property.Name), Format(property.Type));
				this.WriteNewline();
				return sbc.ToString();
			}
		}

        /// <summary>
        /// Formats a function
        /// </summary>
        /// <param name="function">The function</param>
        /// <returns>The string representation of the function</returns>
		public virtual string Format(TsFunction function)
		{
			using (var sbc = new StringBuilderContext(this))
			{
				this.WriteIndent();
				this.Write("{0}{1}({2}){3};",
					Format(function.Name),
					Format(function.TypeParameters),
					Format(function.Parameters),
					function.ReturnType == TsPrimitive.Any ? string.Empty : string.Format(": {0}", FormatReturnType(function.ReturnType))
				);
				this.WriteNewline();
				return sbc.ToString();
			}
		}

        /// <summary>
        /// Formats a return type
        /// </summary>
        /// <param name="tsReturnType">The return type</param>
        /// <returns>The string representation of the return type</returns>
		public virtual string FormatReturnType(TsType tsReturnType)
		{
			return Format(tsReturnType);
		}

        /// <summary>
        /// Formats a type
        /// </summary>
        /// <param name="tsType">The type</param>
        /// <returns>The string representation of the type</returns>
		public virtual string Format(TsType tsType)
		{
			if (tsType is TsGenericType)
				return Format((TsGenericType)tsType);
			return tsType.Name.FullName;
		}

        /// <summary>
        /// Formats an enumeration
        /// </summary>
        /// <param name="tsEnum">The enumeration</param>
        /// <returns>The string representation of the enumeration</returns>
		public virtual string Format(TsEnum tsEnum)
		{
			using (var sbc = new StringBuilderContext(this))
			{
				this.WriteIndent();
				this.Write("const enum {0} {{", Format(tsEnum.Name));
				this.WriteNewline();
				using (Indent())
				{
					var values = tsEnum.Values.OrderBy(x=>x.Key).ToArray();
					for (int i = 0; i < values.Length; i++)
					{
						var postFix = i < values.Length - 1 ? "," : string.Empty;
						var entry = values[i];
						this.WriteIndent();
						if (entry.Value.HasValue)
							this.Write("{0} = {1}{2}", entry.Key, entry.Value, postFix);
						else
							this.Write("{0}{1}", entry.Key, postFix);
						this.WriteNewline();
					}
				}
				this.WriteIndent();
				this.Write("}");
				this.WriteNewline();
				return sbc.ToString();
			}
		}

        /// <summary>
        /// Formats a parameter
        /// </summary>
        /// <param name="parameter">The parameter</param>
        /// <returns>The string representation of a parameter</returns>
		public virtual string Format(TsParameter parameter)
		{
			using (var sbc = new StringBuilderContext(this))
			{
				this.Write("{0}{1}: {2}", Format(parameter.Name), parameter.Optional ? "?" : string.Empty, Format(parameter.Type));
				return sbc.ToString();
			}
		}

        /// <summary>
        /// Formats a set of parameters
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <returns>The string representation of the parameters</returns>
		public virtual string Format(IEnumerable<TsParameter> parameters)
		{
			return string.Join(", ", parameters.Select(Format));
		}

        /// <summary>
        /// Formats a type parameter
        /// </summary>
        /// <param name="typeParameter">The type parameter</param>
        /// <returns>The string representaton of the type parameter</returns>
		public virtual string Format(TsTypeParameter typeParameter)
		{
			return string.Format("{0}{1}", typeParameter.Name, typeParameter.Extends == null ? string.Empty : string.Format(" extends {0}", typeParameter.Extends.FullName));
		}

        /// <summary>
        /// Formats a set of type parameters
        /// </summary>
        /// <param name="typeParameters">The type parameters</param>
        /// <returns>The string representation fo the type parameters</returns>
		public virtual string Format(IEnumerable<TsTypeParameter> typeParameters)
		{
			if (typeParameters.Count() == 0)
				return string.Empty;
			return string.Format("<{0}>", string.Join(", ", typeParameters.Select(Format)));
		}

        /// <summary>
        /// Formats a generic type
        /// </summary>
        /// <param name="tsGenericType">The generic type</param>
        /// <returns>The string representation of the generic type</returns>
		public virtual string Format(TsGenericType tsGenericType)
		{
			return string.Format("{0}{1}", tsGenericType.Name.FullName, tsGenericType.TypeArguments.Count > 0 ? string.Format("<{0}>", string.Join(", ", tsGenericType.TypeArguments.Select(Format))) : string.Empty);
		}

        /// <summary>
        /// Formats a name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>The string representation of the name</returns>
		public virtual string Format(TsName name)
		{
			if (name == null || name.Name == null)
				return string.Empty;
			string result = null;
			if (!this.ReservedWordsMapping.TryGetValue(name.Name, out result))
				result = name.Name;
			return result;
		}
		#endregion

		#region Methods
		private void Write(string output)
		{
			this.Context.StringBuilder.Append(output);
		}

		private void Write(string format, params object[] args)
		{
			Write(string.Format(format, args));
		}

		private void WriteIndent()
		{
			var indent = string.Empty;
			for (int i = 0; i < this.Context.IndentLevel; i++)
				indent += "\t";
			Write(indent);
		}

		private void WriteNewline()
		{
			this.Write(Environment.NewLine);
		}

		private IndentContext Indent()
		{
			return new IndentContext(this);
		}
		#endregion
	}
}
