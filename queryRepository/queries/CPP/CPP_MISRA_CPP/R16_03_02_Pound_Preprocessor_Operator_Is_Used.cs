/*
MISRA CPP RULE 16-3-2
------------------------------
This query searches for usage of the # or ## macro preprocessor operators

	The Example below shows code with vulnerability: 

#define mc2_1913A(x,y) (#x = (y))
#define mc2_1913B(x,y) (x##y = 0)

*/

result = All.FindByRegex(@"#define [^\n]*?#", false, false, false, All.NewCxList());