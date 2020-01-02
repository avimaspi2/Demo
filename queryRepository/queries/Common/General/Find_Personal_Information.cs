if(param.Length == 2){
	CxList includeList = param[0] as CxList;
	CxList excludeList = param[1] as CxList;
	if(includeList != null && excludeList != null){
		result = includeList - excludeList;
	}
}