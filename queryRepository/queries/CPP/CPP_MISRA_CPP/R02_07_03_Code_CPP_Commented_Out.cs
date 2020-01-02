/*
MISRA C++ RULE 2-7-3
------------------------------
This query searches for sections of code that have been commented out using CPP style comments

	The Example below shows code with vulnerability: 

/* Function comment is compliant. * /
void fn ( int32_t i )
{  
     //  ++i       // We want to increment 
	 for ( int32_t j = 0 ; j != i; ++j )
	 {
	 }
}
*/


// Find all comments ending with } or ;
CxList extendedResult = All.NewCxList();

// All /* */ comments
CxList res = All.FindByRegex(@"//.*?\n", true, false, false, extendedResult);

// Search results for } or ; at end of comment
result = All.FindByRegexSecondOrder(@"[{}]|;\s*(\*/|\n|//)|(if|for|while)\s*\(", extendedResult);