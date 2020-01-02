/*
MISRA C RULE 15-5
------------------------------
This query searches for switches not containing any (non default) cases

	The Example below shows code with vulnerability: 

void foo () 
{
    #pragma OPTIMIZE ON
    int i = 1; // irrelevent comment
    
}

*/

CxList switches = All.FindByType(typeof(SwitchStmt));
CxList cases = All.FindByType(typeof(Case));

// go over switches, add switches with no case
foreach (CxList sw in switches){
	CxList sonCases = cases.FindByFathers(sw);
	bool foundCase = false;
	
	// check to see if non default case exists
	foreach (CxList curCase in sonCases){
		Case myCase = curCase.data.GetByIndex(0) as Case;		
		if (!myCase.IsDefault){
			foundCase = true;
		}
	}
	
	if (!foundCase)
		result.Add(sw);
}