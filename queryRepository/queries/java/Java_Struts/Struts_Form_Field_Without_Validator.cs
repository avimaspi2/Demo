// Test struts version
if(Find_Struts1_Presence().Count > 0){
	CxList validationXML = Find_Validation();
	CxList validationClasses = validationXML.FindByType(typeof(ClassDecl));
	CxList classDecl = Find_Class_Decl();
	CxList fieldDecl = Find_Field_Decl();
	foreach(CxList validationClass in validationClasses)
	{
		CxList validationObjects = validationXML.GetByAncs(validationClass);
		CxList properties = validationObjects.FindByName("FORM_VALIDATION.FORMSET.FORM.FIELD.PROPERTY");
	
		CxList forms = validationObjects.FindByName("FORM_VALIDATION.FORMSET.FORM.NAME");
	
		CxList formAssignments = forms.GetFathers();
		CxList formNames = validationObjects.FindByFathers(formAssignments).FindByType(typeof(StringLiteral));
	
		SortedList sortedForms = new SortedList();
		foreach(CxList curForm in formNames)
		{
			CSharpGraph fromGraph = curForm.TryGetCSharpGraph<CSharpGraph>();
			sortedForms.Add(fromGraph.LinePragma.Line, fromGraph.ShortName);
		}
	
		DictionaryEntry formPair = new DictionaryEntry();
		sortedForms.Add(10000000,"");
		foreach(DictionaryEntry nextFormPair in sortedForms)
		{
			if (formPair.Key != null)
			{                                              
				CxList formProperties = All.NewCxList();
				foreach(CxList curProperty in properties)
				{
					CSharpGraph propertyGraph = curProperty.TryGetCSharpGraph<CSharpGraph>();
					if ((propertyGraph.LinePragma.Line > (int)formPair.Key) &&
						(propertyGraph.LinePragma.Line <= (int)nextFormPair.Key))
					{
						formProperties.Add(propertyGraph.NodeId, propertyGraph);
					}
				}
				string className = formPair.Value.ToString().Replace("\"","");
				CxList classObject = classDecl.FindByName(className);
				CxList validatorForm = classObject.InheritsFrom("ValidatorForm");
				if (validatorForm.Count > 0)
				{
					CxList validatorFormFields = fieldDecl.GetByAncs(validatorForm);
					CxList propertyNames = validationObjects.DataInfluencingOn(formProperties);
					foreach (CxList formField in validatorFormFields)
					{
						CSharpGraph fieldGraph = formField.TryGetCSharpGraph<CSharpGraph>();
						string graphName = fieldGraph.ShortName.Replace("\"","");
						if (propertyNames.FindByName("*" + graphName + "*").Count == 0)
						{
							result.Add(fieldGraph.NodeId, fieldGraph);
						}
					}
				}
			}
	 		formPair = nextFormPair;
		}
	}
}