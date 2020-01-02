// Test struts version
if(Find_Struts1_Presence().Count > 0){
	CxList configXML = All.FindByFileName("*config.xml");
	CxList actionValidate = configXML.FindByName("STRUTS_CONFIG.ACTION_MAPPINGS.ACTION.VALIDATE");
	CxList falseStrings = configXML.FindByName("\"false\"").FindByType(typeof(StringLiteral));
	
	CxList validateAssignments = actionValidate.GetFathers();
	result = configXML.FindByFathers(validateAssignments)*falseStrings;
}