/*
MISRA C RULE 20-9
------------------------------
This query searches for usage of the input/output library <stdio.h>.

	The Example below shows code with vulnerability: 

#include <stdio.h>            

void mc2_2009 ( void )
{
(void) printf("The library stdio shall not be used\n"); 
}

*/

// Safety check for the violating h file
CxList includes = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));
CxList hFile = includes.FindByShortName("stdio.h");
if (hFile.Count > 0){
	
	// The functions defined by stdio.h
	CxList methodInvokes = All.FindByType(typeof(MethodInvokeExpr));
	
	
	result.Add(methodInvokes.FindByShortNames(new List<string>(){
			//Operations on files:
			"signal","remove","rename","tmpfile","tmpnam",
			//File access:
			"fclose","fflush","fopen","freopen","setbuf","setvbuf",
			//Formatted input/output:
			"fprintf","fscanf","printf","scanf","sprintf","sscanf","vfprintf","vprintf","vsprintf",
			//Character input/output:
			"fgetc","fgets","fputc","fputs","getc","gets","putc","putchar","puts","ungetc",
			//Direct input/output:
			"fread","fwrite",
			//File positioning:
			"fgetpos","fseek","fsetpos","ftell","rewind",
			//Error-handling:
			"clearerr","feof","ferror","perror"
			}));
	
	// The macros defined by stdio.h
	result.Add(All.FindByShortNames(new List <string>(){
			"_IOFBF","_IOLBF","_IONBF","BUFSIZ","EOF","FOPEN_MAX","FILENAME_MAX",
			"SEEK_CUR","SEEK_END","SEEK_SET","TMP_MAX","stderr","stdin","stdout"}));
	
	// Remove all locally defined instances
	CxList defs = All.FindDefinition(result);
	result -= result.FindAllReferences(defs);
	
	// the include
	result.Add(hFile);
	
	// The types
	result.Add(All.FindByType(typeof(TypeRef)).FindByShortName("size_t") +
		All.FindByType(typeof(TypeRef)).FindByShortName("FILE") +
		All.FindByType(typeof(TypeRef)).FindByShortName("fpos_t"));
}