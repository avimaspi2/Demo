/*
MISRA C RULE 14-8
------------------------------
This query searches for iterations or switch statements not followed by a compound statement

	The Example below shows code with vulnerability: 

for (i=0;i<6;i++)
	a++;

*/

CxList potentials = All.FindByType(typeof(SwitchStmt)) + All.FindByType(typeof(IterationStmt));

// Remove iterations followed by compound statements
CxList directSC = All.FindByType(typeof(StatementCollection)).FindByFathers(potentials);
CxList allCompound = directSC.FindByRegex("{");

allCompound -= (allCompound - directSC).GetByAncs(directSC);

potentials -= allCompound.GetFathers();

// Remove switches followed by compound statements
potentials -= potentials.FindByRegex(@"\Wswitch\s*\([^\)]*?\)[^;\{]*?\{",false,false,false);

result = potentials;