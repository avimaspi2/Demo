result = Find_Plain_Interactive_Inputs();

//Adds GetNextPar as an Interactive Input
CxList getNextPar = All.FindByName("httpContext.GetNextPar");

result.Add(getNextPar);
result.Add(All.FindAllReferences(All.FindDefinition(getNextPar)));
result.Add(All.FindAllReferences(getNextPar));
result.Add(All.FindByMemberAccess("httpContext.GetNextPar"));

result -= Find_Dead_Code_Contents();

//Checks to see if an incoming input influences another incoming input and saves the one being influenced
CxList firstInput = All.NewCxList();
foreach (CxList temp in result) {
	if ((temp.GetRightmostMember() * result).Count > 0) {
		firstInput.Add(temp);	
	}
}
result -= firstInput;