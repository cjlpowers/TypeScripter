using System;
using System.Collections.Generic;
using System.Reflection;

using TypeScripter.TypeScript;

namespace TypeScripter.Resolvers
{
	public interface ITypeResolver
	{
		TsType Resolve(Type type);
	}
}