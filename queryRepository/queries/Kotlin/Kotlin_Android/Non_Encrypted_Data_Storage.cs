// Get sanitizers
CxList sanitizers = Find_Encrypt();

// Get outputs
CxList methods = Find_Methods();
CxList androidStorage = methods.FindByMemberAccess("FileOutputStream.write");
androidStorage.Add(methods.FindByMemberAccess("OutputStreamWriter.write"));
androidStorage.Add(Find_Write());

CxList preferences = methods.FindByMemberAccess("SharedPreferences.edit").GetAssignee();
CxList preferencesMethods = All.FindAllReferences(preferences).GetMembersOfTarget()
	.FindByShortNames(new List<string>{"putString", "putStringSet"});

preferencesMethods.Add(All.FindByMemberAccess("Editor.putString"));
preferencesMethods.Add(All.FindByMemberAccess("Editor.putStringSet"));

CxList outputs = All.NewCxList();
outputs.Add(androidStorage);
outputs.Add(preferencesMethods);

CxList outputParameters = All.GetParameters(outputs);
CxList methodParameters = outputParameters * methods;
CxList preferencesMethodsParams = All.GetParameters(preferencesMethods);
methodParameters.Add(preferencesMethodsParams);

// Get inputs
CxList inputs = Find_Declarators().GetAssigner();
inputs = inputs.InfluencingOn(outputParameters);
inputs.Add(All.FindDefinition(methodParameters));
inputs.Add(preferencesMethodsParams.FindByType(typeof(StringLiteral)));

sanitizers.Add(All.FindAllReferences(sanitizers));
CxList paramsSharedPreferences = All.GetParameters(preferencesMethods, 0);
sanitizers.Add(paramsSharedPreferences);

// Get values passed to outputs and not sanitized
result = inputs.InfluencingOnAndNotSanitized(outputs, sanitizers)
	.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

// Find databases saved in the external storage
CxList externalStorageDir = methods.FindByMemberAccess("Environment.getExternalStoragePublicDirectory");
externalStorageDir.Add(methods.FindByMemberAccess("Environment.getExternalStorageDirectory"));

CxList sqlDBCreation = methods.FindByMemberAccess("SQLiteDatabase.openDatabase");
sqlDBCreation.Add(methods.FindByMemberAccess("SQLiteDatabase.openOrCreateDatabase"));

result.Add(externalStorageDir.InfluencingOn(sqlDBCreation));