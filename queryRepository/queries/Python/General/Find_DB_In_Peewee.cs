CxList target = Find_DB_Conn_Peewee();
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

//Write
result.Add(classAttributes.FindByAssignmentSide(CxList.AssignmentSide.Left));

List<string> methodsNames = new List<string> {
		"create","delete_instance","update","insert","insert_many",
		"get_or_create","drop_table","join"};

result.Add(allMethods.FindByShortNames(methodsNames));