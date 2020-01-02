/*
MISRA C RULE 19-5
------------------------------
This query searches for usage of uses of #define or #undef in block

	The Example below shows code with vulnerability: 

int foo ( void )
{
	int i = 5;

	#define size_macro 6

	i = size_acro;
	#undef size_macro
	return i;
}

*/

// search for open brackets, followed by #define or #undef
result = All.FindByRegex(@"{[^}]*?#define [^{]*?}|{[^}]*?#undef [^{]*?}", false, false, false, All.NewCxList());