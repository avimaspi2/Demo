//review
CxList decls = (Find_Builtin_Types().FindByType("int") + Find_Builtin_Types().FindByType("float") +  
	Find_Builtin_Types().FindByType("double") + Find_Builtin_Types().FindByType("long")  +  
	Find_Builtin_Types().FindByType("decimal")).FindByType(typeof(Declarator));

decls = All.GetByAncs(decls) - All.GetByAncs(Find_ConstantDecl());

CxList lits = 	Find_Integer_Literals() - All.FindByShortName("0") - 
	All.FindByShortName("1") - All.FindByShortName("-1");

decls = decls.DataInfluencedBy(lits);

foreach(CxList curdecl in decls)
{
	Declarator d = curdecl.data.GetByIndex(0) as Declarator;
	if ((d != null) && (d.InitExpression is IntegerLiteral))
	{
		result.Add(curdecl);
	}
}