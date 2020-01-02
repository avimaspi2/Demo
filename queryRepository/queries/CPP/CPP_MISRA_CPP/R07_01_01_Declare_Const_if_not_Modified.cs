/*
 MISRA CPP RULE 7-1-1
 ------------------------------
 This query searches for unmodified declarations that don't have the const keyword.
 
 The Example below shows code with vulnerability: 

      int foo(int a,		//Non-compliant
			int * b, 		//Non-compliant
			int * const c,	//Compliant
			int d) 			//Compliant
		{
			d = 8;
			*b = 8;
		};

*/

CxList decls = Find_All_Declarators() + All.FindByType(typeof(ParamDecl));
decls -= decls.FindByShortName("");
decls -= decls.GetByAncs(All.FindByType(typeof(StructDecl)));
//Remove params of prototype functions
CxList protoMethods = All.FindByType(typeof(MethodDecl));
protoMethods -= All.FindByFathers(protoMethods).FindByType(typeof(StatementCollection)).GetFathers();
decls -= decls.GetParameters(protoMethods);
//Find const declarations on variables - especially pointer.
CxList consts = All.FindByType(typeof(TypeRef)).FindByRegex(@"const\s+(\w+::)?\w+\s*&", false, false, false);
consts = consts.GetAncOfType(typeof(ParamDecl)) + decls.GetByAncs(consts.GetFathers().FindByType(typeof(VariableDeclStmt)));
consts.Add(decls.GetByAncs((All - All.FindByType(typeof(MethodDecl))).FindByFieldAttributes(Modifiers.Readonly)));
CxList pointers = decls.FindByRegex(@"\w+\s*\*",false,false,false);
decls -= consts + pointers;

CxList potPoint = All.FindByType(typeof(TypeRef)).FindByRegex(@"\w+\s*\*\s*const\W", false, false, false);
pointers -= Find_All_Declarators().GetByAncs(potPoint.GetFathers())
	+ potPoint.GetAncOfType(typeof(ParamDecl));

//Find unmodified 
CxList refs = All.FindAllReferences(decls + pointers) - decls - pointers;
refs = (decls + pointers).FindDefinition(refs.FindByAssignmentSide(CxList.AssignmentSide.Left));
result.Add(decls + pointers - refs);

//Add modification with << operator and usage of object's functions.
CxList doubleLeft = All.GetByBinaryOperator(BinaryOperator.ShiftLeft);
CxList sanitizeLeft = All.NewCxList();
foreach(CxList curr in doubleLeft) {
	CSharpGraph left = ((BinaryExpr) curr.data.GetByIndex(0)).Left;
	sanitizeLeft.Add(All.FindById(left.NodeId));
}
doubleLeft = sanitizeLeft.FindByType(typeof(UnknownReference));
CxList members = All.FindByMemberAccess(".*").GetTargetOfMembers();
members.Add(All.FindByFathers(members.FindByType(typeof(IndexerRef))));
doubleLeft.Add(members.FindByType(typeof(UnknownReference)));
result -= (pointers + decls).FindDefinition(doubleLeft);

//Remove typedefs
CxList typedefs = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF");
typedefs = typedefs.GetAncOfType(typeof(VariableDeclStmt))
	+ typedefs.GetAncOfType(typeof(FieldDecl));
typedefs = All.GetByAncs(typedefs).FindByType(typeof(Declarator));
//Remove gotos 
CxList gotos = All.FindByFathers(Find_All_Declarators());
gotos = gotos.FindByName("goto").GetFathers();
//Remove (void) parameters
result -= typedefs + gotos + All.FindByFathers(result).FindByShortName("void").GetFathers();;