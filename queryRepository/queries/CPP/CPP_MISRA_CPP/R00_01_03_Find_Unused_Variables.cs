/*
 MISRA CPP RULE 0-1-3
 ------------------------------
 The purpose of the query to find variables that are defined or defined and initilized but never used.

 The Example below shows code with vulnerability: 

	int main() 
	{
		int i = 3; 		//Non-compliant
	}

*/

// Find All declarators: it may be a local variables, global variables or structurs
CxList varsDecl = Find_All_Declarators();

// The approach :All.GetByAncs(All.FindByType(typeof(FieldDecl))) tooks al childs of FiledDeck that is global variables
// and remove it from the Decl list
// 
varsDecl = varsDecl - All.GetByAncs(All.FindByType(typeof(FieldDecl)));

CxList allVariablesUsining = All.FindAllReferences(varsDecl).FindByType(typeof(UnknownReference));

result = varsDecl - varsDecl.FindDefinition(allVariablesUsining);

//Remove typedefs
CxList typedefs = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF");
typedefs = typedefs.GetAncOfType(typeof(VariableDeclStmt))
	+ typedefs.GetAncOfType(typeof(FieldDecl));
typedefs = All.GetByAncs(typedefs).FindByType(typeof(Declarator));

//Remove gotos 
CxList gotos = All.FindByFathers(Find_All_Declarators());
gotos = gotos.FindByName("goto").GetFathers();

result -= typedefs + gotos;