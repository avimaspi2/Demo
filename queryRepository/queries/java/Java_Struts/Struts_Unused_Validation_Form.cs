// Test struts version
if(Find_Struts1_Presence().Count > 0){
	// General declarations
	CxList classDecl = Find_Class_Decl();
	CxList fieldDecl = Find_Field_Decl();
	
	// All objects in validation files
	CxList validationXML = Find_Validation();
	// The main class of every validation class
	CxList validationClasses = validationXML.FindByType(typeof(ClassDecl));
	
	// For every validation file
	foreach(CxList validationClass in validationClasses)
	{
		try
		{
			// Objects in this file
			CxList validationObjects = validationXML.GetByAncs(validationClass);
			// field property tag
			CxList properties = validationObjects.FindByName("FORM_VALIDATION.FORMSET.FORM.FIELD.PROPERTY");
			// form name tag
			CxList forms = validationObjects.FindByName("FORM_VALIDATION.FORMSET.FORM.NAME");
			// Find the form names
			CxList formAssignments = forms.GetFathers();
			CxList formNames = validationObjects.FindByFathers(formAssignments).FindByType(typeof(StringLiteral));
			// For every form name (we need it because we need the form name when checking its fields)
			foreach (CxList formName in formNames)
			{
				// Find the relevant class object (related to this form name and inherits from ValidationForm)
				CSharpGraph formGraph = formName.TryGetCSharpGraph<CSharpGraph>();
				string className = formGraph.ShortName.Replace("\"", "");
				CxList classObject = classDecl.FindByShortName(className);
				CxList validatorForm = classObject.InheritsFrom("ValidatorForm");
				// If exist such a validation form
				if (validatorForm.Count > 0)
				{
					// Find all the fields in the form
					CxList validatorFormFields = fieldDecl.GetByAncs(validatorForm);
					
					// Find all property names in the validator
					CxList ifStmt = formName.GetAncOfType(typeof(IfStmt));
					CxList inFormName = validationObjects.GetByAncs(ifStmt);
					CxList formProperties = properties * inFormName;
					CxList propertyAssignments = formProperties.GetFathers();
					CxList propertyNames = inFormName.FindByFathers(propertyAssignments).FindByType(typeof(StringLiteral));
	
					// Cross-Check every property with the fields in the form
					foreach (CxList propertyName in propertyNames)
					{
						CSharpGraph propertyGraph = propertyName.TryGetCSharpGraph<CSharpGraph>();
						string graphName = propertyGraph.ShortName.Replace("\"", "");
						// If there is no relevant field, the property is useless
						if (validatorFormFields.FindByShortName(graphName).Count == 0)
						{
							result.Add(propertyName);
						}
					}
				}
				else {
					//If the validation form does not exist anymore in the code
					result.Add(formName);
				}
			}
		}
		catch (Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
	}
}