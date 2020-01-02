/*
A Struts2 validator definition refers to an action field that does not exist.
*/

// Test struts version
if(Find_Struts2_Presence().Count > 0){
	CxList classDecl = Find_ClassDecl();
	CxList validationXML = All.FindByFileName("*-Validation.xml");
	CxList validationClasses = validationXML.FindByType(typeof(ClassDecl));
	
	CxList allFieldDecls = Find_Field_Decl();
	// Run on every Validation file separately
	foreach(CxList validationClass in validationClasses)
	{
		try
		{
			CxList validationObjects = validationXML.GetByAncs(validationClass);
			CxList validators = validationObjects.FindByName("VALIDATORS.FIELD.NAME");
			CxList fieldAssignments = validators.GetFathers();
			CxList fieldNames = validationObjects.FindByFathers(fieldAssignments).FindByType(typeof(StringLiteral));;
			CSharpGraph g = validationClass.TryGetCSharpGraph<CSharpGraph>();
			string fileName = g.LinePragma.FileName;
			int lastMinus = fileName.LastIndexOf("-");
			if (lastMinus >= 0)
			{
				string className = fileName.Substring(0, lastMinus);
				className = className.Substring(fileName.LastIndexOf("\\") + 1);
				CxList relevantClass = classDecl.FindByShortName(className, false).FindByFileName("*.java");
				if (relevantClass.Count > 0)
				{
					CxList fieldDecls = allFieldDecls.GetByAncs(relevantClass);
					foreach (CxList field in fieldNames)
					{
						CSharpGraph g2 = field.TryGetCSharpGraph<CSharpGraph>();
						CxList testField = fieldDecls.FindByShortName(g2.ShortName.Trim('"'));
						if (testField.Count == 0)
						{
							result.Add(field.Concatenate(relevantClass));
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
	}
}