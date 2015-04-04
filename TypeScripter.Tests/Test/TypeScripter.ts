declare module TypeScripter {
	interface Scripter  {
		ToString(): string;
		UsingTypeReader(reader: TypeScripter.Readers.ITypeReader): TypeScripter.Scripter;
		AddType(type: any): TypeScripter.Scripter;
		AddTypes(types: any): TypeScripter.Scripter;
		AddTypes(assembly: any): TypeScripter.Scripter;
		AddTypes(assemblies: any): TypeScripter.Scripter;
		UsingAssembly(assembly: any): TypeScripter.Scripter;
		UsingAssemblies(assemblies: any): TypeScripter.Scripter;
		UsingAssemblyFilter(filter: any): TypeScripter.Scripter;
		UsingTypeFilter(filter: any): TypeScripter.Scripter;
		Modules();
		UsingFormatter(writer: TypeScripter.TypeScript.TsFormatter): TypeScripter.Scripter;
		SaveToFile(path: string): TypeScripter.Scripter;
		SaveToDirectory(directory: string): TypeScripter.Scripter;
	}

}
declare module TypeScripter.Readers {
	interface ITypeReader  {
		GetMethods(type: any);
		GetParameters(method: any);
		GetProperties(type: any);
		GetTypes(assembly: any);
	}

	interface CompositeTypeReader extends TypeScripter.Readers.DefaultTypeReader {
		GetTypes(assembly: any);
		GetProperties(type: any);
		GetMethods(type: any);
		GetParameters(method: any);
	}

	interface DefaultTypeReader  {
		GetTypes(assembly: any);
		GetProperties(type: any);
		GetMethods(type: any);
		GetParameters(method: any);
	}

	interface ServiceContractTypeReader extends TypeScripter.Readers.DefaultTypeReader {
		GetTypes(assembly: any);
		GetProperties(type: any);
		GetMethods(type: any);
		GetParameters(method: any);
	}

	interface DataContractTypeReader extends TypeScripter.Readers.DefaultTypeReader {
		GetTypes(assembly: any);
		GetProperties(type: any);
		GetMethods(type: any);
		GetParameters(method: any);
	}

}
declare module TypeScripter.TypeScript {
	interface TsFormatter  {
		ReservedWordsMapping: any;
		Format(module: TypeScripter.TypeScript.TsModule): string;
		Format(tsInterface: TypeScripter.TypeScript.TsInterface): string;
		Format(property: TypeScripter.TypeScript.TsProperty): string;
		Format(_function: TypeScripter.TypeScript.TsFunction): string;
		Format(tsType: TypeScripter.TypeScript.TsType): string;
		Format(tsEnum: TypeScripter.TypeScript.TsEnum): string;
		Format(parameter: TypeScripter.TypeScript.TsParameter): string;
		Format(parameters: any): string;
		Format(typeParameter: TypeScripter.TypeScript.TsTypeParameter): string;
		Format(typeParameters: any): string;
		Format(tsGenericType: TypeScripter.TypeScript.TsGenericType): string;
		Format(name: TypeScripter.TypeScript.TsName): string;
	}

	interface TsModule extends TypeScripter.TypeScript.TsObject {
		Types: any;
	}

	interface TsObject  {
		Name: TypeScripter.TypeScript.TsName;
		ToString(): string;
	}

	interface TsName  {
		Namespace: string;
		Name: string;
		FullName: string;
		ToString(): string;
	}

	interface TsInterface extends TypeScripter.TypeScript.TsType {
		Properties: any;
		Functions: any;
		BaseInterfaces: any;
		TypeParameters: any;
	}

	interface TsType extends TypeScripter.TypeScript.TsObject {
		ToString(): string;
	}

	interface TsProperty extends TypeScripter.TypeScript.TsObject {
		Optional: boolean;
		Type: TypeScripter.TypeScript.TsType;
	}

	interface TsFunction extends TypeScripter.TypeScript.TsType {
		ReturnType: TypeScripter.TypeScript.TsType;
		Parameters: any;
		TypeParameters: any;
	}

	interface TsEnum extends TypeScripter.TypeScript.TsType {
		Values: any;
	}

	interface TsParameter extends TypeScripter.TypeScript.TsObject {
		Optional: boolean;
		Type: TypeScripter.TypeScript.TsType;
	}

	interface TsTypeParameter extends TypeScripter.TypeScript.TsObject {
		Extends: TypeScripter.TypeScript.TsName;
	}

	interface TsGenericType extends TypeScripter.TypeScript.TsType {
		TypeArguments: any;
	}

	interface TsPrimitive extends TypeScripter.TypeScript.TsType {
	}

	interface TsArray extends TypeScripter.TypeScript.TsPrimitive {
	}

}
