if(All.isWebApplication)
{
	result = All.FindByName("*.Main").FindByType(typeof(MethodDecl))
		.FindByFieldAttributes(Modifiers.Public | Modifiers.Static);
}