/*
MISRA C RULE 20-12
------------------------------
This query searches for usage of of the time handling facilities of <time.h>

	The Example below shows code with vulnerability: 

#include <time.h>

time_t     myTime;

*/


// Safety check for the violating h file
// (it is also a violation in itself)
CxList includes = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));
CxList hFile = includes.FindByShortName("time.h");
if (hFile.Count > 0){
	// The functions defined by time.h
	CxList methodInvokes = All.FindByType(typeof(MethodInvokeExpr));

	//Time manipulation:
	result.Add(methodInvokes.FindByShortNames(new List<string>(){
			"clock","difftime","mktime","time",
			//Conversion:
			"asctime","ctime","gmtime","localtime","strftime"}));

	// The macros defined by time.h
	result.Add(All.FindByShortName("null") +
		All.FindByShortName("CLOCKS_PER_SEC"));

	// Remove all locally defined instances
	CxList defs = All.FindDefinition(result);
	result -= result.FindAllReferences(defs);

	// the include
	result.Add(hFile);

	// The types
	result.Add(All.FindByType(typeof(TypeRef)).FindByShortName("clock_t") +
		All.FindByType(typeof(TypeRef)).FindByShortName("size_t") +
		All.FindByType(typeof(TypeRef)).FindByShortName("time_t") +
		All.FindByType(typeof(TypeRef)).FindByRegex("struct tm"));
	// struct tm members
	CxList memberAccess = All.FindByType(typeof(MemberAccess));
	result.Add(memberAccess.FindByRegex("tm_sec;") +
		memberAccess.FindByRegex("tm_min") +
		memberAccess.FindByRegex("tm_hour") +
		memberAccess.FindByRegex("tm_mday") +
		memberAccess.FindByRegex("tm_mon") +
		memberAccess.FindByRegex("tm_year") +
		memberAccess.FindByRegex("tm_wday") +
		memberAccess.FindByRegex("tm_yday") +
		memberAccess.FindByRegex("tm_yday") +
		memberAccess.FindByRegex("tm"));
}