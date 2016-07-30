using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TypeScripter.TypeScript;

namespace TypeScripter.Readers
{
	/// <summary>
	/// A TypeReader implementation that allows one to combine the output of other TypeReaders
	/// </summary>
	public class CompositeTypeReader : TypeReader
	{
		#region Prperties
		private IEnumerable<TypeReader> Readers
		{
			get;
			set;
		}
		#endregion

		#region Creation
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="readers">The type readers to composite</param>
		public CompositeTypeReader(params TypeReader[] readers)
		{
			this.Readers = readers;
		}
        #endregion

        #region ITypeReader
        /// <summary>
        /// Gets types from an assembly
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>The list of types</returns>
        public override IEnumerable<TypeInfo> GetTypes(Assembly assembly)
		{
			return this.Readers
				.SelectMany(x => x.GetTypes(assembly))
				.Distinct();
		}

        /// <summary>
		/// Gets the fields defined on a particular type
		/// </summary>
		/// <param name="type">The type</param>
		/// <returns>The fields</returns>
		public override IEnumerable<FieldInfo> GetFields(TypeInfo type)
        {
            return this.Readers
                .SelectMany(x => x.GetFields(type))
                .Distinct();
        }

        /// <summary>
		/// Gets the properties defined on a particular type
		/// </summary>
		/// <param name="type">The type</param>
		/// <returns>The properties</returns>
		public override IEnumerable<PropertyInfo> GetProperties(TypeInfo type)
		{
			return this.Readers
				.SelectMany(x => x.GetProperties(type))
				.Distinct();
		}

        /// <summary>
		/// Gets the methods defined on a particular type
		/// </summary>
		/// <param name="type">The type</param>
		/// <returns>The methods</returns>
		public override IEnumerable<MethodInfo> GetMethods(TypeInfo type)
		{
			return this.Readers
				.SelectMany(x => x.GetMethods(type))
				.Distinct();
		}

        /// <summary>
		/// Gets the parameters of a particular method
		/// </summary>
		/// <param name="method">The method</param>
		/// <returns>The parameters</returns>
		public override IEnumerable<ParameterInfo> GetParameters(MethodInfo method)
		{
			return this.Readers
				.SelectMany(x => x.GetParameters(method))
				.Distinct();
		}
		#endregion
	}
}