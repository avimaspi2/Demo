/* This query validates the values of HSTS configuration in xml files : 
		checks if "enabled" is set to "true"
		checks if "max-age" is set to a value equal or greater than 31536000 seconds
		checks if "includeSubDomains" is set to true
	if any of these conditions fail, the result will be the xml node(s) where
	the validation failed
*/
CxList xml = All.FindByFileName("*.xml");
CxList stringLiterals = Find_Strings();

Func < CxList,CxList > getValueOf = (attribute) => {
	attribute = attribute.GetAncOfType(typeof(IfStmt)).GetFathers();
	attribute = xml.GetByAncs(attribute);

	CxList attribValue = stringLiterals.GetByAncs(
		attribute.FindByShortName("param_value", false).GetFathers());
	attribValue = attribValue.FindByType(typeof(StringLiteral));
	
	return attribValue;
	};
CxList config = Find_HSTS_Configuration_In_Config_File();
CxList key = config.FindByShortNames(new List<string>(){"*httpHeaderSecurityFilter","*HstsFilter"}, false);
if(key.Count > 0 ){
	/* handle tomcat web.xml configuration following this sintax:
		<filter>
			<filter-name>httpHeaderSecurity</filter-name>
			<filter-class>org.apache.catalina.filters.HttpHeaderSecurityFilter</filter-class>
			<async-supported>true</async-supported>
			<init-param>
				<param-name>hstsMaxAgeSeconds</param-name>
				<param-value>31536000</param-value>
			</init-param>
			<init-param>
				<param-name>hstsIncludeSubDomains</param-name>
				<param-value>true</param-value>
			</init-param>
		</filter>
	*/
	CxList filter = All.GetByAncs(key.GetAncOfType(typeof(IfStmt)).GetFathers());

	CxList filterMapping = All.GetByAncs(xml.FindByFiles(filter).FindByShortName("mapping", false).GetAncOfType(typeof(IfStmt)));
	filterMapping = filterMapping.FindByShortNames(new List<string>(){"*httpHeaderSecurityFilter","*HstsFilter"}, false).GetAncOfType(typeof(IfStmt)).GetFathers();
	if(filterMapping.Count == 0){
		result.Add(filter);
	}
	else{
		filterMapping = All.GetByAncs(filterMapping);
		CxList urlPatterns = stringLiterals.GetByAncs(filterMapping.FindByShortName("pattern", false).GetAncOfType(typeof(IfStmt)));
		bool correctPattern = false;
		foreach(CxList urlPattern in urlPatterns){
			string pattern = urlPattern.GetName();
			if(pattern.Equals("*") || pattern.Equals("/*")){
				correctPattern = true;
			}
		}
		if(!correctPattern){
			result.Add(urlPatterns);
		}
	}
	/* check the "hstsEnabled" value */
	CxList hstsEnabled = filter.FindByShortName("hstsEnabled", false);
	if(hstsEnabled.Count > 0){
		hstsEnabled = hstsEnabled.GetAncOfType(typeof(IfStmt)).GetFathers();
		hstsEnabled = xml.GetByAncs(hstsEnabled);
		CxList isEnabled = getValueOf(hstsEnabled);
		if(!isEnabled.GetName().Equals("true", StringComparison.CurrentCultureIgnoreCase)){
			result.Add(isEnabled);
		}
	}
	/* check the "maxAgeSeconds" value */
	CxList maxAge = filter.FindByShortName("*maxAgeSeconds", false);
	if(maxAge.Count == 0){
		result.Add(key);
	}
	else{
		maxAge = maxAge.GetAncOfType(typeof(IfStmt)).GetFathers();
		maxAge = xml.GetByAncs(maxAge);
		CxList maxAgeValue = getValueOf(maxAge);
			
		int maxAgeIntValue;
		if(Int32.TryParse(maxAgeValue.GetName(), out maxAgeIntValue)){
			if(maxAgeIntValue < 31536000){
				result.Add(maxAgeValue);
			}
		}
		else{
			result.Add(maxAgeValue);
		}
	}
	/* check "includeSubDomains" value */
	CxList incSubDomains = filter.FindByShortName("*includeSubDomains", false);
	if(incSubDomains.Count == 0){
		result.Add(key);
	}
	else{
		incSubDomains = incSubDomains.GetAncOfType(typeof(IfStmt)).GetFathers();
		incSubDomains = xml.GetByAncs(incSubDomains);
		CxList incSubDomainsValue = getValueOf(incSubDomains);
		if(!incSubDomainsValue.GetName().Equals("true", StringComparison.CurrentCultureIgnoreCase)){
			result.Add(incSubDomainsValue);
		}
	}
}