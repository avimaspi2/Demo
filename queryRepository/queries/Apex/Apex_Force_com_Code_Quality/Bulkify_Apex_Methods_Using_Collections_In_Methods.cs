CxList apex = Find_Apex_Files();
CxList apexObjects = apex * Find_sObjects();
 
// Find db parameters
CxList db = Find_DB_Output();
CxList bareDB = Find_DB_Output();

CxList meth = All.FindAllReferences(db.GetAncOfType(typeof(MethodDecl)));

int numMeth = 0;
for(int i = 0; i < 5 && meth.Count > numMeth; i++)
{
	numMeth = meth.Count;
	meth.Add(All.FindAllReferences(meth.GetAncOfType(typeof(MethodDecl))));
}

db.Add(meth);

CxList updateWhat = apexObjects.GetByAncs(apex.GetParameters(bareDB));
CxList updateParams = apexObjects.GetByAncs(apex.GetParameters(db));

// Find Method parameters
CxList methods = apex.GetMethod(meth);
CxList methodsParams = apexObjects.GetParameters(methods);
methodsParams -= methodsParams.FindByRegex(@"([lL]ist|[Ss]et)\s*<\w+\s*>") 
	+ methodsParams.FindByRegex(@"\[\s*\]") 
	+ methodsParams.FindByType("map");

result = methodsParams.FindAllReferences(updateParams).DataInfluencingOn(updateWhat);

// Add cases where we have flow problem
updateParams = apexObjects.GetMembersOfTarget().GetByAncs(apex.GetParameters(db));
CxList membersParams = updateParams * All.FindAllReferences(methodsParams).GetMembersOfTarget();
result.Add((methodsParams-result).FindDefinition(membersParams.GetTargetOfMembers()));

result -= result.DataInfluencedBy(result);
result -= Find_Test_Code();