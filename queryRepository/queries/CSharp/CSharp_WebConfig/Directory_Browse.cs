/*returns all the following expressions in webconfig file: 
<system.webServer>
 	<directoryBrowse enabled="true" />
</system.webServer>	

*/
CxList webConfig = Find_Web_Config();
CxList ma = webConfig.FindByType(typeof(MemberAccess));
CxList enabled =ma.FindByShortName("WEBSERVER").GetByAncs(webConfig.FindByShortName("DIRECTORYBROWSE"));

CxList ae=enabled.GetAncOfType(typeof(AssignExpr));

CxList curAss = ma.FindByShortName("ENABLED").FindByFathers(ae);
ae=curAss.GetAncOfType(typeof(AssignExpr));

CxList sl = webConfig.FindByType(typeof(StringLiteral)).FindByFathers(ae).FindByShortName("true");
result= sl.FindByAssignmentSide(CxList.AssignmentSide.Right);