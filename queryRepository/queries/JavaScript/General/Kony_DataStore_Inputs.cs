/* This query finds all Kony's data store values retrival */

if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList konyDataStore = Kony_All().FindByMemberAccess("kony.ds");
	result = konyDataStore.GetMembersOfTarget().FindByShortName("read");
}