/*
MISRA C RULE 19-1
------------------------------
This query searches for non - preprocessor or comments preceding '#include' statements in a file

	The Example below shows code with vulnerability: 

int i;

#include "misra.h"

void main(){
...
}

*/

CxList includes = All.FindByType(typeof(MethodDecl)).FindByShortName("INCLUDEREPLACE");

if (includes.Count > 0){
	// Removes includes and all associated dom objects except the class decl
	// (we'll remove that later);
	CxList nonPermitted = All - All.GetByAncs(includes);
	
	// initialize first file stats
	MethodDecl firstInc = includes.data.GetByIndex(0) as MethodDecl;
	string lastFileName = firstInc.LinePragma.FileName;
	int furthestIncludeLineInFile = firstInc.LinePragma.Line;
	int furthestIncludeColumnInFile = firstInc.LinePragma.Column;
	
	// go over all includes, finding out the furthest incldue in each file
	foreach (CxList inc in includes){
		MethodDecl curInc = inc.data.GetByIndex(0) as MethodDecl;
		string curFileName = curInc.LinePragma.FileName;
		int curLine = curInc.LinePragma.Line;
		int curColumn = curInc.LinePragma.Column;
		
		// if new file started, parse old file
		if (String.Compare(lastFileName, curFileName) != 0){
			CxList curFileNonPermitted = nonPermitted.FindByFileName(lastFileName);
			// for all potential violators, check if they are before the last include in the file
			foreach (CxList curNonPermitted in curFileNonPermitted){
				CSharpGraph nonPermittedCommand = curNonPermitted.data.GetByIndex(0) as CSharpGraph;
				int nonPermLine = nonPermittedCommand.LinePragma.Line;
				int nonPermColumn = curInc.LinePragma.Column;
				if (nonPermLine < furthestIncludeLineInFile){
					result.Add(curNonPermitted);
				}
			}
			// and initialize new file stats
			lastFileName = curInc.LinePragma.FileName;
			furthestIncludeLineInFile = curInc.LinePragma.Line;
			furthestIncludeColumnInFile = curInc.LinePragma.Column;
		}
			// else update current file stats
		else{
			if (curLine > furthestIncludeLineInFile){
				furthestIncludeLineInFile = curLine;
				if (curColumn > furthestIncludeColumnInFile){
					furthestIncludeColumnInFile = curColumn;
				}
			}
			
		}
	}
	
	
	// remove all class decls resulting from includes
	foreach (CxList inc in includes){
		MethodDecl curInc = inc.data.GetByIndex(0) as MethodDecl;
		string curFileName = curInc.LinePragma.FileName;
		int curLine = curInc.LinePragma.Line;
		result -= result.FindByPosition(curFileName, curLine);
	}
	
	// clean result duplicates in same line
	foreach (CxList res in result){
		CSharpGraph curRes = res.data.GetByIndex(0) as CSharpGraph;
		string curFileName = curRes.LinePragma.FileName;
		int curLine = curRes.LinePragma.Line;
		result -= result.FindByPosition(curFileName, curLine);
		result.Add(res);
	}
}