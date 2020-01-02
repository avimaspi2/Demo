CxList AllExc = All.FindByType(typeof(Catch));

CxList notGenExc = All.FindAllReferences(AllExc) - 
	All.FindByName("Exception", false).GetAncOfType(typeof(Catch)) - 
	All.FindByName("System.Exception", false).GetAncOfType(typeof(Catch));

CxList genExc = AllExc - notGenExc;

result = AllExc.GetFathers() - (AllExc.GetFathers() * genExc.GetFathers());