/* The query searches for usage of 'getInstallerPackageName'. If found, it checks if it being used inside a condition.
   If so, then it assumes a proper verification is implemented.
   Otherwise, it will alert. 
*/
CxList unknownReferences = Find_UnknownReference();
CxList installerPackageName = All.FindByMemberAccess("*.getInstallerPackageName");
CxList influencedReferences = installerPackageName.DataInfluencingOn(unknownReferences)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
installerPackageName.Add(influencedReferences);
CxList ifStatement = installerPackageName.GetAncOfType(typeof(IfStmt));
ifStatement.Add(installerPackageName.GetAncOfType(typeof(TernaryExpr)));

if(ifStatement.Count == 0)
{
	CxList androidSettings = Find_Android_Settings();
	result = androidSettings.FindByMemberAccess("ACTIVITY.ANDROID_NAME");
}