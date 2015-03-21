declare module TypeScripter {
	interface TypeReader {
		GetTypes(assembly: any);
		GetProperties(type: any);
		GetMethods(type: any);
		GetParameters(method: any);
	}
	interface Scripter {
		ToString();
		WithTypeReader(reader: TypeScripter.TypeReader) :TypeScripter.Scripter;
		AddType(type: any) :TypeScripter.Scripter;
		AddTypes(types: any) :TypeScripter.Scripter;
		AddTypes(assembly: any) :TypeScripter.Scripter;
		Modules();
		WithFormatter(writer: TypeScripter.TypeScript.TsFormatter) :TypeScripter.Scripter;
		SaveToFile(path: any) :TypeScripter.Scripter;
		SaveToDirectory(directory: any) :TypeScripter.Scripter;
	}
}
declare module TypeScripter.TypeScript {
	interface TsFormatter {
		ReservedWordsMapping: any;
		Format(module: TypeScripter.TypeScript.TsModule);
		Format(tsInterface: TypeScripter.TypeScript.TsInterface);
		Format(property: TypeScripter.TypeScript.TsProperty);
		Format(_function: TypeScripter.TypeScript.TsFunction);
		Format(tsEnum: TypeScripter.TypeScript.TsEnum);
		Format(parameter: TypeScripter.TypeScript.TsParameter);
		Format(name: TypeScripter.TypeScript.TsName);
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
		ToString();
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
		Namespace: any;
		Name: any;
		FullName: any;
	}
	interface TsPrimitive {
	}
	interface TsArray {
	}
	interface TsObject {
		Name: TypeScripter.TypeScript.TsName;
	}
}
