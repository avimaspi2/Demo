CxList HTP_package = 
	All.FindByMemberAccess("htp.*", false).FindByType(typeof(MethodInvokeExpr));

CxList HTF_package = 
	All.FindByMemberAccess("htf.*", false).FindByType(typeof(MethodInvokeExpr));

CxList OWA_package = 
	All.FindByMemberAccess("OWA_UTIL.CELLSPRINT", false) + 
	All.FindByMemberAccess("OWA_UTIL.CALENDARPRINT", false) + 
	All.FindByMemberAccess("OWA_UTIL.TABLEPRINT", false);

	
result = HTP_package + HTF_package + OWA_package + Find_APEX_Outputs();