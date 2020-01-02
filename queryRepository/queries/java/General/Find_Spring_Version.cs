/// Checks the Version of Spring Framework
/// Starts by checking the Maven configuration file
/// If maven does not contain the information, try the Beans configuration xml files
///
/// It returns an empty or single Node CxList with one of two types:
/// - StringLiteral: can be obtained the version with .GetName()
/// - Comment: an ( .data.GetByIndex(0) as CSharpGraph).FullName should be used

// Maven
CxList pomData = Find_Pom_Config();
if(pomData.Count != 0){
	CxList literals = pomData.FindByType(typeof(StringLiteral));
	CxList memberAccess = pomData.FindByType(typeof(MemberAccess));

	CxList springLiterals = literals.FindByShortName("*spring*", false);
	CxList assignExpr = springLiterals.GetFathers();
	CxList leftSide = memberAccess.FindByFathers(assignExpr).FindByAssignmentSide(CxList.AssignmentSide.Left);

	CxList pomDependency = pomData.FindByShortName("dependency", false);
	CxList pomDependencyIf = leftSide.GetAncOfType(typeof(IfStmt)).GetFathers().GetFathers();
	pomDependencyIf = pomDependency.FindByFathers(pomDependencyIf).GetFathers();

	CxList pomVersion = memberAccess.FindByShortName("version", false); 
	pomVersion = pomVersion.GetByAncs(pomDependencyIf);

	CxList springVersion = literals.FindByFathers(pomVersion.GetFathers());

	string maxVersion = "";
	foreach(CxList v in springVersion){
		string versionName = v.GetName();
		if(!versionName.StartsWith("$")){
			if(versionName.CompareTo(maxVersion) > 0){
				maxVersion = versionName;
				result = v.Clone();
			}
		} else {
			string propertyName = versionName.Trim(new char[]{'$','{','}'});
			String fullName = "*.properties." + propertyName.Replace("-", "_") + ".text";
			CxList realNameVersion = memberAccess.FindByName(fullName, false);
			CxList realVersion = literals.FindByFathers(realNameVersion.GetFathers());
			versionName = realVersion.GetName();
			if(versionName.CompareTo(maxVersion) > 0){
				maxVersion = versionName;
				result = realVersion;
			}
		}
	}
}

// Beans
if(result.Count == 0){
	string reg = @"(?<=springframework.org/schema/beans/spring-beans-)\d+(\.\d+)?(?=\.xsd)";
	CxList beanVersion = All.FindByRegexExt(reg, "*.xml", false, CxList.CxRegexOptions.None, RegexOptions.IgnoreCase);	
	result = beanVersion;
	
	string maxVersion = "";
	foreach (CxList bean in beanVersion){
		try{
			// required due to GetName ignore everything before the last dot and version contains dots
			String versionName = (bean.TryGetCSharpGraph<CSharpGraph>()).FullName;
			if(versionName.CompareTo(maxVersion) > 0){
				result = bean.Clone();
			}
		}catch(Exception){}
	}
}