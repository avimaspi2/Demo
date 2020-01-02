// Test struts version
if(Find_Struts1_Presence().Count > 0){
	CxList validator_forms = Find_Validator_Forms();
	
	CxList validate_method = All.FindByShortName("validate").FindByType(typeof(MethodDecl)).GetByAncs(validator_forms);
	
	CxList superValidate = All.FindByMemberAccess("ValidatorForm.validate"); 
	superValidate.Add(All.FindByMemberAccess("ValidatorActionForm.validate"));
	superValidate.Add(All.FindByMemberAccess("DynaValidatorForm.validate"));
	superValidate.Add(All.FindByMemberAccess("DynaValidatorActionForm.validate"));
	
	result = validate_method - validate_method.GetMethod(superValidate);
}