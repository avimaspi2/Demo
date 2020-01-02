// Test struts version
if(Find_Struts1_Presence().Count > 0){
	CxList actionClasses = Find_Action_Forms();
	
	CxList allFields = Find_Field_Decl().GetByAncs(actionClasses);
	CxList allPublicFields = allFields.FindByFieldAttributes(Modifiers.Public);
	CxList allProtectedFields = allFields.FindByFieldAttributes(Modifiers.Protected);
		
	result = allProtectedFields;
	result.Add(allPublicFields);
}