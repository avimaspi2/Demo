if(cxScan.IsFrameworkActive("KonyInFF"))
{
	result.Add(All.FindByFileName(@"*\modules\*").FindByFileName(@"*.js"));
	result.Add(All.FindByFileName(@"*\forms\*").FindByFileName(@"*.json"));
}