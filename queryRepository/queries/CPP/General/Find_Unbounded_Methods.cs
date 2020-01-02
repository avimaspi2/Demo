CxList methodInvokes = Find_Methods();
result = methodInvokes.FindByShortNames(
	new List<string>{ "catgets", "gets", "scanf", "fscanf", "vscanf", "vfscanf" }
);