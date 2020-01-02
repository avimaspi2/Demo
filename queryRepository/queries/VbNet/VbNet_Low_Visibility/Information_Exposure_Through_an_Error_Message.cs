if(!All.isWebApplication)
{
	CxList ctch = All.FindByType(typeof(Catch));

	CxList exc = All.FindAllReferences(ctch) - ctch;
	exc.Add(All.FindByName("*Server.GetLastError*", false));

	result = Find_Interactive_Outputs().DataInfluencedBy(exc);
}	
else
{
	CxList main_decl = (All.FindByType(typeof(MethodDecl))).FindByName("*.Main", false).FindByFieldAttributes(Modifiers.Public | Modifiers.Static);
	
	CxList classes_with_main = All.GetClass(main_decl);
	
	CxList ctch = All.FindByType(typeof(Catch));
	CxList class_of_ctch_not_with_main = (All - classes_with_main).GetClass(ctch);
	
	ctch = ctch.GetByAncs(class_of_ctch_not_with_main);
	
	CxList class_not_with_main = All.FindByType(typeof(ClassDecl)) - classes_with_main;
	class_not_with_main = All.GetByAncs(class_not_with_main);
	
	CxList exc = All.FindAllReferences(ctch) - ctch;
	exc.Add(class_not_with_main.FindByName("*Server.GetLastError*", false));
	exc.Add(class_not_with_main.FindByName("*InnerException*", false));
	
	result = Find_Interactive_Outputs().DataInfluencedBy(exc);
}