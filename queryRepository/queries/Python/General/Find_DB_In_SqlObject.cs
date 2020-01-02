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

List<string> methodsNames = new List<string> {
		"sync","syncUpdate","createTable","set","dropTable"};

result.Add(allMethods.FindByShortNames(methodsNames));

//update
result.Add(classAttributes.FindByAssignmentSide(CxList.AssignmentSide.Left));