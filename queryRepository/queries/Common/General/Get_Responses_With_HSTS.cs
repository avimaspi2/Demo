if(param.Length == 2){
	CxList responses = (CxList) param[0];
	CxList hstsHeaders = (CxList) param[1];
	result = responses.InfluencedBy(hstsHeaders);
}