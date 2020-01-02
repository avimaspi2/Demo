/*This query will look for mail interactive outputs */

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	result = XSAll.FindByShortName("text").FindByAssignmentSide(CxList.AssignmentSide.Left)
		.DataInfluencingOn(XSAll.FindByName("*Mail.Part"));
}