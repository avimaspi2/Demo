CxList apex = Find_Apex_Files();
CxList future = apex.FindByCustomAttribute("future");
future = future.GetAncOfType(typeof(MethodDecl));

CxList methods = apex.FindAllReferences(future);
CxList methodsDecl = methods.GetAncOfType(typeof(MethodDecl));

int numMeth = 0;
for(int i = 0; i < 5 && methods.Count > numMeth; i++)
{
	numMeth = methods.Count;
	methods.Add(apex.FindAllReferences(methodsDecl));
}

CxList futurePlus = future + methods;

CxList iterations = futurePlus.GetAncOfType(typeof(IterationStmt)) + futurePlus.GetAncOfType(typeof(ForEachStmt));

CxList callsToFuture = futurePlus.GetByAncs(iterations);

foreach (CxList call in callsToFuture)
{
	CxList specificFuture = future.FindAllReferences(call);
	result.Add(call.Concatenate(specificFuture, true));
}

result -= Find_Test_Code();