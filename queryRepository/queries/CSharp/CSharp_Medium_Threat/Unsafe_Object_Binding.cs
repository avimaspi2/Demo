CxList controllersMethodDecls = Find_ASP_MVC_Controllers();

if(controllersMethodDecls.Count > 0){	
	CxList methods = Find_Methods();
	
	//Find controller methods with model save
	CxList dbUpdateMethods = methods.FindByShortNames(new List<string> {"SaveChanges", "SaveChangesAsync"}, true);
	CxList dbUpdateMethodDecl = dbUpdateMethods.GetAncOfType(typeof(MethodDecl)) * controllersMethodDecls;

	//Sanitized by Bind
	CxList bindMethod = All.FindByType(typeof(CustomAttribute)).FindByShortName("Bind");
	CxList bindInclude = All.FindByFathers(bindMethod).FindByShortNames(new List<string> {"Include", "Exclude"});
	dbUpdateMethodDecl -= bindInclude.GetAncOfType(typeof(MethodDecl));

	//Sanitized by TryUpdateModel
	CxList tryUpdateMethod = methods.FindByShortName("TryUpdateModel", true);
	CxList tryUpdateIfStmt = All.NewCxList();
	foreach(CxList tryUpdate in tryUpdateMethod){
		CxList allParams = All.GetParameters(tryUpdate);
		CxList tryUpdateParams = allParams.FindByType(typeof(Param));
		CxList arrayParams = allParams.FindByType(typeof(ArrayCreateExpr));
		allParams -= arrayParams;
		
		foreach(CxList currentParam in allParams){
			CxList unknownRefParamDef = All.FindDefinition(currentParam);
			CxList defType = All.FindByFathers(unknownRefParamDef.GetAncOfType(typeof(VariableDeclStmt))).FindByType("String");		
			if(defType.Count > 0 && defType.TryGetCSharpGraph<TypeRef>().IsRankSpecifierCollectionExists()){
				arrayParams.Add(currentParam);
			}
		}
		
		if(tryUpdateParams.Count > 1 && arrayParams.Count > 0){
			tryUpdateIfStmt.Add(tryUpdate.GetFathers().FindByType(typeof(IfStmt)));
		}
	}
	dbUpdateMethods -= dbUpdateMethods.GetByAncs(tryUpdateIfStmt);

	//Sanitized by UpdateModel
	CxList updateModelMethod = methods.FindByShortName("UpdateModel", true);
	CxList modelRepository = All.NewCxList();
	foreach(CxList updateModel in updateModelMethod){
		CxList allParams = All.GetParameters(updateModel);
		CxList updateModelParms = allParams.FindByType(typeof(Param));
		CxList arrayParams = allParams.FindByType(typeof(ArrayCreateExpr));
		allParams -= arrayParams;
		
		foreach(CxList currentParam in allParams){
			CxList unknownRefParamDef = All.FindDefinition(currentParam);
			CxList defType = All.FindByFathers(unknownRefParamDef.GetAncOfType(typeof(VariableDeclStmt))).FindByType("String");		
			if(defType.Count > 0 && defType.TryGetCSharpGraph<TypeRef>().IsRankSpecifierCollectionExists()){
				arrayParams.Add(currentParam);
			}
		}

		if(updateModelParms.Count > 1 && arrayParams.Count > 0){
			CxList updateModelModels = All.GetParameters(updateModel, 0);
			CxList modelDefinition = All.FindDefinition(updateModelModels);
			CxList modelDefAssigner = modelDefinition.GetAssigner(methods);
			modelRepository.Add(modelDefAssigner.GetTargetOfMembers());
		}		
	}
	dbUpdateMethods -= dbUpdateMethods.DataInfluencedBy(modelRepository);

	//Build a path between the controller method and the model save
	foreach(CxList mDecl in dbUpdateMethodDecl){
		CxList updateMethod = dbUpdateMethods.GetByAncs(mDecl);
		CxList methodDecl = dbUpdateMethods.GetAncOfType(typeof(MethodDecl)) * mDecl;
		if(updateMethod.Count > 0){
			result.Add(methodDecl.ConcatenateAllPaths(updateMethod, false));
		}
	}
}

result.Add(Find_Unsafe_Object_Binding_NetCore());