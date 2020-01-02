/* Auxiliary information */
CxList methodDecls = Find_MethodDecls();
CxList unknownRefs = Find_UnknownReference();
CxList paramDecls = Find_ParamDecl();
CxList stringLiterals = Find_String_Literal();
CxList lambdaExpr = Find_LambdaExpr();

/* Capture API-related personal information */
CxList captureAPIResults = All.NewCxList();
CxList captureAPI = PhoneGap_Find_Capture_API();

// The parameter of the callback provided to .capture* methods is personal information
CxList captureAPICBParam = lambdaExpr.GetParameters(captureAPI, 0);
CxList captureAPICBMethodDecl = methodDecls.FindDefinition(captureAPICBParam);
CxList captureAPICBMediaFiles = paramDecls.GetParameters(captureAPICBMethodDecl);
captureAPIResults.Add(captureAPICBMediaFiles);


/* Contacts API-related personal information */
CxList contactsAPIResults = All.NewCxList();
CxList contactsAPI = PhoneGap_Find_Contacts_API();
CxList contactsAPICreate = contactsAPI.FindByShortName("create");
CxList contactsAPIClone = contactsAPI.FindByShortName("clone");
CxList contactsAPISave = contactsAPI.FindByShortName("save");
CxList contactsAPIRemove = contactsAPI.FindByShortName("remove");
CxList contactsAPIFind = contactsAPI.FindByShortName("find");

// The return values of .create and .clone methods are personal information
contactsAPIResults.Add(contactsAPICreate);
contactsAPIResults.Add(contactsAPIClone);

// If .clone, .save, .remove or .find methods are provided with a callback argument,
// that callback's parameter is personal information
CxList contactsAPICBParam = All.NewCxList();
CxList contactsManipulation = All.NewCxList();
contactsManipulation.Add(contactsAPIClone);
contactsManipulation.Add(contactsAPISave);
contactsManipulation.Add(contactsAPIRemove);
contactsAPICBParam.Add(lambdaExpr.GetParameters(contactsManipulation, 0));
contactsAPICBParam.Add(lambdaExpr.GetParameters(contactsAPIFind, 1));
CxList contactsAPICBMethodDecl = methodDecls.FindDefinition(contactsAPICBParam);
CxList contactsAPICBContacts = paramDecls.GetParameters(contactsAPICBMethodDecl);
contactsAPIResults.Add(contactsAPICBContacts);


/* Camera API-related personal information */
CxList cameraAPIResults = All.NewCxList();
CxList cameraAPI = PhoneGap_Find_Camera_API();
	
// The parameter of the callback provided to .getPicture method is personal information
CxList cameraAPIGetPicture = cameraAPI.FindByShortName("getPicture");
CxList cameraAPICBParam = lambdaExpr.GetParameters(cameraAPIGetPicture, 0);
CxList cameraAPICBMethodDecl = methodDecls.FindDefinition(cameraAPICBParam);
CxList cameraAPICBImageData = paramDecls.GetParameters(cameraAPICBMethodDecl);
cameraAPIResults.Add(cameraAPICBImageData);


/* Media API-related personal information */
CxList mediaAPIResults = All.NewCxList();
CxList mediaAPI = PhoneGap_Find_Media_API();
	
// The 1st argument of "new Media()" invocations is personal information
CxList mediaAPIMedia = mediaAPI.FindByShortName("Media");

CxList stringLiteralsUnknowRef = All.NewCxList();
stringLiteralsUnknowRef.Add(stringLiterals);
stringLiteralsUnknowRef.Add(unknownRefs);

CxList mediaAPIFile = stringLiteralsUnknowRef.GetParameters(mediaAPIMedia, 0);
mediaAPIResults.Add(All.FindAllReferences(mediaAPIFile));


/* Geolocation API-related personal information */
CxList geolocationAPIResults = PhoneGap_Find_Geolocation();

/* Network Information API-related personal information */
CxList networkInformationAPIResults = Cordova_Find_Network_Information_API();

/* Device API-related personal information */
CxList deviceAPIResults = Cordova_Find_Device_API();

/* Aggregate all the intermediate results */
result.Add(captureAPIResults);
result.Add(contactsAPIResults);
result.Add(cameraAPIResults);
result.Add(mediaAPIResults);
result.Add(geolocationAPIResults);
result.Add(networkInformationAPIResults);
result.Add(deviceAPIResults);