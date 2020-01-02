/*
MISRA C RULE 2-2
------------------------------
This query searches for CPP style "//" comments

	The Example below shows code with vulnerability: 

/* Function comment is compliant. * /
void mc2_0202 ( void )
{
use_int32(0);   // Comment Not Compliant
}

*/

CxList dummy;
CxList commentFinds = All.NewCxList();
CxList extendedResult = All.NewCxList();

// find instances of "//" comments
dummy = All.FindByRegex(@"//", true, false, false, extendedResult);

// remove "//" inside a "/* */" comment
dummy = All.FindByRegex(@"/\*.*?\*/", true, false, false, commentFinds);
extendedResult -= extendedResult.FindByRegexSecondOrder("//", commentFinds);

result = All.FindByRegexSecondOrder(@".*", extendedResult);