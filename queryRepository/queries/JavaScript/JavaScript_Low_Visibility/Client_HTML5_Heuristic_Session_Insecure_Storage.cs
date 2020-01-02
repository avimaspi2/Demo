//finds all session data saved into storage
//this is done by finding the flow from any indicator of session to the first parameter of stoarage setItem
CxList storageInpus = Find_Storage_Inputs();
CxList irrelevant = Find_Parameters();
irrelevant.Add(Find_Binarys());
CxList firstParam = (All - irrelevant).GetByAncs(All.GetParameters(storageInpus));

CxList sessionTokens = All.FindByShortNames(new List<string>{"session*",  "cookie*"}, false);
sessionTokens -= Find_String_Literal();
CxList target = firstParam * sessionTokens;

CxList sanitizer = Sanitize();
CxList flow = sessionTokens.InfluencingOnAndNotSanitized(firstParam - target, sanitizer);
result = flow.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);
result.Add(target);