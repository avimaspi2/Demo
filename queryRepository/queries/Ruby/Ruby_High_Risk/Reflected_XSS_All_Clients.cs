CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_Interactive_Outputs();

bool isXSSVulnerable = true;

CxList gemlocks = Find_Gemlock_Versions();
CxList railsVersion = Find_Gemlock_Versions_For_Package(gemlocks, "rails");

bool useRails = railsVersion.Count > 0;
if (useRails) 
{
	// XSS protection has been back-ported from Rails 3 to Rails 2.3.6.
	// http://weblog.rubyonrails.org/2010/5/23/ruby-on-rails-2-3-6-released/
	CxList railsWithoutXSSProtection = Find_Gemlocks_Not_Satisfying_Version(railsVersion, "2.3.6");

	if (railsWithoutXSSProtection.Count == 0) 
	{
		// Ruby on Rails builtin XSS protection
		isXSSVulnerable = false;
		
		CxList viewMethods = Find_View_Methods();
		CxList viewMembers = Find_View_Members();
		
		CxList ignoreSanitization = viewMethods.FindByShortName("raw");
		ignoreSanitization.Add(viewMembers.FindByShortName("html_safe"));
		
		CxList dataFlows = outputs.DataInfluencedBy(inputs);
		dataFlows.Add(outputs * inputs);
		
		result = dataFlows.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes) * ignoreSanitization;
	}
}

if (isXSSVulnerable) 
{
	CxList sanitized = Find_XSS_Sanitize() + Find_DB();

	result = outputs.InfluencedByAndNotSanitized(inputs, sanitized);
	result.Add(inputs * outputs);
	result.Add(outputs.FindByShortName("params").FindByType(typeof(Param)));
}