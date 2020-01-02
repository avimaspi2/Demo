/*
More than one validator definition was discovered in validators.xml.
Multiple validation definitions with the same name may result in unexpected behavior.

If two validation classes are defined with the same name,the Struts Validator arbitrarily chooses one of the forms
to use for input validation and discards the other. This decision might not correspond to the programmer's expectations.
Moreover, it indicates that the validation logic is not being maintained, and can indicate that other, more subtle,
validation errors are present.
*/

// Test struts version
if(Find_Struts2_Presence().Count > 0){
	CxList validationXML = Find_Validation();
	CxList validationClasses = validationXML.FindByType(typeof(ClassDecl));
	
	// Run on every Validation file separately
	foreach(CxList validationClass in validationClasses)
	{
		CxList validationObjects = validationXML.GetByAncs(validationClass);
		CxList validators = validationObjects.FindByName("VALIDATORS.FIELD.NAME");
		CxList fieldAssignments = validators.GetFathers();
		CxList fieldNames = validationObjects.FindByFathers(fieldAssignments).FindByType(typeof(StringLiteral));;
	
		// For every file check if there are 2 validators with the same name
		foreach(CxList curField in fieldNames)
		{
			if (fieldNames.FindByShortName(curField).Count > 1)
			{
					result.Add(curField);
			}
		}
	}
}