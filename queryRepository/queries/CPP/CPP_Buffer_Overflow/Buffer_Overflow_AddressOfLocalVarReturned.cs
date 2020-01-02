// BufferOverflow_AddressOfLocalVarReturned Query
// //////////////////////////////
// The purpose of the query is to find methods / functions that returns the address of the 
// local variable
// 
// The Example below shows code with vulnerability
// 
//     	  int * v7(void){
//			int a;
//			int b;
//			int c;
//		
//			b = *(&a + &c); 
//			return &b; 
//		  }
// based on this example comments will be referenced
//
// Additionally, 
//   (1) the statement 
// 		b = foo(&a) 
// Should recognize that b is not infliuenced by a
//   (2) return &b->x or &b.x 
// will not be recognized as vulnerability

CxList unknownRef = Find_Unknown_References();
CxList paramDecl = Find_ParamDecl();
CxList fieldDecl = Find_FieldDecls();
CxList objectCreateExpr = Find_ObjectCreations();
CxList returnStmt = Find_ReturnStmt();
CxList arrays = Find_Array_Declaration();
CxList arrayCreate = arrays.FindByType(typeof(ArrayCreateExpr));
CxList arrayDeclare =  arrays.FindByType(typeof(VariableDeclStmt));
CxList declarator = Find_Declarators();
CxList unary = Find_Unarys();
CxList pointers = Find_Pointer_Decl();
CxList alloc = Find_Memory_Allocation();
CxList address = unary.FindByShortName("Address");
CxList addressVars = unknownRef.GetByAncs(address);

//remove all non local variables
CxList unkrUnderMethods = unknownRef.GetAncOfType(typeof(MethodDecl));
CxList localVariables = declarator.GetByAncs(unkrUnderMethods);
CxList notLocalDeclarators = declarator - localVariables;

unknownRef -= unknownRef.FindAllReferences((paramDecl + fieldDecl + notLocalDeclarators).FindDefinition(unknownRef));

//remove all parameters
unknownRef -= unknownRef.GetByAncs(unknownRef.GetAncOfType(typeof(MethodInvokeExpr)));

CxList unknownInReturn = unknownRef.GetByAncs(returnStmt);

//remove all heap variables
unknownInReturn -= unknownInReturn.DataInfluencedBy(alloc + objectCreateExpr);


//only gets address vars that are in the same scope as declarator
CxList addressVarsReturned = All.NewCxList();

foreach(CxList addressVar in addressVars)
{
	CxList definition = All.FindDefinition(addressVar);
	CxList definitionMethods = All.GetMethod(definition - definition.FindByType(typeof(ParamDecl)));
	
	if( ( All.GetMethod(addressVar) * definitionMethods ).Count > 0)
	{
		addressVarsReturned.Add(addressVar);
	}
}

CxList addressToPointers = addressVarsReturned.GetFathers().GetAssignee();

CxList addressesReturned = unknownInReturn.FindAllReferences(addressToPointers);

result.Add(addressesReturned);



//handle the  : return &x statement
result.Add(unknownInReturn.FindByFathers(unknownInReturn.GetFathers() * address));

//remove all cases such as &b.x 
result -= result.GetMembersOfTarget().GetTargetOfMembers();

CxList definitionOfVariable = declarator.FindDefinition(unknownInReturn);
//handle local variables that are declared as pointers 
//pointers are not local variables
//result.Add(unknownInReturn.FindAllReferences(definitionOfVariable * pointers));

//handle local arrays definitions that are not defined on the heap
CxList arrayDef = arrayCreate.FindByFathers(definitionOfVariable);
CxList arrayFathers = arrayDef.GetFathers();
arrayFathers.Add(declarator.GetByAncs(arrayDeclare));
result.Add(unknownInReturn.FindAllReferences(arrayFathers));

//remove all results that are influenced by parameter declarations
CxList parameters = paramDecl.GetByAncs(result.GetAncOfType(typeof(MethodDecl)));
CxList resultInsideMethodWithParam = result.GetByAncs(parameters.GetAncOfType(typeof(MethodDecl)));
result -= resultInsideMethodWithParam.DataInfluencedBy(parameters);

//remove all the static returned variables
CxList allStatics = Find_VariableDeclStmt().FindByFieldAttributes(Modifiers.Static); 
allStatics = All.FindAllReferences(All.GetByAncs(allStatics).FindByType(typeof (Declarator))); 
result -= allStatics;