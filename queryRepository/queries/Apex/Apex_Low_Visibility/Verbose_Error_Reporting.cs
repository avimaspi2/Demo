CxList ctch = All.FindByType(typeof(Catch));
CxList class_of_ctch = All.GetClass(ctch);

ctch = ctch.GetByAncs(class_of_ctch);
CxList outputs = Find_Interactive_Outputs();
CxList exc = All.FindAllReferences(ctch) - ctch;

result = outputs.InfluencedByAndNotSanitized(exc, Find_Test_Code());

result -= Find_Test_Code();