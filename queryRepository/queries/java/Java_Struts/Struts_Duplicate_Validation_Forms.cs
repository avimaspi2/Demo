// Test struts version
if(Find_Struts1_Presence().Count > 0){
	CxList validationXML = Find_Validation();
	CxList validationClasses = validationXML.FindByType(typeof(ClassDecl));
	
	foreach(CxList validationClass in validationClasses)
	{
		CxList validationObjects = validationXML.GetByAncs(validationClass);
		CxList forms = validationObjects.FindByName("FORM_VALIDATION.FORMSET.FORM.NAME");
		CxList formAssignments = forms.GetFathers();
		CxList formNames = validationObjects.FindByFathers(formAssignments).FindByType(typeof(StringLiteral));
	
		foreach(CxList curForm1 in formNames)
		{
			CSharpGraph g1 = curForm1.TryGetCSharpGraph<CSharpGraph>();
			foreach(CxList curForm2 in formNames)
			{
				CSharpGraph g2 = curForm2.TryGetCSharpGraph<CSharpGraph>();
				if(g1.NodeId != g2.NodeId && g1.FullName == g2.FullName)
				{
					result.Add(g1.NodeId, g1);
				}
			}
		}
	}
}