/*
MISRA C RULE 14-6
------------------------------
This query searches for multiple break statements used for loop termination in the same iteration.

	The Example below shows code with vulnerability: 

for (n=1; n<20; n++){
	if (n==5){
		break; 
	}
	if (n = 7){
		break;
	}
}

*/

// find iterations and breaks
CxList iterations = All.FindByType(typeof(IterationStmt));
CxList breaks = All.FindByType(typeof(BreakStmt));
CxList switches = All.FindByType(typeof(SwitchStmt));

foreach(CxList iter in iterations){
	CxList curBreaks = breaks.GetByAncs(iter);
	CxList descIters = iterations.GetByAncs(iter) - iter;
	CxList descSwitches = switches.GetByAncs(iter);
	
	// remove breaks not directly under current loop
	curBreaks -= curBreaks.GetByAncs(descIters);
	curBreaks -= curBreaks.GetByAncs(descSwitches);
	
	if (curBreaks.Count >= 2)
		result.Add(curBreaks);
}