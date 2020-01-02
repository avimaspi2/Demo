CxList codeConfig = Find_HSTS_Configuration_In_Code();
CxList stringLiterals = Find_Strings();
CxList xml = All.FindByFileName("*.config");
CxList methods = Find_Methods();
if(codeConfig.Count > 0){
	CxList hstsElemChanges = All.FindAllReferences(codeConfig)
		.GetAncOfType(typeof(IndexerRef));
	/* At this point we're working on something that should look like this:
			hstsElement["enabled"] = true;
            hstsElement["max-age"] = 31536000;
            hstsElement["includeSubDomains"] = true;
		where the variable "hstsElemChanges" contains all references of
		the "hstsElement" variable in this example */
	
	/* check the value of "enabled" */
	CxList stringEnabled = stringLiterals.FindByShortName("enabled", false);
	CxList enabledChange = stringEnabled.GetByAncs(hstsElemChanges);
	if(enabledChange.Count > 0){
		CxList isEnabled = enabledChange.GetAncOfType(typeof(IndexerRef))
			.GetAssigner();
		if(!isEnabled.GetName().Equals("true", StringComparison.CurrentCultureIgnoreCase)){
			result.Add(isEnabled);
		}
	}
	
	/* check the value of "max-age" */
	CxList stringMaxAge = stringLiterals.FindByShortName("max-age", false);
	CxList maxAgeChange = stringMaxAge.GetByAncs(hstsElemChanges);
	if(maxAgeChange.Count > 0){
		CxList maxAge = maxAgeChange.GetAncOfType(typeof(IndexerRef))
			.GetAssigner();
		int maxAgeValue;
		if(Int32.TryParse(maxAge.GetName(), out maxAgeValue)){
			if(maxAgeValue < 31536000){
				result.Add(maxAge);
			}
		}
		else{
			result.Add(maxAge);
		}
		
		/* check the value of "includeSubDomains" */
		CxList stringIncSubDomains = stringLiterals
			.FindByShortName("includeSubDomains", false);
		CxList incSubDomainsChange = stringIncSubDomains.GetByAncs(hstsElemChanges);
		if(incSubDomainsChange.Count > 0){
			CxList incSubDomains = incSubDomainsChange.GetAncOfType(typeof(IndexerRef))
				.GetAssigner();
			if(!incSubDomains.GetName().Equals("true", StringComparison.CurrentCultureIgnoreCase)){
				result.Add(incSubDomains);
			}
		}
		else{
			result = codeConfig;
		}
	}
	else{
		result = codeConfig;
	}
}
else{	
	CxList config = Find_HSTS_Configuration_In_Config_File();
	CxList key = config.FindByShortName("*strict-transport-security*", false);
	if(key.Count > 0){
			/* handle web.config configuration following this syntax
				<rule name="Add Strict-Transport-Security when HTTPS" enabled="true">
					<match serverVariable="RESPONSE_Strict_Transport_Security" ... />
					<action ... value="max-age=31536000" />
				</rule> */
		
		CxList val = xml.FindByShortName("max-age=*");
		val = val.GetByAncs(key.GetAncOfType(typeof(IfStmt)))
			.FindByType(typeof(StringLiteral));
		if(val.Count > 0){
			CxList x = Validate_HSTS_Header(val);
			if(x.Count > 0){
				result.Add(val);
			}
		}
		else{
			result.Add(key);
		}
	}
	else{
		key = config.FindByShortName("hsts", false).GetAncOfType(typeof(IfStmt));
		if(key.Count > 0){
			/* handle web.config configuration following this syntax
		        <site>
		            <hsts enabled="true" max-age="31536000" includeSubDomains="true" redirectHttpToHttps="true" />
		        </site> */
		
			CxList enabled = xml.FindByShortName("enabled", false)
				.GetByAncs(key).GetAssigner();
			CxList maxAge = xml.FindByShortName("max_age", false)
				.GetByAncs(key).GetAssigner();
			CxList includeSubDomains = xml.FindByShortName("includeSubDomains", false)
				.GetByAncs(key).GetAssigner();
			string isEnabled = enabled.GetName();
			if(!isEnabled.Equals("true", StringComparison.CurrentCultureIgnoreCase)){
				result.Add(enabled.GetAssignee());
			}
			int maxAgeValue;
			if(Int32.TryParse(maxAge.GetName(), out maxAgeValue)){
				if(maxAgeValue < 31536000){
					result = maxAge.GetAssignee();
				}
			}
			else{
				result = maxAge.GetAssignee();
			}
			string subDomains = includeSubDomains.GetName();
			if(!subDomains.Equals("true", StringComparison.CurrentCultureIgnoreCase)){
				result.Add(includeSubDomains.GetAssignee());
			}
		}
		else{
			CxList hstsAspNetCore = Find_HSTS_Configuration_In_Code_ASPNetCore();
			if(hstsAspNetCore.Count > 0){
				/*
				For hsts config such as:
				services.Configure<HstsOptions>(options => {
	                options.IncludeSubDomains = true;
	                options.MaxAge = TimeSpan.FromDays(365);
            	});
				*/
				CxList greaterThanYearInSecs = All.FindByAbstractValue(absVal => 
					absVal is IntegerIntervalAbstractValue iiav && iiav.LowerIntervalBound >= 31536000);
				CxList greaterThanYearInDays = All.FindByAbstractValue(absVal => 
					absVal is IntegerIntervalAbstractValue iiav && iiav.LowerIntervalBound >= 365);
				CxList timSpanFromDays = methods.FindByMemberAccess("TimeSpan.FromDays");
				CxList timSpanFromSecs = methods.FindByMemberAccess("TimeSpan.FromSeconds");
				
				CxList safeSpan = timSpanFromDays.FindByParameters(greaterThanYearInDays);
				safeSpan.Add(timSpanFromSecs.FindByParameters(greaterThanYearInSecs));
				
				CxList safeMaxAge = safeSpan.GetAssignee().FindByShortName("MaxAge");
				CxList safeIncludeSubDomains = All.FindByAbstractValue(absVal => absVal is TrueAbstractValue)
					.FindByShortName("IncludeSubDomains");
				result.Add(hstsAspNetCore - ((safeIncludeSubDomains
					.GetAncOfType(typeof(MethodInvokeExpr)) * safeMaxAge.GetAncOfType(typeof(MethodInvokeExpr))) * hstsAspNetCore));
				//Max age is 30 days by default which is insufficient
				result.Add(hstsAspNetCore.FindByShortName("UseHsts"));
			}
		}
	}
}