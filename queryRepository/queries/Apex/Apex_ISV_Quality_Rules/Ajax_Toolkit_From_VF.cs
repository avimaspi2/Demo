//this query finds all <script src="../../soap/ajax/26.0/connection.js" in pages or  sforce.connection
CxList scriptSrc = All.FindByShortName("Cx_Script_Src", false);

CxList assignExpr = scriptSrc.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList sl = All.FindByType(typeof(StringLiteral));

CxList rightSide = sl.FindByAssignmentSide(CxList.AssignmentSide.Right).GetByAncs(assignExpr.GetAncOfType(typeof(AssignExpr)));

System.Collections.Generic.List<String> fileList = new System.Collections.Generic.List<String>();
foreach(CxList url in rightSide)
{
	StringLiteral conUrl = url.data.GetByIndex(0) as StringLiteral;
	
	if(conUrl != null)
	{
		string stringUrl = conUrl.Value;
		
		System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"soap\s*/\s*ajax\s*/\s*\d*\.\d*\s*/\s*connection.js");
		System.Text.RegularExpressions.Match urlFoundMatch = r.Match(stringUrl);
		if(urlFoundMatch.Success)
		{
			result.Add(conUrl.NodeId, conUrl);			
		}		
	}
}
result.Add(All.FindByMemberAccess("sforce.connection", false));