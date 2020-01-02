/* This query finds all Kony's data store values saving */

if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList konyDataStore = Kony_All().FindByMemberAccess("kony.ds");
	result = konyDataStore.GetMembersOfTarget().FindByShortName("save");
}