CxList corsEnabled = All.NewCxList();

CxList configObjects = All.FindByFileName("*.xml");
CxList filterName = configObjects.FindByShortName("FILTER_NAME").FindByRegex("CorsFilter").GetAncOfType(typeof(IfStmt));
CxList urlPattern = configObjects.FindByShortName("URL_PATTERN").FindByRegex("\\*").GetAncOfType(typeof(IfStmt));


foreach (CxList filter in filterName){
	foreach (CxList url in urlPattern){
		if (filter.GetFathers() == url.GetFathers()){
			corsEnabled.Add(url);
		}
	}
}

CxList paramName = configObjects.FindByShortName("PARAM_NAME").FindByRegex("cors.allowed.origins").GetAncOfType(typeof(IfStmt));
paramName.Add(configObjects.FindByShortName("PARAM_NAME").FindByRegex("cors.allowOrigin").GetAncOfType(typeof(IfStmt)));
CxList paramValue = configObjects.FindByShortName("PARAM_VALUE").FindByRegex("\\*").GetAncOfType(typeof(IfStmt));

foreach (CxList name in paramName){
	foreach (CxList parValue in paramValue){
	if (name.GetFathers() == parValue.GetFathers()){
			corsEnabled.Add(parValue);
		}
	}
}

result = corsEnabled;