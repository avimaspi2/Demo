CxList deadCode = Find_Dead_Code_Contents();
CxList ctch = All.FindByType(typeof(Catch));
CxList class_of_ctch = All.GetClass(ctch);
		
CxList exc = All.FindAllReferences(ctch) - ctch;
CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_Integers() + deadCode;
	
result = inputs.InfluencedByAndNotSanitized(exc, sanitize);