// Test struts version
if(Find_Struts1_Presence().Count > 0){
	CxList forms_without_validators = All.InheritsFrom("ActionForm");
	forms_without_validators.Add(All.InheritsFrom("DynaActionForm"));
	
	result = forms_without_validators;
}