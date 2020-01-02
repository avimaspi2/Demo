CxList target = Find_DB_Conn_SqlObject();
CxList classDecl = All.FindAllReferences(target);
CxList allMethods = classDecl.GetMembersOfTarget();

CxList vList = All.FindAllReferences(
	All.InfluencedBy(classDecl)).FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList varList = All.FindAllReferences(vList).FindByType(typeof(UnknownReference));
allMethods.Add(varList.GetMembersOfTarget());

CxList classAttributes = All.NewCxList();

//class atributes access
foreach(CxList v in varList){
	//CSharpGraph name = v.TryGetCSharpGraph<CSharpGraph>();
	classAttributes.Add(varList.GetMembersOfTarget().FindByType(typeof(MemberAccess)));
}

//Read
result.Add(classAttributes - classAttributes.FindByAssignmentSide(CxList.AssignmentSide.Left));

result.Add(allMethods.FindByShortName("get*"));
result.Add(allMethods.FindByShortName("select*"));