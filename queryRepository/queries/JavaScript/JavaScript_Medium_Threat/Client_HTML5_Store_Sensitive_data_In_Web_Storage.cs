CxList storeData = Find_Storage_Inputs();
CxList parameter = All.GetParameters(storeData, 0);

CxList personalInfo = Find_Personal_Info();
result.Add(personalInfo * parameter);
result.Add(parameter.DataInfluencedBy(personalInfo));

CxList inputs = Find_Inputs();
CxList potential = Find_String_Short_Name(parameter, new List<string>{
		"hash*", "session*", "token*", "cookie*"}, false);
storeData = storeData.FindByParameters(potential);
CxList storeInfluencedByInput = storeData.DataInfluencedBy(inputs);

result.Add(storeInfluencedByInput);