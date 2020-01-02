result = Find_UnknownReference().FindByName("*_SESSION_*");
result -= result.FindByAssignmentSide(CxList.AssignmentSide.Left);