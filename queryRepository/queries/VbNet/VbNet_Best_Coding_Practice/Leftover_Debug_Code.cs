if(All.isWebApplication)
{
	result = All.FindByName("*.Main", false).FindByType(typeof(MethodDecl))
		.FindByFieldAttributes(Modifiers.Public | Modifiers.Static);
}