if(All.isWebApplication)
{
	result = All.FindByName("*.main").FindByType(typeof(MethodDecl))
		.FindByFieldAttributes(Modifiers.Public | Modifiers.Static);
}

result.Add(Find_Strings().FindByName("*DEBUG*", false));
result -= All.FindByFileName("*.MF");