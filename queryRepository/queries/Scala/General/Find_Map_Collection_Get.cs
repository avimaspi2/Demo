// Find val x = Map(); x.get()
result = All.FindAllReferences(All.FindByType(typeof(DictionaryCreateExpr))
	.GetAssignee()).GetMembersOfTarget().FindByShortName("get");