/*
MISRA C RULE 19-12
------------------------------
This query searches for instances of multiple # or ## preprocessor operators in a single macro definition.

	The Example below shows code with vulnerability: 

#define mc2_1912C(x,y) (#x = #y)
#define mc2_1912D(x,y) (x##y##y = 0)
#define mc2_1912E(x,y) (#x##1 = (y))
#define mc2_1912F(x,y) (x##y#y = 0)

*/

result = All.FindByRegex(@"#define [^\n]*?#[^\n]+?#", false, false, false, All.NewCxList());