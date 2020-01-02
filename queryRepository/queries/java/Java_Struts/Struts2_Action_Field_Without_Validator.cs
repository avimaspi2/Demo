/*
One or more Action Fields do not have a corresponding validation definition.
Each field should have an explicit validation routine referenced in ActionClass-validation.xml. 
It is easy for developers to forget to update validation logic when they remove or rename action form mappings.
One indication that validation logic is not being properly maintained is the lack of a validator definition.
*/

// Test struts version
if(Find_Struts2_Presence().Count > 0){
	CxList validators = All.FindByFileName("*-validation.xml");
	CxList validatorsName = validators.FindByName("VALIDATORS.FIELD.NAME");
	validators = Find_Strings().FindByFathers(validatorsName.GetAncOfType(typeof(AssignExpr)));
	
	CxList actions = Find_Action_Classes();
	
	CxList candidates = Find_Integers();
	candidates.Add(All.FindByType("string"));
		
	//Consider also Collections of Strings/Integers
	CxList collections = candidates.FindByType(typeof(GenericTypeRef));
	candidates.Add(collections.GetFathers().GetFathers());
	
	CxList fieldsDecls = candidates * Find_Field_Decl();	
		
	CxList fields = fieldsDecls.FindByFieldAttributes(Checkmarx.Dom.Modifiers.Private);
	fields.Add(fieldsDecls.FindByFieldAttributes(Checkmarx.Dom.Modifiers.Protected));
		
	CxList actionFields = fields.GetByAncs(actions);
	
	foreach (CxList actionField in actionFields)
	{
		CSharpGraph g = actionField.TryGetCSharpGraph<CSharpGraph>();
		string fieldName = g.ShortName;
		if (validators.FindByShortName("\"" + fieldName + "\"").Count == 0)
		{
			result.Add(actionField);
		}
	}
}