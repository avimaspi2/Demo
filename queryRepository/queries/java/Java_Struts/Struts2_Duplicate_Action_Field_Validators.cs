/*
More than one field validator definition with the same name exist in ActionClass-validation.xml.
Duplicate validation definitions with the same name may result in unexpected behavior.
Example: The following entry shows two duplicate field validator definitions.
   <field name="emailField">
      <field-validator type="email" short-circuit="true">
          <message>You must enter a value for email.</message>
      </field-validator>
      <field-validator type="email" short-circuit="true">
          <message>Not a valid e-mail.</message>
      </field-validator>
  </field>
*/

// Test struts version
if(Find_Struts2_Presence().Count > 0){
	CxList validationXML = Find_Validation();
	CxList validationClasses = validationXML.FindByType(typeof(ClassDecl));
	
	// Run on every Validation file separately
	foreach(CxList validationClass in validationClasses)
	{
		CxList validationObjects = validationXML.GetByAncs(validationClass);
		CxList formValidators = validationObjects.FindByName("VALIDATORS.FIELD.NAME");
		CxList fieldGroups = formValidators.GetAncOfType(typeof(IfStmt));
		// Run on every field in the validation file separately
		foreach (CxList fieldGroup in fieldGroups)
		{
			CxList inField = validationObjects.GetByAncs(fieldGroup);
			CxList formValidator = inField.FindByName("VALIDATORS.FIELD.FIELD_VALIDATOR.TYPE");
			CxList formAssignments = formValidator.GetFathers();
			CxList formNames = validationObjects.FindByFathers(formAssignments).FindByType(typeof(StringLiteral));
			// For every field check if there are 2 validators for the same type
			foreach(CxList curForm in formNames)
			{
				if (formNames.FindByShortName(curForm).Count > 1)
				{
					result.Add(curForm);
				}
			}
		}
	}
}