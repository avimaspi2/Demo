if(param.Length == 1){
	CxList elements = param[0] as CxList;
	
	CxList AdressParams = elements.FindByType(typeof(UnaryExpr)).FindByShortName("Address");
	result.Add(elements - AdressParams);
	result.Add(All.FindByFathers(AdressParams));
}