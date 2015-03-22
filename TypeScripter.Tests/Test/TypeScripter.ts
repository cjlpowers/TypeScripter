declare module TypeScripter {
	interface Scripter {
		ToString() :string;
		UsingTypeReader(reader: TypeScripter.Readers.ITypeReader) :TypeScripter.Scripter;
		AddType(type: any) :TypeScripter.Scripter;
		AddTypes(types: any) :TypeScripter.Scripter;
		AddTypes(assembly: any) :TypeScripter.Scripter;
		AddTypes(assemblies: any) :TypeScripter.Scripter;
		UsingAssembly(assembly: any) :TypeScripter.Scripter;
		UsingAssemblies(assemblies: any) :TypeScripter.Scripter;
		UsingAssemblyFilter(filter: any) :TypeScripter.Scripter;
		UsingTypeFilter(filter: any) :TypeScripter.Scripter;
		Modules();
		UsingFormatter(writer: TypeScripter.TypeScript.TsFormatter) :TypeScripter.Scripter;
		SaveToFile(path: string) :TypeScripter.Scripter;
		SaveToDirectory(directory: string) :TypeScripter.Scripter;
	}
}
declare module TypeScripter.Readers {
	interface ITypeReader {
		GetMethods(type: any);
		GetParameters(method: any);
		GetProperties(type: any);
		GetTypes(assembly: any);
	}
	interface CompositeTypeReader {
		GetTypes(assembly: any);
		GetProperties(type: any);
		GetMethods(type: any);
		GetParameters(method: any);
	}
	interface ServiceContractTypeReader {
		GetTypes(assembly: any);
		GetProperties(type: any);
		GetMethods(type: any);
		GetParameters(method: any);
	}
	interface DataContractTypeReader {
		GetTypes(assembly: any);
		GetProperties(type: any);
		GetMethods(type: any);
		GetParameters(method: any);
	}
	interface DefaultTypeReader {
		GetTypes(assembly: any);
		GetProperties(type: any);
		GetMethods(type: any);
		GetParameters(method: any);
	}
}
declare module TypeScripter.TypeScript {
	interface TsFormatter {
		ReservedWordsMapping: any;
		Format(module: TypeScripter.TypeScript.TsModule) :string;
		Format(tsInterface: TypeScripter.TypeScript.TsInterface) :string;
		Format(property: TypeScripter.TypeScript.TsProperty) :string;
		Format(_function: TypeScripter.TypeScript.TsFunction) :string;
		FormatFunctionReturn(returnType: TypeScripter.TypeScript.TsType) :string;
		Format(tsEnum: TypeScripter.TypeScript.TsEnum) :string;
		Format(parameter: TypeScripter.TypeScript.TsParameter) :string;
		Format(name: TypeScripter.TypeScript.TsName) :string;
	}
	interface TsModule {
		Types: any;
	}
	interface TsInterface {
		Properties: any;
		Functions: any;
	}
	interface TsProperty {
		Optional: boolean;
		Type: TypeScripter.TypeScript.TsType;
	}
	interface TsType {
		ToString() :string;
	}
	interface TsFunction {
		ReturnType: TypeScripter.TypeScript.TsType;
		Parameters: any;
	}
	interface TsEnum {
		Values: any;
	}
	interface TsParameter {
		Optional: boolean;
		Type: TypeScripter.TypeScript.TsType;
	}
	interface TsName {
		Namespace: string;
		Name: string;
		FullName: string;
	}
	interface TsPrimitive {
	}
	interface TsArray {
	}
	interface TsObject {
		Name: TypeScripter.TypeScript.TsName;
	}
}
declare module TypeScripter.Resolvers {
	interface DefaultTypeResolver {
		SourceAssemblies: any;
		Resolve(type: any) :TypeScripter.TypeScript.TsType;
	}
	interface ITypeResolver {
		Resolve(type: any) :TypeScripter.TypeScript.TsType;
	}
}
