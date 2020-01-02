if(param.Length == 2){
	CxList include_list = param[0] as CxList;
	CxList exclude_list = param[1] as CxList;
	if(include_list != null && exclude_list != null){
		result = include_list - exclude_list;
	}
}