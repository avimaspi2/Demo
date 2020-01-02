/*
MISRA C RULE 2-4
------------------------------
This query searches for sections of code that have been commented out

	The Example below shows code with vulnerability: 

/* Function comment is compliant. * /
void mc2_0202 ( void )
{
use_int32(0);   // Comment Not Compliant
}

*/


// Find all comments ending with } or ;
CxList extendedResult = All.NewCxList();

// All /* */ comments
CxList res = All.FindByRegex(@"/\*.*?\*/", true, false, false, extendedResult);

// Search results for } or ; at end of comment
result = All.FindByRegexSecondOrder(@"[;{}]\s*\*/", extendedResult);