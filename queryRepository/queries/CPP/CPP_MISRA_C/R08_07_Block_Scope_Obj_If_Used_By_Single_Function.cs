/*
MISRA C RULE 8-7
------------------------------
This query searches for variables with file scope only used by a single function

	The Example below shows code with vulnerability:

int a;	// this should not be global 
int b; 
void foo(){
	b = a + 5;
}
void doo(){
	b = 6;
}

*/

CxList vars = Find_All_Declarators();
vars -= All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();
CxList classes = All.FindByType(typeof(ClassDecl));
classes -= classes.FindByShortName("checkmarx_default_classname*");
CxList atBlockScope = vars.GetByAncs(All.FindByType(typeof(StatementCollection)) 
	+ classes + All.FindByType(typeof(StructDecl)));
CxList globalVars = vars - atBlockScope;
CxList globalVarUses = All.FindAllReferences(globalVars).FindByType(typeof(UnknownReference));

foreach(CxList cur in globalVars){
	CxList curVarUses = globalVarUses.FindAllReferences(cur);
	
	// Check there are uses in only one (or less) function
	if (curVarUses.GetAncOfType(typeof(MethodDecl)).Count <= 1){
		result.Add(cur);
	}
}