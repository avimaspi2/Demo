/*
MISRA CPP RULE 6-6-4
------------------------------
This query searches for a multiple appearance of goto and break statements in an iteration statement

	The Example below shows code with vulnerability: 

for (n=1; n<20; n++){
	if (n==5){
		break;  //non-compliant
		goto s; //non-compliant
	}
	if (n = 7){
		break;  //non-compliant
		goto s; //non-compliant
	}
	s:  //some code;

}

*/

// find iterations and breaks
CxList iterations = All.FindByType(typeof(IterationStmt));
CxList  allGoTos=All.FindByFathers(Find_All_Declarators()).FindByName("goto");
CxList gotos = allGoTos;
CxList breaks = All.FindByType(typeof(BreakStmt));
CxList switches = All.FindByType(typeof(SwitchStmt));
foreach(CxList iter in iterations){
	
	CxList curBreaks = breaks.GetByAncs(iter);
	CxList curGotos = gotos.GetByAncs(iter);
	CxList descIters = iterations.GetByAncs(iter) - iter;
	CxList descSwitches = switches.GetByAncs(iter);
	
	// remove breaks not directly under current loop
	curBreaks -= curBreaks.GetByAncs(descIters);
	curGotos -= curGotos.GetByAncs(descIters);
	curBreaks -= curBreaks.GetByAncs(descSwitches);
	
	if (curBreaks.Count >= 2)
		result.Add(curBreaks);
	if(curGotos.Count >= 2)
	{
		result.Add(curGotos);
	}
}