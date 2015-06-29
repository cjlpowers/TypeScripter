using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
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

		public IDictionary<string, string> ReservedWordsMapping
		{
			get;
			private set;
		}
		#endregion

		#region Creation
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
		public virtual string Format(TsModule module)
		{
			using (var sbc = new StringBuilderContext(this))
			{
				this.WriteIndent();
				this.Write("declare module {0} {{", Format(module.Name));
				this.WriteNewline();
				using (Indent())
				{
					foreach (var type in module.Types.OfType<TsEnum>())
						this.Write(this.Format(type));
					foreach (var type in module.Types.OfType<TsInterface>())
						this.Write(this.Format(type));
				}
				this.WriteIndent();
				this.Write("}");
				this.WriteNewline();
				return sbc.ToString();
			}
		}

		public virtual string Format(TsInterface tsInterface)
		{
			using (var sbc = new StringBuilderContext(this))
			{
				this.WriteIndent();
				this.Write("interface {0}{1} {2} {{",
					Format(tsInterface.Name), 
					Format(tsInterface.TypeParameters),
					tsInterface.BaseInterfaces.Count > 0 ? string.Format("extends {0}", string.Join(", ", tsInterface.BaseInterfaces.Select(Format))) : string.Empty);
				this.WriteNewline();
				using (Indent())
				{
					foreach (var property in tsInterface.Properties)
						this.Write(this.Format(property));

					foreach (var function in tsInterface.Functions)
						this.Write(this.Format(function));
				}
				this.WriteIndent();
				this.Write("}");
				this.WriteNewline();
				this.WriteNewline();
				return sbc.ToString();
			}
		}

		public virtual string Format(TsProperty property)
		{
			using (var sbc = new StringBuilderContext(this))
			{
				this.WriteIndent();
				this.Write("{0}: {1};", Format(property.Name), property.Type.Name.FullName);
				this.WriteNewline();
				return sbc.ToString();
			}
		}

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

        public virtual string FormatReturnType(TsType tsReturnType)
        {
            return Format(tsReturnType);
        }

		public virtual string Format(TsType tsType)
		{
			if (tsType is TsGenericType)
				return Format((TsGenericType)tsType);
			return tsType.Name.FullName;
		}

		public virtual string Format(TsEnum tsEnum)
		{
			using (var sbc = new StringBuilderContext(this))
			{
				this.WriteIndent();
				this.Write("enum {0} {{", Format(tsEnum.Name));
				this.WriteNewline();
				using (Indent())
				{
					var values = tsEnum.Values.ToArray();
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

		public virtual string Format(TsParameter parameter)
		{
			using (var sbc = new StringBuilderContext(this))
			{
				this.Write("{0}{1}: {2}", Format(parameter.Name), parameter.Optional ? "?" : string.Empty, parameter.Type.Name.FullName);
				return sbc.ToString();
			}
		}

		public virtual string Format(IEnumerable<TsParameter> parameters)
		{
			return string.Join(", ", parameters.Select(Format));
		}

		public virtual string Format(TsTypeParameter typeParameter)
		{
			return string.Format("{0}{1}", typeParameter.Name, typeParameter.Extends == null ? string.Empty : string.Format(" extends {0}", typeParameter.Extends.FullName));
		}

		public virtual string Format(IEnumerable<TsTypeParameter> typeParameters)
		{
			if (typeParameters.Count() == 0)
				return string.Empty;
			return string.Format("<{0}>", string.Join(", ",typeParameters.Select(Format)));
		}

		public virtual string Format(TsGenericType tsGenericType)
		{
			return string.Format("{0}{1}", tsGenericType.Name.FullName, tsGenericType.TypeArguments.Count > 0 ? string.Format("<{0}>", string.Join(", ", tsGenericType.TypeArguments.Select(Format))) : string.Empty);
		}

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
