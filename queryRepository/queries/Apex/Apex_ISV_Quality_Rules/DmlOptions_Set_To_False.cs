//finds all dmloptions that are set to false
CxList dml = (All.FindByMemberAccess("DMLOptions.optAllOrNone", false));
dml = dml.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList boolean = All.FindByType(typeof(BooleanLiteral)).FindByShortName("false");
CxList val = boolean.FindByAssignmentSide(CxList.AssignmentSide.Right);
result = val.GetByAncs(dml.GetAncOfType(typeof(AssignExpr)));