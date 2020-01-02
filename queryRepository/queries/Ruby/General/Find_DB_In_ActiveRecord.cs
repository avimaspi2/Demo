//pelase see http://rails-sqli.org/ as a reference

CxList arConnection = All.FindByName("*ActiveRecord.Base.connection");
CxList arDb = arConnection.GetMembersOfTarget();
result = All.GetParameters(arDb, 0);

CxList models = Find_Models();
if(models.Count > 0)
{
	CxList methods = Find_Methods();
	//old ActiveRecord inputs
	CxList find = methods.FindByShortName("find*");
	
	CxList finds = find.FindByShortNames(new List<string> {
			"find",
			"find_by*",
			"find_first",
			"find_last",
			"find_one",
			"find_some",
			"find_take",
			"find_with_associations",
			"find_with_ids",
			"find_each",
			"find_in_batches",
			});
	finds.Add(methods.FindByShortNames(new List<string> {
			"all",
			"first",
			"first!",
			"last",
			"last!"
			}));
	
	CxList whereQ = methods.FindByShortName("where");
	CxList queries = All.NewCxList();
	queries.Add(whereQ);
	queries.Add((methods.FindByShortName("not").GetTargetOfMembers() * whereQ).GetMembersOfTarget());
	queries.Add(methods.FindByShortNames(new List<string> {
			"delete_all",
			"destroy_all",
			"calculate",
			"having",
			"joins",
			"lock",
			"order",
			"pluck",
			"reorder",
			"exists?",
			"includes"
			}));

	CxList additional = methods.FindByShortName("select");
	CxList potentialResult = All.NewCxList();
	potentialResult.Add(queries);
	potentialResult.Add(finds);
	potentialResult.Add(additional);

	CxList modelCreateStmt = Find_TypeRef().FindByFathers(Find_ObjectCreations());

	//now we consider four options:
	/*
	option a:
	Model.queryMethod
	Instance.queryMethod

	option b:
	serial invocation:

	option c: 
	dynamic serial invocation

	option d:
	located in model class
	*/

	//option a:
	CxList classDecl = Find_ClassDecl();
	CxList foundedModels = All.NewCxList();
	foundedModels.Add(classDecl.InheritsFrom(models));
	foundedModels.Add(models);

	CxList modelInPlural = All.NewCxList();
	CxList target = potentialResult.GetTargetOfMembers();
	target.Add(modelCreateStmt);
	foreach(CxList mod in foundedModels)
	{
		string namePlusS = mod.GetName() + "s";		
		modelInPlural.Add(target.FindByShortName(namePlusS, false));
	}

	result.Add(potentialResult.GetTargetOfMembers().FindByShortName(foundedModels).GetMembersOfTarget());
	result.Add(potentialResult.GetTargetOfMembers().FindByType(foundedModels.GetName()).GetMembersOfTarget());
	result.Add(All.FindByFathers(modelCreateStmt.FindAllReferences(foundedModels)));
	result.Add(modelCreateStmt.FindAllReferences(foundedModels).GetFathers());		
	result.Add(modelInPlural.GetMembersOfTarget());
	result.Add(modelInPlural.GetFathers().FindByType(typeof(ObjectCreateExpr)));
	//option b:
	result.Add((potentialResult.GetTargetOfMembers() * potentialResult).GetMembersOfTarget());
	//option c:
	CxList dbOut = Find_DB_Out();
	CxList leftOfAssign = Find_UnknownReference().FindByAssignmentSide(CxList.AssignmentSide.Left);
	CxList left = leftOfAssign.FindByFathers(dbOut.GetFathers().FindByType(typeof(AssignExpr)));
	result.Add(target.FindAllReferences(left).GetMembersOfTarget());
	//option d:	
	potentialResult -= potentialResult.GetMembersWithTargets();
	result.Add(potentialResult.GetByAncs(foundedModels));
}