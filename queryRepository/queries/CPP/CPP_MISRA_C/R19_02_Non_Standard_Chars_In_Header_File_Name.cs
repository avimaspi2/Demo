/*
MISRA C RULE 19-2
------------------------------
This query searches for include statements with
(' \ " /*) chars in a < > header file name, or (' \ /*) in a " " header file name

	The Example below shows code with vulnerability: 

#include <mc2_1902'.h>
#include <.\mc2_1902.h>
#include "mc2_1902'.h"
#include ".\mc2_1902.h"

*/

CxList includes = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));

// check for the illegal name flag
foreach (CxList cur in includes){
	StringLiteral g = (StringLiteral) cur.data.GetByIndex(0);
	if (g == null || g.Value == null) {
		continue;
	}
	string curFileName = g.Value;
	if (curFileName.Contains("INVALID_INCLUDE_FILE_NAME_") && 
	(String.Compare(curFileName, "INVALID_INCLUDE_FILE_NAME_") != 0)){
		result.Add(cur);
	}
}