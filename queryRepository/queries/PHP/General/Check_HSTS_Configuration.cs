CxList globalConfigHeader = Find_HSTS_Configuration_In_Code();
if(Validate_HSTS_Header(globalConfigHeader).Count > 0){
	CxList headerSet = globalConfigHeader.GetAncOfType(typeof(MethodInvokeExpr));
	result = globalConfigHeader.ConcatenatePath(headerSet,false);
}