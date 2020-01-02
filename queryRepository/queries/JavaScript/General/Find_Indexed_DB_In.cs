//Find indexed db in 
CxList objectStore = All.FindByShortName("*ObjectStore", false);
CxList invokes = Find_Methods();
CxList dbIn = invokes.FindByShortNames(new List<string>{"add","put"});
result.Add(dbIn * dbIn.DataInfluencedBy(objectStore));