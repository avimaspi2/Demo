CxList OverflowSanitizing = All.FindByName("!=") + All.FindByName("==") +
	All.FindByName("<") + All.FindByName(">") +
	All.FindByName("<=") + All.FindByName(">=")+
	All.FindByName("&&") + All.FindByName("||") + All.FindByName("!");
CxList indx = All.GetByAncs(Find_IndexerRefs());
CxList invoking = All.GetByAncs(Find_Methods());
CxList arrays = Find_ArrayCreateExpr();

result = OverflowSanitizing + indx + invoking + arrays;
result = All.GetByAncs(result);