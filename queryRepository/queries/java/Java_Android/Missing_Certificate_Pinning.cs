CxList methods = Find_Methods();
CxList parameters = Find_Params();

CxList methodsUnknownsParams = Find_UnknownReference();
methodsUnknownsParams.Add(methods);
methodsUnknownsParams.Add(parameters);
methodsUnknownsParams.Add(Find_ParamDeclaration());

CxList certificate = All.FindByType("Certificate");
certificate.Add(All.FindByType("X509Certificate"));
CxList potentialSanitizerMethod = methods.FindByShortName("setSSLSocketFactory");
CxList sanitizerPath = certificate.DataInfluencingOn(potentialSanitizerMethod);
CxList sanitizerMethod = sanitizerPath.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

CxList openConnectionMethod = methods.FindByShortName("openConnection");
CxList pathToInsertCertificate = openConnectionMethod.DataInfluencingOn(sanitizerMethod);
CxList connectionsWithSertificate = pathToInsertCertificate.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
CxList absentCertificateConnection = openConnectionMethod - connectionsWithSertificate;

CxList httpsString = Find_Strings().FindByShortName("https://*");
result = httpsString.DataInfluencingOn(absentCertificateConnection);

// Support for the RequestQueue class
// There must be an implementation of a TrustManager that validates certificate chains
CxList inheritsFromTrustManager = Find_Class_Decl().InheritsFrom("X509TrustManager");
CxList newRequestQueue = methods.FindByMemberAccess("Volley.newRequestQueue");
CxList sndParamOfRequestQueue = methodsUnknownsParams.GetByAncs(parameters.GetParameters(newRequestQueue, 1));

// Classes that implement the interface X509TrustManager must have a method called "checkServerTrusted" whose 
// first parameter is an array (chain) of certificates
CxList checkServerTrusted = Find_MethodDeclaration().GetByAncs(inheritsFromTrustManager).FindByShortName("checkServerTrusted");
CxList certificateChain = methodsUnknownsParams.GetParameters(checkServerTrusted, 0);

CxList conditions = methodsUnknownsParams.GetByAncs(Find_Conditions());

CxList safeTrustManagers = certificateChain.InfluencingOn(conditions).GetAncOfType(typeof(ClassDecl));
CxList safeTrustManInstances = Find_Object_Create().FindByShortName(safeTrustManagers);
//Workaround because flow is missing when objectCreateExpression is inside an ArrayInitializer
safeTrustManInstances.Add(safeTrustManInstances.GetAncOfType(typeof(ArrayInitializer)).GetAssignee());

CxList safeRequestQueues = sndParamOfRequestQueue.InfluencedBy(safeTrustManInstances);
CxList unsafeRequestQueues = newRequestQueue - newRequestQueue.FindByParameters(safeRequestQueues);
result.Add(unsafeRequestQueues);