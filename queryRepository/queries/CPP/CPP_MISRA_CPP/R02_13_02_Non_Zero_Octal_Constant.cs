/*
MISRA CPP RULE 2-13-2
------------------------------
This query searches for non-zero octal constants (numeric literals or escape sequences)

	The Example below shows code with vulnerability:

n_octal_const = 017;                                  
n_octal_const = 00;                          
n_octal_const = 0017;                       
n_octal_escape = '\01';

*/

// Add numeric octal constants
result.Add(All.FindByType(typeof(IntegerLiteral)).FindByRegex(@"[^\w\.]0[0-7]+?[^\wuU]", false, false, false));

// Add octal escape sequences
result.Add((All.FindByType(typeof(StringLiteral)) + All.FindByType(typeof(CharLiteral))).FindByRegex(@"\\[1-7][0-7]{0,2}|\\0[0-7]{1,2}", false, true, false));