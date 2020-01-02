// Find all the gemlocks nodes which correspond to a given package name
if (param.Length == 2)
{
	CxList res = All.NewCxList();
	CxList gemlocks = param[0] as CxList;
	string packageNameToFind = param[1] as string;
	
	foreach (CxList gemlock in gemlocks) 
	{
		CxList packageNameLst = All.GetByAncs(gemlock).FindByType(typeof(StringLiteral));
		
		CSharpGraph gr = packageNameLst.data.GetByIndex(0) as CSharpGraph;
		if (gr != null) {
			string strPackageName = gr.ShortName.Replace("\"", ""); // Names are surrounded by quotes			
			if (strPackageName.Equals(packageNameToFind)) {
				// Example syntax:
				//    $GemLCK["activesupport"] = ["3.1.0"]+["="]
				// we return the binaryexp node which is the ancestor of other expressions
				CxList assigns = gemlock.GetAncOfType(typeof(AssignExpr));					
				CxList assignTree = All.GetByAncs(assigns); 
				CxList binaryexp = assignTree.FindByType(typeof(BinaryExpr));
				res.Add(binaryexp);
			}
		}
	}
	
	result = res;
}