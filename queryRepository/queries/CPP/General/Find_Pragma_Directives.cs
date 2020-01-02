/**
Find_Pragma_Directives

This general query is responsible for searching for pragma directives.

Arguments:

 0. directive 		 [regex|string] (mandatory)

 1. directiveValue 	 [regex|string] (optional)

 2. fileMask 		 [regex|string]	(optional)

 3. compiler 		 [regex|string] (optional)

**/

CxList pragmas = All.NewCxList();

string directive = "optimize";
string directiveValue = ".*\\n";
string fileMask = "*.cpp";
string compiler = "(GCC)?";

string pragma = @"#pragma";
string whiteSpace = "(\\ +)?";

bool searchInComments = true; 

int paramSize = param.Length;

switch(paramSize)
{

	case 1:
		directive = param[0].ToString();
		break;
	
	case 2:
		directive = param[0].ToString();
		directiveValue = param[1].ToString();
		break;
	
	case 3:
		directive = param[0].ToString();
		directiveValue = param[1].ToString();
		fileMask = param[2].ToString();
		break;
	
	case 4:
		directive = param[0].ToString();
		directiveValue = param[1].ToString();
		fileMask = param[2].ToString();
		compiler = param[3].ToString();
		break;
}

string finalRegex = "";

//Regex to find all pragma directives
if(paramSize == 0){
	
	finalRegex = pragma + directiveValue;
}
	//Regex to find specific pragma directives
else {
	finalRegex = pragma + whiteSpace + compiler + whiteSpace + directive + directiveValue;
}

pragmas = All.FindByRegexExt(finalRegex, fileMask, searchInComments, RegexOptions.Multiline);

result = pragmas;