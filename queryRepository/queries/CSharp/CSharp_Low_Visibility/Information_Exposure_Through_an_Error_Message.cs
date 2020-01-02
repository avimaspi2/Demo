CxList outputs = Find_Outputs() - Find_Console_Outputs();
CxList ctch = All.FindByType(typeof(Catch));
CxList methodDecl = All.FindByType(typeof(MethodDecl));
CxList unknownRef = Find_Unknown_References();
CxList classDecl = All.FindByType(typeof(ClassDecl));
CxList exc = All.FindByType("*Exception").FindByType(typeof(Declarator));
exc = (All - exc).FindAllReferences(exc);

if(!All.isWebApplication)
{
	exc.Add(All.FindByName("*Server.GetLastError*"));
}              
else
{
	CxList main_decl = methodDecl.FindByName("*.Main").FindByFieldAttributes(Modifiers.Public | Modifiers.Static);
                
	CxList classes_with_main = All.GetClass(main_decl);
                
	CxList class_of_ctch_not_with_main = (All - classes_with_main).GetClass(ctch);
                
	ctch = ctch.GetByAncs(class_of_ctch_not_with_main);
                
	CxList class_not_with_main = classDecl - classes_with_main;
	class_not_with_main = All.GetByAncs(class_not_with_main);
	
	exc.Add(class_not_with_main.FindByName("*Server.GetLastError*"));
	exc.Add(class_not_with_main.FindByName("*InnerException*"));
}

CxList methodsToFilter = unknownRef.FindByTypes(new String[]{"IApplicationBuilder","IHostingEnvironment", "IWebHostEnvironment"}).GetMembersOfTarget();
CxList isDelopment = methodsToFilter.FindByShortName("IsDevelopment");
CxList elementIf = All.GetByAncs(isDelopment.GetAncOfType(typeof(IfStmt)));
CxList methodUseDeveloperExceptionPage = methodsToFilter.FindByShortName("UseDeveloperExceptionPage");
CxList sanitizeHandlerException =  methodUseDeveloperExceptionPage * elementIf;
result = outputs.DataInfluencedBy(exc).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

result.Add(methodUseDeveloperExceptionPage - sanitizeHandlerException);