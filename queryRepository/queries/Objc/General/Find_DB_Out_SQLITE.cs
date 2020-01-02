CxList methods = Find_Methods();
CxList methsDecl = Find_MethodDecls();
CxList allParams = Find_UnknownReference();
CxList theParams = Find_Param();
CxList sqlit3Meth = methods.FindByShortName("sqlite3_exec");
CxList executeQuery = methods.FindByShortName("executeQuery*");

CxList callbackFuncasParam = allParams.GetParameters(sqlit3Meth, 2);
callbackFuncasParam.Add(allParams.GetParameters(executeQuery, 0));

char[] trimChars = new char[6] {' ', '\t', '"', '(', '\r', '\n'};
CxList calbackFunc = All.NewCxList();

foreach (CxList curCallback in callbackFuncasParam){
	CSharpGraph gr = curCallback.TryGetCSharpGraph<CSharpGraph>();
	try
	{
		string name = gr.ShortName.Trim(trimChars);
		calbackFunc.Add(methsDecl.FindByShortName(name));
	}
	catch(Exception e){
		cxLog.WriteDebugMessage(e);	
	}
}

CxList getTableMeth = methods.FindByShortName("sqlite3_get_table");
CxList getTableParam = All.GetParameters(getTableMeth, 2) - theParams;
CxList valsFromDB = methods.FindByShortName("sqlite3_column_*");

result.Add(calbackFunc);
result.Add(getTableParam);
result.Add(valsFromDB);