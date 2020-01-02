/*
MISRA C RULE 19-6
------------------------------
This query searches for usage of #undef

	The Example below shows code with vulnerability: 


#define size_macro 6
...
int i = size_acro;
...
#undef size_macro

}

*/

result = All.FindByRegex(@"#undef\W", false, false, false, All.NewCxList());