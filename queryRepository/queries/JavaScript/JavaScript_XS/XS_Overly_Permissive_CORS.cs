/*
This query will look for 'allowOrigin' when it's set to "*".
Cross-Origin Resource Sharing (CORS) permits Web pages from other 
domains to make HTTP requests to your application domain, 
where normally such requests would automatically be refused by the Web browser's security policy,
when setting the 'allowOrigin' to "*" will allow http requests from ALL domains.
*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	// get all ".xsaccess" files
	CxList xsaccess = XSAll.FindByFileName("*.xsaccess");
	
	// get the 'allowOrigin' fields influenced by 'cors'
	CxList cors = xsaccess.FindByShortName("cors", false);
	CxList allowOriginFields = XSAll.FindByShortName("allowOrigin", false).GetByAncs(cors);
	
	// get the strings influenced by the 'allowOrigin' fields
	CxList strings = xsaccess.FindByType(typeof(StringLiteral));
	strings = strings.GetByAncs(allowOriginFields);
	
	foreach(CxList str in strings)
	{	
		CSharpGraph graph = str.TryGetCSharpGraph<CSharpGraph>();
		if(graph == null && graph.ShortName == null)
		{
			continue;	
		}
		// if 'allowOrigin' is "*"
		if(graph.ShortName.Equals(@"""*"""))
		{
			// generate the flow path: ["*"] -> [allowOrigin] -> [cors]
			CxList allowOrigin = str.GetAncOfType(typeof(FieldDecl)) * allowOriginFields;
			CxList strToAllowOrigin = str.ConcatenatePath(allowOrigin, false);
			CxList corsDecl = allowOrigin.GetAncOfType(typeof(Declarator)) * cors;
			CxList allowOriginToCors = strToAllowOrigin.ConcatenatePath(corsDecl, false);
			
			// add flow to result
			result.Add(allowOriginToCors);
		}
	}
}