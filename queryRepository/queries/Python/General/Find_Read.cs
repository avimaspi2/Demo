CxList methods = Find_Methods();

List<string> methodsNames = new List<string>{"read","readline","readlines"};
result.Add(methods.FindByShortNames(methodsNames));

// Pickle file loading
result.Add(Find_Methods_By_Import("pickle", new string[]{"load","loads"}));
result.Add(Find_Methods_By_Import("cPickle", new string[]{"load","loads"}));
CxList classes = Find_ClassDecl();
CxList unpicklers = classes.InheritsFrom("pickle.Unpickler");
foreach(CxList unpickler in unpicklers){
	CSharpGraph unpickl = unpickler.GetFirstGraph();
	result.Add(methods.FindByMemberAccess(unpickl.ShortName + "." + "load"));
}