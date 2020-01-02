/*
MISRA CPP RULE 18-0-4
------------------------------
This query searches for usage of of the time handling facilities of <ctime>

	The Example below shows code with vulnerability: 

		#include <time.h>  //non-compliant
		#include <ctime>   //non-compliant

		time_t     myTime;  //non-compliant

*/

// Safety check for the violating h file
// (it is also a violation in itself)
CxList includes = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));
CxList hFile = includes.FindByShortName("time.h") + includes.FindByShortName("ctime");
if (hFile.Count > 0){
	// The functions defined by time.h
	CxList methodInvokes = All.FindByType(typeof(MethodInvokeExpr));

	//Time manipulation:
	result.Add(methodInvokes.FindByShortName("clock") +
		methodInvokes.FindByShortName("difftime") +
		methodInvokes.FindByShortName("mktime") +
		methodInvokes.FindByShortName("time") +
		//Conversion:
		methodInvokes.FindByShortName("asctime") +
		methodInvokes.FindByShortName("ctime") +
		methodInvokes.FindByShortName("gmtime") +
		methodInvokes.FindByShortName("localtime") +
		methodInvokes.FindByShortName("strftime"));

	// The macros defined by time.h
	result.Add(All.FindByShortName("CLOCKS_PER_SEC"));

	// Remove all locally defined instances
	CxList defs = All.FindDefinition(result);
	result -= result.FindAllReferences(defs);

	// the include
	result.Add(hFile);

	// The types


	result.Add(All.FindByType(typeof(TypeRef)).FindByShortName("clock_t") +
		All.FindByType(typeof(TypeRef)).FindByShortName("size_t") +
		All.FindByType(typeof(TypeRef)).FindByShortName("time_t") +
		All.FindByType(typeof(TypeRef)).FindByRegex("struct tm", false, false, false));
	// struct tm members
	CxList memberAccess = All.FindByType(typeof(MemberAccess));


	result.Add(memberAccess.FindByRegex("tm_sec;", All.NewCxList()) +
		memberAccess.FindByRegex("tm_min", false, false, false) +
		memberAccess.FindByRegex("tm_hour", false, false, false) +
		memberAccess.FindByRegex("tm_mday", false, false, false) +
		memberAccess.FindByRegex("tm_mon", false, false, false) +
		memberAccess.FindByRegex("tm_year", false, false, false) +
		memberAccess.FindByRegex("tm_wday", false, false, false) +
		memberAccess.FindByRegex("tm_yday", false, false, false) +
		memberAccess.FindByRegex("tm_yday", All.NewCxList()) +
		memberAccess.FindByRegex("tm", All.NewCxList()));
}