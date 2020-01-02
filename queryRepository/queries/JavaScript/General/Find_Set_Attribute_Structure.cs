CxList document = Find_DOM_Elements();
CxList DocInputs = All.NewCxList();
List<string> formsFileNames = new List<string>{"*element*",
		"all","body","embeds","forms","head","scripts",
		"querySelector*","currentScript","createDocumentFragment",
		"createTextNode","createAttribute"};

 
foreach(CxList doc in document)
{
	CxList tempDoc = doc.Clone();
	bool found = false;
	for(int i = 0; i < 10; i++)
	{
		CxList temp = tempDoc.GetMembersOfTarget();
		
		found = found || 
			temp.FindByShortNames(formsFileNames, false).Count > 0;
		
		if(temp.Count == 0 && found){
			DocInputs.Add(tempDoc);
			break;
		} 
		tempDoc = temp;
		
	}
}

CxList ae = DocInputs.GetFathers().GetFathers();
CxList ur = Find_UnknownReference();
CxList allLeftOfAssign = Find_Assign_Lefts();
CxList relevantLeft = allLeftOfAssign.GetByAncs(ae);
CxList allRefOfRelevant = ur.FindAllReferences(relevantLeft);
CxList expression = allRefOfRelevant.GetFathers();
CxList ma = Find_MemberAccesses();
CxList invokes = Find_Methods();
//two cases:
//1) AssignExpr
//2) ExprStmt

//in case of AssignExpr
CxList relevantAccessor = (allLeftOfAssign * ma).GetByAncs(expression);

relevantAccessor.Add(allLeftOfAssign * DocInputs);

result = relevantAccessor;
//in case of set attribute
result.Add(invokes.FindByShortName("setAttribute"));