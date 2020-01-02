CxList evilStringInputs = Find_Evil_Strings();
evilStringInputs.Add(Find_Inputs());

result = Find_ReDoS(evilStringInputs, false);