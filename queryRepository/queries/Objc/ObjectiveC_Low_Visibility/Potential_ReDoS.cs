CxList evilStringsInputs = All.NewCxList();
evilStringsInputs.Add(Find_Evil_Strings());
evilStringsInputs.Add(Find_Inputs());

result = Find_ReDoS(evilStringsInputs, true);