/*
Struts2 requires that custom validators be defined in validators.xml before being used in a Action validator definition.
Missing validator definitions are an indication that validation is not up to date.
Example: The following Action validator was not defined in validators.xml.
<validators>
    <validator name="required" class="com.opensymphony.xwork2.validator.validators.RequiredFieldValidator"/>
</validators>
*/

// Test struts version
if(Find_Struts2_Presence().Count > 0){
	CxList strings = Find_Strings().FindByFileName("*.xml");
	
	// Find all validators
	CxList validation = All.FindByFileName("*-validation.xml");
	CxList validatorType = validation.FindByName("VALIDATORS.FIELD.FIELD_VALIDATOR.TYPE");
	CxList allValidators = strings.FindByFathers(validatorType.GetAncOfType(typeof(AssignExpr)));
	
	// Find validators in validators.xml file
	CxList validators = All.FindByFileName("*validators.xml");
	CxList validatorsName = validators.FindByName("VALIDATORS.VALIDATOR.NAME");
	CxList approvedValidators = strings.FindByFathers(validatorsName.GetAncOfType(typeof(AssignExpr)));
	
	// Return validators that do not appear in validators.xml
	result = allValidators - allValidators.FindByShortName(approvedValidators);
}