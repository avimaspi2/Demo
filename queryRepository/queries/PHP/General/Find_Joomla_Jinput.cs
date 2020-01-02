//Finds the methods of JInput, from http://docs.joomla.org/JInput/11.1
CxList urs = All.FindByType(typeof(UnknownReference));
CxList jInput = All.FindByShortName("JFactory", false).GetMembersOfTarget();
jInput = jInput.FindByShortName("getApplication", false).GetMembersOfTarget().FindByShortName("input", false);
CxList binaryAnc = jInput.GetAncOfType(typeof(AssignExpr));
jInput.Add(urs.FindAllReferences(urs.GetByAncs(binaryAnc).FindByAssignmentSide(CxList.AssignmentSide.Left)));
jInput.Add(All.FindByType("JInput"));

CxList jInputGet = jInput.GetMembersOfTarget();
result = jInputGet + jInputGet.GetMembersOfTarget() - jInputGet.GetMembersOfTarget().GetTargetOfMembers();