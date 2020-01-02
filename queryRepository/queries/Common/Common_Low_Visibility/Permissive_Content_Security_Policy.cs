List < string > unsafeDirectives = new List<string>() {"default-src", "form-action", "frame-ancestors"};

// Find CSP in Config files and Code
CxList cspInConfig = general.Find_CSP_Configuration_In_ConfigFiles();
CxList cspInCode = general.Find_CSP_Configuration_In_Code();

CxList cspMethods = All.NewCxList();
cspMethods.Add(cspInConfig);
cspMethods.Add(cspInCode.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

if (cspMethods.Count > 0) {
	CxList strings = Find_Strings();
	CxList cspStrings = strings.GetParameters(cspMethods, 1);
	cspStrings.Add(cspInCode.FindByType(typeof(MemberAccess)).GetAssigner());
	
	foreach(CxList csp in cspStrings) {
		foreach(string directive in unsafeDirectives) {
			CxList exists = csp.FindByRegex(@"["";]\s*" + directive + @"\s+");
			if (exists.Count == 0 || csp.FindByRegex(directive + @"\s+\*\s*[;""]").Count > 0) {
				result.Add(csp);
			}
		}
	}
}

// Find CSP in HTML
CxList cspInHtml = general.Find_CSP_Configuration_In_HTML();
foreach (CxList content in cspInHtml) {
	// Find content string
	string contentRegex = @"content\s*=\s*""([^""]*)(""|$)";
	Match match = Regex.Match(content.GetName(), contentRegex);
	if (match.Success)
	{
		try
		{
			string contentValue = match.Groups[1].Value;
			
			// Find unsafe directives inside content value
			foreach(string directive in unsafeDirectives) {
				Match matchDirective = Regex.Match(contentValue, directive + @"\s+");
				if (matchDirective.Success) {
					// Check if directive's value is a wildcard * 
					Match matchWildcard = Regex.Match(match.Groups[0].Value, directive + @"\s+\*\s*[;""]");
					if (matchWildcard.Success) {
						result.Add(content); 
					}
				}
				else {
					// Didn't find the directive
					result.Add(content);
				}
			}
		}
		catch (Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
	}
}