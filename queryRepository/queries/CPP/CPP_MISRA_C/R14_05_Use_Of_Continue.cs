/*
MISRA C RULE 14-5
------------------------------
This query searches for uses of the continue command

	The Example below shows code with vulnerability: 

for (i=0;i< 10 ;i++ ){
	if (i == 6){
		continue; 
	}
	...
}

*/

result = All.FindByType(typeof(ContinueStmt));