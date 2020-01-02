CxList cout = All.FindByShortNames(new List<string>{"cout", "cerr", "clog", "ostream"});
CxList cout_right = cout.FindByAssignmentSide(CxList.AssignmentSide.Left).GetAncOfType(typeof(AssignExpr));
CxList right = All.GetByAncs(All.FindByAssignmentSide(CxList.AssignmentSide.Right));

result = right - right.GetByAncs(cout_right);