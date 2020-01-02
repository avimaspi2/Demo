/*
MISRA C RULE 20-5
------------------------------
This query searches for usage of the error indicator 'errno' from <errno.h>

	The Example below shows code with vulnerability: 

#include <errno.h>

errno = 0; 

*/

// Safety check for the violating h file
CxList includes = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));
CxList hFile = includes.FindByShortName("errno.h");
if (hFile.Count > 0){
	// Start with all instances of errno
	CxList errnos = All.FindByShortName("errno");
	
	// Remove all locally defined instances
	CxList defs = All.FindDefinition(errnos);
	errnos -= errnos.FindAllReferences(defs);
	
	result = errnos;
}