/*
 MISRA CPP RULE 5-2-11
 ------------------------------
 This query searches for overloads of the comma, && and || operators.
 
 The Example below shows code with vulnerability: 

		bool operator && (int, int); //Non-compliant
		bool operator || (int, int); //Non-compliant
		bool operator , (int, int);  //Non-compliant

*/

CxList methods = All.FindByType(typeof(MethodDecl));
result = methods.FindByShortName("operator__Comma") + 
	methods.FindByShortName("operator__|And") +
	methods.FindByShortName("operator__|Or");