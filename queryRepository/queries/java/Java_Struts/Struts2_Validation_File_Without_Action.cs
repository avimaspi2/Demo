/*
A Struts2 Validation file was discovered without a matching Struts2 Action.
For each ActionClass, Struts2 searches for a corresponding ActionClass-validation.xml for the necessary
validation constraints. In this case, a validation file in the form of ActionClass-validation.xml was found,
but ActionClass does not match an Action defined in the Struts2 configuration file.
*/

// Test struts version
if(Find_Struts2_Presence().Count > 0){
	CxList classDecl = Find_ClassDecl();
	CxList actionValidationClasses = classDecl.FindByFileName("*-Validation.xml");
	foreach (CxList actionValidationClass in actionValidationClasses)
	{
		try
		{
			CSharpGraph g = actionValidationClass.TryGetCSharpGraph<CSharpGraph>();
			string fileName = g.LinePragma.FileName;
			int lastMinus = fileName.LastIndexOf("-");
			if (lastMinus >= 0)
			{
				string className = fileName.Substring(0, lastMinus);
				className = className.Substring(fileName.LastIndexOf("\\") + 1);
				CxList interestingClass = classDecl.FindByShortName(className, false).FindByFileName("*.java");
				if (interestingClass.Count == 0)
				{
					result.Add(actionValidationClass);
				}
			}
		}
		catch (Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
		
	}
}