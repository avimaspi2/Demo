CxList objects = Find_ObjectCreation();
CxList mongoCodeObjects = objects.FindByShortName("MongoCode");

CxList mongoCodeSanitized = All.NewCxList();

foreach(CxList mongoCode in mongoCodeObjects){
	//if MongoCode object contains 2 parameters is considered sanitized
	CxList mongoCodeParams = All.GetParameters(mongoCode).FindByType(typeof(Param));
	if(mongoCodeParams.Count == 2){
		mongoCodeSanitized.Add(mongoCode);
	}
}

result = mongoCodeSanitized;