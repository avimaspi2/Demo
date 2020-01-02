CxList allDataInfluencedByConn = Find_DB_Conn_Django();

CxList methods = All.FindAllReferences((allDataInfluencedByConn)).GetMembersOfTarget();
CxList managerRaw = Find_Django_Model_Components();
CxList allSelects = All.FindByShortName("*SELECT*");

//Explicit methods
result.Add(methods.FindByShortName("fetch*"));
result.Add(methods.FindByShortName("execute*").FindByParameters(allSelects));

//Raw queries
result.Add(managerRaw.FindByParameters(allSelects));

List<string> methodsNames = new List<string> {
		"filter","select_related","get","all","order_by",
		"exclude","distinct","values","values_list","prefetch_related",
		"exclude","get_or_create","aggregate"
		};

result.Add(managerRaw.FindByShortNames(methodsNames));