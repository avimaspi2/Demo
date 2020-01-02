/*
MISRA C RULE 3-4
------------------------------
This query searches for usage of #pragma not explained by a comment in same line, or start of line above it

	The Example below shows code with vulnerability: 

void foo () 
{
    #pragma OPTIMIZE ON
    int i = 1; // irrelevent comment
    
}

*/

CxList dummy;
CxList commentFinds = All.NewCxList();
CxList pragmaFinds = All.NewCxList();
dummy = All.FindByRegex(@"/\*.*?\*/|//",true,false,false,commentFinds);
dummy = All.FindByRegex(@"#pragma",false,false,false,pragmaFinds);

foreach (CxList cur in commentFinds){
	LinePragma curCommentLoc = ((CSharpGraph) cur.data.GetByIndex(0)).LinePragma;
	
	// remove #pragmas in same line as comment
	pragmaFinds -= pragmaFinds.FindByPosition(curCommentLoc.FileName, curCommentLoc.Line);
	
	// if the current comment is at the start of its line, remove #pragmas in the next line
	if (curCommentLoc.Column == 1){
		pragmaFinds -= pragmaFinds.FindByPosition(curCommentLoc.FileName, curCommentLoc.Line + 1);	
	}
}

result = All.FindByRegexSecondOrder(@".",pragmaFinds);