// Fiori apps should call OData services in batch mode. So this query looks for service definitions
// where useBatch flag is either not present or not set to true
if(cxScan.IsFrameworkActive("SAPUI"))
{
	CxList configFiles = All.FindByFileName("*Configuration.js");
	configFiles.Add(All.FindByFileName("*Configuration.ts"));
	CxList associativeArrayExpr = configFiles.FindByType(typeof(AssociativeArrayExpr));

	CxList decls = Find_Declarators();
	foreach (CxList arrayExpr in associativeArrayExpr) {
		CxList declarator = decls.GetByAncs(arrayExpr);
		CxList useBatch = declarator.FindByShortName("useBatch").GetAssigner().FindByName("*true*"); // useBatch: 'true'
		CxList services = declarator.FindByShortName("serviceUrl");
		if(useBatch.Count == 0)
		{
			result.Add(services);
		}	
	}
}