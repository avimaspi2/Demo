CxList include = Find_Read();

CxList fileGetContents = Find_Methods().FindByShortName("file_get_contents");
CxList fileGetContentsFirstParameters = All.GetParameters(fileGetContents, 0);
CxList stringLiterals = All.GetByAncs(fileGetContentsFirstParameters).FindByType(typeof(StringLiteral));
CxList exclusionList = All.NewCxList();
foreach (CxList literal in stringLiterals) {
	string stringLiteral = literal.GetName();
	if (stringLiteral.StartsWith("http://") || stringLiteral.StartsWith("https://")) {
		exclusionList.Add(literal);
	}
}
exclusionList = exclusionList.GetAncOfType(typeof(Param));
exclusionList.Add(All.FindByFathers(exclusionList));
include -= exclusionList;

CxList inputs = Find_Interactive_Inputs();

// find the file input sanitizers (number and string functions)
CxList sanitized = Find_File_Sanitizers();

result = include.InfluencedByAndNotSanitized(inputs, sanitized);