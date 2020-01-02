CxList pomData = Find_Pom_Config();
if(pomData.Count > 0 && Find_Spring_Imports().Count > 0)
{
	CxList unknownReferences = pomData.FindByType(typeof(UnknownReference));
	CxList memberAccesses = pomData.FindByType(typeof(MemberAccess));
	CxList dependencies = unknownReferences.FindByShortName("DEPENDENCY");
	CxList artifactId = unknownReferences.FindByShortName("ARTIFACTID");
	CxList version = unknownReferences.FindByShortName("VERSION");
	
	CxList artifactIdMemberAccess = memberAccesses.FindByMemberAccess("ARTIFACTID.TEXT");
	CxList versionMemberAccess = memberAccesses.FindByMemberAccess("VERSION.TEXT");
	
	CxList stringLiterals = pomData.FindByType(typeof(StringLiteral));
	CxList springOxm = stringLiterals.FindByShortName("spring-oxm");
	CxList springMvc = stringLiterals.FindByShortName("spring-webmvc");

	foreach(CxList dependency in dependencies)
	{
		CxList dependencyIfStmt = dependency.GetAncOfType(typeof(IfStmt));
		CxList artifactIdIfStmt = artifactId.GetByAncs(dependencyIfStmt).GetAncOfType(typeof(IfStmt));
		CxList artifactIdValue = artifactIdMemberAccess.GetByAncs(artifactIdIfStmt).GetAssigner(stringLiterals);
		CxList versionIfStatement = version.GetByAncs(dependencyIfStmt).GetAncOfType(typeof(IfStmt));
		CxList versionValue = versionMemberAccess.GetByAncs(versionIfStatement).GetAssigner(stringLiterals);
		string literalVersion = versionValue.GetName();
		
		if(literalVersion.StartsWith("$"))
		{
			string propertyName = literalVersion.Trim(new char[]{'$','{','}'});
			string fullName = "*.properties." + propertyName.Replace("-", "_") + ".text";
			CxList realNameVersion = memberAccesses.FindByName(fullName, false);
			versionValue = stringLiterals.FindByFathers(realNameVersion.GetFathers());
			literalVersion = versionValue.GetName();
		}
		bool isSpringOxm = (artifactIdValue * springOxm).Count > 0;
		bool isSpringMvc = (artifactIdValue * springMvc).Count > 0;
		if(isSpringOxm)
		{
			result.Add(Find_XXE_Spring_Oxm(literalVersion));
		}
		else if(isSpringMvc)
		{
			result.Add(Find_XXE_Spring_Mvc(literalVersion));
		}
	}
}