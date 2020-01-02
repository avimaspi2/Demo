CxList binary = All.FindByType(typeof(BinaryExpr));

result = All.FindByMemberAccess("String.matches") +
	All.FindByMemberAccess("object.matches") +
	All.FindByMemberAccess("Pattern.matches") + 
	binary.FindByName("==~");