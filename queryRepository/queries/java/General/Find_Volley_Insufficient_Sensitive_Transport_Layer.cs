//add support of Volley Request
CxList getParams = Find_MethodDeclaration().FindByShortName("getParams");
CxList pi = Find_Personal_Info();
CxList ur = Find_UnknownReference();
CxList rs = Find_ReturnStmt();
rs = ur.GetByAncs(rs.GetByAncs(getParams)).DataInfluencedBy(pi);

CxList hurlStack = All.FindByType("HurlStack");
CxList hurlOCE = hurlStack.FindByType(typeof(ObjectCreateExpr));
CxList anonyClass = All.FindByShortName("anonymous*");
CxList cdecl = Find_Class_Decl();
CxList ssl = All.FindByShortName("setssl*", false);
CxList sanitizedHStack = All.NewCxList();
foreach(CxList hStack in hurlOCE)
{
	CxList anonyClassRef = anonyClass.GetByAncs(hStack);
	CxList foundRelatedClass = cdecl.FindDefinition(anonyClassRef);
	
	if(ssl.GetByAncs(foundRelatedClass).Count > 0)
	{
		sanitizedHStack.Add(hStack);
	}
}


CxList assignedToHstack = sanitizedHStack.GetAssignee();
CxList sanitizer = hurlStack.FindAllReferences(assignedToHstack);

CxList requestQueue = All.FindByMemberAccess("RequestQueue.add");
requestQueue.Add(All.FindByShortName("addToRequestQueue"));
CxList sanitized = requestQueue.DataInfluencedBy(sanitizer);
requestQueue -= sanitized;
CxList methods = Find_Methods();
 requestQueue = methods * requestQueue;

foreach(CxList returnStmt in rs.GetCxListByPath())
{
	CSharpGraph rGraph = returnStmt.GetFirstGraph();
	if(rGraph != null && rGraph.LinePragma != null && rGraph.LinePragma.FileName != null)
	{
		string fileId = rGraph.LinePragma.FileName;
		CxList addTOQueue = requestQueue.FindByFileName(fileId);
		result.Add(returnStmt.ConcatenateAllPaths(addTOQueue, true));
	}
	

}