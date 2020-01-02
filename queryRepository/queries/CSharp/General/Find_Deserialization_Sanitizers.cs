CxList methods = Find_Methods();
CxList references = Find_Unknown_References();
CxList fields = Find_FieldDecls();
CxList parameters = All.GetParameters(methods);

result.Add(methods.FindByMemberAccess("HashAlgorithm.ComputeHash")); 
result.Add(methods.FindByMemberAccess("KeyedHashAlgorithm.ComputeHash")); 
result.Add(methods.FindByMemberAccess("*CryptoServiceProvider.ComputeHash"));  

result.Add(methods.FindByMemberAccess("MD5.ComputeHash"));
result.Add(methods.FindByMemberAccess("MD5Managed.ComputeHash"));
result.Add(methods.FindByMemberAccess("MD5Cng.ComputeHash"));

result.Add(methods.FindByMemberAccess("RIPEMD160.ComputeHash")); 
result.Add(methods.FindByMemberAccess("RIPEMD160Managed.ComputeHash")); 
result.Add(methods.FindByMemberAccess("RIPEMD160Cng.ComputeHash")); 

result.Add(methods.FindByMemberAccess("SHA1.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA1Managed.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA1Cng.ComputeHash")); 

result.Add(methods.FindByMemberAccess("SHA256.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA256Managed.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA256Cng.ComputeHash")); 

result.Add(methods.FindByMemberAccess("SHA384.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA384Managed.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA384Cng.ComputeHash")); 

result.Add(methods.FindByMemberAccess("SHA512.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA512Managed.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA512Cng.ComputeHash"));

CxList jsonSerializerSettings = Find_ObjectCreations().FindByShortName("JsonSerializerSettings");
// TypeNameHandling should be used with caution when your application deserializes JSON from an external source. 
// Incoming types should be validated with a custom SerializationBinder when deserializing with a value other than None.
// TypeNameHandling is set to none by default which is safe.
CxList typeNameHandlers = references.FindByShortName("TypeNameHandling");
CxList safeTypeNameHandlers = typeNameHandlers.GetMembersOfTarget().FindByShortNames(new List<string> {"None"});

// A custom serialization binder sanitizes the deserialization for untrusted data.
CxList serializationBinder = fields.FindByShortName("SerializationBinder");
CxList safeJsonSerializerSettings = 
	(safeTypeNameHandlers + serializationBinder).GetAncOfType(typeof(ObjectCreateExpr)) * jsonSerializerSettings;

//If no TypeNameHandling is set, the value is None by default, i.e. safe.
CxList jsonSerializerSettingsWithTypeNamehandler = typeNameHandlers.GetAncOfType(typeof(ObjectCreateExpr)) * jsonSerializerSettings;
safeJsonSerializerSettings.Add(jsonSerializerSettings - jsonSerializerSettingsWithTypeNamehandler);
safeJsonSerializerSettings.Add(safeJsonSerializerSettings.GetFathers());
safeJsonSerializerSettings.Add(safeJsonSerializerSettings.GetAssignee());
safeJsonSerializerSettings.Add(references.FindAllReferences(safeJsonSerializerSettings));
CxList methodsWithSettingArgument = methods.FindByParameters(safeJsonSerializerSettings);
CxList arguments = All.GetParameters(methodsWithSettingArgument);

result.Add(methodsWithSettingArgument);
result.Add(arguments);
result.Add(All.GetByAncs(arguments));

// This next stub of code targets a specific case where settings are passed as an optional parameter to a 
// deserialization util method. All references of these method wich not specify the optional settings parameter
// are added to the sanitizers.
// Ex: 
// public static T DeserializeStuff(string data, JsonSerializerSettings settings = null) = JsonConvert.DeserializeObject<T>(data, settings);
CxList newtonsoftDeserializationMethods = methods.FindByMemberAccess("JsonConvert.DeserializeObject");
CxList settingsArg = All.GetParameters(newtonsoftDeserializationMethods, 1);
CxList paramSettings = Find_ParamDecl().FindDefinition(settingsArg);
CxList nonOptionalArgs = All.NewCxList();
foreach(CxList paramDecl in paramSettings){
	ParamDecl pd = paramDecl.TryGetCSharpGraph<ParamDecl>();
	if(pd.IsOptional){
		CxList prmts = paramDecl.GetFathers().FindByType(typeof(ParamDeclCollection));
		int paramPostion = prmts.TryGetCSharpGraph<ParamDeclCollection>().IndexOf(pd);
		CxList methodReferences = methods.FindAllReferences(prmts.GetFathers());
		CxList refsWithOptionalParam = methodReferences.FindByParameters(All.GetParameters(methodReferences, paramPostion));
		CxList refsWithoutOptionalParameter = methodReferences - refsWithOptionalParam;
		CxList otherArgs = All.GetParameters(refsWithoutOptionalParameter);
		result.Add(refsWithoutOptionalParameter);
		nonOptionalArgs.Add(otherArgs);
	}
}

result.Add(nonOptionalArgs);
result.Add(Find_Expressions().GetByAncs(nonOptionalArgs));