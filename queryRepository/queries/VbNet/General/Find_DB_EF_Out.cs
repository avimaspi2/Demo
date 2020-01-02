// The purpose of the query is to found 
// All database access output in Entity Framework (EF)
// The block below deals with keeping info in DB
CxList methods = Find_Methods();

CxList ef = All.InheritsFrom("objectcontext");
ef.Add(All.FindByType("ObjectContext", false)); 
 
CxList efRef = All.FindAllReferences(ef);
CxList dbDecl = efRef.GetFathers().GetFathers().FindByType(typeof(Declarator));
CxList dbRef = All.FindAllReferences(dbDecl);


// The code below deals with follwoing pattern:
//
// SampleDBEntities db;
// db = new SampleDBEntities();

CxList createExpr = All.FindByType(typeof(ObjectCreateExpr));
CxList relevantCreateExpr = All.NewCxList();

foreach(CxList single in ef)
{
	try{
		CSharpGraph create = single.data.GetByIndex(0) as CSharpGraph;
		relevantCreateExpr.Add(createExpr.FindByType(create.FullName));
	}
	catch (Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}

// dbVar below is for : db = new SampleDBEntities();
CxList dbVar = All.GetByAncs(relevantCreateExpr.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);
dbRef.Add(All.FindAllReferences(dbVar));

result = dbRef.GetMembersOfTarget().FindByShortName("SaveChanges", false);
result.Add(ef.GetMembersOfTarget().FindByShortName("AddObject", false));