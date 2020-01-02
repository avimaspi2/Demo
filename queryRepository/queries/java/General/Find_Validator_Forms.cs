CxList classes = Find_Class_Decl();

result = All.NewCxList();
result.Add(classes.InheritsFrom("ValidatorForm"));
result.Add(classes.InheritsFrom("ValidatorActionForm"));
result.Add(classes.InheritsFrom("DynaValidatorForm"));
result.Add(classes.InheritsFrom("DynaValidatorActionForm"));