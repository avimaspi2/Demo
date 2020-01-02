// Find all ESAPI references
CxList ESAPI = All.FindByMemberAccess("ESAPI.*");
// Add validation rules that might not be found above
ESAPI.Add(All.FindByShortName("*ValidationRule"));

// All members that are assigned an ESAPI reference are considered ESAPI as well, since we don't have types in PHP
ESAPI.Add(All.GetByAncs(ESAPI.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left));
ESAPI.Add(All.FindAllReferences(ESAPI));

// Return the ESAPI methods
result = ESAPI.GetMembersOfTarget();