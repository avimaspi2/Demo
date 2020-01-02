CxList allDataInfluencedByConn = Find_DB_Conn_Django();

//Django Modules
CxList managerRaw = Find_Django_Model_Components();

CxList managerObject = All.FindAllReferences(
	All.InfluencedBy(managerRaw)
	.FindByAssignmentSide(CxList.AssignmentSide.Left)
	);

CxList assginDecla = All.NewCxList();
assginDecla.Add(managerRaw.GetAncOfType(typeof(AssignExpr)));
assginDecla.Add(managerRaw.GetAncOfType(typeof(Declarator)));

managerObject = managerObject.GetByAncs(assginDecla);

CxList modelInstancesMembers = All.FindAllReferences(managerObject)
	.FindByType(typeof(UnknownReference)).GetMembersOfTarget()
	;

CxList methods = All.NewCxList();
//Get 5 nodes for the right of our position
for(int i = 0; i < 5; i++){
	if(methods.Count == 0){
		methods.Add(All.FindAllReferences((allDataInfluencedByConn)).GetMembersOfTarget());	
	}
	else{
		methods.Add(methods.GetMembersOfTarget());	
	}
}

result.Add(methods.FindByShortName("execute*"));

//Raw queries
List<string> methodsNames = new List<string> {
		"create","get_or_create","update","delete","bulk_insert","map_reduce",
		"group","raw"};

result.Add(managerRaw.FindByShortNames(methodsNames));

result.Add(modelInstancesMembers.FindByShortName("save"));