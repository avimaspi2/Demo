// Given a gemlock DOM object from a Gemlock File, Output the version and operator
// e.g.:     multi_json (~> 1.0)
//        version=1.0 , operator = ~>
if (param.Length == 1)
{
	try
	{
		
		CxList gemlock = param[0] as CxList;
		
		CxList packageName = All.GetByAncs(gemlock).FindByType(typeof(StringLiteral));
	
		CSharpGraph gr = packageName.data.GetByIndex(0) as CSharpGraph;
		string strPackageName = "NO NAME";
		if (gr != null) {
			strPackageName = gr.ShortName;
		
			CxList assigns = gemlock.GetAncOfType(typeof(AssignExpr));
		
			CxList assignTree = All.GetByAncs(assigns); 
			CxList binaryexp = assignTree.FindByType(typeof(BinaryExpr));
			CxList literals = assignTree.GetByAncs(binaryexp).FindByType(typeof(StringLiteral));
		
			if (literals.Count > 1) {
			
				CSharpGraph gr1 = literals.data.GetByIndex(0) as CSharpGraph;
				CSharpGraph gr2 = literals.data.GetByIndex(1) as CSharpGraph;
				string versionName = gr1.ShortName;
			}

		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}

}