CxList methods = Find_Methods();

result = Find_DB_In_CoreData();
result.Add(Find_DB_Out_CoreData());
result.Add(Find_DB_In_UserDefaults());
result.Add(Find_DB_Out_UserDefaults());


CxList dbMethods = methods.FindByShortName("sqlite3_bind_*");
result.Add(All.GetParameters(dbMethods, 2));

result.Add(Find_General_Sanitize());

// Parametrised queries
CxList db = Find_DB();

List<string> allMethods = new List<string> {
		"find *",
		"first",
		"last",
		"all",
		"execute",
		"update",
		"scoped_by_*"
		};

CxList dbFind = db.FindByShortNames(allMethods);
	
CxList allParams = All.GetByAncs(All.GetParameters(dbFind));
CxList conditions = allParams.FindByShortName("conditions");
conditions = conditions.GetAncOfType(typeof(Param));
CxList arr = All.GetByAncs(conditions).GetAncOfType(typeof(ArrayInitializer));
CxList arrayThings = All.FindByFathers(arr);

foreach (CxList oneArr in arr)
{
	ArrayInitializer ai = oneArr.TryGetCSharpGraph<ArrayInitializer>();
	CxList leave = arrayThings.FindById(ai.InitialValues[0].NodeId);
	arrayThings -= leave;
}
result.Add(arrayThings); 

CxList assignString = Find_Strings().GetByAncs(db).FindByAssignmentSide(CxList.AssignmentSide.Left);

assignString -= assignString.FindByShortNames(new List<string>{"condition","cond"}, false);

result.Add(All.GetByAncs(assignString.GetFathers()));