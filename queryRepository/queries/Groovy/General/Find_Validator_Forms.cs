CxList classes = Find_Class_Decl();

result = 
	classes.InheritsFrom("ValidatorForm") + 
	classes.InheritsFrom("ValidatorActionForm") + 
	classes.InheritsFrom("DynaValidatorForm") + 
	classes.InheritsFrom("DynaValidatorActionForm");