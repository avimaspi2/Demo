CxList controllersMethodDecls = Find_ASP_MVC_Controllers();
CxList methods = Find_Methods();
CxList customAttributes = All.FindByType(typeof(CustomAttribute));

//Find controller methods with model save
CxList dbUpdateMethods = methods.FindByShortNames(new List<string> {"SaveChanges", "SaveChangesAsync", "SubmitChanges"}, true);
CxList customDbUpdateMethods = methods.FindAllReferences(All.GetMethod(dbUpdateMethods));
dbUpdateMethods.Add(methods.FindByMemberAccess("UserManager", "CreateAsync", true));
//Find custom defined Save methods
dbUpdateMethods.Add(customDbUpdateMethods);
//Ensure that we only have controllers
CxList dbUpdateMethodDecl = dbUpdateMethods;
dbUpdateMethodDecl.Add(customDbUpdateMethods);
dbUpdateMethodDecl = dbUpdateMethodDecl.GetAncOfType(typeof(MethodDecl)) * controllersMethodDecls;
//We also make sure the method is an HttpPost
CxList httpGetAndPost = customAttributes.FindByShortNames(new List<string> { "HttpPost", "HttpGet" });
dbUpdateMethodDecl = All.GetMethod(httpGetAndPost) * dbUpdateMethodDecl;

//Controllers without Token Validation
CxList tokenValidation = customAttributes.FindByShortName("ValidateAntiForgeryToken");
CxList methodsDeclWithoutTokenValidation = dbUpdateMethodDecl - All.GetMethod(tokenValidation);

//BeginForm without Token Validation
CxList beginFormMethods = methods.FindByName("Html.BeginForm");
CxList endFormMethods = methods.FindByName("Html.EndForm");
CxList antiForgeryTokenMethods = methods.FindByName("Html.AntiForgeryToken");
//BeginForm with "using"
CxList beginFormUsingStmt = beginFormMethods.GetAncOfType(typeof(UsingStmt));
CxList usingStmtWithTokenValidation = antiForgeryTokenMethods.GetAncOfType(typeof(UsingStmt));
CxList formWithoutTokenValidation = beginFormUsingStmt - usingStmtWithTokenValidation;
formWithoutTokenValidation = beginFormMethods.GetByAncs(formWithoutTokenValidation);
//BeginForm and EndForm scope
CxList beginFormWithoutUsingStmt = beginFormMethods - beginFormMethods.GetByAncs(beginFormUsingStmt);
foreach(CxList form in beginFormWithoutUsingStmt){
	CxList containsAntiForgeryToken = antiForgeryTokenMethods.FindInScope(form, endFormMethods);
	if(containsAntiForgeryToken.Count == 0)
	{
		formWithoutTokenValidation.Add(form);
	}
}

//Add the vunerable Forms and Methods to results
foreach(CxList mDecl in methodsDeclWithoutTokenValidation){
	CxList updateMethod = dbUpdateMethods.GetByAncs(mDecl);
	CxList methodDecl = dbUpdateMethods.GetAncOfType(typeof(MethodDecl)) * mDecl;
	if(updateMethod.Count > 0){
		result.Add(methodDecl.ConcatenateAllPaths(updateMethod, false));
	}
}
result.Add(formWithoutTokenValidation);