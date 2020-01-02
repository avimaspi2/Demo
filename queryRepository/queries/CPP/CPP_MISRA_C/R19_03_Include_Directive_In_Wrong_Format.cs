/*
MISRA C RULE 19-3
------------------------------
This query searches for '#include' directives followed by something other than a <filename> or "filename".

	The Example below shows code with vulnerability: 

#include header.h

*/

CxList includes = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));

// check for the wrong format flag
foreach (CxList cur in includes){
	StringLiteral g = (StringLiteral) cur.data.GetByIndex(0);
	if (g == null || g.Value == null) {
		continue;
	}
	string curFileName = g.Value;
	if (String.Compare(curFileName, "INVALID_INCLUDE_FILE_NAME_") == 0){
		result.Add(cur);
	}
}