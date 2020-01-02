/*
MISRA C RULE 20-7
------------------------------
This query searches for usage of the macro 'setjmp' macro 'longjmp', in library <setjmp.h>

	The Example below shows code with vulnerability: 

void foo ( void ) 
{
jmp_buf jbuf;
int val = 1;
    
    if ( setjmp ( jbuf ) == 0 ) 
    {
                
    }
    else
    {
                
    }
    
    longjmp ( jbuf, val ) ;

*/

// Safety check for the violating h file
CxList includes = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));
CxList hFile = includes.FindByShortName("setjmp.h");
if (hFile.Count == 0){
	// Start with all instances of jumps
	CxList jmps = All.FindByType(typeof(MethodInvokeExpr)).FindByShortName("setjmp") +
		All.FindByType(typeof(MethodInvokeExpr)).FindByShortName("longjmp");

	// Remove all locally defined instances
	CxList defs = All.FindDefinition(jmps);
	jmps -= jmps.FindAllReferences(defs);

	result = jmps;
}