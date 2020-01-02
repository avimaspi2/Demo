/*
MISRA C RULE 2-3
------------------------------
This query searches for nested C style "/* /* * /" comments

	The Example below shows code with vulnerability: 

/* Function comment is compliant. * /
void mc2_0203 ( void )
{
use_int32(0);

   /* The next function call is very important. However, the end comment
      marker is accidentally omitted from the text that explains this...

   Perform_Safety_Critical_Function ( X );

   /* The function is not called and this comment is not compliant. * /
}

*/

// find all C style comments containing "/*"
CxList extendedResult = All.NewCxList();

// All /* */ comments
CxList res = All.FindByRegex(@"/\*.*?\*/", true, false, false, extendedResult);

// Search for "/*    /*    */"
result = All.FindByRegexSecondOrder(@"/\*.*?/\*[^/]", extendedResult);