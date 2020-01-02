CxList AllExc = All.FindByType(typeof(Catch));
CxList AllExcFathers = AllExc.GetFathers();

CxList notGenExc = 	All.FindAllReferences(AllExc) - 
					All.FindByName("Exception").GetFathers() - 
					All.FindByName("System.Exception").GetFathers();

CxList genExc = AllExc - notGenExc;

result = AllExcFathers  - (AllExcFathers * genExc.GetFathers());