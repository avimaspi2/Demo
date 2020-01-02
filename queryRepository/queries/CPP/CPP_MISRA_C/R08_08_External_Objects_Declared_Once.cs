/*
MISRA C RULE 8-8
------------------------------
This query searches for external objects declared more than once

	The Example below shows code with vulnerability:

extern int32_t ex_head_head_decl;	// in file 1.h

extern int32_t ex_head_head_decl;	// in file 2.h

*/

CxList varsAndMethods = Find_All_Declarators() + All.FindByType(typeof(MethodDecl));
CxList externDecls = varsAndMethods.FindByRegex("extern");

foreach(CxList cur in externDecls){
	// Check if there are declerations in more than one file
	if (externDecls.FindByName(cur).Count != 
	externDecls.FindByShortName(cur).Count){
		result.Add(cur);
	}
}