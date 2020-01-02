CxList methods = Find_MethodDecls();
CxList selectors = All.FindByShortName("@selector");
CxList invokes = selectors.FindByType(typeof(MethodInvokeExpr));
CxList parms = All.GetByAncs(invokes).FindByType(typeof(Param));

CxList suspects = All.NewCxList();
foreach (CxList parm in parms) {
	try {
		CSharpGraph gr = parm.TryGetCSharpGraph<CSharpGraph>();
		// not null; + try catch
		if (gr != null) {
			string name = gr.ShortName;
			if (!name.EndsWith(":")) {				
				CxList method = methods.FindByShortName(name + ":");
				// A method exists with name like the selector param + ":", probably forgot to add the ":"
				if (method.Count > 0) {
					suspects.Add(parm.ConcatenateAllPaths(method));
				}
			}
		}
	} catch (Exception e) {
		cxLog.WriteDebugMessage(e);
	}
}

result = suspects;