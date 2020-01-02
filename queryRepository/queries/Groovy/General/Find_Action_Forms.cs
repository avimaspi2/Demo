CxList classes = Find_Class_Decl();

result = 
	classes.InheritsFrom("ActionForm") + 
	classes.InheritsFrom("DynaActionForm") +
	Find_Validator_Forms();