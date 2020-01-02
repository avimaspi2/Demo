CxList hstsConfigs = Find_HSTS_Configuration_In_Code();
CxList djangoMiddlewares = hstsConfigs.FindByType(typeof(ClassDecl));
CxList flaskApps = hstsConfigs.Minus(djangoMiddlewares);
CxList sslifys = flaskApps.GetAncOfType(typeof(MethodInvokeExpr));
foreach(CxList sslify in sslifys){
	CxList sslifyParams = All.GetParameters(sslify);
	bool setSubDomains = false;
	foreach(CxList sslParam in sslifyParams.FindByType(typeof(Param))){
		Param x = (Param) sslParam.GetFirstGraph();
		string paramName = x.Name;
		if(paramName != null){
			if(paramName.Equals("subdomains") ){
				setSubDomains = true;
				string includeSubDomains = sslifyParams.GetByAncs(sslParam)
					.FindByType(typeof(BooleanLiteral)).GetName();
				if(includeSubDomains == null || !includeSubDomains.Equals("true")){
					result.Add(sslParam);
				}
			}
			if(paramName.Equals("age")){
				string maxAge = sslifyParams.GetByAncs(sslParam)
					.FindByType(typeof(IntegerLiteral)).GetName();
				int maxAgeInt = 0;
				if(Int32.TryParse(maxAge, out maxAgeInt)){
					if(maxAgeInt < 31536000 ){
						result.Add(sslParam);
					}
				}
				else{
					result.Add(sslParam);
				}
			}
		}	
	}
	if(!setSubDomains){
		result.Add(sslify);
	}
}
if(result.Count == 0){
	CxList sts = All.GetByAncs(djangoMiddlewares).FindByShortName("Strict-Transport-Security");
	foreach(CxList headerSet in Get_HSTS_Headers(sts)){
		result.Add(Validate_HSTS_Header(headerSet));
	}
}