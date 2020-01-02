/*
This query implements CWE 326: "Inadequate Encryption Strength"

Description Summary:
"The software stores or transmits sensitive data using an encryption scheme that is 
 theoretically sound, but is not strong enough for the level of protection required".
*/

CxList passwords = Find_Passwords_Unsafe();
CxList personal_info = Find_Personal_Info();
CxList sensitiveData = passwords + personal_info;

// Object of types with weak cryptographic objects
// Similar to the Java query Reversible_One_Way_Hash();
CxList WeakEncrypt = Find_Weak_Encryption();

// Find all references of the "tainted" object variable (i.e. md1/md2 throughout the code)
CxList WeakEncrypterObjects = All.FindAllReferences(WeakEncrypt);

// For objects, add calls to methods of these objects
WeakEncrypterObjects.Add(WeakEncrypterObjects.GetMembersOfTarget());


result = sensitiveData.DataInfluencingOn(WeakEncrypterObjects.FindByType(typeof(MethodInvokeExpr))).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

//General initializers
CxList ur = Find_Unknown_References();
CxList methods = Find_Methods();


//Find all EVP update and EVP init methods
CxList update = methods.FindByShortNames(new List<string> {"EVP_EncryptUpdate","EVP_CipherUpdate"});
CxList EVP_EncryptInit = methods.FindByShortNames(new List<string> {"EVP_EncryptInit*","EVP_CipherInit*"});
//FindFirst and second parameter of EVP init method
CxList secondParamOfEVPInit = All.GetParameters(EVP_EncryptInit, 1);
CxList initFirstParameter = All.GetParameters(EVP_EncryptInit, 0);

//Second parameter of EVP init is weak encryption method
CxList methodEncrypt = secondParamOfEVPInit * methods * WeakEncrypterObjects;
//Second parameter of EVP init is an unknownReference
CxList urEncrypt = secondParamOfEVPInit.FindByType(typeof(UnknownReference));
//First Parameter of EVP update
CxList updateParamAtZero = All.GetParameters(update, 0);


//all first parameters of init that are influencing on first parameter of update (head node is init first parameter)
CxList flowFITU = initFirstParameter.DataInfluencingOn(updateParamAtZero);
//extract the relevant unknown reference from the init first parameter
CxList relevantURInInit = ur.GetByAncs(flowFITU);
//extract the relevant unknown reference from the update first parameter
CxList releveantURInUpdate = ur.GetByAncs(updateParamAtZero);

//get flows from external encrypt to the unknown reference :
/*
	e=EVP_des_ecb();
	EVP_EncryptInit(&ctx, e, NULL, NULL);
   **** a flow from EVP_des_ecb to e in second line will be calculated *****
*/

CxList outerEncryptors = WeakEncrypterObjects.DataInfluencingOn(urEncrypt);


//Get all updates that are influenced by sensitive data- keep iterating over relevant flow
CxList updateInfluencedBySensitive = update.DataInfluencedBy(sensitiveData);
updateInfluencedBySensitive = updateInfluencedBySensitive.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
foreach(CxList UIBS in updateInfluencedBySensitive.GetCxListByPath())
{	
	//for each update that has such flow, get it's first parameter
	CxList updateFirstParam = updateParamAtZero.GetParameters(UIBS, 0);	
	//find relevant references of this first parameter in init first parameters that are influencing on it.
	CxList relevantInitReferenceOfUpdateFirstParam = relevantURInInit.FindAllReferences(releveantURInUpdate.GetByAncs(updateFirstParam));
	//find all init methods from the parameter
	CxList initMethod = EVP_EncryptInit.FindByParameters(relevantInitReferenceOfUpdateFirstParam);
	//get second init parameter
	CxList encryption = secondParamOfEVPInit.GetParameters(initMethod);
	//either the parameter is a method invocation
	CxList imAMethod = methodEncrypt * encryption;
	//or it's an unknown reference
	CxList imUR = urEncrypt * encryption;
	//if invocation add it to sinks
	CxList sink = imAMethod;
	//if unknown reference add the encryptor influencing on the unknown reference to sinks
	sink.Add(outerEncryptors.DataInfluencingOn(imUR).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));
	//concatenate flows from sensitive data to update, to my sink
	foreach(CxList s in sink)
	{
		result.Add(UIBS.ConcatenateAllPaths(s));
	}
}