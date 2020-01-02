CxList production = All.FindByFileName(@"*\environments\production.rb");

CxList consider = production.FindByShortName("consider_all_requests_local");

//if (consider.Count == 0)
//{
//	result = production.GetAncOfType(typeof(NamespaceDecl));
//}
//else
//{
	result = production.DataInfluencingOn(consider).FindByShortName("true");
//}