// Test struts version
if(Find_Struts1_Presence().Count > 0){
	CxList cl = Find_Action_Classes();
	CxList fields = Find_Field_Decl();
	fields = fields.GetByAncs(cl);
	
	CxList staticFields =  fields.FindByFieldAttributes(Checkmarx.Dom.Modifiers.Static);
	result = fields - staticFields;
}