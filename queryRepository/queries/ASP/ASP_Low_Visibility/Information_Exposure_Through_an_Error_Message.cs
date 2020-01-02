CxList main_decl = (All.FindByType(typeof(MethodDecl))).FindByName("*.main").FindByFieldAttributes(Modifiers.Public | Modifiers.Static);

CxList classes_with_main=All.GetClass(main_decl);

CxList ctch = All.FindByType(typeof(Catch));
CxList class_of_ctch = All.GetClass(ctch);

CxList class_of_ctch_not_with_main = (All - classes_with_main).GetClass(ctch);

ctch = ctch.GetByAncs(class_of_ctch_not_with_main);
	
CxList class_not_with_main = All.FindByType(typeof(ClassDecl)) - classes_with_main;
class_not_with_main = All.GetByAncs(class_not_with_main);
	
CxList exc = All.FindAllReferences(ctch) - ctch +
	All.GetByAncs(class_not_with_main).FindByName("*server.getlasterror*") +
	All.GetByAncs(class_not_with_main).FindByName("*innerexception*");
	
CxList err = All.FindByShortName("err");

result = Find_Interactive_Outputs().DataInfluencedBy(exc + err);