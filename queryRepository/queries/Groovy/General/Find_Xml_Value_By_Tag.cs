// Returns the assignment ("=" at the top of parse tree) for <LABEL TAG = VALUE>
if (param.Length == 3)
{
	CxList xmlFiles = param[0] as CxList;
	string labelNameStr = param[1] as string;
	string tagNameStr = param[2] as string;

	// Returns the assignment (top of parse tree) for XMLTAG = VALUE 
	CxList xmlComponentsReference = xmlFiles.FindByMemberAccess(labelNameStr).GetAncOfType(typeof(AssignExpr));
	; // getancoftype assignexpr

	// Returns all left sides under xmlComponentsReference
	CxList xmlVariables = xmlFiles.GetByAncs(xmlComponentsReference).FindByAssignmentSide(CxList.AssignmentSide.Left);
	
	CxList attribute = xmlVariables.FindByName(tagNameStr);
	CxList attributeFathers = attribute.GetAncOfType(typeof(AssignExpr));
	CxList attributeValue = xmlFiles.GetByAncs(attributeFathers).FindByAssignmentSide(CxList.AssignmentSide.Right);
	result = attributeValue;
}