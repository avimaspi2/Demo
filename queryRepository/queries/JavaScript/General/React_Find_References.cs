CxList unknownReferences = Find_UnknownReference();
CxList imports = Find_Import();

List <string> reactReferences = new List<string>{ "React", "ReactDOM", "ReactBrowserEventEmitter" };
CxList requireReferences = unknownReferences.FindByShortNames(reactReferences);

requireReferences.Add(Find_Require("react") - imports);

result = requireReferences;