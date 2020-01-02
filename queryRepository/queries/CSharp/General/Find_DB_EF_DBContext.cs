CxList classDecls = Find_ClassDecl();

CxList contextInherits = classDecls.InheritsFrom("ObjectContext");
contextInherits.Add(classDecls.InheritsFrom("DbContext"));
contextInherits.Add(classDecls.InheritsFrom("IdentityDbContext"));
contextInherits.Add(classDecls.InheritsFrom("TransactionContext"));
contextInherits.Add(classDecls.InheritsFrom("HistoryContext"));

CxList unknownRefs = Find_Unknown_References();
CxList contextInstances = unknownRefs.FindByType(contextInherits);

CxList interfaces = Find_InterfaceDecl();
CxList interfacesParams = Find_ParamDecl().FindByType(interfaces);
CxList contextParams = interfacesParams.DataInfluencedBy(contextInstances).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
contextInstances.Add(unknownRefs.FindAllReferences(contextParams));

contextInstances.Add(All.FindByTypes(new string[]{"DbContext",
	"ObjectContext","IdentityDbContext", "TransactionContext", "HistoryContext"}));

result = contextInstances;