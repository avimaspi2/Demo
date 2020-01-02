CxList classes = Find_Class_Decl();

result = Find_Validator_Forms();
result.Add(classes.InheritsFrom("ActionForm"));
result.Add(classes.InheritsFrom("DynaActionForm"));