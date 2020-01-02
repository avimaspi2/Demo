/*
MISRA C RULE 14-4
------------------------------
This query searches for uses of the goto command

	The Example below shows code with vulnerability: 

for (i=0;i< 10 ;i++ ){
	if (i == 6){
		goto out; 
	}
	...
}

out:

*/

result = All.FindByType(typeof(TypeRef)).FindByShortName("goto");